![Alt](./HawtC.png)
# 关于HawtC 的相关介绍
该软件提供了一个基于C#编译的仿真工具，用于使用凯恩方法分析Spar型浮动海上风力涡轮机的动力学。该模型是使用 5 MW OC3 HyWind Spar 型 FOWT 建立的。然而，这些代码是通用的，适用于不同额定值的风力涡轮机。该程序的输入支持OpenFAST的分布式文件和商业软件Bladed的文件结构，支持全局多目标优化，支持叶片、塔架TMDI设计优化仿真，支持非线性叶片动力学

The software provides a C #compiled simulation tool for analyzing the dynamics of a Spar-type floating offshore wind turbine/(Wind turbine simulation) using the Kane method. The model was built using a 5 MW OC3 HyWind Spar-type FOWT. However, the codes are generic and applicable to wind turbines of different ratings. The input to the program supports OpenFAST's distributed files and the file structure of the commercial software Bladed, supports global multi-objective optimization, supports blade, tower TMDI design optimization simulation, supports nonlinear blade dynamics
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



