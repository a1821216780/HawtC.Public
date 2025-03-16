

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.AeroL.BEMT.UnSteadyBEMT
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


//# 该模块参考了HAWC2的相关论文，并进行了复现。
using MathNet.Numerics.LinearAlgebra;
using OpenWECD.IO.Log;
using OpenWECD.MBD;
using OpenWECD.WindL;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using static OpenWECD.IO.math.InterpolateHelper;
using static System.Math;

namespace OpenWECD.AeroL.Unified_momentum_model
{
    public class UnSteadyBEMT: CallDynamicAeroLoad
    {
        #region 字段
        private int nb_Load;
        private int BldNum;
        private Matrix<double>? px;// = Matrix<double>.Build.Dense(nb_Load, 3);
        private Matrix<double>? py;//= Matrix<double>.Build.Dense(nb_Load, 3);
        private Matrix<double>? Mz;//= Matrix<double>.Build.Dense(nb_Load, 3);
                                   //private Matrix<double>? phi;//= Matrix<double>.Build.Dense(nb_Load, 3);
        #endregion 字段

        public UnSteadyBEMT(AeroL.AER_ParameterType p,MBD_ParameterType mp, WND_RtHndSideType wndrths)
        {
            nb_Load = p.BldNodes;
            BldNum = p.NumBl;
            px = Matrix<double>.Build.Dense(nb_Load, BldNum);
            py = Matrix<double>.Build.Dense(nb_Load, BldNum);
            Mz = Matrix<double>.Build.Dense(nb_Load, BldNum);
            //// phi = Matrix<double>.Build.Dense(nb_Load, BldNum);
            Vector<double> BlSpn = Vector<double>.Build.Dense(nb_Load);
            LogHelper.WriteLog("Run BEMT.UnsteadyBEMT Unified Momentun Model to Solve！", title: "[success]", leval: 1);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public void AL_CalDynamicAeroLoad(double t, double dt, ref MBD_InputLoad u, double q_aim, double rotorSpeeed, ref MBD_OtherStateType OtherState, in MBD_RtHndSideType RtHS, in MBD_CoordSysType CoordSys, in WND_RtHndSideType WND_rths, in Vector<double> Pitch, in AER_ParameterType p, in MBD_ParameterType mp, ref AER_RtHndSideType rths, in T_ALAeroBladeElement[,] SBldEle)
        {
            LogHelper.ErrorLog("AeroL.error! Cant run UMM ,because 赵子祯 no time to do that!");
        }

    }
}
