

//**********************************************************************************************************************************
// LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//                                      
//    This file is part of OpenWECD.MBD by 赵子祯, 2021, 2024
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
////#define MBDLATDOUBLE
//#define BDLATMATHNET

using MathNet.Numerics.LinearAlgebra;
using OpenWECD.HawtC2;
using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using System.Runtime;
using System.Runtime.CompilerServices;
using SourceGeneration.Reflection;
using System.Runtime.InteropServices;
using OpenWECD.MBD;




//# 使用Using语句控制类型，转换时要验证双精度或者单精度
#if MBDLATDOUBLE

using Mat = OpenWECD.IO.Numerics.Matrix3S;
using Vec = OpenWECD.IO.Numerics.VecS;
using NUMT = System.Double;

#elif MBDLATFLOAT

using Mat = OpenWECD.IO.Numerics.Matrix3f;
using Vec = System.Numerics.Vector3;
using NUMT = System.Single;

#elif MBDLATMATHNET

using Mat=MathNet.Numerics.LinearAlgebra.Matrix<double>;
using Vec= MathNet.Numerics.LinearAlgebra.Vector<double>;
using NUMT = System.Double;

#endif

namespace OpenWECD.MSAL
{
    internal static class MSA_INI
    {
        public static MSA_AllOuts MSA_INIAllOuts(MBD_ParameterType p)
        {
            var J = p.BldNodes;
            var K = p.NumBl;
            MSA_AllOuts AllOuts = new MSA_AllOuts();
            //case "BkNjADxi  ":
            AllOuts.BkNjADxi = new double[J, K];
            //case "BkNjADyi  ":
            AllOuts.BkNjADyi = new double[J, K];
            //case "BkNjADzi  ":
            AllOuts.BkNjADzi = new double[J, K];
            //case "BldKADxi  ":
            AllOuts.BldKADxi = new double[K];
            //case "BldKADyi  ":
            AllOuts.BldKADyi = new double[K];
            //case "BldKADzi  ":
            AllOuts.BldKADzi = new double[K];
            //case "BkNjADxt  ":
            AllOuts.BkNjADxt = new double[J, K];
            //case "BkNjADyt  ":
            AllOuts.BkNjADyt = new double[J, K];
            //case "BkNjADzt  ":
            AllOuts.BkNjADzt = new double[J, K];
            //case "BldKADxt  ":
            AllOuts.BldKADxt = new double[K];
            //case "BldKADyt  ":
            AllOuts.BldKADyt = new double[K];
            //case "BldKADzt  ":
            AllOuts.BldKADzt = new double[K];
            //case "BkNjAWxi  ":
            AllOuts.BkNjAWxi = new double[J, K];
            //case "BkNjAWyi  ":
            AllOuts.BkNjAWyi = new double[J, K];
            //case "BkNjAWzi  ":
            AllOuts.BkNjAWzi = new double[J, K];
            //case "BldKAWxi  ":
            AllOuts.BldKAWxi = new double[K];
            //case "BldKAWyi  ":
            AllOuts.BldKAWyi = new double[K];
            //case "BldKAWzi  ":
            AllOuts.BldKAWzi = new double[K];
            //case "BkNjAWxt  ":
            AllOuts.BkNjAWxt = new double[J, K];
            //case "BkNjAWyt  ":
            AllOuts.BkNjAWyt = new double[J, K];
            //case "BkNjAWzt  ":
            AllOuts.BkNjAWzt = new double[J, K];
            //case "BldKAWxt  ":
            AllOuts.BldKAWxt = new double[K];
            //case "BldKAWyt  ":
            AllOuts.BldKAWyt = new double[K];
            //case "BldKAWzt  ":
            AllOuts.BldKAWzt = new double[K];
            //case "TwHtADxi  ":
            AllOuts.TwHtADxi = new double[J];
            //case "TwHtADyi  ":
            AllOuts.TwHtADyi = new double[J];
            //case "TwHtADzi  ":
            AllOuts.TwHtADzi = new double[J];
            //case "TwHtADxt  ":
            AllOuts.TwHtADxt = new double[J];
            //case "TwHtADyt  ":
            AllOuts.TwHtADyt = new double[J];
            //case "TwHtADzt  ":
            AllOuts.TwHtADzt = new double[J];
            //case "TwHtAWxi  ":
            AllOuts.TwHtAWxi = new double[J];
            //case "TwHtAWyi  ":
            AllOuts.TwHtAWyi = new double[J];
            //case "TwHtAWzi  ":
            AllOuts.TwHtAWzi = new double[J];
            //case "TwHtAWxt  ":
            AllOuts.TwHtAWxt = new double[J];
            //case "TwHtAWyt  ":
            AllOuts.TwHtAWyt = new double[J];
            //case "TwHtAWzt  ":
            AllOuts.TwHtAWzt = new double[J];

            return AllOuts;
        }

        public static MSA_ParameterType MSA_INIParameterType(MSAL1 MsaL,MBD_ParameterType mp)
        {
            MSA_ParameterType p = new MSA_ParameterType();


            return p;
        }

        public static MSA_RtHndSideType MSA_INIRtHndSideType(MSAL1 MsaL, MSA_ParameterType p)
        {
            MSA_RtHndSideType rths = new MSA_RtHndSideType();


            return rths;
        }
    }
}
