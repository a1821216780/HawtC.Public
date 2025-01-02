![Alt](./HawtC.png)
# 关于HawtC 的相关介绍
 OpenHAST.MBD 具有可以拓展,自由组合的优势,是下一代风力机仿真软件的标杆,由赵子祯博士首次提出了部件装配的构建方法,可以自定义的轻松实现多风轮涡轮机以及求解多体动力学行为,例如单个塔筒的涡激震动,按个叶片的震动等,目前任然在开发和实现当中.目标是替代Openfast,成为世界首屈一指的开源国产风力机设计软件的标杆!.
# 0 开发计划安排

### 2025-2030 HawtC2 Beta 5年开发计划
从这个版本开始，软件版本号将以2.0开始，未来5年都会是HawtC2的开发进程，之后是HawtC3以此类推，进展顺利，我将推出UI版本
#### 开发目标：
实现商软Bladed95%以上的功能（HawtC已经实现了80%,剩下的都是难啃的骨头），且提高软件的易用性，逐步剥离FAST的函数编程方法，实现模块化-对象化-开发接口化的目标，同时完整覆盖IEC要求的所有工况计算和后处理要求。
#### 开发规划
- <span style="color: green;">&#10004;</span>  1、将AeroL 当中的叶片（塔架）几何与结构输入文件整合，气动与结构文件将统一，便于输入（计划以Bladed和Qblade为模版和蓝图），该处改进是为了HawtC2拥有开源代码，商业易用的特点。

- <span style="color: green;">&#10004;</span>  2、进一步开发CLI和DLL模块，这个模块有利于外部语言的调用，并计划给出原生的Matlab.Simlink 和水动力 AQWA 接口实现。

- <span style="color: green;">&#10004;</span>  3、耦合了我论文第一个创新点，关于实测数据驱动的复杂风环境风谱模型，SimWind模块将不仅仅支持IFFT，还将支持谐波叠加法，以加快湍流风的合成速度，同时在Hawtc的基础上，支持相干系数的自定义（这个工作已经完成）

- <span style="color: green;">&#10004;</span>  4、耦合了我论文第二个创新点，高精度且高效的非线性共旋梁理论，由于柔性结构和多刚体动力学的复杂耦合，我将先实现一个考虑叶片扭转的基础模态法（相比OpenFAST不考虑扭转来说，支持了更高的精度）。之后开发Timoshenko 梁的有限元线性单元，这个方法在商业软件Bladed当中也有实现。这个实现的目的实现是验证耦合的准确性，其次弥补OpenFAST当中无法考虑柔性叶片扭转导致气动攻角的变化的缺点，这对于长柔叶片来说异常重要，不可忽略。之后逐步添加CR共旋非线性和GEBT几何精确梁方法。

- <span style="color: green;">&#10004;</span>  5、耦合和文章的第三个创新点，即叶片TMDI减振与整机多目标Mopt模块。现有功能也完全满足要求，但是多线程的计算还是有待改进，计划添加商软类似的Batch模块，基于Excle文件，完成自己DLC1.1,1.2等多工况的批量计算和极限载荷后处理（疲劳暂时不考虑）

- <span style="color: green;">&#10004;</span>  6、进一步的改进MBD多体动力学模块，一个重要的计划是添加变桨轴承的建模，OpenFAST当中对轴承没有建模，而商软Bladed则考虑了轴承，为了提高模型精度，我们将在HawtC2当中实现这一点。

- <span style="color: green;">&#10004;</span>  7、我将会简化控制器模块，并在其中增加ERROR处理，例如变桨系统的卡顿或者高速运动等IEC当中规定的异常工况，这些功能将有助于软件覆盖IEC工况要求。解决现有OpenFAST无法处理复杂工况的缺陷。

- <span style="color: green;">&#10004;</span>  8、在我看来，未来的风电肯定是海上风电为主，由于大型化不能无限制且吊装困难，类似明阳的双机头可能会成为主流，我也计划在HawtC2当中增加对双机头模型的支持，但是不会考虑提供好多个叶轮的那种支持，找我看来，双机头有缆线简单，科室力互抵以及风轮反转带来的气动增益等诸多好处。同时，为了不影响计算性能，不支持多机头的主要原因是过渡模块化会有性能损失。现有HawtC的速度是OpenFAST的60%.重要的原因就是我们对输出进行了模块化，使得我们的软件和BLaded的输出类似，易于用户查找处理，但代价就是性能损失。

- <span style="color: green;">&#10004;</span>  9、为了配合双机头，气动的BEM理论肯定不能用了，因为其无法考虑反转的性能增益，但是也可以通过修正模型改进（这也是一个论文方向）。为此我们需要实现自由涡尾迹的气动力计算，这部分将会交给社区的其他人来完成，我们也有Matlab代码，但是性能非常低下。

- <span style="color: green;">&#10004;</span>  10、HawtC2 将对水动力部分进行加强！一方面目前的软件已经支持了MoorDyn的调用，但是无法获取实时的系泊力，这意味着无法由软件控制输出，因此系泊模块将会独立开发，以Openmoor为模版和基础，开发MoorL以实现系泊系统的自主可控。其次HawtC的一个严重缺点是只能使用Morsion方程计算水动力，只能计算Spar平台，我将结合WIMAT当中的计算结果，加入势流理论，以实现高精度的水动力计算。这部分的输入文件将会参考Bladed。在开发初期，我还是会考虑将HydroLDyn编译为dll以供软件调用，这样会先实现水动力。之后在逐步开发MoorL和自有的Hydro模块。

- <span style="color: green;">&#10004;</span>  11、对于地震和桩土模型，我也提供了开发接口，但是一致没有开放给大家，HawtC2将开始逐步支持这些计算。

- <span style="color: green;">&#10004;</span>  12、我将对仿真进行类包装，并将环境部分的接口统一起来，包括风和浪流，这样就可以为Farm的植入提供基础。未来会学习DWM的相关理论，并想办法耦合WRF，这样就可以实现大气预报模式与风场的耦合计算，这种计算是中等保真度的，但是速度远快于CFD。在实现FARM后，我将会和MoptL多目标优化模块耦合，这样就可以实现风场级的优化，而不是低保真度的工程尾流模型。

- <span style="color: green;">&#10004;</span>  13、加强开源和社区合作，尽管刚刚起步，没有多少人，欢迎感兴趣的加入。在读的话我感觉你们还是用FAST吧，没有时间搞。除非FAST实现不了你的目标

- <span style="color: green;">&#10004;</span>  14、性能优化，目前我已经在HawtC当中实现了原生AVX多指令集的支持，同时也增加了MPI模块以提供HPC计算集群的支持，这对于大型风电场的设计非常重要！！，FAST.Farm当中尽管支持了多线程但是没有HPC支持，因此要求风机数量小于计算机核心数量-1。为了在高性能计算集群上计算，MPI的开发是必要的。

- <span style="color: green;">&#10004;</span>  15、人工智能技术的应用，现在HawtC已经实现了在MoptL模块当中采用代理模型对整机一体化优化进行加速的方法，这些是人工智能的初级应用。如果有资金支持，可以采用现在的通用大模型，建立风力机知识库，实现模型的自动建立，自动纠错以及自动评价模拟。这些计算是未来建立单机-风场实时数据域人工智能驱动的数字化的必然之路和发展目标。HawtC2将集合开源社区的技术支持，实现通用大模型的支持（如果资金允许），这也是未来的重点发展之路。我认为HawtC和未来风能发展遵循以下路径：

- <span style="color: green;">&#10004;</span> 单机高精度模拟->风场级模拟->实时数据驱动仿真->通用人工风力机知识库


#### 总结

	作为一个即将毕业的学生和王同光教授与王珑教授的弟子，我非常感谢两位老师的培养和鼓励，让我从一个不了解风电到从事风电行业的人感到学以致用四个字的重要性。我的愿望是实现我们国人开发的非套壳的比肩BLaded的专业软件，论文再多，也要落到实处，否则都是浮云。HawtC是我自己完全写的代码，大约有5,6万行了，通过学习这些编程和理论知识，让我了解了很多，也发现了自己的不足，个人能力有限，期待大家加入HawtC2的开发团队中，作为核心耦合的MBD模块，我将在完成且通过测试后，向大家提供开发接口文档和代码，我也会维护开发团队的科研成果，严禁侵犯知识产权！！！ 

##### HawtC2 开发进度

## OpenWECD.HawtC2
全面向商业Bladed与商业Hawc2 学习进化！！！，实现操作逻辑，功能化和模型标准化。
### Beta 版 V2.0.000
<span style="color: green;">&#10004;</span> 表示已经开发完毕
<span style="color: red;">&#10006;</span> 表示在开发列表当中，但还没有开始开发
<span style="color: blue;">&#10010;</span> 表示正在开发当中
<span style="color: brown;">&#10020;</span> 表示部分开发完成
##### 1、AEROL
<span style="color: green;">&#10004;</span> 1、MBD 与AeroL进行了统一，现在的叶片气动结构和塔架气动结构的建模方法与Bladed完全一致，实现了气动结构文件的统一输入
<span style="color: green;">&#10004;</span> 2、对 UnSteadyBEMT 当中的tau1因子取绝对值，防止在轴向诱导因子过大时，造成迭代失败
<span style="color: green;">&#10004;</span> 3、对叶尖修正因子当中的函数进行错误检查，防止出现叶尖损失因子为无穷的问题。
<span style="color: green;">&#10004;</span> 4、Unsteady BEMT 方法类增加了迭代初始化函数，加快了气动力的初始化，解决了海上平台大运动下气动载荷不收敛的问题
<span style="color: green;">&#10004;</span> 5、大柔性叶片的气动载荷严重依赖翼型参数，在这个版本当中,我参考了BLaded的翼型定义方法，方便用户实现翼型依赖于厚度的自动插值（Bladed需要人工定义插值）
<span style="color: red;">&#10006;</span> 6、同样的这个版本也将支持雷诺数的插值。（该功能依赖于我开发的OpenWECD.IO.Numerics 提供数值支持），目前我考虑的是风机运行速度不同的雷诺数时刻变化，我应该是实时插值，还是预先计算静态转数然后静态插值。
<span style="color: green;">&#10004;</span>  7、对气动中心进行插值重构。大型风电叶片的大厚度翼型的气动中心并不在25%处，这意味着，对于变桨中心的相对位置处理不能统一按照25%处理。在这个版本当中我们将对气动中心进行处理优化。以应对长柔叶片的的气动力精确计算（经过数值对比，结果与FAST一致）

##### 2、MBDV2
<span style="color: green;">&#10004;</span> 1、MBD 增强了对机舱的建模，现在机舱的建模方法与Bladed一致，可以计算机舱的空气动力学
<span style="color: green;">&#10004;</span> 2、MBD 进行了重构，删除了大量不必要的参数，增加了与Bladed使用习惯一致的建模方法
<span style="color: green;">&#10004;</span> 3、MBD 与AeroL进行了统一，现在的叶片气动结构和塔架气动结构的建模方法与Bladed完全一致，实现了气动结构文件的统一输入
<span style="color: blue;">&#10010;</span> 4、MBD 支持了大平台运动，防止小角度假设的误差,与OpenFAST4.0.0一致

##### 3、VTKL
<span style="color: green;">&#10004;</span> 1、实现了VTK对轮毂的建模，支持了统一的模型输出
<span style="color: green;">&#10004;</span> 2、对VTK进行高阶建模，使用系统计算的法向量对构建进行建模，修复了机舱错误运动显示的bug

##### 4、HydroL
<span style="color: green;">&#10004;</span> 1、HydroL 当中对浸泡在水当中的塔架进行了建模，可以对塔架添加附加质量
<span style="color: green;">&#10004;</span> 2、HydroL 和ControL 当中对外部控制器的调用时间步长进行了优化，形状支持了更加精细的控制，对于水动力来说，这个改进提高了模型的精度
<span style="color: green;">&#10004;</span> 3、对 HydroL.Wave 当中的Matlab格式波浪文件进行了改进，现在支持了AOT模式下的计算和Matlab当中的读取绘图
<span style="color: green;">&#10004;</span> 4、对 HydroL.Wave 对波浪与Spar基础的配置进行了重构以防止波浪文件与平台不一致
<span style="color: green;">&#10004;</span> 5、修改了HydroL.Wave模块生成波浪文件的格式，现在波浪文件由double转换为float,与风文件的数值类型一致，大幅降低了漂浮式风力机长时序防止的内存消耗，降低了50%内存耗费。
##### 5、WINDL
<span style="color: green;">&#10004;</span> 1、SimWIND 增加了谐波叠加法生成风文件的功能（但是好像有bug)

##### 6、全新SoilL模块
<span style="color: red;">&#10006;</span> 1、全新的SoilL 支持了三种桩土模型

##### 7、全新SubFEML模块
<span style="color: red;">&#10006;</span> 1、SubFEML 继承了FEML当中的函数，照成了基础的Truss框架动力学
<span style="color: red;">&#10006;</span> 2、SubFEML 当中更新和支持了地政模块，支持地震数据频域和时域计算

##### 8、全新BeamL模块
<span style="color: brown;">&#10020;</span> 1、使用共旋法建立非线性结构动力学矩阵，并对叶片进行模态分析。这个模块是Bladed当中的Flexibility模块模态分析的替代。
<span style="color: green;">&#10004;</span> 2、提供了多体动力学的精准接口，从理论角度来看，该方法比hawc2和4.16之前的bladed拥有更高的精度。这是因为我们的模型考虑了Bladed当中的锥形效应，因此，从理论精度来说：Blade4.16&gt;HawtC2.CR=openfast.beamdyn(GEBT)&gt;Bladed&gt;openfast.elasdyn=HawtC2.AMM
<span style="color: green;">&#10004;</span> 3、提供3D梁单元非线性静力学计算
<span style="color: brown;">&#10020;</span> 4、提供2D梁单元非线性动力学计算
<span style="color: brown;">&#10020;</span> 5、提供3D梁单元非线性动力学计算

##### 9、全新ControL模块
<span style="color: red;">&#10006;</span> 1、ControL与MBDV2联合，这个全新的版本当中实现了Bladed.Control.TurbineFaults界面下的风机运行错误功能，支持了Bladed的变桨错误、发电机错误（部分实现）、以及偏航错误（部分实现）
<span style="color: red;">&#10006;</span> 2、ControL与MBDV2联合，这个全新的版本当中实现了Bladed.Control.SafetySystem.Trips 界面下的风机安全链当中的部分功能。

##### 10、全新Calculations模块(Hst主文件)
<span style="color: green;">&#10004;</span> 1、主文件直接运行Bladed.Simulations.PowerProductionLoading 功能而无需进一步更改模型
<span style="color: green;">&#10004;</span> 2、主文件直接运行Bladed.Simulations.Start 功能而无需进一步更改模型
<span style="color: green;">&#10004;</span> 3、主文件直接运行Bladed.Simulations.Iding 功能而无需进一步更改模型
<span style="color: green;">&#10004;</span> 4、主文件直接运行Bladed.Simulations.Parked 功能而无需进一步更改模型
<span style="color: brown;">&#10020;</span> 5、主文件直接运行Bladed.Simulations.NormalStop 功能而无需进一步更改模型
<span style="color: brown;">&#10020;</span> 6、主文件直接运行Bladed.Simulations.EmergencyStop 功能而无需进一步更改模型
<span style="color: green;">&#10004;</span> 7、主文件直接运行Bladed.SteadyCalcuiations.PerformanceCoefficients(当前不考虑叶片柔性) 功能而无需进一步更改模型
<span style="color: green;">&#10004;</span> 8、主文件直接运行Bladed.SteadyCalcuiations.SteadyPowerCurve(当前不考虑叶片柔性) 功能而无需进一步更改模型

使用问题请前往论坛反馈:
http://www.openwecd.fun:22304/member.php?mod=logging&action=login&referer=http%3A%2F%2Fwww.openwecd.fun%3A22304%2Findex.php

HawtC dll的开发以及供python和aqwa调用的方法和实现可前往：
http://www.openwecd.fun/index.php/hawtc-dll-aqwa%e3%80%81matlab%e3%80%81python%e3%80%81c%e3%80%81c-%e6%8e%a5%e5%8f%a3/

往期更新日志前往：
http://www.openwecd.fun/index.php/%e4%b8%8b%e8%bd%bd-openwecd-qhast2/

理论与编程手册前往：
http://www.openwecd.fun/index.php/%e5%85%b3%e4%ba%8e/openhast-%e7%90%86%e8%ae%ba%e6%89%8b%e5%86%8c/

使用课程可前往：
http://www.openwecd.fun/index.php/%e8%af%be%e7%a8%8b/

数学加速器可前往：
http://www.openwecd.fun/index.php/%e4%b8%8b%e8%bd%bd-openwecd-numerrics/



## 与OpenFAST的对比优势与缺点
### 优点：
- <span style="color: green;">&#10004;</span> 1、叶片动力学采用了共旋非线性方法和线性化的模态分析方法，提高更加高效的非线性计算
- <span style="color: green;">&#10004;</span>  2、MBD统一采用了Kanes方法，支持双机头等自定义模型的分析（只有接口，没有时间实现)
- <span style="color: green;">&#10004;</span>  3、更加广泛的文件输出接口，开发包提供了tool自定义输出工具，支持用户自定义输出。采用了与bladed类似的输入和输出文件系统，方便管理
- <span style="color: green;">&#10004;</span>  4、采用了商业软件的模型集成技术，模型采用yaml编码和fast的分布式两种代码，有效解决了模型混乱
- <span style="color: green;">&#10004;</span>  5、支持整机的一体化优化设计，模型当中的任意参数都可以作为优化目标，支持多线程技术，无需代码，即可完成优化设计（我的视频里面以叶片TMDI作为案例）
- <span style="color: green;">&#10004;</span>  6、支持叶片、塔架陀螺TMDI减振动力学建模 。
- <span style="color: green;">&#10004;</span>  7、提供外部接口，方便代码植入。（目前我准备提供aqwa接口和aerodyn接口，以弥补缺点)。其他基本差不多。
### 缺点：
- <span style="color: red;">&#10060;</span> 1、计算速度没有FAST快，bachmark看了一下，大约是FAST的60% 。
- <span style="color: red;">&#10060;</span> 2、水动力计算功能匮乏（我不是搞水动力的）
- <span style="color: red;">&#10060;</span> 3、气动力计算还有待改进（只支持BEMT,FVM等待有缘人协助）。
### 开发的原因与心得：
作为一个起步阶段的“作品”，还是有很大的改进空间，开发初衷是因为FAST二次开发，对于某些功能来说太困难了，不是面向对象的程序理解困难，FAST的fortran的语言开发调试速度慢，采用现代化的语言，开发速度显著增加。基于这些客观条件我就做了一些工作。只是对这方面感兴趣，玩的。和人家团队相比还是逊色不少，有待群里面的各位专家批评改进。如果不是必须去实现HawtC当中的特定功能，建议采用OpenFAST。该程序作为博士期间的成果，该网站和程序是这一阶段的记录点，等到几十年后，我可以以此回忆青年的时光。

## 程序下载：
| 程序  |  下载地址 |
| :------------: | :------------: |
| HawtC 主程序  | http://www.openwecd.fun/update/openhast.zip  |
| MKL 矩阵计算加速器  | http://www.openwecd.fun/update/Inter_MKL_ACC.zip  |
| OpenBLAS 矩阵计算加速器  | http://www.openwecd.fun/update/OpenBlAS_ACC.zip   |
| CUDA 智能风力机矩阵计算加速器  | 内部开发当中  |
| 智能风力机 AIHawtC  | 内部开发当中  |

## HawtC 风力机模型下载：

| 风力机模型  | 类型  | 下载地址  |
| :------------: | :------------: | :------------: |
| 5MW  | Spar 漂浮式  | 请访问论坛  |
| 5MW  |  Onshore 陆上 | 请访问论坛  |
| 15MW  | Onshore 陆上  | 请访问论坛  |
| 22MW  |  Onshore 陆上 | http://www.openwecd.fun/model/HawtC_IEA_22MW_onshore.rar  |
| 22MW整机一体化下的叶片TMDI优化  |  Onshore 陆上 | http://www.openwecd.fun/model/HawtC_IEA_22MW_onshore_BladeTMDI_opt.rar  |

## HawtC 与 OpenFAST 计算验证对比
### 1. 15MW 稳态无风剪切
#### 1）验证结果
http://www.openwecd.fun/data/稳态无风剪切Compare.html
#### 2）验证程序
http://www.openwecd.fun/data/稳态Compare.ipynb

### 4.15MW 湍流风验证

#### 1）验证结果
http://www.openwecd.fun/data/湍流Compare.html
#### 2）验证程序
http://www.openwecd.fun/data/湍流Compare.ipynb


## 开发版当前版本号：2.3.000
###### 1、重构的FEML模块，支持了各向异性梁单元和非线性框架动力学，这意味着，软件支持了高柔框架式塔架和基地的动力学高精度计算。梁的非线性方法采用了本人论文开发的新型各向异性共旋梁方法。而框架则没有采用各向异性单元，这与现实是一致的，因为框架通常为各向均匀的钢管或者水泥等。
###### 2、重构的BeamL模块，作为FEML当中梁部分的增量补充，该模块的主文件将与之前完全不同。
###### 3、重构的和开发中的SubFEML模块，作为FEML当中框架部分的增量补充，该模块的主文件将与之前完全不同。该模块支持线性有限元方法和共旋非线性方法，需要读者阅读开发手册和理论手册进行深入了解。SUbFEML 正在开发和支持地震模块。
###### 4、最近的统一动量模型非常火，我们将在这个版本植入统一动量模型和静态和动态计算方法。
###### 5、如果你想要增加有趣的功能和建议，请联系我们提供支持和帮助！你可以定制功能模块。
#### 正式版 V2.2.405：(VTK 加强和水动力更新)
###### 1、修复了HydroL在平台初始化时的错误，增加了更多错误提醒，仿真文件设置错误导致的问题
###### 2、VTK 模块对于非结构网格增加了celldata的设置，我们为VTK模型增加了颜色，方便我在论文当中直接出图
###### 3、VTK 模块增加了水动力当中Spar模型的绘制，现在VTK支持了叶片、塔架、机舱、Spar平台的绘制
###### 4、修复了VTK在模型平面没有移动时，平面法向量相同而导致叉乘错误的bug,该问题直接导致了初始模型无法正常显示。
###### 5、安装包当中增加了HawtC.Farm但暂时不可以使用，只会输出一个hello world。
###### 6、安装包当中增加了5MW Spar海上漂浮式模型
###### 7、修复了更新模块不显示更新信息的bug
###### 8、VTK当中实现了对HUb的建模,模型更加美观(HawtC2的特性)



#### Beta V2.2.403：(VTK 加强和水动力更新,不支持用户下载，等待正式版)
###### 1、修复了HydroL在平台初始化时的错误，增加了更多错误提醒，仿真文件设置错误导致的问题
###### 2、VTK 模块对于非结构网格增加了celldata的设置，我们为VTK模型增加了颜色，方便我在论文当中直接出图
###### 3、VTK 模块增加了水动力当中Spar模型的绘制，现在VTK支持了叶片、塔架、机舱、Spar平台的绘制
###### 4、修复了VTK在模型平面没有移动时，平面法向量相同而导致叉乘错误的bug,该问题直接导致了初始模型无法正常显示。


#### 正式版 V2.2.402：
###### 1、增加了MPI并行化消息通知库，为 HawtC.Farm 提供并行化基础
###### 2、学习动态尾迹蜿蜒方法（DWM），为HawtC.farm 提供理论基础，试图在未来的版本当中实现复杂地形下的风场级以及大气边界层的计算，在我看来这种技术类似于WRF当中的风机模式，即风力机提取空气的动量。但是WRF的优势就是考虑地形，但是对于风机的建模太过粗糙。
###### 3、更新了程序包当中的OutputParList.xlsx 文件，提供了额外的 HawtC.dll 计算，可以供aqwa，python调用，类似于杨洋老师开发的F2A（Fast to AQWA）。
###### 4、安装包原生提供22MW陆上风力机模型
###### 5、更新了更新模块的显示效果


#### 正式版 V2.2.401：
###### 1、修复了风力机VTK实时显示模块显示异常的bug
###### 2、修复了初始化多体动力学方位角的问题,现在与标准IEC一致（FAST和IEC不一样，相差90度）需要注意
###### 3、修复了HawtC.dll 当中力的获取bug


#### 正式版 V2.2.400（新增SimWind 模块）：
###### 1、优化了全新的HawtC.API 和 MoptL整机一体化优化的联网验证，加快了模拟速度。
###### 2、全新的SimWind 模块，成为取代TurbSim 的湍流生成模块，SimWind 和 IECWind 完整支持了IEC标准，为了之后的整机多工况IEC自动全套仿真提供了环境条件。
###### 3、全新的SimWind 模块，支持了自定义风谱案例。
###### 4、修复了空气动力学的输出bug,现在的输出与openfast基本一致。
###### 5、修复了PresentTime 的输出错误。
###### 6、修复了后处理APIL当中的问题错误


#### 正式版 V2.2.301（存在文件更新，重大更新！！！）：
###### 1、优化了全新的HawtC.API 和 MoptL整机一体化优化的联网验证，加快了模拟速度
###### 2、全新的HawtC.opt 与 MoptL 多目标优化算法库！支持邮件通知
###### 3、修复了空气动力学的输出bug,现在的输出与openfast基本一致
###### 4、主文件系统并入CLI系统
###### 5、Mopt支持了更多了错误改进功能，防止不稳定的模型影响计算

#### 正式版 V2.2.200（存在文件更新，重大更新！！！）：
###### 1、全新的HawtC.API 和 MoptL 完美支持各个模块模型任意参数的整机一体化优化，无需编写代码，适合小白使用!!!!
###### 2、全新的YAML文件系统，现在每个模型只需要一个文件就可以实现，防止类似Openfast的多文件错误！
###### 3、MoptL 支持了单文件模式，后缀名称为.ymlopt
###### 4、MoptL和APIL.pstapi ;APIL.preAPI 支持了单文件模式和新的文件，请访问案例更新
###### 5、修复了MBD.TMDI 的文件验证bug
###### 6、修复了一些IO.YAML文件系统上的bug
###### 7、修复了更新程序逻辑错误的bug

#### 正式版 V2.2.100（存在文件更新）：
###### 1、支持了非定常BEM方法和Oye动态失速模型，修复了其中的bug
###### 2、Oye动态失速模型支持了先关Cl_inv,Cl_st,Cla等参数的自动计算，无需用户输入，简化建模难度，并支持导出，计算结果与HAWC2一致
###### 3、修复了一些bug

#### 正式版 V2.2.030（不存在文件更新）：
###### 1、MBD、ControL、HydroL、AeroL 统一了时间控制器，由MBD当中的ODE求解器统一控制时间间隔dt
###### 2、基于更新1当中的工作，我修复了长期存在的控制器数值稳定性BUG，影响最大的是ControL模块，现在可以稳定计算外部控制器！

#### 正式版 V2.2.020（存在文件更新）：
###### 1、AeroL 支持了塔影效应计算
###### 2、AeroL 支持了动态叶素动量理论
###### 3、AeroL 支持了统一动量模型（开发中 静态和动态开发当中）
#### 正式版 V2.2.010（存在文件更新）：

###### 1、修复了 MBD 当中的错误,现在的计算结果与openfast基本一致，但还在优化当中
###### 2、AeroL 支持了塔架和轮毂气动力计算及其气动力输出

###### 3、BeamL 支持了二维梁的共旋静态和动态非线性计算，以及3D的静态和动态（暂时不开放）非线性计算

###### 4、版本2.2.010 及其之后支持IO 支持了计算完毕或者报错后向您指定邮箱发送通知邮件的功能，需要在HST主文件当中设置，请参阅文件改动

###### 5、QHAST2 在1.0版本之后已经支持了模态法下的叶片TMDI模拟，版本2.4.000 及其之后支持梁动力学与直接法下的叶片TMDI高精度计算


### 国际化更新：
###### 1、增加 yaml 解析器以适配  IEA Wind Task 37 Team 当中的文件一体化要求
###### 2、适配了HAWC2 和 OpenFAST 当中的叶片结构参数
###### 3、支持了最新的主文件格式 .hst 该文件自动解析   IEA Wind Task 37 Team 标注的文件，以生成标准项目文件

### 独家功能：
###### 1、BeamL 支持了共旋梁理论与几何精确梁理论的耦合气弹非线性计算
###### 2、支持了SIMD高级矢量加速，大幅提高计算速度和计算效率，大大快于Bladed 非线性大大快于Openfast。
###### 3、支持了多目标优化，现在你可以轻松实现整机一体化优化技术
###### 4、得利于yaml文件格式，现在可以轻松的实现多目标优化的简便实现


### HawtC.UI(目前只能用于课题组内部使用，外部请申请)
###### 1、增加了UI模块，为学术版的UI界面进行了技术探索，目前稳步推进。将在2026年推出！
###### 2、APIL 与 MoptL 支持了自定义接口，用户只需要点击界面或者编辑简单的输入文件就可以实现整机各个模块的优化，包括：AeroL、BeamL、FEML、WindL、ControL、MBD、HAST、HydroL、SubFEML。


###### 更多信息，请访问 www.openwecd.fun 申请加入开发者成员。获取最新资讯，和技术资讯！


### 文件更新：
#### V2.2.301
##### 1、MoptL 文件更新,支持了邮件通知
```
----------------------  EmailSet  ---------------------------------
false                            AfEmail                 - 是否开启Email
2                                ReMailNum               - 接收邮箱的数量
"smtp.qq.com"                    Host                    - SMTP服务器
25                               Port                    - 端口号
"xxxxxxxx"                       SMIPServiceCode         - 服务器秘钥
"1821216780@qq.com"              SendMailbox             - 秘钥对应的邮箱
"1319073660@qq.com"              RecipientMail           - 接收邮箱，可以设置多个
"1821216780@qq.com"
"............gmail.com"
"..............."
```
#### 2.2.300：
##### 1、AeroL 文件更新，支持了更多关键字
原来的：

```
WakeMod           - Type of wake/induction model (switch) {0=none, 1=BEMT, 2=FreeWake}
```
现在的：
```
WakeMod           - Type of wake/induction model (switch) {0=none, 1=SBEMT, 2=DBEMT,3=FreeWake,4=SBEMT_UMT,5=DBEMT_UMT}
```
其中的UMT后缀表示的是统一动量模型

#### 2.2.010：
##### 1、HST 主文件更新
###### 1、HST主文件删除了AeroELa的气弹计算关键字，现在是否开启气弹计算在MBD当中设置支持

#### 2.2.010：
##### 1、HST 主文件更新
 ###### 1、向HST主文件增加以下代码，以实现邮箱通知功能，必须放在Output之前

```
----------------------  EmailSet  ---------------------------------
false                            AfEmail                 - 是否开启Email
2                                ReMailNum               - 接收邮箱的数量
"smtp.qq.com"                    Host                    - SMTP服务器
25                               Port                    - 端口号
"xxxxxxxx"                       SMIPServiceCode         - 服务器秘钥
"1821216780@qq.com"              SendMailbox             - 秘钥对应的邮箱
"1319073660@qq.com"              RecipientMail           - 接收邮箱，可以设置多个
"1821216780@qq.com"
"............gmail.com"
"..............."
```
##### 2、AeroL 主文件更新
###### 1.  AeroL 支持了更多塔架的输出信息，请查阅输出关键字文件
##### http://www.openwecd.fun/update/QHAST_OutPutParList.xlsx 

#### 2.2.006：
##### 1、HST 主文件更新
###### 1、需要将ELFile关键字改为MBDFile(必要)
###### 2、增加Present预先计算关键字（不必要）




## 源代码支持：
######  请访问论坛
## 开发计划：
######  目前已经完成了很多开发，后续着重进行精度验证（这是根本），基于这些验证修复软件当中的BUG，之后会加入更多功能，个人能力有限，欢迎有兴趣的同学一起完善。

| 功能  | 描述  | 发版计划  |
| :------------: | :------------: | :------------: |
| 模型线性化  | 计算整机稳态，得到整机频率并使用VTK技术实现可视化  |  <p style="color: red;">忙着博士毕业，以后再说。。。。。</p> |
| 支持更多动态失速模型  | -顾名思义 |  <p style="color: red;">忙着博士毕业，以后再说。。。。。</p> |
| 支持GEBT和CR的精确耦合  | -顾名思义 |  <p style="color: red;">忙着博士毕业，以后再说。。。。。</p> |
| UI界面  | 技术已经实现，代码太多，懒得写，寻找有缘人一起 |  <p style="color: red;">忙着博士毕业，以后再说。。。。。</p> |




## HawtC.BeamL非线性梁模型下载：

| 梁模型  | 类型    |动力学特征| 下载地址
| :------------: | :------------: | :------------: | :------------: |
| PreBend 梁  | 复合材料弯扭耦合梁  |  3D非线性静态分析 |http://www.openwecd.fun/model/prebend.rar
| 15MW塔架  | 线性梁  |  3D线性动态分析 |http://www.openwecd.fun/model/prebend.rar

## HawtC.AeroL气动力模型下载，支持Cp,功率曲线等计算：

请参考风力机模型




