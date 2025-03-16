import os
# 获取当前脚本所在的目录
current_path = os.getcwd()
print("当前路径：", current_path)
#设置路径和参数
airnum=30
Fastpath=r"G:\2026\OpenhastV7\demo\openhast_x64\IEA_10MW_Spar\10MW_Baseline\Airfoils/"
HastPath=r"G:\2026\OpenhastV7\demo\openhast_x64\IEA_10MW_Spar\dem\Aero\Airfoils/"
Numalf=50
alfdata=53




fastpath1=[]
hastpath1=[]
for i in range(airnum):
    fastpath1.append(Fastpath+"IEA-10.0-198-RWT_AeroDyn15_Polar_"+"%02d" % i+".dat")
    hastpath1.append(HastPath+"IEA-10.0-198-RWT_AF"+"%02d" % i+".dat")
print(fastpath1)
print(hastpath1)
def WriteHastTitle(file):
    file.write("! AeroL.Airfoil V1.0.2 Powered by 赵子祯 22MW file\n")
    file.write("这个文件是适配OpenWECD 1.0.2及其之上的版本，支持OYE,IAG，GOR,ATEF 以及BL模型，模型的开启与否在AeroL 主文件当中\n")
    file.write("===========  OYG modal  ============\n")
    file.write("8      T_f_OYG  - Time constant\n")
    file.write("===========  IAG modal  ============\n")
    file.write("0.3    A1   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.3]\n")
    file.write("0.7    A2   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]\n")
    file.write("0.7    b1   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.7]\n")
    file.write("0.53   b2   - Constant in the expression of phi_alpha^c and phi_q^c.  This value is relatively insensitive for thin airfoils, but may be different for turbine airfoils. [from experimental results, defaults to 0.53]\n")
    file.write("0.75   ka    -\n")
    file.write("1.7    T_p  - Boundary-layer,leading edge pressure gradient time constant in the expression of Dp. It should be tuned based on airfoil experimental data. [default = 1.7]\n")
    file.write("3      T_f  - Initial value of the time constant associated with Df in the expression of Df and f''. [default = 3]\n")
    file.write("6      T_V  - Initial value of the time constant associated with the vortex lift decay process; it is used in the expression of Cvn. It depends on Re,M, and airfoil class. [default = 6]\n")
    file.write("11     T_VL - Initial value of the time constant associated with the vortex advection process; it represents the non-dimensional time in semi-chords, needed for a vortex to travel from LE to trailing edge (TE); it is used in the expression of Cvn. It depends on Re, M (weakly), and airfoil. [valid range = 6 - 13, default = 6]\n")
    file.write("0.2    K_v\n")
    file.write("0.1    K_Cf\n")
    file.write("1.5    T_Um\n")
    file.write("1.5    T_Dm\n")
    file.write("0      M_IAG\n")
    file.write("===========  GOR modal  ============\n")
    file.write("6      A_M\n")
    file.write("===========  BL modal  =============\n")
    file.write("===========  ATEF modal  ===========\n")
    file.write("3      Tf_ATEF\n")
    file.write("1.7    Tp_ATEF\n")
    file.write("===========  Airfoil data  ===========\n")
    file.write(" 1    InterpOrd  - Interpolation order to use for quasi-steady table lookup {1=linear; 2=cubic spline; \"default\"} [default=1]\n")
for i in range(airnum):
    lens=0
    datatemp=[]
    file=open(fastpath1[i], 'r')
    lines =list( file.readlines())
    file.close
    line3 = lines[Numalf].strip()  # 获取第3行的字符串
    lens =int(line3.strip().split()[0])
    print(len(lines))
    for j in range(len(lines)):
        if(j>=alfdata):
            datatemp.append(lines[j].strip())
    if(len(datatemp)!=lens):
        print("error")
    file=open(hastpath1[i], 'w')
    WriteHastTitle(file)
    file.write(str(lens)+"  NumAlf     - Number of data lines in the following table"+"\n")
    file.write("      Alpha      Cl      Cd        Cm\n")
    file.write("     (deg)      (-)     (-)       (-)\n")
    for j in range(len(datatemp)):
        file.write(datatemp[j]+"\n")