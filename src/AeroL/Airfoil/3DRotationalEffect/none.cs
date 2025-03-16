


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

namespace OpenWECD.AeroL.Airfoil._3DRotationalEffect
{
    public class None : I_ALCal3DRotationalEffect
    {
        private Airfoil1 airfoil;
        private Vector<double> cl_0;
        private Vector<double> cla;
        public None(Airfoil1 air)
        {
            //LogHelper.WriteLog("Run Snel Three-D Rotationa Effect Model to Solve！", title: "[success]", leval: 1);
            //Debug.WriteLine("AirFoilINI com");

            ////#依据叶片翼型计算需要的参数
            //for (int i = 0; i < airfoil.SecDataSet.Length; i++)
            //{
            //    cl_0[i] = AeroL_Airfoil_Subs.AOA_CL_0(air.SecDataSet[i].Column(0), air.SecDataSet[i].Column(1));
            //    cla[i] = AeroL_Airfoil_Subs.AOA_CL_k(air.SecDataSet[i].Column(0), air.SecDataSet[i].Column(1), cl_0[i]);
            //}

        }

        public (double cl, double cd, double cm) LiftDragCoeffInterp(double t, double dt, int nstep, int FoilNum,
            double AOA, ref T_ALAeroBladeElement rths)
        {
            return (rths.DCl, rths.DCd, rths.DCm);
        }
    }
}
