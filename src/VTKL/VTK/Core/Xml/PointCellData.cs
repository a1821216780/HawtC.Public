
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


using System.Collections.Generic;
//using System.Xml.Serialization;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// Represents the "PointData" or "CellData" XML element under the "Piece" element
    /// (see <see cref="UnstructuredGridPiece"/>).
    /// </summary>
    public class PointCellData
    {
        /// <summary>
        /// The name of the active scalars array, if any.
        /// </summary>
       // [XmlAttribute]
        public string Scalars { get; set; }

        /// <summary>
        /// The name of the active vectors array, if any.
        /// </summary>
       // [XmlAttribute]

        public string Vectors { get; set; }

        /// <summary>
        /// The name of the active normals array, if any.
        /// </summary>
        // [XmlAttribute]
        public string Normals { get; set; }

        /// <summary>
        /// The name of the active tensors array, if any.
        /// </summary>
       // [XmlAttribute]
        public string Tensors { get; set; }

        /// <summary>
        /// The name of the active texture coordinates array, if any.
        /// </summary>
       // [XmlAttribute]
        public string TCoords { get; set; }

        /// <summary>
        /// The list of data arrays (actual point / cell data).
        /// </summary>
       // [XmlElement("DataArray")]
        public List<DataArray> DataArrays { get; } = new List<DataArray>();
    }
}