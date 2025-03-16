//! 自动生成 AeroL 非定常与定常空气动力学模块的有效输出变量数量为:72
//**********************************************************************************************************************************
// LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,ZZZ
//  
//    This file is part of OpenWECD.HawtC.AeroL_OutputParam by 赵子祯, 2021, 2025
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
// 该代码由赵子祯编写的代码生成器生成,不可擅自修改!任何bug请联系赵子祯 1821216780@qq.com
//**********************************************************************************************************************************
using System.Collections.Generic;
using System.Collections.Frozen;
using OpenWECD.IO.Log;
using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.Interface1;
namespace OpenWECD.AeroL
{
        /// <summary>
        /// MBD输出参数类
        /// </summary>
    public class AeroL_OutputParam
    {
        /// <summary>
        /// 输出参数的参数表,用来保存变量和单位信息
        /// </summary>
        public static FrozenDictionary<string, string> AER_OutParUnit = new Dictionary<string, string>()
        {
            {"BAzimuth  ","(deg)     "},
            {"BPitch    ","(deg)     "},            {"BkNjVUndx ","(m/s)     "},            {"BkNjVUndy ","(m/s)     "},            {"BkNjVUndz ","(m/s)     "},
            {"BkNjVDisx ","(m/s)     "},            {"BkNjVDisy ","(m/s)     "},            {"BkNjVDisz ","(m/s)     "},            {"BkNjSTVx  ","(m/s)     "},
            {"BkNjSTVy  ","(m/s)     "},            {"BkNjSTVz  ","(m/s)     "},            {"BkNjVRel  ","(m/s)     "},            {"BkNjDynP  ","(Pa)      "},
            {"BkNjRe    ","(-)       "},            {"BkNjMachN ","(-)       "},            {"BkNjVindx ","(m/s)     "},            {"BkNjVindy ","(m/s)     "},
            {"BkNjAxInd ","(-)       "},            {"BkNjTnInd ","(-)       "},            {"BkNjAlpha ","(deg)     "},            {"BkNjTheta ","(deg)     "},
            {"BkNjPhi   ","(deg)     "},            {"BkNjCurve ","(deg)     "},            {"BkNjCl    ","(-)       "},            {"BkNjCd    ","(-)       "},
            {"BkNjCm    ","(-)       "},            {"BkNjCx    ","(-)       "},            {"BkNjCy    ","(-)       "},            {"BkNjCn    ","(-)       "},
            {"BkNjCt    ","(-)       "},            {"BkNjCpmin ","(-)       "},            {"BkNjSigCr ","(-)       "},            {"BkNjSgCav ","(-)       "},
            {"BkNjGam   ","(m^2/s)   "},            {"BkNjFl    ","(N/m)     "},            {"BkNjFd    ","(N/m)     "},            {"BkNjMm    ","(N-m/m)   "},
            {"BkNjFx    ","(N/m)     "},            {"BkNjFy    ","(N/m)     "},            {"BkNjFn    ","(N/m)     "},            {"BkNjFt    ","(N/m)     "},
            {"BkNjClrnc ","(m)       "},            {"RtSpeed   ","(rpm)     "},            {"RtTSR     ","(-)       "},            {"RtVAvgxh  ","(m/s)     "},
            {"RtVAvgyh  ","(m/s)     "},            {"RtVAvgzh  ","(m/s)     "},            {"RtSkew    ","(deg)     "},            {"RtAeroFxh ","(N)       "},
            {"RtAeroFyh ","(N)       "},            {"RtAeroFzh ","(N)       "},            {"RtAeroMxh ","(N-m)     "},            {"RtAeroMyh ","(N-m)     "},
            {"RtAeroMzh ","(N-m)     "},            {"RtAeroPwr ","(KW)      "},            {"RtArea    ","(m^2)     "},            {"RtAeroCp  ","(-)       "},
            {"RtAeroCq  ","(-)       "},            {"RtAeroCt  ","(-)       "},            {"DBEMTau1  ","(s)       "},            {"TwHjVUndx ","(m/s)     "},
            {"TwHjVUndy ","(m/s)     "},            {"TwHjVUndz ","(m/s)     "},            {"TwHjSTVx  ","(m/s)     "},            {"TwHjSTVy  ","(m/s)     "},
            {"TwHjSTVz  ","(m/s)     "},            {"TwHjVrel  ","(m/s)     "},            {"TwHjDynP  ","(Pa)      "},            {"TwHjRe    ","(-)       "},
            {"TwHjM     ","(-)       "},            {"TwHjFdx   ","(N/m)     "},            {"TwHjFdy   ","(N/m)     "},        }.ToFrozenDictionary();
        /// <summary>
        /// 输出参数的通道表,用来保存变量和通道信息
        /// </summary>
        public static FrozenDictionary<string, int> AER_OutParChannel = new Dictionary<string, int>()
        {
            {"BAzimuth  ",0         },
            {"BPitch    ",0         },            {"BkNjVUndx ",0         },            {"BkNjVUndy ",0         },            {"BkNjVUndz ",0         },
            {"BkNjVDisx ",0         },            {"BkNjVDisy ",0         },            {"BkNjVDisz ",0         },            {"BkNjSTVx  ",0         },
            {"BkNjSTVy  ",0         },            {"BkNjSTVz  ",0         },            {"BkNjVRel  ",1         },            {"BkNjDynP  ",1         },
            {"BkNjRe    ",1         },            {"BkNjMachN ",1         },            {"BkNjVindx ",1         },            {"BkNjVindy ",1         },
            {"BkNjAxInd ",1         },            {"BkNjTnInd ",1         },            {"BkNjAlpha ",1         },            {"BkNjTheta ",1         },
            {"BkNjPhi   ",1         },            {"BkNjCurve ",1         },            {"BkNjCl    ",1         },            {"BkNjCd    ",1         },
            {"BkNjCm    ",1         },            {"BkNjCx    ",1         },            {"BkNjCy    ",1         },            {"BkNjCn    ",1         },
            {"BkNjCt    ",1         },            {"BkNjCpmin ",1         },            {"BkNjSigCr ",1         },            {"BkNjSgCav ",1         },
            {"BkNjGam   ",1         },            {"BkNjFl    ",2         },            {"BkNjFd    ",2         },            {"BkNjMm    ",2         },
            {"BkNjFx    ",2         },            {"BkNjFy    ",2         },            {"BkNjFn    ",2         },            {"BkNjFt    ",2         },
            {"BkNjClrnc ",2         },            {"RtSpeed   ",3         },            {"RtTSR     ",3         },            {"RtVAvgxh  ",3         },
            {"RtVAvgyh  ",3         },            {"RtVAvgzh  ",3         },            {"RtSkew    ",3         },            {"RtAeroFxh ",3         },
            {"RtAeroFyh ",3         },            {"RtAeroFzh ",3         },            {"RtAeroMxh ",3         },            {"RtAeroMyh ",3         },
            {"RtAeroMzh ",3         },            {"RtAeroPwr ",3         },            {"RtArea    ",3         },            {"RtAeroCp  ",3         },
            {"RtAeroCq  ",3         },            {"RtAeroCt  ",3         },            {"DBEMTau1  ",3         },            {"TwHjVUndx ",4         },
            {"TwHjVUndy ",4         },            {"TwHjVUndz ",4         },            {"TwHjSTVx  ",4         },            {"TwHjSTVy  ",4         },
            {"TwHjSTVz  ",4         },            {"TwHjVrel  ",4         },            {"TwHjDynP  ",4         },            {"TwHjRe    ",4         },
            {"TwHjM     ",4         },            {"TwHjFdx   ",4         },            {"TwHjFdy   ",4         },        }.ToFrozenDictionary();
        /// <summary>
        /// 输出参数的维度 0 表示没有维度 1 表示只要部件编号 2 表示只要部件截面 3 表示部件编号和截面编号都要
        /// </summary>
        public static FrozenDictionary<string, int> AER_OutParDim = new Dictionary<string, int>()
        {
            {"BAzimuth  ",1},
            {"BPitch    ",1},            {"BkNjVUndx ",3},            {"BkNjVUndy ",3},            {"BkNjVUndz ",3},
            {"BkNjVDisx ",3},            {"BkNjVDisy ",3},            {"BkNjVDisz ",3},            {"BkNjSTVx  ",3},
            {"BkNjSTVy  ",3},            {"BkNjSTVz  ",3},            {"BkNjVRel  ",3},            {"BkNjDynP  ",3},
            {"BkNjRe    ",3},            {"BkNjMachN ",3},            {"BkNjVindx ",3},            {"BkNjVindy ",3},
            {"BkNjAxInd ",3},            {"BkNjTnInd ",3},            {"BkNjAlpha ",3},            {"BkNjTheta ",3},
            {"BkNjPhi   ",3},            {"BkNjCurve ",3},            {"BkNjCl    ",3},            {"BkNjCd    ",3},
            {"BkNjCm    ",3},            {"BkNjCx    ",3},            {"BkNjCy    ",3},            {"BkNjCn    ",3},
            {"BkNjCt    ",3},            {"BkNjCpmin ",3},            {"BkNjSigCr ",3},            {"BkNjSgCav ",3},
            {"BkNjGam   ",3},            {"BkNjFl    ",3},            {"BkNjFd    ",3},            {"BkNjMm    ",3},
            {"BkNjFx    ",3},            {"BkNjFy    ",3},            {"BkNjFn    ",3},            {"BkNjFt    ",3},
            {"BkNjClrnc ",3},            {"RtSpeed   ",0},            {"RtTSR     ",0},            {"RtVAvgxh  ",0},
            {"RtVAvgyh  ",0},            {"RtVAvgzh  ",0},            {"RtSkew    ",0},            {"RtAeroFxh ",0},
            {"RtAeroFyh ",0},            {"RtAeroFzh ",0},            {"RtAeroMxh ",0},            {"RtAeroMyh ",0},
            {"RtAeroMzh ",0},            {"RtAeroPwr ",0},            {"RtArea    ",0},            {"RtAeroCp  ",0},
            {"RtAeroCq  ",0},            {"RtAeroCt  ",0},            {"DBEMTau1  ",0},            {"TwHjVUndx ",2},
            {"TwHjVUndy ",2},            {"TwHjVUndz ",2},            {"TwHjSTVx  ",2},            {"TwHjSTVy  ",2},
            {"TwHjSTVz  ",2},            {"TwHjVrel  ",2},            {"TwHjDynP  ",2},            {"TwHjRe    ",2},
            {"TwHjM     ",2},            {"TwHjFdx   ",2},            {"TwHjFdy   ",2},        }.ToFrozenDictionary();
        /// <summary>
        /// 依据变量名称,输出变量值
        /// </summary>
        public static double AER_GetParamOutput(string param, int J, AER_AllOuts AllOuts,int K=0)
        {
            switch (param)
            {
                case "BAzimuth  ":
                    return AllOuts.BAzimuth[K];
                case "BPitch    ":
                    return AllOuts.BPitch[K];
                case "BkNjVUndx ":
                    return AllOuts.BkNjVUndx[J,K];
                case "BkNjVUndy ":
                    return AllOuts.BkNjVUndy[J,K];
                case "BkNjVUndz ":
                    return AllOuts.BkNjVUndz[J,K];
                case "BkNjVDisx ":
                    return AllOuts.BkNjVDisx[J,K];
                case "BkNjVDisy ":
                    return AllOuts.BkNjVDisy[J,K];
                case "BkNjVDisz ":
                    return AllOuts.BkNjVDisz[J,K];
                case "BkNjSTVx  ":
                    return AllOuts.BkNjSTVx[J,K];
                case "BkNjSTVy  ":
                    return AllOuts.BkNjSTVy[J,K];
                case "BkNjSTVz  ":
                    return AllOuts.BkNjSTVz[J,K];
                case "BkNjVRel  ":
                    return AllOuts.BkNjVRel[J,K];
                case "BkNjDynP  ":
                    return AllOuts.BkNjDynP[J,K];
                case "BkNjRe    ":
                    return AllOuts.BkNjRe[J,K];
                case "BkNjMachN ":
                    return AllOuts.BkNjMachN[J,K];
                case "BkNjVindx ":
                    return AllOuts.BkNjVindx[J,K];
                case "BkNjVindy ":
                    return AllOuts.BkNjVindy[J,K];
                case "BkNjAxInd ":
                    return AllOuts.BkNjAxInd[J,K];
                case "BkNjTnInd ":
                    return AllOuts.BkNjTnInd[J,K];
                case "BkNjAlpha ":
                    return AllOuts.BkNjAlpha[J,K];
                case "BkNjTheta ":
                    return AllOuts.BkNjTheta[J,K];
                case "BkNjPhi   ":
                    return AllOuts.BkNjPhi[J,K];
                case "BkNjCurve ":
                    return AllOuts.BkNjCurve[J,K];
                case "BkNjCl    ":
                    return AllOuts.BkNjCl[J,K];
                case "BkNjCd    ":
                    return AllOuts.BkNjCd[J,K];
                case "BkNjCm    ":
                    return AllOuts.BkNjCm[J,K];
                case "BkNjCx    ":
                    return AllOuts.BkNjCx[J,K];
                case "BkNjCy    ":
                    return AllOuts.BkNjCy[J,K];
                case "BkNjCn    ":
                    return AllOuts.BkNjCn[J,K];
                case "BkNjCt    ":
                    return AllOuts.BkNjCt[J,K];
                case "BkNjCpmin ":
                    return AllOuts.BkNjCpmin[J,K];
                case "BkNjSigCr ":
                    return AllOuts.BkNjSigCr[J,K];
                case "BkNjSgCav ":
                    return AllOuts.BkNjSgCav[J,K];
                case "BkNjGam   ":
                    return AllOuts.BkNjGam[J,K];
                case "BkNjFl    ":
                    return AllOuts.BkNjFl[J,K];
                case "BkNjFd    ":
                    return AllOuts.BkNjFd[J,K];
                case "BkNjMm    ":
                    return AllOuts.BkNjMm[J,K];
                case "BkNjFx    ":
                    return AllOuts.BkNjFx[J,K];
                case "BkNjFy    ":
                    return AllOuts.BkNjFy[J,K];
                case "BkNjFn    ":
                    return AllOuts.BkNjFn[J,K];
                case "BkNjFt    ":
                    return AllOuts.BkNjFt[J,K];
                case "BkNjClrnc ":
                    return AllOuts.BkNjClrnc[J,K];
                case "RtSpeed   ":
                    return AllOuts.RtSpeed;
                case "RtTSR     ":
                    return AllOuts.RtTSR;
                case "RtVAvgxh  ":
                    return AllOuts.RtVAvgxh;
                case "RtVAvgyh  ":
                    return AllOuts.RtVAvgyh;
                case "RtVAvgzh  ":
                    return AllOuts.RtVAvgzh;
                case "RtSkew    ":
                    return AllOuts.RtSkew;
                case "RtAeroFxh ":
                    return AllOuts.RtAeroFxh;
                case "RtAeroFyh ":
                    return AllOuts.RtAeroFyh;
                case "RtAeroFzh ":
                    return AllOuts.RtAeroFzh;
                case "RtAeroMxh ":
                    return AllOuts.RtAeroMxh;
                case "RtAeroMyh ":
                    return AllOuts.RtAeroMyh;
                case "RtAeroMzh ":
                    return AllOuts.RtAeroMzh;
                case "RtAeroPwr ":
                    return AllOuts.RtAeroPwr;
                case "RtArea    ":
                    return AllOuts.RtArea;
                case "RtAeroCp  ":
                    return AllOuts.RtAeroCp;
                case "RtAeroCq  ":
                    return AllOuts.RtAeroCq;
                case "RtAeroCt  ":
                    return AllOuts.RtAeroCt;
                case "DBEMTau1  ":
                    return AllOuts.DBEMTau1;
                case "TwHjVUndx ":
                    return AllOuts.TwHjVUndx[J];
                case "TwHjVUndy ":
                    return AllOuts.TwHjVUndy[J];
                case "TwHjVUndz ":
                    return AllOuts.TwHjVUndz[J];
                case "TwHjSTVx  ":
                    return AllOuts.TwHjSTVx[J];
                case "TwHjSTVy  ":
                    return AllOuts.TwHjSTVy[J];
                case "TwHjSTVz  ":
                    return AllOuts.TwHjSTVz[J];
                case "TwHjVrel  ":
                    return AllOuts.TwHjVrel[J];
                case "TwHjDynP  ":
                    return AllOuts.TwHjDynP[J];
                case "TwHjRe    ":
                    return AllOuts.TwHjRe[J];
                case "TwHjM     ":
                    return AllOuts.TwHjM[J];
                case "TwHjFdx   ":
                    return AllOuts.TwHjFdx[J];
                case "TwHjFdy   ":
                    return AllOuts.TwHjFdy[J];

                default:
                    LogHelper.ErrorLog($"{param} not Support!,Please Visit OutputParList or www.openwecd.fun ",FunctionName: "MBD_GetParamOutput");
                    return 0;
            }
        }
        /// <summary>
        /// 阐述变量的结构体
        /// </summary>
        public struct AER_AllOutss
        {
            /// <summary>
            /// 叶片的方位角 Azimuth angle of blade   unit:(deg)
            /// </summary>
            public double [] BAzimuth;
            /// <summary>
            /// 叶片的变桨角Pitch angle of blade   unit:(deg)
            /// </summary>
            public double [] BPitch;
            /// <summary>
            /// 全局坐标系下叶片K的节点J处的非诱导输入风速的x分量 x-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVUndx;
            /// <summary>
            /// 全局坐标系下叶片K的节点J处的非诱导输入风速的y分量 y-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVUndy;
            /// <summary>
            /// 全局坐标系下叶片K的节点J处的非诱导输入风速的z分量 z-component of undisturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVUndz;
            /// <summary>
            /// x-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVDisx;
            /// <summary>
            /// y-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVDisy;
            /// <summary>
            /// z-component of disturbed wind velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjVDisz;
            /// <summary>
            /// x-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjSTVx;
            /// <summary>
            /// y-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjSTVy;
            /// <summary>
            /// z-component of structural translational velocity at Blade k, Node j  local blade coordinate system unit:(m/s)
            /// </summary>
            public double [,] BkNjSTVz;
            /// <summary>
            /// Relvative wind speed at Blade k, Node j   unit:(m/s)
            /// </summary>
            public double [,] BkNjVRel;
            /// <summary>
            /// Dynamic pressure at Blade k, Node j   unit:(Pa)
            /// </summary>
            public double [,] BkNjDynP;
            /// <summary>
            /// Reynolds number (in millions) at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjRe;
            /// <summary>
            /// Mach number at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjMachN;
            /// <summary>
            /// Axial induced wind velocity at Blade k, Node j   unit:(m/s)
            /// </summary>
            public double [,] BkNjVindx;
            /// <summary>
            /// Tangential induced wind velocity at Blade k, Node j   unit:(m/s)
            /// </summary>
            public double [,] BkNjVindy;
            /// <summary>
            /// Axial induction factor at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjAxInd;
            /// <summary>
            /// Tangential induction factor at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjTnInd;
            /// <summary>
            /// Angle of attack at Blade k, Node j   unit:(deg)
            /// </summary>
            public double [,] BkNjAlpha;
            /// <summary>
            /// Pitch+Twist angle at Blade k, Node j   unit:(deg)
            /// </summary>
            public double [,] BkNjTheta;
            /// <summary>
            /// Inflow angle at Blade k, Node j   unit:(deg)
            /// </summary>
            public double [,] BkNjPhi;
            /// <summary>
            /// Curvature angle at Blade k, Node j   unit:(deg)
            /// </summary>
            public double [,] BkNjCurve;
            /// <summary>
            /// Lift force coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCl;
            /// <summary>
            /// Drag force coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCd;
            /// <summary>
            /// Pitching moment coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCm;
            /// <summary>
            /// Normal force (to plane) coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCx;
            /// <summary>
            /// Tangential force (to plane) coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCy;
            /// <summary>
            /// Normal force (to chord) coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCn;
            /// <summary>
            /// Tangential force (to chord) coefficient at Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCt;
            /// <summary>
            /// Pressure coefficient Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjCpmin;
            /// <summary>
            /// Critical cavitation number Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjSigCr;
            /// <summary>
            /// Cavitation number Blade k, Node j   unit:(-)
            /// </summary>
            public double [,] BkNjSgCav;
            /// <summary>
            /// Circulation on blade k at node j   unit:(m^2/s)
            /// </summary>
            public double [,] BkNjGam;
            /// <summary>
            /// Lift force per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFl;
            /// <summary>
            /// Drag force per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFd;
            /// <summary>
            /// Pitching moment per unit length at Blade k, Node j   unit:(N-m/m)
            /// </summary>
            public double [,] BkNjMm;
            /// <summary>
            /// Normal force (to plane) per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFx;
            /// <summary>
            /// Tangential force (to plane) per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFy;
            /// <summary>
            /// Normal force (to chord) per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFn;
            /// <summary>
            /// Tangential force (to chord) per unit length at Blade k, Node j   unit:(N/m)
            /// </summary>
            public double [,] BkNjFt;
            /// <summary>
            /// Tower clearance at Blade k, Node j (based on the absolute distance to the nearest point in the tower from B1N1 minus the local tower radius, in the deflected configuration); please note that this clearance is only approximate because the calculation assumes that the blade is a line with no volume (however, the calculation does use the local tower radius); when B1N1 is above the tower top (or below the tower base), the absolute distance to the tower top (or base) minus the local tower radius, in the deflected configuration, is output   unit:(m)
            /// </summary>
            public double [,] BkNjClrnc;
            /// <summary>
            /// Rotor speed   unit:(rpm)
            /// </summary>
            public double  RtSpeed;
            /// <summary>
            /// Rotor tip-speed ratio   unit:(-)
            /// </summary>
            public double  RtTSR;
            /// <summary>
            /// Rotor-disk-averaged relative wind velocity (x-component)  the hub coordinate system unit:(m/s)
            /// </summary>
            public double  RtVAvgxh;
            /// <summary>
            /// Rotor-disk-averaged relative wind velocity (y-component)  the hub coordinate system unit:(m/s)
            /// </summary>
            public double  RtVAvgyh;
            /// <summary>
            /// Rotor-disk-averaged relative wind velocity (z-component)  the hub coordinate system unit:(m/s)
            /// </summary>
            public double  RtVAvgzh;
            /// <summary>
            /// Rotor inflow-skew angle   unit:(deg)
            /// </summary>
            public double  RtSkew;
            /// <summary>
            /// Total rotor aerodynamic load (force in x direction)  the hub coordinate system unit:(N)
            /// </summary>
            public double  RtAeroFxh;
            /// <summary>
            /// Total rotor aerodynamic load (force in y direction)  the hub coordinate system unit:(N)
            /// </summary>
            public double  RtAeroFyh;
            /// <summary>
            /// Total rotor aerodynamic load (force in z direction)  the hub coordinate system unit:(N)
            /// </summary>
            public double  RtAeroFzh;
            /// <summary>
            /// Total rotor aerodynamic load (moment in x direction)  the hub coordinate system unit:(N-m)
            /// </summary>
            public double  RtAeroMxh;
            /// <summary>
            /// Total rotor aerodynamic load (moment in y direction)  the hub coordinate system unit:(N-m)
            /// </summary>
            public double  RtAeroMyh;
            /// <summary>
            /// Total rotor aerodynamic load (moment in z direction)  the hub coordinate system unit:(N-m)
            /// </summary>
            public double  RtAeroMzh;
            /// <summary>
            /// Rotor aerodynamic power   unit:(KW)
            /// </summary>
            public double  RtAeroPwr;
            /// <summary>
            /// Rotor swept area   unit:(m^2)
            /// </summary>
            public double  RtArea;
            /// <summary>
            /// Rotor aerodynamic power coefficient   unit:(-)
            /// </summary>
            public double  RtAeroCp;
            /// <summary>
            /// Rotor aerodynamic torque coefficient   unit:(-)
            /// </summary>
            public double  RtAeroCq;
            /// <summary>
            /// Rotor aerodynamic thrust coefficient   unit:(-)
            /// </summary>
            public double  RtAeroCt;
            /// <summary>
            /// time-constant used in DBEMT   unit:(s)
            /// </summary>
            public double  DBEMTau1;
            /// <summary>
            /// Undisturbed x-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjVUndx;
            /// <summary>
            /// Undisturbed y-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjVUndy;
            /// <summary>
            /// Undisturbed z-component wind velocity at Tw node 1  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjVUndz;
            /// <summary>
            /// Structural translational velocity x-component at Tw node 1[塔架运动坐标系]  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjSTVx;
            /// <summary>
            /// Structural translational velocity y-component at Tw node 1[塔架运动坐标系]  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjSTVy;
            /// <summary>
            /// Structural translational velocity z-component at Tw node 1[塔架运动坐标系]  local tower coordinate system unit:(m/s)
            /// </summary>
            public double [] TwHjSTVz;
            /// <summary>
            /// Relative wind speed at Tw node 1   unit:(m/s)
            /// </summary>
            public double [] TwHjVrel;
            /// <summary>
            /// Dynamic Pressure at Tw node 1   unit:(Pa)
            /// </summary>
            public double [] TwHjDynP;
            /// <summary>
            /// Reynolds number (in millions) at Tw node 1   unit:(-)
            /// </summary>
            public double [] TwHjRe;
            /// <summary>
            /// Mach number at Tw node 1   unit:(-)
            /// </summary>
            public double [] TwHjM;
            /// <summary>
            /// x-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
            /// </summary>
            public double [] TwHjFdx;
            /// <summary>
            /// y-component of drag force per unit length at Tw node 1  local tower coordinate system unit:(N/m)
            /// </summary>
            public double [] TwHjFdy;
        }

    }
}
