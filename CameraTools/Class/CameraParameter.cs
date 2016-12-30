using System;

namespace CameraTools
{
    public enum CameraTypes
    {
        AnShiBao = 0,
        HuoYan = 1,
        QianYi = 2
    }

    public class CameraParameter
    {
        public CameraTypes CameraType { get; set; }

        public string pStrDevName { get; set; }

        public string pStrIPAddr { get; set; }

        public ushort uPort1 { get; set; }

        public ushort usPort2 { get; set; }

        public uint SL { get; set; }

        public uint SH { get; set; }

        public IntPtr pUserData { get; set; }
    }
}
