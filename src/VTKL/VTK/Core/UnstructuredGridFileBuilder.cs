
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
    /// The builder to store data for building an <see cref="UnstructuredGrid"/> XML element.
    /// </summary>
    public class UnstructuredGridFileBuilder
    {
        /// <summary>
        /// The unrelated pieces of the grid.
        /// </summary>
        public List<UnstructuredGridPieceBuilder> Pieces { get; } = new List<UnstructuredGridPieceBuilder>();

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="VTKFile"/> instance that represents the XML element.</returns>
        public VTKFile ToXml()
        {
            var unstructuredGrid = new UnstructuredGrid();
            foreach (var pieceBuilder in Pieces)
            {
                unstructuredGrid.Pieces.Add(pieceBuilder.ToXml());
            }

            var vtkFile = new VTKFile();
            vtkFile.SetUnstructuredGrid(unstructuredGrid);
            return vtkFile;
        }
    }
}
