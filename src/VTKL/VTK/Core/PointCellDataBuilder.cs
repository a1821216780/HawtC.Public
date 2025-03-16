
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
    /// The builder to store data for building a <see cref="PointCellData"/> XML element.
    /// </summary>
    public class PointCellDataBuilder
    {
        /// <summary>
        /// The list of point data / cell data.
        /// </summary>
        public List<IDataArrayBuilder> DataArrays { get; } = new List<IDataArrayBuilder>();

        /// <summary>
        /// The active scalars array, if any.
        /// </summary>
        public IDataArrayBuilder Scalars { get; set; }

        /// <summary>
        /// The active vectors array, if any.
        /// </summary>
        public IDataArrayBuilder Vectors { get; set; }

        /// <summary>
        /// The active normals array, if any.
        /// </summary>
        public IDataArrayBuilder Normals { get; set; }

        /// <summary>
        /// The active tensors array, if any.
        /// </summary>
        public IDataArrayBuilder Tensors { get; set; }

        /// <summary>
        /// The active texture coordinates array, if any.
        /// </summary>
        public IDataArrayBuilder TCoords { get; set; }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="PointCellData"/> instance that represents the XML element.</returns>
        public PointCellData ToXml()
        {
            var pointData = new PointCellData()
            {
                Scalars = Scalars?.Name,
                Vectors = Vectors?.Name,
                Normals = Normals?.Name,
                Tensors = Tensors?.Name,
                TCoords = TCoords?.Name,
            };

            foreach (var item in DataArrays)
            {
                pointData.DataArrays.Add(item.ToXml());
            }

            return pointData;
        }
    }
}
