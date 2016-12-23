using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class AnShiBaoParamSet_Form : Form
    {
        private string CameraIp;
        private IntPtr CameraParam;
        private string ServerIp;
        private int ServerPort = 0;
        private string ServerUser;
        private string ServerPassword;
        private string Province;


        public AnShiBaoParamSet_Form(string ip)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.Load += AnShiBaoParamSet_Form_Load;

            this.CameraIp = ip;
        }

        void AnShiBaoParamSet_Form_Load(object sender, EventArgs e)
        {
            CameraParam = Marshal.AllocHGlobal(32768);
            int ret = AnShiBaoClientSdk.IPCSDK_Get_Camera_Para(CameraIp, CameraParam);
            if (ret != 0)
            {
                MessageBox.Show("获取参数失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            IntPtr iphwnd = Marshal.AllocHGlobal(20);
            IntPtr userhwnd = Marshal.AllocHGlobal(100);
            IntPtr pwdhwnd = Marshal.AllocHGlobal(100);
            IntPtr provincehwnd = Marshal.AllocHGlobal(2);
            try
            {

                ret = AnShiBaoClientSdk.IPCSDK_Alg_Get_Upload_Server_Info(CameraParam, iphwnd, ref ServerPort, userhwnd, pwdhwnd);
                if (ret == 0)
                {
                    ServerIp = Marshal.PtrToStringAnsi(iphwnd);
                    ServerUser = Marshal.PtrToStringAnsi(userhwnd);
                    ServerPassword = Marshal.PtrToStringAnsi(pwdhwnd);
                    tb_ServerIp.Text = ServerIp;
                }


                ret = AnShiBaoClientSdk.IPCSDK_Alg_Get_Default_Province(CameraParam, provincehwnd);
                if (ret == 0)
                {
                    Province = Marshal.PtrToStringAnsi(provincehwnd);
                    cb_Provinces.Text = Province;
                }

                l_Ip.Text = "本机IP    " + GetInternalIp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            finally
            {
                Marshal.FreeHGlobal(iphwnd);
                Marshal.FreeHGlobal(userhwnd);
                Marshal.FreeHGlobal(pwdhwnd);
                Marshal.FreeHGlobal(provincehwnd);
            }
        }

        private string GetInternalIp()
        {
            string localIP = "?";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            } return localIP;
        }

        private void btn_SetTime_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            int ret = AnShiBaoClientSdk.IPCSDK_Set_Camera_Time(CameraIp, now.Year, now.Month, now.Day, now.Day, now.Hour, now.Minute, now.Second);
            if (ret != 0)
            {
                MessageBox.Show("时间同步失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btn_SetTime.Enabled = false;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ServerIp = tb_ServerIp.Text;
            if (ServerIp.Length == 0)
            {
                MessageBox.Show("接收地址不能空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Province = cb_Provinces.Text;
            IntPtr iphwnd = Marshal.StringToHGlobalAnsi(ServerIp);
            IntPtr userhwnd = Marshal.StringToHGlobalAnsi(ServerUser);
            IntPtr pwdhwnd = Marshal.StringToHGlobalAnsi(ServerPassword);
            IntPtr provincehwnd = Marshal.StringToHGlobalAnsi(Province);
            try
            {
                int ret = AnShiBaoClientSdk.IPCSDK_Alg_Set_Upload_Server_Info(CameraParam, iphwnd, ServerPort, userhwnd, pwdhwnd);
                if (ret != 0)
                {
                    MessageBox.Show("接收地址设备失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ret = AnShiBaoClientSdk.IPCSDK_Alg_Set_Default_Province(CameraParam, provincehwnd);
                if (ret != 0)
                {
                    MessageBox.Show("预设省份设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                ret = AnShiBaoClientSdk.IPCSDK_Set_Camera_Para(CameraIp, CameraParam);
                if (ret != 0)
                {
                    MessageBox.Show("参数设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Marshal.FreeHGlobal(iphwnd);
                Marshal.FreeHGlobal(userhwnd);
                Marshal.FreeHGlobal(pwdhwnd);
                Marshal.FreeHGlobal(provincehwnd);
            }

        }

        private void AnShiBaoParamSet_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CameraParam != IntPtr.Zero)
                Marshal.FreeHGlobal(CameraParam);
        }
    }
}
