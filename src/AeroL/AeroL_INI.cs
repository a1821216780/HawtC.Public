
#define ENGLISH
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


using MathNet.Numerics.Interpolation;
using OpenWECD.AeroL.BEMT.SteadyBEMT;
using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using OpenWECD.IO.math;
using OpenWECD.MBD;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using OpenWECD.AeroL.Airfoil.DynamicStallModal;
using OpenWECD.WindL;
using static OpenWECD.IO.math.LinearAlgebraHelper;
#if MBDLATDOUBLE

using Mat = OpenWECD.IO.Numerics.Matrix3S;
using Vec = OpenWECD.IO.Numerics.Vector3S;
using NUMT = System.Double;
using static System.Math;
using matrix = MathNet.Numerics.LinearAlgebra.Matrix<double>;
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


    public static class AeroL_INI
    {
        /// <summary>
        /// 初始化输入参数，读取AeroL 和多体动力学 后完成
        /// </summary>
        /// <returns></returns>
        public static AER_ParameterType AER_INIParameterType(AeroL1 aerol, MBD_ParameterType mp)
        {
            AER_ParameterType p = new AER_ParameterType();
            p.NumBl = aerol.Bldnum;
            if (p.NumBl != mp.NumBl)
            {
                LogHelper.ErrorLog("AeroL ERROR! AeroL.Bldnum!=MBD_ParameterType.NumBl", FunctionName: "AER_INIParameterType");
            }
            p.NumTwr = mp.TowerNum;
            p.BldNodes = aerol.BldGeo[0].NumBladeSection;
            p.TwrNodes = aerol.TwrGeo.NumTowerSection;
            p.Tipnode = mp.TipNode;
            p.HubRad = aerol.HubRad;
            p.TwrAero = aerol.TwrAero;
            p.TwrShadow = aerol.TwrShadow;
            //p.AFDynInflow = aerol.AFDynInflow;
            p.AirDens = aerol.AirDens;
            p.KinVisc = aerol.KinVisc;
            p.wakeMode = (WakeMode)aerol.WakeMod;
            p.DyStallMode = (DynamicStallMode)aerol.AFAeroMod;
            p.ThreeDimRotationalEffect = aerol.ThreeDRotationalEffect;
            //p.TwrPotent = (TowerInfluenceMode)aerol.TwrPotent;
            p.SpdSound = aerol.SpdSound;
            p.AeroEla = mp.AeroElastic;
            //p.Precone = mp.Precone;
            if (p.AeroEla == false)
            {
                LogHelper.WarnLog("AEROL 未开启气弹计算，可以在MBD主文件当中设置", FunctionName: "AER_INIParameterType");
            }

            //# 初始化叶素动量理论参数
            BEMOptions bEM = new BEMOptions();
            bEM.HubLoss = aerol.HubLoss;
            bEM.TipLoss = aerol.TipLoss;
            bEM.MaxIter = aerol.MaxIter;
            bEM.BemtError = aerol.BemtError;
            bEM.SkewModFactor = aerol.SkewModFactor;
            bEM.SkewMod = aerol.SkewMod;
            p.BEMopt = bEM;

            //# 初始化翼型插值
            p.Airfoil = aerol.Airfoil;
            AER_INIAirFoilInterp(ref p.Airfoil, aerol.BldGeo[0]);


            //# 叶片几何初始化
            //var BldGeo = new BladeGeometry[p.NumBl];
            //for (int i = 0; i < p.NumBl; i++)
            //{
            //    BladeGeometry bldgeo = new BladeGeometry();
            //    bldgeo.NumBladeSection = aerol.Geometry[i].NumBladeSection;
            //    bldgeo.BlSpn = aerol.Geometry[i].BlSpn.Clone();
            //    bldgeo.BlSpanID = aerol.Geometry[i].BlSpn / aerol.Geometry[i].BlSpn.Max();
            //    bldgeo.BlCrvAC = aerol.Geometry[i].BlCrvAC.Clone();
            //    bldgeo.BlSwpAC = aerol.Geometry[i].BlSwpAC.Clone();
            //    bldgeo.BlCrvAng = aerol.Geometry[i].BlCrvAng.Clone();

            //    bldgeo.BLength = aerol.Geometry[i].BlSpn.Max() - aerol.Geometry[i].BlSpn.Min();
            //    bldgeo.BlChord = aerol.Geometry[i].BlChord.Clone();
            //    bldgeo.BlTwist = aerol.Geometry[i].BlTwist.Clone();
            //    bldgeo.BlAFID = Copy(aerol.Geometry[i].BlAFID);
            //    bldgeo.PitchAxis = aerol.Geometry[i].PitchAxis.Clone();

            //    bldgeo.AeroCentJ1 = aerol.AeroCentJ1[i].Clone();
            //    bldgeo.AeroCentJ2 = aerol.AeroCentJ2[i].Clone();
            //    BldGeo[i] = bldgeo;
            //}
            p.BldGeo = aerol.BldGeo;
            p.Tipnode = aerol.BldGeo[0].NumBladeSection - 1;
            //# 塔架几何初始化
            //var TwrGeo = new TowerGeometry();
            //TwrGeo.TowerHID = (aerol.Geometry[0].TowerH - aerol.Geometry[0].TowerH.Min()) / (aerol.Geometry[0].TowerH.Max() - aerol.Geometry[0].TowerH.Min());
            //TwrGeo.TowerH = aerol.Geometry[0].TowerH - aerol.Geometry[0].TowerH.Min();
            //TwrGeo.TowerD = aerol.Geometry[0].TowerD.Clone();
            //TwrGeo.TowerCd = aerol.Geometry[0].TowerCd.Clone();
            p.TwrGeo = aerol.TwrGeo;
            p.TTopnode = aerol.TwrGeo.NumTowerSection - 1;



            return p;
        }

        /// <summary>
        /// 只能用于静态调用
        /// </summary>
        /// <param name="aerol"></param>
        /// <returns></returns>
        public static AER_ParameterType AER_INIParameterType(AeroL1 aerol)
        {
            AER_ParameterType p = new AER_ParameterType();
            p.NumBl = aerol.Bldnum;
            p.BldNodes = aerol.BldGeo[0].NumBladeSection;
            p.TwrNodes = aerol.TwrGeo.NumTowerSection;
            p.HubRad = aerol.HubRad;
            p.TwrAero = aerol.TwrAero;
            p.TwrShadow = aerol.TwrShadow;
            //p.AFDynInflow=aerol.AFDynInflow;
            p.AirDens = aerol.AirDens;
            p.wakeMode = (WakeMode)aerol.WakeMod;
            p.DyStallMode = (DynamicStallMode)aerol.AFAeroMod;
            p.ThreeDimRotationalEffect = aerol.ThreeDRotationalEffect;
            //p.TwrPotent = (TowerInfluenceMode)aerol.TwrPotent;
            p.SpdSound = aerol.SpdSound;
            //# 初始化叶素动量理论参数
            BEMOptions bEM = new BEMOptions();
            bEM.HubLoss = aerol.HubLoss;
            bEM.TipLoss = aerol.TipLoss;
            bEM.MaxIter = aerol.MaxIter;
            bEM.BemtError = aerol.BemtError;
            bEM.SkewModFactor = aerol.SkewModFactor;
            bEM.SkewMod = aerol.SkewMod;
            p.BEMopt = bEM;

            //# 初始化翼型
            p.Airfoil = aerol.Airfoil;
            AER_INIAirFoilInterp(ref p.Airfoil, aerol.BldGeo[0]);
            //aerol.Airfoil = p.Airfoil;
            //# 叶片几何初始化
            //var BldGeo = new BladeGeometry[p.NumBl];
            //for (int i = 0; i < p.NumBl; i++)
            //{
            //    BladeGeometry bldgeo = new BladeGeometry();
            //    bldgeo.NumBladeSection = aerol.Geometry[i].NumBladeSection;
            //    bldgeo.BlSpn = aerol.Geometry[i].BlSpn.Clone();
            //    bldgeo.BlSpanID = aerol.Geometry[i].BlSpn / aerol.Geometry[i].BlSpn.Max();
            //    bldgeo.BlCrvAC = aerol.Geometry[i].BlCrvAC.Clone();
            //    bldgeo.BlSwpAC = aerol.Geometry[i].BlSwpAC.Clone();
            //    bldgeo.BlCrvAng = aerol.Geometry[i].BlCrvAng.Clone();

            //    bldgeo.BLength = aerol.Geometry[i].BlSpn.Max() - aerol.Geometry[i].BlSpn.Min();
            //    bldgeo.BlChord = aerol.Geometry[i].BlChord.Clone();
            //    bldgeo.BlTwist = aerol.Geometry[i].BlTwist.Clone();
            //    bldgeo.BlAFID = Copy(aerol.Geometry[i].BlAFID);
            //    bldgeo.PitchAxis = aerol.Geometry[i].PitchAxis.Clone();

            //    bldgeo.AeroCentJ1 = aerol.AeroCentJ1[i].Clone();
            //    bldgeo.AeroCentJ2 = aerol.AeroCentJ2[i].Clone();
            //    BldGeo[i] = bldgeo;
            //}
            p.BldGeo = aerol.BldGeo;
            p.Tipnode = aerol.BldGeo[0].NumBladeSection - 1;
            //# 塔架几何初始化
            //var TwrGeo = new TowerGeometry();
            //TwrGeo.TowerHID = (aerol.Geometry[0].TowerH - aerol.Geometry[0].TowerH.Min()) / (aerol.Geometry[0].TowerH.Max() - aerol.Geometry[0].TowerH.Min());
            //TwrGeo.TowerH = aerol.Geometry[0].TowerH - aerol.Geometry[0].TowerH.Min();
            //TwrGeo.TowerD = aerol.Geometry[0].TowerD.Clone();
            //TwrGeo.TowerCd = aerol.Geometry[0].TowerCd.Clone();
            p.TwrGeo = aerol.TwrGeo;
            p.TTopnode = aerol.TwrGeo.NumTowerSection - 1;

            return p;
        }
        /// <summary>
        /// 初始化内存的占用
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static AER_RtHndSideType AER_INIRtHndSideType(AER_ParameterType p)
        {
            AER_RtHndSideType rths = new AER_RtHndSideType();

            rths.BldEle = new T_ALAeroBladeElement[p.BldNodes, p.NumBl];
            rths.TwrEle = new T_ALAeroTowerElement[p.TwrNodes, p.NumTwr];



            //# 对叶片节点进行初始化
            for (int K = 0; K < p.NumBl; K++)
            {
                for (int J = 0; J < p.BldNodes; J++)
                {
                    rths.BldEle[J, K].SRHub = p.HubRad;
                    rths.BldEle[J, K].Da = 0.11f;
                    rths.BldEle[J, K].Daa = 0.028f;
                    rths.BldEle[J, K].SBlspan = p.BldGeo[K].Span[J] + p.HubRad;
                    rths.BldEle[J, K].SChord = p.BldGeo[K].BlChord[J];
                    rths.BldEle[J, K].SFoilNum = J;//p.BldGeo[K].BlAFID[J];
                    rths.BldEle[J, K].STwist = p.BldGeo[K].BlTwist[J];
                    rths.BldEle[J, K].SSolid = ((double)p.NumBl / 2.0 / Math.PI) * rths.BldEle[J, K].SChord;
                    rths.BldEle[J, K].SAeroCent = p.BldGeo[K].AeroCent[J];
                    //rths.BldEle[J, K].SAeroCentJ2 = p.BldGeo[K].AeroCentJ2[J];
                    rths.BldEle[J, K].SPitchAxis = p.BldGeo[K].Pitch[J];
                    rths.BldEle[J, K].STipRad = p.BldGeo[K].BLength + p.HubRad + 1E-5;
                    //rths.BldEle[J, K].historyCl = new Otherhelper.FixedQueue<double>(6, 0);
                    //rths.BldEle[J, K].historyCd = new Otherhelper.FixedQueue<double>(6, 0);
                    //rths.BldEle[J, K].historyCm = new Otherhelper.FixedQueue<double>(6, 0);

                    //# UnsteadyBEM
                    rths.BldEle[J, K].UnsteadyBEM_W = Vec.Zero;
                    rths.BldEle[J, K].UnsteadyBEM_Wint = Vec.Zero;
                    rths.BldEle[J, K].UnsteadyBEM_Wqs = Vec.Zero;

                }
            }

            //# 对塔架节点进行初始化
            for (int K = 0; K < p.NumTwr; K++)
            {
                for (int J = 0; J < p.TwrNodes; J++)
                {
                    rths.TwrEle[J, K].SHeight = p.TwrGeo.TowerH[J];
                    rths.TwrEle[J, K].SChord = p.TwrGeo.TwrDiam[J];
                    rths.TwrEle[J, K].SCd = p.TwrGeo.TwrCd[J];
                    //rths.TwrEle[J, K].DF = new System.Numerics.Vector3[p.TwrNodes, p.NumTwr];
                    //rths.TwrEle[J, K].DM = new System.Numerics.Vector3[p.TwrNodes, p.NumTwr];
                    //rths.TwrEle[J, K].DStrVx = new float[p.TwrNodes, p.NumTwr];
                    //rths.TwrEle[J, K].DStrVy = new float[p.TwrNodes, p.NumTwr];
                    //rths.TwrEle[J, K].DStrVz = new float[p.TwrNodes, p.NumTwr];
                }
            }
            //# 对轮毂节点进行初始化

            rths.NacEle = new T_ALAeroNacalElement[p.NumTwr];
            return rths;
        }


        /// <summary>
        /// 对AER_ParameterType当中的动态失速进行初始化
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static I_ALCalDynamicStall AER_INIDynamicstall(AER_ParameterType p)
        {
            LogHelper.WriteLog("Running AeroL.DynamicStall ", show_title: false, color: ConsoleColor.Blue);
            switch (p.DyStallMode)
            {
                case DynamicStallMode.SteadyModel:
                    return new AeroL.Airfoil.DynamicStallModal.StaedyModel(p);
                case DynamicStallMode.B_L:
                    return new AeroL.Airfoil.DynamicStallModal.Beddoes_Leishman(p);
                case DynamicStallMode.Øye:
                    return new AeroL.Airfoil.DynamicStallModal.Oye(p);
                case DynamicStallMode.IAG:
                    return new AeroL.Airfoil.DynamicStallModal.IAG(p);
                case DynamicStallMode.GOR:
                    return new AeroL.Airfoil.DynamicStallModal.GOR(p);
                case DynamicStallMode.ATEF:
                    return new AeroL.Airfoil.DynamicStallModal.ATEF(p);
                default:
                    LogHelper.ErrorLog(p.DyStallMode + ",No such Dynamic Stall Modol", FunctionName: "AER_INIDynamicstall");
                    return new AeroL.Airfoil.DynamicStallModal.StaedyModel(p);
            }
        }

        /// <summary>
        /// 对AER_ParameterType当中的三维旋转进行初始化，在动态失速后面初始化
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static I_ALCal3DRotationalEffect AER_INIThreeDRotationaEffect(AER_ParameterType p,Airfoil1 air)
        {
            
            switch (p.ThreeDimRotationalEffect)
            {
                case ThreeDimRotationalEffect.None:
                    LogHelper.WriteLog("Running AeroL.Three-D Rotationa Effect ", show_title: false, color: ConsoleColor.Blue);
                    return new OpenWECD.AeroL.Airfoil._3DRotationalEffect.None(air);
                case ThreeDimRotationalEffect.Snel:
                    return new OpenWECD.AeroL.Airfoil._3DRotationalEffect.Snel(air);
                default:
                    LogHelper.ErrorLog(p.DyStallMode + ",No such Three-D Rotationa Effect Modol", FunctionName: "AER_INIThreeDRotationaEffect");
                    return new OpenWECD.AeroL.Airfoil._3DRotationalEffect.None(air);
            }
        }
        /// <summary>
        /// 初始化输出结果的内存占用，由OpenWECD.Tool 生成代码，参数众多，修改困难，因此非人力不可及也！！！
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static AER_AllOuts AER_INIAllOuts(AER_ParameterType p)
        {
            int K = p.NumBl;
            int J = p.BldNodes;
            int J1 = p.TwrNodes;
            AER_AllOuts allout = new AER_AllOuts();
            /// <summary>
            /// Azimuth angle of blade  nan unit:(deg)
            /// </summary>
            allout.BAzimuth = new double[K];
            /// <summary>
            /// Pitch angle of blade  nan unit:(deg)
            /// </summary>
            allout.BPitch = new double[K];
            /// <summary>
            /// x-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVUndx = new double[J, K];
            /// <summary>
            /// y-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVUndy = new double[J, K];
            /// <summary>
            /// z-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVUndz = new double[J, K];
            /// <summary>
            /// x-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVDisx = new double[J, K];
            /// <summary>
            /// y-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVDisy = new double[J, K];
            /// <summary>
            /// z-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjVDisz = new double[J, K];
            /// <summary>
            /// x-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjSTVx = new double[J, K];
            /// <summary>
            /// y-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjSTVy = new double[J, K];
            /// <summary>
            /// z-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            allout.BkNjSTVz = new double[J, K];
            /// <summary>
            /// Relvative wind speed at Blade k, Node j  nan unit:(m/s)
            /// </summary>
            allout.BkNjVRel = new double[J, K];
            /// <summary>
            /// Dynamic pressure at Blade k, Node j  nan unit:(Pa)
            /// </summary>
            allout.BkNjDynP = new double[J, K];
            /// <summary>
            /// Reynolds number (in millions) at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjRe = new double[J, K];
            /// <summary>
            /// Mach number at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjMachN = new double[J, K];
            /// <summary>
            /// Axial induced wind velocity at Blade k, Node j  nan unit:(m/s)
            /// </summary>
            allout.BkNjVindx = new double[J, K];
            /// <summary>
            /// Tangential induced wind velocity at Blade k, Node j  nan unit:(m/s)
            /// </summary>
            allout.BkNjVindy = new double[J, K];
            /// <summary>
            /// Axial induction factor at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjAxInd = new double[J, K];
            /// <summary>
            /// Tangential induction factor at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjTnInd = new double[J, K];
            /// <summary>
            /// Angle of attack at Blade k, Node j  nan unit:(deg)
            /// </summary>
            allout.BkNjAlpha = new double[J, K];
            /// <summary>
            /// Pitch+Twist angle at Blade k, Node j  nan unit:(deg)
            /// </summary>
            allout.BkNjTheta = new double[J, K];
            /// <summary>
            /// Inflow angle at Blade k, Node j  nan unit:(deg)
            /// </summary>
            allout.BkNjPhi = new double[J, K];
            /// <summary>
            /// Curvature angle at Blade k, Node j  nan unit:(deg)
            /// </summary>
            allout.BkNjCurve = new double[J, K];
            /// <summary>
            /// Lift force coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCl = new double[J, K];
            /// <summary>
            /// Drag force coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCd = new double[J, K];
            /// <summary>
            /// Pitching moment coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCm = new double[J, K];
            /// <summary>
            /// Normal force (to plane) coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCx = new double[J, K];
            /// <summary>
            /// Tangential force (to plane) coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCy = new double[J, K];
            /// <summary>
            /// Normal force (to chord) coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCn = new double[J, K];
            /// <summary>
            /// Tangential force (to chord) coefficient at Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCt = new double[J, K];
            /// <summary>
            /// Pressure coefficient Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjCpmin = new double[J, K];
            /// <summary>
            /// Critical cavitation number Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjSigCr = new double[J, K];
            /// <summary>
            /// Cavitation number Blade k, Node j  nan unit:(-)
            /// </summary>
            allout.BkNjSgCav = new double[J, K];
            /// <summary>
            /// Circulation on blade k at node j  nan unit:(m^2/s)
            /// </summary>
            allout.BkNjGam = new double[J, K];
            /// <summary>
            /// Lift force per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFl = new double[J, K];
            /// <summary>
            /// Drag force per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFd = new double[J, K];
            /// <summary>
            /// Pitching moment per unit length at Blade k, Node j  nan unit:(N-m/m)
            /// </summary>
            allout.BkNjMm = new double[J, K];
            /// <summary>
            /// Normal force (to plane) per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFx = new double[J, K];
            /// <summary>
            /// Tangential force (to plane) per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFy = new double[J, K];
            /// <summary>
            /// Normal force (to chord) per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFn = new double[J, K];
            /// <summary>
            /// Tangential force (to chord) per unit length at Blade k, Node j  nan unit:(N/m)
            /// </summary>
            allout.BkNjFt = new double[J, K];
            /// <summary>
            /// Tower clearance at Blade k, Node j (based on the absolute distance to the nearest point in the tower from B1N1 minus the local tower radius, in the deflected configuration); please note that this clearance is only approximate because the calculation assumes that the blade is a line with no volume (however, the calculation does use the local tower radius); when B1N1 is above the tower top (or below the tower base), the absolute distance to the tower top (or base) minus the local tower radius, in the deflected configuration, is output  nan unit:(m)
            /// </summary>
            allout.BkNjClrnc = new double[J, K];
            /// <summary>
            /// Undisturbed x-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjVUndx = new double[J1];
            /// <summary>
            /// Undisturbed y-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjVUndy = new double[J1];
            /// <summary>
            /// Undisturbed z-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjVUndz = new double[J1];
            /// <summary>
            /// Structural translational velocity x-component at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjSTVx = new double[J1];
            /// <summary>
            /// Structural translational velocity y-component at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjSTVy = new double[J1];
            /// <summary>
            /// Structural translational velocity z-component at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            allout.TwHjSTVz = new double[J1];
            /// <summary>
            /// Relative wind speed at Tw node 1  nan unit:(m/s)
            /// </summary>
            allout.TwHjVrel = new double[J1];
            /// <summary>
            /// Dynamic Pressure at Tw node 1  nan unit:(Pa)
            /// </summary>
            allout.TwHjDynP = new double[J1];
            /// <summary>
            /// Reynolds number (in millions) at Tw node 1  nan unit:(-)
            /// </summary>
            allout.TwHjRe = new double[J1];
            /// <summary>
            /// Mach number at Tw node 1  nan unit:(-)
            /// </summary>
            allout.TwHjM = new double[J1];
            /// <summary>
            /// x-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
            /// </summary>
            allout.TwHjFdx = new double[J1];
            /// <summary>
            /// y-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
            /// </summary>
            allout.TwHjFdy = new double[J1];

            return allout;
        }


        /// <summary>
        /// 初始化气动力接口
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static CallDynamicAeroLoad AER_INIDynamicAeroLoad(AER_ParameterType p, MBD_ParameterType mp, WND_RtHndSideType wndrths)
        {
            Lic.INI_moduldata("AeroL");
            LogHelper.WriteLog("Running AeroL.Load ", show_title: false, color: ConsoleColor.Blue);
            switch (p.wakeMode)
            {
                case WakeMode.None:
                    LogHelper.ErrorLog("AeroL.error! Cant set to 0!", FunctionName: "AER_INIDynamicAeroLoad");
                    return new AeroL.BEMT.UnSteadyBEMT.DySteadyBEMT(p,mp,wndrths);
                case WakeMode.SBEMT:
                    return new AeroL.BEMT.UnSteadyBEMT.DySteadyBEMT(p, mp, wndrths);
                case WakeMode.DBEMT:
                    return new AeroL.BEMT.UnSteadyBEMT.UnSteadyBEMT(p, mp, wndrths);
                case WakeMode.FreeWake:
                    return new AeroL.FVW.FreeVorteWake(p, mp, wndrths);
                case WakeMode.SBEMT_UnifiedMomentunModel:
                    return new AeroL.Unified_momentum_model.DySteadyBEM(p, mp, wndrths);
                case WakeMode.DBEMT_UnifiedMomentunModel:
                    return new AeroL.Unified_momentum_model.UnSteadyBEMT(p, mp, wndrths);
                default:
                    LogHelper.ErrorLog("AeroL.error! Cant run ! ", FunctionName: "AER_INIDynamicAeroLoad");
                    return new AeroL.BEMT.UnSteadyBEMT.DySteadyBEMT(p, mp, wndrths);
                    break;
            }
        }


        /// <summary>
        /// 初始化翼型,对翼型进行插值
        /// </summary>
        /// <param name="air"></param>
        /// <param name="bldgeo"></param>
        public static void AER_INIAirFoilInterp(ref Airfoil1 air, BladeGeometry1 bldgeo)
        {
            //# 1、对基本的插值插值进行初始化
            air.IterCl = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];
            air.IterCd = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];
            air.IterCm = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];
            air.IterC4 = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];
            air.IterC5 = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];
            air.IterC6 = new MathNet.Numerics.Interpolation.IInterpolation[bldgeo.NumBladeSection];

            //# 2、对Airfoil当中的数据进行归攻角，即攻角一致，以便于插值
            // note:该部分有以下假设：
            // 1:所有翼型按照最薄翼型的攻角进行插值
            // 2:翼型的插值采用样条插值
            List<airfoil__temp> airfoil = new List<airfoil__temp>();
            for (int i = 0; i < air.Nfoil - 1; i++)//最后一个厚度不管
            {
                var ar = air[i];
                var res = zeros(air[air.Nfoil - 1].DataSet);
                res[':', 0] = air[air.Nfoil - 1].DataSet[':', 0];//赋值攻角
                for (int j = 1; j < air[air.Nfoil - 1].DataSet.ColumnCount; j++)
                {
                    res[':', j] = InterpolateHelper.Inter1D(air[i].DataSet[':', 0], air[i].DataSet[':', j], res[':', 0]);
                }

                ar.DataSet = res;
                airfoil.Add(ar);
            }
            airfoil.Add(air[air.Nfoil - 1]);
            air.list = airfoil;

            var DataSet = air.SecDataSet = InterpolateHelper.Inter1D(air.Thickness, air.list.Select(x => x.DataSet).ToArray(), bldgeo.BlThickness);


            for (int i = 0; i < bldgeo.NumBladeSection; i++)
            {

                // 创建翼型当中的插值
                if (air.InterpOrd == 1)//  airfoil__Temp.InterpOrd == 1)//线性插值
                {

                    air.IterCl[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                    air.IterCd[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(2));
                    air.IterCm[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(3));
                    //air[i].IterC4 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                    //air[i].IterC5 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                    //air[i].IterC6 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));

                }
                else if (air.InterpOrd == 2)// air[i].InterpOrd == 2样条插值
                {
                    if (DataSet[i].Column(0).Count < 5)
                    {
                        air.IterCl[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                        air.IterCd[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(2));
                        air.IterCm[i] = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(3));
                        //air[i].IterC4 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                        //air[i].IterC5 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                        //air[i].IterC6 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                    }
                    else
                    {
                        air.IterCl[i] = CubicSpline.InterpolateAkimaSorted(DataSet[i].Column(0).ToArray(), DataSet[i].Column(1).ToArray());
                        air.IterCd[i] = CubicSpline.InterpolateAkimaSorted(DataSet[i].Column(0).ToArray(), DataSet[i].Column(2).ToArray());
                        air.IterCm[i] = CubicSpline.InterpolateAkimaSorted(DataSet[i].Column(0).ToArray(), DataSet[i].Column(3).ToArray());
                        //air[i].IterC4 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                        //air[i].IterC5 = LinearSpline.Interpolate(DataSet[i].Column(0), DataSet[i].Column(1));
                        //airfoil__Temp.IterC6 = LinearSpline.Interpolate(airfoil__Temp.DataSet.Column(0), airfoil__Temp.DataSet.Column(1));
                    }
                }
                else
                {
                    LogHelper.ErrorLog("未知插值", FunctionName: "ReadAeroLAirfoil");
                }
            }

            //# 其次
        }


        #region Cp计算初始化
        /// <summary>
        /// Cp计算
        /// </summary>
        /// <param name="aeroL"></param>
        public static void AL_INISteadyBEMCp(AeroL1 aeroL)
        {
            LogHelper.WriteLog("Run Blade static Cp Solve！", title: "[success]");
            var p = AER_INIParameterType(aeroL);
            var rths = AER_INIRtHndSideType(p);
            var res = new SteadyBEM(aeroL).CalculateCp(aeroL, aeroL.Minlamda, aeroL.Maxlamda, aeroL.lamdaStep, ref rths, p.Airfoil, aeroL.HubRad, aeroL.MinPitch, aeroL.MaxPitch, aeroL.PitchStep, aeroL.AirDens, aeroL.Bldnum);                //Console.WriteLine(res);
            OutFile outFile = new OutFile(aeroL.CpResultFilePath);
            string[] rowtitle = MathHelper.Range(aeroL.MinPitch, aeroL.MaxPitch, step: aeroL.PitchStep).Select(x => x.ToString()).ToArray();
            string[] columtitle = MathHelper.Range(aeroL.Minlamda, aeroL.Maxlamda, step: aeroL.lamdaStep).Select(x => x.ToString()).ToArray();
            outFile.WriteLine(Otherhelper.ConvertMatrixTitleToOutfile("Tsr[-]\\Cp[-]\\pitch[rad]", rowtitle, columtitle, res));
        }

        #endregion Cp计算初始化



        #region SteadyCurve 计算初始化

        /// <summary>
        /// 计算叶片的静态功率曲线
        /// </summary>
        /// <param name="aeroL"></param>
        public static void AL_INISteadyPowerCurve(AeroL1 aeroL)
        {
            LogHelper.WriteLog("Run the power, thrust etc. curves for a variable pitch blade.", title: "[success]");
            var p = AER_INIParameterType(aeroL);
            var rths = AER_INIRtHndSideType(p);
            T_ALStp_pow stp_Pow = new T_ALStp_pow(aeroL.MinWindSpeed, aeroL.MaxWindSpeed, aeroL.WindSpeedStep, aeroL.orig_pit, aeroL.ωmin, aeroL.opt_KW, aeroL.opt_rpm_rad, aeroL.η, aeroL.pitch_up, aeroL.pitch_down, aeroL.ifpitch, aeroL.Fixed_pitch, aeroL.Fixed_rotationalspeed);
            //SteadyBEM.INISteadyBEM(aeroL);
            var res = new SteadyBEM(aeroL).SteadyPowerCurveFunction(stp_Pow, ref rths, p.Airfoil, aeroL.HubRad, aeroL.AirDens, aeroL.Bldnum, true);
            //Console.WriteLine(res);
            OutFile outFile = new OutFile(aeroL.PowerCurveResultFilePath);
            string[] rowtitle = { "风速v[m/s]", "风轮转速[rpm/min]", "叶尖速比λ[-]", "变桨角θ[rad]", "Cp[-]", "Ct[-]", "P_out[kw]" };
            string[] columtitle = MathHelper.Range(aeroL.MinWindSpeed, aeroL.MaxWindSpeed, step: aeroL.WindSpeedStep).Select(x => x.ToString()).ToArray();
            outFile.WriteLine(Otherhelper.ConvertMatrixTitleToOutfile("WindSpeed[m/s]/inf", rowtitle, columtitle, res));

        }

        public static void AL_INIStradyParkedLoads(AeroL1 aeroL)
        {
            LogHelper.WriteLog("Run the StradyParkedLoads etc. for a variable pitch blade.", title: "[success]");
            var rths = AER_INIRtHndSideType(AER_INIParameterType(aeroL));

        }
        #endregion SteadyCurve 计算初始化
    }
}
