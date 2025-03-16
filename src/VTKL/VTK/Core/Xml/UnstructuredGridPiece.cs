
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
using System.Drawing;
using System.Globalization;
using System.Text;
//using System.Xml.Serialization;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// A piece of unstructured grid.
    /// Represents the "Piece" XML element under <see cref="UnstructuredGrid"/>.
    /// </summary>
    public class UnstructuredGridPiece
    {
        /// <summary>
        /// The total number of points in the piece of grid.
        /// </summary>
       //  [XmlIgnore]
        public int? NumberOfPoints { get; set; }

        /// <summary>
        /// The total number of cells in the piece of grid.
        /// </summary>
       //  [XmlIgnore]
        public int? NumberOfCells { get; set; }

        /// <summary>
        /// The point data (values defined on points) of the piece of grid.
        /// </summary>
        //[XmlElement]
        public PointCellData PointData { get; set; }

        /// <summary>
        /// The cell data (values defined on cells) of the piece of grid.
        /// </summary>
       // [XmlElement]
        public PointCellData CellData { get; set; }

        /// <summary>
        /// The point coordinates of the piece of grid.
        /// </summary>
        //[XmlElement]
        public Points Points { get; set; }


        /// <summary>
        /// The cells of the piece of grid.
        /// </summary>
        //[XmlElement]
        public Cells Cells { get; set; }

        /// <summary>
        /// The string of <see cref="NumberOfPoints"/> for XML serialization.
        /// </summary>
        //[XmlAttribute("NumberOfPoints")]
        public string NumberOfPointsString
        {
            get => NumberOfPoints?.ToString(CultureInfo.InvariantCulture);
            set => NumberOfPoints = value == null ? null : (int?)int.Parse(value);
        }

        /// <summary>
        /// The string of <see cref="NumberOfCells"/> for XML serialization.
        /// </summary>
        //[XmlAttribute("NumberOfCells")]
        public string NumberOfCellsString
        {
            get => NumberOfCells?.ToString(CultureInfo.InvariantCulture);
            set => NumberOfCells = value == null ? null : (int?)int.Parse(value);
        }
    }
}
