using System;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class NetCfg_Form : Form
    {
        private string m_strIP;
        private uint m_nSL = 0;
        private uint m_nSH = 0;

        public NetCfg_Form(string strIP, uint SL, uint SH)
        {
            InitializeComponent();
            m_strIP = strIP;
            m_nSL = SL;
            m_nSH = SH;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strIP = txtIP.Text;

            string strNetmask = txtMask.Text;

            string strGateway = txtGateway.Text;

            int ret = HuoYanClientSdk.VzLPRClient_UpdateNetworkParam(m_nSH, m_nSL, strIP, strGateway, strNetmask);
            if (ret == 2)
            {
                MessageBox.Show("设备IP跟网关不在同一网段，请重新输入!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ret == -1)
            {
                MessageBox.Show("修改网络参数失败，请重新输入!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            MessageBox.Show("修改网络参数成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Tag = strIP;
            Close();
        }

        private void NetCfg_Form_Load(object sender, EventArgs e)
        {
            txtIP.Text = m_strIP;
        }
    }
}
