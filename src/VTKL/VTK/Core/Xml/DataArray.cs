
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


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
//using System.Xml.Serialization;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// Represents a "DataArray" XML element.
    /// </summary>
    /// <remarks>
    /// See VTK User's Guide 11th Edition p.487 for details.
    /// The documents of the members are taken from the VTK User's Guide.
    /// </remarks>
    public partial class DataArray
    {
        /// <summary>
        /// Gets or sets the data type of a single component of the array.
        /// </summary>
       // [XmlAttribute("type")]
        public DataArrayType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the array./
        /// </summary>
       // [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the number of components per value in the array.
        /// </summary>
       ////  [XmlIgnore]
        public uint? NumberOfComponents { get; set; }

        /// <summary>
        /// Gets or sets the means by which the data values themselves are stored in the file.
        /// </summary>
       // [XmlAttribute("format")]
        public DataArrayFormat Format { get; set; }

        /// <summary>
        /// If <see cref="Format"/> is <see cref="DataArrayFormat.Appended"/>, this specifies the
        /// offset from the beginning of the appended data section to the beginning of this
        /// array's data.
        /// </summary>
       ////  [XmlIgnore]
        public uint? Offset { get; set; }

        /// <summary>
        /// The content of the data array as a string.
        /// </summary>
       // [XmlText]
        public string Content { get; set; }

        /// <summary>
        /// The string of <see cref="NumberOfComponents"/> used for XML serialization.
        /// </summary>
       // [XmlAttribute("NumberOfComponents")]
        public string NumberOfComponentsString
        {
            get => NumberOfComponents?.ToString(CultureInfo.InvariantCulture);
            set => NumberOfComponents = value == null ? null : (uint?)uint.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The string of <see cref="Offset"/> used for XML serialization.
        /// </summary>
       // [XmlAttribute("offset")]
        public string OffsetString
        {
            get => Offset?.ToString(CultureInfo.InvariantCulture);
            set => Offset = value == null ? null : (uint?)uint.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}
