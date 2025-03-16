


//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.AeroL.Airfoil.DynamicStallModal
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
using OpenWECD.AeroL.BEMT.SteadyBEMT;
using OpenWECD.IO.Log;
using OpenWECD.IO.math;
using OpenWECD.IO.IO;
using OpenWECD.AeroL.Airfoil.DynamicStallModal;
using static OpenWECD.IO.math.InterpolateHelper;
using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;
using System.Data;
namespace OpenWECD.AeroL.Airfoil.DynamicStallModal
{
    /// <summary>
    ///  Øye 失速模型
    /// </summary>
    public class Oye : I_ALCalDynamicStall
    {
        private Airfoil1 Airfoil;
        int currentT = -1;

        public Oye(AER_ParameterType p)
        {
            LogHelper.WriteLog("Run Oye Dynamic Stall Modol to Solve！", title: "[success]", leval: 1);
            Airfoil = p.Airfoil;//已经在初始化p的时候初始化了airfoil的基础插值功能
            AirFoilINI(ref Airfoil);//初始化当前模型要求的数据
            Debug.WriteLine("AirFoilINI com");
        }
        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum, double AOA, ref T_ALAeroBladeElement rths, T_ALAeroBladeElement srths)
        {
            //#基于该接口实现
            return (rths.DCl, rths.DCd, rths.DCm);
        }
        /// <summary>
        /// 直接返回
        /// </summary>
        /// <param name="t"></param>
        /// <param name="dt"></param>
        /// <param name="nstep"></param>
        /// <param name="FoilNum"></param>
        /// <param name="AOA"></param>
        /// <returns></returns>
        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum, double AOA)
        {
            double DCl = Airfoil.IterCl[FoilNum].Interpolate(AOA);
            double DCd = Airfoil.IterCd[FoilNum].Interpolate(AOA);
            double DCm = Airfoil.IterCm[FoilNum].Interpolate(AOA);
            return (DCl, DCd, DCm);


        }


        public Airfoil1 GetAirfoil1()
        {
            return this.Airfoil;
        }
        private void AirFoilINI(ref Airfoil1 air)
        {
            for (int i = 0; i < air.SecDataSet.Length; i++)
            {


                //先计算分离函数的静态值fs
                //# 1、求解斜率和0升力迎角
                //## 注意：0升力迎角定义在-90--+90度之间，其他的点我们并不需要，为了数值和程序，我们对应为Min(ABS(AOA)).Cl=0
                double AOACl_0 = AeroL_Airfoil_Subs.AOA_CL_0(air.SecDataSet[i].Column(0), air.SecDataSet[i].Column(1));
                if (AOACl_0 == 0)//升力曲线是平的，直接当静态对待
                {
                    air.IterC4[i] = Inter1D(air.SecDataSet[i].Column(0), Vector<double>.Build.Dense(air.SecDataSet[i].Column(0).Count, 1.0));
                    air.IterC5[i] = Inter1D(air.SecDataSet[i].Column(0), Vector<double>.Build.Dense(air.SecDataSet[i].Column(0).Count, 0.0));
                    air.IterC6[i] = Inter1D(air.SecDataSet[i].Column(0), Vector<double>.Build.Dense(air.SecDataSet[i].Column(0).Count, 0.0));
                    continue;
                }
                //# 2、计算每一个攻角下的f_st，对应第4列
                //## 2.1 先计算附着流线性区域的升力线斜率 Cl,a
                double Cl_a = 0;
                List<double> Var1 = new List<double>();
                for (int j = 0; j < air.SecDataSet[i].RowCount; j++)
                {
                    Var1.Add(air.SecDataSet[i].Column(1)[j] / (air.SecDataSet[i].Column(0)[j] - AOACl_0));
                }
                Cl_a = Var1.Max();

                //Console.WriteLine(Cl_a);
                //continue;


                var f_st = LinearAlgebraHelper.zeros(air.SecDataSet[i].RowCount);

                for (int j = 0; j < air.SecDataSet[i].RowCount; j++)
                {
                    var tesp = air.SecDataSet[i].Column(1)[j] / (Cl_a * (air.SecDataSet[i].Column(0)[j] - AOACl_0));
                    if (tesp < 0)
                    {
                        f_st[j] = 0;
                    }
                    else
                    {
                        var tespp = Math.Sqrt(tesp);
                        f_st[j] = Math.Pow(2.0 * tespp - 1.0, 2);
                    }

                }

                //## 2.2 显然的是f_st存在一定的问题，需要对数据进行修剪。
                (double aoalowup, _) = RootsHelper.Fabssearch(air.SecDataSet[i].Column(0), f_st, -89 * Math.PI / 180, 89 * Math.PI / 180);
                aoalowup = Math.Abs(aoalowup);
                //## 2.3 对绝对值大于aoalowup的值进行二次修正，并保证f_st不大于一
                for (int j = 0; j < air.SecDataSet[i].RowCount; j++)
                {
                    if (Math.Abs(air.SecDataSet[i].Column(0)[j]) >= aoalowup)
                    {
                        f_st[j] = 0;
                    }
                    if (f_st[j] > 1)
                    {
                        f_st[j] = 1;
                    }
                    //检查错误
                    if (f_st[j] is double.NaN | f_st[j] is double.PositiveInfinity | f_st[j] is double.NegativeInfinity)
                    {
                        LogHelper.ErrorLog("The f_st is Nan!", FunctionName: "Oye.AirFoilINI");
                    }

                }
                //## 2.4 创建插值第4列的插值
                if (air.InterpOrd == 1)
                {
                    air.IterC4[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), f_st, meth: "linear");
                }
                else
                {
                    air.IterC4[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), f_st, meth: "pchip");
                }

                //# 3、基于上述的0升力攻角以及斜率，获取CL_inv是没有任何分离的非黏性流动升力系数

                //## 3.1 依据斜截法计算直线
                var Cl_inv = LinearAlgebraHelper.zeros(air.SecDataSet[i].RowCount);
                for (int j = 0; j < air.SecDataSet[i].RowCount; j++)
                {
                    //y=K(x-x0),其中K= Cl_a,x0=AOACl_0
                    Cl_inv[j] = Cl_a * (air.SecDataSet[i][j, 0] - AOACl_0);
                    //检查错误
                    if (Cl_inv[j] is double.NaN | Cl_inv[j] is double.NegativeInfinity | Cl_inv[j] is double.PositiveInfinity)
                    {
                        LogHelper.ErrorLog("The Cl_inv is Nan!", FunctionName: "Oye.AirFoilINI");
                    }
                }
                //## 3.2 创建插值
                if (air.InterpOrd == 1)
                {
                    air.IterC5[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), Cl_inv, meth: "linear");
                }
                else
                {
                    air.IterC5[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), Cl_inv, meth: "pchip");
                }

                //# 4、基于上述结果计算完全分离的升力系数Cl_fs
                //## 4.1 依据斜截法计算直线
                var Cl_fs = LinearAlgebraHelper.zeros(air.SecDataSet[i].RowCount);
                List<int> 奇异点集合 = new List<int>();
                for (int j = 0; j < Cl_fs.Count; j++)
                {
                    Cl_fs[j] = (air.SecDataSet[i][j, 1] - Cl_a * (air.SecDataSet[i][j, 0] - AOACl_0) * f_st[j]) / (1.0 - f_st[j]);

                    //显然的是，当f_st值为1时，Cl_fs值为NaN，因此，这一个点的值并不能直接求解，只能用相邻点来插值
                    if (Cl_fs[j] is double.NaN | Cl_fs[j] is double.PositiveInfinity | Cl_fs[j] is double.NegativeInfinity)
                    {
                        奇异点集合.Add(j);
                    }

                    ////检查错误
                    //if (Cl_fs[j] is double.NaN)
                    //{
                    //    LogHelper.ErrorLog("The Cl_fs is Nan!", FunctionName: "Oye.AirFoilINI");
                    //}
                }
                //###4.1.1 对奇异点进行修正
                if (奇异点集合.Count != 0)
                {
                    int min = 奇异点集合.Min();
                    int max = 奇异点集合.Max();
                    if (min <= 0 | max >= air.SecDataSet[i].RowCount - 1)
                    {
                        LogHelper.ErrorLog("奇异点集合无法修正！", FunctionName: "Oye.AirFoilINI");
                    }
                    else
                    {
                        Vector<double> x = LinearAlgebraHelper.zeros(air.SecDataSet[i].Column(0)[min - 1], air.SecDataSet[i].Column(0)[max + 1]);
                        Vector<double> y = LinearAlgebraHelper.zeros(Cl_fs[min - 1], Cl_fs[max + 1]);
                        var itr = InterpolateHelper.Inter1D(x, y);
                        for (int k = 0; k < 奇异点集合.Count; k++)
                        {
                            Cl_fs[奇异点集合[k]] = itr.Interpolate(air.SecDataSet[i].Column(0)[奇异点集合[k]]);
                        }
                    }
                }
                //## 4.2 创建插值
                if (air.InterpOrd == 1)
                {
                    air.IterC6[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), Cl_fs, meth: "linear");
                }
                else
                {
                    air.IterC6[i] = InterpolateHelper.Inter1D(air.SecDataSet[i].Column(0), Cl_fs, meth: "pchip");
                }
                Debug.WriteLine("AirFoilINI 成功！");

            }


        }



    }
}
