
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


using OpenWECD.IO.IO;
using OpenWECD.IO.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace OpenWECD.VTKL.Core.Xml
{
    /// <summary>
    /// Represents the root element of a VTK XML file.
    /// </summary>
    public class VTKFile
    {
        /// <summary>
        /// The type of the VTK file. The "type" XML attribute of the "VTKFile" element.
        /// </summary>
       // [XmlAttribute("type")]
        public VTKFileType Type { get; set; }

        /// <summary>
        /// The data of the unstructured grid. The "UnstructuredGrid" XML element under the "VTKFile" element.
        /// </summary>
        /// [XmlElement]
        public UnstructuredGrid UnstructuredGrid { get; set; }

        /// <summary>
        /// Sets the content to the given "UnstructuredGrid" element and
        /// sets <see cref="Type"/> correspondingly.
        /// </summary>
        /// <param name="grid">The given unstructured grid.</param>
        public void SetUnstructuredGrid(UnstructuredGrid grid)
        {
            UnstructuredGrid = grid;
            Type = VTKFileType.UnstructuredGrid;
        }

        /// <summary>
        /// Saves to a VTK XML file (.vti/.vtp/.vtr/...).
        /// </summary>
        /// <param name="stream">The file stream.</param>
        public void Save(string path)
        {
            //var xmlSerializer = new XmlSerializer(typeof(VTKFile));
            //xmlSerializer.Serialize(stream, this);
            OutFile outFile = new OutFile(path);
            //outFile.WriteLine("<!-- Powered By OpenHast v1.0.0 @copy right 赵子祯 AND TG Team -->");
            outFile.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            outFile.WriteLine("<VTKFile xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" type=\"UnstructuredGrid\">");
            outFile.WriteLine("  <UnstructuredGrid>");
            //# 循环部件Piece
            foreach (var it in this.UnstructuredGrid.Pieces)
            {
                outFile.WriteLine($"    <Piece NumberOfPoints=\"{it.NumberOfPoints}\" NumberOfCells=\"{it.NumberOfCells}\">");
                //# Pointdata写出
                if (it.PointData.DataArrays.Count == 0)
                {
                    outFile.WriteLine("      <PointData />");
                }
                else
                {
                    outFile.WriteLine("      <PointData>");
                    foreach (var item in it.PointData.DataArrays)
                    {
                        outFile.Write("        <DataArray", " ");
                        if (!string.IsNullOrEmpty(item.Type.ToString()))
                        {
                            outFile.Write($"type=\"{item.Type}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            outFile.Write($"Name=\"{item.Name}\""," ");
                        }
                        if (!string.IsNullOrEmpty(item.Format.ToString()))
                        {
                            outFile.Write($"format=\"{item.Format}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.NumberOfComponentsString))
                        {
                            outFile.Write($"NumberOfComponents=\"{item.NumberOfComponentsString}\"", "");
                        }
                        outFile.Write(">", "");
                        outFile.Write(item.Content, "");
                        outFile.WriteLine("</DataArray>");
                    }
                    outFile.WriteLine("      </PointData>");
                }


                
                
                //outFile.Write($" {}");
                //# 把点写出
                outFile.WriteLine("      <Points>");
                var items = it.Points.DataArray;
                outFile.Write("        <DataArray", " ");
                if (!string.IsNullOrEmpty(items.Type.ToString()))
                {
                    outFile.Write($"type=\"{items.Type}\"", " ");
                }
                if (!string.IsNullOrEmpty(items.Name))
                {
                    outFile.Write($"Name=\"{items.Name}\"", " ");
                }
                if (!string.IsNullOrEmpty(items.Format.ToString()))
                {
                    outFile.Write($"format=\"{items.Format}\"", " ");
                }
                if (!string.IsNullOrEmpty(items.NumberOfComponentsString))
                {
                    outFile.Write($"NumberOfComponents=\"{items.NumberOfComponentsString}\"", "");
                }
                outFile.Write(">", "");
                outFile.Write(items.Content, "");
                outFile.WriteLine("</DataArray>");
                outFile.WriteLine("      </Points>");

                //# 把Cells写出
                if (it.Cells.DataArrays.Count == 0)
                {
                    outFile.WriteLine("      <Cells />");
                }
                else
                {
                    outFile.WriteLine("      <Cells>");
                    foreach (var item in it.Cells.DataArrays)
                    {
                        outFile.Write("        <DataArray", " ");
                        if (!string.IsNullOrEmpty(item.Type.ToString()))
                        {
                            outFile.Write($"type=\"{item.Type}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            outFile.Write($"Name=\"{item.Name}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.Format.ToString()))
                        {
                            outFile.Write($"format=\"{item.Format}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.NumberOfComponentsString))
                        {
                            outFile.Write($"NumberOfComponents=\"{item.NumberOfComponentsString}\"", "");
                        }
                        outFile.Write(">", "");
                        outFile.Write(item.Content, "");
                        outFile.WriteLine("</DataArray>");
                    }
                    outFile.WriteLine("      </Cells>");

                    //# 把CellData写出
                    if (it.CellData.DataArrays.Count == 0)
                    {
                        outFile.WriteLine("      <CellData />");
                    }
                    else
                    {
                        if (it.CellData.DataArrays[0].Content.Split(' ', '\t').RemoveNull().Length  != it.NumberOfCells * it.CellData.DataArrays[0].NumberOfComponents)
                        {
                            LogHelper.ErrorLog("The CellData.DataArrays not equel NumberOfCells");
                        }
                        var item = it.CellData.DataArrays[0];
                        outFile.WriteLine("      <CellData Scalars=\"scalars\">");
                        outFile.Write("        <DataArray", " ");
                        if (!string.IsNullOrEmpty(item.Type.ToString()))
                        {
                            outFile.Write($"type=\"{item.Type}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.Name))
                        {
                            outFile.Write($"Name=\"{item.Name}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.Format.ToString()))
                        {
                            outFile.Write($"format=\"{item.Format}\"", " ");
                        }
                        if (!string.IsNullOrEmpty(item.NumberOfComponentsString))
                        {
                            outFile.Write($"NumberOfComponents=\"{item.NumberOfComponentsString}\"", "");
                        }
                        outFile.Write(">", "");
                        outFile.Write(item.Content, "");
                        outFile.WriteLine("</DataArray>");
                        outFile.WriteLine("      </CellData>");
                        //LogHelper.ErrorLog("openhast.VTK not support Celldata");
                    }
                }
                outFile.WriteLine("    </Piece>");
            }
            outFile.WriteLine("  </UnstructuredGrid>");
            outFile.Write("</VTKFile>","");


        }
    }
}
