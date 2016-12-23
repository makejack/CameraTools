using System;
using System.Runtime.InteropServices;

namespace CameraTools
{
    public class AnShiBaoClientSdk
    {


        [StructLayout(LayoutKind.Sequential)]
        public struct CAMERA_IP_TAG
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string ip;
            public ushort port;
            public ushort reserved;
        }

        /* 感兴趣区域定义 */
        public struct ROI_RECT_TAG
        {
            public int left;             /*左 offset */
            public int top;              /*上 offset */
            public int right;            /*右 offset */
            public int bottom;           /*下 offset */
        }

        /* 触发模式 */
        public enum TRIGGER_MODE
        {
            TRIGGER_MODE_VIDEO = 0,        // 视频移动侦测触发
            TRIGGER_MODE_IO = 1,           // 地感触发
            TRIGGER_MODE_MIX = 2,          // 混合模式
            TRIGGER_MODE_MAX = 3           // End of the enum
        };

        /// <summary>
        /// 初始化server端socket, 必须先调用， 否则其他API可能以为未初始化而失败
        /// </summary>
        /// <param name="port">监听的端口号， 相机默认向8190端口发送数据，如果相机不修改端口的话，这个值应该是8190否则按照相机发送端口来填值</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", EntryPoint = "IPCSDK_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Init(int port);

        /// <summary>
        /// 反初始化server端socket
        /// </summary>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_UnInit();

        /// <summary>
        /// 用户的回调函数
        /// </summary>
        /// <param name="ip">发送相机IP地址</param>
        /// <param name="buf">相机发送来的数据指针</param>
        /// <param name="len">相机发送来的数据长度</param>
        /// <returns>正常处理返回值应该为0, 否则会弹出错误提示框并提醒用户回调函数返回值</returns>
        public delegate int IPCSDK_CALLBACK(string ip, IntPtr buff, int len);

        /// <summary>
        /// 注册用户的处理回调函数
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Register_Callback(IPCSDK_CALLBACK callback);

        /// <summary>
        /// 搜索发现网络环境中的相机
        /// </summary>
        /// <param name="camera_num">相机个数</param>
        /// <param name="ip_list">相机IP列表， 需要分配至少128*sizeof(CAMERA_IP)空间</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", EntryPoint = "IPCSDK_Find_Camera", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Find_Camera(ref int camera_num, IntPtr ip_list);

        /// <summary>
        /// 在指定窗体上显示视频流
        /// </summary>
        /// <param name="hMainHwnd">主窗口句柄</param>
        /// <param name="hHwnd">视频显示控件的句柄</param>
        /// <param name="ip">相机IP</param>
        /// <param name="channel">主副码流选择, 0->主码流1920x1080; 1->子码流640x480</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Start_Stream(IntPtr hMainHwnd, IntPtr hHwnd, string ip, int channel);

        /// <summary>
        /// 停止相机视频流
        /// </summary>
        /// <param name="ip">相机IP</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Stop_Stream(string ip);


        /// <summary>
        /// 在视频上话矩形框
        /// </summary>
        /// <param name="ip">相机IP</param>
        /// <param name="left">矩形框左坐标</param>
        /// <param name="right">矩形框右坐标</param>
        /// <param name="top">矩形框上坐标</param>
        /// <param name="bottom">矩形框下坐标</param>
        /// <param name="clean_flag">是否清除之前的矩形框, 0->不清除， 1->清除</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Draw_Rect_On_Stream(string ip, int left, int right, int top, int bottom, int clean_flag);

        /// <summary>
        /// 获取相机端参数
        /// </summary>
        /// <param name="ip">相机IP， 例如"192.168.1.1"</param>
        /// <param name="para">使用者分配空间，将指针作为输入参数，函数成功返回后该指针指向的内存为接收到的相机参数</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Get_Camera_Para(string ip, IntPtr para);

        /// <summary>
        /// 设置相机端参数
        /// </summary>
        /// <param name="ip">相机IP， 例如"192.168.1.1"</param>
        /// <param name="para">参数指针, 来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Set_Camera_Para(string ip, IntPtr para);

        /// <summary>
        /// 设置车牌识别相机的感兴趣区域
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="index">ROI的索引， 最多支持ROI_NUM_MAX个ROI，0-3</param>
        /// <param name="left">车牌识别区域左</param>
        /// <param name="right">车牌识别区域右</param>
        /// <param name="top">车牌识别区域上</param>
        /// <param name="bottom">车牌识别区域下</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_ROI(IntPtr para, int index, int left, int right, int top, int bottom);

        /// <summary>
        /// 获取车牌识别相机的感兴趣区域
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="index">ROI的索引， 最多支持ROI_NUM_MAX个ROI， 0-3</param>
        /// <param name="left">车牌识别区域左</param>
        /// <param name="right">车牌识别区域右</param>
        /// <param name="top">车牌识别区域上</param>
        /// <param name="bottom">车牌识别区域下</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_ROI(IntPtr para, int index, ref int left, ref int right, ref int top, ref int bottom);

        /// <summary>
        /// 获取车牌识别相机RS485设置
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="baudrate">RS485波特率     例如9600</param>
        /// <param name="databit">RS485数据位     例如8</param>
        /// <param name="stopbit">RS485停止位     例如1</param>
        /// <param name="checkbit">RS485校验位     例如0->无校验位 1->奇校验 2->偶校验</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_RS485(IntPtr para, ref int baudrate, ref int databit, ref int stopbit, ref int checkbit);

        /// <summary>
        /// 设置车牌识别相机的RS485参数
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="baudrate"> RS485波特率     例如9600</param>
        /// <param name="databit"> RS485数据位     例如8</param>
        /// <param name="stopbit">RS485停止位     例如1</param>
        /// <param name="checkbit"> RS485校验位     例如0->无校验位 1->奇校验 2->偶校验</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_RS485(IntPtr para, int baudrate, int databit, int stopbit, int checkbit);

        /// <summary>
        /// 获取车牌识别相机RS485工作模式
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="mode">RS485工作模式   0->透明传输 : 1->输出车牌 : 2->LED显示屏输出</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_RS485_Mode(IntPtr para, ref int mode);

        /// <summary>
        /// 设置车牌识别相机RS485工作模式
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="mode"> RS485工作模式   0->透明传输 : 1->输出车牌 : 2->LED显示屏控制</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_RS485_Mode(IntPtr para, int mode);

        /// <summary>
        /// 从接收到的数据中提取车牌特写jpeg数据
        /// </summary>
        /// <param name="data_buf">对应于callback_func的第二个输入参数， buf</param>
        /// <param name="plate_buf">存放车牌特写jpeg的buf指针，外部分配空间，不能为NULL</param>
        /// <param name="plate_len">存放车牌特写jpeg长度的变量指针， 外部分配空间，不能为NULL</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Get_Plate_Jpeg(IntPtr data_buf, IntPtr plate_buf, ref int plate_len);

        /// <summary>
        /// 从接收到的数据中提取车牌信息， 包括车牌坐标，颜色， 置信度等信息
        /// </summary>
        /// <param name="data_buf">对应于callback_func的第二个输入参数， buf</param>
        /// <param name="plate_result">车牌信息输出指针，不能为空指针，需要预分配空间， 至少64KB</param>
        /// <param name="result_len">返回的车牌信息总长度</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Get_Plate_Info(IntPtr data_buf, IntPtr plate_result, ref int result_len);

        /// <summary>
        ///  从接收到的数据中提取车牌号， 此函数用于从接收到的数据中获取车牌号
        /// </summary>
        /// <param name="data_buf">对应于IPCSDK_Get_Plate_Info的第二个输入参数， plate_result</param>
        /// <param name="license">车牌号，不能为空指针，需要预分配至少16字节空间， 输出为字符串格式 例如：“苏A12345”</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Get_Plate_License(IntPtr data_buf, IntPtr license);

        /// <summary>
        /// 从接收到的数据中提取车牌颜色
        /// </summary>
        /// <param name="plate_result">对应于IPCSDK_Get_Plate_Info的第二个输入参数， plate_result</param>
        /// <param name="color">车牌颜色，不能为空指针，需要预分配至少8字节空间， 输出为字符串格式 例如：“蓝” </param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Get_Plate_Color(IntPtr plate_result, IntPtr color);

        /// <summary>
        /// 设置车牌识别相机车牌的触发模式
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="mode">触发模式，参考TRIGGER_MODE枚举</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Trigger_Mode(IntPtr para, uint mode);

        /// <summary>
        /// 获取车牌识别相机车牌的触发模式
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="mode">触发模式，使用者分配t空间，将指针作为输入参数，参考TRIGGER_MODE枚举</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Trigger_Mode(IntPtr para, ref int mode);

        /// <summary>
        /// 获取车牌识别相机是否使能了同一车牌只输出一次
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="enable"> 0->不使能    1->使能</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Enable_Output_Once_Only(IntPtr para, ref int enable);

        /// <summary>
        /// 设置车牌识别相机是否同一车牌只输出一次， 建议调试安装时关闭，正常使用时使能
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="enable">0->不使能 1->使能</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Enable_Output_Once_Only(IntPtr para, int enable);

        /// <summary>
        /// 获取车牌识别相机同一车牌只输出一次的时间间隔
        /// </summary>
        /// <param name="para"> 参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="time">相同车牌只输出一次的时间间隔，单位秒， 返回值为当前相机设置的时间间隔</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Output_Once_Only_Interval(IntPtr para, ref int time);

        /// <summary>
        /// 设置车牌识别相机同一车牌只输出一次的时间间隔， 防止同一辆车快速反复进出
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="time">相同车牌只输出一次的时间间隔，单位秒， 建议值10-30秒， 有效值为10-60</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Output_Once_Only_Interval(IntPtr para, int time);

        /// <summary>
        /// 发送RS485数据, 相机不做任何转换，透明传输到RS485接口
        /// </summary>
        /// <param name="ip">相机IP，例如"192.168.1.1"</param>
        /// <param name="buf">需要传输的数据buffer指针</param>
        /// <param name="len">需要传输的数据长度，最大长度支持256字节</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Send_RS485_Data(string ip, IntPtr buf, int len);

        /// <summary>
        /// 获取车牌识别相机上传服务器信息
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="ip">服务器IP     例如"192.168.1.100"</param>
        /// <param name="server_port">服务器端口   例如"8190"</param>
        /// <param name="server_user">服务器用户名 只有上传模式为FTP模式是才需要 例如"ftpuser"</param>
        /// <param name="server_passwd">服务器密码   只有上传模式为FTP模式是才需要 例如"ftppasswd"</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Upload_Server_Info(IntPtr para, IntPtr ip, ref int server_port, IntPtr server_user, IntPtr server_passwd);

        /// <summary>
        /// 设置车牌识别相机上传服务器信息
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="ip">服务器IP     例如"192.168.1.100"</param>
        /// <param name="server_port">服务器端口   例如"8190"</param>
        /// <param name="server_user">服务器用户名 只有上传模式为FTP模式是才需要 例如"ftpuser"， 最长32个字符</param>
        /// <param name="server_passwd">服务器密码   只有上传模式为FTP模式是才需要 例如"ftppasswd" 最长32个字符</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Upload_Server_Info(IntPtr para, IntPtr ip, int server_port, IntPtr server_user, IntPtr server_passwd);

        /// <summary>
        /// 获取车牌识别相机的感兴趣区域
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="min">曝光最短时间，单位微秒, 1000-4000</param>
        /// <param name="max">曝光最长时间，单位微秒, 5000-10000</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Expourse_Range(IntPtr para, ref int min, ref int max);

        /// <summary>
        /// 设置车牌识别相机的感兴趣区域
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="min">曝光最短时间，单位微秒, 1000-4000</param>
        /// <param name="max">曝光最长时间，单位微秒, 5000-10000</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Expourse_Range(IntPtr para, int min, int max);

        /// <summary>
        /// 设置相机时间
        /// </summary>
        /// <param name="ip">相机IP</param>
        /// <param name="year">设置相机年</param>
        /// <param name="month">设置相机月</param>
        /// <param name="day">设置相机日</param>
        /// <param name="week">设置相机周</param>
        /// <param name="hour">设置相机时</param>
        /// <param name="minute">设置相机分</param>
        /// <param name="secod">设置相机秒</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Set_Camera_Time(string ip, int year, int month, int day, int week, int hour, int minute, int secod);

        /// <summary>
        /// 通过网络手动抓拍一张并保存为文件
        /// </summary>
        /// <param name="ip">相机IP， 例如"192.168.1.1"</param>
        /// <param name="filename">文件名称， 例如"D:\\jpegs\xxxx.jpeg"</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Manual_Capture_Write_File(string ip, string filename);

        /// <summary>
        /// 设置车牌识别相机安装的所在省份，用于车牌模糊时的默认车牌汉字
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="province">省份字符串, 例如相机安装在江苏省，请填入汉字, "苏" ，其他省份参照列表</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Set_Default_Province(IntPtr para, IntPtr province);

        /// <summary>
        /// 获取车牌识别相机安装的默认省份
        /// </summary>
        /// <param name="para">参数buffer指针，来源于IPCSDK_Get_Camera_Para的第二个输入参数para</param>
        /// <param name="province">省份字符串, 使用者分配t空间，将指针作为输入参数</param>
        /// <returns></returns>
        [DllImport("ipcsdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int IPCSDK_Alg_Get_Default_Province(IntPtr para, IntPtr province);

    }
}
