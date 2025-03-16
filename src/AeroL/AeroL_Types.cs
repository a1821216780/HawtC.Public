

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

#define MBDLATFLOAT
//#define MBDLATDOUBLE
//#define BDLATMATHNET


using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.IO;
using OpenWECD.MBD;
using OpenWECD.WindL;
using SourceGeneration.Reflection;
using System.Runtime.InteropServices;
#if MBDLATDOUBLE

using Mat = OpenWECD.IO.Numerics.Matrix3S;
using Vec = OpenWECD.IO.Numerics.Vector3S;
using NUMT = System.Double;
using static System.Math;
using matrix =MathNet.Numerics.LinearAlgebra.Matrix<double>;
using vector = MathNet.Numerics.LinearAlgebra.Vector<double>;

#elif MBDLATFLOAT

using Mat = OpenWECD.IO.Numerics.Matrix3f;
using Vec = System.Numerics.Vector3;
using NUMT = System.Single;
using static System.MathF;
using matrix =MathNet.Numerics.LinearAlgebra.Matrix<float>;
using vector = MathNet.Numerics.LinearAlgebra.Vector<float>;

#elif MBDLATMATHNET

using Mat=MathNet.Numerics.LinearAlgebra.Matrix<double>;
using Vec= MathNet.Numerics.LinearAlgebra.Vector<double>;
using NUMT = System.Double;

#endif

namespace OpenWECD.AeroL
{
    /// <summary>
    /// 静态功率曲线结构体
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct T_ALStp_pow
    {
        /// <summary>
        /// 最小风速[m/s]
        /// </summary>
        public double min_windv; // =3.0

        /// <summary>
        /// 最大风速[m/s]
        /// </summary>
        public double max_windv; // =25.0

        /// <summary>
        /// 风速间隔 [m/s]
        /// </summary>
        public double wind_step; // =0.1

        /// <summary>
        /// 额度桨距角[rad]
        /// </summary>
        public double orig_pit; // =0.0

        /// <summary>
        /// 切入转速[rpm/min]
        /// </summary>
        public double ωmin; // =1.0

        /// <summary>
        /// 额定功率[kw]
        /// </summary>
        public double opt_KW; // =0.0

        /// <summary>
        /// 额定转速[rpm/min]
        /// </summary>
        public double opt_rpm_rad; // =0.0

        /// <summary>
        /// 发电机效率%
        /// </summary>
        public double η; // =1.0

        /// <summary>
        /// 最大桨距角[rad]
        /// </summary>
        public double pitch_up; // =pi/2

        /// <summary>
        /// 最小桨距角[rad]
        /// </summary>
        public double pitch_down; // =0.0

        /// <summary>
        /// 是否变桨
        /// </summary>
        public bool ifpitch; // =true

        /// <summary>
        /// 
        /// </summary>
        public double Fixed_pitch; // =0.0

        /// <summary>
        /// 
        /// </summary>
        public double Fixed_rotationalspeed; // =0.0

        /// <remarks>
        /// 计算叶片静态功率曲线的
        /// </remarks>
        /// <param name="min_windv">最小风速[m/s]</param>
        /// <param name="max_windv">最大风速[m/s]</param>
        /// <param name="wind_step">间隔 -</param>
        /// <param name="orig_pit">额度桨距角[rad]【</param>
        /// <param name="ωmin">切入转速[rpm/min]</param>
        /// <param name="opt_KW">额定功率[kw]</param>
        /// <param name="opt_rpm_rad">>额定转速[rpm/min]</param>
        /// <param name="η">发电机效率%</param>
        /// <param name="pitch_up">最大桨距角[rad]</param>
        /// <param name="pitch_down">最小桨距角[rad]</param>
        /// <param name="ifpitch">是否变桨</param>
        /// <param name="Fixed_pitch"></param>
        /// <param name="Fixed_rotationalspeed"></param>
        public T_ALStp_pow(double min_windv = 3.0, double max_windv = 25.0, double wind_step = 0.1, double orig_pit = 0.0, double ωmin = 1.0, double opt_KW = 0.0, double opt_rpm_rad = 0.0, double η = 1.0, double pitch_up = PhysicalParameters.PiBy2, double pitch_down = 0.0, bool ifpitch = true, double Fixed_pitch = 0.0, double Fixed_rotationalspeed = 0.0)
        {
            this.min_windv = min_windv;
            this.max_windv = max_windv;
            this.wind_step = wind_step;
            this.orig_pit = orig_pit;
            this.ωmin = ωmin;
            this.opt_KW = opt_KW;
            this.opt_rpm_rad = opt_rpm_rad;
            this.η = η;
            this.pitch_up = pitch_up;
            this.pitch_down = pitch_down;
            this.ifpitch = ifpitch;
            this.Fixed_pitch = Fixed_pitch;
            this.Fixed_rotationalspeed = Fixed_rotationalspeed;
        }
    }

    /// <summary>
    /// 动态失速模型结构体
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct T_ALDynamicStallPar
    {
        /// <summary>
        /// 当前仿真时间,Elastic 设置
        /// </summary>
        public double t;
        /// <summary>
        /// 仿真时间间隔,Elastic 设置
        /// </summary>
        public double dt;

        public int I_t;

        public double[] Tspan;
        /// <summary>
        /// 当前的风力机是否使用动态失速模型；AeroL_Pro(string path)当中设置
        /// </summary>
        public bool UseDynamicStallPar;

        /// <summary>
        /// 当前要计算动态失速的界面位置编号，UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public int Numsec;

        /// <summary>
        /// 当前要计算动态失速的叶片编号，UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public int Numb;

        /// <summary>
        /// 当前截面的风速,UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public double Urel;

        /// <summary>
        /// 当前截面的弦长,UnSteadyBEMT.BEMTMex实时更新
        /// </summary>
        public double Cord;
    }


    ///// <summary>
    ///// 动态失速模型接口
    ///// </summary>
    //public interface I_ALDynamicStall
    //{
    //    public (double Cl, double Cd, double Cm) AL_DynamicStall(double phi, double theta, int FoilNo, ref T_ALDynamicStallPar par);

    //    public (double Cl, double Cd, double Cm) AL_CalBladeDynamicStall(double phi, double theta, int FoilNo);
    //    public void AirfoilSolve();
    //}
    /// <summary>
    /// 要使用动态失速模型，必须实现这个接口
    /// </summary>
    public interface I_ALCalDynamicStall
    {
        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum, double AOA, ref T_ALAeroBladeElement rths, T_ALAeroBladeElement srths);
        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum, double AOA);

        public Airfoil1 GetAirfoil1();
    }

    public interface I_ALCal3DRotationalEffect
    {
        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum, double AOA, ref T_ALAeroBladeElement rths);

    }
    /// <summary>
    /// 要计算非定常空气动力学力要继承这个接口。
    /// </summary>
    public interface CallDynamicAeroLoad
    {
        /// <summary>
        /// 计算非定常空气动力
        /// </summary>
        /// <param name="t">当前仿真时间[s]</param>
        /// <param name="dt">空气动力学仿真时间步长[s]</param>
        /// <param name="u">将计算的外力赋值到这个指针上 ref u </param>
        /// <param name="q_aim">当前标准IEC右手坐标系下的风轮方位角[rad]</param>
        /// <param name="rotorSpeeed">当前风轮的转数[rad/s]</param>
        /// <param name="OtherState"></param>
        /// <param name="RtHSdat"></param>
        /// <param name="cooeSys"></param>
        /// <param name="WND_rths"></param>
        /// <param name="Pitch"></param>
        /// <param name="p"></param>
        /// <param name="mp"></param>
        /// <param name="rths"></param>
        public void AL_CalDynamicAeroLoad(double t, double dt, ref MBD_InputLoad u, double q_aim, double rotorSpeeed, ref MBD_OtherStateType OtherState, in MBD_RtHndSideType RtHSdat, in MBD_CoordSysType cooeSys, in WND_RtHndSideType WND_rths, in Vector<double> Pitch, in AER_ParameterType p, in MBD_ParameterType mp, ref AER_RtHndSideType rths,in T_ALAeroBladeElement[,] rthslast);

        //public void AL_CalTowerDynamicAeroLoad(ref MeshType[] BladePtLoads, double[,,] VMB, double[,,] UMB, double[,] FlexBlSpn);
    }

    

    /// <summary>
    /// 叶片节点单元气动结构体。
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct T_ALAeroBladeElement
    {

        // BEMT理论下定义的叶片节点单元
        /// <summary>
        /// 叶片单元节点的弦长[m]
        /// </summary>
        public double SChord;

        /// <summary>
        /// 叶片单元的气动扭角[rad]
        /// </summary>
        public double STwist;

        /// <summary>
        /// 当前节点的翼型编号[-]
        /// </summary>
        public int SFoilNum;

        /// <summary>
        /// 叶片当前节点的厚度[%],当前的模型版本不需要
        /// </summary>
        public double Sthickness;

        /// <summary>
        /// 当前叶片节点的展向位置(包括了轮毂半径)[m]
        /// </summary>
        public double SBlspan;

        /// <summary>
        /// 当前叶片节点的轮毂半径[m]
        /// </summary>
        public double SRHub;

        /// <summary>
        /// 叶片的叶片顶端到轴中心的距离[m]
        /// </summary>
        public double STipRad;

        /// <summary>
        /// 当前叶片节点的实度。是一个定值。[-]没有除以展向位置
        /// </summary>
        public double SSolid;

        /// <summary>
        /// 当前的变桨轴向的位置[m]
        /// </summary>
        public double SPitchAxis;

        /// <summary>
        /// 当前的气动力矩中心J1
        /// </summary>
        public double SAeroCent;

        ///// <summary>
        ///// 当前的气动力矩中心J2
        ///// </summary>
        //public double SAeroCentJ2;

        /// <summary>
        /// 当地叶尖速比
        /// </summary>
        public double DTsr;
        /// <summary>
        ///  0.5 * p.AirDens * rths.BldEle[J, K].SChord * absVrel * absVrel;
        /// </summary>
        public double DConst;
        /// <summary>
        /// 叶片当前当前节点的攻角[rad]
        /// </summary>
        public double DAOA;

        /// <summary>
        /// 当前叶片节点的入流角[rad]
        /// </summary>
        public double Dphi;

        /// <summary>
        /// 当前叶片节点由于结构变形而产生的扭转角。[rad]
        /// </summary>
        public double DStrTwist;


        /// <summary>
        /// 当前叶片节点由于变形而产生的展向位移的改变[m]
        /// </summary>
        public double DFlexSpan;

        /// <summary>
        /// 当前叶片节点的方位角，相同叶片上的节点方位角相同。[rad]
        /// </summary>
        public double DAzimuth;

        /// <summary>
        /// 当前节点的x向当地水平风速[m/s],不是诱导风速，m 坐标系下
        /// </summary>
        public NUMT DVx;
        /// <summary>
        /// 当前节点的y向切向风速[m/s],不是诱导风速，m 坐标系下
        /// </summary>
        public NUMT DVy;

        /// <summary>
        /// 当前节点的z向展向风速[m/s],不是诱导风速，m 坐标系下
        /// </summary>
        public NUMT DVz;



        /// <summary>
        /// 由（DVx,DVy,DVz）组成的向量
        /// </summary>
        public Vec Dv;

        ///// <summary>
        ///// 外部输入的风速,m坐标系下
        ///// </summary>
        //public float DEmVx;

        ///// <summary>
        ///// 外部输入的风速,m坐标系下
        ///// </summary>
        //public float DEmVy;

        ///// <summary>
        ///// 外部输入的风速,m坐标系下
        ///// </summary>
        //public float DEmVz;
        ///// <summary>
        /////  外部输入的风速,m坐标系下,由（DEnVx，DEnVy,DEnVz）组成
        ///// </summary>
        //public Vector3 DEmV;

        /// <summary>
        /// 外部输入的风速,n坐标系下
        /// </summary>
        public NUMT DEnVx;

        /// <summary>
        /// 外部输入的风速,n坐标系下
        /// </summary>
        public NUMT DEnVy;

        /// <summary>
        /// 外部输入的风速,m坐标系下
        /// </summary>
        public NUMT DEnVz;
        /// <summary>
        ///  外部输入的风速,m坐标系下,由（DEnVx，DEnVy,DEnVz）组成
        /// </summary>
        public Vec DEnV;

        /// <summary>
        /// 由于结构运动引起的相对风速,m坐标系下
        /// </summary>
        public NUMT DStrVx;
        /// <summary>
        /// 由于结构运动引起的相对风速,m坐标系下
        /// </summary>
        public NUMT DStrVy;
        /// <summary>
        /// 由于结构运动引起的相对风速,m坐标系下
        /// </summary>
        public NUMT DStrVz;

        /// <summary>
        /// 由于结构运动而产生的相对风速x,z坐标系下
        /// </summary>
        public NUMT DZStrVx;
        /// <summary>
        /// 由于结构运动而产生的相对风速y,z坐标系下
        /// </summary>
        public NUMT DZStrVy;
        /// <summary>
        /// 由于结构运动而产生的相对风速z,z坐标系下
        /// </summary>
        public NUMT DZStrVz;




        /// <summary>
        /// Axial induced wind velocity at each node,轴向输入风速，诱导的风速
        /// </summary>
        public double Vindx;

        /// <summary>
        /// Tangential induced wind velocity at each node，切向输入风速，诱导的风速
        /// </summary>
        public double Vindy;


        /// <summary>
        ///  当前节点的入流相对风速[m/s]
        /// </summary>
        public NUMT DVrel;

        /// <summary>
        ///  当前节点的入流相对风速[m/s]
        /// </summary>
        public Vec DVecVrel;
        /// <summary>
        /// 当前节点的实际风速入流，不存在诱导[m/s]
        /// </summary>
        public double DU;

        /// <summary>
        /// 轴向诱导因子[-]
        /// </summary>
        public double Da;

        /// <summary>
        /// 切向诱导因子[-]
        /// </summary>
        public double Daa;

        /// <summary>
        /// 当前节点的插值升力系数[-]
        /// </summary>
        public double DCl;

        /// <summary>
        /// 当前节点的插值阻力系数[-]
        /// </summary>
        public double DCd;

        /// <summary>
        /// 当前节点的插值倾覆力矩系数[-]
        /// </summary>
        public double DCm;
        /// <summary>
        /// 轴向力系数Cx[-]
        /// </summary>
        public double DCx;

        /// <summary>
        /// 切向力系数Cy[-]
        /// </summary>
        public double DCy;

        /// <summary>
        /// 当前节点的变桨角[rad]
        /// </summary>
        public double DPitch;

        /// <summary>
        /// 轴向气动力[N]
        /// </summary>
        public double DPx;

        /// <summary>
        /// 切向气动力[N]
        /// </summary>
        public double DPy;
        /// <summary>
        /// 展向气动力[N]
        /// </summary>
        public double DPz;


        /// <summary>
        /// MBD 定义坐标系下的气动力矢量，由DPx，DPy，DPz组成
        /// </summary>
        public Vec DP;

        public double DFl;


        public double DFd;

        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMx;
        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMy;
        /// <summary>
        /// 变桨气动力矩[Nm]
        /// </summary>
        public double DMz;
        /// <summary>
        /// MBD 定义坐标系下的气动弯矩矢量，由DMx，DMy，DMz组成
        /// </summary>
        public Vec DM;


        /// <summary>
        /// Pitch+Twist angle at each node
        /// </summary>
        public double DTheta;
        /// <summary>
        /// 单个叶片气动推力[N]
        /// </summary>
        public double Thrust;
        /// <summary>
        /// 单个叶片的气动扭矩[Nm]
        /// </summary>
        public double Torque;

        /// <summary>
        /// 动态实度
        /// </summary>
        public double DSolid;




        //public Otherhelper.FixedQueue<double> historyCm;
        #region 非定常空气动力学的参数
        public Vec UnsteadyBEM_W;
        public Vec UnsteadyBEM_Wint;

        public Vec UnsteadyBEM_Wqs;
        //public Vector3 UnsteadyBEM_V4;
        #endregion 非定常空气动力学的参数


        #region 动态失速模型要求的参数
        ///// <summary>
        ///// 历史上的Cl
        ///// </summary>
        //public double historyCl;
        ////public Otherhelper.FixedQueue<double> historyCl;

        ///// <summary>
        ///// 历史上的Cd
        ///// </summary>
        //public double historyCd;
        ////public Otherhelper.FixedQueue<double> historyCd;

        ///// <summary>
        ///// 历史上的Cm
        ///// </summary>
        //public double historyCm;

        public double fs;
        ///// <summary>
        ///// 之前的时间编号
        ///// </summary>
        //public bool cal;

        #endregion 动态失速模型要求的参数

    }

    /// <summary>
    /// 塔架节点单元气动结构体。
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct T_ALAeroTowerElement
    {
        /// <summary>
        /// 塔架单元节点的直径[m]
        /// </summary>
        public double SChord;

        /// <summary>
        /// 塔架单元的高度[m]，没有叠加基础高度
        /// </summary>
        public double SHeight;

        /// <summary>
        /// 塔架节点的气动力系数[-]
        /// </summary>
        public double SCd;
        /// <summary>
        /// 轴向气动力[N]
        /// </summary>
        public double DPx;

        /// <summary>
        /// 切向气动力[N]
        /// </summary>
        public double DPy;
        /// <summary>
        /// 展向气动力[N]
        /// </summary>
        public double DPz;
        /// <summary>
        /// MBD 定义坐标系下的气动力矢量，由DPx，DPy，DPz组成
        /// </summary>
        public Vec DP;


        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMx;
        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMy;
        /// <summary>
        /// 变桨气动力矩[Nm]
        /// </summary>
        public double DMz;

        /// <summary>
        /// 气动力
        /// </summary>
        public Vec DF;
        /// <summary>
        /// 气动扭矩
        /// </summary>
        public Vec DM;

        /// <summary>
        /// 由于结构运动而产生的相对风速x,m坐标系下
        /// </summary>
        public NUMT DStrVx;
        /// <summary>
        /// 由于结构运动而产生的相对风速y,m坐标系下
        /// </summary>
        public NUMT DStrVy;
        /// <summary>
        /// 由于结构运动而产生的相对风速z,m坐标系下
        /// </summary>
        public NUMT DStrVz;

        /// <summary>
        /// 由于结构运动而产生的相对风速x,z坐标系下
        /// </summary>
        public float DZStrVx;
        /// <summary>
        /// 由于结构运动而产生的相对风速y,z坐标系下
        /// </summary>
        public float DZStrVy;
        /// <summary>
        /// 由于结构运动而产生的相对风速z,z坐标系下
        /// </summary>
        public float DZStrVz;

        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVx;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVy;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVz;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVx;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVy;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVz;
        /// <summary>
        /// 合风速
        /// </summary>
        public NUMT DVrel;
    }


    /// <summary>
    /// 机舱节点单元气动结构体。
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct T_ALAeroNacalElement
    {
        ///// <summary>
        ///// 塔架单元节点的直径[m]
        ///// </summary>
        //public double SChord;

        ///// <summary>
        ///// 塔架单元的高度[m]，没有叠加基础高度
        ///// </summary>
        //public double SHeight;

        ///// <summary>
        ///// 塔架节点的气动力系数[-]
        ///// </summary>
        //public double SCd;
        /// <summary>
        /// 轴向气动力[N]
        /// </summary>
        public double DPx;

        /// <summary>
        /// 切向气动力[N]
        /// </summary>
        public double DPy;
        /// <summary>
        /// 展向气动力[N]
        /// </summary>
        public double DPz;
        /// <summary>
        /// MBD 定义坐标系下的气动力矢量，由DPx，DPy，DPz组成
        /// </summary>
        public Vec DP;


        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMx;
        /// <summary>
        /// 气动扭矩[Nm]
        /// </summary>
        public double DMy;
        /// <summary>
        /// 变桨气动力矩[Nm]
        /// </summary>
        public double DMz;

        /// <summary>
        /// 气动力
        /// </summary>
        public Vec DF;
        /// <summary>
        /// 气动扭矩
        /// </summary>
        public Vec DM;

        /// <summary>
        /// 由于结构运动而产生的相对风速x,m坐标系下
        /// </summary>
        public NUMT DStrVx;
        /// <summary>
        /// 由于结构运动而产生的相对风速y,m坐标系下
        /// </summary>
        public NUMT DStrVy;
        /// <summary>
        /// 由于结构运动而产生的相对风速z,m坐标系下
        /// </summary>
        public NUMT DStrVz;

        /// <summary>
        /// 由于结构运动而产生的相对风速x,z坐标系下
        /// </summary>
        public NUMT DZStrVx;
        /// <summary>
        /// 由于结构运动而产生的相对风速y,z坐标系下
        /// </summary>
        public NUMT DZStrVy;
        /// <summary>
        /// 由于结构运动而产生的相对风速z,z坐标系下
        /// </summary>
        public NUMT DZStrVz;

        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVx;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVy;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DEnVz;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVx;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVy;
        /// <summary>
        /// 外部风速向结构运动方向上的投影
        /// </summary>
        public NUMT DVz;
        /// <summary>
        /// 合风速
        /// </summary>
        public NUMT DVrel;
    }

    /// <remarks>
    /// 所有的AeroL当中的参数
    /// </remarks>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct AeroL1
    {
        #region 文件生成结构体

        /// <remarks>
        /// AeoL主文件的文件路径
        /// </remarks>
        public string AeroLfilepath;

        /// <summary>
        /// AeroL 主文件里面的字符串内容
        /// </summary>
        public string[] AerolData;

        #endregion 文件生成结构体

        
        #region 计算选项
        /// <remarks>
        /// 计算选项{0=外部调用将读取塔架叶片等模型参数，1=计算叶片Cp,2=计算叶片的功率，推力等变桨曲线表}
        /// </remarks>
        public int ApOfMb;
        #endregion 计算选项


        #region  叶片信息  [used only when ApOfMb=1 and 2]
        /// <remarks>
        /// - 轮毂半径
        /// </remarks>
        public double HubRad;
        /// <remarks>
        ///  - 叶片数量
        /// </remarks>
        public int Bldnum;

        #endregion  叶片信息  [used only when ApOfMb=1 and 2]
        

        #region 常规选项

        /// <remarks>
        /// 尾流/感应模型（开关）的类型{0=none, 1=SBEMT, 2=DBEMT,3=FreeWake,4=SBEMT_UMT,5=DBEMT_UMT}
        /// </remarks>
        public WakeMode WakeMod; //       

        /// <remarks>
        /// 叶片动态失速模型类型（开关）｛1=steady model, 2=BL ，3=Øye ，4=IAG，5=GOR，6=ATEF｝
        /// </remarks>
        public DynamicStallMode AFAeroMod;

        /// <summary>
        /// 3维旋转效应模型
        /// </summary>
        public ThreeDimRotationalEffect ThreeDRotationalEffect;
        /// <remarks>
        /// 基于塔周围潜在流量的类型塔对风的影响（开关）｛0=无，1=基线潜在流量，2=带Bak校正的潜在流量｝
        /// </remarks>
        ///public TowerInfluenceMode TwrPotent;

        /// <remarks>
        /// 是否计算塔影效应
        /// </remarks>
        public bool TwrShadow;

        /// <remarks>
        ///  计算塔架开启动力学
        /// </remarks>
        public bool TwrAero;

        /// <summary>
        /// 是否计算机舱/(轮毂)的气动力 (flag)
        /// </summary>
        public bool NacAero;
        #endregion 常规选项


        #region 环境物理参数
        /// <remarks>
        /// 空气密度kg/m^3
        /// </remarks>
        public double AirDens;

        /// <remarks>
        /// 运动空气粘度（m^2/s）
        /// </remarks>
        public double KinVisc;

        /// <remarks>
        /// 声速（m/s）
        /// </remarks>
        public double SpdSound;
        #endregion 环境物理参数


        #region 叶素动量理论选项

        /// <remarks>
        /// 偏斜尾流修正模型类型（开关） { 1 = none, 2 = Pitt / Peters}[used only when WakeMod=1]
        /// </remarks>
        public int SkewMod;

        /// <remarks>
        /// Pitt/Peters斜尾流模型中使用的参数 {or "default" is 15/32*pi} (-) [used only when SkewMod=2; unused when WakeMod=0 or 3]
        /// </remarks>
        public double SkewModFactor;

        /// <remarks>
        /// 叶尖损失修正
        /// </remarks>
        public bool TipLoss;

        /// <remarks>
        /// 叶根损失修正
        /// </remarks>
        public bool HubLoss;

        /// <remarks>
        /// 叶素动量理论的迭代误差要求 [used only when WakeMod=1]
        /// </remarks>
        public double BemtError;


        /// <remarks>
        /// 叶素动量理论的最大迭代次数
        /// </remarks>
        public int MaxIter;

        #endregion 叶素动量理论选项


        #region Beddoes Leishman不稳定翼型空气动力学选项
        /// <remarks>
        /// 非定常气动模型开关（switch）{1=基准模型（原始模型），2=Gonzalez变型（改变Cn、Cc、Cm），3=Minemma/Pierce变型（改变Cc和Cm）} [仅在AFAeroMod=2时使用]。<br/>
        /// <br/> - 这段英文描述了一个关于非定常气动学模型的开关选项，其中"switch"指的是选择哪种非定常气动学模型。共有三种模型可供选择：基准模型、Gonzalez变型和Minemma/Pierce变型。其中，Gonzalez变型和Minemma/Pierce变型都是基于基准模型进行的改进，分别改变了不同的气动系数（如升力系数Cn、阻力系数Cc、弯矩系数Cm等）。在实际应用中，这个开关选项通常与AFAeroMod参数一起使用，用于指定非定常气动学计算所采用的模型和算法。
        /// </remarks>
        public int UAMod;

        /// <remarks>
        /// 是否计算流场参数f'的查找表（TRUE），还是使用最佳拟合指数方程（FALSE）的标志。<br/>
        /// <br/> - 这段英文描述了一个关于流场参数f'的计算方法的选择。其中，f'是一个描述流场来流状态的无量纲参数，通常用于非定常气动学计算中。在计算过程中，可以选择计算f'的查找表，也可以使用最佳拟合指数方程。如果选择计算查找表，则需要预先计算和存储一系列f'的数值和相应的计算结果，以便在实际计算中进行查找和插值。而如果选择使用最佳拟合指数方程，则可以通过对已知的f'和相应的计算结果进行拟合，得到一个数学模型，从而在实际计算中直接使用该模型进行计算。该标志用于指示所选择的计算方法。
        /// </remarks>
        public bool FLookup;
        #endregion Beddoes Leishman不稳定翼型空气动力学选项


        #region 自由涡尾迹模型选项

        /// <remarks>
        /// 自由涡尾迹模型设置文件 [used only when WakeMod=3]
        /// </remarks>
        public string OLAFInputFileName;


        public FreeWake1 FreeWake;
        #endregion  自由涡尾迹模型选项


        #region 叶片和塔架Geometry的气动结构耦合文件

        /// <summary>
        /// 在计算中包括气动俯仰力矩  (flag)
        /// </summary>
        public bool UseBlCm;
        /// <remarks>
        /// 包含叶片分空气动力学特性的文件名称
        /// </remarks>
        public string[] ADBlFile;

        /// <summary>
        /// 叶片的气动几何参数
        /// </summary>
        public BladeGeometry1[] BldGeo;

        ///// <remarks>
        ///// 塔基开始的基础高度在水面上的位置（陆上风力机设置为0，海上按照ElastoDyn.TowerBsHt的高度进行设置）
        ///// </remarks>
        //public double TowerBsHt;

        ///// <remarks>
        ///// 分析中使用的塔节点数 [used only when TwrPotent/=0, TwrShadow=True, or TwrAero=True]
        ///// </remarks>
        //public int NumTwrNds;




        ///// <remarks>
        ///// AeroL读取的叶片和塔架几何信息
        ///// </remarks>
        //public Geometry1 Geometry;

        /// <summary>
        /// 包含塔架空气动力学特性的文件名称
        /// </summary>
        public string ADTwFile;
        /// <summary>
        /// Openhast 当中增加的新的参数，OpenWECd当中没有，这个函数支持不同叶片的定义
        /// </summary>
        public TowerGeometry1 TwrGeo;
        #endregion 叶片和塔架Geometry的气动结构耦合文件




        #region AerofoiLs 信息
        /// <summary>
        /// 插值模式
        /// </summary>
        public int InterpOrd;
        /// <remarks>
        /// Number of airfoil files
        /// </remarks>
        public int NumAFfiles;

        /// <remarks>
        /// 翼型路径，厚度从高到低
        /// </remarks>
        public string[] airfoilpath;

        /// <remarks>
        /// AeroL读取到的翼型信息
        /// </remarks>
        public Airfoil1 Airfoil;

        #endregion AerofoiLs 信息


        #region 计算Cp曲线 [used only when ApOfMb=1]

        /// <remarks>
        ///  -最小的叶尖速比
        /// </remarks>
        public double Minlamda;

        /// <remarks>
        /// -最大的叶尖速比
        /// </remarks>
        public double Maxlamda;

        /// <remarks>
        ///   -叶尖速比的间隔
        /// </remarks>
        public double lamdaStep;

        /// <remarks>
        /// -最小的叶尖速比
        /// </remarks>
        public double MinPitch;

        /// <remarks>
        /// -最大的叶尖速比
        /// </remarks>
        public double MaxPitch;

        /// <remarks>
        /// -叶尖速比的间隔
        /// </remarks>
        public double PitchStep;

        /// <remarks>
        /// -计算Cp曲线的结果文件
        /// </remarks>
        public string CpResultFilePath;

        #endregion 计算Cp曲线 [used only when ApOfMb=1]


        #region 计算功率曲线 [used only when ApOfMb=2]

        /// <remarks>
        ///  -最小风速
        /// </remarks>
        public double MinWindSpeed;

        /// <remarks>
        /// -最大风速
        /// </remarks>
        public double MaxWindSpeed;

        /// <remarks>
        ///   -风速间隔
        /// </remarks>
        public double WindSpeedStep;



        /// <remarks>
        /// - 初始桨距角[rad]
        /// </remarks>
        public double orig_pit;

        /// <remarks>
        /// - 切入转速[rpm / min]
        /// </remarks>
        public double ωmin;

        /// <remarks>
        /// - 额定功率[kw]
        /// </remarks>
        public double opt_KW;

        /// <remarks>
        /// - 额定转速[rpm / min]
        /// </remarks>
        public double opt_rpm_rad;

        /// <remarks>
        /// - 发电机效率%
        /// </remarks>
        public double η;

        /// <remarks>
        ///  - 最大桨距角[rad]
        /// </remarks>
        public double pitch_up;

        /// <remarks>
        ///  - 最小桨距角[rad]
        /// </remarks>
        public double pitch_down;

        /// <remarks>
        ///  - 是否变桨
        /// </remarks>
        public bool ifpitch;

        /// <remarks>
        /// - 固定桨距角[rad][used only when ifpitch = false]
        /// </remarks>
        public double Fixed_pitch;

        /// <remarks>
        ///  - 固定转速[rpm / min][used only when ifpitch = false]
        /// </remarks>
        public double Fixed_rotationalspeed;

        /// <remarks>
        /// -计算功率曲线的结果文件目录
        /// </remarks>
        public string PowerCurveResultFilePath;
        #endregion 计算功率曲线 [used only when ApOfMb=2]

        
        //#region 其他参数
        ///// <summary>
        ///// 变桨轴线到叶片前缘的位置。
        ///// </summary>
        //public Vector<double>[] PitchAxis;

        //public Vector<double>[] AeroCentJ1;

        //public Vector<double>[] AeroCentJ2;
        //#endregion 其他参数


        #region output

        /// <summary>
        /// 是否生成输出文件
        /// </summary>
        public bool SumPrint;

        /// <summary>
        /// 输出模式选择是节点·还是变量。如果是节点将生成下面节点数量的文件，否则生成输出变量个数的文件。
        /// </summary>
        public bool AfSpanput;
        /// <summary>
        /// 输出的叶片编号，默认只有0
        /// </summary>
        public int[] BldOutSig;
        /// <summary>
        /// 生成的文件夹名称，注意是文件夹！
        /// </summary>
        public string SumPath;

        /// <remarks>
        /// 叶片的输出截面个数
        /// </remarks>
        public int NBlOuts;

        /// <remarks>
        /// 输出的叶片节点编号
        /// </remarks>
        public int[] BlOutNd;

        /// <remarks>
        /// 塔架的输出截面个数
        /// </remarks>
        public int NTwOuts;

        /// <remarks>
        /// 输出的塔架节点编号
        /// </remarks>
        public int[] TwOutNd;

        public string[] Outputs_OutList;

        #endregion  output
    }

    /// <summary>
    /// 塔架几何信息
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct TowerGeometry1
    {

        /// <remarks>
        /// 塔架的截面个数
        /// </remarks>
        public int NumTowerSection;

        /// <summary>
        /// 塔架的净高度
        /// </summary>
        public double TwrLength;
        /// <summary>
        /// 塔架的高度归一化
        /// </summary>
        public Vector<double> TowerHID;

        /// <remarks>
        /// 塔架的高度向量，从0开始[m]
        /// </remarks>
        public Vector<double> TowerH;

        /// <remarks>
        /// 塔架第二列的直径[m]
        /// </remarks>
        public Vector<double> TwrDiam;

        /// <remarks>
        /// 塔架第二列的Cd[.]
        /// </remarks>
        public Vector<double> TwrCd;
    }

    /// <summary>
    /// 叶片的几何信息
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct BladeGeometry1
    {
        /// <remarks>
        /// 叶片的截面个数[-]
        /// </remarks>
        public int NumBladeSection;

        /// <summary>
        /// 叶片的长度,不包含轮毂半径[m]
        /// </summary>
        public double BLength;
        /// <remarks>
        /// 叶片展向位置，从0开始[m]
        /// </remarks>
        public Vector<double> Span;

        /// <remarks>
        ///  弦长[m]
        /// </remarks>
        public Vector<double> BlChord;

        /// <remarks>
        /// 第五列BlTwist扭角[rad](输入文件是deg,在读取的时候转换为标准的rad)
        /// </remarks>
        public Vector<double> BlTwist;

        /// <remarks>
        /// 变桨轴线，输入是%,会处理为[-]数值
        /// </remarks>
        public Vector<double> Pitch;

        /// <remarks>
        /// BlCrvAC(预弯)，对应新版本结构文件当中的YR[m]
        /// </remarks>
        public Vector<double> BlCrvAC;

        /// <remarks>
        /// BlSwpAC(后掠)，对应新版本结构文件当中的XR[m]
        /// </remarks>
        public Vector<double> BlSwpAC;

        /// <summary>
        /// 相对厚度[-]
        /// </summary>
        public Vector<double> BlThickness;
        ///// <remarks>
        ///// BlGeoID，对应的是插值ID,从0开始
        ///// </remarks>
        //public int[] BlGeoID;

        /// <summary>
        /// 归一化1后的叶片展长[-]
        /// </summary>
        public Vector<double> BlSpanID;


        /// <summary>
        /// 处理的变桨轴线与气动中心25%处的距离[m]
        /// </summary>
        public Vector<double> AeroCent;

        ///// <summary>
        ///// 处理的变桨轴线与气动中心25%处的距离[m]
        ///// </summary>
        //public Vector<double> AeroCentJ2;
    }

    /// <summary>
    /// 自由涡尾迹文件结构体
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct FreeWake1
    {
        /// <summary>
        /// 最大步长参数
        /// </summary>
        public double Alpha;
        /// <summary>
        /// 动量估计的第一衰减率
        /// </summary>
        public double Beta_1;
        /// <summary>
        /// 动量估计的第二衰减率
        /// </summary>
        public double Beta_2;
        /// <summary>
        /// 允许误差
        /// </summary>
        public double eps;
        /// <summary>
        /// 最大迭代次数
        /// </summary>
        public int Max_iter;
        /// <summary>
        /// 维度，支持2维或者3维计算
        /// </summary>
        public int Dim;
    }

    /// <summary>
    /// 定义叶片截面结构体，包含了失速模型参数，升阻力参数，和外形参数
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct airfoil__temp
    {
        /// <summary>
        /// 翼型厚度[-]
        /// </summary>
        public double Thickness;

        /// <summary>
        /// 雷诺数(Reynolds Number)(-)
        /// </summary>
        public double ReyNum;

        /// <summary>
        /// CM测量的位置(气动中心) Pitching Moment Centre(%)
        /// </summary>
        public Vector<double> PMCentre;

        /// <summary>
        /// 翼型的外形几何数据文件(必须填写，以实现VTK)
        /// </summary>
        public string Geometry;
        /// <summary>
        /// 翼型几何参数
        /// </summary>
        public Airfoil_Geo Airfoil_Geo;


        //#region===========  OYG modal  ============
        //public double T_f_OYG;
        //#endregion ===========  OYG modal  ============

        //#region===========  IAG modal modal  ============
        ///// <summary>
        ///// Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.3]
        ///// </summary>
        //public double A1;
        ///// <summary>
        ///// Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]
        ///// </summary>
        //public double A2;
        ///// <summary>
        ///// //  Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]
        ///// </summary>
        //public double b1;
        ///// <summary>
        /////  Constant in the expression of phi_alpha^c and phi_q^c.This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.53]
        ///// </summary>
        //public double b2;
        ///// <summary>
        ///// 
        ///// </summary>
        //public double ka;
        ///// <summary>
        /////  Boundary-layer, leading edge pressure gradient time constant in the expression of Dp. It should be tuned based on airfoil experimental data. [default = 1.7]
        ///// </summary>
        //public double T_p;
        ///// <summary>
        ///// Initial value of the time constant associated with Df in the expression of Df and f''. [default = 3]
        ///// </summary>
        //public double T_f;
        ///// <summary>
        ///// //  Initial value of the time constant associated with the vortex lift decay process; it is used in the expression of Cvn.It depends on Re, M, and airfoil class. [default = 6]
        ///// </summary>
        //public double T_V;
        /////<summary>
        /////Initial value of the time constant associated with the vortex advection process; it represents the non-dimensional time in semi-chords, needed for a vortex to travel from LE to trailing edge(TE); it is used in the expression of Cvn.It depends on Re, M (weakly), and airfoil. [valid range = 6 - 13, default = 6]
        ///// </summary>
        //public double T_VL;
        ///// <summary>
        ///// 
        ///// </summary>
        //public double K_v;// 
        ///// <summary>
        ///// 
        ///// </summary>
        //public double K_Cf;// 
        ///// <summary>
        ///// 
        ///// </summary>
        //public double T_Um;// 
        ///// <summary>
        ///// 
        ///// </summary>
        //public double T_Dm;// 
        ///// <summary>
        ///// 
        ///// </summary>
        //public double M_IAG;// 
        //#endregion ===========  IAG modal modal  ============

        //#region===========  OYG modal  ============
        //public double A_M;
        //#endregion ===========  OYG modal  ============


        #region  ===========  Airfoil data  ===========

        ///// <remarks>
        ///// 在准稳态表格查找中要使用的插值次序 {1=线性；2=三次样条插值；"默认"} [默认=1]。
        ///// </remarks>
        //public int InterpOrd;
        /// <remarks>
        /// 翼型行数
        /// </remarks>
        public int NumAlf;
        /// <remarks>
        /// 表示该翼型在不同攻角下的升力系数、阻力系数等数据。每一列分别是：[攻角 Cl Cd CM ......]
        /// </remarks>
        public Matrix<double> DataSet;

        #endregion  ===========  Airfoil data  ===========
    }
    /// <summary>
    /// 叶片翼型结构体
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct Airfoil1
    {
        public Airfoil1()
        {
            list = new List<airfoil__temp>();
        }

        /// <summary>
        /// 翼型文件数量
        /// </summary>
        public int Nfoil;
        /// <summary>
        /// 叶片翼型的厚度向量从大到小，与翼型文件数量一致
        /// </summary>
        public Vector<double> Thickness;

        /// <summary>
        /// 弦长方向气动中心
        /// </summary>
        public Vector<double> DistX;
        /// <summary>
        /// 垂直于弦长方向气动中心
        /// </summary>
        public Vector<double> DistY;
        /// <summary>
        /// 插值模式, 0: 线性插值 meth = "linear"，1: 样条插值meth == "pchip"
        /// </summary>
        public int InterpOrd;
        /// <summary>
        /// 翼型集合
        /// </summary>
        public List<airfoil__temp> list;

        /// <summary>
        /// 为每个截面插值后的数据，AeroL_INI.AER_INIAirFoilInterp 翼型初始化后长会有此数据
        /// </summary>
        public Matrix<double>[] SecDataSet;

        public airfoil__temp this[int a]
        {
            get
            {
                return list[a];
            }
            set
            {
                list[a] = value;
            }
        }



        /// <summary>
        /// 创建升力系数插值
        /// </summary>
        public MathNet.Numerics.Interpolation.IInterpolation[] IterCl;
        /// <summary>
        /// 创建阻力系数插值
        /// </summary>
        public MathNet.Numerics.Interpolation.IInterpolation[] IterCd;
        /// <summary>
        /// 创建扭转系数插值
        /// </summary>
        public MathNet.Numerics.Interpolation.IInterpolation[] IterCm;


        public MathNet.Numerics.Interpolation.IInterpolation[] IterC4;

        public MathNet.Numerics.Interpolation.IInterpolation[] IterC5;

        public MathNet.Numerics.Interpolation.IInterpolation[] IterC6;

    }
    /// <summary>
    /// 用来定义翼型几何外形的结构体
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct Airfoil_Geo
    {
        /// <summary>
        /// 描述翼型的节点数量
        /// </summary>
        public int Pointnum;
        /// <summary>
        /// 叶片点集合的x坐标
        /// </summary>
        public Vector<double> x;
        /// <summary>
        /// 叶片点集合的y坐标
        /// </summary>
        public Vector<double> y;
        /// <summary>
        /// 读取文件的原始数据，第一列是x,第二列是y
        /// </summary>
        public Matrix<double> data;
        /// <summary>
        /// 气动中心
        /// </summary>
        public Vector<double> center;
    }
    /// <summary>
    /// 自由涡尾迹参数
    /// </summary>
    public struct FVW1
    {
        #region 输入文件信息
        /// <summary>
        /// 输入文件路径
        /// </summary>
        public string path;

        /// <summary>
        /// 输入文件内容
        /// </summary>
        public string[] Filedata;


        #endregion
    }
    
    /// <summary>
    /// 主要用于外部函数的调用，动力学分析，和计算。所以有一些参数暂时没有用
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct AER_ParameterType
    {
        /// <summary>
        /// 叶片的数量[-]
        /// </summary>
        public int NumBl;

        /// <summary>
        /// 塔架的数量[-]
        /// </summary>
        public int NumTwr;

        /// <summary>
        /// 叶片的节点数量[-]
        /// </summary>
        public int BldNodes;

        /// <summary>
        /// 塔架的节点数量[-]
        /// </summary>
        public int TwrNodes;

        /// <summary>
        /// 轮毂半径[m]
        /// </summary>
        public double HubRad;

        /// <summary>
        /// 锥角，一般由MBD模块来初始化
        /// </summary>
        public double Precone;
        /// <summary>
        /// 是否计算塔架气动力，默认不计算
        /// </summary>
        public bool TwrAero;

        /// <summary>
        /// 是否开启塔影效应的计算，默认不开启
        /// </summary>
        public bool TwrShadow;

        /// <summary>
        /// 叶片动态入流{0=off,1=open}
        /// </summary>
        public int AFDynInflow;
        /// <summary>
        /// 是否开启气动弹性计算，默认不开启
        /// </summary>
        public bool AeroEla;
        /// <summary>
        ///  空气密度(kg/m^3)
        /// </summary>
        public double AirDens;

        /// <summary>
        /// 空气的运动粘度(m^2/s)
        /// </summary>
        public double KinVisc;
        /// <summary>
        /// 声速(m/s)
        /// </summary>
        public double SpdSound;
        /// <summary>
        /// 尾流感应模型{0=none, 1=SBEMT, 2=DBEMT,3=FreeWake,4=SBEMT_UMT,5=DBEMT_UMT}
        /// </summary>
        public WakeMode wakeMode;

        /// <summary>
        /// 动态失速模型
        /// </summary>
        public DynamicStallMode DyStallMode;

        /// <summary>
        /// 3D旋转模型
        /// </summary>
        public ThreeDimRotationalEffect ThreeDimRotationalEffect;

        ///// <summary>
        ///// 塔影效应模型
        ///// </summary>
        //public TowerInfluenceMode TwrPotent;

        /// <summary>
        /// 叶素动量理论的输入参数
        /// </summary>
        public BEMOptions BEMopt;

        /// <summary>
        /// 翼型信息
        /// </summary>
        public Airfoil1 Airfoil;

        /// <summary>
        /// 叶片的几何信息
        /// </summary>
        public BladeGeometry1[] BldGeo;

        /// <summary>
        /// 塔架几何信息
        /// </summary>
        public TowerGeometry1 TwrGeo;





        /// <summary>
        /// 叶片顶部节点编号
        /// </summary>
        public int Tipnode;

        /// <summary>
        /// 塔架顶部节点编号
        /// </summary>
        public int TTopnode;
    }
    /// <summary>
    /// 保存计算过程当中的某个步长下的数据
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct AER_RtHndSideTypeTemp
    {

    }

    /// <summary>
    /// 保存计算过程当中的数据
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct AER_RtHndSideType
    {
        /// <summary>
        /// AeroL 模块的输出流模块
        /// </summary>
        public AeroL_IO_Out Aer_IO_Out;

        /// <summary>
        /// 计算动态失速的接口
        /// </summary>
        public I_ALCalDynamicStall CallDynamicStall;

        /// <summary>
        /// 计算动态空气动力学载荷的接口
        /// </summary>
        public CallDynamicAeroLoad CallDynamicAeroLoad;

        /// <summary>
        /// 3维旋转效应计算模型
        /// </summary>
        public I_ALCal3DRotationalEffect Call3DRotationalEffect;

        /// <summary>
        /// 单元的属性，保存单元的计算信息和计算结构
        /// </summary>
        public T_ALAeroBladeElement[,] BldEle;

        /// <summary>
        /// 塔架节点的属性，保存塔架的计算信息和计算结构
        /// </summary>
        public T_ALAeroTowerElement[,] TwrEle;

        /// <summary>
        /// 机舱节点的属性，保存塔架的计算信息和计算结构
        /// </summary>
        public T_ALAeroNacalElement[] NacEle;

        /// <summary>
        /// 风轮转速[rad/s]
        /// </summary>
        public double RtSpeed;

        /// <summary>
        /// 风轮叶尖速比[-]
        /// </summary>
        public double RtTSR;

        /// <summary>
        /// 风轮偏航角[rad]
        /// </summary>
        public double RtSkew;

        /// <summary>
        /// 分轮气动功率[W](只有一个节点会被赋值)
        /// </summary>
        public double RtAeroPwr;

        /// <summary>
        /// Rotor swept area
        /// </summary>
        public double RtArea;

        /// <summary>
        /// Rotor aerodynamic power coefficient
        /// </summary>
        public double RtAeroCp;
        /// <summary>
        /// Rotor aerodynamic torque coefficient
        /// </summary>
        public double RtAeroCq;
        /// <summary>
        /// Rotor aerodynamic thrust coefficient
        /// </summary>
        public double RtAeroCt;

        public Vec WSHub;

        public Vec WSRotor;
    }





    /// <summary>
    /// 叶素动量理论的输入参数
    /// </summary>
    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct BEMOptions
    {
        /// <summary>
        /// 1=uncoupled, 2=Pitt/Peters  [unused when WakeMod=1 or 2]
        /// </summary>
        public int SkewMod;

        /// <summary>
        /// Pitt/Peters斜尾流模型中使用的常数
        /// </summary>
        public double SkewModFactor;

        /// <summary>
        /// 是否支持叶尖损失
        /// </summary>
        public bool TipLoss;

        /// <summary>
        /// 是否支持叶根损失
        /// </summary>
        public bool HubLoss;

        /// <summary>
        /// 允许的最大叶素动量理论的迭代误差要求
        /// </summary>
        public double BemtError;

        /// <summary>
        /// 最大的迭代次数，超过最大迭代测试回报错迭代不完全
        /// </summary>
        public int MaxIter;
    }

    /// <summary>
    /// 尾流感应模型{0=none, 1=SBEMT, 2=DBEMT,3=FreeWake,4=SBEMT_UMT,5=DBEMT_UMT}
    /// </summary>
    [SourceReflection]
    public enum WakeMode
    {
        /// <summary>
        /// 动态不允许这个选项
        /// </summary>
        None = 0,
        /// <summary>
        /// 静态叶素动量理论 Blade-Element/Momentum Theory 
        /// </summary>
        SBEMT = 1,

        /// <summary>
        /// 动态叶素动量理论 Blade-Element/Momentum Theory
        /// </summary>
        DBEMT = 2,

        /// <summary>
        /// 自由涡尾迹模型 Free Vortex Wake
        /// </summary>
        FreeWake = 3,

        /// <summary>
        /// 基于统一动量模型的静态叶素动量理论 Blade-Element/Momentum Theory with Unified Momentum Model
        /// </summary>
        SBEMT_UnifiedMomentunModel=4,

        /// <summary>
        /// 基于统一动量模型的动态叶素动量理论 Blade-Element/Momentum Theory with Unified Momentum Model
        /// </summary>
        DBEMT_UnifiedMomentunModel=5
    }

    /// <summary>
    /// 动态失速模型
    /// </summary>
    [SourceReflection]
    public enum DynamicStallMode
    {
        /// <summary>
        /// 静态的模型，不考虑动态入流
        /// </summary>
        SteadyModel = 1,
        /// <summary>
        /// Beddoes-Leishman(B-L) 动态入流模型
        /// </summary>
        B_L = 2,
        /// <summary>
        /// 文献 Øye S. Dynamic stall simulated as time lag of separation [R]. Denmark: Department of Fluid Mechanics,Technical University of Denmark, 1991. 
        /// </summary>
        Øye = 3,
        /// <summary>
        /// IAG Dynamic stall model,文献：Development and Validation of the IAG Dynamic Stall Model inState-Space Representation for Wind Turbine Airfoils
        /// </summary>
        IAG = 4,
        /// <summary>
        /// 
        /// </summary>
        GOR = 5,
        ATEF = 6,
    }

    [SourceReflection]
    public enum ThreeDimRotationalEffect
    {
        /// <summary>
        /// 不考虑3D旋转效应
        /// </summary>
        None = 0,
        /// <summary>
        /// Snel模型
        /// </summary>
        Snel = 1,
       
    }
    ///// <summary>
    ///// 塔影效应模型
    ///// </summary>
    //[SourceReflection]
    //public enum TowerInfluenceMode
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    Baseline_potential_flow = 1,
    //    PotentialFlowWithBakCorrection = 2,
    //}



    [SourceReflection][StructLayout(LayoutKind.Sequential)]
    public struct AER_AllOuts
    {
        /// <summary>
        /// 叶片的方位角 Azimuth angle of blade  nan unit:(deg)
        /// </summary>
        public double[] BAzimuth;
        /// <summary>
        /// 叶片的变桨角Pitch angle of blade  nan unit:(deg)
        /// </summary>
        public double[] BPitch;
        /// <summary>
        /// 全局坐标系下叶片K的节点J处的非诱导输入风速的x分量 x-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVUndx;
        /// <summary>
        /// 全局坐标系下叶片K的节点J处的非诱导输入风速的y分量 y-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVUndy;
        /// <summary>
        /// 全局坐标系下叶片K的节点J处的非诱导输入风速的z分量 z-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVUndz;
        /// <summary>
        /// x-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVDisx;
        /// <summary>
        /// y-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVDisy;
        /// <summary>
        /// z-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjVDisz;
        /// <summary>
        /// x-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjSTVx;
        /// <summary>
        /// y-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjSTVy;
        /// <summary>
        /// z-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
        /// </summary>
        public double[,] BkNjSTVz;
        /// <summary>
        /// Relvative wind speed at Blade k, Node j  nan unit:(m/s)
        /// </summary>
        public double[,] BkNjVRel;
        /// <summary>
        /// Dynamic pressure at Blade k, Node j  nan unit:(Pa)
        /// </summary>
        public double[,] BkNjDynP;
        /// <summary>
        /// Reynolds number (in millions) at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjRe;
        /// <summary>
        /// Mach number at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjMachN;
        /// <summary>
        /// Axial induced wind velocity at Blade k, Node j  nan unit:(m/s)
        /// </summary>
        public double[,] BkNjVindx;
        /// <summary>
        /// Tangential induced wind velocity at Blade k, Node j  nan unit:(m/s)
        /// </summary>
        public double[,] BkNjVindy;
        /// <summary>
        /// Axial induction factor at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjAxInd;
        /// <summary>
        /// Tangential induction factor at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjTnInd;
        /// <summary>
        /// Angle of attack at Blade k, Node j  nan unit:(deg)
        /// </summary>
        public double[,] BkNjAlpha;
        /// <summary>
        /// Pitch+Twist angle at Blade k, Node j  nan unit:(deg)
        /// </summary>
        public double[,] BkNjTheta;
        /// <summary>
        /// Inflow angle at Blade k, Node j  nan unit:(deg)
        /// </summary>
        public double[,] BkNjPhi;
        /// <summary>
        /// Curvature angle at Blade k, Node j  nan unit:(deg)
        /// </summary>
        public double[,] BkNjCurve;
        /// <summary>
        /// Lift force coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCl;
        /// <summary>
        /// Drag force coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCd;
        /// <summary>
        /// Pitching moment coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCm;
        /// <summary>
        /// Normal force (to plane) coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCx;
        /// <summary>
        /// Tangential force (to plane) coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCy;
        /// <summary>
        /// Normal force (to chord) coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCn;
        /// <summary>
        /// Tangential force (to chord) coefficient at Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCt;
        /// <summary>
        /// Pressure coefficient Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjCpmin;
        /// <summary>
        /// Critical cavitation number Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjSigCr;
        /// <summary>
        /// Cavitation number Blade k, Node j  nan unit:(-)
        /// </summary>
        public double[,] BkNjSgCav;
        /// <summary>
        /// Circulation on blade k at node j  nan unit:(m^2/s)
        /// </summary>
        public double[,] BkNjGam;
        /// <summary>
        /// Lift force per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFl;
        /// <summary>
        /// Drag force per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFd;
        /// <summary>
        /// Pitching moment per unit length at Blade k, Node j  nan unit:(N-m/m)
        /// </summary>
        public double[,] BkNjMm;
        /// <summary>
        /// Normal force (to plane) per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFx;
        /// <summary>
        /// Tangential force (to plane) per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFy;
        /// <summary>
        /// Normal force (to chord) per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFn;
        /// <summary>
        /// Tangential force (to chord) per unit length at Blade k, Node j  nan unit:(N/m)
        /// </summary>
        public double[,] BkNjFt;
        /// <summary>
        /// Tower clearance at Blade k, Node j (based on the absolute distance to the nearest point in the tower from B1N1 minus the local tower radius, in the deflected configuration); please note that this clearance is only approximate because the calculation assumes that the blade is a line with no volume (however, the calculation does use the local tower radius); when B1N1 is above the tower top (or below the tower base), the absolute distance to the tower top (or base) minus the local tower radius, in the deflected configuration, is output  nan unit:(m)
        /// </summary>
        public double[,] BkNjClrnc;
        /// <summary>
        /// Rotor speed  nan unit:(rpm)
        /// </summary>
        public double RtSpeed;
        /// <summary>
        /// Rotor tip-speed ratio  nan unit:(-)
        /// </summary>
        public double RtTSR;
        /// <summary>
        /// Rotor-disk-averaged relative wind velocity (x-component)  the hub coordinate system unit:(m/s)
        /// </summary>
        public double RtVAvgxh;
        /// <summary>
        /// Rotor-disk-averaged relative wind velocity (y-component)  the hub coordinate system unit:(m/s)
        /// </summary>
        public double RtVAvgyh;
        /// <summary>
        /// Rotor-disk-averaged relative wind velocity (z-component)  the hub coordinate system unit:(m/s)
        /// </summary>
        public double RtVAvgzh;
        /// <summary>
        /// Rotor inflow-skew angle  nan unit:(deg)
        /// </summary>
        public double RtSkew;
        /// <summary>
        /// Total rotor aerodynamic load (force in x direction)  the hub coordinate system unit:(N)
        /// </summary>
        public double RtAeroFxh;
        /// <summary>
        /// Total rotor aerodynamic load (force in y direction)  the hub coordinate system unit:(N)
        /// </summary>
        public double RtAeroFyh;
        /// <summary>
        /// Total rotor aerodynamic load (force in z direction)  the hub coordinate system unit:(N)
        /// </summary>
        public double RtAeroFzh;
        /// <summary>
        /// Total rotor aerodynamic load (moment in x direction)  the hub coordinate system unit:(N-m)
        /// </summary>
        public double RtAeroMxh;
        /// <summary>
        /// Total rotor aerodynamic load (moment in y direction)  the hub coordinate system unit:(N-m)
        /// </summary>
        public double RtAeroMyh;
        /// <summary>
        /// Total rotor aerodynamic load (moment in z direction)  the hub coordinate system unit:(N-m)
        /// </summary>
        public double RtAeroMzh;
        /// <summary>
        /// Rotor aerodynamic power  nan unit:(KW)
        /// </summary>
        public double RtAeroPwr;
        /// <summary>
        /// Rotor swept area  nan unit:(m^2)
        /// </summary>
        public double RtArea;
        /// <summary>
        /// Rotor aerodynamic power coefficient  nan unit:(-)
        /// </summary>
        public double RtAeroCp;
        /// <summary>
        /// Rotor aerodynamic torque coefficient  nan unit:(-)
        /// </summary>
        public double RtAeroCq;
        /// <summary>
        /// Rotor aerodynamic thrust coefficient  nan unit:(-)
        /// </summary>
        public double RtAeroCt;
        /// <summary>
        /// time-constant used in DBEMT  nan unit:(s)
        /// </summary>
        public double DBEMTau1;
        /// <summary>
        /// Undisturbed x-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjVUndx;
        /// <summary>
        /// Undisturbed y-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjVUndy;
        /// <summary>
        /// Undisturbed z-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjVUndz;
        /// <summary>
        /// Structural translational velocity x-component at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjSTVx;
        /// <summary>
        /// Structural translational velocity y-component at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjSTVy;
        /// <summary>
        /// Structural translational velocity z-component at Tw node 1  local tower coordinate system unit:(m/s)
        /// </summary>
        public double[] TwHjSTVz;
        /// <summary>
        /// Relative wind speed at Tw node 1  nan unit:(m/s)
        /// </summary>
        public double[] TwHjVrel;
        /// <summary>
        /// Dynamic Pressure at Tw node 1  nan unit:(Pa)
        /// </summary>
        public double[] TwHjDynP;
        /// <summary>
        /// Reynolds number (in millions) at Tw node 1  nan unit:(-)
        /// </summary>
        public double[] TwHjRe;
        /// <summary>
        /// Mach number at Tw node 1  nan unit:(-)
        /// </summary>
        public double[] TwHjM;
        /// <summary>
        /// x-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
        /// </summary>
        public double[] TwHjFdx;
        /// <summary>
        /// y-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
        /// </summary>
        public double[] TwHjFdy;
    }
    /// <summary>
    /// 载荷输出的通道枚举体,要和c#自动化文件对应
    /// </summary>
    [SourceReflection]
    public enum AeroL_Loadchannels
    {
        Blade_Information = 0,
        Blade_Airfoil_Aerodynamic_Information = 1,
        Blade_Airfoil_Load = 2,
        Rotor_Aerodynamic_Load_And_Information = 3,
        Tower_Aerodynamic_Load_And_Information = 4,
    }

}

