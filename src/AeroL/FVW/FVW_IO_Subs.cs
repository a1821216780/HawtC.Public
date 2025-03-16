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
using OpenWECD.IO.Interface1;
using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using System;
using System.Linq;

namespace OpenWECD.AeroL.FVW
{
    /// <summary>
    /// FVW_IO_Subs 读取自由涡尾迹输入文件
    /// </summary>
    public class FVW_IO_Subs : IConvertStruct2Yml<FVW1>
    {
        public static void ReadFVM_MainFile(string path)
        {
            
        }
        
        public static void ReadFVW_MainFile(in YML yml, string key = "OpenWECD.OpenHAST.AeroL.FVW")
        {
            
        }

        public void ConvertToYML(ref YML yml, FVW1 aeroL)
        {
            LogHelper.WriteLogO("开始转换AeroL 为 yml");
            YML.ConvertStructToYML(ref yml, "OpenWECD.OpenHAST.AeroL", 4, aeroL);
            LogHelper.WriteLogO("转换AeroL 成功！");

        }
    }
}
