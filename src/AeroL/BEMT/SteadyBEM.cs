

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.AeroL.BEMT
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
using OpenWECD.IO.Log;
using OpenWECD.IO.math;
using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using static OpenWECD.IO.IO.PhysicalParameters;
using static OpenWECD.IO.math.MathHelper;
using static OpenWECD.IO.math.RootsHelper;
using NUMT = System.Single;

namespace OpenWECD.AeroL.BEMT.SteadyBEMT
{
    public class SteadyBEM
    {
        #region 字段区域

        private AeroL1 aerol;
        private Vector<double> BlSpn;
        private int nb_Load;
        private int Bldnum;
        #endregion 字段区域



        public SteadyBEM(AeroL1 aerols)
        {
            LogHelper.WriteLog("Run SteadyBEM Solve！", title: "[success]");
            aerol = aerols;
            BlSpn = aerol.BldGeo[0].Span + aerol.HubRad;
            nb_Load = aerol.BldGeo[0].NumBladeSection;
            Bldnum = aerol.Bldnum;
        }

        #region 计算方法实现区域


        /// <remarks>
        /// BEM静态理论来计算叶片的各种系数
        /// </remarks>
        /// <param name="Airfoils"></param>
        /// <param name="λ"></param>
        /// <param name="HubRad"></param>
        /// <param name="pitch">[rad]</param>
        /// <param name="rho"></param>
        /// <param name="BladeNum"></param>
        /// <returns><br/>
        /// 轴向诱导因子a<br/> 
        /// 切向诱导因子b<br/>
        /// 入流角phi<br/>
        /// 攻角aoa<br/>
        /// Cx推力系数<br/>
        /// Cy扭矩系数<br/>
        /// Cp 扭矩系数-》功率系数
        /// Ct 总的推力系数
        /// </returns>
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double Cp, double Ct) BEM(ref AER_RtHndSideType bld, Airfoil1 Airfoils, double λ, double windspeed, double HubRad, double pitch = 0.0, double rho = 1.225, int BladeNum = 3)
        {

            double TipRad = bld.BldEle[0, 0].STipRad;
            bld.RtTSR = λ;
            double v = windspeed;
            double ω = λ * v / TipRad;

            //输出
            double Power = 0;
            double TuiLi = 0;
            Vector<double> test = Vector<double>.Build.Dense(nb_Load);
            Vector<double> test2 = Vector<double>.Build.Dense(nb_Load);
            for (int i = 0; i < Bldnum; i++)
            {


                double a1 = 0.3;
                double b1 = 0.005;
                double φ = 0.0;
                double AOA = 0.0;
                double cn = 0.0;
                double Ct = 0.0;
                double cl = 0.0;
                double cd = 0.0;
                double cm = 0.0;
                int n = 0;
                double Vlocal = 0.0;
                double sp = 0;
                for (int j = 1; j < nb_Load - 1; j++)
                {
                    double theta = bld.BldEle[j, i].STwist + pitch;
                    double Solid = bld.BldEle[j, i].SSolid / bld.BldEle[j, i].SBlspan;
                    (a1, b1, φ, AOA, cn, Ct, n, cl, cd, cm) = CalcOutputS(λ, theta, bld.BldEle[j, i].SFoilNum, Airfoils, TipRad, HubRad, bld.BldEle[j, i].SBlspan, Solid, a1, b1, BladeNum);
                    if (n <= 1998)
                    {
                        Vlocal = v * (1.0 - a1) / Math.Sin(φ);
                        sp = 0.5 * rho * Vlocal * Vlocal * bld.BldEle[j, i].SChord;

                        test[j] = sp * Ct;
                        test2[j] = sp * cn;
                    }
                    else
                    {
                        test[j] = 0.0;
                        test2[j] = 0.0;
                    }
                    //赋值
                    bld.RtTSR = λ;
                    bld.BldEle[j, i].Da = a1;
                    bld.BldEle[j, i].Daa = b1;
                    bld.BldEle[j, i].Dphi = φ;
                    bld.BldEle[j, i].DAOA = AOA;
                    bld.BldEle[j, i].DCx = cn;
                    bld.BldEle[j, i].DCy = Ct;
                    bld.BldEle[j, i].DCl = cl;
                    bld.BldEle[j, i].DCd = cd;
                    bld.BldEle[j, i].DCm = cm;
                    bld.BldEle[j, i].DU = Vlocal;
                    bld.BldEle[j, i].DVx = (NUMT)v;
                    bld.BldEle[j, i].DVy = (NUMT)ω * (NUMT)bld.BldEle[j, i].SBlspan;
                    bld.BldEle[j, i].DPx = test2[j];
                    bld.BldEle[j, i].DPy = test[j];
                    bld.BldEle[j, i].DTheta = theta;
                    bld.BldEle[j, i].DMz =  cm * sp * bld.BldEle[j, i].SChord;
                    bld.BldEle[j, i].DMx = test[j] * bld.BldEle[j, i].SBlspan;
                    bld.BldEle[j, i].Vindx = v * (1.0 - a1);
                    bld.BldEle[j, i].Vindy = v * (1.0 + b1);
                    bld.BldEle[j, i].DVrel = (float)Vlocal;
                    bld.RtSpeed = ω;

                }
                double tor = IntegrationHelper.Trapz(BlSpn, test.PointwiseMultiply(BlSpn));
                Power = Power + tor * ω;
                double tui = IntegrationHelper.Trapz(BlSpn, test2);
                TuiLi = TuiLi + tui;
                for (int k = 0; k < nb_Load - 1; k++)
                {
                    bld.BldEle[k, i].Torque = tor;
                    bld.BldEle[k, i].Thrust = tui;
                }

            }
            bld.RtAeroPwr = Power;
            double Cp = bld.RtAeroCp = Power / (0.5 * rho * Math.PI * TipRad * TipRad * v * v * v);
            double Ct_result = bld.RtAeroCt = TuiLi / (0.5 * rho * Math.PI * TipRad * TipRad * v * v);

            if (Cp > 0.59)
            {
                Console.WriteLine($"Cp:{Cp} too large!");
            }
            return (Cp, Ct_result);
        }



        /// <remarks>
        /// 计算叶片的气动力
        /// </remarks>
        /// <param name="TSR">叶尖速比</param>
        /// <param name="Pit">变桨角[rad]</param>
        /// <param name="Bld">叶片几何</param>
        /// <param name="Airfoil">翼型结构体</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="rho"></param>
        /// <param name="Bldnum"></param>
        /// <returns></returns>
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private double CalculateCp(double TSR, double Pit, ref AER_RtHndSideType Bld, Airfoil1 Airfoil, double HubRad, double rho = 1.225, int Bldnum = 3)
        {
            (double Cp, _) = BEM(ref Bld, Airfoil, TSR, 10, HubRad, Pit, rho, Bldnum);
            return Cp; // 返回 Cp 值

        }
        /// <remarks>
        /// 计算多个Cp值，每一行是相同的尖速比，不同的变桨角。
        /// </remarks>
        /// <param name="minTSR"></param>
        /// <param name="maxTSR"></param>
        /// <param name="TSRstp"></param>
        /// <param name="Bld"></param>
        /// <param name="Airfoil"></param>
        /// <param name="HubRad"></param>
        /// <param name="minPit"></param>
        /// <param name="maxPit"></param>
        /// <param name="Pitstp"></param>
        /// <param name="rho"></param>
        /// <param name="Bldnum"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Matrix<double> CalculateCp(AeroL1 aerol, double minTSR, double maxTSR, double TSRstp, ref AER_RtHndSideType Bld, Airfoil1 Airfoil, double HubRad, double minPit = 0.0, double maxPit = 0.0, double Pitstp = 0.0, double rho = 1.225, int Bldnum = 3)
        {
            var TSR = Range(minTSR, maxTSR, step: TSRstp);
            var Pit = Range(minPit, maxPit, step: Pitstp);

            int TSRnum = TSR.Count;
            int Pitnum = Pit.Count;
            var p = AeroL_INI.AER_INIParameterType(aerol);
            var outfile = new AeroL_IO_Out(aerol, p, "TSR", "(-)");
            var result = Matrix<double>.Build.Dense(TSRnum, Pitnum);
            //WriteUnit(directory, GetResUnit, aerol.Outputs_OutList, "TSR", "Pitch", "(-)", "(deg)");
            //WriteLine(directory);
            for (int i = 0; i < TSRnum; i++)
            {
                for (int j = 0; j < Pitnum; j++)
                {
                    //Write(directory, TSR[i]);
                    //Write(directory, Pit[j] * R2D);
                    result[i, j] = CalculateCp(TSR[i], Pit[j], ref Bld, Airfoil, HubRad, rho, Bldnum);
                    //# 计算输出
                    outfile.AER_CalcOutput(in Bld, p);
                    outfile.AER_OutWriteResult(TSR[i], i);
                    //WriteBEMEleValue();
                    //WriteLine(directory);
                    //LogHelper.WriteLog($"TSR: {TSR[i].ToString("f4")}\tPitch: {Pit[j].ToString("f4")} deg\tCp:{result[i, j].ToString("f4")}", title: "", color: ConsoleColor.Yellow);
                }
            }

            return result;
        }

        /// <remarks>
        /// 计算静态功率曲线
        /// </remarks>
        /// <param name="std_pow"></param>
        /// <param name="Bld"></param>
        /// <param name="Airfoil"></param>
        /// <param name="HubRad"></param>
        /// <param name="rho"></param>
        /// <param name="Bldnum"></param>
        /// <returns>一个矩阵<br/>
        /// 第一列是风速v[m/s]<br/>
        /// 第二列是风轮转速v[rpm/min]<br/>
        /// 第三列是叶尖速比λ[-]<br/>
        /// 第四列是变桨角θ[rad]<br/>
        /// 第五列是Cp[-]<br/>
        /// 第六列是Ct[-]<br/>
        /// 第七列是P_out[kw]<br/>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public Matrix<double> SteadyPowerCurveFunction(T_ALStp_pow std_pow, ref AER_RtHndSideType Bld, Airfoil1 Airfoil, double HubRad, double rho = 1.225, int Bldnum = 3, bool show = true)
        {
            // Extract properties from std_pow object
            double min_windv = std_pow.min_windv;
            double max_windv = std_pow.max_windv;
            double wind_step = std_pow.wind_step;
            bool ifpitch = std_pow.ifpitch;
            double Fixed_pitch = std_pow.Fixed_pitch;
            double Fixed_rotationalspeed = std_pow.Fixed_rotationalspeed;
            double orig_pit = std_pow.orig_pit;
            double opt_KW = std_pow.opt_KW;
            double opt_rpm = std_pow.opt_rpm_rad; //[rpm/min]
            double η = std_pow.η;
            double ωmin = std_pow.ωmin;
            double pitch_up = std_pow.pitch_up;
            double pitch_down = std_pow.pitch_down;

            double Rrotor = Bld.BldEle[0, 0].STipRad;
            double ω = 0.0;
            double θ = pitch_down;
            double rpm = 0.0;
            var p = AeroL_INI.AER_INIParameterType(aerol);
            int data_length = Range(start: min_windv, end: max_windv, step: wind_step).Count;
            var V_tes = Range(start: min_windv, end: max_windv, step: wind_step);
            Matrix<double> result = Matrix<double>.Build.Dense(data_length, 7);
            //WriteTitle(directory, $"OPenWECD旗下产品 OpenHast By TGTeam ! 当前文件版本为 {ProjectVision}", $"如果有相关问题请登录官方网站{url} 寻求帮助");
            //WriteTitle(directory, $"OPenhast 静态功率曲线结果输出文件");
            //WriteUnit(directory, GetResUnit, aerol.Outputs_OutList, "windSpeed", "(m/s)");
            //WriteLine(directory);
            var outfile = new AeroL_IO_Out(aerol, AeroL_INI.AER_INIParameterType(aerol), "windSpeed", "(m/s)");
            if (ifpitch)
            {
                (double Cp_max, double Tsr) = FindCpOptAndTsropt(Bld, Airfoil, HubRad, rho, Bldnum, orig_pit);
                double v = 10;
                int np = 0;

                for (int kkk = 0; kkk < data_length; kkk++)
                {
                    v = V_tes[kkk];
                    //Write(directory, v);
                    double KwMax = (0.5 * rho * Math.PI * Rrotor * Rrotor) * v * v * v / 1000.0 * η / 100;
                    double ω_opt = Tsr * v / Rrotor;
                    ω = Math.Min(Math.Max(ω_opt, ωmin * Math.PI / 30.0), opt_rpm * Math.PI / 30.0);//rad/s
                    rpm = ω * 30.0 / Math.PI;
                    double λ = ω * Rrotor / v;
                    // Assume the Calculate_Cp function is implemented in C#
                    double Cp = CalculateCp(λ, θ, ref Bld, Airfoil, HubRad, rho, Bldnum);
                    double P_out = Cp * KwMax;
                    var test = Bld;
                    if (P_out > opt_KW)
                    {

                        double fd(double x) => CalculateCp(λ, x, ref test, Airfoil, HubRad, rho, Bldnum) * KwMax - opt_KW;
                        θ = fzero(fd, θ, pitch_up, "AeroL.SteadyBEM", 1E-15, 200);
                    }

                    // Assume the Calculate_Cp_Ct function is implemented in C#
                    (Cp, double Ct) = CalculateCpCt(λ, v, θ, ref Bld, Airfoil, HubRad, rho, Bldnum);
                    //WriteBEMEleValue();
                    //WriteLine(directory);
                    P_out = Cp * KwMax;
                    if (show)
                        LogHelper.WriteLog($"WindS: {v.ToString("f4")} m/s\tRoterS: {rpm.ToString("f4")} rpm/m\tPit：{(θ * 180 / Math.PI).ToString("f4")} deg\tP_out:{P_out.ToString("f2")} Kw\tCp:{Cp.ToString("f4")}\tTSR:{λ.ToString("f2")}", title: "", color: ConsoleColor.Yellow);
                    else
                        LogHelper.WriteLogO($"WindS: {v.ToString("f4")} m/s\tRoterS: {rpm.ToString("f4")} rpm/m\tPit：{(θ * 180 / Math.PI).ToString("f4")} deg\tP_out:{P_out.ToString("f2")} Kw\tCp:{Cp.ToString("f4")}\tTSR:{λ.ToString("f2")}", title: "");

                    outfile.AER_CalcOutput(in Bld, in p);
                    outfile.AER_OutWriteResult(v, kkk);




                    result[np, 0] = v;
                    result[np, 1] = rpm;//ω*60/2/Math.PI;
                    result[np, 2] = λ;
                    result[np, 3] = θ;
                    result[np, 4] = Cp;
                    result[np, 5] = Ct;
                    result[np, 6] = P_out;
                    np++;
                }

                // Assume the SteadyPowerCurveOT function is implemented in C#
                return result;
            }
            else
            {
                LogHelper.WarnLog("您将`ifpitch`设置为了`False`,这意味着风力机是定速和定桨距角的!从而不追求TSR", FunctionName: "SteadyPowerCurveFunction");

                int np = 0;
                for (double i = min_windv; i <= max_windv; i += wind_step)
                {
                    double λ = Fixed_rotationalspeed * Rrotor / i;

                    // Assume the Calculate_Cp_Ct function is implemented in C#
                    (double Cp, double Ct) = CalculateCpCt(λ, i, Fixed_pitch, ref Bld, Airfoil, HubRad, rho, Bldnum);
                    double P_in = Cp * (0.5 * rho * Math.PI * Rrotor * Rrotor) * i * i * i / 1000;
                    double P_out = P_in * η;

                    result[np, 0] = i;
                    result[np, 1] = Fixed_rotationalspeed;
                    result[np, 2] = λ;
                    result[np, 3] = Fixed_pitch;
                    result[np, 4] = Cp;
                    result[np, 5] = Ct;
                    result[np, 6] = P_out;
                    np++;
                }

                return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double Cp, double Ct) CalculateCpCt(double TSR, double windspeed, double Pit, ref AER_RtHndSideType Bld, Airfoil1 Airfoil, double HubRad, double rho = 1.225, int Bldnum = 3)
        {
            (double Cp, double Ct) = BEM(ref Bld, Airfoil, TSR, windspeed, HubRad, Pit, rho, Bldnum);
            return (Cp, Ct); // 返回 Cp 值
        }
        /// <remarks>
        /// 计算最大的cp系数及其对应的叶尖速比和cP曲线
        /// </remarks>
        /// <param name="Bld"></param>
        /// <param name="Airfoil"></param>
        /// <param name="HubRad"></param>
        /// <param name="rho"></param>
        /// <param name="Bldnum"></param>
        /// <param name="pit"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double CpMax, double Tsr) FindCpOptAndTsropt(AER_RtHndSideType Bld, Airfoil1 Airfoil, double HubRad, double rho = 1.225, int Bldnum = 3, double pit = 0.0)
        {
            double fd(double x) => (CalculateCp(x + 0.0000000001, pit, ref Bld, Airfoil, HubRad, rho, Bldnum) - CalculateCp(x, pit, ref Bld, Airfoil, HubRad, rho, Bldnum)) / 0.0000000001;
            double Tsr = fzero(fd, 0.5, 30, "AeroL.SteadyBEM", 1E-15);
            double cp = CalculateCp(Tsr, pit, ref Bld, Airfoil, HubRad, rho, Bldnum);
            return (cp, Tsr);

        }


        /// <remarks>
        /// (a, b, phi, AOA, cn, Ct, n) 静态BEM 求解函数
        /// </remarks>
        /// <param name="lambda">叶片的叶尖速比[rad]</param>
        /// <param name="theta">当前截面的扭角[rad]</param>
        /// <param name="foilNo">当前截面的翼型编号[.]</param>
        /// <param name="airfoils">翼型的一个结构体</param>
        /// <param name="tipRad">叶片长度,是包含轮毂半径的[m]</param>
        /// <param name="hubRad">轮毂半径[m]</param>
        /// <param name="blSpn">当前截面的位置,包含轮毂半径[m]</param>
        /// <param name="solid">当前叶片截面的实度[m^2]</param>
        /// <param name="aStart">初始的轴向系数,一般设置为相邻截面的轴向系数作为初始值[.]</param>
        /// <param name="bStart">初始的切向系数,一般设置为相邻截面的切向系数作为初始值[.]</param>
        /// <param name="bldnum">叶片数量[.]</param>
        /// <returns>(a1, b1, φ, AOA, cn, Ct, n)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double a1, double b1, double φ, double AOA, double cn, double Ct, int n, double cl, double cd, double cm) CalcOutputS(double lambda, double theta, int foilNo, Airfoil1 airfoils, double tipRad, double hubRad, double blSpn, double solid, double aStart = 0.3, double bStart = 0.05, int bldnum = 3)
        {
            int n = 0;
            double v = 10.0;
            double omega = lambda * v / tipRad;

            double Vx = 10;
            double Vy = omega * blSpn;
            double phi = Math.Atan2(Vx, Vy);
            double a, b, AOA;// cn, Ct;
            double Residue = CallStateResidual(phi, theta, Vx, Vy, foilNo, airfoils, tipRad, hubRad, blSpn, solid, 0, 0);
            double Cx, Cy, Cl, Cd, Cm;
            if (Vx == 0 || Vy == 0)
            {
                phi = Math.Atan2(Vx, Vy);
                (a, b, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi, theta, foilNo, airfoils, tipRad, hubRad, blSpn, solid, 0, 0);
            }
            else if (Math.Abs(Residue) < 1e-3)
            {
                (a, b, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi, theta, foilNo, airfoils, tipRad, hubRad, blSpn, solid, 0, 0);
            }
            else
            {
                phi = UpdateInflowAngle(theta, Vx, Vy, foilNo, airfoils, tipRad, hubRad, blSpn, solid, 0, 0);
                (a, b, Cx, Cy, Cl, Cd, Cm) = CalcOutput(phi, theta, foilNo, airfoils, tipRad, hubRad, blSpn, solid, 0, 0);
            }

            AOA = phi - theta;
            return (a, b, phi, AOA, Cx, Cy, n, Cl, Cd, Cm);

        }

        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="phi"></param>
        /// <param name="theta"></param>
        /// <param name="FoilNo"></param>
        /// <param name="Airfoils"></param>
        /// <param name="TipRad"></param>
        /// <param name="HubRad"></param>
        /// <param name="BlSpn"></param>
        /// <param name="Solid"></param>
        /// <param name="Azimuth"></param>
        /// <param name="chi0"></param>
        /// <returns>a, aa, Cx, Cy, Cl, Cd</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double a, double aa, double Cx, double Cy, double Cl, double Cd, double Cm) CalcOutput(double phi, double theta, int FoilNo, Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth, double chi0)
        {
            double AOA = phi - theta;
            (double Cl, double Cd, double Cm) = LiftDragCoeffInterp(AOA, FoilNo, Airfoils);
            double Cx = Cl * Math.Cos(phi) + Cd * Math.Sin(phi);
            double Cy = Cl * Math.Sin(phi) - Cd * Math.Cos(phi);
            //(double Cx, double Cy) = ForceCoefficents(AOA, phi, FoilNo, Airfoils);
            double F = Hub_tip_loss(phi, TipRad, HubRad, BlSpn, Bldnum);

            double cosphi = Math.Cos(phi);
            double sinphi = Math.Sin(phi);

            // Non dimensional parameters
            double k;
            if (sinphi == 0.0)
            {
                k = double.MaxValue;
            }
            else
            {
                k = Solid * Cx / (4 * F * sinphi * sinphi);
            }

            // Different equation depending on solution region
            double a;
            if (phi > 0.0)
            {
                if (k <= 2.0 / 3.0) // momentum region
                {
                    if (k == -1.0)
                    {
                        k = k - 0.1;
                    }
                    a = k / (1.0 + k);
                    a = Math.Max(a, -10.0); // Patch
                }
                else // emperical region
                {
                    double g1 = 2.0 * F * k - (10.0 / 9.0 - F);
                    double g2 = 2.0 * F * k - F * (4.0 / 3.0 - F);
                    double g3 = 2.0 * F * k - (25.0 / 9.0 - 2.0 * F);
                    if (Math.Abs(g3) < 1e-6)
                    {
                        a = 1 - 1 / (2 * Math.Sqrt(g2));
                    }
                    else
                    {
                        a = (g1 - Math.Sqrt(g2)) / g3;
                    }
                }
            }
            else if (phi < 0.0)
            {
                if (k > 1) // propeller brake region
                {
                    a = k / (k - 1.0);
                    a = Math.Min(a, 10); // Patch
                }
                else
                {
                    a = 0.0;
                }
            }
            else
            {
                return (0.0, 0.0, Cx, Cy, Cl, Cd, Cm);
            }

            // Pitt and Peters yaw correction model
            double x = (0.6 * a + 1) * chi0;
            a = a * (1.0 + 15 * Math.PI / 64 * Math.Tan(x / 2.0) * BlSpn / TipRad * Math.Sin(Azimuth));

            // Tangential induction factor
            double kk;
            if (sinphi * cosphi == 0)
            {
                kk = double.MaxValue;
            }
            else
            {
                kk = Solid * Cy / (4 * F * sinphi * cosphi);
            }

            double aa;
            if (kk == 1.0)
            {
                a = 0.0;
                aa = 0.0;
            }
            else
            {
                aa = kk / (1 - kk);
                if (Math.Abs(aa) > 10.0)
                {
                    aa = 10 * Math.Sign(aa); // Patch
                }
            }

            return (a, aa, Cx, Cy, Cl, Cd, Cm);
        }

        /// <remarks>
        /// 计算当前迭代的解和残差
        /// </remarks>
        /// <param name="phi">入流角</param>
        /// <param name="theta">扭角</param>
        /// <param name="Vx">x方向上的入流风速度</param>
        /// <param name="Vy">y方向上的入流风速度</param>
        /// <param name="FoilNo">当前截面的易信编号</param>
        /// <param name="Airfoils">Airfoil1结构体，摘IO.Type当中定义</param>
        /// <param name="TipRad">叶片长度,是包含轮毂半径的[m]</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="BlSpn">展向位置</param>
        /// <param name="Solid">当前截面的实度</param>
        /// <param name="Azimuth">当前的偏航角</param>
        /// <param name="chi0">默认为0，偏航修正参数</param>
        /// <returns>一个残差，用来判断当前是否收敛</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private double CallStateResidual(double phi, double theta, double Vx, double Vy, int FoilNo,
             Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth,
             double chi0)
        {
            double AOA = phi - theta;
            (double Cx, double Cy) = ForceCoefficents(AOA, phi, FoilNo, Airfoils);
            double F = Hub_tip_loss(phi, TipRad, HubRad, BlSpn, Bldnum);
            double cosphi = Math.Cos(phi);
            double sinphi = Math.Sin(phi);

            // Non-dimensional parameters
            double k;
            if (sinphi == 0.0)
                k = double.MaxValue;
            else
                k = Solid * Cx / (4.0 * F * Math.Pow(sinphi, 2));

            double a = 0.0;
            // Different equation depending on solution region
            if (phi > 0)
            {
                if (k <= 2.0 / 3.0) // momentum region
                {
                    if (k == -1.0)
                        k = k - 0.1;

                    a = k / (1.0 + k);
                    a = Math.Max(a, -10.0); // Patch
                }
                else // empirical region
                {
                    double g1 = 2.0 * F * k - (10.0 / 9.0 - F);
                    double g2 = 2.0 * F * k - F * (4.0 / 3.0 - F);
                    double g3 = 2.0 * F * k - (25.0 / 9.0 - 2.0 * F);

                    if (Math.Abs(g3) < 1e-6)
                        a = 1 - 1 / (2 * Math.Sqrt(g2));
                    else
                        a = (g1 - Math.Sqrt(g2)) / g3;
                }
            }
            else if (phi < 0)
            {
                if (k > 1) // propeller brake region
                {
                    a = k / (k - 1.0);
                    a = Math.Min(a, 10.0); // Patch
                }
                else
                {
                    a = 0.0;
                }
            }
            else if (phi == 0)
            {
                //a = 0;
                //aa = 0;
                //Residue = sinphi / (1 - a) - Vx / Vy * cosphi / (1 + aa);
                return sinphi - Vx / Vy * cosphi;
            }

            // Pitt and Peters yaw correction model
            double x = (0.6 * a + 1.0) * chi0;
            a = a * (1.0 + 15.0 * Math.PI / 64.0 * Math.Tan(x / 2.0) * BlSpn / TipRad * Math.Sin(Azimuth));

            // Tangential induction factor
            double kk;

            if (sinphi * cosphi == 0)
                kk = double.MaxValue;
            else
                kk = Solid * Cy / (4.0 * F * sinphi * cosphi);

            double aa;

            if (kk == 1.0)
            {
                a = 0.0;
                aa = 0.0;
            }
            else
            {
                aa = kk / (1.0 - kk);

                if (Math.Abs(aa) > 10.0)
                    aa = 10.0 * Math.Sign(aa); // Patch
            }

            // Residue
            double Residue;

            if (a == 1.0)
                Residue = -Vx / Vy * cosphi / (1.0 + aa);
            else
                Residue = sinphi / (1.0 - a) - Vx / Vy * cosphi / (1.0 + aa);

            return Residue;
        }

        /// <remarks>
        /// 该函数用来计算损失修正系数F
        /// </remarks>
        /// <param name="phi">入流角</param>
        /// <param name="TipRad">叶片长度,是包含轮毂半径的</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="BlSpn">叶片的展向位置</param>
        /// <param name="Bldnum">叶片数，默认为3</param>
        /// <returns>F: 叶尖和轮毂损失F=Fhub*Ftip</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private double Hub_tip_loss(double phi, double TipRad, double HubRad, double BlSpn, int Bldnum)
        {
            double abssinphi = Math.Abs(Math.Sin(phi));
            double ftip = Bldnum / 2.0 * (TipRad - BlSpn) / (BlSpn * abssinphi);
            double Ftip = 2.0 / Math.PI * Math.Acos(Math.Min(1, Math.Exp(-ftip)));

            double fhub = Bldnum / 2.0 * (BlSpn - HubRad) / (HubRad * abssinphi);
            double Fhub = 2.0 / Math.PI * Math.Acos(Math.Min(1, Math.Exp(-fhub)));
            double F = Ftip * Fhub;
            return Math.Max(F, 0.0001);
        }


        /// <remarks>
        /// 计算当前攻角下的气动力系数，是经过转换的
        /// </remarks>
        /// <param name="AOA">叶片的攻角</param>
        /// <param name="phi">叶片的入流角</param>
        /// <param name="FoilNo">叶片的翼型编号</param>
        /// <param name="Airfoils">Aiffoil1结构体</param>
        /// <returns>Cx升力系数，Cy阻力系数</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double Cx, double Cy) ForceCoefficents(double AOA, double phi, int FoilNo, Airfoil1 Airfoils)
        {
            double Cl, Cd;

            (Cl, Cd, _) = LiftDragCoeffInterp(AOA, FoilNo, Airfoils);

            double Cx = Cl * Math.Cos(phi) + Cd * Math.Sin(phi);
            double Cy = Cl * Math.Sin(phi) - Cd * Math.Cos(phi);

            return (Cx, Cy);
        }


        /// <remarks>
        /// 升力系数和阻力系数插值函数
        /// </remarks>
        /// <param name="AOA">攻角</param>
        /// <param name="FoilNo">翼型编号</param>
        /// <param name="Airfoils">一个Airfoils结构体</param>
        /// <returns></returns>
        /// 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private (double cl, double cd, double cm) LiftDragCoeffInterp(double AOA, int FoilNo, Airfoil1 Airfoils)
        {
            double Cl = 0.0;
            double Cd = 0.0;
            double Cm = 0.0;

            Cl = Airfoils.IterCl[FoilNo].Interpolate(AOA); //Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(1), AOA);
            Cd = Airfoils.IterCd[FoilNo].Interpolate(AOA); //Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(2), AOA);
            Cm = Airfoils.IterCm[FoilNo].Interpolate(AOA); //Inter1D(Airfoils[FoilNo].DataSet.Column(0), Airfoils[FoilNo].DataSet.Column(3), AOA);

            return (Cl, Cd, Cm);
        }

        ///// <remarks>
        ///// 存储入流角，亟已经计算过的入流角，以加快计算速度
        ///// </remarks>
        //private double phiStore = -100;
        /// <remarks>
        /// 更新入流角，如果上面的CallResidual函数表明，当前计算没有收敛这更新入流角来迭代求解，
        /// </remarks>
        /// <param name="theta">扭角</param>
        /// <param name="Vx">x方向上的入流风速度</param>
        /// <param name="Vy">y方向上的入流风速度</param>
        /// <param name="FoilNo">当前截面的易信编号</param>
        /// <param name="Airfoils">Airfoil1结构体，摘IO.Type当中定义</param>
        /// <param name="TipRad">叶片长度,是包含轮毂半径的[m]</param>
        /// <param name="HubRad">轮毂半径</param>
        /// <param name="BlSpn">展向位置</param>
        /// <param name="Solid">当前截面的实度</param>
        /// <param name="Azimuth">当前的偏航角</param>
        /// <param name="chi0">默认为0，偏航修正参数</param>
        /// <returns>更新后的入流角</returns>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        private double UpdateInflowAngle(double theta, double Vx, double Vy, int FoilNo, Airfoil1 Airfoils, double TipRad, double HubRad, double BlSpn, double Solid, double Azimuth, double chi0)
        {
            double ep = 1e-6;
            double Res(double x) => CallStateResidual(x, theta, Vx, Vy, FoilNo, Airfoils, TipRad, HubRad, BlSpn, Solid, Azimuth, chi0);

            double x0, x1, phi;
            if (Res(PiBy2) * Res(ep) < 0)
            {
                x0 = ep;
                x1 = PiBy2;
                phi = fzero(Res, x0, x1, "AeroL.SteadyBEM", 1E-15, 200);
                //phi = fzero(Res, x0, x1, 1E-3);
            }
            else if (Res(-Math.PI / 4.0) * Res(-ep) < 0)
            {
                x0 = -Math.PI / 4.0;
                x1 = -ep;
                phi = fzero(Res, x0, x1, "AeroL.SteadyBEM", 1E-15, 200);
            }
            else
            {
                x0 = PiBy2;
                x1 = Math.PI - ep;
                phi = fzero(Res, x0, x1, "AeroL.SteadyBEM", 1E-15, 200);
            }

            return phi;
        }

        #endregion 计算方法实现区域
    }
}
