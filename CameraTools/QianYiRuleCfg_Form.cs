using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace CameraTools
{
    public partial class QianYiRuleCfg_Form : Form
    {
        private int _cameraHwnd;
        private const double ROIWIDTH = 1920;
        private const double ROIHEIGHT = 1080;
        private const int BOUNDVALUE = 10;
        private int _drawWidth = 1920;
        private int _drawHeight = 1080;
        private System.Timers.Timer _getCameraVodie;
        private System.Threading.Mutex _myMutex;
        private string _savePath;
        private QianYiClientSdk.T_VideoDetectParamSetup tVideoDetectParamSetup;
        private Point[] _pointLoopArry;
        private Point[] _pointPhotographArry;
        private Pen _linePen;
        private Pen _photographPen;
        private int _selectIndex = -1;
        private bool _isMouseDown;
        private bool _isSelectLoop;
        private Point _selectPoint;


        public QianYiRuleCfg_Form(int camerahwnd)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this._cameraHwnd = camerahwnd;

            this.Load += QianYiRuleCfg_Form_Load;
            this.FormClosing += QianYiRuleCfg_Form_FormClosing;
        }

        void QianYiRuleCfg_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            _getCameraVodie.Stop();
            _getCameraVodie.Dispose();
        }

        void QianYiRuleCfg_Form_Load(object sender, EventArgs e)
        {
            int iRet = QianYiClientSdk.Net_QueryVideoDetectSetup(_cameraHwnd, ref tVideoDetectParamSetup);
            if (iRet != 0)
            {
                MessageBox.Show("查询视频检测区域参数失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            _linePen = new Pen(Brushes.LemonChiffon, 2);
            _photographPen = new Pen(Brushes.Red, 2)
            {
                DashStyle = DashStyle.DashDot,
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };
            _savePath = Environment.CurrentDirectory + "\\Temp.jpg";
            _pointLoopArry = new Point[tVideoDetectParamSetup.atPlateRegion.Length];
            _pointPhotographArry = new Point[2];
            for (int i = 0; i < _pointLoopArry.Length; i++)
            {
                _pointLoopArry[i] = new Point(tVideoDetectParamSetup.atPlateRegion[i].sX, tVideoDetectParamSetup.atPlateRegion[i].sY);
            }
            if (_getCameraVodie == null)
            {
                _myMutex = new System.Threading.Mutex();
                _getCameraVodie = new System.Timers.Timer(80);
                _getCameraVodie.AutoReset = true;
                _getCameraVodie.Elapsed += _getCameraVodie_Elapsed;
            }
            _getCameraVodie.Start();
        }

        void _getCameraVodie_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _myMutex.WaitOne();

            int iRet = QianYiClientSdk.Net_SaveJpgFile(_cameraHwnd, _savePath);
            if (File.Exists(_savePath))
            {
                byte[] by;
                using (FileStream fs = new FileStream(_savePath, FileMode.Open, FileAccess.Read))
                {
                    by = new byte[fs.Length];
                    fs.Read(by, 0, by.Length);
                    fs.Close();
                }
                File.Delete(_savePath);

                MemoryStream ms = new MemoryStream(by);
                Bitmap img = new Bitmap(ms);
                if (img.Width != _drawWidth || img.Height != _drawHeight)
                {
                    _drawWidth = img.Width;
                    _drawHeight = img.Height;
                    for (int i = 0; i < _pointLoopArry.Length; i++)
                    {
                        _pointLoopArry[i].X = (int)(((double)tVideoDetectParamSetup.atPlateRegion[i].sX / ROIWIDTH) * _drawWidth);
                        _pointLoopArry[i].Y = (int)(((double)tVideoDetectParamSetup.atPlateRegion[i].sY / ROIHEIGHT) * _drawHeight);
                    }
                }
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.DrawPolygon(_linePen, _pointLoopArry);

                    DarwPhotograph(g);

                    Rectangle rect;
                    for (int i = 0; i < _pointLoopArry.Length; i++)
                    {
                        rect = new Rectangle(_pointLoopArry[i].X - BOUNDVALUE, _pointLoopArry[i].Y - BOUNDVALUE, BOUNDVALUE * 2, BOUNDVALUE * 2);
                        if (i == _selectIndex)
                            g.FillEllipse(Brushes.Green, rect);
                        else
                            g.FillEllipse(Brushes.LemonChiffon, rect);
                    }
                    pb_img.Image = img;
                }
            }

            _myMutex.ReleaseMutex();
        }

        private void DarwPhotograph(Graphics g)
        {
            Point p1 = _pointLoopArry[0];
            Point p2 = _pointLoopArry[1];
            Point p3 = _pointLoopArry[2];
            Point p4 = _pointLoopArry[3];

            int height1 = p2.Y - p1.Y;
            int photographheight1 = height1 - (int)((double)height1 * 0.8);
            int height2 = p3.Y - p4.Y;
            int photographheight2 = height2 - (int)((double)height2 * 0.8);

            int x1 = GetPhotographX(photographheight1, p1, p2);
            int x2 = GetPhotographX2(photographheight2, p4, p3);

            g.DrawLine(_photographPen, x1, p2.Y - photographheight1, x2, p3.Y - photographheight2);

        }

        private int GetPhotographX(int height, Point p1, Point p2)
        {
            double k = ((double)(p2.Y - p1.Y)) / (p2.X - p1.X);
            int py = p2.Y - height;
            if (p2.X > p1.X)
            {
                for (int i = p1.X; i < p2.X; i++)
                {
                    double y = k * (i - p1.X) + p1.Y;
                    if (py < (int)y)
                        return i;
                }
            }
            else
            {
                for (int i = p1.X - 1; i >= p2.X; i--)
                {
                    double y = k * (i - p2.X) + p2.Y;
                    if (py < (int)y)
                        return i;
                }
            }
            return p2.X;
        }

        private int GetPhotographX2(int height, Point p1, Point p2)
        {
            double k = ((double)(p2.Y - p1.Y)) / (p2.X - p1.X);
            int py = p2.Y - height;
            if (p2.X > p1.X)
            {
                for (int i = p2.X - 1; i >= p1.X; i--)
                {
                    double y = k * (i - p1.X) + p1.Y;
                    if (py >= (int)y)
                    {
                        return i;
                    }
                }

            }
            else
            {
                for (int i = p2.X; i < p1.X; i++)
                {
                    double y = k * (i - p2.X) + p2.Y;
                    if (py >= (int)y)
                        return i;
                }
            }
            return p2.X;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _pointLoopArry.Length; i++)
            {
                tVideoDetectParamSetup.atPlateRegion[i].sX = (short)(((double)_pointLoopArry[i].X / (double)_drawWidth) * ROIWIDTH);
                tVideoDetectParamSetup.atPlateRegion[i].sY = (short)(((double)_pointLoopArry[i].Y / (double)_drawHeight) * ROIHEIGHT);
            }
            int iRet = QianYiClientSdk.Net_VideoDetectSetup(_cameraHwnd, ref tVideoDetectParamSetup);
            if (iRet != 0)
            {
                MessageBox.Show("识别区域设置失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            QianYiClientSdk.Net_UpdatePlateRegion(_cameraHwnd);
        }

        private void pb_img_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = PointToImgPoint(e.Location);
            _selectIndex = GetSelectIndex(p);
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(_pointLoopArry);
            _isSelectLoop = path.IsVisible(p);
            _isMouseDown = true; ;
            _selectPoint = p;
        }

        private Point PointToImgPoint(Point p)
        {
            int x, y;
            x = (int)((double)p.X / (double)pb_img.Width * _drawWidth);
            y = (int)((double)p.Y / (double)pb_img.Height * _drawHeight);
            return new Point(x, y);
        }

        private int GetSelectIndex(Point p)
        {
            Rectangle rect;
            for (int i = 0; i < _pointLoopArry.Length; i++)
            {
                rect = new Rectangle(_pointLoopArry[i].X - BOUNDVALUE, _pointLoopArry[i].Y - BOUNDVALUE, BOUNDVALUE * 2, BOUNDVALUE * 2);
                if (rect.Contains(p))
                    return i;
            }
            return -1;
        }

        private void pb_img_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToImgPoint(e.Location);
            //this.Text = string.Format("X {0} Y {1} ", p.X, p.Y);
            if (_isMouseDown)
            {
                if (_selectIndex > -1 && _selectIndex < _pointLoopArry.Length)
                {
                    Point pnew = MovePointLimit(p.X, p.Y);
                    if (!GetPointLock(pnew))
                    {
                        _pointLoopArry[_selectIndex] = pnew;
                    }

                }
                else if (_isSelectLoop)
                {
                    pb_img.Cursor = Cursors.NoMove2D;
                    int x, y;
                    x = p.X - _selectPoint.X;
                    y = p.Y - _selectPoint.Y;
                    MoveBoundLimit(ref x, ref y);
                    if (x != 0 || y != 0)
                    {
                        MoveAllPoints(x, y);
                        _selectPoint.X += x;
                        _selectPoint.Y += y;
                    }
                }
            }
        }

        private bool GetPointLock(Point p)
        {
            Point pnew;
            double k;
            int finst = _selectIndex - 1;
            if (finst < 0)
                finst = _pointLoopArry.Length - 1;
            int two = _selectIndex + 1;
            if (two >= _pointLoopArry.Length)
                two = 0;
            Point p1 = new Point(_pointLoopArry[finst].X, _pointLoopArry[finst].Y);
            Point p2 = new Point(_pointLoopArry[two].X, _pointLoopArry[two].Y);
            if (p1.Y > p2.Y)
            {
                pnew = new Point(p1.X, p1.Y);
                p1 = p2;
                p2 = pnew;
            }
            switch (_selectIndex)
            {
                case 0:
                    pnew = new Point(p.X + BOUNDVALUE, p.Y + BOUNDVALUE);
                    if (pnew.X > p1.X) return true;
                    if (pnew.Y > p2.Y) return true;
                    k = ((double)(p2.Y - p1.Y)) / (p2.X - p1.X);
                    for (int i = p2.X; i < p1.X; i++)
                    {
                        double y = k * (i - p2.X) + p2.Y;
                        if (i == p.X && pnew.Y > (int)y)
                        {
                            return true;
                        }
                    }
                    break;
                case 1:
                    pnew = new Point(p.X + BOUNDVALUE, p.Y - BOUNDVALUE);
                    if (pnew.Y < p1.Y) return true;
                    if (pnew.X > p2.X) return true;
                    k = ((double)(p1.Y - p2.Y)) / (p1.X - p2.X);
                    for (int i = p1.X; i < p2.X; i++)
                    {
                        double y = k * (i - p1.X) + p1.Y;
                        if (i == p.X && pnew.Y < (int)y)
                        {
                            return true;
                        }
                    }
                    break;
                case 2:
                    pnew = new Point(p.X - BOUNDVALUE, p.Y - BOUNDVALUE);
                    if (pnew.Y < p1.Y) return true;
                    if (pnew.X < p2.X) return true;
                    k = ((double)(p2.Y - p1.Y)) / (p2.X - p1.X);
                    for (int i = p2.X; i < p1.X; i++)
                    {
                        double y = k * (i - p2.X) + p2.Y;
                        if (i == p.X && pnew.Y < (int)y)
                        {
                            return true;
                        }
                    }
                    break;
                case 3:
                    pnew = new Point(p.X - BOUNDVALUE, p.Y + BOUNDVALUE);
                    if (pnew.X < p1.X) return true;
                    if (pnew.Y > p2.Y) return true;
                    k = ((double)(p1.Y - p2.Y)) / (p1.X - p2.X);
                    for (int i = p1.X; i < p2.X; i++)
                    {
                        double y = k * (i - p1.X) + p1.Y;
                        if (i == p.X && pnew.Y > (int)y)
                        {
                            return true;
                        }
                    }
                    break;

            }
            return false;
        }

        private Point MovePointLimit(int x, int y)
        {
            if (x + BOUNDVALUE < 0)
                x = BOUNDVALUE;
            else if (x > _drawWidth - BOUNDVALUE)
                x = _drawWidth - BOUNDVALUE;
            if (y - BOUNDVALUE < 0)
                y = BOUNDVALUE;
            else if (y > _drawHeight - BOUNDVALUE)
                y = _drawHeight - BOUNDVALUE;
            return new Point(x, y);
        }

        private void MoveAllPoints(int x, int y)
        {
            for (int i = 0; i < _pointLoopArry.Length; i++)
            {
                _pointLoopArry[i].X += x;
                _pointLoopArry[i].Y += y;
            }
        }

        private void MoveBoundLimit(ref int x, ref int y)
        {
            int newx, newy;
            for (int i = 0; i < _pointLoopArry.Length; i++)
            {
                newx = _pointLoopArry[i].X + x;
                newy = _pointLoopArry[i].Y + y;
                if (newx > _drawWidth - BOUNDVALUE)
                {
                    newx = _drawWidth - BOUNDVALUE;
                }
                else if (newx + BOUNDVALUE < 0)
                {
                    newx = BOUNDVALUE;
                }
                x = newx - _pointLoopArry[i].X;
                if (newy > _drawHeight - BOUNDVALUE)
                {
                    newy = _drawHeight - BOUNDVALUE;
                }
                else if (newy - BOUNDVALUE < 0)
                {
                    newy = BOUNDVALUE;
                }
                y = newy - _pointLoopArry[i].Y;
            }
        }

        private void pb_img_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            pb_img.Cursor = Cursors.Default;
        }



    }
}
