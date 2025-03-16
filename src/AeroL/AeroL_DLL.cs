
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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using OpenWECD.AeroL;


namespace OpenWECD.HawtC2.DLL
{
    public static partial class CAndC_DLL
    {
        [UnmanagedCallersOnly(EntryPoint = "AER_GetBldNum")]
        public static int AER_GetBldNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].AER_p.NumBl;
        }
        public static int GetBldNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].AER_p.NumBl;
        }


        [UnmanagedCallersOnly(EntryPoint = "AER_GetBldSecNum")]
        public static int AER_GetBldSecNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].MBD_p.BldNodes;
        }
        public static int GetBldSecNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].MBD_p.BldNodes;
        }


        [UnmanagedCallersOnly(EntryPoint = "AER_GetTwrSecNum")]
        public static int AER_GetTwrSecNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].MBD_p.TwrNodes;
        }
        public static int GetTwrSecNum(int turbnum)
        {
            return OpenWECD.HawtC2.Hast_INI.HST_moduleparaList[turbnum].MBD_p.TwrNodes;
        }
    }
}
