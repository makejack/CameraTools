using System;
using System.Runtime.InteropServices;

namespace CameraTools
{
    /// <summary>
    /// 火眼SDK开发包
    /// </summary>
    class HuoYanClientSdk
    {
        public HuoYanClientSdk()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        /**可过滤的车牌识别触发类型*/
        public const int VZ_LPRC_TRIG_ENABLE_STABLE = 0x1;     /**<允许触发稳定结果*/
        public const int VZ_LPRC_TRIG_ENABLE_VLOOP = 0x2;     /**<允许触发虚拟线圈结果*/
        public const int VZ_LPRC_TRIG_ENABLE_IO_IN1 = 0x10;   /**<允许外部IO_IN_1触发*/
        public const int VZ_LPRC_TRIG_ENABLE_IO_IN2 = 0x20;    /**<允许外部IO_IN_2触发*/
        public const int VZ_LPRC_TRIG_ENABLE_IO_IN3 = 0x40;    /**<允许外部IO_IN_3触发*/

        //车牌类型
        public const int LT_UNKNOWN = 0;   //未知车牌
        public const int LT_BLUE = 1;   //蓝牌小汽车
        public const int LT_BLACK = 2;   //黑牌小汽车
        public const int LT_YELLOW = 3;   //单排黄牌
        public const int LT_YELLOW2 = 4;   //双排黄牌（大车尾牌，农用车）
        public const int LT_POLICE = 5;   //警车车牌
        public const int LT_ARMPOL = 6;   //武警车牌
        public const int LT_INDIVI = 7;   //个性化车牌
        public const int LT_ARMY = 8;   //单排军车牌
        public const int LT_ARMY2 = 9;   //双排军车牌
        public const int LT_EMBASSY = 10;  //使馆车牌
        public const int LT_HONGKONG = 11;  //香港进出中国大陆车牌
        public const int LT_TRACTOR = 12;  //农用车牌
        public const int LT_COACH = 13;  //教练车牌
        public const int LT_MACAO = 14;  //澳门进出中国大陆车牌
        public const int LT_ARMPOL2 = 15; //双层武警车牌
        public const int LT_ARMPOL_ZONGDUI = 16;  // 武警总队车牌
        public const int LT_ARMPOL2_ZONGDUI = 17; // 双层武警总队车牌

        /**可配置的识别类型*/
        public const int VZ_LPRC_REC_BLUE = (1 << (LT_BLUE));						/**<蓝牌车*/
        public const int VZ_LPRC_REC_YELLOW = (1 << (LT_YELLOW) | 1 << (LT_YELLOW2));	/**<黄牌车*/
        public const int VZ_LPRC_REC_BLACK = (1 << (LT_BLACK));						/**<黑牌车*/
        public const int VZ_LPRC_REC_COACH = (1 << (LT_COACH));						/**<教练车*/
        public const int VZ_LPRC_REC_POLICE = (1 << (LT_POLICE));					/**<警车*/
        public const int VZ_LPRC_REC_AMPOL = (1 << (LT_ARMPOL));				/**<武警车*/
        public const int VZ_LPRC_REC_ARMY = (1 << (LT_ARMY) | 1 << (LT_ARMY2));		/**<军车*/
        public const int VZ_LPRC_REC_GANGAO = (1 << (LT_HONGKONG) | 1 << (LT_MACAO));	/**<港澳进出大陆车*/
        public const int VZ_LPRC_REC_EMBASSY = (1 << (LT_EMBASSY));                   /**<使馆车*/

        //触发输入的类型
        public enum VZ_InputType : uint
        {
            nWhiteList = 0,	//通过
            nNotWhiteList,	//不通过
            nNoLicence,		//无车牌
            nBlackList,		//黑名单
            nExtIoctl1,		//开关量/电平输入 1
            nExtIoctl2,		//开关量/电平输入 2
            nExtIoctl3		//开关量/电平输入 3
        };

        //输出配置
        public struct VZ_LPRC_OutputConfig
        {
            public int switchout1;					//开关量输出 1
            public int switchout2;					//开关量输出 2
            public int switchout3;					//开关量输出 3
            public int switchout4;					//开关量输出 4
            public int levelout1;					//电平输出 1 
            public int levelout2;					//电平输出 2
            public int rs485out1;					//RS485-1
            public int rs485out2;					//RS485-2
            VZ_InputType eInputType;		//触发输入的类型
        };

        public enum VZ_LENS_OPT : uint
        {
            VZ_LENS_OPT_STOP,		/**<停止调节*/
            VZ_LENS_FOCUS_FAR,		/**<往远处聚焦*/
            VZ_LENS_FOCUS_NEAR,		/**<往近处聚焦*/
            VZ_LENS_ZOOM_TELE,		/**<往长焦变倍*/
            VZ_LENS_ZOOM_WIDE,		/**<往短焦变倍*/
        }


        public const int MAX_OutputConfig_Len = 7;
        //输出配置信息
        public struct VZ_OutputConfigInfo
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = MAX_OutputConfig_Len, ArraySubType = UnmanagedType.I1)]
            public VZ_LPRC_OutputConfig[] oConfigInfo;	//多个输出配置输出的消息
        };

        /**串口参数*/
        public struct VZ_SERIAL_PARAMETER
        {
            public UInt32 uBaudRate;		// <波特率 300,600,1200,2400,4800,9600,19200,34800,57600,115200
            public UInt32 uParity;		// <校验位 其值为0-2=no,odd,even
            public UInt32 uDataBits;		// <数据位 其值为7,8 位数据位
            public UInt32 uStopBit;		// <停止位 其值为1,2位停止位
        };

        //设置回调函数时需要制定的类型
        public enum VZ_LPRC_CALLBACK_TYPE : uint
        {
            VZ_LPRC_CALLBACK_COMMON_NOTIFY = 0,	//SDK通用信息反馈
            VZ_LPRC_CALLBACK_PLATE_STR,		//车牌号码字符
            VZ_LRPC_CALLBACK_FULL_IMAGE,	//完整图像
            VZ_LPRC_CALLBACK_CLIP_IMAGE,	//截取图像
            VZ_LPRC_CALLBACK_PLATE_RESULT,	//实时识别结果
            VZ_LPRC_CALLBACK_PLATE_RESULT_STABLE,	//稳定识别结果
            VZ_LPRC_CALLBACK_PLATE_RESULT_TRIGGER,	//触发的识别结果，包括API（软件）和IO（硬件）方式的
            VZ_LPRC_CALLBACK_VIDEO,			//视频帧回调
        }
        //通用信息反馈类型
        public enum VZ_LPRC_COMMON_NOTIFY : uint
        {
            VZ_LPRC_NO_ERR = 0,
            VZ_LPRC_ACCESS_DENIED,	//用户名密码错误
            VZ_LPRC_NETWORK_ERR,	//网络连接故障
        }

        //识别结果的类型
        public enum VZ_LPRC_RESULT_TYPE : uint
        {
            VZ_LPRC_RESULT_REALTIME,		/*<实时识别结果*/
            VZ_LPRC_RESULT_STABLE,			/*<稳定识别结果*/
            VZ_LPRC_RESULT_FORCE_TRIGGER,	/*<调用“VzLPRClient_ForceTrigger”触发的识别结果*/
            VZ_LPRC_RESULT_IO_TRIGGER,		/*<外部IO信号触发的识别结果*/
            VZ_LPRC_RESULT_VLOOP_TRIGGER,	/*<虚拟线圈触发的识别结果*/
            VZ_LPRC_RESULT_MULTI_TRIGGER,	/*<由_FORCE_\_IO_\_VLOOP_中的一种或多种同时触发，具体需要根据每个识别结果的TH_PlateResult::uBitsTrigType来判断*/
            VZ_LPRC_RESULT_TYPE_NUM			/*<结果种类个数*/
        }

        //顶点定义
        //X_1000和Y_1000的取值范围为[0, 1000]；
        //即位置信息为实际图像位置在整体图像位置的相对尺寸；
        //例如X_1000 = x*1000/win_width，其中x为点在图像中的水平像素位置，win_width为图像宽度
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_VERTEX
        {
            public uint X_1000;
            public uint Y_1000;
        }

        public const int VZ_LPRC_VIRTUAL_LOOP_NAME_LEN = 32;
        public const int VZ_LPRC_VIRTUAL_LOOP_VERTEX_NUM = 4;
        //虚拟线圈信息定义
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_VIRTUAL_LOOP
        {
            public byte byID;		//序号
            public byte byEnable;	//是否有效
            public byte byDraw;		//是否绘制
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.I1)]
            public byte[] byRes;	//预留
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = VZ_LPRC_VIRTUAL_LOOP_NAME_LEN)]
            public string strName;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = VZ_LPRC_VIRTUAL_LOOP_VERTEX_NUM)]
            public VZ_LPRC_VERTEX[] struVertex;	//顶点数组
            public uint eCrossDir;	                    // 穿越方向限制
            public uint uTriggerTimeGap;	            // 对相同车牌的触发时间间隔的限制，单位为秒
            public uint uMaxLPWidth;		            // 最大车牌尺寸限制
            public uint uMinLPWidth;		            // 最小车牌尺寸限制
        }


        public const int VZ_LPRC_VIRTUAL_LOOP_MAX_NUM = 8;
        //虚拟线圈序列
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_VIRTUAL_LOOPS
        {
            public uint uNumVirtualLoop;	//实际个数
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = VZ_LPRC_VIRTUAL_LOOP_MAX_NUM)]
            public VZ_LPRC_VIRTUAL_LOOP[] struLoop;
        }

        public const int VZ_LPRC_PROVINCE_STR_LEN = 128;

        //预设省份信息
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_PROVINCE_INFO
        {
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = VZ_LPRC_PROVINCE_STR_LEN, ArraySubType = UnmanagedType.I1)]
            public char[] strProvinces;	//所有支持的省份简称构成的字符串
            public int nCurrIndex;	//当前的预设省份的序号，在strProvinces中的，-1为未设置
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TH_RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /**分解时间*/
        [StructLayout(LayoutKind.Sequential)]
        public struct VzBDTime
        {
            public byte bdt_sec;    /*<秒，取值范围[0,59]*/
            public byte bdt_min;    /*<分，取值范围[0,59]*/
            public byte bdt_hour;   /*<时，取值范围[0,23]*/
            public byte bdt_mday;   /*<一个月中的日期，取值范围[1,31]*/
            public byte bdt_mon;    /*<月份，取值范围[1,12]*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = UnmanagedType.I1)]
            public byte[] res1;    /*<预留*/
            public uint bdt_year;   /*<年份*/
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I1)]
            public byte[] res2;    /*<预留*/
        }   //broken-down time

        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_TIMEVAL
        {
            public uint uTVSec;
            public uint uTVUSec;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TH_PlateResult
        {
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] license;   // 车牌号码
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] color;      // 车牌颜色
            public int nColor;			// 车牌颜色序号
            public int nType;			// 车牌类型
            public int nConfidence;	// 车牌可信度
            public int nBright;		// 亮度评价
            public int nDirection;		// 运动方向，0 unknown, 1 left, 2 right, 3 up , 4 down	
            public TH_RECT rcLocation; //车牌位置
            public int nTime;          //识别所用时间
            public VZ_TIMEVAL tvPTS;    //识别时间点
            public uint uBitsTrigType;  //强制触发结果的类型，见TH_TRIGGER_TYPE_BIT
            public byte nCarBright;		//车的亮度
            public byte nCarColor;		//车的颜色
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] reserved0;   //为了对齐
            public uint uId;            //记录的编号
            public VzBDTime struBDTime; /*<分解时间*/

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 84, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public char[] reserved;				// 保留
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VzYUV420P
        {
            public IntPtr pY;
            public IntPtr pU;
            public IntPtr pV;
            int widthStepY;
            int widthStepU;
            int widthStepV;
            int width;
            int height;
        }

        //图像信息
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_IMAGE_INFO
        {
            public uint uWidth;
            public uint uHeight;
            public uint uPitch;
            public uint uPixFmt;
            public IntPtr pBuffer;
        }

        //智能视频        
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_LPRC_DRAWMODE
        {
            public byte byDspAddTarget;		//dsp叠加报警目标
            public byte byDspAddRule;			//dsp叠加设置规则
            public byte byDspAddTrajectory;	//dsp叠加轨迹	

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I1)]
            public byte[] dwRes;
        };

        //设备序列号
        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_DEV_SERIAL_NUM
        {
            public uint uHi;
            public uint uLo;
        }
        //********白名单********//


        [StructLayout(LayoutKind.Sequential)]
        public struct VZ_TM
        {

            /// short
            public short nYear;

            /// short
            public short nMonth;

            /// short
            public short nMDay;

            /// short
            public short nHour;

            /// short
            public short nMin;

            /// short
            public short nSec;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_TM_WEEK_DAY
        {

            /// char
            public byte bSun;

            /// char
            public byte bMon;

            /// char
            public byte bTue;

            /// char
            public byte bWed;

            /// char
            public byte bThur;

            /// char
            public byte bFri;

            /// char
            public byte bSat;

            /// char
            public byte reserved;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPRC_OSD_Param
        {
            public byte dstampenable;					// 0 off 1 on
            public int dateFormat;						// 0:YYYY/MM/DD;1:MM/DD/YYYY;2:DD/MM/YYYY
            public int datePosX;
            public int datePosY;
            public byte tstampenable;   				// 0 off 1 on
            public int timeFormat;						// 0:12Hrs;1:24Hrs
            public int timePosX;
            public int timePosY;
            public byte nLogoEnable;					// 0 off 1 on
            public int nLogoPositionX;   				//<  logo position
            public int nLogoPositionY;   				//<  logo position
            public byte nTextEnable;					//0 off 1 on
            public int nTextPositionX;   				//<  text position
            public int nTextPositionY;   				//<  text position
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 16)]
            public string overlaytext;              	//user define text           	//user define text
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_TM_DAY
        {

            /// short
            public short nHour;

            /// short
            public short nMin;

            /// short
            public short nSec;

            /// short
            public short reserved;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_TM_WEEK_SEGMENT
        {

            /// unsigned int
            public uint uEnable;

            /// VZ_TM_WEEK_DAY->Anonymous_a54d933b_d2e6_4eba_97b3_61ea9b47dd3b
            public VZ_TM_WEEK_DAY struDaySelect;

            /// VZ_TM_DAY->Anonymous_2bafa8b8_e11f_4cdc_a109_eb09791f91d6
            public VZ_TM_DAY struDayTimeStart;

            /// VZ_TM_DAY->Anonymous_2bafa8b8_e11f_4cdc_a109_eb09791f91d6
            public VZ_TM_DAY struDayTimeEnd;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_TM_RANGE
        {

            /// VZ_TM->Anonymous_40d76b6c_816a_4821_a5db_3480cee2a116
            public VZ_TM struTimeStart;

            /// VZ_TM->Anonymous_40d76b6c_816a_4821_a5db_3480cee2a116
            public VZ_TM struTimeEnd;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_TM_PERIOD_OR_RANGE
        {

            /// unsigned int
            public uint uEnable;

            /// VZ_TM_WEEK_SEGMENT[8]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.Struct)]
            public VZ_TM_WEEK_SEGMENT[] struWeekSeg;
        }

        /**设备日期时间参数*/
        public struct VZ_DATE_TIME_INFO
        {
            public uint uYear;		/*<年*/
            public uint uMonth;	    /*<月 [1, 12]*/
            public uint uMDay;		/*<月中的天数 [1, 31]*/
            public uint uHour;		/*<时*/
            public uint uMin;		/*<分*/
            public uint uSec;		/*<秒*/
        }

        public enum VZ_LPR_WLIST_ERROR
        {

            /// VZ_LPR_WLIST_ERROR_NO_ERROR -> 0
            VZ_LPR_WLIST_ERROR_NO_ERROR = 0,

            VZ_LPR_WLIST_ERROR_PLATEID_EXISTS,

            VZ_LPR_WLIST_ERROR_INSERT_CUSTOMERINFO_FAILED,

            VZ_LPR_WLIST_ERROR_INSERT_VEHICLEINFO_FAILED,

            VZ_LPR_WLIST_ERROR_UPDATE_CUSTOMERINFO_FAILED,

            VZ_LPR_WLIST_ERROR_UPDATE_VEHICLEINFO_FAILED,

            VZ_LPR_WLIST_ERROR_PLATEID_EMPTY,

            VZ_LPR_WLIST_ERROR_ROW_NOT_CHANGED,

            VZ_LPR_WLIST_ERROR_CUSTOMERINFO_NOT_CHANGED,

            VZ_LPR_WLIST_ERROR_VEHICLEINFO_NOT_CHANGED,

            VZ_LPR_WLIST_ERROR_CUSTOMER_VEHICLE_NOT_MATCH,

            VZ_LPR_WLIST_ERROR_SERVER_GONE,
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_WLIST_VEHICLE
        {

            /// unsigned int
            public uint uVehicleID;

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string strPlateID;

            /// unsigned int
            public uint uCustomerID;

            /// unsigned int
            public uint bEnable;

            /// unsigned int
            public uint bEnableTMEnable;

            /// unsigned int
            public uint bEnableTMOverdule;

            /// VZ_TM*
            public VZ_TM struTMEnable;

            public VZ_TM struTMOverdule;

            /// unsigned int
            public uint bUsingTimeSeg;

            /// VZ_TM_PERIOD_OR_RANGE->Anonymous_6f46bf7e_03f5_450b_84da_e56739a41561
            public VZ_TM_PERIOD_OR_RANGE struTimeSegOrRange;

            /// unsigned int
            public uint bAlarm;

            public uint iColor;

            public uint iPlateType;										/**<车牌类型*/

            // 车辆代码
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string strCode;

            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
            public string strComment;	/**<车辆备注*/
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_WLIST_CUSTOMER
        {

            /// unsigned int
            public uint uCustomerID;

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string strName;

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string strCode;

            /// char[256]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string reserved;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_ROW
        {

            /// VZ_LPR_WLIST_CUSTOMER*
            public System.IntPtr pCustomer;

            /// VZ_LPR_WLIST_VEHICLE*
            public System.IntPtr pVehicle;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_WLIST_SEARCH_CONSTRAINT
        {

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string key;

            /// char[128]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 128)]
            public string search_string;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_MSG_PLATE_INFO
        {

            /// char[32]
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string plate;


            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 37)]
            //public string time;

            /// char[128]
            [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 200)]
            public string img_path;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_DEVICE_INFO
        {
            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string device_ip;

            /// char[64]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 64)]
            public string serial_no;
        }

        public enum VZ_LPR_WLIST_LIMIT_TYPE
        {

            VZ_LPR_WLIST_LIMIT_TYPE_ONE,

            VZ_LPR_WLIST_LIMIT_TYPE_ALL,

            VZ_LPR_WLIST_LIMIT_TYPE_RANGE,
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_RANGE_LIMIT
        {

            /// int
            public int startIndex;

            /// int
            public int count;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_LIMIT
        {

            /// VZ_LPR_WLIST_LIMIT_TYPE->Anonymous_988ed792_488c_49e0_9b97_4fef91401704
            public VZ_LPR_WLIST_LIMIT_TYPE limitType;

            /// VZ_LPR_WLIST_RANGE_LIMIT*
            public System.IntPtr pRangeLimit;
        }

        public enum VZ_LPR_WLIST_SORT_DIRECTION
        {

            /// VZ_LPR_WLIST_SORT_DIRECTION_DESC -> 0
            VZ_LPR_WLIST_SORT_DIRECTION_DESC = 0,

            /// VZ_LPR_WLIST_SORT_DIRECTION_ASC -> 1
            VZ_LPR_WLIST_SORT_DIRECTION_ASC = 1,
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_WLIST_SORT_TYPE
        {

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string key;

            /// VZ_LPR_WLIST_SORT_DIRECTION->Anonymous_dde74036_93c7_4601_966c_0439d47c4836
            public VZ_LPR_WLIST_SORT_DIRECTION direction;
        }

        public enum VZ_LPR_WLIST_SEARCH_TYPE
        {

            VZ_LPR_WLIST_SEARCH_TYPE_LIKE,

            VZ_LPR_WLIST_SEARCH_TYPE_EQUAL,
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_SEARCH_WHERE
        {

            /// VZ_LPR_WLIST_SEARCH_TYPE->Anonymous_e3b38339_d7de_4d6d_998f_8f03f1a82e9c
            public VZ_LPR_WLIST_SEARCH_TYPE searchType;

            /// unsigned int
            public uint searchConstraintCount;

            /// VZ_LPR_WLIST_SEARCH_CONSTRAINT*
            public System.IntPtr pSearchConstraints;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_LOAD_CONDITIONS
        {

            /// VZ_LPR_WLIST_SEARCH_WHERE*
            public System.IntPtr pSearchWhere;

            /// VZ_LPR_WLIST_LIMIT*
            public System.IntPtr pLimit;

            /// VZ_LPR_WLIST_SORT_TYPE*
            public System.IntPtr pSortType;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public struct VZ_LPR_WLIST_KEY_DEFINE
        {

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string key;

            /// char[32]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 32)]
            public string name;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct VZ_LPR_WLIST_IMPORT_RESULT
        {

            /// int
            public int ret;

            /// int
            public int error_code;
        }

        public enum VZLPRC_WLIST_CB_TYPE
        {

            /// VZLPRC_WLIST_CB_TYPE_ROW -> 0
            VZLPRC_WLIST_CB_TYPE_ROW = 0,

            VZLPRC_WLIST_CB_TYPE_CUSTOMER,

            VZLPRC_WLIST_CB_TYPE_VEHICLE,
        }

        /**LED补光灯命令*/
        public enum VZ_LED_CTRL
        {
            VZ_LED_AUTO,		/*<自动控制LED的开和关*/
            VZ_LED_MANUAL_ON,	/*<手动控制LED开启*/
            VZ_LED_MANUAL_OFF,	/*<手动控制LED关闭*/
        }

        //API
        /**
        *  @brief 全局初始化，在所有接口调用之前调用
        *  @return 0表示成功，-1表示失败
        */


        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_Setup();

        /**
        *  @brief 全局释放
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern void VzLPRClient_Cleanup();

        /// <summary>
        /// 通过该回调函数获得设备的一般状态信息 
        /// </summary>
        /// <param name="handle">[IN] handle 由VzLPRClient_Open函数获得的句柄</param>
        /// <param name="pUserData">[IN] pUserData 回调函数上下文 </param>
        /// <param name="eNotify">[IN] eNotify 通用信息反馈类型  </param>
        /// <param name="pStrDetail">[IN] pStrDetail 详细描述字符串 </param>
        public delegate void VZLPRC_COMMON_NOTIFY_CALLBACK(int handle, IntPtr pUserData,
                                                       VZ_LPRC_COMMON_NOTIFY eNotify, string pStrDetail);

        /**
        *  @brief 设置设备连接反馈结果相关的回调函数
        *  @param  [IN] func 设备连接结果和状态，通过该回调函数返回
        *  @param [IN] pUserData 回调函数中的上下文
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VZLPRClient_SetCommonNotifyCallBack(VZLPRC_COMMON_NOTIFY_CALLBACK func, IntPtr pUserData);

        /**
        *  @brief 打开一个设备
        *  @param  [IN] pStrIP 设备的IP地址
        *  @param [IN] wPort 设备的端口号
        *  @param  [IN] pStrUserName 访问设备所需用户名
        *  @param [IN] pStrPassword 访问设备所需密码
        *  @return 返回设备的操作句柄，当打开失败时，返回-1
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_Open(string pStrIP, ushort wPort, string pStrUserName, string pStrPassword);

        /**
        *  @brief 关闭一个设备
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_Close(int handle);

        /**
        *  @brief 通过IP地址关闭一个设备
        *  @param  [IN] pStrIP 设备的IP地址
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_CloseByIP(string pStrIP);

        /**
        *  @brief 获取连接状态
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param[IN/OUT] pStatus 输入获取状态的变量地址，输出内容为 1已连上，0未连上
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_IsConnected(int handle, ref byte pStatus);

        /**
        *  @brief 播放实时视频
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] hWnd 窗口的句柄
        *  @return 播放句柄，小于0表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_StartRealPlay(int handle, IntPtr hWnd);

        /**
        *  @brief 停止正在播放的窗口上的实时视频
        *  @param  [IN] hWnd 窗口的句柄
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_StopRealPlay(int hRealHandle);

        public delegate int VZLPRC_PLATE_INFO_CALLBACK(int handle, IntPtr pUserData,
                                                    IntPtr pResult, uint uNumPlates,
                                                    VZ_LPRC_RESULT_TYPE eResultType,
                                                    IntPtr pImgFull,
                                                    IntPtr pImgPlateClip);

        /**
        *  @brief 设置识别结果的回调函数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] func 识别结果回调函数
        *  @param  [IN] pUserData 回调函数中的上下文
        *  @param  [IN] bEnableImage 指定识别结果的回调是否需要包含截图信息：1为需要，0为不需要
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int VzLPRClient_SetPlateInfoCallBack(int handle, VZLPRC_PLATE_INFO_CALLBACK func, IntPtr pUserData, int bEnableImage);


        /**
        *  @brief  通过该回调函数获得实时图像数据
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pUserData	回调函数的上下文
        *  @param  [IN] pFrame		图像帧信息，详见结构体定义VzYUV420P
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_callback
        */
        public delegate void VZLPRC_VIDEO_FRAME_CALLBACK(int handle, IntPtr pUserData, ref VzYUV420P pFrame);

        /**
        *  @brief 设置实时图像数据的回调函数
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] func		实时图像数据函数
        *  @param  [IN] pUserData	回调函数中的上下文
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int VzLPRClient_SetVideoFrameCallBack(int handle, VZLPRC_VIDEO_FRAME_CALLBACK pFunc, IntPtr pUserData);

        /**
        *  @brief 发送软件触发信号，强制处理当前时刻的数据并输出结果
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_ForceTrigger(int handle);

        /**
        *  @brief 设置虚拟线圈
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pVirtualLoops 虚拟线圈的结构体指针
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVirtualLoop(int handle, ref VZ_LPRC_VIRTUAL_LOOPS pVirtualLoops);

        /**
        *  @brief 获取已设置的虚拟线圈
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pVirtualLoops 虚拟线圈的结构体指针
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVirtualLoop(int handle, ref VZ_LPRC_VIRTUAL_LOOPS pVirtualLoops);

        /**
        *  @brief 获取已设置的预设省份
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pProvInfo 预设省份信息指针
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetSupportedProvinces(int handle, ref VZ_LPRC_PROVINCE_INFO pProvInfo);

        /**
        *  @brief 设置预设省份
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] nIndex 设置预设省份的序号，序号需要参考VZ_LPRC_PROVINCE_INFO::strProvinces中的顺序，从0开始，如果小于0，则表示不设置预设省份
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_PresetProvinceIndex(int handle, int nIndex);

        /**
        *  @brief 将图像保存为JPEG到指定路径
        *  @param  [IN] pImgInfo 图像结构体，目前只支持默认的格式，即ImageFormatRGB
        *  @param  [IN] pFullPathName 设带绝对路径和JPG后缀名的文件名字符串
        *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
        *  @return 0表示成功，-1表示失败
        *  @note   给定的文件名中的路径需要存在
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_ImageSaveToJpeg(IntPtr pImgInfo, string pFullPathName, int nQuality);


        /**
        *  @brief 读出设备序列号，可用于二次加密
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN/OUT] pSN 用于存放读到的设备序列号，详见定义 VZ_DEV_SERIAL_NUM
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetSerialNumber(int handle, ref VZ_DEV_SERIAL_NUM pSN);

        /**
        *  @brief 保存正在播放的视频的当前帧的截图到指定路径
        *  @param  [IN] nPlayHandle 播放的句柄
        *  @param  [IN] pFullPathName 设带绝对路径和JPG后缀名的文件名字符串
        *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
        *  @return 0表示成功，-1表示失败
        *  @note   使用的文件名中的路径需要存在
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetSnapShootToJpeg2(int nPlayHandle, string pFullPathName, int nQuality);

        /**
        *  @brief 通过该回调函数获得透明通道接收的数据
        *  @param  [IN] nSerialHandle VzLPRClient_SerialStart返回的句柄
        *  @param  [IN] pStrIPAddr	设备IP地址
        *  @param  [IN] usPort1		设备端口号
        *  @param  [IN] usPort2		预留
        *  @param  [IN] pUserData	回调函数上下文
        *  @ingroup group_global
        */
        public delegate int VZDEV_SERIAL_RECV_DATA_CALLBACK(int nSerialHandle, IntPtr pRecvData, int uRecvSize, IntPtr pUserData);

        /**
        *  @brief 开启透明通道
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] nSerialPort 指定使用设备的串口序号：0表示第一个串口，1表示第二个串口
        *  @param  [IN] func 接收数据的回调函数
        *  @param  [IN] pUserData 接收数据回调函数的上下文
        *  @return 返回透明通道句柄，0表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SerialStart(int handle, int nSerialPort, VZDEV_SERIAL_RECV_DATA_CALLBACK func, IntPtr pUserData);

        /**
        *  @brief 透明通道发送数据
        *  @param [IN] nSerialHandle 由VzLPRClient_SerialStart函数获得的句柄
        *  @param [IN] pData 将要传输的数据块的首地址
        *  @param [IN] uSizeData 将要传输的数据块的字节数
        *  @return 0表示成功，其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SerialSend(int nSerialHandle, IntPtr pData, int uSizeData);

        /**
        *  @brief 透明通道停止发送数据
        *  @param [IN] nSerialHandle 由VzLPRClient_SerialStart函数获得的句柄
        *  @return 0表示成功，其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SerialStop(int nSerialHandle);

        /**
        *  @brief 设置IO输出的状态
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] uChnId IO输出的通道号，从0开始
        *  @param  [OUT] nOutput 将要设置的IO输出的状态，0表示继电器开路，1表示继电器闭路
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetIOOutput(int handle, int uChnId, int nOutput);

        /**
        *  @brief 获取IO输出的状态
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] uChnId IO输出的通道号，从0开始
        *  @param  [OUT] pOutput IO输出的状态，0表示继电器开路，1表示继电器闭路
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetIOOutput(int handle, int uChnId, ref int pOutput);

        /**
        *  @brief 获取GPIO的状态
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] gpioIn 数据为0或1
        *  @param  [OUT] value 0代表短路，1代表开路
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetGPIOValue(int handle, int gpioIn, IntPtr value);

        /**
        *  @brief 根据ID获取车牌图片
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] id     车牌记录的ID
        *  @param  [IN] pdata  存储图片的内存
        *  @param  [IN][OUT] size 为传入传出值，传入为图片内存的大小，返回的是获取到jpg图片内存的大小
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_LoadImageById(int handle, int id, IntPtr pdata, IntPtr size);

        /**
        *  @brief 向白名单表导入客户和车辆记录
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] rowcount 记录的条数
        *  @param  [IN] pRowDatas 记录的内容数组的地址
        *  @param  [OUT] results 每条数据是否导入成功
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListImportRows(int handle,
                                                                  uint rowcount,
                                                                  ref VZ_LPR_WLIST_ROW pRowDatas,
                                                                  ref VZ_LPR_WLIST_IMPORT_RESULT pResults);

        /**
        *  @brief 从数据库删除车辆信息
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] strPlateID 车牌号码
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListDeleteVehicle(int handle, string strPlateID);

        /**
        *  @brief 查询白名单表车辆记录数据
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pLoadCondition 查询条件
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListLoadVehicle(int handle,
                                                            ref VZ_LPR_WLIST_LOAD_CONDITIONS pLoadCondition);

        public delegate void VZLPRC_WLIST_QUERY_CALLBACK(VZLPRC_WLIST_CB_TYPE type, IntPtr pLP,
                                                         IntPtr pCustomer,
                                                         IntPtr pUserData);
        /**
        *  @brief 设置白名单表和客户信息表的查询结果回调
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] func 查询结果回调函数
        *  @param  [IN] pUserData 回调函数中的上下文
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListSetQueryCallBack(int handle, VZLPRC_WLIST_QUERY_CALLBACK func, IntPtr pUserData);

        /**
        *  @brief 往白名单表中更新一个车辆信息
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pVehicle 将要更新的车辆信息，详见结构体定义VZ_LPR_WLIST_VEHICLE
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListUpdateVehicleByID(int handle, ref VZ_LPR_WLIST_VEHICLE pVehicle);


        /**
        *  @brief 查询白名单表客户和车辆记录条数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [OUT] pCount 记录的条数
        *  @param  [IN] search_constraints 搜索的条件
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_database
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WhiteListGetRowCount(int handle, ref int count, ref VZ_LPR_WLIST_SEARCH_WHERE pSearchWhere);

        /**
        *  @brief 获取LED当前控制模式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] pCtrl 用于输出当前LED开关控制模式的地址，详见定义 VZ_LED_CTRL  
        *  @return 返回值为0表示成功，返回其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetLEDLightControlMode(int handle, ref VZ_LED_CTRL pCtrl);
        /**
        *  @brief 设置LED控制模式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] eCtrl 控制LED开关模式，详见定义 VZ_LED_CTRL
        *  @return 返回值为0表示成功，返回其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetLEDLightControlMode(int handle, VZ_LED_CTRL eCtrl);
        /**
        *  @brief 获取LED当前亮度等级和最大亮度等级
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] pLevelNow 用于输出当前亮度等级的地址
        *  @param [OUT] pLevelMax 用于输出最高亮度等级的地址
        *  @return 0表示成功，其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetLEDLightStatus(int handle, ref int pLevelNow, ref int pLevelMax);

        /**
        *  @brief 设置LED亮度等级
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] nLevel，LED亮度等级
        *  @return 0表示成功，其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetLEDLightLevel(int handle, int nLevel);

        /**
        *  @brief 开始录像功能
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] sFileName 录像文件的路径
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SaveRealData(int handle, string sFileName);

        /**
        *  @brief 停止录像
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_StopSaveRealData(int handle);

        /**
        *  @brief 开启脱机功能
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pUserData 接收数据回调函数的上下文
        *   @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetOfflineCheck(int handle);

        /**
        *  @brief 关闭脱机功能
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pUserData 接收数据回调函数的上下文
        *   @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_CancelOfflineCheck(int handle);
        /**
        *  @brief 通过该回调函数获得透明通道接收的数据
        *  @param  [IN] nSerialHandle VzLPRClient_SerialStart返回的句柄
        *  @param  [IN] pRecvData	接收的数据的首地址
        *  @param  [IN] uRecvSize	接收的数据的尺寸
        *  @param  [IN] pUserData	回调函数上下文
        *  @ingroup group_callback
        */

        /**
        *  @brief 透明通道发送数据
        *  @param [IN] nSerialHandle 由VzLPRClient_SerialStart函数获得的句柄
        *  @param [IN] pData 将要传输的数据块的首地址
        *  @param [IN] uSizeData 将要传输的数据块的字节数
        *  @return 0表示成功，其他值表示失败
        *  @ingroup group_device
        */
        //[DllImport("VzLPRSDK.dll")]
        //public static extern int VzLPRClient_SerialSend(int nSerialHandle,string pData, uint uSizeData);

        /**
        *  @brief 通过该回调函数获得找到的设备基本信息
        *  @param  [IN] pStrDevName 设备名称
        *  @param  [IN] pStrIPAddr	设备IP地址
        *  @param  [IN] usPort1		设备端口号
        *  @param  [IN] usPort2		预留
        *  @param  [IN] pUserData	回调函数上下文
        *  @ingroup group_callback
        */
        public delegate void VZLPRC_FIND_DEVICE_CALLBACK(string pStrDevName, string pStrIPAddr, ushort usPort1, ushort usPort2, uint SL, uint SH, IntPtr pUserData);

        /**
        *  @brief 开始查找设备
        *  @param  [IN] func 找到的设备通过该回调函数返回
        *  @param  [IN] pUserData 回调函数中的上下文
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VZLPRClient_StartFindDevice(VZLPRC_FIND_DEVICE_CALLBACK func, IntPtr pUserData);

        /**
        *  @brief 停止查找设备
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VZLPRClient_StopFindDevice();

        /**
        *  @brief 根据起始时间和车牌关键字查询记录
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pStartTime 起始时间，格式如"2015-01-02 12:20:30"
        *  @param  [IN] pEndTime   起始时间，格式如"2015-01-02 19:20:30"
        *  @param  [IN] keyword    车牌号关键字, 如"川"
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @说明   通过回调返回数据，最多返回100条数据，超过时请调用分页查询的接口
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_QueryRecordByTimeAndPlate(int handle, string pStartTime, string pEndTime, string keyword);


        /**
        *  @brief 根据时间和车牌号查询记录条数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pStartTime 起始时间，格式如"2015-01-02 12:20:30"
        *  @param  [IN] pEndTime   起始时间，格式如"2015-01-02 19:20:30"
        *  @param  [IN] keyword    车牌号关键字, 如"川"
        *  @return 返回值为0表示失败，大于0表示记录条数
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_QueryCountByTimeAndPlate(int handle, string pStartTime, string pEndTime, string keyword);

        /**
        *  @brief 根据时间和车牌号查询分页查询记录
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] pStartTime 起始时间，格式如"2015-01-02 12:20:30"
        *  @param  [IN] pEndTime   起始时间，格式如"2015-01-02 19:20:30"
        *  @param  [IN] keyword    车牌号关键字, 如"川"
        *  @param  [IN] start      起始位置大于0,小于结束位置
        *  @param  [IN] end        结束位置大于0,大于起始位置，获取记录条数不能大于100
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_QueryPageRecordByTimeAndPlate(int handle, string pStartTime, string pEndTime, string keyword, int start, int end);


        /**
        *  @brief 设置查询车牌记录的回调函数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] func 识别结果回调函数，如果为NULL，则表示关闭该回调函数的功能
        *  @param  [IN] pUserData 回调函数中的上下文
        *  @param  [IN] bEnableImage 指定识别结果的回调是否需要包含截图信息：1为需要，0为不需要
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetQueryPlateCallBack(int handle, VZLPRC_PLATE_INFO_CALLBACK func, IntPtr pUserData);

        /**
        *  @brief 获取视频OSD参数；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetOsdParam(int handle, IntPtr pParam);

        /**
        *  @brief 设置视频OSD参数；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetOsdParam(int handle, IntPtr pParam);

        /**
        *  @brief 设置设备的日期时间
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] pDTInfo 将要设置的设备日期时间信息，详见定义 VZ_DATE_TIME_INFO
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetDateTime(int handle, IntPtr IntpDTInfo);

        /**
        *  @brief 读出用户私有数据，可用于二次加密
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN/OUT] pBuffer 用于存放读到的用户数据
        *  @param [IN] uSizeBuf 用户数据缓冲区的最小尺寸，不小于128字节
        *  @return 返回值为实际用户数据的字节数，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_ReadUserData(int handle, IntPtr pBuffer, uint uSizeBuf);

        /**
        *  @brief 写入用户私有数据，可用于二次加密
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] pUserData 用户数据
        *  @param [IN] uSizeData 用户数据的长度，最大128字节
        *  @return 返回值为0表示成功，返回其他值表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_WriteUserData(int handle, IntPtr pUserData, uint uSizeData);

        /**
        *  @brief 将图像编码为JPEG，保存到指定内存
        *  @param  [IN] pImgInfo 图像结构体，目前只支持默认的格式，即ImageFormatRGB
        *  @param  [IN/OUT] pDstBuf JPEG数据的目的存储首地址
        *  @param  [IN] uSizeBuf JPEG数据地址的内存的最大尺寸；
        *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
        *  @return >0表示成功，即编码后的尺寸，-1表示失败，-2表示给定的压缩数据的内存尺寸不够大
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_ImageEncodeToJpeg(IntPtr pImgInfo, IntPtr pDstBuf, int uSizeBuf, int nQuality);

        /**
        *  @brief 设置IO输出，并自动复位
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] uChnId IO输出的通道号，从0开始
        *  @param  [IN] nDuration 延时时间，取值范围[500, 5000]毫秒
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetIOOutputAuto(int handle, int uChnId, int nDuration);

        public delegate void VZLPRC_VIDEO_FRAME_CALLBACK_EX(int handle, IntPtr pUserData, ref VZ_LPRC_IMAGE_INFO pFrame);

        /**
        *  @brief 获取实时视频帧，图像数据通过回调函数到用户层，用户可改动图像内容，并且显示到窗口
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] hWnd		窗口的句柄，如果为有效值，则视频图像会显示到该窗口上，如果为空，则不显示视频图像
        *  @param  [IN] func		实时图像数据函数
        *  @param  [IN] pUserData	回调函数中的上下文
        *  @return 播放的句柄，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int VzLPRClient_StartRealPlayFrameCallBack(int handle, IntPtr hWnd, VZLPRC_VIDEO_FRAME_CALLBACK_EX func, IntPtr pUserData);

        /**
        *  @brief 获取已设置的允许的车牌识别触发类型
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] pBitsTrigType 允许的车牌识别触发类型按位或的变量的地址，允许触发类型位详见定义VZ_LPRC_TRIG_ENABLE_XXX
        *  @return 返回值：返回值为0表示成功，返回其他值表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetPlateTrigType(int handle, ref int pBitsTrigType);

        /**
        *  @brief 设置允许的车牌识别触发类型
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] uBitsTrigType 允许的车牌识别触发类型按位或的值，允许触发类型位详见定义VZ_LPRC_TRIG_ENABLE_XXX
        *  @return 返回值：返回值为0表示成功，返回其他值表示失败
        *  @note  如果设置不允许某种类型的触发，那么该种类型的触发结果也不会保存在设备的SD卡中
        *  @note  默认输出稳定触发和虚拟线圈触发
        *  @note  不会影响手动触发和IO输入触发
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetPlateTrigType(int handle, UInt32 uBitsTrigType);

        /**
        *  @brief 获取智能视频显示模式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] pDrawMode 显示模式，参考VZ_LPRC_DRAWMODE
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetDrawMode(int handle, ref VZ_LPRC_DRAWMODE pDrawMode);

        /**
        *  @brief 设置智能视频显示模式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] pDrawMode 显示模式，参考VZ_LPRC_DRAWMODE
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetDrawMode(int handle, ref VZ_LPRC_DRAWMODE pDrawMode);

        /**
        *  @brief 获取已设置的需要识别的车牌类型位
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] pBitsRecType 需要识别的车牌类型按位或的变量的地址，车牌类型位详见定义VZ_LPRC_REC_XXX
        *  @return 返回值：返回值为0表示成功，返回其他值表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetPlateRecType(int handle, ref int pBitsRecType);

        /**
        *  @brief 设置需要识别的车牌类型
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] uBitsRecType 需要识别的车牌类型按位或的值，车牌类型位详见定义VZ_LPRC_REC_XXX
        *  @return 返回值：返回值为0表示成功，返回其他值表示失败
        *  @note  在需要识别特定车牌时，调用该接口来设置，将不同类型的车牌位定义取或，得到的结果作为参数传入；
        *  @note  在不必要的情况下，使用最少的车牌识别类型，将最大限度提高识别率；
        *  @note  默认识别蓝牌和黄牌；
        *  @note  例如，需要识别蓝牌、黄牌、警牌，那么输入参数uBitsRecType = VZ_LPRC_REC_BLUE|VZ_LPRC_REC_YELLOW|VZ_LPRC_REC_POLICE
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetPlateRecType(int handle, UInt32 uBitsRecType);

        /**
        *  @brief 获取输出配置0
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] pOutputConfig 输出配置
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetOutputConfig(int handle, ref VZ_OutputConfigInfo pOutputConfigInfo);

        /**
        *  @brief 设置输出配置
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] pOutputConfig 输出配置
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetOutputConfig(int handle, ref VZ_OutputConfigInfo pOutputConfigInfo);

        /**
        *  @brief 设置车牌识别触发延迟时间
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] nDelay 触发延迟时间,时间范围[0, 10000)
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetTriggerDelay(int handle, int nDelay);

        /**
        *  @brief 获取车牌识别触发延迟时间
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [OUT] nDelay 触发延迟时间,时间范围[0, 10000)
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetTriggerDelay(int handle, ref int nDelay);

        /**
        *  @brief 设置白名单验证模式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] nType 0 脱机自动启用;1 启用;2 不启用
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetWLCheckMethod(int handle, int nType);

        /**
        *  @brief 获取白名单验证模式
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT]	nType 0 脱机自动启用;1 启用;2 不启用
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetWLCheckMethod(int handle, ref int nType);

        /**
        *  @brief 设置白名单模糊匹配
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] nFuzzyType 0  精确匹配;1 相似字符匹配;2 普通字符模糊匹配
        *  @param [IN] nFuzzyLen  允许误识别长度
        *  @param [IN] nFuzzyType 忽略汉字
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetWLFuzzy(int handle, int nFuzzyType, int nFuzzyLen, bool bFuzzyCC);

        /**
        *  @brief 获取白名单模糊匹配
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] nFuzzyType 0  精确匹配;1 相似字符匹配;2 普通字符模糊匹配
        *  @param [IN] nFuzzyLen  允许误识别长度
        *  @param [IN] nFuzzyType 忽略汉字
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetWLFuzzy(int handle, ref int nFuzzyType, ref int nFuzzyLen, ref bool bFuzzyCC);

        /**
        *  @brief 设置串口参数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] nSerialPort 指定使用设备的串口序号：0表示第一个串口，1表示第二个串口
        *  @param  [IN] pParameter 将要设置的串口参数，详见定义 VZ_SERIAL_PARAMETER
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetSerialParameter(int handle, int nSerialPort,
                                                         ref VZ_SERIAL_PARAMETER pParameter);

        /**
        *  @brief 获取串口参数
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] nSerialPort 指定使用设备的串口序号：0表示第一个串口，1表示第二个串口
        *  @param  [OUT] pParameter 将要获取的串口参数，详见定义 VZ_SERIAL_PARAMETER
        *  @return 0表示成功，-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetSerialParameter(int handle, int nSerialPort,
                                                         ref VZ_SERIAL_PARAMETER pParameter);
        //        /**
        //        *  @brief 保存正在播放的视频的当前帧的截图到指定路径
        //        *  @param  [IN] nPlayHandle 播放的句柄
        //        *  @param  [IN] pFullPathName 设带绝对路径和JPG后缀名的文件名字符串
        //        *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
        //        *  @return 0表示成功，-1表示失败
        //        *  @note   使用的文件名中的路径需要存在
        //        *  @ingroup group_device
        //*/
        //        [DllImport("VzLPRSDK.dll")]
        //        public static extern int VzLPRClient_GetSnapShootToJpeg2(int nPlayHandle, string pFullPathName, int nQuality);
        /**
        *  @brief 获取主码流分辨率；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] sizeval 详见VZDEV_FRAMESIZE_宏定义
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoFrameSizeIndex(int handle, ref int sizeval);

        /**
        *  @brief 设置主码流分辨率；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] sizeval 详见VZDEV_FRAMESIZE_宏定义
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoFrameSizeIndex(int handle, int sizeval);

        /**
        *  @brief 获取主码流帧率
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] Rateval 帧率，范围1-25
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoFrameRate(int handle, ref int Rateval);//1-25

        /**
        *  @brief 设置主码流帧率；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] Rateval 帧率，范围1-25
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoFrameRate(int handle, int Rateval);//1-25

        /**
        *  @brief 获取主码流压缩模式；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] modeval 详见VZDEV_VIDEO_COMPRESS_宏定义
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoCompressMode(int handle, ref int modeval);//VZDEV_VIDEO_COMPRESS_XXX

        /**
        *  @brief 设置主码流压缩模式；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] modeval 详见VZDEV_VIDEO_COMPRESS_宏定义
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoCompressMode(int handle, int modeval);//VZDEV_VIDEO_COMPRESS_XXX


        /**
        *  @brief 获取主码流比特率；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] rateval 当前视频比特率
        *  @param [OUT] ratelist 暂时不用
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoCBR(int handle, ref int rateval/*Kbps*/, ref int ratelist);

        /**
        *  @brief 设置主码流比特率；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] rateval 当前视频比特率
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoCBR(int handle, int rateval/*Kbps*/);


        /**
        *  @brief 获取视频参数；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] brt 亮度
        *  @param [OUT] cst 对比度
        *  @param [OUT] sat 饱和度
        *  @param [OUT] hue 色度
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoPara(int handle, ref int brt, ref int cst, ref int sat, ref int hue);

        /**
        *  @brief 设置视频参数；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] brt 亮度
        *  @param [IN] cst 对比度
        *  @param [IN] sat 饱和度
        *  @param [IN] hue 色度
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoPara(int handle, int brt, int cst, int sat, int hue);

        /**
        *  @brief 设置通道主码流编码方式
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [OUT] cmd    返回的编码方式, 0->H264  1->MPEG4  2->JPEG  其他->错误
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoEncodeType(int handle, int cmd);

        /**
        *  @brief 获取视频的编码方式
        *  @param  [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [OUT] pEncType	返回的编码方式, 0:H264  1:MPEG4  2:JPEG  其他:错误
        *  @return 返回值为0表示成功，返回-1表示失败
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoEncodeType(int handle, ref int pEncType);

        /**
        *  @brief 获取视频图像质量；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] levelval //0~6，6最好
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetVideoVBR(int handle, ref int levelval);

        /**
        *  @brief 设置视频图像质量；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] levelval //0~6，6最好
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetVideoVBR(int handle, int levelval);


        /**
        *  @brief 获取视频制式；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] frequency 0:MaxOrZero, 1: 50Hz, 2:60Hz
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetFrequency(int handle, ref int frequency);

        /**
        *  @brief 设置视频制式；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] frequency 0:MaxOrZero, 1: 50Hz, 2:60Hz
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetFrequency(int handle, int frequency);

        /**
        *  @brief 获取曝光时间；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] shutter 2:>0~8ms 停车场推荐, 3: 0~4ms, 4:0~2ms 卡口推荐
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetShutter(int handle, ref int shutter);

        /**
        *  @brief 设置曝光时间；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] shutter 2:>0~8ms 停车场推荐, 3: 0~4ms, 4:0~2ms 卡口推荐
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetShutter(int handle, int shutter);

        /**
        *  @brief 获取图像翻转；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] flip, 0: 原始图像, 1:上下翻转, 2:左右翻转, 3:中心翻转
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetFlip(int handle, ref int flip);

        /**
        *  @brief 设置图像翻转；
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param [IN] flip, 0: 原始图像, 1:上下翻转, 2:左右翻转, 3:中心翻转
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetFlip(int handle, int flip);

        /**
        *  @brief 修改网络参数
        *  @param  [IN] SL        设备序列号低位字节
        *  @param  [IN] SH		  设备序列号高位字节	
        *  @param [IN] strNewIP   新IP     格式如"192.168.3.109"
        *  @param [IN] strGateway 网关     格式如"192.168.3.1"
        *  @param [IN] strNetmask 子网掩码 格式如"255.255.255.0"
        *  @note 可以用来实现跨网段修改IP的功能
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_UpdateNetworkParam(uint SL, uint SH, string strNewIP, string strGateway, string strNetmask);


        /**
        *  @brief 获取设备序列号；
        *  @param [IN] ip ip统一使用字符串的形式传入
        *  @param [IN] port 使用和登录时相同的端口
        *  @param [OUT] SerHi 序列号高位
        *  @param [OUT] SerLo 序列号低位
        *  @return 返回值为0表示成功，返回其他值表示失败。
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetSerialNo(string ip, short port, ref int SerHi, ref int SerLo);


        /**
        *  @brief 开始实时图像数据流，用于实时获取图像数据
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @return 返回值为0表示成功，返回其他值表示失败。
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_StartRealPlayDecData(int handle);

        /**
        *  @brief 停止实时图像数据流
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @return 返回值为0表示成功，返回其他值表示失败。
        *  @ingroup group_device
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_StopRealPlayDecData(int handle);

        /**
        *  @brief 从解码流中获取JPEG图像，保存到指定内存
        *  @param  [IN] handle		由VzLPRClient_Open函数获得的句柄
        *  @param  [IN/OUT] pDstBuf JPEG数据的目的存储首地址
        *  @param  [IN] uSizeBuf JPEG数据地址的内存的最大尺寸；
        *  @param  [IN] nQuality JPEG压缩的质量，取值范围1~100；
        *  @return >0表示成功，即编码后的尺寸，-1表示失败，-2表示给定的压缩数据的内存尺寸不够大
        *  @ingroup group_global
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_GetJpegStreamFromRealPlayDec(int handle, IntPtr pDstBuf, uint uSizeBuf, int nQuality);


        /**
        *  @brief 设置是否输出实时结果
        *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
        *  @param  [IN] bOutput 是否输出
        *  @return 0表示成功，-1表示失败
        */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_SetIsOutputRealTimeResult(int handle, bool bOutput);

        /**
         *  @brief 调整设备镜头的变倍和聚焦
         *  @param [IN] handle 由VzLPRClient_Open函数获得的句柄
         *  @param [IN] eOPT 操作类型，详见定义VZ_LENS_OPT  
         */
        [DllImport("VzLPRSDK.dll")]
        public static extern int VzLPRClient_CtrlLens(int handle, VZ_LENS_OPT eOPT);
    }
}
