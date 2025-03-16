//! 自动生成 MSA 模型线性化与稳定求解器的有效输出变量数量为:60
//**********************************************************************************************************************************
// LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,ZZZ
//  
//    This file is part of OpenWECD.HawtC.MSA by 赵子祯, 2021, 2025
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
namespace OpenWECD.MSAL
{
        /// <summary>
        /// MSA输出参数类
        /// </summary>
    public class MSA_OutputParam
    {
        /// <summary>
        /// 输出参数的参数表,用来保存变量和单位信息
        /// </summary>
        public static FrozenDictionary<string, string> MSA_OutParUnit = new Dictionary<string, string>()
        {
            {"BkNjADxi  ","(%)       "},
            {"BkNjADyi  ","(%)       "},            {"BkNjADzi  ","(%)       "},            {"BldKADxi  ","(%)       "},            {"BldKADyi  ","(%)       "},
            {"BldKADzi  ","(%)       "},            {"BkNjADxt  ","(%)       "},            {"BkNjADyt  ","(%)       "},            {"BkNjADzt  ","(%)       "},
            {"BldKADxt  ","(%)       "},            {"BldKADyt  ","(%)       "},            {"BldKADzt  ","(%)       "},            {"BkNjAWxi  ","(Kw)      "},
            {"BkNjAWyi  ","(Kw)      "},            {"BkNjAWzi  ","(Kw)      "},            {"BldKAWxi  ","(Kw)      "},            {"BldKAWyi  ","(Kw)      "},
            {"BldKAWzi  ","(Kw)      "},            {"BkNjAWxt  ","(Kw)      "},            {"BkNjAWyt  ","(Kw)      "},            {"BkNjAWzt  ","(Kw)      "},
            {"BldKAWxt  ","(Kw)      "},            {"BldKAWyt  ","(Kw)      "},            {"BldKAWzt  ","(Kw)      "},            {"TwHtADxi  ","(%)       "},
            {"TwHtADyi  ","(%)       "},            {"TwHtADzi  ","(%)       "},            {"TwHtADxt  ","(%)       "},            {"TwHtADyt  ","(%)       "},
            {"TwHtADzt  ","(%)       "},            {"TwrADxi   ","(%)       "},            {"TwrADyi   ","(%)       "},            {"TwrADzi   ","(%)       "},
            {"TwrADxt   ","(%)       "},            {"TwrADyt   ","(%)       "},            {"TwrADzt   ","(%)       "},            {"TwHtAWxi  ","(Kw)      "},
            {"TwHtAWyi  ","(Kw)      "},            {"TwHtAWzi  ","(Kw)      "},            {"TwHtAWxt  ","(Kw)      "},            {"TwHtAWyt  ","(Kw)      "},
            {"TwHtAWzt  ","(Kw)      "},            {"TwrAWxi   ","(Kw)      "},            {"TwrAWyi   ","(Kw)      "},            {"TwrAWzi   ","(Kw)      "},
            {"TwrAWxt   ","(Kw)      "},            {"TwrAWyt   ","(Kw)      "},            {"TwrAWzt   ","(Kw)      "},            {"RtADxi    ","(%)       "},
            {"RtADyi    ","(%)       "},            {"RtADzi    ","(%)       "},            {"RtADxt    ","(%)       "},            {"RtADyt    ","(%)       "},
            {"RtADzt    ","(%)       "},            {"RtAWxi    ","(Kw)      "},            {"RtAWyi    ","(Kw)      "},            {"RtAWzi    ","(Kw)      "},
            {"RtAWxt    ","(Kw)      "},            {"RtAWyt    ","(Kw)      "},            {"RtAWzt    ","(Kw)      "},        }.ToFrozenDictionary();
        /// <summary>
        /// 输出参数的通道表,用来保存变量和通道信息
        /// </summary>
        public static FrozenDictionary<string, int> MSA_OutParChannel = new Dictionary<string, int>()
        {
            {"BkNjADxi  ",0         },
            {"BkNjADyi  ",0         },            {"BkNjADzi  ",0         },            {"BldKADxi  ",0         },            {"BldKADyi  ",0         },
            {"BldKADzi  ",0         },            {"BkNjADxt  ",0         },            {"BkNjADyt  ",0         },            {"BkNjADzt  ",0         },
            {"BldKADxt  ",0         },            {"BldKADyt  ",0         },            {"BldKADzt  ",0         },            {"BkNjAWxi  ",0         },
            {"BkNjAWyi  ",0         },            {"BkNjAWzi  ",0         },            {"BldKAWxi  ",0         },            {"BldKAWyi  ",0         },
            {"BldKAWzi  ",0         },            {"BkNjAWxt  ",0         },            {"BkNjAWyt  ",0         },            {"BkNjAWzt  ",0         },
            {"BldKAWxt  ",0         },            {"BldKAWyt  ",0         },            {"BldKAWzt  ",0         },            {"TwHtADxi  ",1         },
            {"TwHtADyi  ",1         },            {"TwHtADzi  ",1         },            {"TwHtADxt  ",1         },            {"TwHtADyt  ",1         },
            {"TwHtADzt  ",1         },            {"TwrADxi   ",1         },            {"TwrADyi   ",1         },            {"TwrADzi   ",1         },
            {"TwrADxt   ",1         },            {"TwrADyt   ",1         },            {"TwrADzt   ",1         },            {"TwHtAWxi  ",1         },
            {"TwHtAWyi  ",1         },            {"TwHtAWzi  ",1         },            {"TwHtAWxt  ",1         },            {"TwHtAWyt  ",1         },
            {"TwHtAWzt  ",1         },            {"TwrAWxi   ",1         },            {"TwrAWyi   ",1         },            {"TwrAWzi   ",1         },
            {"TwrAWxt   ",1         },            {"TwrAWyt   ",1         },            {"TwrAWzt   ",1         },            {"RtADxi    ",2         },
            {"RtADyi    ",2         },            {"RtADzi    ",2         },            {"RtADxt    ",2         },            {"RtADyt    ",2         },
            {"RtADzt    ",2         },            {"RtAWxi    ",2         },            {"RtAWyi    ",2         },            {"RtAWzi    ",2         },
            {"RtAWxt    ",2         },            {"RtAWyt    ",2         },            {"RtAWzt    ",2         },        }.ToFrozenDictionary();
        /// <summary>
        /// 输出参数的维度 0 表示没有维度 1 表示只要部件编号 2 表示只要部件截面 3 表示部件编号和截面编号都要
        /// </summary>
        public static FrozenDictionary<string, int> MSA_OutParDim = new Dictionary<string, int>()
        {
            {"BkNjADxi  ",3},
            {"BkNjADyi  ",3},            {"BkNjADzi  ",3},            {"BldKADxi  ",1},            {"BldKADyi  ",1},
            {"BldKADzi  ",1},            {"BkNjADxt  ",3},            {"BkNjADyt  ",3},            {"BkNjADzt  ",3},
            {"BldKADxt  ",1},            {"BldKADyt  ",1},            {"BldKADzt  ",1},            {"BkNjAWxi  ",3},
            {"BkNjAWyi  ",3},            {"BkNjAWzi  ",3},            {"BldKAWxi  ",1},            {"BldKAWyi  ",1},
            {"BldKAWzi  ",1},            {"BkNjAWxt  ",3},            {"BkNjAWyt  ",3},            {"BkNjAWzt  ",3},
            {"BldKAWxt  ",1},            {"BldKAWyt  ",1},            {"BldKAWzt  ",1},            {"TwHtADxi  ",2},
            {"TwHtADyi  ",2},            {"TwHtADzi  ",2},            {"TwHtADxt  ",2},            {"TwHtADyt  ",2},
            {"TwHtADzt  ",2},            {"TwrADxi   ",0},            {"TwrADyi   ",0},            {"TwrADzi   ",0},
            {"TwrADxt   ",0},            {"TwrADyt   ",0},            {"TwrADzt   ",0},            {"TwHtAWxi  ",2},
            {"TwHtAWyi  ",2},            {"TwHtAWzi  ",2},            {"TwHtAWxt  ",2},            {"TwHtAWyt  ",2},
            {"TwHtAWzt  ",2},            {"TwrAWxi   ",0},            {"TwrAWyi   ",0},            {"TwrAWzi   ",0},
            {"TwrAWxt   ",0},            {"TwrAWyt   ",0},            {"TwrAWzt   ",0},            {"RtADxi    ",0},
            {"RtADyi    ",0},            {"RtADzi    ",0},            {"RtADxt    ",0},            {"RtADyt    ",0},
            {"RtADzt    ",0},            {"RtAWxi    ",0},            {"RtAWyi    ",0},            {"RtAWzi    ",0},
            {"RtAWxt    ",0},            {"RtAWyt    ",0},            {"RtAWzt    ",0},        }.ToFrozenDictionary();
        /// <summary>
        /// 依据变量名称,输出变量值
        /// </summary>
        public static double MSA_GetParamOutput(string param, int J, MSA_AllOuts AllOuts,int K=0)
        {
            switch (param)
            {
                case "BkNjADxi  ":
                    return AllOuts.BkNjADxi[J,K];
                case "BkNjADyi  ":
                    return AllOuts.BkNjADyi[J,K];
                case "BkNjADzi  ":
                    return AllOuts.BkNjADzi[J,K];
                case "BldKADxi  ":
                    return AllOuts.BldKADxi[K];
                case "BldKADyi  ":
                    return AllOuts.BldKADyi[K];
                case "BldKADzi  ":
                    return AllOuts.BldKADzi[K];
                case "BkNjADxt  ":
                    return AllOuts.BkNjADxt[J,K];
                case "BkNjADyt  ":
                    return AllOuts.BkNjADyt[J,K];
                case "BkNjADzt  ":
                    return AllOuts.BkNjADzt[J,K];
                case "BldKADxt  ":
                    return AllOuts.BldKADxt[K];
                case "BldKADyt  ":
                    return AllOuts.BldKADyt[K];
                case "BldKADzt  ":
                    return AllOuts.BldKADzt[K];
                case "BkNjAWxi  ":
                    return AllOuts.BkNjAWxi[J,K];
                case "BkNjAWyi  ":
                    return AllOuts.BkNjAWyi[J,K];
                case "BkNjAWzi  ":
                    return AllOuts.BkNjAWzi[J,K];
                case "BldKAWxi  ":
                    return AllOuts.BldKAWxi[K];
                case "BldKAWyi  ":
                    return AllOuts.BldKAWyi[K];
                case "BldKAWzi  ":
                    return AllOuts.BldKAWzi[K];
                case "BkNjAWxt  ":
                    return AllOuts.BkNjAWxt[J,K];
                case "BkNjAWyt  ":
                    return AllOuts.BkNjAWyt[J,K];
                case "BkNjAWzt  ":
                    return AllOuts.BkNjAWzt[J,K];
                case "BldKAWxt  ":
                    return AllOuts.BldKAWxt[K];
                case "BldKAWyt  ":
                    return AllOuts.BldKAWyt[K];
                case "BldKAWzt  ":
                    return AllOuts.BldKAWzt[K];
                case "TwHtADxi  ":
                    return AllOuts.TwHtADxi[J];
                case "TwHtADyi  ":
                    return AllOuts.TwHtADyi[J];
                case "TwHtADzi  ":
                    return AllOuts.TwHtADzi[J];
                case "TwHtADxt  ":
                    return AllOuts.TwHtADxt[J];
                case "TwHtADyt  ":
                    return AllOuts.TwHtADyt[J];
                case "TwHtADzt  ":
                    return AllOuts.TwHtADzt[J];
                case "TwrADxi   ":
                    return AllOuts.TwrADxi;
                case "TwrADyi   ":
                    return AllOuts.TwrADyi;
                case "TwrADzi   ":
                    return AllOuts.TwrADzi;
                case "TwrADxt   ":
                    return AllOuts.TwrADxt;
                case "TwrADyt   ":
                    return AllOuts.TwrADyt;
                case "TwrADzt   ":
                    return AllOuts.TwrADzt;
                case "TwHtAWxi  ":
                    return AllOuts.TwHtAWxi[J];
                case "TwHtAWyi  ":
                    return AllOuts.TwHtAWyi[J];
                case "TwHtAWzi  ":
                    return AllOuts.TwHtAWzi[J];
                case "TwHtAWxt  ":
                    return AllOuts.TwHtAWxt[J];
                case "TwHtAWyt  ":
                    return AllOuts.TwHtAWyt[J];
                case "TwHtAWzt  ":
                    return AllOuts.TwHtAWzt[J];
                case "TwrAWxi   ":
                    return AllOuts.TwrAWxi;
                case "TwrAWyi   ":
                    return AllOuts.TwrAWyi;
                case "TwrAWzi   ":
                    return AllOuts.TwrAWzi;
                case "TwrAWxt   ":
                    return AllOuts.TwrAWxt;
                case "TwrAWyt   ":
                    return AllOuts.TwrAWyt;
                case "TwrAWzt   ":
                    return AllOuts.TwrAWzt;
                case "RtADxi    ":
                    return AllOuts.RtADxi;
                case "RtADyi    ":
                    return AllOuts.RtADyi;
                case "RtADzi    ":
                    return AllOuts.RtADzi;
                case "RtADxt    ":
                    return AllOuts.RtADxt;
                case "RtADyt    ":
                    return AllOuts.RtADyt;
                case "RtADzt    ":
                    return AllOuts.RtADzt;
                case "RtAWxi    ":
                    return AllOuts.RtAWxi;
                case "RtAWyi    ":
                    return AllOuts.RtAWyi;
                case "RtAWzi    ":
                    return AllOuts.RtAWzi;
                case "RtAWxt    ":
                    return AllOuts.RtAWxt;
                case "RtAWyt    ":
                    return AllOuts.RtAWyt;
                case "RtAWzt    ":
                    return AllOuts.RtAWzt;

                default:
                    LogHelper.ErrorLog($"{param} not Support!,Please Visit OutputParList or www.openwecd.fun ",FunctionName: "MSA_GetParamOutput");
                    return 0;
            }
        }

    }
}
