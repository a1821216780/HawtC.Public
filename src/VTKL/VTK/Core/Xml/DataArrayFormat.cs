
//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.VTKL.Core
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


//using System.Xml.Serialization;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// The values of "format" attribute in <see cref="DataArray"/> XML element.
    /// </summary>
    public enum DataArrayFormat
    {
        // Refer to VTK user guide for definitions of each data array format.
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        //[XmlEnum("ascii")]
        Ascii,

        //[XmlEnum("binary")]
        Binary,

       // [XmlEnum("appended")]
        Appended,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}