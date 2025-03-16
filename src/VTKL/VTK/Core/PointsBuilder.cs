
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
    /// The builder to store data for building a <see cref="Points"/> XML element.
    /// </summary>
    public class PointsBuilder
    {
        // x1, y1, z1, x2, y2, z2, ...
        private readonly List<double> points = new List<double>();

        /// <summary>
        /// The count of points.
        /// </summary>
        public int Count => points.Count / 3;

        /// <summary>
        /// Adds a point into the internal list.
        /// </summary>
        /// <param name="x">The x component of the point.</param>
        /// <param name="y">The y component of the point.</param>
        /// <param name="z">The z component of the point.</param>
        public void AddPoint(double x, double y, double z)
        {
            points.Add(x);
            points.Add(y);
            points.Add(z);
        }

        /// <summary>
        /// Builds the XML element for serializing.
        /// </summary>
        /// <returns>The <see cref="Points"/> instance that represents the XML element.</returns>
        public Points ToXml()
        {
            var pointsXml = new Points();
            pointsXml.FillPoints(points);
            return pointsXml;
        }
    }
}
