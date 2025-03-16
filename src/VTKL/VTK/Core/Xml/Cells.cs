
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
////using System.Xml.Serialization;
namespace  OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// Represents the "Cells" XML element under <see cref="UnstructuredGridPiece"/>.
    /// </summary>
    public class Cells
    {
        /// <summary>
        /// The data arrays under "Cells" XML element.
        /// Should contain a "connectivity", an "offsets", and a "types" array.
        /// </summary>
        //[XmlElement("DataArray")]
        public List<DataArray> DataArrays { get; } = new List<DataArray>();

        /// <summary>
        /// Fills the cells array with connectivity, offsets and types array.
        /// </summary>
        /// <param name="connectivity">The point connectivity (points in each cell).</param>
        /// <param name="offsets">The offsets into the connectivity array at the end of each cell.</param>
        /// <param name="types">The type of each cell.</param>
        public void FillCells(IEnumerable<int> connectivity, IEnumerable<int> offsets, IEnumerable<byte> types)
        {
            var connectivityArray = new DataArray();
            connectivityArray.Name = "connectivity";
            connectivityArray.FillData(connectivity);

            var offsetsArray = new DataArray();
            offsetsArray.Name = "offsets";
            offsetsArray.FillData(offsets);

            var typesArray = new DataArray();
            typesArray.Name = "types";
            typesArray.FillData(types);

            DataArrays.Clear();
            DataArrays.Add(connectivityArray);
            DataArrays.Add(offsetsArray);
            DataArrays.Add(typesArray);
        }
    }
}