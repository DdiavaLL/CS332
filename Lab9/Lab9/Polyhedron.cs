using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Lab9
{
    class Polyhedron
    {
        public List<Polygon> Polygons { get; set; } = null;
        public Point3D Center { get; set; } = new Point3D(0, 0, 0);

        private Dictionary<Point3D, List<float>> point_to_normal = null;

        private Dictionary<Point3D, float> point_to_intensive = null;

        private Dictionary<Point3D, List<int>> map = null;

        public Polyhedron(List<Polygon> fs = null)
        {
            if (fs != null)
            {
                Polygons = fs.Select(face => new Polygon(face)).ToList();
                UpdateCenter();
            }
        }

        public Polyhedron(Polyhedron polyhedron)
        {
            Polygons = polyhedron.Polygons.Select(face => new Polygon(face)).ToList();
            Center = new Point3D(polyhedron.Center);
        }

        public Polyhedron(string s)
        {
            Polygons = new List<Polygon>();

            var arr = s.Split('\n');

            for (int i = 0; i < arr.Length; ++i)
            {
                if (string.IsNullOrEmpty(arr[i]))
                    continue;
                Polygon f = new Polygon(arr[i]);
                Polygons.Add(f);
            }
            UpdateCenter();
        }
        private void UpdateCenter()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (Polygon f in Polygons)
            {
                Center.X += f.Center.X;
                Center.Y += f.Center.Y;
                Center.Z += f.Center.Z;
            }
            Center.X /= Polygons.Count;
            Center.Y /= Polygons.Count;
            Center.Z /= Polygons.Count;
        }

        public void Show(Graphics g, Projection pr = 0, Pen pen = null)
        {
            foreach (Polygon f in Polygons)
            {
                f.FindNormal(Center, new Edge(new Point3D(0, 0, 500), new Point3D(0, 0, 500)));
                if (f.IsVisible)
                    f.Show(g, pr, pen);
            }
        }

        public void Translate(float x, float y, float z)
        {
            foreach (Polygon f in Polygons)
                f.translate(x, y, z);
            UpdateCenter();
        }

        public void Rotate(double angle, Axis a, Edge line = null)
        {
            foreach (Polygon f in Polygons)
                f.rotate(angle, a, line);
            UpdateCenter();
        }

        public void Scale(float kx, float ky, float kz)
        {
            foreach (Polygon f in Polygons)
                f.scale(kx, ky, kz);
            UpdateCenter();
        }

        public void reflectX()
        {
            if (Polygons != null)
                foreach (var f in Polygons)
                    f.reflectX();
            UpdateCenter();
        }

        public void reflectY()
        {
            if (Polygons != null)
                foreach (var f in Polygons)
                    f.reflectY();
            UpdateCenter();
        }

        public void reflectZ()
        {
            if (Polygons != null)
                foreach (var f in Polygons)
                    f.reflectZ();
            UpdateCenter();
        }

        public void Hexahedron(float size = 50)
        {
            Polygon f = new Polygon(
                new List<Point3D>
                {
                    new Point3D(-size, size, size),
                    new Point3D(size, size, size),
                    new Point3D(size, -size, size),
                    new Point3D(-size, -size, size)
                }
            );


            Polygons = new List<Polygon> { f };

            Polygon f1 = new Polygon(
                    new List<Point3D>
                    {
                        new Point3D(-size, size, -size),
                        new Point3D(-size, -size, -size),
                        new Point3D(size, -size, -size),
                        new Point3D(size, size, -size)
                    });

            Polygons.Add(f1);

            List<Point3D> l2 = new List<Point3D>
            {
                new Point3D(f.Points[2]),
                new Point3D(f1.Points[2]),
                new Point3D(f1.Points[1]),
                new Point3D(f.Points[3]),
            };
            Polygon f2 = new Polygon(l2);
            Polygons.Add(f2);

            List<Point3D> l3 = new List<Point3D>
            {
                new Point3D(f1.Points[0]),
                new Point3D(f1.Points[3]),
                new Point3D(f.Points[1]),
                new Point3D(f.Points[0]),
            };
            Polygon f3 = new Polygon(l3);
            Polygons.Add(f3);

            List<Point3D> l4 = new List<Point3D>
            {
                new Point3D(f1.Points[0]),
                new Point3D(f.Points[0]),
                new Point3D(f.Points[3]),
                new Point3D(f1.Points[1])
            };
            Polygon f4 = new Polygon(l4);
            Polygons.Add(f4);

            List<Point3D> l5 = new List<Point3D>
            {
                new Point3D(f1.Points[3]),
                new Point3D(f1.Points[2]),
                new Point3D(f.Points[2]),
                new Point3D(f.Points[1])
            };
            Polygon f5 = new Polygon(l5);
            Polygons.Add(f5);

            UpdateCenter();
        }

        public void Tetrahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.Hexahedron();
            }
            Polygon f0 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[0].Points[0]),
                    new Point3D(cube.Polygons[1].Points[1]),
                    new Point3D(cube.Polygons[1].Points[3])
                }
            );

            Polygon f1 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[1].Points[3]),
                    new Point3D(cube.Polygons[1].Points[1]),
                    new Point3D(cube.Polygons[0].Points[2])
                }
            );

            Polygon f2 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[0].Points[2]),
                    new Point3D(cube.Polygons[1].Points[1]),
                    new Point3D(cube.Polygons[0].Points[0])
                }
            );

            Polygon f3 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[0].Points[2]),
                    new Point3D(cube.Polygons[0].Points[0]),
                    new Point3D(cube.Polygons[1].Points[3])
                }
            );

            Polygons = new List<Polygon> { f0, f1, f2, f3 };
            UpdateCenter();
        }

        public void Octahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.Hexahedron();
            }

            Polygon f0 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[2].Center),
                    new Point3D(cube.Polygons[1].Center),
                    new Point3D(cube.Polygons[4].Center)
                }
            );

            Polygon f1 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[2].Center),
                    new Point3D(cube.Polygons[1].Center),
                    new Point3D(cube.Polygons[5].Center)
                }
            );

            Polygon f2 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[2].Center),
                    new Point3D(cube.Polygons[5].Center),
                    new Point3D(cube.Polygons[0].Center)
                }
            );

            Polygon f3 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[2].Center),
                    new Point3D(cube.Polygons[0].Center),
                    new Point3D(cube.Polygons[4].Center)
                }
            );

            Polygon f4 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[3].Center),
                    new Point3D(cube.Polygons[1].Center),
                    new Point3D(cube.Polygons[4].Center)
                }
            );

            Polygon f5 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[3].Center),
                    new Point3D(cube.Polygons[1].Center),
                    new Point3D(cube.Polygons[5].Center)
                }
            );

            Polygon f6 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[3].Center),
                    new Point3D(cube.Polygons[5].Center),
                    new Point3D(cube.Polygons[0].Center)
                }
            );

            Polygon f7 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Polygons[3].Center),
                    new Point3D(cube.Polygons[0].Center),
                    new Point3D(cube.Polygons[4].Center)
                }
            );

            Polygons = new List<Polygon> { f0, f1, f2, f3, f4, f5, f6, f7 };
            UpdateCenter();
        }

        public void Icosahedron()
        {
            Polygons = new List<Polygon>();

            float size = 100;

            float r1 = size * (float)Math.Sqrt(3) / 4;
            float r = size * (3 + (float)Math.Sqrt(5)) / (4 * (float)Math.Sqrt(3));

            Point3D up_center = new Point3D(0, -r1, 0);
            Point3D down_center = new Point3D(0, r1, 0);

            double a = Math.PI / 2;
            List<Point3D> up_points = new List<Point3D>();
            for (int i = 0; i < 5; ++i)
            {
                up_points.Add(new Point3D(up_center.X + r * (float)Math.Cos(a), up_center.Y, up_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            a = Math.PI / 2 - Math.PI / 5;
            List<Point3D> down_points = new List<Point3D>();
            for (int i = 0; i < 5; ++i)
            {
                down_points.Add(new Point3D(down_center.X + r * (float)Math.Cos(a), down_center.Y, down_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            var R = Math.Sqrt(2 * (5 + Math.Sqrt(5))) * size / 4;

            Point3D p_up = new Point3D(up_center.X, (float)(-R), up_center.Z);
            Point3D p_down = new Point3D(down_center.X, (float)R, down_center.Z);

            for (int i = 0; i < 5; ++i)
            {
                Polygons.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(p_up),
                        new Point3D(up_points[i]),
                        new Point3D(up_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Polygons.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(p_down),
                        new Point3D(down_points[i]),
                        new Point3D(down_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Polygons.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(up_points[i]),
                        new Point3D(up_points[(i+1) % 5]),
                        new Point3D(down_points[(i+1) % 5])
                    })
                    );

                Polygons.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(up_points[i]),
                        new Point3D(down_points[i]),
                        new Point3D(down_points[(i+1) % 5])
                    })
                    );
            }

            UpdateCenter();
        }

        public void Dodecahedron()
        {
            Polygons = new List<Polygon>();
            Polyhedron ik = new Polyhedron();
            ik.Icosahedron();

            List<Point3D> pts = new List<Point3D>();
            foreach (Polygon f in ik.Polygons)
            {
                pts.Add(f.Center);
            }

            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[0]),
                new Point3D(pts[1]),
                new Point3D(pts[2]),
                new Point3D(pts[3]),
                new Point3D(pts[4])
            }));

            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[5]),
                new Point3D(pts[6]),
                new Point3D(pts[7]),
                new Point3D(pts[8]),
                new Point3D(pts[9])
            }));

            for (int i = 0; i < 5; ++i)
            {
                Polygons.Add(new Polygon(new List<Point3D>
                {
                    new Point3D(pts[i]),
                    new Point3D(pts[(i + 1) % 5]),
                    new Point3D(pts[(i == 4) ? 10 : 2*i + 12]),
                    new Point3D(pts[(i == 4) ? 11 : 2*i + 13]),
                    new Point3D(pts[2*i + 10])
                }));
            }

            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[5]),
                new Point3D(pts[6]),
                new Point3D(pts[13]),
                new Point3D(pts[10]),
                new Point3D(pts[11])
            }));
            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[6]),
                new Point3D(pts[7]),
                new Point3D(pts[15]),
                new Point3D(pts[12]),
                new Point3D(pts[13])
            }));
            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[7]),
                new Point3D(pts[8]),
                new Point3D(pts[17]),
                new Point3D(pts[14]),
                new Point3D(pts[15])
            }));
            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[8]),
                new Point3D(pts[9]),
                new Point3D(pts[19]),
                new Point3D(pts[16]),
                new Point3D(pts[17])
            }));
            Polygons.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[9]),
                new Point3D(pts[5]),
                new Point3D(pts[11]),
                new Point3D(pts[18]),
                new Point3D(pts[19])
            }));

            UpdateCenter();
        }
        public string Save()
        {
            string res = "";
            foreach (var poly in Polygons)
            {
                foreach (var point in poly.Points)
                {
                    res += Math.Truncate(point.X) + " ";
                    res += Math.Truncate(point.Y) + " ";
                    res += Math.Truncate(point.Z) + " ";
                }
                res += '\n';
            }
            return res;
        }

        public void calculateZBuffer(Edge camera, int width, int height, out int[] buf, out int[] colors)
        {
            buf = new int[width * height];
            for (int i = 0; i < width * height; ++i)
                buf[i] = int.MinValue;
            colors = new int[width * height];
            for (int i = 0; i < width * height; ++i)
                colors[i] = 255;

            int color = 0;
            foreach (var f in Polygons)
            {
                color = (color + 30) % 255;
                // треугольник
                Point3D P0 = new Point3D(f.Points[0]);
                Point3D P1 = new Point3D(f.Points[1]);
                Point3D P2 = new Point3D(f.Points[2]);
                help(camera, P0, P1, P2, buf, width, height, colors, color);
                // 4
                if (f.Points.Count > 3)
                {
                    P0 = new Point3D(f.Points[2]);
                    P1 = new Point3D(f.Points[3]);
                    P2 = new Point3D(f.Points[0]);
                    help(camera, P0, P1, P2, buf, width, height, colors, color);
                }
                // 5
                if (f.Points.Count > 4)
                {
                    P0 = new Point3D(f.Points[3]);
                    P1 = new Point3D(f.Points[4]);
                    P2 = new Point3D(f.Points[0]);
                    help(camera, P0, P1, P2, buf, width, height, colors, color);
                }
            }

            int min_v = int.MaxValue;
            int max_v = 0;
            for (int i = 0; i < width * height; ++i)
            {
                if (buf[i] != int.MinValue && buf[i] < min_v)
                    min_v = buf[i];
                if (buf[i] > max_v)
                    max_v = buf[i];
            }
            if (min_v < 0)
            {
                min_v = -min_v;
                max_v += min_v;
                for (int i = 0; i < width * height; ++i)
                    if (buf[i] != int.MinValue)
                        buf[i] = (buf[i] + min_v) % int.MaxValue;
            }
            for (int i = 0; i < width * height; ++i)
                if (buf[i] == int.MinValue)
                    buf[i] = 255;
                else if (max_v != 0) buf[i] = buf[i] * 225 / max_v;
        }

        private void help(Edge camera, Point3D P0, Point3D P1, Point3D P2, int[] buff, int width, int height, int[] colors, int color)
        {
            PointF p0 = P0.make_perspective();
            PointF p1 = P1.make_perspective();
            PointF p2 = P2.make_perspective();

            if (p1.Y < p0.Y)
            {
                Point3D tmpp = new Point3D(P0);
                P0.X = P1.X; P0.Y = P1.Y; P0.Z = P1.Z;
                P1.X = tmpp.X; P1.Y = tmpp.Y; P1.Z = tmpp.Z;
                PointF tmppp = new PointF(p0.X, p0.Y);
                p0.X = p1.X; p0.Y = p1.Y;
                p1.X = tmppp.X; p1.Y = tmppp.Y;
            }
            if (p2.Y < p0.Y)
            {
                Point3D tmpp = new Point3D(P0);
                P0.X = P2.X; P0.Y = P2.Y; P0.Z = P2.Z;
                P2.X = tmpp.X; P2.Y = tmpp.Y; P2.Z = tmpp.Z;
                PointF tmppp = new PointF(p0.X, p0.Y);
                p0.X = p2.X; p0.Y = p2.Y;
                p2.X = tmppp.X; p2.Y = tmppp.Y;
            }
            if (p2.Y < p1.Y)
            {
                Point3D tmpp = new Point3D(P1);
                P1.X = P2.X; P1.Y = P2.Y; P1.Z = P2.Z;
                P2.X = tmpp.X; P2.Y = tmpp.Y; P2.Z = tmpp.Z;
                PointF tmppp = new PointF(p1.X, p1.Y);
                p1.X = p2.X; p1.Y = p2.Y;
                p2.X = tmppp.X; p2.Y = tmppp.Y;
            }

            drawRectangle(camera, P0, P1, P2, buff, width, height, colors, color);
        }

        private void drawRectangle(Edge camera, Point3D P0, Point3D P1, Point3D P2, int[] buff, int width, int height, int[] colors, int color)
        {
            PointF p0 = P0.make_perspective();
            PointF p1 = P1.make_perspective();
            PointF p2 = P2.make_perspective();

            // y0 <= y1 <= y2
            int y0 = (int)p0.Y; int x0 = (int)p0.X; int z0 = (int)P0.Z;
            int y1 = (int)p1.Y; int x1 = (int)p1.X; int z1 = (int)P1.Z;
            int y2 = (int)p2.Y; int x2 = (int)p2.X; int z2 = (int)P2.Z;

            var x01 = Interpolate(y0, x0, y1, x1);
            var x12 = Interpolate(y1, x1, y2, x2);
            var x02 = Interpolate(y0, x0, y2, x2);

            var h01 = Interpolate(y0, z0, y1, z1);
            var h12 = Interpolate(y1, z1, y2, z2);
            var h02 = Interpolate(y0, z0, y2, z2);

            // Конкатенация коротких сторон
            int[] x012 = x01.Take(x01.Length - 1).Concat(x12).ToArray();
            int[] h012 = h01.Take(h01.Length - 1).Concat(h12).ToArray();

            // Определяем, какая из сторон левая и правая
            int m = x012.Length / 2;
            int[] x_left, x_right, h_left, h_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                h_left = h02;
                h_right = h012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                h_left = h012;
                h_right = h02;
            }

            // Отрисовка горизонтальных отрезков
            for (int y = y0; y <= y2; ++y)
            {
                int x_l = x_left[y - y0];
                int x_r = x_right[y - y0];
                int[] h_segment;

                h_segment = Interpolate(x_l, h_left[y - y0], x_r, h_right[y - y0]);
                for (int x = x_l; x <= x_r; ++x)
                {
                    int z = h_segment[x - x_l];

                    int xx = x + width / 2;
                    int yy = -y + height / 2;
                    if (z > buff[xx * height + yy])
                    {
                        buff[xx * height + yy] = (int)(z);
                    }
                }
            }          
        }

        static int[] Interpolate(int i0, int d0, int i1, int d1)
        {
            if (i0 == i1)
            {
                return new int[] { d0 };
            }
            int[] values = new int[i1 - i0 + 1];
            float a = (float)(d1 - d0) / (i1 - i0);
            float d = d0;
            int ind = 0;
            for (int i = i0; i <= i1; ++i)
            {
                values[ind] = (int)(d);
                d = d + a;
                ++ind;
            }
            return values;
        }

        public void calc_gouraud(Edge camera, int width, int height, out float[] intensive, Point3D light)
        {
            int[] buf = new int[width * height];
            for (int i = 0; i < width * height; ++i)
                buf[i] = int.MinValue;
            intensive = new float[width * height];
            for (int i = 0; i < width * height; ++i)
                intensive[i] = 0;

            create_map(camera, light);
            foreach (var f in Polygons)
            {
                // треугольник
                Point3D P0 = new Point3D(f.Points[0]);
                Point3D P1 = new Point3D(f.Points[1]);
                Point3D P2 = new Point3D(f.Points[2]);
                float i_p0 = point_to_intensive[P0], i_p1 = point_to_intensive[P1], i_p2 = point_to_intensive[P2];
                G_magic(camera, P0, P1, P2, buf, width, height, intensive, i_p0, i_p1, i_p2);
                // 4
                if (f.Points.Count > 3)
                {
                    P0 = new Point3D(f.Points[2]);
                    P1 = new Point3D(f.Points[3]);
                    P2 = new Point3D(f.Points[0]);
                    i_p0 = point_to_intensive[P0]; i_p1 = point_to_intensive[P1]; i_p2 = point_to_intensive[P2];
                    G_magic(camera, P0, P1, P2, buf, width, height, intensive, i_p0, i_p1, i_p2);
                }
                // 5
                if (f.Points.Count > 4)
                {
                    P0 = new Point3D(f.Points[3]);
                    P1 = new Point3D(f.Points[4]);
                    P2 = new Point3D(f.Points[0]);
                    i_p0 = point_to_intensive[P0]; i_p1 = point_to_intensive[P1]; i_p2 = point_to_intensive[P2];
                    G_magic(camera, P0, P1, P2, buf, width, height, intensive, i_p0, i_p1, i_p2);
                }
            }

            SortedSet<float> test = new SortedSet<float>();
            for (int i = 0; i < width * height; ++i)
                test.Add(intensive[i]);
        }

        private void G_magic(Edge camera, Point3D P0, Point3D P1, Point3D P2, int[] buff, int width, int height, float[] colors, float c_P0, float c_P1, float c_P2)
        {
            // сортируем p0, p1, p2: y0 <= y1 <= y2
            PointF p0 = P0.make_perspective();
            PointF p1 = P1.make_perspective();
            PointF p2 = P2.make_perspective();

            if (p1.Y < p0.Y)
            {
                Point3D tmpp = new Point3D(P0);
                P0.X = P1.X; P0.Y = P1.Y; P0.Z = P1.Z;
                P1.X = tmpp.X; P1.Y = tmpp.Y; P1.Z = tmpp.Z;
                PointF tmppp = new PointF(p0.X, p0.Y);
                p0.X = p1.X; p0.Y = p1.Y;
                p1.X = tmppp.X; p1.Y = tmppp.Y;
                var tmpc = c_P1;
                c_P1 = c_P0;
                c_P0 = tmpc;
            }
            if (p2.Y < p0.Y)
            {
                Point3D tmpp = new Point3D(P0);
                P0.X = P2.X; P0.Y = P2.Y; P0.Z = P2.Z;
                P2.X = tmpp.X; P2.Y = tmpp.Y; P2.Z = tmpp.Z;
                PointF tmppp = new PointF(p0.X, p0.Y);
                p0.X = p2.X; p0.Y = p2.Y;
                p2.X = tmppp.X; p2.Y = tmppp.Y;
                var tmpc = c_P2;
                c_P2 = c_P0;
                c_P0 = tmpc;
            }
            if (p2.Y < p1.Y)
            {
                Point3D tmpp = new Point3D(P1);
                P1.X = P2.X; P1.Y = P2.Y; P1.Z = P2.Z;
                P2.X = tmpp.X; P2.Y = tmpp.Y; P2.Z = tmpp.Z;
                PointF tmppp = new PointF(p1.X, p1.Y);
                p1.X = p2.X; p1.Y = p2.Y;
                p2.X = tmppp.X; p2.Y = tmppp.Y;
                var tmpc = c_P1;
                c_P1 = c_P2;
                c_P2 = tmpc;
            }

            G_DrawFilledTriangle(camera, P0, P1, P2, buff, width, height, colors, c_P0, c_P1, c_P2);
        }

        private void G_DrawFilledTriangle(Edge camera, Point3D P0, Point3D P1, Point3D P2, int[] buff, int width, int height, float[] colors, float c_P0, float c_P1, float c_P2)
        {
            PointF p0 = P0.make_perspective();
            PointF p1 = P1.make_perspective();
            PointF p2 = P2.make_perspective();

            //y0 <= y1 <= y2
            int y0 = (int)p0.Y; int x0 = (int)p0.X; int z0 = (int)P0.Z;
            int y1 = (int)p1.Y; int x1 = (int)p1.X; int z1 = (int)P1.Z;
            int y2 = (int)p2.Y; int x2 = (int)p2.X; int z2 = (int)P2.Z;

            var x01 = Interpolate(y0, x0, y1, x1);
            var x12 = Interpolate(y1, x1, y2, x2);
            var x02 = Interpolate(y0, x0, y2, x2);

            var h01 = Interpolate(y0, z0, y1, z1);
            var h12 = Interpolate(y1, z1, y2, z2);
            var h02 = Interpolate(y0, z0, y2, z2);

            var c01 = Interpolate(y0, (int)(c_P0 * 100), y1, (int)(c_P1 * 100));
            var c12 = Interpolate(y1, (int)(c_P1 * 100), y2, (int)(c_P2 * 100));
            var c02 = Interpolate(y0, (int)(c_P0 * 100), y2, (int)(c_P2 * 100));
            // Конкатенация коротких сторон
            int[] x012 = x01.Take(x01.Length - 1).Concat(x12).ToArray();
            int[] h012 = h01.Take(h01.Length - 1).Concat(h12).ToArray();
            int[] c012 = c01.Take(c01.Length - 1).Concat(c12).ToArray();

            // Определяем, какая из сторон левая и правая
            int m = x012.Length / 2;
            int[] x_left, x_right, h_left, h_right, c_left, c_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                h_left = h02;
                h_right = h012;

                c_left = c02;
                c_right = c012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                h_left = h012;
                h_right = h02;

                c_left = c012;
                c_right = c02;
            }

            // Отрисовка горизонтальных отрезков
            for (int y = y0; y <= y2; ++y)
            {
                int x_l = x_left[y - y0];
                int x_r = x_right[y - y0];
                int[] h_segment;
                int[] c_segment;
                // interpolation
                if (x_l > x_r)
                    continue;
                h_segment = Interpolate(x_l, h_left[y - y0], x_r, h_right[y - y0]);
                c_segment = Interpolate(x_l, c_left[y - y0], x_r, c_right[y - y0]);
                for (int x = x_l; x <= x_r; ++x)
                {
                    int z = h_segment[x - x_l];
                    float color = c_segment[x - x_l] / 100f;
                    // i, j, z - координаты в пространстве, в пикчербоксе x, y
                    //int xx = (x + width / 2) % width;
                    //int yy = (-y + height / 2) % height;
                    int xx = x + width / 2;
                    int yy = -y + height / 2;
                    if (xx < 0 || xx > width || yy < 0 || yy > height || (xx * height + yy) < 0 || (xx * height + yy) > (buff.Length - 1))
                        continue;
                    if (z > buff[xx * height + yy])
                    {
                        buff[xx * height + yy] = (int)(z + 0.5);
                        colors[xx * height + yy] = color;
                    }
                }
            }
        }

        private void create_map(Edge camera, Point3D light)
        {
            map = new Dictionary<Point3D, List<int>>(new Point3dComparer());
            point_to_normal = new Dictionary<Point3D, List<float>>(new Point3dComparer());
            point_to_intensive = new Dictionary<Point3D, float>(new Point3dComparer());
            for (int i = 0; i < Polygons.Count; ++i)
            {
                Polygons[i].FindNormal(Center, camera);
                var n = Polygons[i].Normal;
                foreach (var p in Polygons[i].Points)
                {
                    if (!map.ContainsKey(p))
                        map[p] = new List<int>();
                    map[p].Add(i);
                    if (!point_to_normal.ContainsKey(p))
                        point_to_normal[p] = new List<float>() { 0, 0, 0 };
                    point_to_normal[p][0] += n[0];
                    point_to_normal[p][1] += n[1];
                    point_to_normal[p][2] += n[2];
                }
            }
            float max = 0;
            foreach (var el in map)
            {
                var p = el.Key;
                var lenght = (float)Math.Sqrt(point_to_normal[p][0] * point_to_normal[p][0] + point_to_normal[p][1] * point_to_normal[p][1] + point_to_normal[p][2] * point_to_normal[p][2]);
                point_to_normal[p][0] /= lenght;
                point_to_normal[p][1] /= lenght;
                point_to_normal[p][2] /= lenght;

                List<float> to_light = new List<float>() { -light.X + p.X, -light.Y + p.Y, -light.Z + p.Z };
                lenght = (float)Math.Sqrt(to_light[0] * to_light[0] + to_light[1] * to_light[1] + to_light[2] * to_light[2]);
                to_light[0] /= lenght; to_light[1] /= lenght; to_light[2] /= lenght;

                //ka - свойство материала воспринимать фоновое освещение, ia - мощность фонового освещения
                float ka = 1; float ia = 0.7f;
                float Ia = ka * ia;
                //kd - свойство материала воспринимать рассеянное освещение, id - мощность рассеянного освещения
                float kd = 0.7f; float id = 1f;
                float Id = kd * id * (point_to_normal[p][0] * to_light[0] + point_to_normal[p][1] * to_light[1] + point_to_normal[p][2] * to_light[2]);
                point_to_intensive[p] = Ia + Id;
                if (point_to_intensive[p] > max)
                    max = point_to_intensive[p];
            }
            //может ли быть больше 1?
            if (max != 0)
                foreach (var el in point_to_normal)
                {
                    point_to_intensive[el.Key] /= max;
                    if (point_to_intensive[el.Key] < 0)
                        point_to_intensive[el.Key] = 0;
                }
        }

        public class Point3dComparer : IEqualityComparer<Point3D>
        {
            public bool Equals(Point3D x, Point3D y)
            {
                return x.X.Equals(y.X) && x.Y.Equals(y.Y) && x.Z.Equals(y.Z);
            }

            public int GetHashCode(Point3D obj)
            {
                return obj.X.GetHashCode() + obj.Y.GetHashCode() + obj.Z.GetHashCode();
            }
        }
    }
}
