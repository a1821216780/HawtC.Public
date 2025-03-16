
//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.BeamL
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
using System.Text;

namespace OpenWECD.AeroL.Airfoil.AeroDamp
{
    /// <summary>
    /// 翼型阻尼分析
    /// </summary>
    public class DampAnalysis
    {
        airfoil__temp airfoil;
        /// <summary>
        /// 翼型阻尼分析器
        /// </summary>
        /// <param name="airfoil"></param>
        public DampAnalysis(airfoil__temp airfoil)
        {
            this.airfoil = airfoil;
        }
        /// <summary>
        /// 翼型文件路径
        /// </summary>
        /// <param name="path"></param>
        public DampAnalysis(string path)
        {
            //this.airfoil =AeroL_IO_Subs.
        }
    }
}
