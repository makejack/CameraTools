using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class QianYiParamSet_Form : Form
    {
        private QianYiClientSdk.T_VehicleVAFunSetup tVehicleVaFunSetup;
        private int cameraHwnd;
        private uint[] g_uiPlateDefaultWord = new uint[32];
        private string[] szPlateDefaultWord = new string[]{
	        "京",
	        "津",
	        "沪",
	        "渝",
	        "冀",
	        "晋",
	        "辽",
	        "吉",
	        "黑",
	        "苏",
	        "浙",
	        "皖",
	        "闽",
	        "赣",
	        "鲁",
	        "豫",
	        "鄂",
	        "湘",
	        "粤",
	        "琼",
	        "川",
	        "贵",
	        "云",
	        "陕",
	        "甘",
	        "宁",
	        "青",
	        "藏",
	        "桂",
	        "蒙",
	        "新",
	        "全国"
        };

        public QianYiParamSet_Form(int camerahwnd)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;

            this.cameraHwnd = camerahwnd;

            this.Load += QianYiParamSet_Form_Load;
        }

        void QianYiParamSet_Form_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < szPlateDefaultWord.Length; i++)
            {
                cb_Provinces.Items.Add(szPlateDefaultWord[i]);
                if (i != szPlateDefaultWord.Length - 1)
                {
                    byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(szPlateDefaultWord[i]);
                    g_uiPlateDefaultWord[i] = (uint)((utf8[2] << 16) | (utf8[1] << 8) | utf8[0]);
                }
                else
                {
                    g_uiPlateDefaultWord[i] = 0;
                }
            }

            int iRet = QianYiClientSdk.Net_QueryVehicleVAFunSetup(cameraHwnd, ref tVehicleVaFunSetup);
            if (iRet != 0)
            {
                MessageBox.Show("参数获取失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
            else
            {
                for (int i = 0; i < g_uiPlateDefaultWord.Length; i++)
                {
                    if (g_uiPlateDefaultWord[i] == tVehicleVaFunSetup.uiPlateDefaultWord)
                    {
                        cb_Provinces.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btn_SetTime_Click(object sender, EventArgs e)
        {
            QianYiClientSdk.T_DCTimeSetup timesetup = new QianYiClientSdk.T_DCTimeSetup();

            int iRet = QianYiClientSdk.Net_QueryTimeSetup(cameraHwnd, ref timesetup);
            if (iRet != 0)
            {
                MessageBox.Show("获取时间失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime now = DateTime.Now;
            timesetup.usYear = (ushort)now.Year;
            timesetup.ucMonth = (byte)now.Month;
            timesetup.ucDay = (byte)now.Day;
            timesetup.ucHour = (byte)now.Hour;
            timesetup.ucMinute = (byte)now.Minute;
            timesetup.ucSecond = (byte)now.Second;
            iRet = QianYiClientSdk.Net_TimeSetup(cameraHwnd, ref timesetup);
            if (iRet != 0)
            {
                MessageBox.Show("同步时间失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_Provinces_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (g_uiPlateDefaultWord[cb_Provinces.SelectedIndex] != tVehicleVaFunSetup.uiPlateDefaultWord)
            {
                tVehicleVaFunSetup.uiPlateDefaultWord = g_uiPlateDefaultWord[cb_Provinces.SelectedIndex];
                int iRet = QianYiClientSdk.Net_VehicleVAFunSetup(cameraHwnd, ref tVehicleVaFunSetup);
                if (iRet != 0)
                {
                    MessageBox.Show("预设省份设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_FocusNear_MouseDown(object sender, MouseEventArgs e)
        {
            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_FOCUS_AUTO_START,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_SUBTRACT,
                ucLensSteps = 20
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);
        }

        private void btn_FocusNear_MouseUp(object sender, MouseEventArgs e)
        {
            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_FOCUS_AUTO_STOP,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_SUBTRACT,
                ucLensSteps = 20
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);
        }

        private void btn_FocusFar_MouseDown(object sender, MouseEventArgs e)
        {

            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_FOCUS_AUTO_START,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_ADD,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref  lenscontrol);
        }

        private void btn_FocusFar_MouseUp(object sender, MouseEventArgs e)
        {
            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_FOCUS_AUTO_STOP,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_ADD,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);
        }

        private void btn_ZoomWide_MouseDown(object sender, MouseEventArgs e)
        {

            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_ZOOM_AUTO_START,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_SUBTRACT,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref  lenscontrol);
        }

        private void btn_ZoomWide_MouseUp(object sender, MouseEventArgs e)
        {
            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_ZOOM_AUTO_STOP,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_SUBTRACT,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);

        }

        private void btn_ZoomTele_MouseDown(object sender, MouseEventArgs e)
        {

            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_ZOOM_AUTO_START,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_ADD,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);
        }

        private void btn_ZoomTele_MouseUp(object sender, MouseEventArgs e)
        {
            QianYiClientSdk.T_LensControl lenscontrol = new QianYiClientSdk.T_LensControl()
            {
                ucLensType = QianYiClientSdk.E_LensType.LENS_TYPE_ZOOM_AUTO_STOP,
                ucLensAction = QianYiClientSdk.E_LensAction.LENS_ACTION_ADD,
                ucLensSteps = 1
            };
            QianYiClientSdk.Net_LensControl(cameraHwnd, ref lenscontrol);

        }


    }
}
