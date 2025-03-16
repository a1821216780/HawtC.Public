

//**********************************************************************************************************************************
//LICENSING
// Copyright(C) 2021, 2025  TG Team,Key Laboratory of Jiangsu province High-Tech design of wind turbine,WTG,WL,赵子祯
//
//    This file is part of OpenWECD.PCS
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

using MathNet.Numerics.LinearAlgebra;
using SourceGeneration.Reflection;

namespace OpenWECD.PreComp
{
    /// <summary>
    /// 叶片铺层的主文件结构体
    /// </summary>
    [SourceReflection]
    public struct PreCompL1
    {
        public string path;

        public string[] data;
        #region 基础配置参数
        /// <summary>叶片总长度（米）</summary>
        public double BladeLength;

        /// <summary>截面总数</summary>
        public int SectionCount;

        /// <summary>材料种类数</summary>
        public int MaterialTypeCount;

        /// <summary>输出格式（1-5）</summary>
        public int OutputFormat;

        /// <summary>是否使用制表符分隔</summary>
        public bool UseTabDelimiter;


        #endregion 基础配置参数

        public BladeSection[] Sections;

        #region 腹板定义
        /// <summary>
        /// 腹板总数（-）! 若无腹板填0
        /// </summary>
        public int Nweb;

        /// <summary>
        ///  腹板最内端对应的叶片截面编号（-）
        /// </summary>
        public int Ib_sp_stn;

        /// <summary>
        /// 腹板最外端对应的叶片截面编号（-）
        /// </summary>
        public int Ob_sp_stn;
        #endregion 腹板定义
        public WebStructure[] Webs;

        /// <summary>
        /// 材料信息
        /// </summary>
        public CompositeMaterial[] Material;
    }

    /// <summary>
    /// 叶片截面定义
    /// </summary>
    [SourceReflection]
    public struct BladeSection
    {
        /// <summary>归一化跨度位置（0-1）</summary>
        public double SpanLocation;

        /// <summary>气动中心位置（弦长比例 0-1）</summary>
        public double AerodynamicCenter;

        /// <summary>弦长（米）</summary>
        public double ChordLength;

        /// <summary>气动扭转角（度）</summary>
        public double TwistAngle;

        /// <summary>翼型数据文件路径</summary>
        public string AirfoilFile;

        /// <summary>铺层定义文件路径</summary>
        public string LayupFile;

        /// <summary>
        /// 翼型数据结构体
        /// </summary>
        public Af_shape_file Airfoil;

        /// <summary>
        /// 上表面铺层
        /// </summary>
        public SurfaceConfiguration TopConfiguration;

        /// <summary>
        /// 下表面铺层
        /// </summary>
        public SurfaceConfiguration LowConfiguration;

        /// <summary>
        /// 腹板层的定义
        /// </summary>
        public SurfaceConfiguration WebConfiguration;

    }
    /// <summary>
    /// 叶片翼型文件的定义
    /// </summary>
    [SourceReflection]
    public struct Af_shape_file
    {
        /// <summary>
        /// 节点数
        /// </summary>
        public int N_af_nodes;
        public Vector<double> X;
        public Vector<double> Y;
        /// <summary>
        /// 上表面翼型的x坐标
        /// </summary>
        public Vector<double> Xu;
        /// <summary>
        /// 上表面翼型的y坐标
        /// </summary>
        public Vector<double> Yu;

        /// <summary>
        /// 上表面翼型的x坐标
        /// </summary>
        public Vector<double> Xl;
        /// <summary>
        /// 上表面翼型的y坐标
        /// </summary>
        public Vector<double> Yl;
    }
    [SourceReflection]
    public struct CompositeMaterial
    {

        /// <summary>材料ID</summary>
        public int Id;

        /// <summary>纵向弹性模量 (Pa)</summary>
        public double E1;

        /// <summary>横向弹性模量 (Pa)</summary>
        public double E2;

        /// <summary>面内剪切模量 (Pa)</summary>
        public double G12;

        /// <summary>主泊松比</summary>
        public double Nu12;

        /// <summary>密度 (kg/m³)</summary>
        public double Density;

        /// <summary>材料名称</summary>
        public string Name;
    }
    /// <summary>
    /// 腹板结构定义
    /// </summary>
    [SourceReflection]
    public struct WebStructure
    {
        /// <summary>腹板编号</summary>
        public int WebId;

        /// <summary>内端弦向位置（0-1）</summary>
        public double InnerChordPosition;

        /// <summary>外端弦向位置（0-1）</summary>
        public double OuterChordPosition;
    }

    #region 铺层定义的一对文件
    /// <summary>
    /// 表面配置
    /// </summary>
    [SourceReflection]
    public struct SurfaceConfiguration
    {
        /// <summary>
        /// 表面扇区数量
        /// </summary>
        public int N_scts;

        /// <summary>
        /// 定义扇区边界的弦向归一化位置
        /// </summary>
        public double[] xsec_node;

        /// <summary>
        /// 表面扇区的定义
        /// </summary>
        public SectorLaminae[] Sector;

    }
    /// <summary>
    /// 扇区层压板定义
    /// </summary>
    [SourceReflection]
    public struct SectorLaminae
    {
        /// <summary>
        /// 扇区编号
        /// </summary>
        public int Sect_num;

        /// <summary>
        /// 铺层层数
        /// </summary>
        public int N_laminas;

        /// <summary>
        /// 当前扇区的压板层
        /// </summary>
        public Laminae[] Laminae;
    }
    /// <summary>
    /// 单层材料定义
    /// </summary>
    [SourceReflection]
    public struct Laminae
    {
        /// <summary>层编号</summary>
        public int LayerId;
        /// <summary>铺层数量</summary>
        public int PlyCount;

        /// <summary>单层厚度（米）</summary>
        public double Thickness;

        /// <summary>纤维方向（度）</summary>
        public double FiberOrientation;

        /// <summary>材料ID</summary>
        public int MaterialId;
    }
    #endregion 铺层定义的一对文件














    /// <summary>
    /// PreComp输入参数结构体 - 定义叶片截面材料铺层和几何特性
    /// </summary>
    [SourceReflection]
    public struct Input
    {
        /// <summary>弦长 (米)</summary>
        public double Chord;

        /// <summary>气动扭转角 (度)</summary>
        public double TwAeroD;

        /// <summary>结构预扭角 (度)</summary>
        public double TwPrimeD;

        /// <summary>前缘位置 (弦长比例，0-1)</summary>
        public double LeLoc;

        /// <summary>截面X坐标节点数组 (米)</summary>
        public double[] XNode;

        /// <summary>截面Y坐标节点数组 (米)</summary>
        public double[] YNode;

        /// <summary>材料纵向弹性模量数组 (Pa) - 对应材料ID</summary>
        public double[] E1;

        /// <summary>材料横向弹性模量数组 (Pa) - 对应材料ID</summary>
        public double[] E2;

        /// <summary>材料剪切模量数组 (Pa) - 对应材料ID</summary>
        public double[] G12;

        /// <summary>材料泊松比数组 - 对应材料ID</summary>
        public double[] Anu12;

        /// <summary>材料密度数组 (kg/m³) - 对应材料ID</summary>
        public double[] Density;

        /// <summary>顶部表面扇区边界弦向位置 (0-1)</summary>
        public double[] XSecNodeU;

        /// <summary>顶部各扇区层数数组</summary>
        public int[] NLaminaU;

        /// <summary>顶部各层铺层数数组</summary>
        public double[] NPliesU;

        /// <summary>顶部各层单层厚度数组 (米)</summary>
        public double[] TLamU;

        /// <summary>顶部各层纤维方向角数组 (度)</summary>
        public double[] ThtLamU;

        /// <summary>顶部各层材料ID数组</summary>
        public int[] MatLamU;

        /// <summary>底部表面扇区边界弦向位置 (0-1)</summary>
        public double[] XSecNodeL;

        /// <summary>底部各扇区层数数组</summary>
        public int[] NLaminaL;

        /// <summary>底部各层铺层数数组</summary>
        public double[] NPliesL;

        /// <summary>底部各层单层厚度数组 (米)</summary>
        public double[] TLamL;

        /// <summary>底部各层纤维方向角数组 (度)</summary>
        public double[] ThtLamL;

        /// <summary>底部各层材料ID数组</summary>
        public int[] MatLamL;

        /// <summary>腹板弦向位置数组 (0-1)</summary>
        public double[] LocWeb;

        /// <summary>腹板层数数组</summary>
        public int[] NLaminaW;

        /// <summary>腹板各层铺层数数组</summary>
        public double[] NPliesW;

        /// <summary>腹板各层单层厚度数组 (米)</summary>
        public double[] TLamW;

        /// <summary>腹板各层纤维方向角数组 (度)</summary>
        public double[] ThtLamW;

        /// <summary>腹板各层材料ID数组</summary>
        public int[] MatLamW;
    }

    /// <summary>
    /// PreComp输出结果结构体 - 包含截面力学特性
    /// </summary>
    [SourceReflection]
    public struct Output
    {

        /// <summary>挥舞刚度 (N·m²)</summary>
        public double EiFlap;

        /// <summary>摆振刚度 (N·m²)</summary>
        public double EiLag;

        /// <summary>扭转刚度 (N·m²/rad)</summary>
        public double GJ;

        /// <summary>轴向刚度 (N)</summary>
        public double EA;

        /// <summary>挥舞方向剪切中心坐标 (米)</summary>
        public double SFl;

        /// <summary>前缘剪切中心坐标 (米)</summary>
        public double SAf;

        /// <summary>后缘剪切中心坐标 (米)</summary>
        public double SAl;

        /// <summary>前缘拉伸中心坐标 (米)</summary>
        public double SFt;

        /// <summary>后缘拉伸中心坐标 (米)</summary>
        public double SLt;

        /// <summary>扭转中心坐标 (米)</summary>
        public double SAt;

        /// <summary>剪切中心X坐标 (米)</summary>
        public double XSc;

        /// <summary>剪切中心Y坐标 (米)</summary>
        public double YSc;

        /// <summary>扭转中心X坐标 (米)</summary>
        public double XTc;

        /// <summary>扭转中心Y坐标 (米)</summary>
        public double YTc;

        /// <summary>单位长度质量 (kg/m)</summary>
        public double Mass;

        /// <summary>挥舞惯性矩 (kg·m²)</summary>
        public double FlapIner;

        /// <summary>摆振惯性矩 (kg·m²)</summary>
        public double LagIner;

        /// <summary>扭转惯性矩 (kg·m²·deg)</summary>
        public double TwInerD;

        /// <summary>质心X坐标 (米)</summary>
        public double XCm;

        /// <summary>质心Y坐标 (米)</summary>
        public double YCm;

        /// <summary>
        /// 机构扭转角【rad】
        /// </summary>
        public double str_tw;
    }
}
