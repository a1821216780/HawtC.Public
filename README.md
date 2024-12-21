![Alt](./HawtC.png)
# 关于HawtC 的相关介绍
 OpenHAST.MBD 具有可以拓展,自由组合的优势,是下一代风力机仿真软件的标杆,由赵子祯博士首次提出了部件装配的构建方法,可以自定义的轻松实现多风轮涡轮机以及求解多体动力学行为,例如单个塔筒的涡激震动,按个叶片的震动等,目前任然在开发和实现当中.目标是替代Openfast,成为世界首屈一指的开源国产风力机设计软件的标杆!.
 OpenHAST.HawtC offers the advantages of extensibility and flexible combinations, making it a benchmark for next-generation wind turbine simulation software. Dr. Zhao Zizhen first proposed the component assembly construction method, allowing for easy customization to achieve multi-rotor turbines and solve multi-body dynamic behaviors, such as vortex-induced vibrations of a single tower and vibrations of individual blades. It is currently still in development and implementation. The goal is to replace OpenFAST and become the leading open-source domestic wind turbine design software in the world!
# 0 开发计划安排

## 0.0 2024年软件模块安排

### 0.0.0 模块重构
- <span style="color: green;">&#10004;</span> 初步完成任务 最基本的模块重构,将不含调用类关键字的方法重构为static 将其他方法重构为非static 方法,这样,将会为Hast.Foram的多线程和Hast.MoptL提供内存安全机制,防止计算错误.主要是内存隔离盒安全机制.大幅提高计算效率,现在的计算效率是Bladed的6倍以上,达到了Fast 80% 的计算效率,主要受限于C#的托管内存机制,尽管长度有损耗,但内存管理更加安全,不会内存泄漏!

### 0.0.1 AeroL
- <span style="color: red;">&#10060;</span> 未完成任务 FVW 模块
- <span style="color: red;">&#10060;</span> 未完成任务 接入更多的动态失速模型

### 0.0.2 ApiL
- <span style="color: green;">&#10004;</span>  以完成任务，实现了更加高级的API功能

### 0.0.3 BeamL
- <span style="color: green;">&#10004;</span> 已完成任务 开发动态几何精确梁
- <span style="color: green;">&#10004;</span> 已完成任务 植入静态共旋梁方法
- <span style="color: green;">&#10004;</span> 已完成任务  开发动态共旋梁模型,耦合工作正在进行
- <span style="color: red;">&#10060;</span> 未完成任务  向 MBD 模块当中耦合

### 0.0.4 PostL
- <span style="color: red;">&#10060;</span> 未完成任务  支持将计算结果直接写入到Excle文件当中,处理极限载荷
- <span style="color: red;">&#10060;</span> 未完成任务  支持将计算结果直接写入到Excle文件当中,处理疲劳载荷

### 0.0.4 MBD
- <span style="color: red;">&#10060;</span> 未完成任务  逐步支持双机头风机建模
- <span style="color: green;">&#10004;</span>  以完成任务  优化塔架和叶片TMDI 模型
- <span style="color: red;">&#10060;</span> 未完成任务  增加TMDI接口,为后期的TLD等其他被动减振方法提供开发接口

### 0.0.5 SubFEML
- <span style="color: red;">&#10060;</span> 未完成任务  支持地震波的导入,支持更多的基础模型(这个非常简单)

### 0.0.6 ControL
- <span style="color: green;">&#10004;</span> 已完成任务 支持Bladed DLL 控制器
- <span style="color: green;">&#10004;</span> 已完成任务 支持DTU DLL 控制器
- <span style="color: green;">&#10004;</span> 已完成认为 支持TUB控制器

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




