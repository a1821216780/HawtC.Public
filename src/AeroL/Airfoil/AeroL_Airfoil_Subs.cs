

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
using OpenWECD.IO.math;
using System.Linq;
using static System.Math;

namespace OpenWECD.AeroL.Airfoil
{
    public static class AeroL_Airfoil_Subs
    {
        /// <summary>
        /// 这个函数用来寻找一个零升力迎角,HawtC当中统一为弧度制
        /// </summary>
        /// <returns></returns>
        public static double AOA_CL_0(Vector<double> AOA, Vector<double> Cl)
        {
            (var roots, var sus) = RootsHelper.fzero(AOA, Cl);
            if (sus == false)
            {
                return 0;
            }
            var abs = roots.Select(x => Abs(x)).ToList();
            return roots[abs.IndexOf(abs.Min())];
        }

        /// <summary>
        /// 先计算附着流线性区域的升力线斜率 Cl,k
        /// </summary>
        /// <param name="AOA"></param>
        /// <param name="Cl"></param>
        /// <returns></returns>
        public static double AOA_CL_k(Vector<double> AOA, Vector<double> Cl,double cl0)
        {
            
            double Cl_a = 0;
            List<double> Var1 = new List<double>();
            for (int j = 0; j < AOA.Count; j++)
            {
                Var1.Add(Cl[j] / (AOA[j] - cl0));
            }
            Cl_a = Var1.Max();
            return Cl_a;
        }
    }
}
