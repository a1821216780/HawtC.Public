

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
using OpenWECD.IO.IO;
using OpenWECD.IO.math;
using static OpenWECD.IO.IO.PhysicalParameters;
using static OpenWECD.IO.math.LinearAlgebraHelper;
using static System.Math;
using OpenWECD.IO.Log;

namespace OpenWECD.PreComp
{
    public class PciL_Subs
    {
        public double eps = 1E-15;
        private int naf;
        private Input[] input;
        private Output[] output;
        private Vector<double> tw_prime_d;
        private Vector<double> twist;
        private PreCompL1 pre;
        public PciL_Subs(PreCompL1 pre)
        {
            this.pre = pre;
            this.naf = pre.SectionCount;
            this.input = new Input[this.naf];
            this.output = new Output[this.naf];
            //使用Linq将结构体数组pre.Sections当中的每个扭角twist提取出来为一个新的数组
            twist = zeros(pre.Sections.Select(x => x.TwistAngle).ToArray());
            var sloc = zeros(pre.Sections.Select(x => x.SpanLocation).ToArray());
            tw_prime_d = tw_rate(sloc, twist);
            //检查文件
            CheckPreComl(pre);
            //调用函数计算


        }
        private void CheckPreComl(PreCompL1 pre)
        {
            //检查截面
            for (int i = 0; i < pre.SectionCount; i++)
            {

                var node = pre.Sections[i];
                if (node.Airfoil.X.Count != node.Airfoil.Y.Count)
                {
                    LogHelper.ErrorLog($"x and y node lengths do not match at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (pre.Material.Length != pre.MaterialTypeCount)
                {
                    LogHelper.ErrorLog($"lengths of specified material properties do not match at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (node.AerodynamicCenter < 0)
                {
                    LogHelper.ErrorLog($"leading edge aft of reference axis->AerodynamicCenter smaller than 0 at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                if (node.Airfoil.N_af_nodes <= 2)
                {
                    LogHelper.ErrorLog($"min 3 nodes reqd to define airfoil geom at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                if (MathHelper.FindIndex(node.Airfoil.X, node.Airfoil.X.Minimum(), false) != 0)//最小的横坐标点不是第一个，这是不允许的
                {
                    LogHelper.ErrorLog($"the first airfoil node not a leading node at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                if (Abs(node.Airfoil.X[0]) > eps | Abs(node.Airfoil.Y[0]) > eps)
                {
                    LogHelper.ErrorLog($"leading-edge node not located at (0,0) at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                if (MathHelper.FindIndex(node.Airfoil.X, node.Airfoil.X.Maximum(), false) <= 0)
                {
                    LogHelper.ErrorLog($"trailing-edge node not located at (1,0) at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                if (node.Airfoil.X.Maximum() > 1)
                {
                    LogHelper.ErrorLog($"trailing-edge node exceeds chord boundary at section {i + 1} ", FunctionName: "CheckPreComl");
                }

                //# 检查翼型
                for (int j = 1; j < node.Airfoil.Xu.Count; j++)
                {
                    if (node.Airfoil.Xu[j] - node.Airfoil.Xu[j - 1] <= eps)
                    {
                        LogHelper.ErrorLog($"upper surface not single-valued at section {i + 1} .s airfoil {j}", FunctionName: "CheckPreComl");
                    }
                }
                for (int j = 1; j < node.Airfoil.Xl.Count; j++)
                {
                    if (node.Airfoil.Xl[j] - node.Airfoil.Xl[j - 1] <= eps)
                    {
                        LogHelper.ErrorLog($"lower surface not single-valued at section {i + 1} .s airfoil {j}", FunctionName: "CheckPreComl");
                    }
                }
                //# 检查方向
                if (node.Airfoil.Yu[1] / node.Airfoil.Xu[1] <=
                    node.Airfoil.Yl[1] / node.Airfoil.Xl[1])
                {
                    LogHelper.ErrorLog($"airfoil node numbering not clockwise {i + 1}", FunctionName: "CheckPreComl");
                }

                for (int j = 1; j < node.Airfoil.Xl.Count - 1; j++)  // loop over lower-surface nodes
                {
                    double x = node.Airfoil.Xl[j];

                    for (int k = 0; k < node.Airfoil.Xu.Count - 1; k++)  // loop over upper-surface nodes
                    {
                        double xl = node.Airfoil.Xu[k];
                        double xr = node.Airfoil.Xu[k + 1];

                        if (x >= xl && x <= xr)
                        {
                            double yl = node.Airfoil.Yu[k];
                            double yr = node.Airfoil.Yu[k + 1];
                            double y = yl + (yr - yl) * (x - xl) / (xr - xl);

                            if (node.Airfoil.Yl[j] >= y)
                            {
                                LogHelper.ErrorLog($"airfoil shape self-crossing  at section {i + 1}", FunctionName: "CheckPreComl");
                            }
                        }
                    }  // end loop over upper-surface nodes
                }  // end loop over lower-surface nodes

                //# 检查扇区
                //## 检查上扇区
                if (pre.Sections[i].TopConfiguration.N_scts <= 0)
                {
                    LogHelper.ErrorLog($"upper-surf no of sectors not positive at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (pre.Sections[i].TopConfiguration.xsec_node[0] < 0)
                {
                    LogHelper.ErrorLog($"upper-surf sector node x-location not positive at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (pre.Sections[i].TopConfiguration.xsec_node[pre.Sections[i].TopConfiguration.N_scts] > pre.Sections[i].Airfoil.Xu.Last())
                {
                    LogHelper.ErrorLog($"upper-surf last sector node out of bounds at section {i + 1}", FunctionName: "CheckPreComl");
                }



                for (int j = 0; j < pre.Sections[i].TopConfiguration.N_scts; j++)
                {
                    if (pre.Sections[i].TopConfiguration.xsec_node[j + 1] <=
                        pre.Sections[i].TopConfiguration.xsec_node[j])
                    {
                        LogHelper.ErrorLog($"upper-surf sector nodal x-locations not in ascending order at section {i + 1}", FunctionName: "CheckPreComl");
                    }
                }

                //## 检查下扇区
                if (pre.Sections[i].LowConfiguration.N_scts <= 0)
                {
                    LogHelper.ErrorLog($"Lower-surf no of sectors not positive at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (pre.Sections[i].LowConfiguration.xsec_node[0] < 0)
                {
                    LogHelper.ErrorLog($"Lower-surf sector node x-location not positive at section {i + 1}", FunctionName: "CheckPreComl");
                }

                if (pre.Sections[i].LowConfiguration.xsec_node[pre.Sections[i].LowConfiguration.N_scts] > pre.Sections[i].Airfoil.Xl.Last())
                {
                    LogHelper.ErrorLog($"Lower-surf last sector node out of bounds at section {i + 1}", FunctionName: "CheckPreComl");
                }

                for (int j = 0; j < pre.Sections[i].LowConfiguration.N_scts; j++)
                {
                    if (pre.Sections[i].LowConfiguration.xsec_node[j + 1] <=
                        pre.Sections[i].LowConfiguration.xsec_node[j])
                    {
                        LogHelper.ErrorLog($"Lower-surf sector nodal x-locations not in ascending order at section {i + 1}", FunctionName: "CheckPreComl");
                    }
                }



            }
            //#检查材料系数
            for (int i = 0; i < pre.MaterialTypeCount; i++)
            {
                var m = pre.Material[i];
                if (m.Nu12 > Sqrt(m.E1 / m.E2))
                {
                    LogHelper.ErrorLog($"material coefficient Nu12 larger than sqrt(E1/E2) at material {i + 1} ", FunctionName: "CheckPreComl");
                }
            }


        }
        /// <summary>
        /// 使用有限元方法计算截面特性参数
        /// </summary>
        public void runFEM()
        {

        }
        /// <summary>
        /// 使用经典的层压板理论计算截面特性参数
        /// </summary>
        public void run()
        {
            var anu12 = zeros(pre.Material.Select(x => x.Nu12).ToArray());
            var e1 = zeros(pre.Material.Select(x => x.E1).ToArray());
            var e2 = zeros(pre.Material.Select(x => x.E2).ToArray());
            var g12 = zeros(pre.Material.Select(x => x.G12).ToArray());


            var anud = 1.0 - anu12.PointwiseMultiply(anu12).PointwiseMultiply(e2).PointwiseDivide(e1);
            var q11 = e1.PointwiseDivide(anud);
            var q22 = e2.PointwiseDivide(anud);
            var q12 = anu12.PointwiseMultiply(e2).PointwiseDivide(anud);
            var q66 = g12;
            var density = zeros(pre.Material.Select(x => x.Density).ToArray());
            double[] weby_u = new double[pre.Nweb];
            double[] weby_l = new double[pre.Nweb];


            for (int i = 0; i < pre.SectionCount; i++)
            {
                LogHelper.WriteLog($"{Otherhelper.GetCurrentProjectName()} BEGIN ANALYSIS BLADE STATION {i}");
                double tphip = tw_prime_d[i] * D2R;
                double[] locw = new double[pre.Nweb];
                int max_sectors = new int[] { pre.Sections[i].TopConfiguration.N_scts + 1, pre.Sections[i].LowConfiguration.N_scts + 1, pre.Nweb }.Max();//最大扇区数量，腹板相当于一个扇区
                //# 初始化内存
                //var xsec_node = zeros(2, max_sectors);
                //xsec_node[0,':']=zeros(pre.Sections[i].TopConfiguration.xsec_node) ;
                //xsec_node[1,':']=zeros(pre.Sections[i].LowConfiguration.xsec_node);
                //var n_laminas = new int[2, max_sectors];
                //# 处理腹板对于的翼型




                if (i >= pre.Ib_sp_stn - 1 &
                    i <= pre.Ob_sp_stn - 1)
                {



                    for (int j = 0; j < pre.Nweb; j++)
                    {
                        //# 计算locw
                        var rle = pre.Sections[i].AerodynamicCenter;
                        var r1w = pre.Sections[pre.Ib_sp_stn - 1].AerodynamicCenter;
                        var r2w = pre.Sections[pre.Ob_sp_stn - 1].AerodynamicCenter;
                        var p1w = pre.Webs[j].InnerChordPosition;// ib_ch_loc[j];
                        var p2w = pre.Webs[j].OuterChordPosition; //ob_ch_loc[j];
                        var ch1 = pre.Sections[pre.Ib_sp_stn - 1].ChordLength;// chord[ib_sp_stn];
                        var ch2 = pre.Sections[pre.Ob_sp_stn - 1].ChordLength; //chord[ob_sp_stn];
                        var x1w = pre.Sections[pre.Ib_sp_stn - 1].SpanLocation;// sloc[ib_sp_stn];
                        var l_web = pre.Sections[pre.Ob_sp_stn - 1].SpanLocation - x1w;// sloc[ob_sp_stn] - x1w;
                        var xlocn = (pre.Sections[i].SpanLocation - x1w) / l_web;
                        var ch = pre.Sections[i].ChordLength;
                        locw[j] = rle - (r1w - p1w) * ch1 * (1 - xlocn) / ch - (r2w - p2w) * ch2 * xlocn / ch;



                        //# 处理上翼面
                        int newnode = MathHelper.FindIndex(pre.Sections[i].Airfoil.Xu, pre.Webs[j].InnerChordPosition) + 1;
                        double ynd = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xu, pre.Sections[i].Airfoil.Yu, pre.Webs[j].InnerChordPosition);
                        // 向其中的数据插入到数组中
                        if (!pre.Sections[i].Airfoil.Xu.Contains(pre.Webs[j].InnerChordPosition))
                        {
                            pre.Sections[i].Airfoil.Xu = Hact(pre.Sections[i].Airfoil.Xu[0, newnode].Append(pre.Webs[j].InnerChordPosition), pre.Sections[i].Airfoil.Xu[newnode, -1]);
                            pre.Sections[i].Airfoil.Yu = Hact(pre.Sections[i].Airfoil.Yu[0, newnode].Append(ynd), pre.Sections[i].Airfoil.Yu[newnode, -1]);
                            weby_u[j] = ynd;
                        }


                        //# 处理下翼面
                        newnode = MathHelper.FindIndex(pre.Sections[i].Airfoil.Xl, pre.Webs[j].OuterChordPosition) + 1;
                        ynd = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xl, pre.Sections[i].Airfoil.Yl, pre.Webs[j].OuterChordPosition);
                        // 向其中的数据插入到数组中
                        if (!pre.Sections[i].Airfoil.Xl.Contains(pre.Webs[j].OuterChordPosition))
                        {
                            pre.Sections[i].Airfoil.Xl = Hact(pre.Sections[i].Airfoil.Xl[0, newnode].Append(pre.Webs[j].OuterChordPosition), pre.Sections[i].Airfoil.Xl[newnode, -1]);
                            pre.Sections[i].Airfoil.Yl = Hact(pre.Sections[i].Airfoil.Yl[0, newnode].Append(ynd), pre.Sections[i].Airfoil.Yl[newnode, -1]);
                            weby_l[j] = ynd;
                        }

                    }



                    if (locw[0] < pre.Sections[i].TopConfiguration.xsec_node[0] ||
                        locw[0] < pre.Sections[i].LowConfiguration.xsec_node[0])
                    {
                        LogHelper.ErrorLog($"first web out of sectors-bounded airfoil at section {i + 1}", FunctionName: "CheckPreComl");
                    }
                    if (locw[pre.Nweb - 1] > pre.Sections[i].TopConfiguration.xsec_node.Last() ||
                        locw[pre.Nweb - 1] > pre.Sections[i].LowConfiguration.xsec_node.Last())
                    {
                        LogHelper.ErrorLog($"last web out of sectors-bounded airfoil at section {i + 1}", FunctionName: "CheckPreComl");
                    }
                }
                //# 处理扇区
                //## 根据上扇区的扇区边界，找到翼型的边界
                var ynu = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xu.ToArray(), pre.Sections[i].Airfoil.Yu.ToArray(), pre.Sections[i].TopConfiguration.xsec_node);
                //## 根据下扇区的扇区边界，找到翼型的边界
                var ynl = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xl.ToArray(), pre.Sections[i].Airfoil.Yl.ToArray(), pre.Sections[i].LowConfiguration.xsec_node);

                if (Abs(pre.Sections[i].TopConfiguration.xsec_node[0] - pre.Sections[i].LowConfiguration.xsec_node[0]) > eps)
                {
                    LogHelper.WarnLog($"the leading edge may be open; check closure at section {i + 1}", FunctionName: "CheckPreComl");
                }
                else
                {
                    if (ynu[0] - ynl[0] > eps)
                    {
                        int wreq = 1;
                        if (i >= pre.Ib_sp_stn - 1 &
                            i <= pre.Ob_sp_stn - 1) //如果当前截面有腹板
                        {
                            if (Abs(pre.Sections[i].TopConfiguration.xsec_node[0] - locw[0]) < eps)
                            {
                                wreq = 0;
                            }
                        }

                        if (wreq == 1)
                        {
                            LogHelper.WarnLog("open leading edge; check web requirement at section {i + 1}", FunctionName: "CheckPreComl");
                        }
                    }
                }


                //# 将扇区的节点插值到数组当中


                //## 处理上翼面的扇区
                for (int j = 0; j < pre.Sections[i].TopConfiguration.N_scts; j++)
                {
                    int newnodeiIndex = MathHelper.FindIndex(pre.Sections[i].Airfoil.Xu, pre.Sections[i].TopConfiguration.xsec_node[j]) + 1;
                    double newy = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xu, pre.Sections[i].Airfoil.Yu, pre.Sections[i].TopConfiguration.xsec_node[j]);

                    if (!pre.Sections[i].Airfoil.Xu.Contains(pre.Sections[i].TopConfiguration.xsec_node[j]))
                    {
                        pre.Sections[i].Airfoil.Xu = Hact(pre.Sections[i].Airfoil.Xu[0, newnodeiIndex].Append(pre.Sections[i].TopConfiguration.xsec_node[j]), pre.Sections[i].Airfoil.Xu[newnodeiIndex, -1]);
                        pre.Sections[i].Airfoil.Yu = Hact(pre.Sections[i].Airfoil.Yu[0, newnodeiIndex].Append(newy), pre.Sections[i].Airfoil.Yu[newnodeiIndex, -1]);
                    }
                }
                //## 处理下翼面的扇区
                for (int j = 0; j < pre.Sections[i].LowConfiguration.N_scts; j++)
                {
                    int newnodeiIndex = MathHelper.FindIndex(pre.Sections[i].Airfoil.Xl, pre.Sections[i].LowConfiguration.xsec_node[j]) + 1;
                    double newy = InterpolateHelper.Inter1D(pre.Sections[i].Airfoil.Xl, pre.Sections[i].Airfoil.Yl, pre.Sections[i].LowConfiguration.xsec_node[j]);

                    if (!pre.Sections[i].Airfoil.Xl.Contains(pre.Sections[i].LowConfiguration.xsec_node[j]))
                    {
                        pre.Sections[i].Airfoil.Xl = Hact(pre.Sections[i].Airfoil.Xl[0, newnodeiIndex].Append(pre.Sections[i].LowConfiguration.xsec_node[j]), pre.Sections[i].Airfoil.Xl[newnodeiIndex, -1]);
                        pre.Sections[i].Airfoil.Yl = Hact(pre.Sections[i].Airfoil.Yl[0, newnodeiIndex].Append(newy), pre.Sections[i].Airfoil.Yl[newnodeiIndex, -1]);
                    }
                }



                if (Abs(pre.Sections[i].TopConfiguration.xsec_node.Last() - pre.Sections[i].LowConfiguration.xsec_node.Last()) > eps)
                {
                    LogHelper.WarnLog($"the trailing edge may be open; check closure at section {i + 1}", FunctionName: "CheckPreComl");
                }
                else
                {
                    if (ynu.Last() - ynl.Last() > eps)
                    {
                        int wreq = 1;
                        if (i >= pre.Ib_sp_stn - 1 &
                            i <= pre.Ob_sp_stn - 1) //如果当前截面有腹板
                        {
                            if (Abs(pre.Sections[i].TopConfiguration.xsec_node.Last() - locw[pre.Nweb - 1]) < eps)
                            {
                                wreq = 0;
                            }
                        }

                        if (wreq == 1)
                        {
                            LogHelper.WarnLog("open trailing edge; check web requirement at section {i + 1}", FunctionName: "CheckPreComl");
                        }
                    }
                }

                //# 开始基于板层理论计算

                int nseg_u = pre.Sections[i].Airfoil.Xu.Count - 1;
                int nseg_l = pre.Sections[i].Airfoil.Xl.Count - 1;
                int nseg_p = nseg_u + nseg_l;
                int nseg = nseg_p;
                if (pre.Sections[i].WebConfiguration.N_scts != 0) //当前截面有腹板的定义
                {
                    nseg = nseg_p + pre.Sections[i].WebConfiguration.N_scts;
                }

                (int[] isur, int[] idsect, double[] yseg, double[] zseg, double[] wseg,
                    double[] sthseg, double[] cthseg, double[] s2thseg, double[] c2thseg) = SegInfo(
                    pre.Sections[i].ChordLength, pre.Sections[i].AerodynamicCenter, nseg, nseg_u, nseg_p,
                    pre.Sections[i].Airfoil.Xu, pre.Sections[i].Airfoil.Yu, pre.Sections[i].Airfoil.Xl,
                    pre.Sections[i].Airfoil.Yl,
                    MathHelper.FindIndex(pre.Sections[i].Airfoil.Xl, pre.Sections[i].LowConfiguration.xsec_node[0]) + 1,
                    MathHelper.FindIndex(pre.Sections[i].Airfoil.Xu, pre.Sections[i].TopConfiguration.xsec_node[0]) + 1,
                    locw, weby_u, weby_l,
                    new[] { pre.Sections[i].TopConfiguration.N_scts, pre.Sections[i].LowConfiguration.N_scts }, i
                );


                double eabar = 0.0;
                double q11ya = 0.0;
                double q11za = 0.0;
                double sigma = 0.0;

                //# 外层循环：处理所有周边分段,只处理上下翼面
                for (int iseg = 0; iseg < nseg_p; iseg++)
                {
                    // 获取当前分段属性 (转换为0-based索引)
                    int surfaceType = isur[iseg];
                    int sectionId = idsect[iseg] - 1; // 转换为0-based截面ID
                    double ysg = yseg[iseg];
                    double zsg = zseg[iseg];
                    double w = wseg[iseg];
                    double sths = sthseg[iseg];
                    double cths = cthseg[iseg];
                    int nlam = 0;
                    // 获取当前分段的层数
                    if (surfaceType == 1) //上表面
                    {
                        nlam = pre.Sections[i].TopConfiguration.Sector[sectionId].N_laminas;
                    }
                    else if (surfaceType == 2)//下表面
                    {
                        nlam = pre.Sections[i].LowConfiguration.Sector[sectionId].N_laminas;
                    }
                    //int nlam = n_laminas[surfaceType - 1, sectionId];

                    // 分段层压板参数初始化
                    double tbar = 0.0;
                    double q11t = 0.0;
                    double q11yt_u = 0.0;
                    double q11zt_u = 0.0;
                    double q11yt_l = 0.0;
                    double q11zt_l = 0.0;

                    // 内层循环：处理所有铺层
                    for (int ilam = 0; ilam < nlam; ilam++)
                    {
                        // 获取层参数 (转换为0-based索引)
                        double t = 0;//tlam[surfaceType - 1, sectionId, ilam];
                        double thp = 0;// tht_lam[surfaceType - 1, sectionId, ilam];
                        int mat = 0;//mat_id[surfaceType - 1, sectionId, ilam] - 1; // 转换为0-based材料ID

                        if (surfaceType == 1) //上表面
                        {
                            //#计算铺层厚度
                            t = pre.Sections[i].TopConfiguration.Sector[sectionId].Laminae[ilam].Thickness * pre.Sections[i].TopConfiguration.Sector[sectionId].Laminae[ilam].PlyCount;
                            //#计算铺层角度
                            thp = pre.Sections[i].TopConfiguration.Sector[sectionId].Laminae[ilam].FiberOrientation * D2R;
                            //#计算材料ID
                            mat = pre.Sections[i].TopConfiguration.Sector[sectionId].Laminae[ilam].MaterialId - 1;
                        }
                        else if (surfaceType == 2)//下表面
                        {
                            //#计算铺层厚度
                            t = pre.Sections[i].LowConfiguration.Sector[sectionId].Laminae[ilam].Thickness * pre.Sections[i].LowConfiguration.Sector[sectionId].Laminae[ilam].PlyCount;
                            //#计算铺层角度
                            thp = pre.Sections[i].LowConfiguration.Sector[sectionId].Laminae[ilam].FiberOrientation * D2R;
                            //#计算材料ID
                            mat = pre.Sections[i].LowConfiguration.Sector[sectionId].Laminae[ilam].MaterialId - 1;
                        }


                        // 计算累积厚度
                        tbar += t / 2.0;

                        // 计算符号系数 (-1)^surfaceType
                        double sign = (surfaceType % 2 == 1) ? -1.0 : 1.0;

                        // 计算当前层坐标
                        double y0 = ysg - (sign * tbar * sths);
                        double z0 = zsg + (sign * tbar * cths);

                        // 计算刚度矩阵
                        var qbars = Q_Bars(
                            mat, thp, density, q11, q22, q12, q66);

                        Matrix<double> qtil = QTildas(
                            qbars.Item1, qbars.Item2, qbars.Item3,
                            qbars.Item4, qbars.Item5, qbars.Item6,
                            mat + 1); // 转换回1-based材料ID用于错误提示

                        // 计算层贡献
                        double qtil11t = qtil[0, 0] * t;
                        q11t += qtil11t;

                        // 上下表面分别累加
                        if (iseg < nseg_u)
                        {
                            q11yt_u += qtil11t * y0;
                            q11zt_u += qtil11t * z0;
                        }
                        else
                        {
                            q11yt_l += qtil11t * y0;
                            q11zt_l += qtil11t * z0;
                        }

                        tbar += t / 2.0;
                    }

                    // 计算分段总贡献
                    double signTotal = (surfaceType % 2 == 1) ? -1.0 : 1.0;
                    sigma += w * Math.Abs(zsg + signTotal * 0.5 * tbar * cths) * cths;
                    eabar += q11t * w;
                    q11ya += (q11yt_u + q11yt_l) * w;
                    q11za += (q11zt_u + q11zt_l) * w;
                }

                double y_sc = q11ya / eabar;
                double z_sc = q11za / eabar;



                eabar = 0.0;
                q11ya = 0.0;
                q11za = 0.0;
                double ap = 0.0;
                double bp = 0.0;
                double cp = 0.0;
                double dp = 0.0;
                double ep = 0.0;
                double q11ysqa = 0.0;
                double q11zsqa = 0.0;
                double q11yza = 0.0;

                double mass = 0.0;
                double rhoya = 0.0;
                double rhoza = 0.0;
                double rhoysqa = 0.0;
                double rhozsqa = 0.0;
                double rhoyza = 0.0;
                //# 外层循环：处理所有周边分段,处理上下翼面和有腹板的地方
                for (int iseg = 0; iseg < nseg; iseg++)
                {
                    int surfaceType = isur[iseg];
                    int idsec = idsect[iseg];
                    double ysg = yseg[iseg];
                    double zsg = zseg[iseg];
                    double w = wseg[iseg];
                    double sths = sthseg[iseg];
                    double cths = cthseg[iseg];
                    double s2ths = s2thseg[iseg];
                    double c2ths = c2thseg[iseg];
                    int nlam = 0;//铺层数量
                    if (surfaceType == 1) //上表面
                    {
                        nlam = pre.Sections[i].TopConfiguration.Sector[idsec - 1].N_laminas;
                    }
                    else if (surfaceType == 2)//下表面
                    {
                        nlam = pre.Sections[i].LowConfiguration.Sector[idsec - 1].N_laminas;
                    }
                    else if (surfaceType == 0)//腹板
                    {
                        nlam = pre.Sections[i].WebConfiguration.Sector[idsec - 1].N_laminas;
                    }
                    else
                    {
                        LogHelper.ErrorLog("未知结构");
                    }
                    //# 初始化局部参数
                    double tbar = 0.0;
                    double q11t = 0.0;
                    double q11yt = 0.0;
                    double q11zt = 0.0;
                    double dtbar = 0.0;
                    double q2bar = 0.0;
                    double zbart = 0.0;
                    double ybart = 0.0;
                    double tbart = 0.0;
                    double q11ysqt = 0.0;
                    double q11zsqt = 0.0;
                    double q11yzt = 0.0;

                    double rhot = 0.0;
                    double rhoyt = 0.0;
                    double rhozt = 0.0;
                    double rhoysqt = 0.0;
                    double rhozsqt = 0.0;
                    double rhoyzt = 0.0;

                    //# 处理每一个铺层
                    for (int ilam = 0; ilam < nlam; ilam++)
                    {
                        // 获取层参数 (转换为0-based索引)
                        double t = 0;//tlam[surfaceType - 1, sectionId, ilam];
                        double thp = 0;// tht_lam[surfaceType - 1, sectionId, ilam];
                        int mat = 0;//mat_id[surfaceType - 1, sectionId, ilam] - 1; // 转换为0-based材料ID
                        double y0 = 0;
                        double z0 = 0;
                        if (surfaceType == 1) //上表面
                        {
                            //#计算铺层厚度
                            t = pre.Sections[i].TopConfiguration.Sector[idsec - 1].Laminae[ilam].Thickness * pre.Sections[i].TopConfiguration.Sector[idsec - 1].Laminae[ilam].PlyCount;
                            //#计算铺层角度
                            thp = pre.Sections[i].TopConfiguration.Sector[idsec - 1].Laminae[ilam].FiberOrientation * D2R;
                            //#计算材料ID
                            mat = pre.Sections[i].TopConfiguration.Sector[idsec - 1].Laminae[ilam].MaterialId - 1;
                            tbar = tbar + t / 2.0;
                            y0 = ysg - Math.Pow(-1.0, surfaceType) * tbar * sths - y_sc;
                            z0 = zsg + Math.Pow(-1.0, surfaceType) * tbar * cths - z_sc;

                        }
                        else if (surfaceType == 2)//下表面
                        {
                            //#计算铺层厚度
                            t = pre.Sections[i].LowConfiguration.Sector[idsec - 1].Laminae[ilam].Thickness * pre.Sections[i].LowConfiguration.Sector[idsec - 1].Laminae[ilam].PlyCount;
                            //#计算铺层角度
                            thp = pre.Sections[i].LowConfiguration.Sector[idsec - 1].Laminae[ilam].FiberOrientation * D2R;
                            //#计算材料ID
                            mat = pre.Sections[i].LowConfiguration.Sector[idsec - 1].Laminae[ilam].MaterialId - 1;
                            tbar = tbar + t / 2.0;
                            y0 = ysg - Math.Pow(-1.0, surfaceType) * tbar * sths - y_sc;
                            z0 = zsg + Math.Pow(-1.0, surfaceType) * tbar * cths - z_sc;
                        }
                        else if (surfaceType == 0) //腹板
                        {

                            //#计算铺层厚度
                            t = pre.Sections[i].WebConfiguration.Sector[idsec - 1].Laminae[ilam].Thickness * pre.Sections[i].WebConfiguration.Sector[idsec - 1].Laminae[ilam].PlyCount;
                            //#计算铺层角度
                            thp = pre.Sections[i].WebConfiguration.Sector[idsec - 1].Laminae[ilam].FiberOrientation * D2R;
                            //#计算材料ID
                            mat = pre.Sections[i].WebConfiguration.Sector[idsec - 1].Laminae[ilam].MaterialId - 1;
                            tbar = tbar + t / 2.0;
                            y0 = ysg - tbar / 2.0 - y_sc;
                            z0 = zsg - z_sc;
                        }

                        double y0sq = y0 * y0;
                        double z0sq = z0 * z0;

                        // 计算刚度矩阵
                        (var qbar11, var qbar22, var qbar12, var qbar16, var qbar26, var qbar66, var rho_m) = Q_Bars(
                            mat, thp, density, q11, q22, q12, q66);

                        Matrix<double> qtil = QTildas(qbar11, qbar22, qbar12, qbar16, qbar26, qbar66, mat + 1); // 转换回1-based材料ID用于错误提示

                        double ieta1 = (t * t) / 12.0;
                        double izeta1 = (w * w) / 12.0;
                        double iepz = 0.5 * (ieta1 + izeta1);
                        double iemz = 0.5 * (ieta1 - izeta1);
                        double ipp = iepz + (iemz * c2ths);
                        double iqq = iepz - (iemz * c2ths);
                        double ipq = iemz * s2ths;

                        double qtil11t = qtil[0, 0] * t;
                        double rot = rho_m * t;
                        if (surfaceType == 1 | surfaceType == 2) //上表面
                        {

                            double qtil12t = qtil[0, 1] * t;
                            double qtil22t = qtil[1, 1] * t;

                            q11t = q11t + qtil11t;
                            q11yt = q11yt + qtil11t * y0;
                            q11zt = q11zt + qtil11t * z0;

                            dtbar = dtbar + qtil12t * (y0sq + z0sq) * tphip * t;
                            q2bar = q2bar + qtil22t;
                            zbart = zbart + z0 * qtil12t;
                            ybart = ybart + y0 * qtil12t;
                            tbart = tbart + qtil12t;

                            q11ysqt = q11ysqt + qtil11t * (y0sq + iqq);
                            q11zsqt = q11zsqt + qtil11t * (z0sq + ipp);
                            q11yzt = q11yzt + qtil11t * (y0 * z0 + ipq);

                            rhot = rhot + rot;
                            rhoyt = rhoyt + rot * y0;
                            rhozt = rhozt + rot * z0;
                            rhoysqt = rhoysqt + rot * (y0sq + iqq);
                            rhozsqt = rhozsqt + rot * (z0sq + ipp);
                            rhoyzt = rhoyzt + rot * (y0 * z0 + ipq);
                        }
                        else if (surfaceType == 0) //腹板
                        {
                            q11t = q11t + qtil11t;
                            q11yt = q11yt + qtil11t * y0;
                            q11zt = q11zt + qtil11t * z0;
                            q11ysqt = q11ysqt + qtil11t * (y0sq + iqq);
                            q11zsqt = q11zsqt + qtil11t * (z0sq + ipp);
                            q11yzt = q11yzt + qtil11t * (y0 * z0 + ipq);

                            rhot = rhot + rot;
                            rhoyt = rhoyt + rot * y0;
                            rhozt = rhozt + rot * z0;
                            rhoysqt = rhoysqt + rot * (y0sq + iqq);
                            rhozsqt = rhozsqt + rot * (z0sq + ipp);
                            rhoyzt = rhoyzt + rot * (y0 * z0 + ipq);


                        }

                        tbar = tbar + t / 2.0;
                    }//结束铺层循环
                    //添加seg贡献，以获得sc aban处ref平行轴的sec支撑
                    eabar = eabar + q11t * w;
                    q11ya = q11ya + q11yt * w;
                    q11za = q11za + q11zt * w;
                    q11ysqa = q11ysqa + q11ysqt * w;
                    q11zsqa = q11zsqa + q11zsqt * w;
                    q11yza = q11yza + q11yzt * w;
                    if (surfaceType == 1 | surfaceType == 2) //上下表面
                    {
                        double wdq2bar = w / q2bar;
                        ap = ap + wdq2bar;
                        bp = bp + wdq2bar * tbart;
                        cp = cp + wdq2bar * dtbar;
                        dp = dp + wdq2bar * zbart;
                        ep = ep + wdq2bar * ybart;
                    }

                    mass = mass + rhot * w;
                    rhoya = rhoya + rhoyt * w;
                    rhoza = rhoza + rhozt * w;
                    rhoysqa = rhoysqa + rhoysqt * w;
                    rhozsqa = rhozsqa + rhozsqt * w;
                    rhoyza = rhoyza + rhoyzt * w;
                }//结束扇区循环

                double y_tc = q11ya / eabar;
                double z_tc = q11za / eabar;

                double sfbar = q11za;
                double slbar = q11ya;
                double eifbar = q11zsqa;
                double eilbar = q11ysqa;
                double eiflbar = q11yza;

                double sigm2 = sigma * 2.0;
                double gjbar = sigm2 * (sigm2 + cp) / ap;
                double sftbar = -sigm2 * dp / ap;
                double sltbar = -sigm2 * ep / ap;
                double satbar = sigm2 * bp / ap;

                double ycm_sc = rhoya / mass;
                double zcm_sc = rhoza / mass;

                double iflap_sc = rhozsqa;
                double ilag_sc = rhoysqa;
                double ifl_sc = rhoyza;

                //获取截面tc和cm  get section tc and cm
                double ytc_ref = y_tc + y_sc;//wrt the ref axes
                double ztc_ref = z_tc + z_sc;//#wrt the ref axes

                double ycm_ref = ycm_sc + y_sc;//#wrt the ref axes
                double zcm_ref = zcm_sc + z_sc;//#wrt the ref axes

                // inertia principal axes orientation and moments of inertia
                /* 理论背景说明：
   1. 该算法用于确定截面的主惯性轴
   2. 当惯性积不为零时，需要通过旋转坐标系消除惯性积
   3. 角度计算公式推导自转轴公式：tan(2θ) = 2Ixy/(Ix-Iy)
   4. 主惯性矩计算公式为：I₁,₂ = (Ix+Iy)/2 ± √[((Ix-Iy)/2)² + Ixy²]
*/
                double iflap_cm = iflap_sc - mass * zcm_sc * zcm_sc;
                double ilag_cm = ilag_sc - mass * ycm_sc * ycm_sc;
                double ifl_cm = ifl_sc - mass * ycm_sc * zcm_sc;
                double m_inertia = 0.5 * (ilag_cm + iflap_cm);
                double r_inertia = Math.Sqrt(0.25 * Math.Pow(ilag_cm - iflap_cm, 2) + ifl_cm * ifl_cm);
                double iflap_eta = 0;
                double ilag_zeta = 0;
                // 第一部分：确定主惯性轴方向
                if (iflap_cm <= ilag_cm)
                {
                    // 当挥舞惯性矩小于等于摆振惯性矩时
                    iflap_eta = m_inertia - r_inertia;  // 计算η方向主惯性矩
                    ilag_zeta = m_inertia + r_inertia;  // 计算ζ方向主惯性矩
                }
                else
                {
                    // 当挥舞惯性矩大于摆振惯性矩时
                    iflap_eta = m_inertia + r_inertia; // 交换计算方式
                    ilag_zeta = m_inertia - r_inertia;

                }
                // 第二部分：计算主轴旋转角度
                double th_pa = 0;
                const double epsilon = 1e-6; // 浮点比较阈值
                if (Math.Abs(ilag_cm - iflap_cm) < eps) // 严格相等判断
                {
                    th_pa = Math.PI / 4.0; // 默认45度

                    // 当惯性积接近零时修正角度
                    if (Math.Abs(ifl_cm / iflap_cm) < epsilon)
                    {
                        th_pa = 0.0; // 无旋转角度
                    }
                }
                else
                {
                    // 计算主轴旋转角度（基于惯性积公式）
                    th_pa = 0.5 * Math.Abs(Math.Atan(2.0 * ifl_cm / (ilag_cm - iflap_cm)));
                }


                // ==================== 第一部分：调整th_pa符号 ====================
                if (Math.Abs(ifl_cm) < eps)
                {
                    th_pa = 0.0;
                }
                else
                {
                    // 根据惯性矩比较和惯性积符号调整角度方向
                    if (iflap_cm >= ilag_cm)
                    {
                        th_pa = (ifl_cm > 0.0) ? -th_pa : th_pa;
                    }
                    else
                    {
                        th_pa = (ifl_cm < 0.0) ? -th_pa : th_pa;
                    }
                }

                // ==================== 第二部分：计算主刚度参数 ====================
                // 计算平均刚度和等效刚度
                double em_stiff = 0.5 * (eilbar + eifbar);
                double er_stiff = Math.Sqrt(0.25 * Math.Pow(eilbar - eifbar, 2) + Math.Pow(eiflbar, 2));
                double pflap_stff = 0;
                double plag_stff = 0;
                double the_pa = 0;
                // 确定主刚度方向
                if (eifbar <= eilbar)
                {
                    pflap_stff = em_stiff - er_stiff;
                    plag_stff = em_stiff + er_stiff;
                }
                else
                {
                    pflap_stff = em_stiff + er_stiff;
                    plag_stff = em_stiff - er_stiff;
                }

                // ==================== 第三部分：计算主轴角度 ====================
                // 计算主轴旋转角度（基于刚度参数）
                if (Math.Abs(eilbar - eifbar) < eps)
                {
                    the_pa = Math.PI / 4.0;
                }
                else
                {
                    the_pa = 0.5 * Math.Abs(Math.Atan(2.0 * eiflbar / (eilbar - eifbar)));
                }

                // 调整角度符号
                if (Math.Abs(eiflbar) > eps)
                {
                    if (eifbar >= eilbar)
                    {
                        the_pa = (eiflbar > 0) ? -the_pa : the_pa;
                    }
                    else
                    {
                        the_pa = (eiflbar < 0) ? -the_pa : the_pa;
                    }
                }
                else
                {
                    the_pa = 0.0;
                }

                // ==================== 第四部分：准备输出参数 ====================
                // 坐标系转换逻辑
                int id_form = 1;
                double tw_iner_d;
                double str_tw;
                if (id_form == 1)  // 风力涡轮机坐标系
                {
                    tw_iner_d = twist[i] * D2R - th_pa;
                    str_tw = twist[i] * D2R - the_pa;
                    y_sc = -y_sc;
                    ytc_ref = -ytc_ref;
                    ycm_ref = -ycm_ref;
                }
                else  // 直升机坐标系
                {
                    tw_iner_d = twist[i] * D2R + th_pa;
                    str_tw = twist[i] * D2R + the_pa;
                }

                // ==================== 单位转换和符号调整 ====================
                eiflbar = -eiflbar;    // 惯性积符号调整
                sfbar = -sfbar;      // 保留其他转换（示例未完全实现）
                sltbar = -sltbar;
                tw_iner_d = tw_iner_d * R2D;  // 弧度转角度


                this.output[i].EiFlap = eifbar;

                /// <summary>摆振刚度 (N·m²)</summary>
                this.output[i].EiLag = eilbar;

                /// <summary>扭转刚度 (N·m²/rad)</summary>
                this.output[i].GJ = gjbar;

                /// <summary>轴向刚度 (N)</summary>
                this.output[i].EA = eabar;

                /// <summary>挥舞方向剪切中心坐标 (米)</summary>
                this.output[i].SFl = eiflbar;

                /// <summary>前缘剪切中心坐标 (米)</summary>
                this.output[i].SAf = sfbar;

                /// <summary>后缘剪切中心坐标 (米)</summary>
                this.output[i].SAl = slbar;

                /// <summary>前缘拉伸中心坐标 (米)</summary>
                this.output[i].SFt = sftbar;

                /// <summary>后缘拉伸中心坐标 (米)</summary>
                this.output[i].SLt = sltbar;

                /// <summary>扭转中心坐标 (米)</summary>
                this.output[i].SAt = satbar;

                /// <summary>剪切中心X坐标 (米)</summary>
                this.output[i].XSc = z_sc;

                /// <summary>剪切中心Y坐标 (米)</summary>
                this.output[i].YSc = y_sc;

                /// <summary>扭转中心X坐标 (米)</summary>
                this.output[i].XTc = ztc_ref;

                /// <summary>扭转中心Y坐标 (米)</summary>
                this.output[i].YTc = ytc_ref;

                /// <summary>单位长度质量 (kg/m)</summary>
                this.output[i].Mass = mass;

                /// <summary>挥舞惯性矩 (kg·m²)</summary>
                this.output[i].FlapIner = iflap_eta;

                /// <summary>摆振惯性矩 (kg·m²)</summary>
                this.output[i].LagIner = ilag_zeta;

                /// <summary>扭转惯性矩 (kg·m²·deg)</summary>
                this.output[i].TwInerD = tw_iner_d;

                /// <summary>质心X坐标 (米)</summary>
                this.output[i].XCm = zcm_ref;

                /// <summary>质心Y坐标 (米)</summary>
                this.output[i].YCm = ycm_ref;

                this.output[i].str_tw = str_tw;



            }

            Writefile();


            Console.WriteLine();
        }
        /// <summary>
        /// 生成普通的输出文件
        /// </summary>
        private void GenerFile()
        {
            LogHelper.WriteLog($"{Otherhelper.GetCurrentProjectName()} BEGIN OUTPUT GENERATE FILE", leval: 1, color: ConsoleColor.Yellow);
            string path = pre.path.GetDirectoryName() + $"./{pre.data[1].Trim()}.out_gen";
            OutFile f = new OutFile(path,Scientific:6);
            f.WriteLine("====================================================================================");
            f.WriteLine($"结果由 {Otherhelper.GetCurrentProjectName()}(v{Otherhelper.GetCurrentVersion()}, {Otherhelper.GetBuildTime()}),文件生成于：{Otherhelper.GetCurrentTime()}");
            f.WriteLine($"{pre.data[1]}");
            f.WriteLine("====================================================================================");
            f.WriteLine("");
            f.WriteLine($"叶片总长(m) :{pre.Sections.Last().SpanLocation}");
            f.WriteLine("");
            f.Write(new string[]
            { "span", "chord", "tw_aero", "EiFlap", "EiLag", "GJ", "EA", "SFl", "s_af", "s_al", "s_ft", 
                "s_lt", "s_at", "x_sc", "y_sc", "x_tc", "y_tc", "Mass", "flap_iner", "lag_iner", "tw_iner", "x_cm", "y_cm"
            });
            f.WriteLine();
            f.Write(new string[]
            {
                "(m)", "(m)", "(deg)", "(Nm^2)", "(Nm^2)", "(Nm^2)", "(N)", "(Nm^2)", "(Nm)", "(Nm)", "(Nm^2)",
                "(Nm^2)", "(Nm)", "(m)", "(m)", "(m)", "(m)", "(Kg/m)", "(Kg-m)", "(Kg-m)", "(deg)", "(m)", "(m)"
            });
            f.WriteLine();
            for (int i = 0; i < pre.Sections.Length; i++)
            {
                string fg = "\t";
                if (!pre.UseTabDelimiter)
                {
                    fg = " ";
                }

                f.Write(pre.Sections[i].SpanLocation,fg);
                f.Write(pre.Sections[i].ChordLength,fg);
                f.Write(pre.Sections[i].TwistAngle,fg);
                f.Write(output[i].EiFlap,fg);
                f.Write(output[i].EiLag,fg);
                f.Write(output[i].GJ,fg);
                f.Write(output[i].EA,fg);
                f.Write(output[i].SFl,fg);
                f.Write(output[i].SAf,fg);
                f.Write(output[i].SAl,fg);
                f.Write(output[i].SFt,fg);
                f.Write(output[i].SLt,fg);
                f.Write(output[i].SAt,fg);
                f.Write(output[i].XSc,fg);
                f.Write(output[i].YSc,fg);
                f.Write(output[i].XTc,fg);
                f.Write(output[i].YTc,fg);
                f.Write(output[i].Mass,fg);
                f.Write(output[i].FlapIner,fg);
                f.Write(output[i].LagIner,fg);
                f.Write(output[i].TwInerD,fg);
                f.Write(output[i].XCm,fg);
                f.WriteLine(output[i].YCm);
            }
            f.WriteLine();
            f.Outfinish();
            LogHelper.WriteLog($"OUTPUT FILE SUCCESSFUL!", leval: 2, color: ConsoleColor.Green);
        }
        /// <summary>
        /// 输出BModes文件
        /// </summary>
        private void BmodesFile()
        {
            LogHelper.WriteLog($"{Otherhelper.GetCurrentProjectName()} BEGIN OUTPUT BMODES FILE", leval: 1, color: ConsoleColor.Yellow);
            string path = pre.path.GetDirectoryName() + $"./{pre.data[1].Trim()}.out_bmd";
            OutFile f = new OutFile(path, Scientific: 6);
            f.WriteLine("====================================================================================");
            f.WriteLine($"结果由 {Otherhelper.GetCurrentProjectName()}(v{Otherhelper.GetCurrentVersion()}, {Otherhelper.GetBuildTime()}),文件生成于：{Otherhelper.GetCurrentTime()}");
            f.WriteLine($"{pre.data[1]}");
            f.WriteLine("====================================================================================");
            f.WriteLine("");
            f.WriteLine($"叶片总长(m) :{pre.Sections.Last().SpanLocation}");
            f.WriteLine("");
            f.Write(new string[]
            {  "span","str_tw","tw_iner","mass_den","flp_iner","edge_iner","flp_stff","edge_stff","tor_stff","axial_stff","cg_offst ","sc_offst","tc_offst"

            });
            f.WriteLine();
            f.Write(new string[]
            {
                "(m)","(deg)","(deg)","(kg/m)","(kg-m)","(kg-m)","(Nm^2)","(Nm^2)","(Nm^2)","(N)","(m)","(m)","(m)"
            });
            f.WriteLine();

            for (int i = 0; i < pre.Sections.Length; i++)
            {
                string fg = "\t";
                if (!pre.UseTabDelimiter)
                {
                    fg = " ";
                }

                f.Write(pre.Sections[i].SpanLocation, fg);
                f.Write(output[i].str_tw*R2D, fg);
                f.Write(output[i].TwInerD, fg);
                f.Write(output[i].Mass, fg);
                f.Write(output[i].FlapIner, fg);
                f.Write(output[i].LagIner, fg);
                f.Write(output[i].EiFlap, fg);
                f.Write(output[i].EiLag, fg);
                f.Write(output[i].GJ, fg);
                f.Write(output[i].EA, fg);
                f.Write(output[i].YCm);
                f.Write(output[i].YSc, fg);
                f.WriteLine(output[i].YTc);

             
            }
            f.WriteLine();
            f.Outfinish();
            LogHelper.WriteLog($"OUTPUT FILE SUCCESSFUL!", leval: 2, color: ConsoleColor.Green);

        }
        private void BeamLFile()
        {
            LogHelper.WriteLog($"{Otherhelper.GetCurrentProjectName()} BEGIN OUTPUT BEAML FILE", leval: 1, color: ConsoleColor.Yellow);
            string path = pre.path.GetDirectoryName() + $"./{pre.data[1].Trim()}.out_beam";
            OutFile f = new OutFile(path);



            f.Outfinish();
            LogHelper.WriteLog($"OUTPUT FILE SUCCESSFUL!", leval: 2, color: ConsoleColor.Green);
        }
        /// <summary>
        /// 输出文件
        /// </summary>
        private void Writefile()
        {
            Console.WriteLine();
            LogHelper.WriteLog($"{Otherhelper.GetCurrentProjectName()} BEGIN OUTPUT FILE:", leval: 0, color: ConsoleColor.Blue);
            switch (pre.OutputFormat)
            {
                case 1: //通用文件
                    GenerFile();

                    break;
                case 2: // BModes文件
                    BmodesFile();
                    break;
                case 3: //通用与BModes文件
                    BmodesFile();
                    GenerFile();
                    break;
                case 4://矩阵形式的BeamL
                    BeamLFile();
                    break;
                case 5: //三格式
                    BeamLFile();
                    GenerFile();
                    BmodesFile();
                    break;
                default:
                    LogHelper.ErrorLog("未知输出格式");
                    break;



            }


        }

        /// <summary>
        /// 计算修正的刚度矩阵Q_tilde（对应Julia函数q_tildas的C#实现）
        /// </summary>
        /// <param name="qbar11">Q11刚度分量</param>
        /// <param name="qbar22">Q22刚度分量</param>
        /// <param name="qbar12">Q12刚度分量</param>
        /// <param name="qbar16">Q16刚度分量</param>
        /// <param name="qbar26">Q26刚度分量</param>
        /// <param name="qbar66">Q66刚度分量</param>
        /// <param name="mat">材料编号（用于错误提示）</param>
        /// <returns>2x2修正刚度矩阵</returns>
        private Matrix<double> QTildas(
            double qbar11, double qbar22, double qbar12,
            double qbar16, double qbar26, double qbar66,
            int mat)
        {
            // 初始化2x2矩阵（使用MathNet的稠密矩阵）
            Matrix<double> qtil = Matrix<double>.Build.Dense(2, 2);

            if (qbar22 == 0)
            {
                LogHelper.ErrorLog($"The Dev num qbar22 is 0!", FunctionName: "QTildas");
            }
            // 计算Q_tilde[1,1]分量（注意0-based索引）
            qtil[0, 0] = qbar11 - (qbar12 * qbar12) / qbar22;

            // 物理可行性检查
            if (qtil[0, 0] < 0.0)
            {
                LogHelper.ErrorLog($"Material {mat} properties are not physically realizable", FunctionName: "QTildas");
            }

            // 计算非对角线分量
            qtil[0, 1] = qbar16 - (qbar12 * qbar26) / qbar22;

            // 计算Q_tilde[2,2]分量
            qtil[1, 1] = qbar66 - (qbar26 * qbar26) / qbar22;


            // 保持矩阵对称性（如果需要）
            qtil[1, 0] = qtil[0, 1];

            return qtil;
        }

        /// <summary>
        /// 计算复合材料铺层的刚度矩阵分量（使用MathNet数学库）
        /// 对应原始Julia函数q_bars的转换实现
        /// </summary>
        /// <param name="mat">材料索引（0-based）</param>
        /// <param name="thp">铺层角度（弧度）</param>
        /// <param name="density">材料密度数组</param>
        /// <param name="q11">Q11刚度参数数组</param>
        /// <param name="q22">Q22刚度参数数组</param>
        /// <param name="q12">Q12刚度参数数组</param>
        /// <param name="q66">Q66刚度参数数组</param>
        /// <returns>包含7个刚度分量和材料密度的元组</returns>
        private (double qbar11, double qbar22, double qbar12, double qbar16, double qbar26, double qbar66, double rho_m) Q_Bars(
            int mat,
            double thp,
            Vector<double> density,
            Vector<double> q11,
            Vector<double> q22,
            Vector<double> q12,
            Vector<double> q66)
        {
            // 三角函数计算（使用MathNet的三角函数，支持复数等高级操作）
            //double ct = Math.Cos(thp);
            //double st = Math.Sin(thp);
            (double st, double ct) = Math.SinCos(thp);
            // 预计算多次项（使用幂函数优化计算）
            double c2t = Math.Pow(ct, 2);
            double c3t = Math.Pow(ct, 3);
            double c4t = Math.Pow(ct, 4);
            double s2t = Math.Pow(st, 2);
            double s3t = Math.Pow(st, 3);
            double s4t = Math.Pow(st, 4);

            // 中间参数计算（保持与原始公式相同的计算顺序）
            double s2thsq = 4.0 * s2t * c2t;

            // 材料参数获取（使用0-based索引）
            double k11 = q11[mat];
            double k22 = q22[mat];
            double k12 = q12[mat];
            double k66 = q66[mat];

            // 中间刚度组合参数
            double kmm = k11 - k12 - 2.0 * k66;
            double kmp = k12 - k22 + 2.0 * k66;

            // 计算旋转后的刚度分量（保持原始公式结构）
            double qbar11 = k11 * c4t + 0.5 * (k12 + 2.0 * k66) * s2thsq + k22 * s4t;
            double qbar22 = k11 * s4t + 0.5 * (k12 + 2.0 * k66) * s2thsq + k22 * c4t;
            double qbar12 = 0.25 * (k11 + k22 - 4.0 * k66) * s2thsq + k12 * (s4t + c4t);
            double qbar16 = kmm * st * c3t + kmp * s3t * ct;
            double qbar26 = kmm * s3t * ct + kmp * st * c3t;
            double qbar66 = 0.25 * (kmm + k22 - k12) * s2thsq + k66 * (s4t + c4t);

            // 材料密度获取
            double rho_m = density[mat];

            return (qbar11, qbar22, qbar12, qbar16, qbar26, qbar66, rho_m);
        }



        private (int[] isur, int[] idsect, double[] yseg, double[] zseg,
            double[] wseg, double[] sthseg, double[] cthseg, double[] s2thseg,
            double[] c2thseg) SegInfo(
         double ch, double rle, int nseg, int nseg_u, int nseg_p,
         Vector<double> xnode_u, Vector<double> ynode_u,
         Vector<double> xnode_l, Vector<double> ynode_l,
         int ndl1, int ndu1, double[] loc_web,
         double[] weby_u, double[] weby_l,
         int[] n_scts, int sec)
        {
            // 初始化输出数组（C# 0-based索引）
            int[] isur = new int[nseg];
            int[] idsect = new int[nseg];
            double[] yseg = new double[nseg];
            double[] zseg = new double[nseg];
            double[] wseg = new double[nseg];
            double[] sthseg = new double[nseg];
            double[] cthseg = new double[nseg];
            double[] s2thseg = new double[nseg];
            double[] c2thseg = new double[nseg];


            for (int iseg = 0; iseg < nseg; iseg++) // 0-based循环
            {
                int surfaceType = -1;
                double xa = 0, ya = 0, xb = 0, yb = 0;
                int currentWeb = 0;

                // 判断分段类型 (1-based逻辑转换为0-based)
                if (iseg < nseg_u) // 上表面分段
                {
                    int ndIndex = ndu1 - 1 + iseg; // 转换为0-based索引
                    xa = xnode_u[ndIndex];
                    ya = ynode_u[ndIndex];
                    xb = xnode_u[ndIndex + 1];
                    yb = ynode_u[ndIndex + 1];
                    surfaceType = 1;
                }
                else if (iseg < nseg_p) // 下表面分段
                {
                    int offset = iseg - nseg_u;
                    int ndIndex = ndl1 - 1 + offset;
                    xa = xnode_l[ndIndex];
                    ya = ynode_l[ndIndex];
                    xb = xnode_l[ndIndex + 1];
                    yb = ynode_l[ndIndex + 1];
                    surfaceType = 2;
                }
                else // 腹板分段
                {
                    currentWeb = iseg - nseg_p;
                    xa = loc_web[currentWeb];
                    xb = xa;
                    ya = weby_u[currentWeb];
                    yb = weby_l[currentWeb];
                    surfaceType = 0;
                }

                if (surfaceType == -1)
                {
                    LogHelper.ErrorLog($"无效的分段索引: {iseg + 1}", FunctionName: "SegInfo");
                }


                isur[iseg] = surfaceType;

                // 确定截面ID
                if (surfaceType > 0)
                {
                    bool found = false;
                    int sections = n_scts[surfaceType - 1]; // 获取对应表面的截面数量

                    for (int i = 0; i < sections; i++)
                    {
                        double lowerBound = 0; //xsec_node[surfaceType - 1,i] - eps;
                        double upperBound = 0;// xsec_node[surfaceType - 1,i + 1] + eps;
                        if (surfaceType == 1) //上表面
                        {
                            lowerBound = pre.Sections[sec].TopConfiguration.xsec_node[i] - eps;
                            upperBound = pre.Sections[sec].TopConfiguration.xsec_node[i + 1] + eps;
                        }

                        if (surfaceType == 2) //下表面
                        {
                            lowerBound = pre.Sections[sec].LowConfiguration.xsec_node[i] - eps;
                            upperBound = pre.Sections[sec].LowConfiguration.xsec_node[i + 1] + eps;
                        }
                        if (xa > lowerBound && xb < upperBound)
                        {
                            idsect[iseg] = i + 1; // 保持1-based截面ID
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        LogHelper.ErrorLog($"分段 {iseg + 1} 找不到对应截面", FunctionName: "SegInfo");
                    }

                }
                else
                {
                    idsect[iseg] = currentWeb + 1; // 腹板ID保持1-based
                }

                // 几何计算
                double xDiff = xb - xa;
                double yDiff = ya - yb;

                // 中点坐标计算
                yseg[iseg] = ch * (2.0 * rle - xa - xb) / 2.0;
                zseg[iseg] = ch * (ya + yb) / 2.0;

                // 分段长度
                wseg[iseg] = ch * Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

                // 角度计算
                double theta;
                if (surfaceType == 0)
                {
                    theta = -Math.PI / 2.0;
                }
                else
                {
                    theta = Math.Atan2(yDiff, xDiff); // 使用Atan2处理所有象限
                }

                sthseg[iseg] = Math.Sin(theta);
                cthseg[iseg] = Math.Cos(theta);
                s2thseg[iseg] = Math.Sin(2 * theta);
                c2thseg[iseg] = Math.Cos(2 * theta);
            }

            return (isur, idsect, yseg, zseg, wseg, sthseg, cthseg, s2thseg, c2thseg);
        }
        /// <summary>
        /// 计算扭角的变化率
        /// </summary>
        private Vector<double> tw_rate(Vector<double> sloc, Vector<double> tw_aero_d)
        {
            var naf = tw_aero_d.Count;
            var tw_aero = tw_aero_d * OpenWECD.IO.IO.PhysicalParameters.D2R;
            var th_prime = zeros(tw_aero_d.Count);
            // 计算中间点
            for (int i = 1; i < naf - 1; i++)
            {
                double f0 = tw_aero[i];
                double f1 = tw_aero[i - 1];
                double f2 = tw_aero[i + 1];
                double h1 = sloc[i] - sloc[i - 1];
                double h2 = sloc[i + 1] - sloc[i];
                th_prime[i] = (h1 * (f2 - f0) + h2 * (f0 - f1)) / (2.0 * h1 * h2);
            }

            // 计算边界点
            th_prime[0] = (tw_aero[1] - tw_aero[0]) / (sloc[1] - sloc[0]);
            th_prime[naf - 1] = (tw_aero[naf - 1] - tw_aero[naf - 2]) / (sloc[naf - 1] - sloc[naf - 2]);

            // 将结果从弧度转换为度
            for (int i = 0; i < naf; i++)
            {
                th_prime[i] *= (180.0 / Math.PI);
            }

            return th_prime;
        }
    }
}
