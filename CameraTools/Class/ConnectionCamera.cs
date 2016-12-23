
namespace CameraTools
{
    public class ConnectionCamera
    {
        public CameraTypes CameraType { get; set; }

        public int Index { get; set; }

        public string IP { get; set; }

        public int OpenHwnd { get; set; }

        public int ShowHwnd { get; set; }

        public int IoState { get; set; }

        public ushort Port { get; set; }

        public uint SL { get; set; }

        public uint SH { get; set; }

    }
}
