using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Lab7
{
    class Edge
    {
        public Point3D First { get; set; }
        public Point3D Second { get; set; }

        public Edge(Point3D p1, Point3D p2)
        {
            First = new Point3D(p1);
            Second = new Point3D(p2);
        }

        private List<PointF> make_perspective(int k = 1000)
        {
            List<PointF> res = new List<PointF>
            {
                First.make_perspective(k),
                Second.make_perspective(k)
            };

            return res;
        }

        private List<PointF> make_orthographic(Axis a)
        {
            List<PointF> res = new List<PointF>
            {
                First.make_orthographic(a),
                Second.make_orthographic(a)
            };
            return res;
        }

        private List<PointF> make_isometric()
        {
            List<PointF> res = new List<PointF>
            {
                First.make_isometric(),
                Second.make_isometric()
            };
            return res;
        }

        public void show(Graphics g, Projection pr = 0, Pen pen = null)
        {
            if (pen == null)
                pen = Pens.Black;

            List<PointF> pts;
            switch (pr)
            {
                case Projection.ISOMETRIC:
                    pts = make_isometric();
                    break;
                case Projection.ORTHOGR_X:
                    pts = make_orthographic(Axis.AXIS_X);
                    break;
                case Projection.ORTHOGR_Y:
                    pts = make_orthographic(Axis.AXIS_Y);
                    break;
                case Projection.ORTHOGR_Z:
                    pts = make_orthographic(Axis.AXIS_Z);
                    break;
                default:
                    pts = make_perspective();
                    break;
            }

            g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
        }

        public void translate(float x, float y, float z)
        {
            First.translate(x, y, z);
            Second.translate(x, y, z);
        }

        public void rotate(double angle, Axis a, Edge line = null)
        {
            First.rotate(angle, a, line);
            Second.rotate(angle, a, line);
        }

        public void scale(float kx, float ky, float kz)
        {
            First.Scale(kx, ky, kz);
            Second.Scale(kx, ky, kz);
        }
    }
}
