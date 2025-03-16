
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
using System.Text;
using OpenWECD.VTKL.Core.Xml;

namespace OpenWECD.VTKL.Core
{
    /// <summary>
    /// The builder to store data for building a "Piece" XML element
    /// (see <see cref="UnstructuredGridPiece"/>).
    /// </summary>
    public class UnstructuredGridPieceBuilder
    {
        /// <summary>
        /// Gets the point / node coordinates builder.
        /// </summary>
        public PointsBuilder Points { get; } = new PointsBuilder();


        /// <summary>
        /// Gets the cell / element builder.
        /// </summary>
        public CellsBuilder Cells { get; } = new CellsBuilder();

        /// <summary>
        /// Gets the point data builder (values defined at points / nodes).
        /// </summary>
        public PointCellDataBuilder PointData { get; } = new PointCellDataBuilder();

        /// <summary>
        /// Gets the cell data builder (values defined in cells).
        /// </summary>
        public PointCellDataBuilder CellData { get; } = new PointCellDataBuilder();

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="UnstructuredGridPiece"/> instance that represents the XML element.</returns>
        public UnstructuredGridPiece ToXml()
        {
            var piece = new UnstructuredGridPiece();
            piece.Cells = Cells.ToXml();
            piece.Points = Points.ToXml();
            piece.PointData = PointData.ToXml();
            piece.CellData = CellData.ToXml();
            piece.NumberOfCells = Cells.Count;
            piece.NumberOfPoints = Points.Count;
            return piece;
        }
    }
}
