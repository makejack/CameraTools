using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class Vodio_Form : Form
    {
        private bool IsMove;
        private int OpenHwnd;
        private HuoYanClientSdk.VZ_LPRC_OSD_Param OsdParam;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="openhwnd"></param>
        public Vodio_Form(int openhwnd)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.OpenHwnd = openhwnd;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraVodio_Load(object sender, EventArgs e)
        {
            LoadVideoCfg();

            LoadVdeoSource();

            LoadOSDParam();
        }

        /// <summary>
        /// 加载视频源
        /// </summary>
        private void LoadVdeoSource()
        {
            int brt = 0, cst = 0, sat = 0, hue = 0, frequency = 0, shutter = 0, flip = 0;

            //获取 亮度 对比度 饱和度 清晰度
            int result = HuoYanClientSdk.VzLPRClient_GetVideoPara(OpenHwnd, ref brt, ref cst, ref sat, ref hue);
            if (result == 0)
            {
                tb_bright.Value = brt;
                tb_contrast.Value = cst;
                tb_saturation.Value = sat;
                tb_definition.Value = hue;
            }

            //视频制式
            result = HuoYanClientSdk.VzLPRClient_GetFrequency(OpenHwnd, ref frequency);
            if (result == 0)
            {
                cb_videostandrad.SelectedIndex = frequency;
            }

            //曝光时间
            result = HuoYanClientSdk.VzLPRClient_GetShutter(OpenHwnd, ref shutter);
            if (result == 0)
            {
                cb_exposuretime.SelectedIndex = shutter;
            }

            //图像翻转
            result = HuoYanClientSdk.VzLPRClient_GetFlip(OpenHwnd, ref flip);
            if (result == 0)
            {
                cb_imgpos.SelectedIndex = flip;
            }
        }

        /// <summary>
        /// 加载主流码参数
        /// </summary>
        private void LoadVideoCfg()
        {
            int sizeval = 0, rateval = 0, ratelist = 0, penctype = 0, modeval = 0, levelval = 0;

            //分辨率
            int result = HuoYanClientSdk.VzLPRClient_GetVideoFrameSizeIndex(OpenHwnd, ref sizeval);
            if (result == 0)
            {
                cb_framesize.SelectedIndex = sizeval;
            }

            //帧率
            result = HuoYanClientSdk.VzLPRClient_GetVideoFrameRate(OpenHwnd, ref rateval);
            if (result == 0 && rateval > 0)
            {
                cb_framerate.SelectedIndex = rateval - 1;
            }

            //编码方式
            result = HuoYanClientSdk.VzLPRClient_GetVideoEncodeType(OpenHwnd, ref penctype);
            if (result == 0)
                if (penctype == 0)
                {
                    cb_VideoEncodeType.SelectedIndex = 0;
                    cb_compressmode.Enabled = true;
                }
                else
                {
                    cb_VideoEncodeType.SelectedIndex = 1;
                    cb_compressmode.Enabled = false;
                }

            //流码控制
            result = HuoYanClientSdk.VzLPRClient_GetVideoCompressMode(OpenHwnd, ref modeval);
            if (result == 0)
            {
                cb_compressmode.SelectedIndex = modeval;
                ud_rateval.Enabled = modeval == 0;
            }

            //图像质量
            result = HuoYanClientSdk.VzLPRClient_GetVideoVBR(OpenHwnd, ref levelval);
            if (result == 0)
            {
                cb_imgquality.SelectedIndex = levelval;
            }

            //流码上限
            result = HuoYanClientSdk.VzLPRClient_GetVideoCBR(OpenHwnd, ref rateval, ref ratelist);
            if (result == 0)
            {
                ud_rateval.Value = ratelist;
            }
        }

        /// <summary>
        /// 加载OSD参数
        /// </summary>
        private void LoadOSDParam()
        {
            //获取OSD参数
            OsdParam = new HuoYanClientSdk.VZ_LPRC_OSD_Param();
            IntPtr hosdparam = Marshal.AllocHGlobal(Marshal.SizeOf(OsdParam));
            Marshal.StructureToPtr(OsdParam, hosdparam, true);
            HuoYanClientSdk.VzLPRClient_GetOsdParam(OpenHwnd, hosdparam);
            OsdParam = (HuoYanClientSdk.VZ_LPRC_OSD_Param)Marshal.PtrToStructure(hosdparam, typeof(HuoYanClientSdk.VZ_LPRC_OSD_Param));

            cb_dstampenable.Checked = OsdParam.dstampenable != 0;
            cb_tstampenable.Checked = OsdParam.tstampenable != 0;
            cb_nTextEnable.Checked = OsdParam.nTextEnable != 0;

            if (OsdParam.dateFormat >= 0 && OsdParam.dateFormat <= 3)//显示日期格式
                cb_dateFormat.SelectedIndex = OsdParam.dateFormat;
            else
                cb_dateFormat.SelectedIndex = 0;
            ud_datePosX.Value = OsdParam.datePosX;
            ud_datePosY.Value = OsdParam.datePosY;

            if (OsdParam.timeFormat == 0 || OsdParam.timeFormat == 1)//显示时间格式
                cb_timeFormat.SelectedIndex = OsdParam.timeFormat;
            else
                cb_timeFormat.SelectedIndex = 0;
            ud_timePosX.Value = OsdParam.timePosX;
            ud_timePosY.Value = OsdParam.timePosY;

            tb_overlaytext.Text = OsdParam.overlaytext;//显示内容
            ud_nTextPositionX.Value = OsdParam.nTextPositionX;
            ud_nTextPositionY.Value = OsdParam.nTextPositionY;

            Marshal.FreeHGlobal(hosdparam);
        }

        /// <summary>
        /// osd参数保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_osdsave_Click(object sender, EventArgs e)
        {
            OsdParam.dstampenable = cb_dstampenable.Checked ? (byte)1 : (byte)0;
            OsdParam.tstampenable = cb_tstampenable.Checked ? (byte)1 : (byte)0;
            OsdParam.nTextEnable = cb_nTextEnable.Checked ? (byte)1 : (byte)0;

            OsdParam.dateFormat = cb_dateFormat.SelectedIndex;
            OsdParam.datePosX = (int)ud_datePosX.Value;
            OsdParam.datePosY = (int)ud_datePosY.Value;

            OsdParam.timeFormat = cb_timeFormat.SelectedIndex;
            OsdParam.timePosX = (int)ud_timePosX.Value;
            OsdParam.timePosY = (int)ud_timePosY.Value;

            OsdParam.overlaytext = tb_overlaytext.Text;
            OsdParam.nTextPositionX = (int)ud_nTextPositionX.Value;
            OsdParam.nTextPositionY = (int)ud_nTextPositionY.Value;

            //修改osd参数
            IntPtr hosdparam = Marshal.AllocHGlobal(Marshal.SizeOf(OsdParam));
            Marshal.StructureToPtr(OsdParam, hosdparam, true);
            int osdresult = HuoYanClientSdk.VzLPRClient_SetOsdParam(OpenHwnd, hosdparam);
            Marshal.FreeHGlobal(hosdparam);

            if (osdresult == 0)
            {
                DateTime time = DateTime.Now;
                HuoYanClientSdk.VZ_DATE_TIME_INFO datetimeinfo = new HuoYanClientSdk.VZ_DATE_TIME_INFO()
                {
                    uYear = (uint)time.Year,
                    uMonth = (uint)time.Month,
                    uMDay = (uint)time.Day,
                    uHour = (uint)time.Hour,
                    uMin = (uint)time.Minute,
                    uSec = (uint)time.Second
                };

                //修改时间参数
                IntPtr hdatetimeinfo = Marshal.AllocHGlobal(Marshal.SizeOf(datetimeinfo));
                Marshal.StructureToPtr(datetimeinfo, hdatetimeinfo, true);
                int timeresult = HuoYanClientSdk.VzLPRClient_SetDateTime(OpenHwnd, hdatetimeinfo);
                Marshal.FreeHGlobal(hdatetimeinfo);
                if (osdresult == 0 && timeresult == 0)
                {
                    MessageBox.Show("   修改成功    ", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("   修改失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("   修改失败。   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 主流码保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cfgsave_Click(object sender, EventArgs e)
        {

            int sizeval = cb_framesize.SelectedIndex;
            int result = HuoYanClientSdk.VzLPRClient_SetVideoFrameSizeIndex(OpenHwnd, sizeval);
            if (result != 0)
            {
                MessageBox.Show("   分辨率设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int rateval = cb_framerate.SelectedIndex;
            result = HuoYanClientSdk.VzLPRClient_SetVideoFrameRate(OpenHwnd, rateval);
            if (result != 0)
            {
                MessageBox.Show("   帧率设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int penctype = cb_VideoEncodeType.SelectedIndex == 0 ? 0 : 2;
            result = HuoYanClientSdk.VzLPRClient_SetVideoEncodeType(OpenHwnd, penctype);
            if (result != 0)
            {
                MessageBox.Show("   编码方式设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cb_compressmode.Enabled)
            {
                int modeval = cb_compressmode.SelectedIndex;
                result = HuoYanClientSdk.VzLPRClient_SetVideoCompressMode(OpenHwnd, modeval);
                if (result != 0)
                {
                    MessageBox.Show("   流码控制设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int levelval = cb_imgquality.SelectedIndex;
            result = HuoYanClientSdk.VzLPRClient_SetVideoVBR(OpenHwnd, levelval);
            if (result != 0)
            {
                MessageBox.Show("   图像质量设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ud_rateval.Enabled)
            {
                int ratelist = (int)ud_rateval.Value;
                result = HuoYanClientSdk.VzLPRClient_SetVideoCBR(OpenHwnd, ratelist);
                if (result != 0)
                {
                    MessageBox.Show("   流码上限设置失败   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("   参数设置成功   ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 编码方式选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_VideoEncodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_compressmode.Enabled = cb_VideoEncodeType.SelectedIndex == 0;
        }

        /// <summary>
        /// 流码控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_compressmode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_compressmode.Enabled)
            {
                ud_rateval.Enabled = cb_compressmode.SelectedIndex == 0;
            }
            else
            {
                ud_rateval.Enabled = false;
            }
        }

        /// <summary>
        /// 恢复默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_recovery_Click(object sender, EventArgs e)
        {
            int brt = 50, cst = 40, sat = 30, hue = 50;
            tb_bright.Value = brt;
            tb_contrast.Value = cst;
            tb_saturation.Value = sat;
            tb_definition.Value = hue;

            HuoYanClientSdk.VzLPRClient_SetVideoPara(OpenHwnd, brt, cst, sat, hue);

            cb_videostandrad.SelectedIndex = 1;
            HuoYanClientSdk.VzLPRClient_SetFrequency(OpenHwnd, 1);

            cb_exposuretime.SelectedIndex = 1;
            HuoYanClientSdk.VzLPRClient_SetShutter(OpenHwnd, 3);

            cb_imgpos.SelectedIndex = 0;
            HuoYanClientSdk.VzLPRClient_SetFlip(OpenHwnd, 0);

        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_MouseUp(object sender, MouseEventArgs e)
        {
            if (IsMove)
            {
                int brt = tb_bright.Value;
                int cst = tb_contrast.Value;
                int sat = tb_saturation.Value;
                int hue = tb_definition.Value;
                HuoYanClientSdk.VzLPRClient_SetVideoPara(OpenHwnd, brt, cst, sat, hue);
                IsMove = false;
            }
        }

        /// <summary>
        /// 滑块移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_Scroll(object sender, EventArgs e)
        {
            IsMove = true;
        }

        /// <summary>
        /// 视频制式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_videostandrad_SelectedIndexChanged(object sender, EventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_SetFrequency(OpenHwnd, cb_videostandrad.SelectedIndex);
        }

        /// <summary>
        /// 曝光时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_exposuretime_SelectedIndexChanged(object sender, EventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_SetShutter(OpenHwnd, cb_exposuretime.SelectedIndex);
        }

        /// <summary>
        /// 图像翻转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_imgpos_SelectedIndexChanged(object sender, EventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_SetFlip(OpenHwnd, cb_imgpos.SelectedIndex);
        }


    }
}
