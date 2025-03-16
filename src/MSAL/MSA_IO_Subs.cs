

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.AeroL
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
using OpenWECD.IO.Log;
using System;
using System.Diagnostics;
using System.Linq;
using OpenWECD.IO.math;
using static OpenWECD.IO.math.LinearAlgebraHelper;

namespace OpenWECD.MSAL
{
    public class MSA_IO_Subs :  IModulInterYml<MSAL1>
    {
        #region 文件读取区域

        public static MSAL1 ReadMSA_MainFile(string path)
        {
            var msa = new MSAL1();
            CheckError.Filexists(path);
            msa.MSALfilepath = path;
            CheckError.Filexists(path);
            msa.MSALData = File.ReadAllLines(path);

            int fd(string temp, bool error = true, bool show = true) => Otherhelper.GetMatchingLineIndexes(msa.MSALData, temp, path, error, show)[0];





            //# 输出文件的读取和设置
            msa.SumPrint = Otherhelper.ParseLine<bool>(msa.MSALData, path, fd(" SumPrint "), moren: true, key: "SumPrint");
            if (msa.SumPrint)
            {
                msa.AfSpanput = Otherhelper.ParseLine<bool>(msa.MSALData, path, fd(" AfSpanput "));
                msa.SumPath = Otherhelper.ParseLine<string>(msa.MSALData, path, fd(" SumPath "));
                msa.BldOutSig = Otherhelper.ParseLine<int[]>(msa.MSALData, path, fd(" BldOutSig "), fg1: ',', moren: new int[] { 0 });
                string temp = msa.SumPath + "1.out";
                CheckError.Filexists(path, ref temp, true, ".out", true);
                msa.SumPath = temp.Replace("1.out", " ").Trim();
                //# 叶片输出节点
                msa.NBlOuts = Otherhelper.ParseLine<int>(msa.MSALData, path, fd(" NBlOuts "));
                if (msa.NBlOuts != 0)
                {
                    msa.BlOutNd = Otherhelper.ParseLine<int[]>(msa.MSALData, path, fd(" BlOutNd "), fg1: ',');
                    if (msa.NBlOuts > msa.BlOutNd.Length)
                    {
                        LogHelper.ErrorLog($"msa.BlOutNd>msa.BlOutNd.Length", FunctionName: "Readmsa_MainFile");
                    }
                    msa.BlOutNd = msa.BlOutNd[0..msa.NBlOuts];
                }
                //# 塔架输出节点
                msa.NTwOuts = Otherhelper.ParseLine<int>(msa.MSALData, path, fd(" NTwOuts "));
                if (msa.NTwOuts != 0)
                {
                    msa.TwOutNd = Otherhelper.ParseLine<int[]>(msa.MSALData, path, fd(" TwOutNd "), fg1: ',');
                    if (msa.NTwOuts > msa.TwOutNd.Length)
                    {
                        LogHelper.ErrorLog($"msa.NTwOuts > msa.TwOutNd.Length", FunctionName: "Readmsa_MainFile");
                    }
                    msa.TwOutNd = msa.TwOutNd[0..msa.NTwOuts];
                }
                msa.Outputs_OutList = Otherhelper.ReadOutputWord(msa.MSALData, fd(" OutList ") + 1, true);
            }
            return msa;
        }

        public static MSAL1 ReadMSA_MainFile(in YML yml, string key = "OpenWECD.OpenHAST.MSAL")
        {
            var msa = new MSAL1();
            string genkey(string value) => key + "." + value;
            //# 检查文件是否包含相关信息
            if (!yml.ChickfindNodeByKey(key))
            {
                LogHelper.ErrorLog($"Cant find the key:{key} information in the file", FunctionName: "ReadMSA_MainFile");
            }
            msa.MSALfilepath = yml.read(genkey("MSALfilepath")).YmlToString();
            msa.MSALData = yml.read(genkey("MSALData")).YmlToStringArray();





            //# 输出文件的读取和设置
            msa.SumPrint = yml.read(genkey("SumPrint")).YmlToBool();//<bool>(msa.msaData, path, fd(" SumPrint "), true);
            if (msa.SumPrint)
            {
                msa.AfSpanput = yml.read(genkey("AfSpanput")).YmlToBool();//<bool>(msa.msaData, path, fd(" AfSpanput "));
                msa.SumPath = yml.read(genkey("SumPath")).YmlToString();//<string>(msa.msaData, path, fd(" SumPath "));
                msa.BldOutSig = yml.read(genkey("BldOutSig")).YmlToIntArray();//<int[]>(msa.msaData, path, fd(" BldOutSig "), fg1: ',', moren: new int[] { 0 });
                //string temp = msa.SumPath + "1.out";
                //CheckError.Filexists(path, ref temp, true, ".out", true);
                msa.SumPath = yml.read(genkey("SumPath")).YmlToString();// temp.Replace("1.out", " ").Trim();
                //# 叶片输出节点
                msa.NBlOuts = yml.read(genkey("NBlOuts")).YmlToInt();//<int>(msa.msaData, path, fd(" NBlOuts "));
                if (msa.NBlOuts != 0)
                {
                    msa.BlOutNd = yml.read(genkey("BlOutNd")).YmlToIntArray();//<int[]>(msa.msaData, path, fd(" BlOutNd "), fg1: ',');
                    if (msa.NBlOuts > msa.BlOutNd.Length)
                    {
                        LogHelper.ErrorLog($"msa.BlOutNd>msa.BlOutNd.Length", FunctionName: "Readmsa_MainFile");
                    }
                    msa.BlOutNd = msa.BlOutNd[0..msa.NBlOuts];
                }
                //# 塔架输出节点
                msa.NTwOuts = yml.read(genkey("NTwOuts")).YmlToInt();//<int>(msa.msaData, path, fd(" NTwOuts "));
                if (msa.NTwOuts != 0)
                {
                    msa.TwOutNd = yml.read(genkey("TwOutNd")).YmlToIntArray();//<int[]>(msa.msaData, path, fd(" TwOutNd "), fg1: ',');
                    if (msa.NTwOuts > msa.TwOutNd.Length)
                    {
                        LogHelper.ErrorLog($"msa.NTwOuts > msa.TwOutNd.Length", FunctionName: "Readmsa_MainFile");
                    }
                    msa.TwOutNd = msa.TwOutNd[0..msa.NTwOuts];
                }
                msa.Outputs_OutList = yml.read(genkey("Outputs_OutList")).YmlToStringArray();//<string[]>(msa.msaData, path, fd(" Outputs_OutList "), fg1: ',');
            }
            return msa;
        }
        #endregion 文件读取区域

        /// <summary>
        /// 向YML文件中写入MSAL1 结构体
        /// </summary>
        /// <param name="yml"></param>
        /// <param name="aeroL"></param>
        public void ConvertToYML(ref YML yml, MSAL1 aeroL)
        {
            LogHelper.WriteLogO("开始转换MSAL 为 yml");
            YML.ConvertStructToYML(ref yml, "OpenWECD.OpenHAST.MSAL", 4, aeroL);
            LogHelper.WriteLogO("转换MSAL 成功！");

        }
    }
}
