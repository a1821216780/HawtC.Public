

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.PCS
//
// Licensed under the Boost Software License - Version 1.0 - August 17th, 2003
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.HawtC.cn/licenses.txt
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
// SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
// FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
//**********************************************************************************************************************************

using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.Interface1;
using OpenWECD.IO.IO;
using static OpenWECD.IO.math.LinearAlgebraHelper;
using OpenWECD.IO.Log;

namespace OpenWECD.PreComp
{
    /// <summary>
    /// 读取输入文件
    /// </summary>
    public class PciL_IO_Subs : IModulInterYml<PreCompL1>
    {

        /// <summary>
        /// 向YML文件中写入 PreCompL 结构体
        /// </summary>
        /// <param name="yml"></param>
        /// <param name="PreComp"></param>
        public void ConvertToYML(ref YML yml, PreCompL1 PreComp)
        {
            LogHelper.WriteLogO("开始转换 PreCompL 为 yml");
            YML.ConvertStructToYML(ref yml, "OpenWECD.OpenHAST.PreComp", 4, PreComp);
            LogHelper.WriteLogO("转换 PreCompL 成功！");

        }

        public static PreCompL1 ReadPreCompL_MainFile(string path)
        {
            CheckError.Filexists(path);
            PreCompL1 pre = new PreCompL1();
            pre.path = path;
            pre.data = File.ReadAllLines(path);

            int fd(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(pre.data, temp, path, error, show)[0];

            //# 读取基础的项
            pre.BladeLength = Otherhelper.ParseLine<double>(pre.data, path, fd(" Bl_length "));
            pre.SectionCount = Otherhelper.ParseLine<int>(pre.data, path, fd("  N_sections "));
            pre.MaterialTypeCount = Otherhelper.ParseLine<int>(pre.data, path, fd(" N_materials "));
            pre.OutputFormat = Otherhelper.ParseLine<int>(pre.data, path, fd(" Out_format "));
            pre.UseTabDelimiter = Otherhelper.ParseLine<bool>(pre.data, path, fd(" TabDelim "));
            //# 读取材料信息
            var matfilepath = Path.Combine(path.GetDirectoryName(), "materials.inp").GetABSPath();
            CheckError.Filexists(matfilepath);
            var data = File.ReadAllLines(matfilepath);
            pre.Material = new CompositeMaterial[pre.MaterialTypeCount];
            string[] name2 = { "Id", "E1", "E2", "G12", "Nu12", "Density", "Name" };
            for (int i = 0; i < pre.MaterialTypeCount; i++)
            {
                pre.Material[i] = Otherhelper.ParseLine<CompositeMaterial>(data, path, 3 + i, pre.Material[i],
                    namelist: name2);
            }
            //#为了防止序号不对，想要按照序号排序
            Array.Sort(pre.Material, (x, y) => x.Id.CompareTo(y.Id));

            pre.Nweb = Otherhelper.ParseLine<int>(pre.data, path, fd(" Nweb "));
            pre.Ib_sp_stn = Otherhelper.ParseLine<int>(pre.data, path, fd(" Ib_sp_stn "));
            pre.Ob_sp_stn = Otherhelper.ParseLine<int>(pre.data, path, fd(" Ob_sp_stn "));

            pre.Webs = new WebStructure[pre.Nweb];
            string[] name1 = { "WebId", "InnerChordPosition", "OuterChordPosition" };
            for (int i = 0; i < pre.Nweb; i++)
            {
                pre.Webs[i] = Otherhelper.ParseLine<WebStructure>(pre.data, path, fd("Web_num") + 1 + i, pre.Webs[i],
                    namelist: name1);

            }

            //# 读取叶片的截面数据
            pre.Sections = new OpenWECD.PreComp.BladeSection[pre.SectionCount];
            string[] name = { "SpanLocation", "AerodynamicCenter", "ChordLength", "TwistAngle", "AirfoilFile", "LayupFile" };
            for (int i = 0; i < pre.SectionCount; i++)
            {
                pre.Sections[i] = Otherhelper.ParseLine<BladeSection>(pre.data, path, fd("Span_loc") + 3 + i, pre.Sections[i],
                    namelist: name);
                //# 针对展长处理

                pre.Sections[i].SpanLocation = pre.Sections[i].SpanLocation * pre.BladeLength;

                //# 针对其中的翼型文件路径进行处理
                pre.Sections[i].AirfoilFile = Path.Combine(path.GetDirectoryName(),
                    pre.Sections[i].AirfoilFile.Split(' ', '\'', '"', '\t').RemoveNull()[0]).GetABSPath();
                CheckError.Filexists(pre.Sections[i].AirfoilFile, true, true,
                    $"ReadPreCompL_MainFile->叶片的翼型文件{pre.Sections[i].AirfoilFile}不存在!");
                pre.Sections[i].Airfoil = ReadPreCompL_Af_shape_file(pre.Sections[i].AirfoilFile);



                //# 针对其中的铺层文件路径进行处理
                pre.Sections[i].LayupFile = Path.Combine(path.GetDirectoryName(),
                    pre.Sections[i].LayupFile.Split(' ', '\'', '"', '\t').RemoveNull()[0]).GetABSPath();
                CheckError.Filexists(pre.Sections[i].LayupFile, true, true,
                    $"ReadPreCompL_MainFile->叶片的铺层文件{pre.Sections[i].LayupFile}不存在!");

                if (i >= pre.Ib_sp_stn - 1 &
                    i <= pre.Ob_sp_stn - 1)
                {
                    (pre.Sections[i].TopConfiguration, pre.Sections[i].LowConfiguration, pre.Sections[i].WebConfiguration) =
                        ReadPreCompL_SurfaceConfiguration_file(pre.Sections[i].LayupFile, pre.Nweb);
                }
                else
                {
                    (pre.Sections[i].TopConfiguration, pre.Sections[i].LowConfiguration, pre.Sections[i].WebConfiguration) =
                        ReadPreCompL_SurfaceConfiguration_file(pre.Sections[i].LayupFile, 0);
                }
                


            }




            return pre;
        }


        private static Af_shape_file ReadPreCompL_Af_shape_file(string path)
        {
            CheckError.Filexists(path);
            Af_shape_file af = new Af_shape_file();
            var data = File.ReadAllLines(path);
            int fd(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(data, temp, path, error, show)[0];
            af.N_af_nodes = Otherhelper.ParseLine<int>(data, path, 0);
            var mat = Otherhelper.ParseLine<Matrix<double>>(data, path, 4, num: af.N_af_nodes);
            var x = af.X = mat[':', 0];
            var y = af.Y = mat[':', 1];


            if (x[0] != 0 | y[0] != 0)
            {
                LogHelper.ErrorLog(" the first node, leading-edge node, must be (0,0)\r\n", FunctionName: "ReadPreCompL_Af_shape_file");
            }

            //#找到x坐标是1，y坐标是0的点的位置
            int index = 0;
            for (int i = 0; i < af.N_af_nodes; i++)
            {
                if (x[i] == 1 & y[i] == 0)
                {
                    index = i;
                    break;
                }
            }

            af.Xu = x[0, index + 1];
            af.Yu = y[0, index + 1];

            af.Xl = zeros(Hact(x[index, -1], 0).Reverse().ToArray());
            af.Yl = zeros(Hact(y[index, -1], 0).Reverse().ToArray());
            return af;
        }
        private static (SurfaceConfiguration top, SurfaceConfiguration low, SurfaceConfiguration web) ReadPreCompL_SurfaceConfiguration_file(string path, int Nweb)
        {
            CheckError.Filexists(path);

            var data = File.ReadAllLines(path);
            int[] fd(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(data, temp, path, error, show).ToArray();
            string[] name = { "LayerId", "PlyCount", "Thickness", "FiberOrientation", "MaterialId" };
            //# 翼型上下便面扇区分开处理。
            //# 上翼面扇区
            SurfaceConfiguration aftop = new SurfaceConfiguration();
            aftop.N_scts = Otherhelper.ParseLine<int>(data, path, fd("N_scts(1)")[0]);
            aftop.xsec_node = Otherhelper.ParseLine<double[]>(data, path, fd("xsec_node")[0] + 1, row: true);
            aftop.Sector = new SectorLaminae[aftop.N_scts];
            for (int i = 0; i < aftop.N_scts; i++)
            {
                aftop.Sector[i].Sect_num = Otherhelper.ParseLine<int[]>(data, path, fd("Sect_num")[i] + 1, row: true)[0];
                aftop.Sector[i].N_laminas = Otherhelper.ParseLine<int[]>(data, path, fd("Sect_num")[i] + 1, row: true)[1];
                aftop.Sector[i].Laminae = new Laminae[aftop.Sector[i].N_laminas];
                for (int j = 0; j < aftop.Sector[i].N_laminas; j++)
                {
                    aftop.Sector[i].Laminae[j] = Otherhelper.ParseLine<Laminae>(data, path, fd("Sect_num")[i] + 6 + j, namelist: name, moren: aftop.Sector[i].Laminae[j]);
                }
            }


            //# 下翼面扇区
            //int[] fd1(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(data, temp, path, error, show)[aftop.N_scts..^-1].ToArray();
            SurfaceConfiguration aflow = new SurfaceConfiguration();
            aflow.N_scts = Otherhelper.ParseLine<int>(data, path, fd("N_scts(2)")[0]);
            aflow.xsec_node = Otherhelper.ParseLine<double[]>(data, path, fd("xsec_node")[1] + 1, row: true);
            aflow.Sector = new SectorLaminae[aflow.N_scts];
            for (int i = 0; i < aflow.N_scts; i++)
            {
                aflow.Sector[i].Sect_num = Otherhelper.ParseLine<int[]>(data, path, fd("Sect_num")[i + aftop.N_scts] + 1, row: true)[0];
                aflow.Sector[i].N_laminas = Otherhelper.ParseLine<int[]>(data, path, fd("Sect_num")[i + aftop.N_scts] + 1, row: true)[1];
                aflow.Sector[i].Laminae = new Laminae[aflow.Sector[i].N_laminas];
                for (int j = 0; j < aflow.Sector[i].N_laminas; j++)
                {
                    aflow.Sector[i].Laminae[j] = Otherhelper.ParseLine<Laminae>(data, path, fd("Sect_num")[i + aftop.N_scts] + 6 + j, namelist: name, moren: aflow.Sector[i].Laminae[j]);
                }
            }



            //# 腹板
            int[] fd2(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(data, temp, path, error, show).ToArray();
            SurfaceConfiguration afweb = new SurfaceConfiguration();
            afweb.N_scts = Nweb;
            afweb.xsec_node = Array.Empty<double>();
            afweb.Sector = new SectorLaminae[afweb.N_scts];
            for (int i = 0; i < afweb.N_scts; i++)
            {
                afweb.Sector[i].Sect_num = Otherhelper.ParseLine<int[]>(data, path, fd2("web_num")[i] + 1, row: true)[0];
                afweb.Sector[i].N_laminas = Otherhelper.ParseLine<int[]>(data, path, fd2("web_num")[i] + 1, row: true)[1];
                afweb.Sector[i].Laminae = new Laminae[afweb.Sector[i].N_laminas];
                for (int j = 0; j < afweb.Sector[i].N_laminas; j++)
                {
                    afweb.Sector[i].Laminae[j] = Otherhelper.ParseLine<Laminae>(data, path, fd2("wlam_num")[i] + j + 1, namelist: name, moren: afweb.Sector[i].Laminae[j]);
                }
            }



            return (aftop, aflow, afweb);
        }

    }
}
