
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

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// Represents the types of each cell in the "types" array under <see cref="Cells"/> XML element.
    /// Refer to VTK User's Guide 11th Edition p.481 for definitons of each cell type.
    /// </summary>
    public enum CellType : byte
    {
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        Vertex = 1,
        PolyVertex,
        Line,
        PolyLine,
        Triangle,
        TriangleStrip,
        Polygon,
        Pixel,
        Quad,
        Tetra,
        Voxel,
        Hexahedron,
        Wedge,
        Pyramid,
        PentagonalPrism,
        HexagonalPrism,
        QuadraticEdge = 21,
        QuadraticTriangle,
        QuadraticQuad,
        QuadraticTetra,
        QuadraticHexahedron,
        QuadraticWedge,
        QuadraticPyramid,
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
#pragma warning restore SA1602 // Enumeration items should be documented
    }
}
