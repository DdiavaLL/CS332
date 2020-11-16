using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Lab9
{
    class Graphic : Polyhedron
    {
        public Func<double, double, double> F;
        public int X0 { get; }
        public int X1 { get; }
        public int Y0 { get; }
        public int Y1 { get; }
        public int CountOfSplits { get; }

        public PictureBox picture;

        public double phi = 60, psi = 100;

        readonly Func<double, double, double> func = (x, y) => Math.Cos(Math.Sqrt(x * x + y * y));

        //границы горизонта
        List<double> upFloatingHorizon, downFloatingHorizon;

        public Graphic(int func)
        {
            switch (func)
            {
                case 0:
                    F = (x, y) => x + y;
                    break;
                case 1:
                    F = (x, y) => (float)Math.Cos(x * x + y * y);
                    break;
                case 2:
                    F = (x, y) => (float)Math.Sin(x) + (float)Math.Cos(y);
                    break;
                case 3:
                    F = (x, y) => (float)Math.Sin(x);
                    break;
                case 4:
                    F = (x, y) => Math.Cos(Math.Sqrt(x * x + y * y));
                    break;
                case 5:
                    F = (x, y) => Math.Sin(Math.Sqrt(x * x + y * y));
                    break;
                default:
                    F = (x, y) => x + y;
                    break;
            }
        }

        public void DrawGraphic()
        {
            //создаем и заполняем границы горизонта
            upFloatingHorizon = new List<double>(picture.Width);
            downFloatingHorizon = new List<double>(picture.Width);
            for (int i = 0; i < picture.Width; i++)
            {
                upFloatingHorizon.Add(0);
                downFloatingHorizon.Add(1000);
            }

            Bitmap res = new Bitmap(picture.Width, picture.Height);

            for (double x = -5; x <= 5.001; x += 0.2)
            {
                List<Point> currentCurve = new List<Point>();
                for (double y = -5; y <= 5.001; y += 0.2)
                {
                    double z = F(x, y);

                    //отображение координат на данной проекции
                    double _phi = phi * Math.PI / 180, _psi = psi * Math.PI / 180;
                    double fx = x * Math.Cos(_psi) - (-Math.Sin(_phi) * y + Math.Cos(_phi) * z) * Math.Sin(_psi);
                    double fy = y * Math.Cos(_psi) + z * Math.Sin(_phi);
                    double k = 50d;

                    //отображение на pictureBox
                    currentCurve.Add(new Point((int)Math.Round(picture.Width / 2 + fx * k), (int)Math.Round(picture.Height / 2 + fy * k)));
                }
                DrawCurve(res, currentCurve, upFloatingHorizon, downFloatingHorizon);
            }
            if (picture.Image != null) picture.Image.Dispose();
            picture.Image = res;
        }

        public void DrawCurve(Bitmap res, List<Point> curve, List<double> upFloatingHorizon, List<double> downFloatingHorizon)
        {
            for (int i = 1; i < curve.Count; i++)
            {
                Point p1 = curve[i - 1];
                Point p2 = curve[i];

                int x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
                bool needChange = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

                if (needChange)
                {
                    Swap(ref x1, ref y1);
                    Swap(ref x2, ref y2);
                }
                if (x1 > x2)
                {
                    Swap(ref x1, ref x2);
                    Swap(ref y1, ref y2);
                }

                double df = (y2 * 1.0 - y1) / (x2 * 1.0 - x1); //находим градиент
                double y = y1 + df;

                for (int x = x1 + 1; x < x2; x++)
                {
                    int xx1 = needChange ? (int)Math.Round(y) : x;
                    int xx2 = needChange ? (int)Math.Round(y) : x;
                    int yy1 = needChange ? x : (int)Math.Round(y);
                    int yy2 = needChange ? x : (int)Math.Round(y + 1);

                    if (xx1 < 0 || xx2 < 0 || yy1 < 0 || yy2 < 0) continue;
                    if (xx1 >= res.Width || xx2 >= res.Width || yy1 >= res.Height || yy2 >= res.Height) continue;

                    if ((yy1 >= upFloatingHorizon[xx1] && yy2 >= upFloatingHorizon[xx2]))
                    {
                        res.SetPixel(xx2, yy2, Color.Black);
                        upFloatingHorizon[xx1] = yy1;
                        upFloatingHorizon[xx2] = yy2;
                    }
                    else if (yy1 <= downFloatingHorizon[xx1] && yy2 <= downFloatingHorizon[xx2])
                    {
                        res.SetPixel(xx1, yy1, Color.Black);
                        res.SetPixel(xx2, yy2, Color.Black);
                        downFloatingHorizon[xx1] = yy1;
                        downFloatingHorizon[xx2] = yy2;

                    }
                    y += df;
                }
            }
        }

        private void Swap(ref int o1, ref int o2)
        {
            int t = o1;
            o1 = o2;
            o2 = t;
        }

        private void FloatingHorizonForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                psi -= 10;
            else if (e.KeyCode == Keys.D)
                psi += 10;
            else if (e.KeyCode == Keys.W)
                phi -= 10;
            else if (e.KeyCode == Keys.S)
                phi += 10;
            DrawGraphic();
        }
    }

    public sealed class ReverseFloatComparer : IComparer<float>
    {
        public int Compare(float x, float y)
        {
            return y.CompareTo(x);
        }
    }
}
