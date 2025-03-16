# Guide Applicabilty to HawtC2.PCS

This guide details the internal interface, inputs, outputs, and assumptions, which are applicable to the current code.  The legacy file input and output method is also maintained as is detailed in the test scripts including how to execute the CSharp version of HawtC2.PCS with file based i/o.  However, it is recommended to use the direct call methods as detailed in the index when coupling with other codes, which is required for propagating automatic gradients.

___________

**User’s Guide to HawtC2.PCS** 

THE PreComp User’s Guide written by:
(Pre-Processor for Computing Composite Blade Properties)

Gunjit S. Bir

September 2005 

National Renewable Energy Laboratory, 1617, Cole Blvd, Golden, CO 80401 

## About This Guide

This guide explains data preparation and execution with HawtC2.PCS, a code developed to provide span- variant structural properties for composite blades.  HawtC2.PCS computes these properties with a novel approach that integrates a modified classic laminate theory with a shear-flow approach.  The computed properties include cross-coupled stiffness properties, inertia properties, and offsets of the blade shear center, tension center, and center of mass with respect to the blade pitch axis.  Analysts need these properties to properly model the major flexible components of a wind turbine—blades, tower, and drivetrain shaft.  Almost every aeroelastic code (FAST, ADAMS, BLADED, etc.) requires these properties as inputs.  Designers need these properties to rapidly evaluate alternate composite layouts and their effects on blade properties and material strains.  Structural properties are difficult to extract from 3- D finite element models (FEMs), which are primarily suited to obtaining detailed stress and displacement distributions.  Such models also take time and effort to develop and are typically used during the final design stage. 

Salient features of HawtC2.PCS are accurate computation of blade torsion stiffness and cross-stiffness properties.  The cross-stiffness properties (flap-torsion, lag-torsion, flap-lag, axial-torsion, flap-axial, and lag-axial stiffness) arise if an anisotropic (unbalanced) layup of composite laminates is used.  These properties couple flap, lag, axial, and torsion motions of the blade and can substantially influence the turbine performance, loads, and aeroelastic stability.  Accurate estimates of cross-stiffness properties are of interest to the wind industry members who tailor composites to mitigate turbine loads and enhance performance.  Computation of torsion stiffness and cross-stiffness properties can be tricky even with 3-D FEMs.  We therefore merged a modified classic laminate theory with a shear-flow approach to reliably compute these properties. 

HawtC2.PCS requires that the blade external shape and the internal layup of composite laminates be described for inputs.  The external shape is specified in terms of the chord, twist, and airfoil geometry variation along the blade.  The internal structural layup is specified in terms of the laminates schedule, orientation of fibers in each laminate, and the laminate constituent properties.  The code allows for a general layup of composite laminates, both spanwise and chordwise, and an arbitrary number of webs. 

This guide provides step-by-step instructions on how to prepare input files (specify blade external geometry and internal structural layup of composite laminates), how to execute the code, and how to interpret the output properties.  HawtC2.PCS performs extensive checks for completeness, range, and viability of input data; these are also discussed in this manual.  The code runs fast, usually in a fraction of a second, and requires only a modest knowledge of composites and laminates schedule typically used in blades.

**DOCUMENT REVISION RECORD**



| **Revision** | **Date**      | **Description**                                                                                          |
|--------------|---------------|----------------------------------------------------------------------------------------------------------|
| 1.00        | 15-3-2025   | Modified HawtC2.PCS for more accurate analysis of properties. Included note on shear center computation in the manual. Supports HawtC2.PCS v1.0.000                                                           |



## Introduction

HawtC2.PCS (**P**re-processor for computing **C**omposite blade **S**tructural properties) was developed to compute the stiffness and inertial properties of a composite blade.  The code may also be used to compute the structural properties of a metallic blade by treating it as a special case of an isotropic composite material. All wind turbine aeroelastic codes, such as FAST [1] and ADAMS [2,3], need such properties to model the major flexible components—blades, tower, and drivetrain shaft.  These properties are listed in Table 1 and, in general, vary along the blade span.  A salient feature of HawtC2.PCS is its ability to accurately compute torsion stiffness and cross-stiffness properties.  The cross-stiffness properties (flap-torsion, lag-torsion, axial-torsion, flap-axial, flap- lag, and lag-axial stiffness) arise if an anisotropic (unbalanced) layup of composite laminates is used.  These properties couple flap, lag, axial, and torsion motions of the blade and can substantially influence turbine performance, loads, and aeroelastic stability.  Accurate estimates of cross-stiffness properties are of interest to the wind industry members who tailor composites to mitigate turbine loads and enhance performance.  Composite tailoring involves anisotropic layup of laminas and results in coupling of blade extension, bending, and torsion.  Figure 1 shows one example of this concept; the blade twists nose-down as it flaps up because of material coupling of flap and torsion displacements. 

**Table 1  Blade Structural Properties Required for Aeroelastic Modeling** 


| **Properties Category**     | **Section Properties**                                                                                     |
|-----------------------------|-----------------------------------------------------------------------------------------------------------|
| Direct stiffnesses          | Flap, lag (edgewise), axial, and torsion stiffnesses                                                      |
| Cross-coupled stiffnesses   | Flap-twist, lag-twist, flap-lag, axial-twist, axial-flap, and axial-lag stiffnesses                     |
| Principal axes              | Orientation of principal axes for inertia and for stiffness                                               |
| Inertias                    | Mass, mass moments of inertia about the principal axes                                                    |
| Offsets                     | Shear-center, center-of-mass, and tension-center offsets                                                  |

Finite-element techniques, despite their capability for accurate stress and displacement analysis, cannot yield these properties directly.  One must rely on computationally complex post- processing of force-displacement data.  Blade Properties Extractor, BPE [4], represents one such post-processing tool.  Researchers have tried to use 3-D laminate theories to obtain structural properties directly.  These theories, however, overestimate torsion stiffness by as much as 50−80 times because warping effects are difficult to model and affect the torsion stiffness significantly.  This is especially true for asymmetrical sections that are typified by turbine blades.  Computation of torsion stiffness and cross-stiffness properties can be tricky even with 3-D finite element models (FEMs).  We developed a modified 2-D model and combined it with a shear flow approach, akin to Bredt-Batho’s approach for metallic blades, which implicitly accounts for the dominant warping effects.  HawtC2.PCS uses this approach to compute the torsion stiffness, cross- stiffness properties, and other structural properties. Structural properties listed in the table include geometric offsets such as that of the section center of mass from the shear center.  Unlike the cross-stiffness properties, which cause elastic couplings, these offsets cause dynamic coupling of the blade bending, torsion, and axial motions. 

This objective of this report is to provide guidelines on data preparation and HawtC2.PCS execution.  The inputs for HawtC2.PCS require that the blade external shape and the internal layup of composite laminate be specified.  The external shape is specified in terms of the variation of chord, twist, 

and airfoil geometry along the blade.  The internal structural layup is specified in terms of the laminates schedule, orientation of fibers in each laminate, and the laminate constituent properties.  The code allows for a general layup of composite laminates, both spanwise and chordwise, and an arbitrary number of webs.  The code uses these inputs to compute the sectional inertia and stiffness properties at user-specified stations (sections) along the blade, and outputs these in a tabular form. 

HawtC2.PCS should not be confused with the Preliminary Blade Design Code developed earlier [5,6], which is also based on the classical laminate theory.  The former is an analysis code that provides detailed structural properties. The latter is a design code that provides the thickness of various composite laminates required for ultimate strength and buckling resistance against extreme loads.  HawtC2.PCS has been developed anew and offers features not available in the earlier code:   

-  It allows for anisotropic layup of composite laminates that lead to stiffness cross-couplings.   

-  It allows general variation of laminates scheduling both along the blade span and around a section periphery.   

-  It eliminates the restriction of a circular section near the blade root area and allows for arbitrary section geometries along the whole blade length.   

-  It computes structural properties with an improved laminate theory.   

HawtC2.PCS is not finite element based and therefore cannot provide detailed load-displacement or load-stress distribution the way a sophisticated 3-D finite-element approach such as NuMAD developed for composite wind turbine blades can [7].  However, it directly computes the structural properties and runs fast, usually in a fraction of a second.  It also eliminates the need for an interactive approach and requires only a modest knowledge of composites and laminate layups typically used in blades. 

The HawtC2.PCS approach for computing torsion stiffness, though more accurate than the other direct approaches suggested in the literature, still uses two assumptions—thin-walled sections and free warping—to make the problem analytically tractable.  Though the thin-walled assumption is not a serious limitation, the free warping assumption is obviously violated near a constrained blade root section.  We plan to check the validity of these assumptions and the efficacy of the overall HawtC2.PCS approach with test data from experiments, currently underway at Sandia and NREL, and with analysis data from NuMAD/Ansys FEMs.  Meanwhile, we advise exercising caution until HawtC2.PCS is fully validated.  Limited verification studies show excellent agreement, in particular for the torsion stiffness, between HawtC2.PCS and analytical models (elliptical-, rhombus-, and rectangular-section blades made of isotropic materials).  However, more extensive verifications are required and would welcome any data or feedback from readers. Another note of caution: the shear-center computation in current version of HawtC2.PCS is approximate. We are still investigating if it is meaningful to define a shear center for sections with anisotropic lay-up of composite materials. If we can come out with a meaningful definition, we will modify HawtC2.PCS to compute the shear center more accurately. Otherwise, we will need to model blades as Timoshenko beams and compute the full 6X6 section stiffness matrices. 

This guide is divided into seven sections, including this introduction:   

-  **Section 2** describes the general structural layup of composite laminates used by 

HawtC2.PCS.  An understanding of this layup is essential to prepare input data.   

-  **Section 3** lists the underlying assumptions that will help you understand the applicability 

and limitations of HawtC2.PCS. 

- **Section 4** lists the types of input files and provides step-by-step instructions on input data 

preparation (specifying blade external geometry, internal structural lay-up of laminates, and material properties). 

-  **Section 5** discusses error messages and warnings.      **Section 6** shows how to execute the code.  

-  **Section 7** describes how to interpret the HawtC2.PCS output.  

-  **Section 8** concludes the report with discussions of planned upgrades and verification 

studies. 

## Blade Structural Layup

HawtC2.PCS assumes that the blade is fabricated with composite laminas.  Most of the modern blades, specially the large-turbine blades, are built this way[∗].  For such a blade, irrespective of the 

complexity of its laminas layup, its cross-sections would show stacks of laminas, whose thickness and number are piecewise constant along the section periphery.  Figures 1 and 2 exemplify such layups.  Figure 1 shows a box-type layup in which the two webs, together with the midsections of upper and lower surfaces, form a box.  Figure 2 shows a spar-cap type layup in which the midsections cap the two webs. 

The number of lamina stacks along the section periphery, the number of laminas in each stack, and the thickness of the laminas generally vary along the blade length.  Each figure shows two webs; however, HawtC2.PCS allows any number of webs including zero.  The webs may begin at any section and end at any other section on the blade.  Each web is assumed straight and normal to the chord at each cross-section; its cross-sectional dimensions and composites layup may vary along the blade.  The internal structural layup at a section is thus characterized by the variation of composite laminas that stack along the section periphery and over the web cross-section.  This layup usually varies from section to section.  At any section, we assume that the composite structural layup is within the confines of the section external shape.  External shape at a section is characterized by its chord length and airfoil geometry.

Other blades, e.g. pultruded blades, are built using a different process.  If these blades are characterized by piecewise constant-thickness sectors along the blade periphery, we may treat these sectors as laminas and use HawtC2.PCS to compute their properties. 

## Underlying Assumptions

HawtC2.PCS makes a number of assumptions, an understanding of which will help you scope the applicability and limitations of HawtC2.PCS.  It assumes that: 

- Each blade section is a thin-walled, closed, multicellular section.  This implies a constant shear flow around each cell.  Figure 2 is an example of such a section with three cells.  In general, if we have *n* webs, we should have *n+1* cells. 

- There are no hoop stresses in any wall of a section.  This is quite a valid assumption for physical considerations. 

- The blade is straight (no built-in curvature). 

- Transverse shearing is negligible.  This assumption is fairly valid for a blade whose length far exceeds the transverse dimensions (chord and thickness). 

- The blade sections experience no distortion within the plane of a cross section.  This is also a fairly valid assumption, as borne out by experimental results and FEMs. 

- Each blade section is free to warp out of its plane.  This implies an unconstrained warping throughout the blade length and is a valid assumption except in the immediate vicinity of the blade root, where warping may be constrained by the cantilevered boundary condition.  The effect of constrained warping is usually confined to about one chord length from the root.  A circular section near the root, which typifies most of the wind turbine blades, does not warp, and therefore experiences no effects of constrained warping. 

- The webs at any blade section are normal to the chord.  Therefore, if the blade has a pre-twist, the web twists with the blade. 

- At any section, the composite layup of a web is a single stack of laminas; the stacking though may change from section to section. Most blades are built this way. 

## Input Data Description

You will need to provide the following information before HawtC2.PCS can compute blade structural properties: 

1. Description of the blade external shape, which is defined by: 

   - Blade length. 

   - Chord and built-in aerodynamic twist distribution along the blade. - Section airfoil geometry distribution along the blade. 

2. Description of the blade internal structural layup.  This layup must lie within the external shape of the blade and is defined by: 

   - Number of distinct laminates along the blade periphery.  A laminate is a stack of laminas and is distinguished by the number, sequence, and material of the laminas in that stack.  HawtC2.PCS allows any number of such laminates (at least one each for the upper and lower surfaces).  Figure 2 shows three laminates (sectors) each on the upper and lower surfaces. 

   - Number of webs and their placements within the blade. - Laminas schedule for each web. 

   - The principal material direction, material type, and number of plies in each lamina.  A lamina is composed of similar plies that are usually commercially available.  A ply is characterized by its thickness, material type, and orientation of its principal material direction with respect to the blade axis. 

3. Materials table.  As described in #2, specifying internal structural layup requires identification of composite material for each lamina.  A material is directly identified from a materials table provided with the HawtC2.PCS code.  This table lists typical composite materials and their properties.  You may add new materials and their properties to this table. 

   These data are provided to HawtC2.PCS via four sets of input files:  a main input file, airfoil data files, internal structural data files, and a materials file.  The main input file and airfoil data files help describe the blade external shape and specify number and location of blade sections at which structural properties are computed.  The internal structural data files describe the detailed composite laminar layup at user-selected blade sections.  This material file technically is an input file; it is read by HawtC2.PCS like other input files.  However, it comes with HawtC2.PCS and need not be modified unless you would like to add new-material properties to it. 

   All input files, described later, are written in a simple text format that can be created or modified with any text editor.  Any line in these files either is a comment line, a blank line, or a HawtC2.PCS- readable line.  Comment lines help understanding the data that follow the comment lines (see sample input files, Figures 3-5).  Though one may alter text in a comment line, one should not add or remove any comment line.  Blank lines delineate blocks of data and help clarify data organization.  Like the comment lines, these also cannot be added or deleted.  A readable line can have two formats.  It may have a single value followed by associated parameter name and a brief description of that parameter (the name and its description are not read by HawtC2.PCS). Or, a readable line may have a set of values separated by commas, tabs or spaces; any number of tabs or spaces may be inserted for clarity.  A description of the input files follows. 

### Main Input File

This file can have any name.  An example main input file, called HawtC2.PCS.pci, is shown in Figure 3.  The second line is a title line.  HawtC2.PCS reads this as a character string.  You may insert any text (up to 99characters); this text is repeated in the output file.  Next, we have a blank line followed by a block of general information.  [Table 1] describes the parameters that appear in this data block.  The parameters are identified in *italics*.  The last column in the table identifies the associated units.  A dash indicates a non-dimensional parameter. 

**Table 1.  General Parameters** 


| **Parameter**    | **Description**                                                                                                                                                                                                                                         | **Unit** |
|------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *Bl_length*      | Blade length, the distance between the blade-to-hub root attachment and the blade tip. It is not the rotor radius.                                                                                                                                   | m        |
| *N_sections*     | Number of blade sections at which HawtC2.PCS computes structural properties. You may select any number of sections and place them arbitrarily over the blade length (see Figure 6).                                                                         | -        |
| *N_materials*    | Number of materials whose properties would be read from the materials table, *materials.inp*. It should be less than or equal to the maximum number of materials listed in the table. In the former case, only the first *N_materials* properties are read. | -        |
| *Out_format*     | Integer switch identifying type of the output file. 1: Output file showing all properties computed by HawtC2.PCS is generated. These properties refer to axes systems shown in Figure 13 (described in Section [7]). 2: Output file showing select properties required by BModes code is generated. These properties and associated reference axes are described in Reference [9]. 3: Both output files are generated. | -        |
| *TabDelim*       | A logical switch. If set to *t* or *true*, the output properties, printed as a table, are tab-delimited. Such a table, when exported to a spreadsheet such as Excel, automatically converts to columns. If this switch is set to *f* or *false*, the output properties, printed as a table, are space-delimited. Such a table (see Figure 12) helps easy reading of output properties. In the sample input file, this switch is set to *false*. | -        |

The next input block is for blade-sections-specific data.  The data is entered in six columns and *N\_sections* rows, where *N\_sections* is the number of blade sections defined above.  [Table 2] describes the section parameters, which show as headers of the six columns. 

The last data block provides webs-related information.  Any number of webs may be specified (the sample main input file though shows only two webs).  Each web, assumed straight, may originate at any section (station) of the blade and terminate at any other outboard section.  These sections must, however, be selectable from the sections specified earlier via parameter *Span\_loc* (described in [Table 2]).  Normally, the blades are constructed such that all webs originate at a single station and terminate at another single station.  However, should these originate or terminate at different stations, *Ib\_sp\_stn* should be the location of the innermost end of webs and *Ob\_sp\_stn* the location of the outermost end of webs.  The reason for this will be explained in Section [4.3]. 

**Table 2.  Blade-Section Parameters** 


| **Parameter**        | **Description**                                                                                                                                                                                                                                                                                                                                                     | **Unit** |
|----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *Span_loc*           | This is a subscripted array. *Sloc(i)* is the *i*th-section span location measured from the blade root and normalized with respect to the blade length. The first section is always located at *0.0* and the last at *1.0*. At least two sections must be specified.                                                                                                 | -        |
| *Le_loc*             | This is a subscripted array. At *i*th-section, *le_loc(i)* is the distance of the leading edge from the blade reference axis, measured along and normalized with respect to the chord length at that section. The reference axis is usually selected to coincide with the blade pitch axis.                                                                                | -        |
| *Chord*              | *Chord(i)* is the chord length in meters at the *i*th section.                                                                                                                                                                                                                                                                                                      | m        |
| *Tw_aero*            | *Tw_aero(i)* is the blade twist in degrees at the *i*th section. It indicates the orientation of the chord of the local section with respect to the hub plane (Figure 13). A positive twist moves the section leading edge into the wind.                                                                                                                       | deg      |
| *Af_shape_file*      | For each blade station specified in this input file, you must supply an auxiliary input file that describes the airfoil shape (section external shape) at that station. Any name up to 99 characters may be specified for such a file and must be enclosed within quotation marks (see the sample main input file). *Af_shape_file(i)* represents the name of the auxiliary airfoil shape input file for the *i*th section. The same airfoil shape input file name may be supplied for different sections. For example, as seen in the main input file, Figure 3, the same airfoil shape file name, *af1-6.inp*, is supplied for sections 1 to 6. This implies that these sections have the same external shape. We will explain the airfoil shape input file in Section [4.2]. | -        |
| *Int_str_file*       | For each blade station, you must also name an auxiliary input file that describes the internal structural layup at that station. Any name up to 99 characters may be specified for such a file and must be enclosed within quotation marks. *Int_str_file(i)* represents the name of the internal structural layup input file for the *i*th section. The same internal structural input file name may be supplied for different sections. For example, as seen in Figure 3, the same airfoil shape file name, *int01.inp*, is supplied for all sections. This implies that these sections have the same internal structural layup of composite laminas. We will explain the internal structural layup input file in Section [4.3]. | -        |

**Table 3.  Parameters for the Webs** 


| **Parameter**           | **Description**                                                                                                                                                                                                                       | **Unit** |
|-------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *Nweb*                  | Total number of webs. Each web is assumed to be straight between its inboard and outboard ends.                                                                                                                                     | -        |
| *Ib_sp_stn*             | Blade station at which the inboard end of webs is located. This implies that all webs must originate at the same inboard location. If webs originate at different locations, *Ib_sp_stn* should be the location of the innermost end of webs. | -        |
| *Ob_sp_stn*             | Blade station at which the outboard end of webs is located. This implies that all webs must end at the same outboard location. If webs end at different locations, *Ob_sp_stn* should be the location of the outermost end of webs. | -        |
| *Web_num*               | Web number.                                                                                                                                                                                                                           | -        |
| *Inb_end_ch_loc*        | This subscripted variable with *inb_end_ch_loc(i)* defines the chordwise location of the *i*th web at the inboard blade station, *ib_sp_stn*, specified earlier. This chordwise location is measured from the leading edge and is normalized with respect to the chord length. | -        |
| *Oub_end_ch_loc*        | This subscripted variable with *oub_end_ch_loc(i)* defines the chordwise location of the *i*th web at the outboard blade station, *ob_sp_stn*, specified earlier. This chordwise location is measured from the leading edge and is normalized with respect to the chord length. | -        |

### Airfoil Data File

A complete description of blade geometry needs chord, twist, and airfoil shape distribution along the blade.  You provide the chord and twist distributions in the main input file.  In that file, you also specify the names of files that contain airfoil shapes data at the blade stations.  The number of such files is equal to or less than the number of blade stations specified in the main input file.  For blade stations with the same airfoil shape, though possibly different chord lengths, a single airfoil data file suffices.  Figure 4 is a sample airfoil-data input file.   It has three input parameters described in [Table 4]. 

Note 1:  The leading edge is a first node and must have (0,0) coordinates. 

Note 2:  The x-coordinate of the airfoil nodes must monotonically increase if we trace the upper surface from the leading edge to the trailing edge, and monotonically decrease if we trace the lower surface from the trailing edge toward the leading edge. 

Note 3:  Figure 8 shows admissible and non-admissible airfoil shapes.  Airfoil geometry cannot cross itself.  Also, the airfoil curve segments on the upper and lower surfaces must be single- valued functions, *y\_airfoil=f(x\_af)*.  A blunt trailing edge is admissible. 

The input, *N\_af\_nodes*, is specified in the first field of the first line in the airfoil input file (see Figure 4).  As with all other HawtC2.PCS inputs, spaces or tabs may precede this input.  This is followed by three lines ignored by HawtC2.PCS.  The x- and y-coordinates of the first node, the leading edge, are always *(0,0)* and are specified on line 5.  The following lines specify coordinates of the subsequent nodes. 

**Table 4 .  Airfoil Input File Parameters** 



| **Parameter**   | **Description**                                                                                                                                                                                                                                           | **Unit** |
|------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *N_af_nodes*     | Number of nodes that describe the airfoil shape (see Figure 7). The node numbering begins at the leading edge, moves over the top surface, reaches the trailing edge, moves over the lower surface, and finally arrives at the last node, just below the leading edge. | -        |
| *Xnode*          | Subscripted variable with *xnode(i)* defining the x-coordinate of the *i*th node with respect to *(x_af, y_af)* airfoil reference axes. These reference axes originate at the leading edge with *x_af* directed along the section chord, and the normal, *y_af* axis, pointing toward the upper-surface side (Figure 7). This coordinate is normalized with respect to chord length. The maximum permissible value for an x-coordinate is 1. Two nodes with the same maximum x-coordinate, but different y-coordinates specify a blunt edge. | -        |
| *Ynode*          | Subscripted variable with *ynode(i)* defining the y-coordinate of the *i*th node. This coordinate, like *xnode(i)*, is also normalized with respect to chord length.                                                                                     | -        |

### Internal Structure Data File

The airfoil data files, together with the main input file, completely define the blade’s external shape.  Internal structure data files help us specify the composite layup within the external shape.  Each blade station needs an internal structure data file that describes the blade internal structure as seen at that section.  The name of this file is user specifiable and is identified in the main input file (see Figure 3).  If some sections have similar layups, which is likely for most blades, a single internal structure data file may be used for these sections.  As an example, in our sample main input file, a single structural layup file, *int01.inp*, describes the layup for all sections. 

Before reading the description of the internal structure data file, see Figure 2, which shows how we idealize the internal structure layup.  First, we identify the upper and lower surfaces, which are the upper and the lower parts, respectively, of the section periphery between the leading and the trailing edges.  Each surface is divided into sectors.  A sector is a laminate, i.e., a stack of laminas of different composite materials and principal material directions.  Each lamina is composed of similar plies, which are typically commercially obtained.  Figure 2 shows three sectors each on the upper and lower surfaces.  HawtC2.PCS, however, allows different and arbitrary numbers of sectors for the two surfaces.  The thick middle sectors, seen on the upper and lower surfaces in the figure, are called *spar-caps* in blade manufacturing jargon.  These provide the main bending stiffness and strength.  Some optimized blade structures may show stepped spar- caps with different number and placement of webs.  The sample layup in the figure shows that the upper middle sector is composed of eight laminas (view AA) and the web is composed of five laminas (view BB).  The thick middle lamina in each view is usually a light material such as balsa that resists panel buckling. 

Now we describe the internal structure data file (Figure 5 is a sample file).  The length of this file may differ from section to section, but its format―sequence of comment, blank lines, and data blocks―stays the same.  We enter data in a hierarchical fashion—first for the upper surface, next for the lower surface, and finally for the webs.  For the upper surface, for example, we specify number of sectors, number of laminas in each sector, number of plies in each lamina, and constituent properties of each ply.  We enter similar information for the lower surface and finally for the webs. 

As the sample file shows, we begin with the upper surface.  We enter the number of sectors for this surface; this is three in our sample file.  Next, to locate placement of sectors on the surface, we enter x-coordinate of the points that define the sector boundaries.  The x-coordinate is referenced to the airfoil coordinate frame (Figure 7).  Next, we enter data for each sector, starting with sector 1.  We first identify the number of laminas in this sector.  In our example file, we identify three laminas for the first sector.  Then we enter data for each lamina at the plies level.  The lamina numbering proceeds from the exterior surface to the blade interior, with the first lamina at the exterior surface.  In our sample file, we have three laminas for the first sector.  Therefore, we enter lamina-level data on three lines.  This is followed by similar information for the remaining sectors (sectors 2 and 3 in our example file). 

We repeat the data entry procedure for the lower surface and finally for each web.  Though you may find that the data entry is self-explanatory in the sample file, you may refer to [Table 5] for a detailed description of the input parameters. 

**Table 5.  Parameters for the Internal Structure Input File.** 


| **Parameter**        | **Description**                                                                                                                                                                                                                                                                                                                                                     | **Unit** |
|----------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *N_scts*             | Subscripted integer variable. *N_scts(1)* and *N_scts(2)* are the number of sectors on the upper and lower surfaces, respectively.                                                                                                                                                                                                                                  | -        |
| *Xsec_node*          | Real array input that identifies locations of sectors on the upper and lower surfaces. The array contains a sequence of values that specify x-coordinates of the sector boundaries. The x-coordinate is normalized with respect to the chord length and is referenced to the airfoil coordinate frame (Figure 7). In our sample file, we have three sectors on the upper surface, which imply four points that define sector boundaries. The x-coordinates of these four points are 0, 0.15, 0.50, and 1.0. Sector 1 is bounded by x-coordinates *0* and *0.15*, sector 2 is bounded by x-coordinates *0.15* and *0.5*, and sector 3 is bounded between x-coordinates *0.50* and *1.0*. Note: The x-coordinates of sector boundaries must be positive and in ascending order. The first coordinate may be greater than 0 and the last less than 1. In this case, however, HawtC2.PCS checks to determine whether there is a gap (no laminate) at the leading and trailing edges of the section and, if necessary, issues a warning to place webs at those locations. | -        |
| *Sect_num*           | Sector number.                                                                                                                                                                                                                                                                                                                                                       | -        |
| *N_laminas*          | Number of laminas for a particular sector identified in the file (must be a positive integer).                                                                                                                                                                                                                                                                      | -        |
| *Lam_num*            | The lamina number. The first lamina is always at the exterior surface and the numbering proceeds from the exterior surface to the interior of the blade.                                                                                                                                                                                                              | -        |
| *N_plies*            | Number of plies in an identified lamina (must be a positive integer).                                                                                                                                                                                                                                                                                               | -        |
| *Tply*               | The thickness of each ply in an identified lamina.                                                                                                                                                                                                                                                                                                                  | m        |
| *Tht_lam*            | Ply angle representing orientation of the principal material (fiber) direction of each ply of a lamina. Figure 1 shows how a positive ply angle is defined. S is a point on the blade surface at which we wish to determine the ply angle. The *r-t-s* is a right-hand coordinate system with *r* axis parallel to the blade axis and pointing outboard. The *t* axis is normal to *r* and tangent to the blade surface. The *n* axis is normal to the blade surface at point S. Line SL is the principal (longitudinal) material direction and α, the angle between SL and *r* axis, represents the ply angle. A rotation α of the *r-t-s* axes system about the *n* axis thus aligns the *r* axis with principal material direction SL. A positive rotation about the *n* axis implies a positive ply angle. | deg      |
| *Mat_id*             | Material identifier for each ply in a lamina. HawtC2.PCS uses this identifier to read ply properties from a materials table.                                                                                                                                                                                                                                           | -        |
| *N_weblams*          | Number of laminas in an identified web (must be a positive integer). Note that unlike the blade surfaces, which may have multiple laminates (sectors or stacks of laminas), a web is assumed to have only a single laminate. Most blade webs are built this way.                                                                                                   | -        |
| *W_tply*             | The thickness of each ply in an identified web. If this section lies within the blade sections *ib_sp_loc* and *ob_sp_loc* specified earlier, but does not have the identified web, set *W_tply* value to zero.                                                                                                                                                      | m        |
| *Tht_Wlam*           | The ply angle representing orientation of principal material direction of each ply in an identified web. The definition of ply angle for a web lamina follows that of *Tht_lam* defined earlier. In this case, however, the *n* axis, normal to the web surface, always points to the leading edge.                                                                 | deg      |
| *Wmat_Id*            | Material identifier for each ply in a web. HawtC2.PCS uses this identifier to read ply properties from a materials table, discussed in Section [4.4].                                                                                                                                                                                                                   | -        |



### Materials Data File

This file contains material properties of plies typically available commercially.  The file comes with HawtC2.PCS and you may add new-material properties to this file.  This file, shown in Figure 9, is in tabular form and has a fixed name, *materials.inp*.  Its first line is a header that lists material property names.  The second comment line identifies units for each material property, if applicable.  The material properties are specified in a columnar format as seen in the figure.  [Table 6] describes the parameters for this file. 

Note: [Table 6] defines ν*12*, which is one of the Poisson’s ratios.  The other, ν*21*, is related to ν*12* as follows: 

![](V12.png)



**Table 6.  Parameters for the Material File** 


| **Parameter**      | **Description**                                                                                                                                                                                                 | **Unit** |
|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------|
| *Mat_Id*            | Material identifier; an integer.                                                                                                                                                                              | -        |
| *E1*                | Young’s modulus in the principal direction and assumed to be the same in tension and compression (in Pascal, ie.).                                                                                           | N/m²     |
| *E2*                | Young’s modulus in the lateral direction (normal to the principal direction) and is assumed to be the same in tension and compression (in Pascal).                                                            | Pa       |
| *G12*               | The shear modulus with respect to the principal and lateral directions (in Pascal).                                                                                                                           | Pa       |
| *Nu12*              | Poisson’s ratio, ν*12*, defined as the contraction strain in direction 2 (lateral direction) caused by unit extensional strain in direction 1 (principal direction).                                         | -        |
| *Density*           | This is the material density in kg/m³.                                                                                                                                                                       | kg/m³    |
| ***Mat_Name***      | This is the material name associated with the material identifier. However, it is not read or required by HawtC2.PCS.                                                                                           | -        |


## Error Messages and Warnings

HawtC2.PCS performs extensive checks to ensure that the user-supplied data are: 

1. ***Within range***.  An example of out-of-range data would be an airfoil node whose chord- normalized *x*-coordinate is less than *0* or more than *1*.  Two other examples, illustrated in Figure 8, are an airfoil periphery that crosses itself (enclosing what mathematicians call a multiconnected region), and a periphery that turns sharply (more than 90°) either on the upper or lower blade surface.  Both geometries may be physically realizable, but these will be outside the range of permissible geometries that HawtC2.PCS can handle. 
1. ***Consistent***.  An example of inconsistent data would be specification of material properties, *E1*, *E2*, and ν*12*, (see Section 4.4) that would violate the following relation [8]: 

   ![](inconsistency.png)

   Each property, *E1*, *E2*, or ν*12*, may be physically viable independently, but would not be consistent unless it satisfies the above relation. 

3. ***Realizable***.  Examples of unrealizable data are negative chord length and a specification of sector nodes that is not monotonically ascending (see Section 4.3).  Other examples, illustrated in Figure 10, are webs that lie outside the blade geometry. 

   Detected errors are displayed on the screen and the program execution aborts.  The displayed messages are self-explanatory and HawtC2.PCS can detect more errors than we can discuss in this section. 

   Besides error messages, warnings also may be issued on the screen.  For example, the following warning  

   *WARNING\*\* leading edge aft of reference axis \*\** 

   is issued if, at any section, the leading edge is aft of the blade pitch axis.  Such a blade is not impossible to construct, but it would most likely be a result of wrong data specification.  The execution proceeds. 

## Executing HawtC2.PCS

First follow the guidelines in Section 4 to create the main and auxiliary input files.  To do this, use the sample input files supplied with the HawtC2.PCS executable and modify it to suit your composite blade.  Identify all auxiliary files in the main input file. 

Next, open a command window, change over to the directory you wish to work in, and issue the following command: 

*HawtC2.PCS  [input path name]\main\_input\_file\_name* 

where *input path name* is the path name for the directory in which the HawtC2.PCS main input file resides, and *main\_input\_file\_name* is the name of the main input file.  Follow the instructions at <http://wind.nrel.gov/designcodes/setup.pdf>, and that will eliminate the need to specify the executable path name.  For example, if the main input files reside in the working directory, and *HawtC2.PCS.pci* is the name of the main input file, then the command would be: 

*HawtC2.PCS  HawtC2.PCS.pci* 

If HawtC2.PCS detects any errors, either during input reading or during computations, errors or warning messages will be issued on the screen.  If no errors are detected, HawtC2.PCS embeds extra nodes in the blade periphery (Figure 11), in addition to the airfoil nodes, which already exist.  The extra nodes represent intersection of webs with the airfoil and the end location of sectors over the airfoil geometry.  The sectors, as explained in section 4.3, represent distinct stack of laminas.  Each airfoil segment, spanned by any two consecutive nodes, is idealized as a flat laminate for computing structural properties.  The computed properties are output to either a single file or two files depending on how the integer switch *out\_format* is set in the main input file.  If *out\_format* is 1 signifying a general-format output, a single output file is generated whose name is the same as that of the main input file except that the extension is changed to *out\_gen*.  If *out\_format* is 2 signifying a BModes-compatible output, a single output file is again generated with extension *out\_bmd*.  If *out\_format* is 3, two output files are generated, one with extension *out\_gen* and the other with *out\_bmd*.  Each output file is generated in the same directory in which the main input file resides.  The next section describes the output files. 

## Output Description

HawtC2.PCS outputs blade section properties in tabular form, which may be readily exported to a plotting utility such as an Excel Spreadsheet, to a beam analysis code such as BModes [9], or to an aeroelastic code such as FAST.  In fact, the main motivation for HawtC2.PCS development is to eventually integrate it with FAST to handle composite rotor blades. 

As mentioned in the previous section, an output file is either general type or BModes-compatible type.  Each type is best viewed using a simple text editor.  We first describe the general-type output file, whose name always ends with the extension *out\_gen*.  This type shows all the computed properties.  Figure 12 is a sample of such an output file.  The second line shows the HawtC2.PCS version that generates the output along with the date and time at which it creates the output file. The third line repeats (echoes) the *title* specified in the first line of the main input file.  A blank line follows.  The third line shows the blade length in meters.  The sixth line shows the blade length in meters. 

The computed section properties are output in 23 columns and *N\_sections* rows, where *N\_sections* is the number of blade sections specified in the main input file.  Each column is associated with a particular section property and each row with a particular section.  There are two rows of column headers.  The first row lists parameters that identify section properties and the second row identifies the associated units. 

Before we describe the output properties, we need to understand the axes systems shown in Figure 13.  These axes conform to the IEC specifications [10] and all output properties are referred to these axes.  The *XR-YR* are the section reference axes with origin at *R*, where point *R* is the intersection of the section with the blade reference axis, usually selected to be the blade pitch axis.  The *YR* axis coincides with the section chord points toward the trailing edge.  The *XR* axis is normal to *YR* and points toward the upper (suction) side of the blade.  This reference frame is different from the *Xaf-Yaf* frame (Figure 7) that is used to define the blade airfoil geometry as described in Section 4.2.  The θ*aero* is the aerodynamic twist that defines the orientation of the chord with respect to a blade-twist reference plane *BB*, usually the hub plane (plane normal to the shaft at the hub location).  Point E is the section shear center.  The *XE-YE* frame is parallel to *XR- YR* with origin at *E*.  All the section elastic properties are referred to this frame.  Point *T* is the tension center of the section, and point *G* is the section center of mass.  The frame *XG-YG* originates at the center of mass with *YG* axis oriented at an angle θ*I* with respect to the reference plane BB.  The θ*I* is the orientation of the principal inertia axes with respect to hub plane.  All the section inertia properties are referred to this frame. 

[Table 7] describes parameters that appear in a general-type output file.  These parameters represent all properties computed by HawtC2.PCS and refer to axes systems shown in Figure 13.  BModes-compatible output file shows only select section properties, which are compatible with the BModes code and refer to axes system explained the BModes User’s Guide [9]. As noted in the Section 1, the computation of shear center is approximate. Its definition for composite materials is currently controversial. After this controversy is settled in the composites field, we will upgrade HawtC2.PCS. 

**Table 7.  Ouput Parameters** 


| **Parameter**        | **Description**                                                                                                                            | **Unit** |
|----------------------|--------------------------------------------------------------------------------------------------------------------------------------------|----------|
| ***Span_loc***       | Span location of the section measured from the blade root and normalized with respect to the blade length.                                | -        |
| ***Chord***          | Chord length of the section.                                                                                                              | m        |
| ***Tw_aero***        | Section aerodynamic twist, θ*aero*.                                                                                                       | deg      |
| ***EI_flap***        | Section flap bending stiffness about the *YE* axis.                                                                                       | Nm²      |
| ***EI_lag***         | Section lag (edgewise) bending stiffness about the *XE* axis.                                                                             | Nm²      |
| ***GJ***             | Section torsion stiffness.                                                                                                                 | Nm²      |
| ***EA***             | Section axial stiffness.                                                                                                                    | N        |
| ***S_f***            | Coupled flap-lag stiffness with respect to the *XE-YE* frame.                                                                             | Nm²      |
| ***S_airfoil***      | Coupled axial-flap stiffness with respect to the *XE-YE* frame.                                                                          | Nm       |
| ***S_al***           | Coupled axial-lag stiffness with respect to the *XE-YE* frame.                                                                           | Nm       |
| ***S_ft***           | Coupled flap-torsion stiffness with respect to the *XE-YE* frame.                                                                        | Nm²      |
| ***S_lt***           | Coupled lag-torsion stiffness with respect to the *XE-YE* frame.                                                                         | Nm²      |
| ***S_at***           | Coupled axial-torsion stiffness.                                                                                                          | Nm       |
| ***X_sc***           | X-coordinate of the shear-center offset with respect to the *XR-YR* axes.                                                                | m        |
| ***Y_sc***           | Chordwise offset of the section shear-center with respect to the reference frame, *XR-YR*.                                               | m        |
| ***X_tc***           | X-coordinate of the tension-center offset with respect to the *XR-YR* axes.                                                               | m        |
| ***Y_tc***           | Chordwise offset of the section tension-center with respect to the *XR-YR* axes.                                                         | m        |
| ***Mass***           | Section mass per unit length.                                                                                                              | kg/m     |
| ***Flap_iner***      | Section flap inertia about the *YG* axis per unit length.                                                                                 | kg·m     |
| ***Lag_iner***       | Section lag inertia about the *XG* axis per unit length.                                                                                  | kg·m     |
| ***Tw_iner***        | Orientation of the section principal inertia axes with respect to the blade reference plane, θ*I*.                                        | deg      |
| ***X_cm***           | X-coordinate of the center-of-mass offset with respect to the *XR-YR* axes.                                                              | m        |
| ***Y_cm***           | Chordwise offset of the section center of mass with respect to the *XR-YR* axes.                                                         | m        |

**Future Plans**

We have provided step-by-step instructions for preparing input files:  specifying blade external geometry and internal structural layup of composite laminates, executing the code, and interpreting the output properties.  Specifying blade geometry and complex internal materials layup is a challenging task that we have tried to simplify as much as possible.  If you encounter problems or have suggestions to improve the user interface, contact us.  Also, let us know if you would like to see additional capabilities in the code within the constraints of the laminate theory. 

As requested by several wind industry members, we will extend HawtC2.PCS next year to compute load-induced strains.  The objective is to help designers accelerate the preliminary design phase.  The flexural strain computations, however, will ignore the secondary warping effects. 

We have already verified HawtC2.PCS for metallic blades with elliptical and rectangular sections for which analytical results may be readily obtained [11].  However, we still have to verify the code for composite blades, particularly those with anisotropic layup of composites.  We plan to do so when experimental data become available for such blades. 

As discussed in the Section 7, the shear center is computed only approximately. Its definition is currently controversial in the composites field. After this controversy is settled, probably in consultation with material experts, we will upgrade HawtC2.PCS. 

**Acknowledgments** 

Thanks are due to Sandy Butterfield for motivating the development of HawtC2.PCS and providing sustained encouragement.  Thanks go  to Marshall Buhl for his excellent computer support.  Finally, the author would like to thank Mike Robinson, NREL, for his constant support.  DOE supported this work under contract number DE-AC36-83CH10093. 

**References** 

1. Jonkman,  J.M.;  Buhl  Jr.,  M.L.  (2005).  *FAST  User's  Guide*,  NREL/EL-500-29798. Golden, Colorado: National Renewable Energy Laboratory. 
1. Elliott, A.S.; McConville, J.B. (1989).  “Application of a General-Purpose Mechanical Systems Analysis Code to Rotorcraft Dynamics Problems.”  Prepared for the American Helicopter Society National Specialists’ Meeting on Rotorcraft Dynamics, 1989. 
1. Elliott,  A.S.   (1989).   “Analyzing  Rotor  Dynamics  with  a  General-Purpose  Code,” *Mechanical Engineering* 112, no. 12 (December 1990): pp. 21-25. 
1. Malcolm, D.J., Laird D.L.  (2005).  “Identification and Use of Blade Physical Properties.”  Proceedings of the ASME/AIAA Wind Energy Symposium, Reno, Nevada, January. 
1. Bir,  G.S.   (2001.   “A  Computerized  Method  for  Preliminary  Structural  Design  of Composite Wind Turbine Blades.” Special Issue of the Journal of Solar Engineering, Volume 123, Number 4, November.  Also presented at the AIAA/ASME Wind Energy Symposium, Reno, January. 
1. Bir, G. S., Migliore P.  (2004).  “Preliminary Structural Design of Composite Blades for Two- and Three-Blade Rotors.” NREL Report NREL/TP-500-31486, September. 
1. Laird, D.  (2001).  “A Numerical Manufacturing and Design Tool Odyssey.” Proceedings of the ASME/AIAA Wind Energy Symposium, Reno, Nevada, January. 
1. Jones,  R.M.   (1975).   *Mechanics  of  Composite  Materials*,  Hemisphere  Publishing Corporation. 
1. Bir, G.S.  (2005).  User’s Guide to BModes; Software for Computing Rotating Beam Coupled Modes, NREL TP-500-38976, Golden, Colorado: National Renewable Energy Laboratory. 
1. Draft IEC 61400-13 TS, Ed. 1: Wind turbine generator systems – Part 13: Measurements of mechanical loads, Jan 21, 2000. 
1. Cook, R.D., Young, W.C. (1998).  *Advanced Mechanics of Materials*, Prentice Hall, 2nd edition. 


![](Fig1.png)
![](Fig2.png)
![](Fig3.png)
![](Fig4.png)
![](Fig5.png)
![](Fig6.png)
![](Fig7.png)
![](Fig8.png)
![](Fig9.png)
![](Fig10.png)
