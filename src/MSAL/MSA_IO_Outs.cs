
//**********************************************************************************************************************************
// LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//                                      
//    This file is part of OpenWECD.MSA by 赵子祯, 2021, 2025
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

using OpenWECD.HawtC2;
using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using OpenWECD.MBD;
using System;
using System.Collections.Generic;
using System.Linq;
using static OpenWECD.IO.IO.PhysicalParameters;

namespace OpenWECD.MSAL
{
    public class MSA_IO_Outs : MSA_OutputParam
    {


        #region 初始化要求的信息
        private MSA_AllOuts AllOuts;

        private MSAL1 MSA;
        #endregion 初始化要求的信息

        private readonly bool SumPrint;
        private readonly int ChannelNum;


        private IO_ChannelType[] Hstchannel;

        /// <summary>
        /// 外部调用初始化模式,
        /// </summary>
        /// <param name="hst"></param>
        /// <param name="p"></param>
        public MSA_IO_Outs(HST_ModulInputFile hst, MSA_ParameterType p, MBD_ParameterType mp, string title = "Time", string unit = "(s)")
        {
            this.SumPrint = hst.AL_InputFile.SumPrint;
            MSA = hst.MSA_InputFile;
            //# 为输出变量申请内存
            AllOuts = MSA_INI.MSA_INIAllOuts(mp);//
            if (SumPrint)
            {
                MSA_OutCheckOutPar();//检查字典当中的变量长度是否正确
                MSA_OutFomart(ref MSA.Outputs_OutList, 10);
                MSA_OutCheckParmExist(MSA.Outputs_OutList);
                //# 申请内存之前，检查那些变量的的维度是不是正确，主要针对叶片和塔架以及划分单元的结构


                MSA_OutCheckParmSet(mp);


                //Get_MSAOutType = ControL_INI.INI_OutputType(p, hst.CL_InputFile.Servo); //# 不需要给外部模块所以不需要

                Hstchannel = MSA_OutSetChannelsOneHand(MSA, p);
                ChannelNum = Hstchannel.Length;//这个变量必须给定
                MSA_OutWriteTitle(hst.HstL_InputFile.SumPath, hst.HstL_InputFile.OutType, hst.HstL_InputFile.modelname, OpenWECD.IO.IO.PhysicalParameters.hsttitle, $"Powered By OpenWECD.{Otherhelper.GetCurrentProjectName()}.MSA @CopyRight 赵子祯");
                MSA_OutWriteUnit(title, unit);
            }
        }


        /// <summary>
        /// 根据当前的自由度和坐标，外力等信息计算输出。这个函数供外部调用MSA 使用
        /// </summary>
        /// <param name="t"></param>
        /// <param name="i_t"></param>
        /// <param name="p"></param>
        /// <param name="RtHS"></param>

        public void MSA_CalcOutput(double t, int i_t, MSA_ParameterType p, in MSA_RtHndSideType RtHS, in MBD_OtherStateType y, in MBD_RtHndSideType M_rths)
        {

           
        }
        /// <summary>
        /// 整个函数是将MSA作为静态模块来调用的，不建议使用
        /// </summary>
        /// <param name="RtHS"></param>
        /// <param name="p"></param>
        public void MSA_CalcOutput(in MSA_RtHndSideType RtHS, in MSA_ParameterType p)
        {

        }
        /// <summary>
        /// 将计算结果输出到文件当中,当前函数静静输出了PointList 表当中记录的风速，对于其他的输出变量，请调用另外的一个输出函数。
        /// </summary>
        /// <param name="t">当t=</param>
        /// <param name="i_t"></param>
        public void MSA_OutWriteResult(double t, int i_t)
        {
            for (int i = 0; i < ChannelNum; i++)
            {
                Hstchannel[i].OutFile.Write(t);
                for (int J = 0; J < Hstchannel[i].VariablesNum; J++)
                {
                    var temp = Hstchannel[i].OutputVariables[J];
                    Hstchannel[i].OutFile.Write(MSA_GetParamOutput(temp.name, temp.J, AllOuts, temp.K));
                }
                Hstchannel[i].OutFile.WriteLine();
            }
        }

        /// <summary>
        /// 为每个输入文件写入他们各自的单位和表头
        /// </summary>
        private void MSA_OutWriteUnit(string title = "Time", string unit = "(s)")
        {
            for (int i = 0; i < ChannelNum; i++)
            {
                Hstchannel[i].OutFile.Write(title);
                for (int j = 0; j < Hstchannel[i].VariablesNum; j++)
                {
                    Hstchannel[i].OutFile.Write(Hstchannel[i].OutputVariables[j].title);
                }
                Hstchannel[i].OutFile.WriteLine();
                Hstchannel[i].OutFile.Write(unit);
                for (int j = 0; j < Hstchannel[i].VariablesNum; j++)
                {
                    Hstchannel[i].OutFile.Write(Hstchannel[i].OutputVariables[j].unit);
                }
                Hstchannel[i].OutFile.WriteLine();

            }
        }
        /// <summary>
        /// 创建输出文件且写入标题
        /// </summary>
        /// <param name="file"></param>
        /// <param name="title"></param>
        private void MSA_OutWriteTitle(string Sumpath, IO_OutFileType filetype, string title1, params string[] title)
        {
            for (int i = 0; i < ChannelNum; i++)
            {
                //# 这个严格和后处理函数匹配，不允许更改！
                switch (filetype)
                {
                    case IO_OutFileType.文本:
                        Hstchannel[i].OutFile = new OutFile(Sumpath + "./" + MSA_OutGetChannelFileName(Hstchannel[i].ChannelNum, title1));
                        break;
                    case IO_OutFileType.二进制:
                        Hstchannel[i].OutFile = new OutFileB(Sumpath + "./" + MSA_OutGetChannelFileName(Hstchannel[i].ChannelNum, title1));
                        break;
                    default:
                        Hstchannel[i].OutFile = new OutFile(Sumpath + "./" + MSA_OutGetChannelFileName(Hstchannel[i].ChannelNum, title1));
                        break;
                }

                for (int j = 0; j < title.Length; j++)
                {
                    Hstchannel[i].OutFile.WriteLine(title[j]);
                }
            }
        }
        /// <summary>
        /// 获取当前频道的输出文件路径名称
        /// </summary>
        /// <param name="channelnum"></param>
        /// <param name="ProjectName"></param>
        /// <returns></returns>
        public static string MSA_OutGetChannelFileName(int channelnum, string ProjectName)
        {
            return "MSA_" + MSA_OutGetChannelName(channelnum) + ProjectName + ".MSA.out";
        }
        /// <summary>
        /// 这个方法是输出的核心，控制了变量的频道和输出的变量
        /// </summary>
        /// <param name="dyn"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private IO_ChannelType[] MSA_OutSetChannelsOneHand(MSAL1 dyn, MSA_ParameterType p)
        {
            List<IO_ChannelType> outputchannels = new List<IO_ChannelType>();

            List<int> channels = new List<int>();
            for (int i = 0; i < dyn.Outputs_OutList.Length; i++)
            {
                int cam = MSA_OutParChannel[dyn.Outputs_OutList[i]];
                if (channels.IndexOf(cam) == -1)
                {
                    channels.Add(cam);
                }
            }
            var temp = channels.ToArray();
            Array.Sort(temp);
            channels = temp.ToList();

            //遍历通道数量
            for (int i = 0; i < channels.Count; i++)
            {
                IO_ChannelType ChannelType = new IO_ChannelType();
                ChannelType.name = MSA_OutGetChannelName(channels[i]);//这个需要和API函数当中的读取方法同步，因此
                ChannelType.ChannelNum = channels[i];
                var ChannelTypeV = new List<IO_OutputVariablesType>();
                //# 想办法得到每个通道的输出变量 ChannelTypeV 
                for (int j = 0; j < dyn.Outputs_OutList.Length; j++)
                {

                    if (channels[i] == MSA_OutParChannel[dyn.Outputs_OutList[j]])
                    {
                        if (MSA_OutParDim[dyn.Outputs_OutList[j]] == 0) //0 表示没有维度
                        {
                            IO_OutputVariablesType hst = new IO_OutputVariablesType();
                            hst.name = dyn.Outputs_OutList[j];
                            hst.title = dyn.Outputs_OutList[j];
                            hst.unit = MSA_OutParUnit[dyn.Outputs_OutList[j]];
                            hst.ChannelNum = channels[i];
                            hst.ChannelName = MSA_OutGetChannelName(hst.ChannelNum);//         Enum.GetName(typeof(Loadchannls), hst.ChannelNum).Replace('_', ' ');
                            hst.J = 0;
                            hst.K = 0;
                            ChannelTypeV.Add(hst);
                        }
                        if (MSA_OutParDim[dyn.Outputs_OutList[j]] == 1)//1 表示只要部件 是叶片的目前
                        {
                            for (int K = 0; K < dyn.BldOutSig.Length; K++)
                            {
                                IO_OutputVariablesType hst = new IO_OutputVariablesType();
                                hst.name = dyn.Outputs_OutList[j];
                                hst.title = dyn.Outputs_OutList[j] + "B_" + dyn.BldOutSig[K];
                                hst.unit = MSA_OutParUnit[dyn.Outputs_OutList[j]];
                                hst.ChannelNum = channels[i];
                                hst.ChannelName = MSA_OutGetChannelName(hst.ChannelNum); ;// Enum.GetName(typeof(Loadchannls), hst.ChannelNum).Replace('_', ' ');
                                hst.J = 0;
                                hst.K = dyn.BldOutSig[K];
                                ChannelTypeV.Add(hst);
                            }
                        }
                        if (MSA_OutParDim[dyn.Outputs_OutList[j]] == 2)//2 表示只要节点 循环J
                        {
                            if (dyn.Outputs_OutList[j][0..3] == "TwH")//塔架，要和MSA excle当中的关键字一致，塔架是TwH的前缀
                            {
                                //# BJJ 2024.07.31 该"TwN"是因为MSA模块当中的

                                for (int J = 0; J < dyn.TwOutNd.Length; J++)
                                {
                                    IO_OutputVariablesType hst = new IO_OutputVariablesType();
                                    hst.name = dyn.Outputs_OutList[j];
                                    hst.title = dyn.Outputs_OutList[j] + "N_" + dyn.TwOutNd[J];
                                    hst.unit = MSA_OutParUnit[dyn.Outputs_OutList[j]];
                                    hst.ChannelNum = channels[i];
                                    hst.ChannelName = MSA_OutGetChannelName(hst.ChannelNum); //Enum.GetName(typeof(Loadchannls), hst.ChannelNum).Replace('_', ' ');
                                    hst.J = dyn.TwOutNd[J];
                                    hst.K = 0;
                                    ChannelTypeV.Add(hst);
                                }
                            }
                            else//叶片
                            {

                                for (int J = 0; J < dyn.BlOutNd.Length; J++)
                                {
                                    IO_OutputVariablesType hst = new IO_OutputVariablesType();
                                    hst.name = dyn.Outputs_OutList[j];
                                    hst.title = dyn.Outputs_OutList[j] + "N_" + dyn.BlOutNd[J];
                                    hst.unit = MSA_OutParUnit[dyn.Outputs_OutList[j]];
                                    hst.ChannelNum = channels[i];
                                    hst.ChannelName = MSA_OutGetChannelName(hst.ChannelNum); // Enum.GetName(typeof(Loadchannls), hst.ChannelNum).Replace('_', ' ');
                                    hst.J = dyn.BlOutNd[J];
                                    hst.K = 0;
                                    ChannelTypeV.Add(hst);
                                }
                            }

                        }
                        if (MSA_OutParDim[dyn.Outputs_OutList[j]] == 3)//只有叶片是要[J,K]
                        {
                            for (int K = 0; K < dyn.BldOutSig.Length; K++)
                            {
                                for (int J = 0; J < dyn.BlOutNd.Length; J++)
                                {
                                    IO_OutputVariablesType hst = new IO_OutputVariablesType();
                                    hst.name = dyn.Outputs_OutList[j];
                                    hst.title = dyn.Outputs_OutList[j] + "B_" + dyn.BldOutSig[K] + "_N_" + dyn.BlOutNd[J];
                                    hst.unit = MSA_OutParUnit[dyn.Outputs_OutList[j]];
                                    hst.ChannelNum = channels[i];
                                    hst.ChannelName = MSA_OutGetChannelName(hst.ChannelNum);// Enum.GetName(typeof(Loadchannls), hst.ChannelNum).Replace('_', ' ');
                                    hst.J = dyn.BlOutNd[J];
                                    hst.K = dyn.BldOutSig[K];
                                    ChannelTypeV.Add(hst);
                                }
                            }
                        }

                    }
                }
                ChannelType.OutputVariables = ChannelTypeV.ToArray();
                ChannelType.VariablesNum = ChannelType.OutputVariables.Length;
                outputchannels.Add(ChannelType);

            }

            return outputchannels.ToArray();
        }
        /// <summary>
        /// 获取当前频道的名称字符串
        /// </summary>
        /// <param name="channelnum"></param>
        /// <returns></returns>
        public static string MSA_OutGetChannelName(int channelnum)
        {
            string temp = Enum.GetName(typeof(MSA_Loadchannels), channelnum);
            return temp.Replace('_', ' ') + " ";
        }
        /// <summary>
        /// 检查字典当中的变量长度是否正确
        /// </summary>
        /// <param name="length"></param>
        private void MSA_OutCheckOutPar(int length = 10)
        {
            foreach (var item in MSA_OutParUnit)
            {
                if (item.Value.Length != length)
                {
                    LogHelper.ErrorLog("The length of " + item.Key + " is not " + length, FunctionName: "MSA_OutCheckOutPar");
                }
            }
        }
        /// <summary>
        /// 将文件当中的变量名转换为指定长度，以便符合字典的查找规制
        /// </summary>
        /// <param name="list"></param>
        /// <param name="length"></param>
        private void MSA_OutFomart(ref string[] list, int length)
        {
            for (int i = 0; i < list.Length; i++)
            {
                MSA_OutFomart(ref list[i], length);
            }
        }
        /// <summary>
        /// 将文件当中的变量名转换为指定长度，以便符合字典的查找规制
        /// </summary>
        /// <param name="key"></param>
        /// <param name="length"></param>
        public static void MSA_OutFomart(ref string key, int length = 10)
        {

            if (key.Length != length & key.Length < length)
            {
                int num = length - key.Length;
                string temp = "";
                for (int j = 0; j < num; j++)
                {
                    temp = temp + " ";
                }
                key = key + temp;
            }
            else if (key.Length > length)
            {
                LogHelper.ErrorLog("MSA_OutFomart Error!The outputkey length must equel 9", FunctionName: "MSA_OutFomart");
            }

        }
        /// <summary>
        /// 检查变量数组的长度是否正确，变量是否存在
        /// </summary>
        /// <param name="list"></param>
        private void MSA_OutCheckParmExist(string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                MSA_OutCheckParmExist(list[i]);
            }
        }
        /// <summary>
        /// 检查当前关键字是否纯在
        /// </summary>
        /// <param name="key"></param>
        public static void MSA_OutCheckParmExist(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                LogHelper.ErrorLog("The " + key + " is null or Empty", FunctionName: "MSA_OutCheckParmExist");
            }
            MSA_OutFomart(ref key);
            if (!MSA_OutParUnit.ContainsKey(key))
            {
                LogHelper.ErrorLog("The MSA Outpar:" + "\"" + key + "\"" + " is not exist,please check MSA_main file", FunctionName: "MSA_OutCheckParmExist");
            }
        }

        /// <summary>
        /// 申请内存之前，检查那些变量的的维度是不是正确，主要针对叶片和塔架以及划分单元的结构
        /// </summary>
        /// <param name="p"></param>
        private void MSA_OutCheckParmSet(MBD_ParameterType p)
        {
            //# 先检查叶片数量
            if (MSA.BldOutSig.Max() > p.NumBl)
            {
                LogHelper.ErrorLog($"MSA.Out error!The BldOutsig node{MSA.BldOutSig.Max()} exceed blade num [0,{p.NumBl}]", FunctionName: "MSA_OutCheckParmSet");
            }
            //# 再检查叶片的节点
            if (MSA.BlOutNd is null)
            {
                LogHelper.WarnLog("MSA.Out Warning!The BlOutNd is null", FunctionName: "MSA_OutCheckParmSet");
            }
            else if (MSA.BlOutNd.Max() > p.BldNodes - 1)
            {
                LogHelper.ErrorLog($"MSA.Out error!The BlOutNd node {MSA.BlOutNd.Max()} exceed blade nodes [0,{p.BldNodes - 1}]", FunctionName: "MSA_OutCheckParmSet");
            }
            //# 再检查塔架的节点
            if (MSA.TwOutNd is null)
            {
                LogHelper.WarnLog("MSA.Out Warning!The TwOutNd is null", FunctionName: "MSA_OutCheckParmSet");
            }
            else if (MSA.TwOutNd.Max() > p.TwrNodes)
            {
                LogHelper.ErrorLog($"MSA.Out error!The TwOutNd node {MSA.TwOutNd.Max()} exceed tower nodes [0,{p.TwrNodes - 1}]", FunctionName: "MSA_OutCheckParmSet");
            }

        }

    }
}
