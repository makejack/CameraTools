using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CameraTools
{
    /// <summary>
    /// 芊熠SDK开发包
    /// </summary>
    public class QianYiClientSdk
    {
        public const int MAX_HOST_LEN = 16;
        public const int ONE_DIRECTION_LANES = 5;
        public const int VERSION_NAME_LEN = 64;

        public static uint Reverse_uint(uint uiNum)
        {
            return ((uiNum & 0x000000FF) << 24) |
                   ((uiNum & 0x0000FF00) << 8) |
                   ((uiNum & 0x00FF0000) >> 8) |
                   ((uiNum & 0xFF000000) >> 24);
        }

        public static uint IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return uint.Parse(items[0]) << 24
                    | uint.Parse(items[1]) << 16
                    | uint.Parse(items[2]) << 8
                    | uint.Parse(items[3]);
        }

        public static string IntToIp(uint ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF);
            return sb.ToString();
        }

        /// <summary>
        /// SDK库初始化，初始化相机管理资源
        /// </summary>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_Init();

        /// <summary>
        /// SDK库反初始化，断开相机连接，释放相机管理资源
        /// </summary>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void Net_UNinit();

        /// <summary>
        /// 添加相机，分配相机管理项
        /// </summary>
        /// <param name="ptIp">相机ip，格式为”192.168.0.142”</param>
        /// <returns>相机句柄, -1为无效句柄, >=0有效</returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_AddCamera(string ptIp);

        /// <summary>
        /// 断开与相机之间的连接，释放相机管理项
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_DelCamera(int tHandle);

        /// <summary>
        /// 与相机建立控制信令
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="usPort">相机信令端口，为0时，使用默认端口30000</param>
        /// <param name="usTimeout">信令处理超时时长，单位为秒</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ConnCamera(int tHandle, ushort usPort, ushort usTimeout);

        /// <summary>
        ///  断开与相机之间的信令链路，释放图片接收资源
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_DisConnCamera(int tHandle);

        /// <summary>
        /// 开始接收视频码流
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="niStreamType">码流类型:0 -> 主流码 , 1 -> 辅流码</param>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StartVideo(int tHandle, int niStreamType, IntPtr hWnd);

        /// <summary>
        /// 停止接收视频码流
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StopVideo(int tHandle);

        /// <summary>
        /// 获取SDK版本信息
        /// </summary>
        /// <param name="szVersion">传入的SDK版本信息缓冲区</param>
        /// <param name="ptLen">缓冲区大小</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GetSdkVersion([MarshalAs(UnmanagedType.LPStr)]StringBuilder szVersion, ref int ptLen);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_ImageUserInfo
        {
            public ushort usWidth;   /*图片的宽度，单位:像素*/
            public ushort usHeight;  /*图片的高度，单位:像素*/
            public byte ucVehicleColor;/*车身颜色，E_ColorType*/
            public byte ucVehicleBrand;/*车标，E_VehicleFlag*/
            public byte ucVehicleSize;/*车型(大中小)，ITS_Tb_Vt*/
            public byte ucPlateColor;/*车牌颜色，E_ColorType*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] szLprResult;/*车牌，若为'\0'，表示无效GB2312编码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] usLpBox;  /*车牌位置，左上角(0, 1), 右下角(2,3)*/
            public byte ucLprType;/*车牌类型*/
            public uint uiSpeed;     /*单位km/h*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] acSnapTime;         /*图片抓拍时间:格式YYYYMMDDHHMMSSmmm(年月日时分秒毫秒)*/
            public byte ucViolateCode;    /*违法代码E_ViolationCode*/
            public byte ucLaneNo;          /*车道号,从0开始编码*/
            public uint uiVehicleId; 		/*检测到的车辆id，若为同一辆车，则id相同*/
            public byte ucScore;    		/*车牌识别可行度*/
            public byte ucDirection;       /*行车方向E_Direction*/
            public byte ucTotalNum;        /*该车辆抓拍总张数*/
            public byte ucSnapshotIndex;   /*当前抓拍第几张，从0开始编号*/
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_PicInfo
        {
            public uint uiPanoramaPicLen;  /*全景图片大小*/
            public uint uiVehiclePicLen;      /*车牌图片大小*/
            public IntPtr ptPanoramaPicBuff;   /*全景图片缓冲区*/
            public IntPtr ptVehiclePicBuff;  /*车牌图片缓冲区*/
        };

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCB(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecv(FGetImageCB fCb);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetImageCBEx(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegImageRecvEx(int tHandle, FGetImageCBEx fCb, IntPtr obj);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetOffLineImageCBEx(int tHandle, uint uiImageId, ref T_ImageUserInfo tImageInfo, ref T_PicInfo tPicInfo, IntPtr obj);

        /// <summary>
        /// 通过注册回调获取脱机图片及识别结果,无小车牌图片，回调函数为stcdcall调用；获取数据必须注册岗亭端Net_RegOffLineClient，必须开启脱机数据Net_StartGetOffLineData，Net_RegOffLineImageRecv的扩展函数
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="fCb">回调函数，用于传出脱机数据</param>
        /// <param name="obj">回调函数上下文</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLineImageRecvEx(int tHandle, FGetOffLineImageCBEx fCb, IntPtr obj);

        /// <summary>
        /// 将当前客户端注册为岗亭端，用于判断是否脱机
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLineClient(int tHandle);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_FrameInfo
        {
            public uint uiFrameId;          //帧ID
            public uint uiTimeStamp;        //RTP时间戳
            public uint uiEncSize;          //帧大小
            public uint uiFrameType;        //1:i帧 0:其它
        };

        /// <summary>
        /// 下发图片抓拍命令，需要使用Net_RegImageRecv注册的回调函数获取图片信息
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptImageSnap">图片抓拍参数，可全部置0</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ImageSnap(int tHandle, ref T_FrameInfo ptImageSnap);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_ControlGate
        {
            [MarshalAs(UnmanagedType.U1)]
            public byte ucState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] ucReserved;
        };

        /// <summary>
        /// 闸机控制
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptControlGate">闸机控制参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GateSetup(int tHandle, ref T_ControlGate ptControlGate);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_BlackWhiteList
        {
            public byte LprMode;  /* 0：黑名单；1：白名单*/
            public byte LprCode;      /* 0：车牌号码utf-8字符编码；1：车牌号码gb2312字符编码*/
            public byte Lprnew; /*0： 重新发送；1：续传；2:删除；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };

        /// <summary>
        /// 将黑白名单文件发送到相机，黑白名单文件内字符编码必须为GB2312编码；先调用Net_BlackWhiteListSetup将黑白名单信息按照指定格式写在本地，可循环调用该函数向文件内写车牌信息，最后再调用一次Net_BlackWhiteListSend将黑白名单发送到相机，但是一次发送的车牌信息不超过20000条
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptBalckWhiteList">黑白名单文件参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_BlackWhiteListSend(int tHandle, ref T_BlackWhiteList ptBalckWhiteList);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_GetBlackWhiteList
        {
            public byte LprMode;  /* 0：黑名单；1：白名单*/
            public byte LprCode;      /* 0：车牌号码utf-8字符编码；1：车牌号码gb2312字符编码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };

        /// <summary>
        /// 获取相机黑白名单，白名单内编码格式为“&粤B12356@20160223165468$20160223165468”
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptGetBalckWhiteList">获取黑白名单参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_GetBlackWhiteList(int tHandle, ref T_GetBlackWhiteList ptGetBalckWhiteList);

        /// <summary>
        /// 获取相机黑白名单，将白名单保存为.csv格式，可用excel打开，白名单内编码格式为“粤B12356,20160223_16:54:68,20160223_16:54:68”
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptGetBalckWhiteList">获取黑白名单参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_GetBlackWhiteListAsCSV(int tHandle, ref T_GetBlackWhiteList ptGetBalckWhiteList);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_LprResult
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] LprResult;/*车牌号码；单条消息最多80条车牌号码；其它分多条消息发送*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] StartTime;//eg:20151012190303 YYYYMMDDHHMMSS
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] EndTime;//eg:20151012190303 YYYYMMDDHHMMSS
        };
        [StructLayout(LayoutKind.Sequential)]
        public struct T_BlackWhiteListCount
        {
            public int uiCount;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] aucLplPath;
        };

        /// <summary>
        /// 黑白名单文件内字符编码必须为GB2312编码；先调用Net_BlackWhiteListSetup将黑白名单信息按照指定格式写在本地，可循环调用该函数向文件内写车牌信息，最后再调用一次Net_BlackWhiteListSend将黑白名单发送到相机，但是一次发送的车牌信息不超过20000条
        /// </summary>
        /// <param name="ptLprResult">黑白名单信息指针</param>
        /// <param name="ptBlackWhiteListCount">黑白名单参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_BlackWhiteListSetup(ref T_LprResult ptLprResult, ref T_BlackWhiteListCount ptBlackWhiteListCount);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveImageToJpeg(int tHandle, string ucPathNmme);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_GetJpgBuffer(int tHandle, ref IntPtr ucJpgBuffer, ref ulong ulSize);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_FreeBuffer(IntPtr pJpgBuffer);

        /// <summary>
        /// 图片抓拍，并将图片保存在指定路径
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="path">图片保存的路径</param>
        /// <returns></returns> 
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveJpgFile(int tHandle, string strJpgFile);

        /// <summary>
        /// 图片抓拍，并将图片保存在指定路径
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="path">图片保存的路径</param>
        /// <returns></returns> 
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SaveBmpFile(int tHandle, string strBmpFile);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StartRecord(int tHandle, string strFile);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_StopRecord(int tHandle);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ShowPlateRegion(int tHandle, int niShowMode);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_UpdatePlateRegion(int tHandle);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_VehPayRsp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] acPlate;	 /* 车牌号码，GB2312编码 */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] acEntryTime; /* 入场时间*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
            public byte[] acExitTime; /* 出场时间*/
            public uint uiRequired;  /* 应付金额，0.1元为单位*/
            public uint uiPrepaid;  	/* 已付金额，0.1元为单位*/
            public byte ucVehType;  /* 车辆类型1小车2大车 E_ParkVehSize*/
            public byte ucUserType;  /*会员类型E_MemberType*/
            public byte ucResultCode; /* 收费结果状态码0 成功 1 没有找到入场记录*/
            public byte acReserved;
        };

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetOffLinePayRecordCB(int tHandle, byte ucType, ref T_VehPayRsp ptVehPayInfo, uint uiLen, ref T_PicInfo ptPicInfo, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegOffLinePayRecord(int tHandle, FGetOffLinePayRecordCB fCb, IntPtr obj);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_NetSetup
        {
            public byte ucEth;				/* 以太网口编号,目前只支持0*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] aucReserved;      /*保留字段*/
            public uint uiIPAddress;        /*ip地址*/
            public uint uiMaskAddress;      /*子网掩码*/
            public uint uiGatewayAddress;   /*网关*/
            public uint uiDNS1;             /*DNS1地址*/
            public uint uiDNS2;             /*DNS2地址*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_HOST_LEN)]
            public byte[] szHostName;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_NETSetup(int tHandle, ref T_NetSetup ptNetSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryNETSetup(int tHandle, ref T_NetSetup ptNetSetup);


        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FDrawFunCallBack(int tHandle, IntPtr hdc, int width, int height, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SetDrawFunCallBack(int tHandle, FDrawFunCallBack fCb, IntPtr obj);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_ReadTwoEncpyption(int tHandle, [MarshalAs(UnmanagedType.LPStr)]StringBuilder pBuff, uint uiSizeBuff);

        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_WriteTwoEncpyption(int tHandle, string pUserData, uint uiDataLen);

        /* 点*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Point
        {
            public short sX;
            public short sY;
        };

        /* 线*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Line
        {
            public T_Point tStartPoint;
            public T_Point tEndPoint;
        };

        /* 区域*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_Rect
        {
            public T_Point tLefTop;
            public T_Point tRightBottom;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_DivisionLine
        {
            public byte ucDashedLine;
            public byte ucReserved;
            public T_Line tLine;
        };

        /*视频检测区域参数配置*/
        [StructLayout(LayoutKind.Sequential)]
        public struct T_VideoDetectParamSetup
        {
            public byte ucLanes;		/*车道数  */
            public byte ucSnapAutoBike; /*摩托车抓拍1:抓拍0:不抓拍*/
            public ushort usBanTime;		/*违停时长阀值，单位:秒*/
            public byte ucVirLoopNum;	/*虚拟线圈，抓拍摩托车使用*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] aucReserved;

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_Point[] atPlateRegion;		/*车牌识别区域*/
            public T_Line aStopLine;              /*卡口:触发线 电子警察:停止线*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_Point[] atSpeedRegion;		/*测速区域  */
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_Line[] atOccupCheckLine;	/*占有率检测线   */
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = ONE_DIRECTION_LANES + 1, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_DivisionLine[] atDivisionLine;/*车道分割线*/
            public T_Line tPrefixLine;		/*电警前置线*/
            public T_Line tLeftBorderLine;	/*电警左边界线*/
            public T_Line tRightBorderLine;	/*电警右边界线*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = ONE_DIRECTION_LANES * 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_Point[] atVirLoop; /*虚拟线圈*/

            /*非法停车区*/
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public T_Point[] atBanRegion;   /*非法禁停区*/

            public ushort usCameraHeight;        /*相机安装高度*/
            public ushort usRectLength;          /*路面矩形长度(厘米)*/
            public ushort usRectWidth;           /*路面矩形宽度(厘米)*/
            public ushort usTotalDis;            /*矩形坐上角到摄像机垂直投影的距离(厘米)*/
        };

        /// <summary>
        /// 设置视频检测模式参数
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptVideoDetectParamSetup">视频检测模式参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_VideoDetectSetup(int tHandle, ref T_VideoDetectParamSetup ptVideoDetectParamSetup);

        /// <summary>
        /// 查询视频检测模式参数
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptVideoDetectParamSetup">视频检测模式参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryVideoDetectSetup(int tHandle, ref T_VideoDetectParamSetup ptVideoDetectParamSetup);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_RS485Data
        {
            public byte rs485Id;
            public byte ucReserved;
            public ushort dataLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
            public byte[] data;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_SendRS485Data(int tHandle, ref T_RS485Data ptRS485Data);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_DCTimeSetup
        {
            public ushort usYear;
            public byte ucMonth;
            public byte ucDay;
            public byte ucHour;
            public byte ucMinute;
            public byte ucSecond;
            public byte ucDayFmt;
            public byte ucTimeFmt;
            public byte ucTimeZone;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] aucReserved;
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_TimeSetup(int tHandle, ref T_DCTimeSetup ptTimeSetup);

        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryTimeSetup(int tHandle, ref T_DCTimeSetup ptTimeSetup);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_VehicleVAFunSetup
        {
            public byte ucPlateRecogEn;				/* 车牌识别使能*/
            public byte ucVehicleSizeRecogEn;		/* 车型识别使能*/
            public byte ucVehicleColorRecogEn;		/* 车身颜色识别使能*/
            public byte ucVehicleBrandRecogEn;		/* 车标识别使能*/
            public byte ucVehicleSizeClassify;		/* 同一辆车抓拍间隔时间高字节*/
            public byte ucLocalCity; 				/*车牌的第二个字符，'A'~'Z'的数字编码*/
            public byte ucPlateDirection;           /*车牌方向E_PlateDirection*/
            public byte ucCpTimeInterval;           //同一辆车抓拍间隔时间低字节
            public uint uiPlateDefaultWord;			/* 默认省份，采用UTF-8编码*/

            public ushort usMinPlateW;				/*车牌识别最小宽度,单位:像素*/
            public ushort usMaxPlateW;				/*车牌识别最大宽度，单位:像素*/
            public byte ucDoubleYellowPlate;		/*双层黄牌识别，1：识别 0：不知别*/
            public byte ucDoubleArmyPlate;			/*双层军牌识别，1：识别 0：不知别*/
            public byte ucPolicePlate;				/*武警车牌识别，1：识别 0：不知别*/
            public byte ucPlateFeature;	            /*车牌特写*/
        };
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_VehicleVAFunSetup(int tHandle, ref T_VehicleVAFunSetup ptVehicleVAFunSetup);

        /// <summary>
        /// 查询车牌识别配置参数
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptVehicleVAFunSetup">车牌识别配置参数指针</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int Net_QueryVehicleVAFunSetup(int tHandle, ref T_VehicleVAFunSetup ptVehicleVAFunSetup);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_MACSetup
        {
            public byte ucEth;				/* 以太网口编号,目前只支持0*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] aucReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] aucMACAddresss;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct T_RcvMsg
        {
            public uint uiFlag;								/*标志位，111表示Version、IP、MAC*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] aucDstMACAdd;					    /*目标MAC地址*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
            public byte[] aucAdapterName;	                    /*网络适配器名称*/
            public uint uiAdapterSubMask;					/*网络适配器子网掩码*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] ancDevType;						    /* 设备类型，出厂时设定；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] ancSerialNum;					    /* 设备序列号*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
            public byte[] ancAppVersion;	                    /* 软件版本*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = VERSION_NAME_LEN)]
            public byte[] ancDSPVersion;	                    /* DSP版本*/
            public T_NetSetup tNetSetup;						/* 网络信息*/
            public T_MACSetup tMacSetup;						/* MAC信息*/
        };
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FNetFindDeviceCallback(ref T_RcvMsg ptFindDevice, IntPtr obj);

        /// <summary>
        /// 通过回调函数搜索局域网内相机的ip等信息,若要调用该函数，电脑需装有winshark抓包工具，提供必要的库支持
        /// </summary>
        /// <param name="fCb">搜索到的一台相机的IP，MAC等信息</param>
        /// <param name="obj">回调函数上下文</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_FindDevice(FNetFindDeviceCallback fCb, IntPtr obj);

        [StructLayout(LayoutKind.Sequential)]
        public struct T_QueVersionRsp
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szKernelVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szFileSystemVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szAppVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szWebVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szHardwareVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szDevType;		/* 设备类型描述，出厂时设定；*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szSerialNum;    	/*产品序列号*/
            public uint uiDateOfExpiry;		/*产品有效期*/
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szDSPVersion;
        };



        [StructLayout(LayoutKind.Sequential)]
        public struct T_LensControl
        {
            public E_LensType ucLensType;
            public E_LensAction ucLensAction;
            public ushort ucLensSteps;// 1或20步
        }

        public enum E_LensType : uint
        {
            LENS_TYPE_ZOOM_MANU = 0,  /*表示当前操作为单步调试变倍马达*/
            LENS_TYPE_ZOOM_AUTO_START, /* 表示当前操作变倍马达，下发开始转到命令*/
            LENS_TYPE_ZOOM_AUTO_STOP, /*表示当前操作变倍马达，下发停止转动命令*/
            LENS_TYPE_FOCUS_MANU, /* 表示当前操作为单步调试聚焦马达*/
            LENS_TYPE_FOCUS_AUTO_START, /*表示当前操作聚焦马达，下发开始转到命令*/
            LENS_TYPE_FOCUS_AUTO_STOP, /*表示当前操作聚焦马达，下发停止转动命令*/
            LENS_TYPE_MAX
        }

        public enum E_LensAction : uint
        {
            LENS_ACTION_SUBTRACT = 0,
            LENS_ACTION_ADD,
            LENS_ACTION_MAX,
        }

        /// <summary>
        /// 控制相机镜头的变倍变焦马达
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="ptLensControl">指向控制相机马达的结构体</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_LensControl(int tHandle, ref T_LensControl ptLensControl);

        /// <summary>
        /// 通过注册回调码流接收函数，获取实时帧数据
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="pReadlDataCallBack">回调句柄</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_SetRealDataCallBack(int tHandle, FNetReadDataCallBack pReadlDataCallBack);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void FNetReadDataCallBack(int tHandle, ulong dwDataType, IntPtr pBuffer, ulong dwBufSize, ref T_FrameInfo pUser);

        /// <summary>
        /// 通过注册回调函数，获取上报的报警消息
        /// </summary>
        /// <param name="tHandle">相机句柄</param>
        /// <param name="fCb">回调句柄</param>
        /// <param name="pUserData">回调函数上下文</param>
        /// <returns></returns>
        [DllImport("NetSDK.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Net_RegReportMessEx(int tHandle, FGetReportCB fCb, IntPtr pUserData);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate int FGetReportCB(int tHandle, E_ReportMess ucType, IntPtr ptMessage, IntPtr pUserData);

        public enum E_ReportMess
        {
            REPORT_MESS_MAC, //消息信息
            REPORT_MESS_ACE, //能见度消息 
            REPORT_MESS_PARK_STATE,// 联机脱机消息 
            REPORT_MESS_RESET_KEY_PRESS,// 复位键短按长按消息 
            REPORT_MESS_VEH_INFO,//上报车辆信息 
            REPORT_MESS_ASS_CAM_IP_SEARCH,//上报搜索到的辅相机IP 
            REPORT_MESS_RS485_IN_DATA,//上报接收到的485数据 
            REPORT_MESS_LOOP_DETECTOR,//上报IO状态 
            REPORT_MESS_MAX
        }



    }
}
