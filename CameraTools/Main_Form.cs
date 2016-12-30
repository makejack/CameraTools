using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class Main_Form : Form
    {
        #region 变量

        private delegate void DelegateThread();

        private HuoYanClientSdk.VZLPRC_PLATE_INFO_CALLBACK HuoYanPlateInfoCallBack;
        private HuoYanClientSdk.VZLPRC_FIND_DEVICE_CALLBACK HuoYanFindDeviceCallBack;
        private HuoYanClientSdk.VZLPRC_COMMON_NOTIFY_CALLBACK HuoYanCommonNotifyCallBack;

        private AnShiBaoClientSdk.IPCSDK_CALLBACK AnShiBaoPlateInfoCallback;

        private QianYiClientSdk.FNetFindDeviceCallback QianYiFindDeviceCallBack;
        private QianYiClientSdk.FGetImageCB QianYiGetImageCallBack;

        private List<CameraParameter> mSearchCamera;
        private List<ConnectionCamera> mConnectionCamera;

        private System.Timers.Timer HideRecordImg;
        private bool InitAnShiBao;
        private bool InitHuoYan;
        private bool InitQianYi;
        private int HuoYanFindDeviceHwnd;
        private PictureBox _selectedcamera;
        private PictureBox SelectedCamera
        {
            get { return _selectedcamera; }
            set
            {
                if (_selectedcamera != value)
                {
                    DarwInvalidate();
                }
                _selectedcamera = value;
                DarwInvalidate();
            }
        }
        private long Number;

        #region 安视宝绘制虚拟线圈参数

        private bool AnShiBaoIsSetRoi;
        private bool IsDrawRoi;
        private IntPtr AnShiBaoCameraPara;
        private string AnShiBaoCameraIp;
        private float m_vRate;
        private float m_hRate;
        private float m_nX = 1920f;
        private float m_nY = 1080f;
        private float m_ROIhRate = 704f;
        private float m_ROIvRate = 576f;
        private AnShiBaoClientSdk.ROI_RECT_TAG RoiRect;

        #endregion 安视宝绘制虚拟线圈参数

        #endregion 变量

        /// <summary>
        /// 构造函数
        /// </summary>
        public Main_Form()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Form_Load(object sender, EventArgs e)
        {
            mConnectionCamera = new List<ConnectionCamera>();

            //设置识别回调函数
            HuoYanPlateInfoCallBack = PlateInfo;
            //设置搜索相机回调函数
            HuoYanFindDeviceCallBack = FindDevice;
            //设备连接反馈结果相关的回调函数
            HuoYanCommonNotifyCallBack = CommonNotify;

            //火眼全局初始化
            int result = HuoYanClientSdk.VzLPRClient_Setup();
            if (result == 0)
            {
                HuoYanClientSdk.VZLPRClient_SetCommonNotifyCallBack(HuoYanCommonNotifyCallBack, IntPtr.Zero);

                InitHuoYan = true;
            }

            //设置识别回调函数
            AnShiBaoPlateInfoCallback = PlateInfo;

            //安视定全局初始化
            result = AnShiBaoClientSdk.IPCSDK_Init(8190);
            if (result == 0)
            {
                AnShiBaoClientSdk.IPCSDK_Register_Callback(AnShiBaoPlateInfoCallback);
                InitAnShiBao = true;
            }

            //设置搜索相机回调函数
            QianYiFindDeviceCallBack = FindDevice;

            //设置获取图片信息回调
            QianYiGetImageCallBack = GetImage;

            //芊熠全局初始化
            result = QianYiClientSdk.Net_Init();
            if (result == 0)
            {
                QianYiClientSdk.Net_RegImageRecv(QianYiGetImageCallBack);
                InitQianYi = true;
            }

        }

        /// <summary>
        /// 通过注册回调获取图片信息
        /// </summary>
        /// <param name="tHandle"></param>
        /// <param name="uiImageId"></param>
        /// <param name="tImageInfo"></param>
        /// <param name="tPicInfo"></param>
        /// <returns></returns>
        private int GetImage(int tHandle, uint uiImageId, ref QianYiClientSdk.T_ImageUserInfo tImageInfo, ref QianYiClientSdk.T_PicInfo tPicInfo)
        {
            //车辆图像
            if (tImageInfo.ucViolateCode == 0)
            {
                string plate = System.Text.Encoding.Default.GetString(tImageInfo.szLprResult).Replace("\0", "");
                string cartype = "未知类型";
                switch (tImageInfo.ucVehicleSize)//车型
                {
                    case 1:
                        {
                            cartype = "大型车";
                            break;
                        }
                    case 2:
                        {
                            cartype = "中型车";
                            break;
                        }
                    case 3:
                        {
                            cartype = "小型车";
                            break;
                        }
                    case 4:
                        {
                            cartype = "摩托车";
                            break;
                        }
                    case 5:
                        {
                            cartype = "行人";
                            break;
                        }
                    default:
                        {
                            cartype = "未知车型";
                            break;
                        }
                }
                string color = "未识别";
                switch (tImageInfo.ucPlateColor)//车牌颜色
                {
                    case 0:
                        color = "蓝色";
                        break;
                    case 1:
                        color = "黄色";
                        break;
                    case 2:
                        color = "白色";
                        break;
                    case 3:
                        color = "黑色";
                        break;
                    case 4:
                    default:
                        color = "未识别";
                        break;
                }

                string fullpath = string.Empty;
                string platepath = string.Empty;
                DateTime now = GetImgSavePath(plate, ref fullpath, ref platepath);

                FileStream fs;
                if (tPicInfo.ptPanoramaPicBuff != IntPtr.Zero && tPicInfo.uiPanoramaPicLen != 0)
                {
                    byte[] BytePanoramaPicBuff = new byte[tPicInfo.uiPanoramaPicLen];
                    Marshal.Copy(tPicInfo.ptPanoramaPicBuff, BytePanoramaPicBuff, 0, (int)tPicInfo.uiPanoramaPicLen);
                    fs = new FileStream(fullpath, FileMode.Create, FileAccess.Write | FileAccess.Read, FileShare.None);
                    fs.Write(BytePanoramaPicBuff, 0, (int)tPicInfo.uiPanoramaPicLen);
                    fs.Close();
                    fs.Dispose();
                }

                if (tPicInfo.ptVehiclePicBuff != IntPtr.Zero && tPicInfo.uiVehiclePicLen != 0)
                {
                    byte[] ByteVehiclePicBuff = new byte[tPicInfo.uiVehiclePicLen];
                    Marshal.Copy(tPicInfo.ptVehiclePicBuff, ByteVehiclePicBuff, 0, (int)tPicInfo.uiVehiclePicLen);
                    fs = new FileStream(platepath, FileMode.Create, FileAccess.Write | FileAccess.Read, FileShare.None);
                    fs.Write(ByteVehiclePicBuff, 0, (int)tPicInfo.uiVehiclePicLen);
                    fs.Close();
                    fs.Dispose();
                }

                ShowPlateInfo(tHandle, platepath, plate, now, cartype, color);
            }
            return 0;
        }

        /// <summary>
        /// 窗体关闭前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                foreach (ConnectionCamera item in mConnectionCamera)
                {
                    switch (item.CameraType)
                    {
                        case CameraTypes.AnShiBao:
                            AnShiBaoClientSdk.IPCSDK_Stop_Stream(item.IP);
                            break;
                        case CameraTypes.HuoYan:
                            //停止播放指定的播放句柄
                            int result = HuoYanClientSdk.VzLPRClient_StopRealPlay(item.ShowHwnd);
                            if (result == 0)
                            {
                                //关闭回调
                                HuoYanClientSdk.VzLPRClient_SetPlateInfoCallBack(item.OpenHwnd, null, IntPtr.Zero, 1);
                                //关闭一个设备
                                HuoYanClientSdk.VzLPRClient_Close(item.OpenHwnd);
                            }
                            break;
                        case CameraTypes.QianYi:

                            QianYiClientSdk.Net_StopVideo(item.OpenHwnd);
                            QianYiClientSdk.Net_DisConnCamera(item.OpenHwnd);
                            QianYiClientSdk.Net_DelCamera(item.OpenHwnd);
                            break;
                    }
                }

                //全局释放
                if (InitHuoYan)
                    HuoYanClientSdk.VzLPRClient_Cleanup();
                if (InitAnShiBao)
                    AnShiBaoClientSdk.IPCSDK_UnInit();
                if (InitQianYi)
                    QianYiClientSdk.Net_UNinit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 设置设备连接反馈结果相关的回调事件
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="pUserData"></param>
        /// <param name="eNotify"></param>
        /// <param name="pStrDetail"></param>
        private void CommonNotify(int handle, IntPtr pUserData, HuoYanClientSdk.VZ_LPRC_COMMON_NOTIFY eNotify, string pStrDetail)
        {
            switch (eNotify)
            {
                case HuoYanClientSdk.VZ_LPRC_COMMON_NOTIFY.VZ_LPRC_NO_ERR:
                    break;
                case HuoYanClientSdk.VZ_LPRC_COMMON_NOTIFY.VZ_LPRC_ACCESS_DENIED:
                    MessageBox.Show("用户名密码错误" + pStrDetail);
                    break;
                case HuoYanClientSdk.VZ_LPRC_COMMON_NOTIFY.VZ_LPRC_NETWORK_ERR:
                    MessageBox.Show("网络连接故障" + pStrDetail);
                    break;
            }
        }

        /// <summary>
        /// 搜索相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SearchCamera_Click(object sender, EventArgs e)
        {
            btn_SearchCamera.Enabled = false;
            if (mSearchCamera == null)
                mSearchCamera = new List<CameraParameter>();

            try
            {
                if (InitHuoYan && HuoYanFindDeviceHwnd == 0)
                    //开始查找设备  
                    HuoYanFindDeviceHwnd = HuoYanClientSdk.VZLPRClient_StartFindDevice(HuoYanFindDeviceCallBack, IntPtr.Zero);
                if (InitAnShiBao)
                {
                    FindDevice();
                }

                if (InitQianYi)
                    //开始查找设备
                    QianYiClientSdk.Net_FindDevice(FindDevice, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btn_SearchCamera.Enabled = true;
        }

        /// <summary>
        /// 通过该函数获取找到的设备基本信息
        /// </summary>
        private void FindDevice()
        {
            if (mSearchCamera == null)
                mSearchCamera = new List<CameraParameter>();
            int cameranum = 0;
            int ipsize = Marshal.SizeOf(typeof(AnShiBaoClientSdk.CAMERA_IP_TAG));
            IntPtr iplist = Marshal.AllocHGlobal(Marshal.SizeOf(ipsize * 128));
            try
            {
                int ret = AnShiBaoClientSdk.IPCSDK_Find_Camera(ref cameranum, iplist);
                if (ret == 0)
                {
                    for (int i = 0; i < cameranum; i++)
                    {
                        int currenthwnd = iplist.ToInt32() + i * ipsize;
                        AnShiBaoClientSdk.CAMERA_IP_TAG findcamera = (AnShiBaoClientSdk.CAMERA_IP_TAG)Marshal.PtrToStructure((IntPtr)currenthwnd, typeof(AnShiBaoClientSdk.CAMERA_IP_TAG));
                        CameraParameter newcamera = new CameraParameter()
                        {
                            CameraType = CameraTypes.AnShiBao,
                            pStrIPAddr = findcamera.ip,
                            uPort1 = findcamera.port
                        };

                        if (!CameraIsExist(newcamera))
                        {
                            ShowFindDevice(newcamera);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Marshal.FreeHGlobal(iplist);
            }
        }

        /// <summary>
        /// 识别像机是否存在
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private bool CameraIsExist(CameraParameter para)
        {
            foreach (CameraParameter item in mSearchCamera)
            {
                if (item.pStrIPAddr == para.pStrIPAddr && item.CameraType == para.CameraType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过回调函数搜索局域网内相机的ip等信息,若要调用该函数，电脑需装有winshark抓包工具，提供必要的库支持
        /// </summary>
        /// <param name="ptFindDevice">相机搜索信息结构体</param>
        /// <param name="obj">回调函数上下文</param>
        /// <returns></returns>
        private int FindDevice(ref QianYiClientSdk.T_RcvMsg ptFindDevice, IntPtr obj)
        {
            CameraParameter param = new CameraParameter()
            {
                CameraType = CameraTypes.QianYi,
                pStrIPAddr = QianYiClientSdk.IntToIp(QianYiClientSdk.Reverse_uint(ptFindDevice.tNetSetup.uiIPAddress)),
                uPort1 = 30000
            };

            if (!CameraIsExist(param))
            {
                ShowFindDevice(param);
            }

            return 0;
        }

        /// <summary>
        /// 通过该回调函数获得找到的设备基本信息 
        /// </summary>
        /// <param name="pStrDevName">设备名称 </param>
        /// <param name="pStrIPAddr">设备IP地址 </param>
        /// <param name="uPort1">设备端口号 </param>
        /// <param name="usPort2">预留 </param>
        /// <param name="SL">设备序列号低位字节 </param>
        /// <param name="SH">设备序列号高位字节 </param>
        /// <param name="pUserData">回调函数上下文 </param>
        private void FindDevice(string pStrDevName, string pStrIPAddr, ushort uPort1, ushort usPort2, uint SL, uint SH, IntPtr pUserData)
        {
            try
            {
                CameraParameter param = new CameraParameter()
                {
                    CameraType = CameraTypes.HuoYan,
                    pStrDevName = pStrDevName,
                    pStrIPAddr = pStrIPAddr,
                    uPort1 = uPort1,
                    usPort2 = usPort2,
                    SL = SL,
                    SH = SH,
                    pUserData = pUserData
                };

                ShowFindDevice(param);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示找到的设备信息
        /// </summary>
        /// <param name="param"></param>
        private void ShowFindDevice(CameraParameter param)
        {
            DelegateThread ShowDeviceDelegate = delegate()
            {
                mSearchCamera.Add(param);

                TreeNode node = new TreeNode(param.pStrIPAddr + ":" + param.uPort1);
                node.Nodes.Add("CameraBrand", param.CameraType.ToString());
                node.Nodes.Add("DevName", param.pStrDevName);
                node.Nodes.Add("IP", param.pStrIPAddr);
                node.Nodes.Add("Port", param.uPort1.ToString());
                node.Nodes.Add("SL", param.SL.ToString());
                node.Nodes.Add("SH", param.SH.ToString());
                tv_CameraList.Nodes.Add(node);
                if (tv_CameraList.SelectedNode == null)
                    btn_Open.Enabled = true;
            };
            tv_CameraList.BeginInvoke(ShowDeviceDelegate);
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Open_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = tv_CameraList.SelectedNode ?? tv_CameraList.Nodes[0];
                CameraParameter cameraparam = mSearchCamera[node.Index];
                if (IsConnection(cameraparam.pStrIPAddr))
                {
                    MessageBox.Show("   IP地址" + cameraparam.pStrIPAddr + "已经连接，请重新选择。   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PictureBox pb = CreateCameraShowControl(node.Index);
                ConnectionCamera connectionparam = new ConnectionCamera()
                {
                    CameraType = cameraparam.CameraType,
                    IP = cameraparam.pStrIPAddr,
                    SH = cameraparam.SH,
                    SL = cameraparam.SL,
                    Port = cameraparam.uPort1,
                    Index = node.Index,
                };

                switch (cameraparam.CameraType)
                {
                    case CameraTypes.AnShiBao:
                        int ret = AnShiBaoClientSdk.IPCSDK_Start_Stream(this.Handle, pb.Handle, cameraparam.pStrIPAddr, 0);
                        if (ret == 0)
                        {
                            OpenCameraShowControls(node, pb, connectionparam);
                        }
                        else
                        {
                            pb.Dispose();
                            pb = null;
                        }
                        break;
                    case CameraTypes.HuoYan:
                        //打开一个设备
                        int m_hLPRClient = HuoYanClientSdk.VzLPRClient_Open(cameraparam.pStrIPAddr, cameraparam.uPort1, "admin", "admin");
                        if (m_hLPRClient == 0)
                        {
                            MessageBox.Show("打开失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pb.Dispose();
                            pb = null;
                            return;
                        }
                        connectionparam.OpenHwnd = m_hLPRClient;
                        //播放实时视频 
                        int m_hPlay = HuoYanClientSdk.VzLPRClient_StartRealPlay(m_hLPRClient, pb.Handle);
                        if (m_hPlay > -1)
                        {
                            //设置识别结果的回调函数 
                            HuoYanClientSdk.VzLPRClient_SetPlateInfoCallBack(m_hLPRClient, HuoYanPlateInfoCallBack, IntPtr.Zero, 1);

                            connectionparam.ShowHwnd = m_hPlay;

                            OpenCameraShowControls(node, pb, connectionparam);
                        }
                        else
                        {
                            pb.Dispose();
                            pb = null;
                        }
                        break;
                    case CameraTypes.QianYi:

                        int nCamId = QianYiClientSdk.Net_AddCamera(cameraparam.pStrIPAddr);
                        if (nCamId != 0)
                        {
                            MessageBox.Show("添加相机失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pb.Dispose();
                            pb = null;
                            return;
                        }
                        connectionparam.OpenHwnd = nCamId;

                        int iRet = QianYiClientSdk.Net_ConnCamera(nCamId, 0, 10);
                        if (iRet != 0)
                        {
                            QianYiClientSdk.Net_DelCamera(nCamId);
                            MessageBox.Show("连接相机失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pb.Dispose();
                            pb = null;
                            return;
                        }

                        iRet = QianYiClientSdk.Net_StartVideo(nCamId, 0, pb.Handle);
                        if (iRet != 0)
                        {
                            QianYiClientSdk.Net_DisConnCamera(nCamId);
                            QianYiClientSdk.Net_DelCamera(nCamId);
                            MessageBox.Show("打开视频失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            pb.Dispose();
                            pb = null;
                            return;
                        }
                        OpenCameraShowControls(node, pb, connectionparam);
                        break;
                    default:
                        pb.Dispose();
                        pb = null;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 打开相机后显示控件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        private void OpenCameraShowControls(TreeNode node, PictureBox pb, ConnectionCamera parameter)
        {
            node.Tag = fp_Left.Controls.Count;
            pb.Tag = parameter;
            fp_Left.Controls.Add(pb);
            mConnectionCamera.Add(parameter);
            ConnectionState(node, 1);
            SelectedCamera = pb;
            btn_Close.Enabled = true;
            btn_Close.Focus();
            btn_Open.Enabled = false;
            btn_RuleCfg.Enabled = true;
            btn_ParamSet.Enabled = true;
            btn_ModifyIp.Enabled = true;
        }

        /// <summary>
        /// 连接时显示的状态
        /// </summary>
        /// <param name="index"></param>
        private void ConnectionState(TreeNode node, int index)
        {
            node.ImageIndex = index;
            node.SelectedImageIndex = index;
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    ConnectionState(item, index);
                }
            }
        }

        /// <summary>
        /// 是否连接
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool IsConnection(string ip)
        {
            foreach (ConnectionCamera item in mConnectionCamera)
            {
                if (ip == item.IP)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 添加相机显示的画面
        /// </summary>
        private PictureBox CreateCameraShowControl(int index)
        {
            PictureBox pb = new PictureBox();
            pb.Name = "pb" + index;
            pb.Size = new Size(384, 216);
            pb.Visible = GetIsMaxShowVodio();
            pb.Margin = new Padding(pb.Margin.Left, pb.Margin.Top, pb.Margin.Right, 30);
            pb.DoubleClick += pb_DoubleClick;
            pb.Click += pb_Click;
            pb.Move += pb_Move;
            return pb;
        }

        /// <summary>
        /// 控件移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pb_Move(object sender, EventArgs e)
        {
            fp_Left.Refresh();
        }

        /// <summary>
        /// 获取是否最大化显示图视频
        /// </summary>
        /// <returns></returns>
        private bool GetIsMaxShowVodio()
        {
            foreach (Control item in fp_Left.Controls)
            {
                if (item is PictureBox)
                {
                    if (item.Width != 384 && item.Height != 216)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                object obj = pb.Tag;
                if (obj != null)
                {
                    ConnectionCamera currentcamera = obj as ConnectionCamera;
                    if (currentcamera != null)
                    {
                        tv_CameraList.SelectedNode = tv_CameraList.Nodes[currentcamera.Index];
                    }
                }
            }
        }

        /// <summary>
        /// 相机显示的画面双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pb_DoubleClick(object sender, EventArgs e)
        {
            if (!AnShiBaoIsSetRoi)
            {
                PictureBox pb = sender as PictureBox;
                foreach (PictureBox item in fp_Left.Controls)
                {
                    if (item == pb) continue;
                    item.Visible = !item.Visible;
                }

                if (pb.Width != 384 && pb.Height != 216)
                {
                    pb.Size = new Size(384, 216);
                    fp_Left.AutoScroll = true;
                    Text = "识别像机配置工具";
                }
                else
                {
                    pb.Width = fp_Left.Width - 8;
                    pb.Height = fp_Left.Height - 8;
                    fp_Left.AutoScroll = false;
                    ConnectionCamera param = pb.Tag as ConnectionCamera;
                    Text = param.IP;
                }
                fp_Left.Refresh();
            }
        }

        /// <summary>
        /// 通过该回调函数获得车牌识别信息 
        /// </summary>
        /// <param name="handle">由VzLPRClient_Open函数获得的句柄 </param>
        /// <param name="pUserData">回调函数的上下文 </param>
        /// <param name="pResult">车牌信息数组首地址，详见结构体定义 TH_PlateResult </param>
        /// <param name="uNumPlates">车牌数组中的车牌个数 </param>
        /// <param name="eResultType">车牌识别结果类型，详见枚举类型定义VZ_LPRC_RESULT_TYPE </param>
        /// <param name="pImgFull">当前帧的图像内容，详见结构体定义VZ_LPRC_IMAGE_INFO </param>
        /// <param name="pImgPlateClip">当前帧中车牌区域的图像内容数组，其中的元素与车牌信息数组中的对应 </param>
        private int PlateInfo(int handle, IntPtr pUserData, IntPtr pResult, uint uNumPlates, HuoYanClientSdk.VZ_LPRC_RESULT_TYPE eResultType, IntPtr pImgFull, IntPtr pImgPlateClip)
        {
            try
            {
                if (eResultType == HuoYanClientSdk.VZ_LPRC_RESULT_TYPE.VZ_LPRC_RESULT_REALTIME) return 0;

                //获取车牌识别结果信息
                HuoYanClientSdk.TH_PlateResult plateresult = (HuoYanClientSdk.TH_PlateResult)Marshal.PtrToStructure(pResult, typeof(HuoYanClientSdk.TH_PlateResult));
                string strlicense = new string(plateresult.license).Replace("\0", "");
                string cartype = "未知车牌";
                switch (plateresult.nType)
                {
                    //车牌类型
                    case HuoYanClientSdk.LT_UNKNOWN:   //未知车牌
                        cartype = "未知车牌";
                        break;
                    case HuoYanClientSdk.LT_BLUE://蓝牌小汽车
                        cartype = "蓝牌小汽车";
                        break;
                    case HuoYanClientSdk.LT_BLACK:   //黑牌小汽车
                        cartype = "黑牌小汽车";
                        break;
                    case HuoYanClientSdk.LT_YELLOW://单排黄牌
                        cartype = "单排黄牌";
                        break;
                    case HuoYanClientSdk.LT_YELLOW2:   //双排黄牌（大车尾牌，农用车）
                        cartype = "又排黄牌";
                        break;
                    case HuoYanClientSdk.LT_POLICE:   //警车车牌
                        cartype = "警车车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMPOL:   //武警车牌
                        cartype = "武警车牌";
                        break;
                    case HuoYanClientSdk.LT_INDIVI://个性化车牌
                        cartype = "个性化车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMY:   //单排军车牌
                        cartype = "单排军车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMY2:   //双排军车牌
                        cartype = "双排军车牌";
                        break;
                    case HuoYanClientSdk.LT_EMBASSY:  //使馆车牌
                        cartype = "使馆车牌";
                        break;
                    case HuoYanClientSdk.LT_HONGKONG://香港进出中国大陆车牌
                        cartype = "香港进出中国大陆车牌";
                        break;
                    case HuoYanClientSdk.LT_TRACTOR:  //农用车牌
                        cartype = "农用车牌";
                        break;
                    case HuoYanClientSdk.LT_COACH://教练车牌
                        cartype = "教练车牌";
                        break;
                    case HuoYanClientSdk.LT_MACAO://澳门进出中国大陆车牌
                        cartype = "澳门进出中国大陆车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMPOL2://双层武警车牌
                        cartype = "双层武警车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMPOL_ZONGDUI:// 武警总队车牌
                        cartype = "武警总队车牌";
                        break;
                    case HuoYanClientSdk.LT_ARMPOL2_ZONGDUI: // 双层武警总队车牌
                        cartype = "双层武警总队车牌";
                        break;
                }
                string color = "未知";
                switch (plateresult.nColor)
                {
                    case 0:
                        color = "未知";
                        break;
                    case 1:
                        color = "蓝色";
                        break;
                    case 2:
                        color = "黄色";
                        break;
                    case 3:
                        color = "白色";
                        break;
                    case 4:
                        color = "黑色";
                        break;
                    case 5:
                        color = "绿色";
                        break;
                }
                string fullpath = string.Empty;
                string platepath = string.Empty;
                DateTime now = GetImgSavePath(strlicense, ref fullpath, ref platepath);
                if (!File.Exists(fullpath))
                    HuoYanClientSdk.VzLPRClient_ImageSaveToJpeg(pImgFull, fullpath, 100);
                if (!File.Exists(platepath))
                    HuoYanClientSdk.VzLPRClient_ImageSaveToJpeg(pImgPlateClip, platepath, 100);

                ShowPlateInfo(handle, platepath, strlicense, now, cartype, color);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }

        /// <summary>
        /// 显示识别到的车牌信息
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="platepath"></param>
        /// <param name="plate"></param>
        /// <param name="now"></param>
        /// <param name="platetype"></param>
        /// <param name="platecolor"></param>
        private void ShowPlateInfo(int handle, string platepath, string plate, DateTime now, string platetype, string platecolor)
        {
            string ip = string.Empty;
            foreach (ConnectionCamera item in mConnectionCamera)
            {
                if (item.OpenHwnd != handle) continue;
                ip = item.IP;
                break;
            }
            ShowPlateInfo(ip, platepath, plate, now, platetype, platecolor);
        }

        /// <summary>
        /// 通过该回调函数获得车牌识别信息 
        /// </summary>
        /// <param name="ip">发送相机IP地址</param>
        /// <param name="buf">相机发送来的数据指针</param>
        /// <param name="len">相机发送来的数据长度</param>
        /// <returns>正常处理返回值应该为0, 否则会弹出错误提示框并提醒用户回调函数返回值</returns>
        private int PlateInfo(string ip, IntPtr buff, int len)
        {
            /* 16KB 用于存储车牌信息足够了 */
            IntPtr plateresult = IntPtr.Zero;
            // 车牌特写图临时空间
            IntPtr platejpeg = IntPtr.Zero;
            //获取车牌号
            IntPtr license = IntPtr.Zero;
            //车牌颜色
            IntPtr color = IntPtr.Zero;
            try
            {
                plateresult = Marshal.AllocHGlobal(64 * 1024);
                platejpeg = Marshal.AllocHGlobal(32 * 1024);
                int resultlen = 0;
                int ret = AnShiBaoClientSdk.IPCSDK_Get_Plate_Info(buff, plateresult, ref resultlen);
                if (ret == 0)
                {
                    license = Marshal.AllocHGlobal(20);
                    ret = AnShiBaoClientSdk.IPCSDK_Get_Plate_License(plateresult, license);
                    if (ret == 0)
                    {
                        string plate = Marshal.PtrToStringAnsi(license);
                        string fullpath = string.Empty;
                        string platepath = string.Empty;
                        string strcolor = string.Empty;
                        DateTime now = GetImgSavePath(plate, ref fullpath, ref platepath);
                        FileStream fs;
                        byte[] by;
                        if (!File.Exists(fullpath))
                        {
                            using (fs = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                by = new byte[len];
                                Marshal.Copy(buff, by, 0, len);
                                fs.Write(by, 0, by.Length);
                                fs.Flush();
                            }
                        }
                        if (!File.Exists(platepath))
                        {
                            ret = AnShiBaoClientSdk.IPCSDK_Get_Plate_Jpeg(buff, platejpeg, ref resultlen);
                            if (ret == 0)
                            {
                                using (fs = new FileStream(platepath, FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    by = new byte[resultlen];
                                    Marshal.Copy(platejpeg, by, 0, by.Length);
                                    fs.Write(by, 0, by.Length);
                                    fs.Flush();
                                }
                            }
                        }
                        color = Marshal.AllocHGlobal(8);
                        ret = AnShiBaoClientSdk.IPCSDK_Get_Plate_Color(plateresult, color);
                        if (ret == 0)
                        {
                            strcolor = Marshal.PtrToStringAnsi(color);

                        }
                        ShowPlateInfo(ip, platepath, plate, now, "未知", strcolor);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (plateresult != IntPtr.Zero)
                    Marshal.FreeHGlobal(plateresult);
                if (platejpeg != IntPtr.Zero)
                    Marshal.FreeHGlobal(platejpeg);
                if (license != IntPtr.Zero)
                    Marshal.FreeHGlobal(license);
                if (color != IntPtr.Zero)
                    Marshal.FreeHGlobal(color);
            }
            return 0;
        }

        /// <summary>
        /// 显示识别到的车牌信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="platepath"></param>
        /// <param name="plate"></param>
        /// <param name="now"></param>
        /// <param name="platetype"></param>
        /// <param name="color"></param>
        private void ShowPlateInfo(string ip, string platepath, string plate, DateTime now, string platetype, string color)
        {
            DelegateThread ShowInfo = delegate
            {
                Number++;
                dgv_ResultList.Rows.Add(new object[] { Number, ip, Image.FromFile(platepath), plate, now, platetype, color });
                if (dgv_ResultList.RowCount > 50)
                {
                    dgv_ResultList.Rows.RemoveAt(0);
                }
            };
            dgv_ResultList.Invoke(ShowInfo);
        }

        /// <summary>
        /// 获取识别的照片保存地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="platepath"></param>
        private DateTime GetImgSavePath(string plate, ref string fullpath, ref string platepath)
        {
            DateTime now = DateTime.Now;
            GetImgSavePath(plate, now, ref fullpath, ref platepath);
            return now;
        }

        /// <summary>
        /// 获取识别的照片保存地址
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="platepath"></param>
        private void GetImgSavePath(string plate, DateTime now, ref string fullpath, ref string platepath)
        {
            string path = string.Format("{0}\\{1:yyyyMMdd}", Environment.CurrentDirectory, now);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = string.Format("{0}\\{1:yyyyMMddHHmmssffff}_{2}", path, now, plate);
            fullpath = path + ".jpg";
            platepath = path + "_plate.jpg";
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (SelectedCamera == null) return;
            ConnectionCamera param = SelectedCamera.Tag as ConnectionCamera;
            CloseCamera(param);
        }

        /// <summary>
        /// 关闭摄像机
        /// </summary>
        /// <param name="param"></param>
        private void CloseCamera(ConnectionCamera param)
        {
            try
            {
                if (param != null)
                {
                    switch (param.CameraType)
                    {
                        case CameraTypes.AnShiBao:
                            AnShiBaoClientSdk.IPCSDK_Stop_Stream(param.IP);

                            RemoveShowControls(SelectedCamera, param);
                            break;
                        case CameraTypes.HuoYan:
                            //停止播放指定的播放句柄
                            int result = HuoYanClientSdk.VzLPRClient_StopRealPlay(param.ShowHwnd);
                            if (result == 0)
                            {
                                //关闭回调
                                HuoYanClientSdk.VzLPRClient_SetPlateInfoCallBack(param.OpenHwnd, null, IntPtr.Zero, 1);
                                //关闭一个设备
                                HuoYanClientSdk.VzLPRClient_Close(param.OpenHwnd);

                                RemoveShowControls(SelectedCamera, param);
                            }
                            break;
                        case CameraTypes.QianYi:

                            QianYiClientSdk.Net_StopVideo(param.OpenHwnd);
                            QianYiClientSdk.Net_DisConnCamera(param.OpenHwnd);
                            QianYiClientSdk.Net_DelCamera(param.OpenHwnd);

                            RemoveShowControls(SelectedCamera, param);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 移除显示的控件
        /// </summary>
        private void RemoveShowControls(PictureBox pb, ConnectionCamera connectioncamera)
        {
            btn_Open.Enabled = true;
            tv_CameraList.Enabled = true;
            btn_Close.Enabled = false;
            btn_ParamSet.Enabled = false;
            btn_RuleCfg.Enabled = false;
            btn_ModifyIp.Enabled = false;
            ConnectionState(tv_CameraList.Nodes[connectioncamera.Index], 0);
            SelectedCamera = null;
            fp_Left.Controls.Remove(pb);
            mConnectionCamera.Remove(connectioncamera);
        }

        /// <summary>
        /// 控件修改大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fp_Left_Resize(object sender, EventArgs e)
        {
            foreach (Control item in fp_Left.Controls)
            {
                if (item.Width != 384 && item.Height != 216)
                {
                    item.Width = fp_Left.Width - 8;
                    item.Height = fp_Left.Height - 8;
                }
            }
        }

        /// <summary>
        /// 重绘控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fp_Left_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                if (SelectedCamera != null && SelectedCamera.Visible)
                {
                    if (SelectedCamera.Width == 384 && SelectedCamera.Height == 216)
                        g.DrawRectangle(new Pen(Color.Red, 1), SelectedCamera.Left - 2, SelectedCamera.Top - 2, SelectedCamera.Width + 4, SelectedCamera.Height + SelectedCamera.Margin.Bottom + 4);
                }
                foreach (Control item in fp_Left.Controls)
                {
                    ConnectionCamera cameraparam = item.Tag as ConnectionCamera;
                    if (cameraparam != null)
                    {
                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        g.DrawString(cameraparam.IP, Font, Brushes.Blue, new RectangleF(item.Left, item.Bottom, item.Width, item.Margin.Bottom), sf);
                    }
                }
            }
        }

        /// <summary>
        /// 更改选定的内容后发生的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_CameraList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node.Parent;
            node = node ?? e.Node;
            if (mSearchCamera.Count <= 0) return;
            if (string.IsNullOrEmpty(mSearchCamera[node.Index].pStrIPAddr))
            {
                btn_Open.Enabled = false;
                btn_Close.Enabled = true;
                btn_ModifyIp.Enabled = false;
                btn_RuleCfg.Enabled = false;
                btn_ParamSet.Enabled = false;
            }
            else
            {
                btn_Open.Enabled = node.ImageIndex < 1;
                btn_Close.Enabled = node.ImageIndex >= 1;
                btn_ModifyIp.Enabled = btn_Close.Enabled;
                btn_RuleCfg.Enabled = btn_Close.Enabled;
                btn_ParamSet.Enabled = btn_Close.Enabled;
            }
            if (node.ImageIndex >= 1)
            {
                object obj = node.Tag;
                if (obj != null)
                {
                    int index = Convert.ToInt32(obj);
                    Control[] findcontrol = fp_Left.Controls.Find("pb" + node.Index, false);
                    foreach (Control item in findcontrol)
                    {
                        if (item is PictureBox)
                        {
                            PictureBox pb = item as PictureBox;
                            if (pb != null)
                            {
                                SelectedCamera = pb;
                            }
                        }
                    }
                }
            }
            else
            {
                SelectedCamera = null;
            }
        }

        /// <summary>
        /// 列表添加行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_ResultList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgv_ResultList.FirstDisplayedScrollingRowIndex = dgv_ResultList.RowCount - 1;
        }

        /// <summary>
        /// 内容格式发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_ResultList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        /// <summary>
        /// 视频参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ParamSet_Click(object sender, EventArgs e)
        {
            if (SelectedCamera == null) return;
            ConnectionCamera cameraparam = SelectedCamera.Tag as ConnectionCamera;
            if (cameraparam != null)
            {
                switch (cameraparam.CameraType)
                {
                    case CameraTypes.AnShiBao:
                        using (AnShiBaoParamSet_Form asbps = new AnShiBaoParamSet_Form(cameraparam.IP))
                        {
                            asbps.ShowDialog();
                        }
                        break;
                    case CameraTypes.HuoYan:
                        using (HuoYanParamSet_Form hyps = new HuoYanParamSet_Form(cameraparam.OpenHwnd))
                        {
                            hyps.ShowDialog();
                        }
                        break;
                    case CameraTypes.QianYi:
                        using (QianYiParamSet_Form qyps = new QianYiParamSet_Form(cameraparam.OpenHwnd))
                        {
                            qyps.ShowDialog();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 线圈设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RuleCfg_Click(object sender, EventArgs e)
        {
            if (SelectedCamera == null) return;
            ConnectionCamera cameraparam = SelectedCamera.Tag as ConnectionCamera;
            if (cameraparam != null)
            {
                switch (cameraparam.CameraType)
                {
                    case CameraTypes.AnShiBao:
                        AnShiBaoSetRoi(cameraparam.IP);
                        break;
                    case CameraTypes.HuoYan:
                        using (HuoYanRuleCfg_Form rf = new HuoYanRuleCfg_Form(cameraparam.OpenHwnd))
                        {
                            rf.ShowDialog();
                        }
                        break;
                    case CameraTypes.QianYi:
                        using (QianYiRuleCfg_Form rf = new QianYiRuleCfg_Form(cameraparam.OpenHwnd))
                        {
                            rf.ShowDialog();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 安视宝设置虚拟线圈
        /// </summary>
        /// <param name="cameraip"></param>
        private void AnShiBaoSetRoi(string cameraip)
        {
            if (!AnShiBaoIsSetRoi)
            {
                if (SelectedCamera.Width == 384 && SelectedCamera.Height == 216)
                    pb_DoubleClick(SelectedCamera, null);
                m_ROIhRate /= m_nX;
                m_ROIvRate /= m_nY;
                AnShiBaoCameraPara = Marshal.AllocHGlobal(32768);
                int ret = AnShiBaoClientSdk.IPCSDK_Get_Camera_Para(cameraip, AnShiBaoCameraPara);
                if (ret != 0)
                {
                    Marshal.FreeHGlobal(AnShiBaoCameraPara);
                    AnShiBaoCameraPara = IntPtr.Zero;
                    MessageBox.Show("无法获取识别相机参数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AnShiBaoCameraIp = cameraip;
                btn_RuleCfg.Text = "保存设置";
                DrawRoi();
                SelectedCamera.MouseDown += AnShiBaoDrawRoiMouseDown;
                SelectedCamera.MouseMove += AnShiBaoDrawRoiMouseMove;
                SelectedCamera.MouseUp += AnshiBaoDrawRoiMouseUp;
            }
            else
            {
                btn_RuleCfg.Text = "线圈设置";
                SelectedCamera.MouseDown -= AnShiBaoDrawRoiMouseDown;
                SelectedCamera.MouseMove -= AnShiBaoDrawRoiMouseMove;
                SelectedCamera.MouseUp -= AnshiBaoDrawRoiMouseUp;
            }
            AnShiBaoIsSetRoi = !AnShiBaoIsSetRoi;

            btn_SearchCamera.Enabled = !btn_SearchCamera.Enabled;
            btn_ParamSet.Enabled = !btn_ParamSet.Enabled;
            tv_CameraList.Enabled = !tv_CameraList.Enabled;
            btn_ModifyIp.Enabled = !btn_ModifyIp.Enabled;
        }

        /// <summary>
        /// 绘制虚拟线圈鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AnshiBaoDrawRoiMouseUp(object sender, MouseEventArgs e)
        {
            if (!IsDrawRoi) return;
            IsDrawRoi = false;
            SetRoiRect(sender, e.X, e.Y);
            DrawRoi();
            int num = AnShiBaoClientSdk.IPCSDK_Set_Camera_Para(AnShiBaoCameraIp, AnShiBaoCameraPara);
            Marshal.FreeHGlobal(AnShiBaoCameraPara);
            AnShiBaoCameraPara = IntPtr.Zero;
            AnShiBaoSetRoi(AnShiBaoCameraIp);
            if (num != 0)
            {
                MessageBox.Show("请严格遵守左上角向右下角绘制识别区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /// <summary>
        /// 绘制虚拟线圈鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AnShiBaoDrawRoiMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDrawRoi) return;
            if (RoiRect.top > e.Y || RoiRect.left > e.X)
            {
                Marshal.FreeHGlobal(AnShiBaoCameraPara);
                AnShiBaoCameraPara = IntPtr.Zero;
                IsDrawRoi = false;
                AnShiBaoSetRoi(AnShiBaoCameraIp);
                MessageBox.Show("请严格遵守左上角向右下角绘制识别区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.SetRoiRect(sender, e.X, e.Y);
                this.DrawRoi();
            }
        }

        /// <summary>
        /// 绘制虚拟线圈鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AnShiBaoDrawRoiMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (AnShiBaoIsSetRoi)
                {
                    PictureBox pb = sender as PictureBox;
                    RoiRect = new AnShiBaoClientSdk.ROI_RECT_TAG();
                    RoiRect.left = e.X;
                    RoiRect.top = e.Y - pb.DisplayRectangle.Top;
                    m_hRate = (float)pb.Width / this.m_nX;
                    m_vRate = (float)pb.Height / this.m_nY;
                    DrawRoi();
                    IsDrawRoi = true;
                }
            }
        }

        /// <summary>
        /// 设置虚拟线圈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetRoiRect(object sender, int x, int y)
        {
            if (!(AnShiBaoCameraPara == IntPtr.Zero))
            {
                PictureBox pb = sender as PictureBox;
                RoiRect.right = x - pb.DisplayRectangle.Left;
                RoiRect.bottom = y - pb.DisplayRectangle.Top;
                AnShiBaoClientSdk.ROI_RECT_TAG rOI_RECT_TAG = new AnShiBaoClientSdk.ROI_RECT_TAG
                {
                    left = RoiRect.left,
                    top = RoiRect.top,
                    right = RoiRect.right,
                    bottom = RoiRect.bottom
                };
                rOI_RECT_TAG.left = (int)((float)rOI_RECT_TAG.left / this.m_hRate);
                rOI_RECT_TAG.top = (int)((float)rOI_RECT_TAG.top / this.m_vRate);
                rOI_RECT_TAG.right = (int)((float)rOI_RECT_TAG.right / this.m_hRate);
                rOI_RECT_TAG.bottom = (int)((float)rOI_RECT_TAG.bottom / this.m_vRate);
                if (rOI_RECT_TAG.left < 0)
                {
                    rOI_RECT_TAG.left = 0;
                }
                if (rOI_RECT_TAG.top < 0)
                {
                    rOI_RECT_TAG.top = 0;
                }
                if (rOI_RECT_TAG.right > 1920)
                {
                    rOI_RECT_TAG.right = 1920;
                }
                if (rOI_RECT_TAG.bottom > 1080)
                {
                    rOI_RECT_TAG.bottom = 1080;
                }
                AnShiBaoClientSdk.IPCSDK_Alg_Set_ROI(AnShiBaoCameraPara, 0, rOI_RECT_TAG.left, rOI_RECT_TAG.right, rOI_RECT_TAG.top, rOI_RECT_TAG.bottom);
            }
        }

        /// <summary>
        /// 绘制虚拟线圈
        /// </summary>
        private void DrawRoi()
        {
            if (AnShiBaoIsSetRoi && AnShiBaoCameraPara != IntPtr.Zero)
            {
                AnShiBaoClientSdk.ROI_RECT_TAG rOI_RECT_TAG = default(AnShiBaoClientSdk.ROI_RECT_TAG);
                for (int i = 0; i < 3; i++)
                {
                    AnShiBaoClientSdk.IPCSDK_Alg_Get_ROI(AnShiBaoCameraPara, i, ref rOI_RECT_TAG.left, ref rOI_RECT_TAG.right, ref rOI_RECT_TAG.top, ref rOI_RECT_TAG.bottom);
                    if (rOI_RECT_TAG.right == 0 || rOI_RECT_TAG.bottom == 0)
                    {
                        break;
                    }
                    AnShiBaoClientSdk.IPCSDK_Draw_Rect_On_Stream(AnShiBaoCameraIp, rOI_RECT_TAG.left, rOI_RECT_TAG.right, rOI_RECT_TAG.top, rOI_RECT_TAG.bottom, 1);
                }
            }
        }

        /// <summary>
        /// 修改摄像IP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyIp_Click(object sender, EventArgs e)
        {
            if (SelectedCamera == null) return;
            ConnectionCamera cameraparam = SelectedCamera.Tag as ConnectionCamera;
            if (cameraparam == null) return;
            switch (cameraparam.CameraType)
            {
                case CameraTypes.AnShiBao:
                    string processpath = Environment.CurrentDirectory + @"\DeviceSearch.exe";
                    Process[] ps = Process.GetProcessesByName("DeviceSearch");
                    if (ps.Length > 0)
                    {
                        return;
                    }
                    Process process = new Process();
                    process.StartInfo.FileName = processpath;
                    process.Start();
                    break;
                case CameraTypes.QianYi:
                case CameraTypes.HuoYan:
                    if (cameraparam.CameraType == CameraTypes.HuoYan)
                    {
                        using (HuoYanNetCfg_Form nf = new HuoYanNetCfg_Form(cameraparam.IP, cameraparam.SL, cameraparam.SH))
                        {
                            if (nf.ShowDialog() != DialogResult.OK)
                                return;
                        }
                    }
                    else if (cameraparam.CameraType == CameraTypes.QianYi)
                    {
                        using (QianYiModifyIP mi = new QianYiModifyIP()
                        {
                            strIp = cameraparam.IP,
                            nCamId = cameraparam.OpenHwnd
                        })
                        {
                            if (mi.ShowDialog() != DialogResult.OK)
                                return;
                        }
                    }
                    foreach (CameraParameter item in mSearchCamera)
                    {
                        if (item.pStrIPAddr == cameraparam.IP && item.CameraType == cameraparam.CameraType)
                        {
                            mSearchCamera.Remove(item);
                            break;
                        }
                    }

                    TreeNode node = tv_CameraList.Nodes[cameraparam.Index];
                    CloseCamera(cameraparam);
                    tv_CameraList.Nodes.Remove(node);
                    break;
            }
        }

        /// <summary>
        /// 列表单元格单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_ResultList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1) return;

            DateTime time = Convert.ToDateTime(dgv_ResultList.Rows[e.RowIndex].Cells["c_Time"].Value);
            string plate = dgv_ResultList.Rows[e.RowIndex].Cells["c_StrPlate"].Value.ToString();
            string fullpath = string.Empty;
            string platepath = string.Empty;
            GetImgSavePath(plate, time, ref fullpath, ref platepath);
            if (!File.Exists(fullpath)) return;
            Control[] findcontrol = Controls.Find("pb_ShowRecordImg", false);
            if (findcontrol.Length > 0)
            {
                foreach (Control item in findcontrol)
                {
                    if (item is PictureBox)
                    {
                        PictureBox pb = item as PictureBox;
                        pb.Image = Image.FromFile(fullpath);
                        pb.Location = GetImgShowPoint(pb.Size);
                        break;
                    }
                }
            }
            else
            {
                PictureBox pb = new PictureBox();
                pb.Name = "pb_ShowRecordImg";
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Image = Image.FromFile(fullpath);
                pb.Size = new Size(384, 216);
                pb.Location = GetImgShowPoint(pb.Size);
                Controls.Add(pb);
                pb.Show();
                pb.BringToFront();
            }

            if (HideRecordImg == null)
            {
                HideRecordImg = new System.Timers.Timer(5000);
                HideRecordImg.AutoReset = false;
                HideRecordImg.Elapsed += HideRecordImg_Elapsed;
            }
            HideRecordImg.Start();
        }

        /// <summary>
        /// 获取PictureBox控件显示的位置
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private Point GetImgShowPoint(Size size)
        {
            Point p = PointToClient(MousePosition);
            p.Y -= size.Height;
            if (p.Y < 0)
            {
                p.Y = 0;
            }
            if (p.X + size.Width > Width)
                p.X = Width - size.Width;
            return p;
        }

        /// <summary>
        /// 隐藏PictureBox控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HideRecordImg_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Control[] findcontrol = Controls.Find("pb_ShowRecordImg", false);
            foreach (Control item in findcontrol)
            {
                if (item is PictureBox)
                {
                    PictureBox pb = item as PictureBox;
                    Controls.Remove(pb);
                    pb.Dispose();
                    pb = null;
                }
            }
            System.Timers.Timer currenttimer = sender as System.Timers.Timer;
            currenttimer.Stop();
            currenttimer.Dispose();
            currenttimer = null;
        }

        /// <summary>
        /// 重新绘制选择的区域
        /// </summary>
        private void DarwInvalidate()
        {
            if (_selectedcamera != null && _selectedcamera.Visible)
            {
                fp_Left.Invalidate(new Rectangle(_selectedcamera.Left - 2, _selectedcamera.Top - 2, _selectedcamera.Width + 5, _selectedcamera.Height + 35));
            }
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void p_Right_Paint(object sender, PaintEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                g.DrawLine(new Pen(Brushes.Black, 1), 0, 0, 0, p_Right.Height);
            }
        }
    }
}