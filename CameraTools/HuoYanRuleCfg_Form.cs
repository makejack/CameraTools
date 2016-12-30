using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CameraTools
{
    public partial class HuoYanRuleCfg_Form : Form
    {
        private int _hplay;
        private uint _draw_Width = 1920;
        private uint _draw_Height = 720;
        private double zoom_x_rate = 0.0;
        private double zoom_y_rate = 0.0;
        private System.Timers.Timer ti;
        private Mutex _mutex;
        private byte[] _pic_data;
        private Point[] _pointlooparray;
        private IntPtr _img_buf_ptr;
        private HuoYanClientSdk.VZ_LPRC_VIRTUAL_LOOPS _m_virtual_loops;
        private Graphics _g;
        private Pen _p;
        private Brush _b;
        private bool _ismousedown;
        private bool _selectedloop;
        //private bool _isselected;
        private Point _selectedpoint;
        private int _selectedindex = -1;

        private const uint IMG_SIZE = 1920 * 1080; //图片的大小
        private const int LOOP_POINT_COUNT = 4;//座标数量
        private const int BOUND_VALUE = 5;
        private const int SELECT_BOUND_VALUE = 10;
        private const int SELE_BOUND_VALUE = 8;
        private const int CONTROL_BORDER = 2;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hplay"></param>
        public HuoYanRuleCfg_Form(int hplay)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this._hplay = hplay;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleCfg_Load(object sender, EventArgs e)
        {
            _mutex = new Mutex();
            _pic_data = new byte[IMG_SIZE];
            _pointlooparray = new Point[LOOP_POINT_COUNT];
            _g = pb_img.CreateGraphics();
            _p = new Pen(Brushes.LemonChiffon, 2);

            HuoYanClientSdk.VzLPRClient_StartRealPlayDecData(_hplay);
            GCHandle gc = GCHandle.Alloc(_pic_data, GCHandleType.Pinned);
            _img_buf_ptr = gc.AddrOfPinnedObject();

            LoadRuleParam();

            ti = new System.Timers.Timer();
            ti.Interval = 80;
            ti.AutoReset = true;
            ti.Elapsed += ti_Elapsed;
            ti.Start();

        }

        /// <summary>
        /// 加载线圈座标
        /// </summary>
        private void LoadRuleParam()
        {
            _m_virtual_loops = new HuoYanClientSdk.VZ_LPRC_VIRTUAL_LOOPS();

            HuoYanClientSdk.VzLPRClient_GetVirtualLoop(_hplay, ref _m_virtual_loops);
            if (_m_virtual_loops.uNumVirtualLoop > 0)
            {
                LoadLoopParam();

                uint x, y;
                for (int i = 0; i < LOOP_POINT_COUNT; i++)
                {
                    //计算在控件内画线的座标
                    x = _m_virtual_loops.struLoop[0].struVertex[i].X_1000 * _draw_Width / 1000;
                    y = _m_virtual_loops.struLoop[0].struVertex[i].Y_1000 * _draw_Height / 1000;
                    _pointlooparray[i] = new Point((int)x, (int)y);
                }
                GetZoom();
            }
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        private void LoadLoopParam()
        {
            uint time_gap = _m_virtual_loops.struLoop[0].uTriggerTimeGap;
            ud_Time.Value = time_gap;

            uint cross_dir = _m_virtual_loops.struLoop[0].eCrossDir;
            if (cross_dir >= 0 && cross_dir < 3)
            {
                cb_Dir.SelectedIndex = (int)cross_dir;
            }
            else
            {
                cb_Dir.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 间隔处理画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ti_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _mutex.WaitOne();

            //获取虚拟线圈的座标
            int result = HuoYanClientSdk.VzLPRClient_GetJpegStreamFromRealPlayDec(_hplay, _img_buf_ptr, IMG_SIZE, 100);
            if (result > 0)
            {
                MemoryStream ms = new MemoryStream(_pic_data);
                Bitmap bimg = new Bitmap(ms);
                Graphics g = Graphics.FromImage(bimg);
                if (_m_virtual_loops.uNumVirtualLoop == 1)
                {
                    //绘制线
                    g.DrawPolygon(_p, _pointlooparray);
                    if (_draw_Width != bimg.Width || _draw_Height != bimg.Height)
                    {
                        _draw_Width = (uint)bimg.Width;
                        _draw_Height = (uint)bimg.Height;
                        GetZoom();
                        //重新计算座标
                        for (int i = 0; i < LOOP_POINT_COUNT; i++)
                        {
                            _pointlooparray[i].X = (int)(_m_virtual_loops.struLoop[0].struVertex[i].X_1000 * _draw_Width / 1000);
                            _pointlooparray[i].Y = (int)(_m_virtual_loops.struLoop[0].struVertex[i].Y_1000 * _draw_Height / 1000);
                        }
                    }
                }

                //绘制四角
                for (int i = 0; i < LOOP_POINT_COUNT; i++)
                {
                    _b = Brushes.Red;
                    if (i == _selectedindex)
                        _b = Brushes.Green;
                    g.FillEllipse(_b, _pointlooparray[i].X - BOUND_VALUE, _pointlooparray[i].Y - BOUND_VALUE, BOUND_VALUE * 2, BOUND_VALUE * 2);
                }

                Rectangle rc_dest = new Rectangle(0, 0, pb_img.Width, pb_img.Height);
                Rectangle rc_src = new Rectangle(0, 0, bimg.Width, bimg.Height);
                _g.DrawImage(bimg, rc_dest, rc_src, GraphicsUnit.Pixel);
            }

            _mutex.ReleaseMutex();
        }

        private void GetZoom()
        {
            zoom_x_rate = (double)pb_img.Width / (double)_draw_Width;
            zoom_y_rate = (double)pb_img.Height / (double)_draw_Height;
        }

        /// <summary>
        /// 窗体关闭前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RuleCfg_FormClosing(object sender, FormClosingEventArgs e)
        {
            ti.Stop();
            HuoYanClientSdk.VzLPRClient_StopRealPlayDecData(_hplay);
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_img_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = PointToImgPoint(e.X, e.Y);
            _selectedindex = GetSelectedIndex(p);
            System.Drawing.Drawing2D.GraphicsPath gpath = new System.Drawing.Drawing2D.GraphicsPath();
            gpath.AddPolygon(_pointlooparray);
            _selectedloop = gpath.IsVisible(p);
            gpath.Dispose();
            _selectedpoint = p;
            _ismousedown = true;
        }

        /// <summary>
        /// 获取选择的点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetSelectedIndex(Point p)
        {
            int x, y;
            Rectangle rc;
            for (int i = 0; i < LOOP_POINT_COUNT; i++)
            {
                x = _pointlooparray[i].X;
                y = _pointlooparray[i].Y;
                rc = new Rectangle(x - BOUND_VALUE - 1, y - BOUND_VALUE - 1, BOUND_VALUE * 2 + 2, BOUND_VALUE * 2 + 2);
                if (rc.Contains(p))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 将控件座标转换成图片座标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point PointToImgPoint(int x, int y)
        {
            Point p = new Point();
            p.X = GetImgX(x);
            p.Y = GetImgY(y);
            return p;
        }

        /// <summary>
        /// 获取鼠标在图片上的X座标
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int GetImgX(int x)
        {
            int img_x = x;
            if (zoom_x_rate > 0.01)
            {
                img_x = (int)(x / zoom_x_rate);
            }
            return img_x;
        }

        /// <summary>
        /// 获取鼠标在图片上的Y座标
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private int GetImgY(int y)
        {
            int img_y = y;
            if (zoom_y_rate > 0.01)
            {
                img_y = (int)(y / zoom_y_rate);
            }
            return img_y;
        }


        /// <summary>
        /// 鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_img_MouseUp(object sender, MouseEventArgs e)
        {
            _ismousedown = false;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_img_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToImgPoint(e.X, e.Y);
            if (_ismousedown)
            {
                if (_selectedindex >= 0 && _selectedindex < LOOP_POINT_COUNT)//移动单个线圈的点
                {
                    Point p_new = GetInBoundPoint(p);
                    if (!GetEditIsCross(_selectedindex, p_new))
                    {
                        _pointlooparray[_selectedindex] = p_new;
                    }
                }
                else if (_selectedloop)//移动整个线圈
                {
                    int x, y;
                    x = p.X - _selectedpoint.X;
                    y = p.Y - _selectedpoint.Y;
                    MoveBoundLimit(ref x, ref y);
                    if (x != 0 || y != 0)
                    {
                        MoveAllPoints(x, y);
                        _selectedpoint.X += x;
                        _selectedpoint.Y += y;
                    }
                }
            }

        }
        private bool GetEditIsCross(int nPtIndex, Point ptMouse)
        {
            int nMaxIndex = LOOP_POINT_COUNT - 1;

            bool bCross = false;
            bool bCross1, bCross2;

            Point ptEdge1Start, ptEdge1End;
            Point ptEdge2Start, ptEdge2End;

            Point ptStart, ptEnd;

            // 移动起点
            if (nPtIndex == 0)
            {
                ptStart = _pointlooparray[1];
                ptEnd = ptStart;

                // 第一条边
                ptEdge1Start = ptMouse;
                ptEdge1End = _pointlooparray[1];

                for (int i = 2; i < LOOP_POINT_COUNT; i++)
                {
                    ptEnd = _pointlooparray[i];
                    bCross = GetLineIsCross(ptEdge1Start, ptEdge1End, ptStart, ptEnd);

                    if (bCross)
                    {
                        break;
                    }

                    ptStart = ptEnd;
                }
            } // 移动终点
            else if (nPtIndex == nMaxIndex)
            {
                ptStart = _pointlooparray[0];
                ptEnd = ptStart;

                // 第一条边
                ptEdge1Start = _pointlooparray[nMaxIndex - 1];
                ptEdge1End = ptMouse;

                for (int i = 1; i < nMaxIndex; i++)
                {
                    ptEnd = _pointlooparray[i];
                    bCross = GetLineIsCross(ptEdge1Start, ptEdge1End, ptStart, ptEnd);

                    if (bCross)
                    {
                        break;
                    }

                    ptStart = ptEnd;
                }
            }
            else
            {
                ptStart = _pointlooparray[0];
                ptEnd = ptStart;

                // 第一条边
                ptEdge1Start = _pointlooparray[nPtIndex - 1];
                ptEdge1End = ptMouse;

                // 第二条边
                ptEdge2Start = ptMouse;
                ptEdge2End = _pointlooparray[nPtIndex + 1];

                for (int i = 1; i < LOOP_POINT_COUNT; i++)
                {
                    ptEnd = _pointlooparray[i];

                    if ((i == nPtIndex) || (i == (nPtIndex + 1)))
                    {
                        ptStart = ptEnd;
                        continue;
                    }

                    bCross1 = GetLineIsCross(ptEdge1Start, ptEdge1End, ptStart, ptEnd);
                    bCross2 = GetLineIsCross(ptEdge2Start, ptEdge2End, ptStart, ptEnd);

                    if (bCross1 || bCross2)
                    {
                        bCross = true;
                        break;
                    }

                    ptStart = ptEnd;
                }
            }

            return bCross;
        }

        private bool GetLineIsCross(Point ptMa, Point ptMb, Point ptNa, Point ptNb)
        {
            double dbV1, dbV2, dbV3, dbV4;

            dbV1 = (ptMb.X - ptMa.X) * (ptNb.Y - ptMa.Y) - (ptMb.Y - ptMa.Y) * (ptNb.X - ptMa.X);
            dbV2 = (ptMb.X - ptMa.X) * (ptNa.Y - ptMa.Y) - (ptMb.Y - ptMa.Y) * (ptNa.X - ptMa.X);

            double dbResult1 = dbV1 * dbV2;
            if (dbResult1 >= 0)
            {
                return false;
            }

            dbV3 = (ptNb.X - ptNa.X) * (ptMb.Y - ptNa.Y) - (ptNb.Y - ptNa.Y) * (ptMb.X - ptNa.X);
            dbV4 = (ptNb.X - ptNa.X) * (ptMa.Y - ptNa.Y) - (ptNb.Y - ptNa.Y) * (ptMa.X - ptNa.X);

            double dbResult2 = dbV3 * dbV4;
            if (dbResult2 >= 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移动所有座标点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveAllPoints(int x, int y)
        {
            for (int i = 0; i < LOOP_POINT_COUNT; i++)
            {
                _pointlooparray[i].X += x;
                _pointlooparray[i].Y += y;
            }
        }

        /// <summary>
        /// 移动限制
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MoveBoundLimit(ref int x, ref int y)
        {
            int curx, cury;
            for (int i = 0; i < LOOP_POINT_COUNT; i++)
            {
                curx = _pointlooparray[i].X + x;
                cury = _pointlooparray[i].Y + y;
                if (curx > ((int)_draw_Width - CONTROL_BORDER))
                    curx = (int)_draw_Width - CONTROL_BORDER;
                else if (curx < 0)
                    curx = 0;
                x = curx - _pointlooparray[i].X;
                if (cury > ((int)_draw_Height - CONTROL_BORDER))
                    cury = (int)_draw_Height - CONTROL_BORDER;
                else if (cury < 0)
                    cury = 0;
                y = cury - _pointlooparray[i].Y;
            }
        }

        /// <summary>
        /// 控制座标不超出范围
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point GetInBoundPoint(Point p)
        {
            if (p.X < 0)
                p.X = 0;
            if (p.X > ((int)_draw_Width - CONTROL_BORDER))
                p.X = (int)_draw_Width - CONTROL_BORDER;
            if (p.Y < 0)
                p.Y = 0;
            if (p.Y > ((int)_draw_Height - CONTROL_BORDER))
                p.Y = (int)_draw_Height - CONTROL_BORDER;
            for (int i = 0; i < LOOP_POINT_COUNT; i++)
            {
                if (_selectedindex == LOOP_POINT_COUNT) continue;
            }
            return p;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (_hplay > 0)
            {
                for (int i = 0; i < LOOP_POINT_COUNT; i++)
                {
                    _m_virtual_loops.struLoop[0].struVertex[i].X_1000 = (uint)(_pointlooparray[i].X * 1000 / _draw_Width);
                    _m_virtual_loops.struLoop[0].struVertex[i].Y_1000 = (uint)(_pointlooparray[i].Y * 1000 / _draw_Height);
                }

                //_m_virtual_loops.struLoop[0].uMinLPWidth = uint.Parse(txtMinWidth.Text);
                //_m_virtual_loops.struLoop[0].uMaxLPWidth = uint.Parse(txtMaxWidth.Text);
                _m_virtual_loops.struLoop[0].uTriggerTimeGap = (uint)ud_Time.Value;
                _m_virtual_loops.struLoop[0].eCrossDir = (uint)(cb_Dir.SelectedIndex);
                _m_virtual_loops.struLoop[0].byDraw = (byte)1;
                _m_virtual_loops.struLoop[0].byEnable = (byte)1;


                int ret = HuoYanClientSdk.VzLPRClient_SetVirtualLoop(_hplay, ref _m_virtual_loops);
                if (ret == 0)
                {
                    MessageBox.Show("保存线圈参数成功！");
                }
                else
                {
                    MessageBox.Show("保存线圈参数失败！");
                }
            }
        }


    }
}
