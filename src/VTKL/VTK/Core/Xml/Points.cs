
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
    /// Represents the "Points" XML element under "Piece" element (see <see cref="UnstructuredGridPiece"/>).
    /// </summary>
    public class Points
    {
        /// <summary>
        /// The "DataArray" element that represents the components of point coordinates.
        /// </summary>
        public DataArray DataArray { get; set; }

        /// <summary>
        /// Fills the data array with point coordinates.
        /// </summary>
        /// <param name="points">All coordinate components ordered by point.</param>
        public void FillPoints(IEnumerable<double> points)
        {
            var dataArray = new DataArray();
            dataArray.FillData(points);
            dataArray.NumberOfComponents = 3;

            DataArray = dataArray;
        }
    }
}