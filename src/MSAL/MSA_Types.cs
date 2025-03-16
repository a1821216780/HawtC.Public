

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
    /// <remarks>
    /// 所有的MSAL当中的参数
    /// </remarks>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct MSAL1
    {
        public string MSALfilepath;

        public string[] MSALData;

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
    /// MSAL当中的参数结构体
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct MSA_ParameterType
    {

    }
    /// <summary>
    /// MSAL当中的运行时参数结构体
    /// </summary>
    [SourceReflection]
    [StructLayout(LayoutKind.Sequential)]
    public struct MSA_RtHndSideType
    {
        public MSA_IO_Outs MSA_IO_Out;
    }
    /// <summary>
    /// 
    /// </summary>
    public struct MSA_AllOuts
    {
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADzi;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADzi;
        /// <summary>
        /// 叶片局部坐标系下，沿x方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADxt;
        /// <summary>
        /// 叶片局部坐标系下，沿y方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADyt;
        /// <summary>
        /// 叶片局部坐标系下，沿z方向上的叶片K节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[,] BkNjADzt;
        /// <summary>
        /// 叶片局部坐标系下，沿x方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADxt;
        /// <summary>
        /// 叶片局部坐标系下，沿y方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADyt;
        /// <summary>
        /// 叶片局部坐标系下，沿z方向上的叶片K的气动阻尼比   unit:(%)
        /// </summary>
        public double[] BldKADzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWzi;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWzi;
        /// <summary>
        /// 叶片局部坐标系下，沿x方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWxt;
        /// <summary>
        /// 叶片局部坐标系下，沿y方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWyt;
        /// <summary>
        /// 叶片局部坐标系下，沿z方向上的叶片K节点J的气动功   unit:(Kw)
        /// </summary>
        public double[,] BkNjAWzt;
        /// <summary>
        /// 叶片局部坐标系下，沿x方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWxt;
        /// <summary>
        /// 叶片局部坐标系下，沿y方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWyt;
        /// <summary>
        /// 叶片局部坐标系下，沿z方向上的叶片K的气动功   unit:(Kw)
        /// </summary>
        public double[] BldKAWzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADzi;
        /// <summary>
        /// 塔架局部坐标系下，沿x方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADxt;
        /// <summary>
        /// 塔架局部坐标系下，沿y方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADyt;
        /// <summary>
        /// 塔架局部坐标系下，沿z方向上的塔架节点J的气动阻尼比   unit:(%)
        /// </summary>
        public double[] TwHtADzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADzi;
        /// <summary>
        /// 塔架局部坐标系下，沿x方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADxt;
        /// <summary>
        /// 塔架局部坐标系下，沿y方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADyt;
        /// <summary>
        /// 塔架局部坐标系下，沿z方向上的塔架气动阻尼比   unit:(%)
        /// </summary>
        public double TwrADzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWzi;
        /// <summary>
        /// 塔架局部坐标系下，沿x方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWxt;
        /// <summary>
        /// 塔架局部坐标系下，沿y方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWyt;
        /// <summary>
        /// 塔架局部坐标系下，沿z方向上的塔架节点J的气动功   unit:(Kw)
        /// </summary>
        public double[] TwHtAWzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWzi;
        /// <summary>
        /// 塔架局部坐标系下，沿x方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWxt;
        /// <summary>
        /// 塔架局部坐标系下，沿y方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWyt;
        /// <summary>
        /// 塔架局部坐标系下，沿z方向上的塔架气动功   unit:(Kw)
        /// </summary>
        public double TwrAWzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADzi;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿x方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADxt;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿y方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADyt;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿z方向上的风轮气动阻尼比   unit:(%)
        /// </summary>
        public double RtADzt;
        /// <summary>
        /// 全局惯性坐标系下，沿x方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWxi;
        /// <summary>
        /// 全局惯性坐标系下，沿y方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWyi;
        /// <summary>
        /// 全局惯性坐标系下，沿z方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWzi;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿x方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWxt;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿y方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWyt;
        /// <summary>
        /// 风轮轴线局部坐标系下，沿z方向上的风轮气动功   unit:(Kw)
        /// </summary>
        public double RtAWzt;
    }
    /// <summary>
    /// 载荷输出的通道枚举体,要和c#自动化文件对应
    /// </summary>
    [SourceReflection]
    public enum MSA_Loadchannels
    {
        /// <summary>
        /// 叶片和叶片节点上的气动功与气动阻尼比
        /// </summary>
        Blade_Aerodynamic_damping_And_Power = 0,
        /// <summary>
        /// 塔架和塔架节点上的气动功与气动阻尼比
        /// </summary>
        Tower_Aerodynamic_damping_And_Power = 1,
        /// <summary>
        /// 风轮的气动功与气动阻尼比
        /// </summary>
        Rotor_Aerodynamic_damping_And_Power = 2,
    }
}
