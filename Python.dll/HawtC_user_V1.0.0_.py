import ctypes
from ctypes import *
from ctypes import CDLL
# # 加载MBD
# MBD = ctypes.cdll(r"G:\2026\HawtC2\build\HawtC2_User_C.dll")

# # 设置函数参数类型和返回类型
# MBD.INIMain.argtypes = [ctypes.c_char_p]  # 假设传入的是ANSI字符串
# MBD.INIMain.restype = ctypes.c_int

# # 创建字符串指针
# config_path = "G:\\2026\OpenhastV7\demo\openhast_x64\IEA_15MW_onshore\\HawtC_15MW_PowerProduction_land_trb.hst\0"  # ANSI字符串，以null结尾
# config_path_bytes = config_path.encode('utf-8')

# # 将字符串转换为字符指针
# config_path_ptr = ctypes.c_char_p(config_path_bytes)

# # 调用函数
# result = MBD.INIMain(config_path_ptr)

# print(f"Function returned: {result}")


class HawtC:

    """
    HawtC类，用于调用HawtC.dll中的INIMain函数
    """
    def __init__(self, dll_path):
        print(dll_path)
        self.dll=CDLL(dll_path)
    class HawtCModel:
        """
        HawtC模型类，用于调用HawtC.dll模型初始化当中的函数
        """
        def __init__(self,MBD):
            self.dll=MBD
            
        def INIMain(self,path):
            """
            用于调用HawtC.dll中的INIMain函数,读取运行文件,并完成初始化和运行工作.该模式下无法运行MoptL模式
            """
            config_path=path
            config_path_bytes = config_path.encode('utf-8')
            # 将字符串转换为字符指针
            config_path_ptr = ctypes.c_char_p(config_path_bytes)
            # 调用函数
            result = self.dll.INIMain(config_path_ptr)
            return result
        
        def Close(self,turbinenum):
            """
            用于调用HawtC.dll中的Close函数,释放内存
            """     
            turbinenum =ctypes.c_int(turbinenum)   
            self.dll.Close(turbinenum)
    
    class MBD:
        """
        MBD类，用于调用HawtC.dll中的MBD函数
        """
        def __init__(self,MBD):
            self.dll=MBD
            
        def MBD_GetTimesN(self,turbinenum ):
            self.dll.MBD_GetTimesN.restype = ctypes.c_int
            turbinenum =ctypes.c_int(turbinenum)   
            return self.dll.MBD_GetTimesN(turbinenum)
        
        def MBD_GetTimesSpan(self,num):
            num =ctypes.c_int(num)
            # self.dll.MBD_GetTimesSpan.argtypes = [c_int]
            # 设置函数的返回类型为指针类型
            self.dll.MBD_GetTimesSpan.restype = ctypes.POINTER(ctypes.c_double)  # 假设返回类型是 double 指针
            
            ptr=self.dll.MBD_GetTimesSpan(num)
            N=self.dll.MBD_GetTimesN(num)
            array_type = c_double * N
            array_from_pointer = array_type.from_address(ctypes.cast(ptr, ctypes.c_void_p).value)
            # 转换为 Python 列表
            result_array = list(array_from_pointer)
            return result_array

        def MBD_Update_Step(self,turbinenum,times_n,dt):
            """
            用于调用HawtC.dll中的Update函数,进行时间步进
            """
            turbinenum =ctypes.c_int(turbinenum)
            times_n =ctypes.c_int(times_n)
            dt =ctypes.c_double(dt)
            
            
            self.dll.MBD_Update_Step(turbinenum,times_n,dt)
            
        def MBD_Solve(self,turbinenum):
            """
            用于调用HawtC.dll中的求解函数,不能自定义时间步进
            """        
            turbinenum =ctypes.c_int(turbinenum)

            self.dll.MBD_Solve(turbinenum)
            
        def MBD_GetTowerBaseFx(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseFx函数,获取塔基X方向的力
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseFx.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseFx(num)
        
        def MBD_GetTowerBaseFy(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseFy函数,获取塔基Y方向的力
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseFy.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseFy(num)
        
        def MBD_GetTowerBaseFz(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseFz函数,获取塔基Z方向的力
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseFz.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseFz(num)
        
        def MBD_GetTowerBaseMx(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseMx函数,获取塔基X方向的力矩
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseMx.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseMx(num)
        
        def MBD_GetTowerBaseMy(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseMy函数,获取塔基Y方向的力矩
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseMy.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseMy(num)
        
        def MBD_GetTowerBaseMz(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetTowerBaseMz函数,获取塔基Z方向的力矩
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetTowerBaseMz.restype = ctypes.c_double
            return  self.dll.MBD_GetTowerBaseMz(num)
        
        def MBD_GetLocalTowerMxLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerMxLoads函数,获取塔架各节点X方向的力矩
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerMxLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerMxLoads(num,J)
        
        
        def MBD_GetLocalTowerMyLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerMyLoads函数,获取塔架各节点Y方向的力矩
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerMyLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerMyLoads(num,J)
        
        def MBD_GetLocalTowerMzLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerMzLoads函数,获取塔架各节点Z方向的力矩
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerMzLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerMzLoads(num,J)
        
        def MBD_GetLocalTowerFxLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerFxLoads函数,获取塔架各节点X方向的力
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerFxLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerFxLoads(num,J)
        
        def MBD_GetLocalTowerFyLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerFyLoads函数,获取塔架各节点Y方向的力
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerFyLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerFyLoads(num,J)
        
        def MBD_GetLocalTowerFzLoads(self,turbnum,J):
            """
            用于调用HawtC.dll中的MBD_GetLocalTowerFzLoads函数,获取塔架各节点Z方向的力
            """
            J =ctypes.c_int(J)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetLocalTowerFzLoads.restype = ctypes.c_double
            return  self.dll.MBD_GetLocalTowerFzLoads(num,J)
        
        
        def MBD_GetBladeRootFx(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootFx函数,获取叶片根X方向的力
            """
            blade =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootFx.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootFx(num,blade)
        
        def MBD_GetBladeRootFy(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootFy函数,获取叶片根Y方向的力
            """
            blade =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootFy.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootFy(num,blade)
        
        def MBD_GetBladeRootFz(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootFz函数,获取叶片根Z方向的力
            """
            blade =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootFz.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootFz(num,blade)
        
        def MBD_GetBladeRootMx(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootMx函数,获取叶片根X方向的力矩
            """
            blade1 =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootMx.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootMx(num,blade1)
        
        def MBD_GetBladeRootMy(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootMy函数,获取叶片根Y方向的力矩
            """
            blade1 =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootMy.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootMy(num,blade1)
        
        def MBD_GetBladeRootMz(self,turbnum,blade):
            """
            用于调用HawtC.dll中的MBD_GetBladeRootMz函数,获取叶片根Z方向的力矩
            """
            blade1 =ctypes.c_int(blade)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeRootMz.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeRootMz(num,blade1)
        
        def MBD_GetBladeLocalMx(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalMx函数,获取叶片各节根X方向的力矩
            """
            blade1 =ctypes.c_int(blade)
            section1 =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalMx.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalMx(num,blade1,section1)
        
        def MBD_GetBladeLocalMy(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalMy函数,获取叶片各节根Y方向的力矩
            """
            blade1 =ctypes.c_int(blade)
            section1 =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalMy.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalMy(num,blade1,section1)
        
        def MBD_GetBladeLocalMz(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalMz函数,获取叶片各节根Z方向的力矩
            """
            blade =ctypes.c_int(blade)
            section =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalMz.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalMz(num,blade,section)
        
        def MBD_GetBladeLocalFx(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalFx函数,获取叶片各节根X方向的力
            """
            blade =ctypes.c_int(blade)
            section =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalFx.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalFx(num,blade,section)

        def MBD_GetBladeLocalFy(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalFy函数,获取叶片各节根Y方向的力
            """
            blade =ctypes.c_int(blade)
            section =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalFy.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalFy(num,blade,section)
        
        def MBD_GetBladeLocalFz(self,turbnum,blade,section):
            """
            用于调用HawtC.dll中的MBD_GetBladeLocalFz函数,获取叶片各节根Z方向的力
            """
            blade =ctypes.c_int(blade)
            section =ctypes.c_int(section)
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetBladeLocalFz.restype = ctypes.c_double
            return  self.dll.MBD_GetBladeLocalFz(num,blade,section)
        
        def MBD_GetPlatformSurge(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSurge函数,获取平台 surge方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSurge.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSurge(num)
        
        def MBD_GetPlatformSway(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSway函数,获取平台 sway方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSway.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSway(num)
        
        def MBD_GetPlatformHeave(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformHeave函数,获取平台 heave方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformHeave.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformHeave(num)
        
        def MBD_GetPlatformRoll(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformRoll函数,获取平台 roll方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformRoll.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformRoll(num)
        
        def MBD_GetPlatformPitch(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformPitch函数,获取平台 pitch方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformPitch.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformPitch(num)
        
        def MBD_GetPlatformYaw(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformYaw函数,获取平台 yaw方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformYaw.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformYaw(num)
        
        def MBD_GetPlatformSurgeVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSurge函数,获取平台 surge方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSurgeVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSurgeVel(num)
        
        def MBD_GetPlatformSwayVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSway函数,获取平台 sway方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSwayVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSwayVel(num)
        
        def MBD_GetPlatformHeaveVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformHeave函数,获取平台 heave方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformHeaveVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformHeaveVel(num)
        
        def MBD_GetPlatformRollVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformRoll函数,获取平台 roll方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformRollVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformRollVel(num)
        
        def MBD_GetPlatformPitchVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformPitch函数,获取平台 pitch方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformPitchVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformPitchVel(num)
        
        def MBD_GetPlatformYawVel(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformYaw函数,获取平台 yaw方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformYawVel.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformYawVel(num)
    
    
        def MBD_GetPlatformSurgeAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSurge函数,获取平台 surge方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSurgeAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSurgeAcc(num)
        
        def MBD_GetPlatformSwayAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformSway函数,获取平台 sway方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformSwayAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformSwayAcc(num)
        
        def MBD_GetPlatformHeaveAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformHeave函数,获取平台 heave方向的位移
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformHeaveAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformHeaveAcc(num)
        
        def MBD_GetPlatformRollAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformRoll函数,获取平台 roll方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformRollAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformRollAcc(num)
        
        def MBD_GetPlatformPitchAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformPitch函数,获取平台 pitch方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformPitchAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformPitchAcc(num)
        
        def MBD_GetPlatformYawAcc(self,turbnum):
            """
            用于调用HawtC.dll中的MBD_GetPlatformYaw函数,获取平台 yaw方向的旋转
            """
            num =ctypes.c_int(turbnum)
            self.dll.MBD_GetPlatformYawAcc.restype = ctypes.c_double
            return  self.dll.MBD_GetPlatformYawAcc(num)
       
    class AeroL:
        """
        用于调用HawtC.dll中的AeroL函数
        """ 
        def __init__(self,MBD):
            self.dll=MBD
          
    class Test:
        """
        用于测试HawtC.dll中的测试函数
        """
        def __init__(self,MBD):
            self.dll=MBD
            
        def GetAddc(self,a,b):
            """
            用于调用HawtC.dll中的GetAddc函数,获取平台 yaw方向的旋转
            """
            get_addc=self.dll.GetAddc
            self.dll.GetAddc.restype = ctypes.c_void_p
            AddcType = ctypes.CFUNCTYPE(ctypes.c_double, ctypes.c_double, ctypes.c_double)
            addc_from_MBD = ctypes.cast(self.dll.GetAddc(), AddcType)
            addc_from_MBD.restype = ctypes.c_double
            a=ctypes.c_double(a)
            b=ctypes.c_double(b)
            return addc_from_MBD(a,b)
        
        def  SetAddc(self,add_func):
            # 创建 Python 委托
            AddcType = ctypes.CFUNCTYPE(ctypes.c_double, ctypes.c_double, ctypes.c_double)
            addc_delegate = AddcType(add_func)
            self.dll.SetAddc.argtypes = [ctypes.c_void_p]  # 设置参数类型
        # 将 Python 委托传递给 C# MBD
            self.dll.SetAddc(ctypes.cast(addc_delegate, ctypes.c_void_p))
    
    
    
    
    
    
    
    

    
    


    
Hawtc = HawtC(r'G:\\2026\\HawtC2\\build\\HawtC2_User_C.dll')
HaetCModel=Hawtc.HawtCModel(Hawtc.dll)
MBD=Hawtc.MBD(Hawtc.dll)
Test=Hawtc.Test(Hawtc.dll)


p1=HaetCModel.INIMain(r"G:\2026\HawtC2\demo\15MW\HawtC2_15MW_PowerProduction_land.hst")

result = Test.GetAddc(2,3)
print(f"Result: {result}")   



# 定义 Addc 委托的 Python 函数
def add_func(a, b):
    print(f"这是Python 加法: {a} + {b}")
    return a + b


Test.SetAddc(add_func)

# #调用 C# 中的 Addc 委托
# result = addc_delegate(3.0, 4.0)
# print(f" {result}")
# result = Test.GetAddc(2,6)
# print(f" {result}")


# result = MBD.GetAddc(3.0, 4.0)
print(MBD.MBD_Update_Step)
#MBD.MBD_Solve(0)
for i in range(10):
    print(i)
    MBD.MBD_Update_Step(0,i,0.05)
    # print(MBD.MBD_GetTowerBaseFz(0)-MBD.MBD_GetLocalTowerFzLoads(0,0))
    #print(MBD.MBD_GetTowerBaseMz(0)-MBD.MBD_GetLocalTowerMzLoads(0,0))
    # print(MBD.MBD_GetBladeRootFz(0,0))
    # print(MBD.MBD_GetTowerBaseFx(0))
    # print(MBD.MBD_GetPlatformHeave(0))
    # print(MBD.MBD_GetPlatformHeaveVel(0))
    # print(MBD.MBD_GetPlatformHeaveAcc(0))
    # print(MBD.MBD_GetTimesSpan(0))   
    # MBD.MBD_Solve(0)
    # MBD.MBD_Update_step(0,0,0.03)
    # print(MBD.MBD_GetTowerBaseFx(0))
    # print(MBD.MBD_GetLocalTowerFxLoads(0,0))
    # print(MBD.MBD_GetTowerBaseFy(0))
    # print(MBD.MBD_GetLocalTowerFyLoads(0,0))
    # print(MBD.MBD_GetTowerBaseFz(0))
    # print(MBD.MBD_GetLocalTowerFzLoads(0,0))
    # print(MBD.MBD_GetTowerBaseMx(0))
    # print(MBD.MBD_GetLocalTowerMxLoads(0,0))
    # print(MBD.MBD_GetTowerBaseMy(0))
    # print(MBD.MBD_GetLocalTowerMyLoads(0,0))
    # print(MBD.MBD_GetTowerBaseMz(0))
    # print(MBD.MBD_GetLocalTowerMzLoads(0,0))

