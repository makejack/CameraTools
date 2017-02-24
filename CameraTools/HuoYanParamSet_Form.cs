using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class HuoYanParamSet_Form : Form
    {
        private int OpenHwnd;
        private int Shutter = 0, Plevenow = 0, Plevemax = 0;
        private HuoYanClientSdk.VZ_LED_CTRL Ledctrl;
        private HuoYanClientSdk.VZ_LPRC_PROVINCE_INFO Provinceinfo;

        public HuoYanParamSet_Form(int openhwnd)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.OpenHwnd = openhwnd;

        }

        private void ParamSet_Form_Load(object sender, EventArgs e)
        {
            //曝光时间
            int ret = HuoYanClientSdk.VzLPRClient_GetShutter(OpenHwnd, ref Shutter);
            if (ret == 0)
            {
                cb_ExposureTime.SelectedIndex = Shutter;
            }

            //预设省份
            Provinceinfo = new HuoYanClientSdk.VZ_LPRC_PROVINCE_INFO();
            ret = HuoYanClientSdk.VzLPRClient_GetSupportedProvinces(OpenHwnd, ref Provinceinfo);
            if (ret == 0)
            {
                if (Provinceinfo.nCurrIndex >= 0)
                    cb_Provinces.SelectedIndex = Provinceinfo.nCurrIndex + 1;
                else
                    cb_Provinces.SelectedIndex = 0;
            }


            Ledctrl = HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_AUTO;
            ret = HuoYanClientSdk.VzLPRClient_GetLEDLightControlMode(OpenHwnd, ref Ledctrl);
            switch (Ledctrl)
            {
                case HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_AUTO:
                    rb_LedMode1.Checked = true;
                    break;
                case HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_ON:
                    rb_LedMode2.Checked = true;
                    break;
                case HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_OFF:
                    rb_LedMode3.Checked = true;
                    break;
            }

            ret = HuoYanClientSdk.VzLPRClient_GetLEDLightStatus(OpenHwnd, ref Plevenow, ref Plevemax);
            if (ret == 0)
            {
                tb_Led.Value = Plevenow;
                tb_Led.Maximum = Plevemax;
            }

        }

        private void cb_ExposureTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Shutter != cb_ExposureTime.SelectedIndex)
            {
                Shutter = cb_ExposureTime.SelectedIndex;
                int ret = HuoYanClientSdk.VzLPRClient_SetShutter(OpenHwnd, Shutter);
                if (ret != 0)
                {
                    MessageBox.Show("曝光设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cb_Provinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Provinceinfo.nCurrIndex != cb_Provinces.SelectedIndex)
            {
                Provinceinfo.nCurrIndex = cb_Provinces.SelectedIndex - 1;
                int ret = HuoYanClientSdk.VzLPRClient_PresetProvinceIndex(OpenHwnd, Provinceinfo.nCurrIndex);
                if (ret != 0)
                {
                    MessageBox.Show("预设省份设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tb_Led_Scroll(object sender, EventArgs e)
        {
            if (Ledctrl == HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_ON)
            {
                if (Plevenow != tb_Led.Value)
                {
                    Plevenow = tb_Led.Value;
                    int ret = HuoYanClientSdk.VzLPRClient_SetLEDLightLevel(OpenHwnd, Plevenow);
                    if (ret != 0)
                    {
                        MessageBox.Show("LED亮度设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void rb_LedMode1_CheckedChanged(object sender, EventArgs e)
        {
            tb_Led.Enabled = false;
            if (Ledctrl != HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_AUTO)
            {
                Ledctrl = HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_AUTO;
                SetLedMode();
            }
        }

        private void rb_LedMode2_CheckedChanged(object sender, EventArgs e)
        {
            tb_Led.Enabled = true;
            if (Ledctrl != HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_ON)
            {
                Ledctrl = HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_ON;
                SetLedMode();
            }
        }

        private void rb_LedMode3_CheckedChanged(object sender, EventArgs e)
        {
            tb_Led.Enabled = false;
            if (Ledctrl != HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_OFF)
            {
                Ledctrl = HuoYanClientSdk.VZ_LED_CTRL.VZ_LED_MANUAL_OFF;
                SetLedMode();
            }
        }

        private void SetLedMode()
        {
            int ret = HuoYanClientSdk.VzLPRClient_SetLEDLightControlMode(OpenHwnd, Ledctrl);
            if (ret != 0)
            {
                MessageBox.Show("LED补光设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_SetTime_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            HuoYanClientSdk.VZ_DATE_TIME_INFO datetimeinfo = new HuoYanClientSdk.VZ_DATE_TIME_INFO()
            {
                uYear = (uint)now.Year,
                uMonth = (uint)now.Month,
                uMDay = (uint)now.Day,
                uHour = (uint)now.Hour,
                uMin = (uint)now.Minute,
                uSec = (uint)now.Second
            };
            IntPtr hwnd = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HuoYanClientSdk.VZ_DATE_TIME_INFO)));
            Marshal.StructureToPtr(datetimeinfo, hwnd, true);
            int ret = HuoYanClientSdk.VzLPRClient_SetDateTime(OpenHwnd, hwnd);
            if (ret != 0)
            {
                MessageBox.Show("同步时间失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Marshal.FreeHGlobal(hwnd);
        }

        private void btn_FocusNear_MouseDown(object sender, MouseEventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_CtrlLens(OpenHwnd, HuoYanClientSdk.VZ_LENS_OPT.VZ_LENS_FOCUS_NEAR);
        }

        private void btn_FocusNear_MouseUp(object sender, MouseEventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_CtrlLens(OpenHwnd, HuoYanClientSdk.VZ_LENS_OPT.VZ_LENS_OPT_STOP);
        }

        private void btn_FocusFar_MouseDown(object sender, MouseEventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_CtrlLens(OpenHwnd, HuoYanClientSdk.VZ_LENS_OPT.VZ_LENS_FOCUS_FAR);
        }

        private void btn_ZoomWide_MouseDown(object sender, MouseEventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_CtrlLens(OpenHwnd, HuoYanClientSdk.VZ_LENS_OPT.VZ_LENS_ZOOM_WIDE);
        }

        private void btn_ZoomTele_MouseDown(object sender, MouseEventArgs e)
        {
            HuoYanClientSdk.VzLPRClient_CtrlLens(OpenHwnd, HuoYanClientSdk.VZ_LENS_OPT.VZ_LENS_ZOOM_TELE);
        }
    }
}
