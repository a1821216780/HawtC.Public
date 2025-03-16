


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
using OpenWECD.MBD;
using OpenWECD.WindL;

namespace OpenWECD.AeroL.FVW
{
    public class FreeVorteWake : CallDynamicAeroLoad
    {
        public FreeVorteWake(AER_ParameterType p,MBD_ParameterType mp, WND_RtHndSideType wndrths)
        {
            LogHelper.ErrorLog("AeroL.error! Cant run FVW ,because 赵子祯 no time to do that!",FunctionName: "FreeVorteWake(AER_ParameterType p)");
        }
        public void AL_CalDynamicAeroLoad(double t, double dt, ref MBD_InputLoad u, double q_aim, double rotorSpeeed, ref MBD_OtherStateType OtherState, 
            in MBD_RtHndSideType RtHS, in MBD_CoordSysType CoordSys, in WND_RtHndSideType WND_rths, in Vector<double> Pitch, in AER_ParameterType p,
            in MBD_ParameterType mp, ref AER_RtHndSideType rths, in T_ALAeroBladeElement[,] SBldEle)
        {
            LogHelper.ErrorLog("AeroL.error! Cant run FVW ,because 赵子祯 no time to do that!",FunctionName: "FreeVorteWake.AL_CalDynamicAeroLoad");
        }
    }
}
