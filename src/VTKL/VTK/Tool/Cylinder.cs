

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.VTKL.Core.Tool
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



using OpenWECD.IO.math;
using OpenWECD.VTKL.Core;
using OpenWECD.VTKL.Core.Xml;
using MathNet.Numerics.LinearAlgebra;
//using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using OpenWECD.IO.Log;
using static System.Math;
using System.IO;
using System.Xml.Linq;
using OpenWECD.FEML;
using OpenWECD.FEML.FEMVTK;
using Vector3 = System.Numerics.Vector3;
using OpenWECD.IO.Numerics;
using static OpenWECD.IO.IO.PhysicalParameters;
using System.Collections.Immutable;


namespace OpenWECD.VTKL.Tool
{
    public class Cylinder
    {
        //public static void CylinderVTK(string path, double[] center, double radius, double height, int resolution = 8)
        //{
        //    resolution = resolution + 1;
        //    var builder = new UnstructuredGridFileBuilder();
        //    var pieceBuilder = new UnstructuredGridPieceBuilder();
        //    var rad = MathHelper.Range(0.0, 2 * PI, length: resolution);
        //    var x = rad.PointwiseCos() * radius;
        //    var y = rad.PointwiseSin() * radius;
        //    var xlist = new List<int>();
        //    var ylist = new List<int>();
        //    for (int i = 0; i < resolution; i++)
        //    {
        //        pieceBuilder.Points.AddPoint(x[i], y[i], 0);
        //        xlist.Add(i);
        //    }
        //    pieceBuilder.Cells.AddCell(xlist.ToArray(), CellType.Triangle);
        //    for (int i = resolution; i < 2 * resolution; i++)
        //    {
        //        pieceBuilder.Points.AddPoint(x[i - resolution], y[i - resolution], height);
        //        ylist.Add(i);
        //    }
        //    pieceBuilder.Cells.AddCell(ylist.ToArray(), CellType.Triangle);

        //    for (int i = 0; i < x.Count - 1; i++)
        //    {
        //        var index = new int[] { xlist[i], ylist[i], xlist[i + 1], ylist[i + 1] };
        //        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
        //    }

        //    builder.Pieces.Add(pieceBuilder);
        //    var vtkFile = builder.ToXml();
        //    vtkFile.Save($"G:\\2024\\Openhast\\unit_tests\\ExtimeTest.vtu");
        //    //var xmlSerializer = new XmlSerializer(typeof(VTKFile));
        //    //using (var stream = File.Create($"G:\\2024\\Openhast\\unit_tests\\ExtimeTest.vtu"))
        //    //{
        //    //    vtkFile.Save(stream);
        //    //    //xmlSerializer.Serialize(stream, vtkFile);
        //    //}
        //}

        public static UnstructuredGridPieceBuilder BuildTowerOrBlade(int secnum, Vector<double>[] x, Vector<double>[] y, Vector<double>[] z, double[] x1, double[] y1, double[] z1, double[] x0, double[] y0, double[] z0, bool plotsec, bool col, byte color)
        {
            var pieceBuilder = new UnstructuredGridPieceBuilder();
            if (secnum == x.Length & secnum == y.Length & secnum == z.Length & secnum == z.Length & secnum == x1.Length
              & secnum == y1.Length & secnum == z1.Length & secnum == z0.Length & secnum == x0.Length & secnum == y0.Length)
            {

            }
            else
            {
                LogHelper.ErrorLog("VTK error ，because the unequel sec num!");
            }
            //创建截面的点集合
            List<Vector<double>>[] pointlist = new List<Vector<double>>[secnum];
            //创建点的编号集合
            List<int>[] pointindex = new List<int>[secnum];
            // 一个截面的点数量
            int pointnumonesec = x[0].Count;
            //初始点的编号为0
            int pointnum = 0;

            for (int i = 0; i < secnum; i++)//循环截面
            {
                pointlist[i] = new List<Vector<double>>();
                pointindex[i] = new List<int>();
                //平移向量
                var T = LinearAlgebraHelper.zeros(x1[i], y1[i], z1[i]);
                //旋转向量
                double[,] I1temp = new double[,]
                 {
                {1.0,     0.0    ,     0.0    },
                {0.0, Cos(x0[i]) ,-Sin(x0[i]) },
                {0.0, Sin(x0[i]) , Cos(x0[i]) }

                };
                var R1 = Matrix<double>.Build.DenseOfArray(I1temp);
                double[,] I2temp = new double[,]
                {
                {Cos(y0[i]) , 0.0 ,Sin(y0[i]) },
                {0.0        , 1.0    ,0.0     },
                {-Sin(y0[i]), 0.0, Cos(y0[i])}

                };
                var R2 = Matrix<double>.Build.DenseOfArray(I2temp);
                double[,] I3temp = new double[,]
                {
                {Cos(z0[i]), -Sin(z0[i]) ,0.0 },
                {Sin(z0[i]), Cos(z0[i])  ,0.0 },
                {0.0       ,     0.0     ,1.0}
                };
                var R3 = Matrix<double>.Build.DenseOfArray(I3temp);
                var R = R1 * R2 * R3;
                for (int j = 0; j < x[0].Count; j++)//循环每个截面上的点
                {
                    //创建点向量
                    var point = LinearAlgebraHelper.zeros(x[i][j], y[i][j], z[i][j]);

                    //转换后的点
                    point = R * point + T;
                    //实体添加点信息
                    pieceBuilder.Points.AddPoint(point[0], point[1], point[2]);
                    //向点集合添加点
                    pointlist[i].Add(point);
                    //向点集合添加点编号
                    pointindex[i].Add(pointnum);
                    //控制点的编号
                    pointnum++;
                }
            }
            var cellData1 = new DataArrayBuilderUInt8("Colors", 1);

            //绘制截面
            if (plotsec)
            {
                pieceBuilder.Cells.AddCell(pointindex[0], CellType.Triangle);
                cellData1.AddScalarDatum(color);



                pieceBuilder.Cells.AddCell(pointindex[secnum - 1], CellType.Triangle);

               cellData1.AddScalarDatum(color);

                //for (int i = 0; i < secnum; i++)
                //{
                //    pieceBuilder.Cells.AddCell(pointindex[i], CellType.Triangle);
                //}
            }
            //绘制侧面

            for (int i = 0; i < pointnumonesec; i++)
            {
                for (int j = 0; j < secnum - 1; j++)
                {
                    if (i + 1 != pointnumonesec)
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][i + 1], pointindex[j][i + 1] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);

                       cellData1.AddScalarDatum(color);
                    }
                    else
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][0], pointindex[j][0] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
                       cellData1.AddScalarDatum(color);
                    }

                }
            }
            if (col)
            {
                pieceBuilder.CellData.DataArrays.Add(cellData1);
            }

            return pieceBuilder;
        }

        /// <summary>
        /// 给出叶片或者塔架的非结构网格。
        /// </summary>
        /// <param name="pointx">截面的点二维数组，代表【secnum 是截面数量,secpointnum 是截面上的点数量】</param>
        /// <param name="XYZ">代表转换后的线的点云表达式，【secnum 是截面数量，3】</param>
        /// <param name="plotsec"></param>
        /// <returns></returns>
        public static UnstructuredGridPieceBuilder BuildTowerOrBlade(Vector3S[,] pointx, Vector3S[] XYZ, bool plotsec, bool colors, byte color)
        {
            var pieceBuilder = new UnstructuredGridPieceBuilder();
            if (pointx.GetLength(0) != XYZ.Length)
            {
                LogHelper.ErrorLog("VTK.BuildTowerOrBlade error! ，because the unequel sec num!");
            }
            int secnum = XYZ.Length;
            //创建截面的点集合
            List<Vector<double>>[] pointlist = new List<Vector<double>>[secnum];
            //创建点的编号集合
            List<int>[] pointindex = new List<int>[secnum];
            // 一个截面的点数量
            int pointnumonesec = pointx.GetLength(1);
            //初始点的编号为0
            int pointnum = 0;
            //# 计算线的法向量数组
            var FN = BaseMethord.CalVectorNvec(XYZ);

            for (int i = 0; i < secnum; i++)//循环截面
            {
                pointlist[i] = new List<Vector<double>>();
                pointindex[i] = new List<int>();
                (var rot, var tran, var ts) = BaseMethord.CalculateTransformation(Vector3.UnitZ, System.Numerics.Vector3.Zero, Vector3S.TVec3(FN[i]), Vector3S.TVec3(XYZ[i]));

                for (int j = 0; j < pointnumonesec; j++)//循环每个截面上的点
                {
                    var point2 = System.Numerics.Vector3.Transform(Vector3S.TVec3(pointx[i, j]), rot);
                    var point1 = System.Numerics.Vector3.Transform(point2, tran);
                    //创建点向量
                    var point = LinearAlgebraHelper.zeros(point1.X, point1.Y, point1.Z);

                    //var R3 = Rot_Mat(FN[i], Rn[i]);
                    ////转换后的点
                    //point = R3 * point;
                    //实体添加点信息
                    pieceBuilder.Points.AddPoint(point[0], point[1], point[2]);
                    //向点集合添加点
                    pointlist[i].Add(point);
                    //向点集合添加点编号
                    pointindex[i].Add(pointnum);
                    //控制点的编号
                    pointnum++;
                }

            }
            var cellData1 = new DataArrayBuilderUInt8("Colors", 1);

            //绘制截面
            if (plotsec)
            {
                pieceBuilder.Cells.AddCell(pointindex[0], CellType.Triangle);
               cellData1.AddScalarDatum(color);



                pieceBuilder.Cells.AddCell(pointindex[secnum - 1], CellType.Triangle);
               cellData1.AddScalarDatum(color);

                //var cellData = new DataArrayBuilderInt8("Color");
                ////for (int i = 0; i < secnum; i++)

                //List<sbyte> color = new List<sbyte> { 127,0,0};
                //pieceBuilder.CellData.DataArrays.Add(cellData);
                //cellData.AddVectorDatum(color);
                //for (int i = 0; i < secnum; i++)
                //{
                //    pieceBuilder.Cells.AddCell(pointindex[i], CellType.Triangle);
                //}
            }
            //绘制侧面

            for (int i = 0; i < pointnumonesec; i++)
            {
                for (int j = 0; j < secnum - 1; j++)
                {
                    if (i + 1 != pointnumonesec)
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][i + 1], pointindex[j][i + 1] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
                       cellData1.AddScalarDatum(color);

                    }
                    else
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][0], pointindex[j][0] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
                       cellData1.AddScalarDatum(color);

                    }

                }
            }
            if (colors)
            {
                pieceBuilder.CellData.DataArrays.Add(cellData1);
            }
            return pieceBuilder;
        }

        /// <summary>
        /// 这个函数用于绘制塔筒或者叶片,返回一个结构网格数组，最大大小为2,可以指定是否绘制原始未变形体,且截面点的数量必须相等
        /// </summary>
        /// <param name="fEM"></param>
        /// <returns></returns>
        public static UnstructuredGridPieceBuilder BuildTowerOrBlade(FEMModel fEM, bool plotsec)
        {
            int secnum = fEM.Nnode;
            var pieceBuilder = new UnstructuredGridPieceBuilder();
            //创建截面的点集合
            List<Vector<double>>[] pointlist = new List<Vector<double>>[secnum];
            //创建点的编号集合
            List<int>[] pointindex = new List<int>[secnum];
            // 一个截面的点数量
            int pointnumonesec = 100;
            if (fEM.Elements[0].Vertice_a is null)
            {
                LogHelper.ErrorLog("BuildTowerorBlade Error!,because fEM.Elements[0].Vertice_a is null");
            }
            if (fEM.Elements[0].Vertice_a.RowCount != 1)//自定义形状
            {
                pointnumonesec = fEM.Elements[0].Vertice_a.RowCount;
            }
            else
            {
                LogHelper.ErrorLog("BuildTowerorBlade Error!,because fEM.Elements[0].Vertice_a must biger than 3");
            }
            //# 计算线的法向量数组
            var xyz = fEM.GetNodeDisplacementArray();
            var FN = BaseMethord.CalVectorNvec(xyz);

            //初始点的编号为0
            int pointnum = 0;

            for (int i = 0; i < secnum; i++)//循环截面
            {
                pointlist[i] = new List<Vector<double>>();
                pointindex[i] = new List<int>();
                var tran = BaseMethord.CalculateTransformation(Vector3S.TVec3(fEM.Elements[0].Reference), System.Numerics.Vector3.Zero, Vector3S.TVec3(FN[i]), Vector3S.TVec3(xyz[i])).all;
                if (i != secnum - 1)
                {
                    for (int j = 0; j < pointnumonesec; j++)//循环每个截面上的点
                    {
                        var point1 = System.Numerics.Vector3.Transform(LinearAlgebraHelper.ToVec3(fEM.Elements[i].Vertice_a.Row(j)), tran);
                        //创建点向量
                        var point = LinearAlgebraHelper.zeros(point1.X, point1.Y, point1.Z);

                        //var R3 = Rot_Mat(FN[i], Rn[i]);
                        ////转换后的点
                        //point = R3 * point;
                        //实体添加点信息
                        pieceBuilder.Points.AddPoint(point[0], point[1], point[2]);
                        //向点集合添加点
                        pointlist[i].Add(point);
                        //向点集合添加点编号
                        pointindex[i].Add(pointnum);
                        //控制点的编号
                        pointnum++;
                    }
                }
                else
                {
                    for (int j = 0; j < pointnumonesec; j++)//循环每个截面上的点
                    {
                        var point1 = System.Numerics.Vector3.Transform(LinearAlgebraHelper.ToVec3(fEM.Elements[secnum - 2].Vertice_b.Row(j)), tran);
                        ////创建点向量
                        var point = LinearAlgebraHelper.zeros(point1.X, point1.Y, point1.Z);

                        //var R3 = Rot_Mat(FN[i], Rn[i]);
                        //转换后的点
                        //point = R3 * point;
                        //实体添加点信息
                        pieceBuilder.Points.AddPoint(point[0], point[1], point[2]);
                        //向点集合添加点
                        pointlist[i].Add(point);
                        //向点集合添加点编号
                        pointindex[i].Add(pointnum);
                        //控制点的编号
                        pointnum++;
                    }
                }

            }
            //var cellData1 = new DataArrayBuilderUInt8("Colors", 1);
            //byte color = new byte { 0, 255, 255 };
            //绘制截面
            if (plotsec)
            {
                pieceBuilder.Cells.AddCell(pointindex[0], CellType.Triangle);

                //cellData1.AddVectorDatum(color);



                pieceBuilder.Cells.AddCell(pointindex[secnum - 1], CellType.Triangle);
                //cellData1.AddVectorDatum(color);
                //for (int i = 0; i < secnum; i++)
                //{
                //    pieceBuilder.Cells.AddCell(pointindex[i], CellType.Triangle);
                //}
            }
            //绘制侧面

            for (int i = 0; i < pointnumonesec; i++)
            {
                for (int j = 0; j < secnum - 1; j++)
                {
                    if (i + 1 != pointnumonesec)
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][i + 1], pointindex[j][i + 1] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
                        //cellData1.AddVectorDatum(color);

                    }
                    else
                    {
                        int[] index = new int[] { pointindex[j][i], pointindex[j + 1][i], pointindex[j + 1][0], pointindex[j][0] };
                        pieceBuilder.Cells.AddCell(index, CellType.Tetra);
                        //cellData1.AddVectorDatum(color);

                    }

                }
            }
            //pieceBuilder.CellData.DataArrays.Add(cellData1);
            return pieceBuilder;
        }

        public static Matrix3S RotZ_Mat(double rad)
        {
            var temp = Matrix3S.Zeros();
            temp[0, 0] = Cos(rad);
            temp[0, 1] = -Sin(rad);
            temp[0, 2] = 0.0;
            temp[1, 0] = Sin(rad);
            temp[1, 1] = Cos(rad);
            temp[1, 2] = 0.0;
            temp[2, 0] = 0.0;
            temp[2, 1] = 0.0;
            temp[2, 2] = 1.0;
            return temp;
        }
        public static Matrix3S RotX_Mat(double rad)
        {
            var temp = Matrix3S.Zeros();
            temp[0, 0] = 1.0;
            temp[0, 1] = 0.0;
            temp[0, 2] = 0.0;
            temp[1, 0] = 0.0;
            temp[1, 1] = Math.Cos(rad);
            temp[1, 2] = -Math.Sin(rad);
            temp[2, 0] = 0.0;
            temp[2, 1] = Math.Sin(rad);
            temp[2, 2] = Math.Cos(rad);
            return temp;
        }

        public static Matrix3S RotY_Mat(double rad)
        {
            var temp = Matrix3S.Zeros();
            temp[0, 0] = Math.Cos(rad);
            temp[0, 1] = 0.0;
            temp[0, 2] = Math.Sin(rad);
            temp[1, 0] = 0.0;
            temp[1, 1] = 1.0;
            temp[1, 2] = 0.0;
            temp[2, 0] = -Math.Sin(rad);
            temp[2, 1] = 0.0;
            temp[2, 2] = Math.Cos(rad);
            return temp;
        }
        /// <summary>
        /// 向非结构网格当中添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="secnum"></param>
        /// <param name="secpointnum"></param>

        public static void AddDataToTowerOrBladeGrid(ref UnstructuredGridPieceBuilder model, string name, double[] data, int secnum, int secpointnum)
        {
            if (data.Length != secnum)
            {
                LogHelper.ErrorLog("Error AddDataToTowerOrBladeGrid because of data num not equel to secnum");
            }
            var pointData = new DataArrayBuilderFloat64(name);
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < secpointnum; j++)
                {
                    pointData.AddScalarDatum(data[i]);
                }
            }
            model.PointData.DataArrays.Add(pointData);
        }
        /// <summary>
        /// 向模型当中添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fEM"></param>
        /// <param name="secnum"></param>
        /// <param name="secpointnum"></param>
        public static void AddDataToTowerOrBladeGrid(ref UnstructuredGridPieceBuilder model, FEMModel fEM, int secnum, int secpointnum)
        {
            if (fEM.Nnode != secnum)
            {
                LogHelper.ErrorLog("Error AddDataToTowerOrBladeGrid because of FEM.Nnode  not equel to UnstructuredGridPieceBuilder.secnum");
            }
            var disx = new DataArrayBuilderFloat64("Dis x [m]");
            var disy = new DataArrayBuilderFloat64("Dis y [m]");
            var disz = new DataArrayBuilderFloat64("Dis z [m]");
            var disRx = new DataArrayBuilderFloat64("Dis Rx [deg]");
            var disRy = new DataArrayBuilderFloat64("Dis Ry [deg]");
            var disRz = new DataArrayBuilderFloat64("Dis Rz [deg]");

            var Vx = new DataArrayBuilderFloat64("Vx [m/s]");
            var Vy = new DataArrayBuilderFloat64("Vy [m/s]");
            var Vz = new DataArrayBuilderFloat64("Vz [m/s]");
            var VRx = new DataArrayBuilderFloat64("VRx [deg/s]");
            var VRy = new DataArrayBuilderFloat64("VRy [deg/s]");
            var VRz = new DataArrayBuilderFloat64("VRz [deg/s]");

            var Accx = new DataArrayBuilderFloat64("Accx [m/s^2]");
            var Accy = new DataArrayBuilderFloat64("Accy [m/s^2]");
            var Accz = new DataArrayBuilderFloat64("Accz [m/s^2]");
            var AccRx = new DataArrayBuilderFloat64("AccRx [deg/s^2]");
            var AccRy = new DataArrayBuilderFloat64("AccRy [deg/s^2]");
            var AccRz = new DataArrayBuilderFloat64("AccRz [deg/s^2]");


            var Orix = new DataArrayBuilderFloat64("Ori x [m]");
            var Oriy = new DataArrayBuilderFloat64("Ori y [m]");
            var Oriz = new DataArrayBuilderFloat64("Ori z [m]");
            var OriRx = new DataArrayBuilderFloat64("Ori Rx [deg]");
            var OriRy = new DataArrayBuilderFloat64("Ori Ry [deg]");
            var OriRz = new DataArrayBuilderFloat64("Ori Rz [deg]");

            var IFx = new DataArrayBuilderFloat64("IFx [N]");
            var IFy = new DataArrayBuilderFloat64("IFy [N]");
            var IFz = new DataArrayBuilderFloat64("IFz [N]");
            var IMx = new DataArrayBuilderFloat64("IMx [N*m]");
            var IMy = new DataArrayBuilderFloat64("IMy [N*m]");
            var IMz = new DataArrayBuilderFloat64("IMz [N*m]");

            var Fx = new DataArrayBuilderFloat64("Fx [N]");
            var Fy = new DataArrayBuilderFloat64("Fy [N]");
            var Fz = new DataArrayBuilderFloat64("Fz [N]");
            var Mx = new DataArrayBuilderFloat64("Mx [N*m]");
            var My = new DataArrayBuilderFloat64("My [N*m]");
            var Mz = new DataArrayBuilderFloat64("Mz [N*m]");

            for (int i = 0; i < fEM.Nnode; i++)
            {
                for (int j = 0; j < secpointnum; j++)
                {
                    disx.AddScalarDatum(fEM.Nodes[i].Displacement.X - fEM.Nodes[i].XYZ.X);
                    disy.AddScalarDatum(fEM.Nodes[i].Displacement.Y - fEM.Nodes[i].XYZ.Y);
                    disz.AddScalarDatum(fEM.Nodes[i].Displacement.Z - fEM.Nodes[i].XYZ.Z);
                    disRx.AddScalarDatum((fEM.Nodes[i].Displacement.K - fEM.Nodes[i].RXYZ.X) * R2D);
                    disRy.AddScalarDatum((fEM.Nodes[i].Displacement.J - fEM.Nodes[i].RXYZ.Y) * R2D);
                    disRz.AddScalarDatum((fEM.Nodes[i].Displacement.L - fEM.Nodes[i].RXYZ.Z) * R2D);

                    Vx.AddScalarDatum(fEM.Nodes[i].Velocities.X);
                    Vy.AddScalarDatum(fEM.Nodes[i].Velocities.Y);
                    Vz.AddScalarDatum(fEM.Nodes[i].Velocities.Z);
                    VRx.AddScalarDatum(fEM.Nodes[i].Velocities.K * R2D);
                    VRy.AddScalarDatum(fEM.Nodes[i].Velocities.J * R2D);
                    VRz.AddScalarDatum(fEM.Nodes[i].Velocities.L * R2D);

                    Accx.AddScalarDatum(fEM.Nodes[i].Accelerations.X);
                    Accy.AddScalarDatum(fEM.Nodes[i].Accelerations.Y);
                    Accz.AddScalarDatum(fEM.Nodes[i].Accelerations.Z);
                    AccRx.AddScalarDatum(fEM.Nodes[i].Accelerations.K * R2D);
                    AccRy.AddScalarDatum(fEM.Nodes[i].Accelerations.J * R2D);
                    AccRz.AddScalarDatum(fEM.Nodes[i].Accelerations.L * R2D);

                    Orix.AddScalarDatum(fEM.Nodes[i].XYZ.X);
                    Oriy.AddScalarDatum(fEM.Nodes[i].XYZ.Y);
                    Oriz.AddScalarDatum(fEM.Nodes[i].XYZ.Z);
                    OriRx.AddScalarDatum(fEM.Nodes[i].RXYZ.X * R2D);
                    OriRy.AddScalarDatum(fEM.Nodes[i].RXYZ.Y * R2D);
                    OriRz.AddScalarDatum(fEM.Nodes[i].RXYZ.Z * R2D);

                    IFx.AddScalarDatum(fEM.Nodes[i].Interforce.X);
                    IFy.AddScalarDatum(fEM.Nodes[i].Interforce.Y);
                    IFz.AddScalarDatum(fEM.Nodes[i].Interforce.Z);
                    IMx.AddScalarDatum(fEM.Nodes[i].Interforce.K);
                    IMy.AddScalarDatum(fEM.Nodes[i].Interforce.J);
                    IMz.AddScalarDatum(fEM.Nodes[i].Interforce.L);

                    Fx.AddScalarDatum(fEM.Nodes[i].forces.X);
                    Fy.AddScalarDatum(fEM.Nodes[i].forces.Y);
                    Fz.AddScalarDatum(fEM.Nodes[i].forces.Z);
                    Mx.AddScalarDatum(fEM.Nodes[i].forces.K);
                    My.AddScalarDatum(fEM.Nodes[i].forces.J);
                    Mz.AddScalarDatum(fEM.Nodes[i].forces.L);
                }
            }

            model.PointData.DataArrays.Add(disx);
            model.PointData.DataArrays.Add(disy);
            model.PointData.DataArrays.Add(disz);
            model.PointData.DataArrays.Add(disRx);
            model.PointData.DataArrays.Add(disRy);
            model.PointData.DataArrays.Add(disRz);

            model.PointData.DataArrays.Add(Vx);
            model.PointData.DataArrays.Add(Vy);
            model.PointData.DataArrays.Add(Vz);
            model.PointData.DataArrays.Add(VRx);
            model.PointData.DataArrays.Add(VRy);
            model.PointData.DataArrays.Add(VRz);

            model.PointData.DataArrays.Add(Accx);
            model.PointData.DataArrays.Add(Accy);
            model.PointData.DataArrays.Add(Accz);
            model.PointData.DataArrays.Add(AccRx);
            model.PointData.DataArrays.Add(AccRy);
            model.PointData.DataArrays.Add(AccRz);

            model.PointData.DataArrays.Add(Orix);
            model.PointData.DataArrays.Add(Oriy);
            model.PointData.DataArrays.Add(Oriz);
            model.PointData.DataArrays.Add(OriRx);
            model.PointData.DataArrays.Add(OriRy);
            model.PointData.DataArrays.Add(OriRz);

            model.PointData.DataArrays.Add(IFx);
            model.PointData.DataArrays.Add(IFy);
            model.PointData.DataArrays.Add(IFz);
            model.PointData.DataArrays.Add(IMx);
            model.PointData.DataArrays.Add(IMy);
            model.PointData.DataArrays.Add(IMz);

            model.PointData.DataArrays.Add(Fx);
            model.PointData.DataArrays.Add(Fy);
            model.PointData.DataArrays.Add(Fz);
            model.PointData.DataArrays.Add(Mx);
            model.PointData.DataArrays.Add(My);
            model.PointData.DataArrays.Add(Mz);
        }
    }
}
