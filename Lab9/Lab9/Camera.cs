using System.Drawing;

namespace Lab9
{
    class Camera
    {
        public Edge view = new Edge(new Point3D(0, 0, 300), new Point3D(0, 0, 250));
        Polyhedron small_cube = new Polyhedron();
        public Edge rot_line { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Camera(int w, int h)
        {
            int camera_half_size = 5;
            small_cube.Hexahedron(camera_half_size);
            small_cube.Translate(view.First.X, view.First.Y, view.First.Z);
            set_rot_line();
            Width = w;
            Height = h;
        }

        public void set_rot_line(Axis a = Axis.AXIS_X)
        {
            Point3D p1, p2;
            p1 = new Point3D(view.First);
            switch (a)
            {
                case Axis.AXIS_Y:
                    p2 = new Point3D(p1.X, p1.Y + 10, p1.Z);
                    break;
                case Axis.AXIS_Z:
                    p2 = new Point3D(p1.X, p1.Y, p1.Z + 10);
                    break;
                default:
                    p2 = new Point3D(p1.X + 10, p1.Y, p1.Z);
                    break;
            }
            rot_line = new Edge(p1, p2);
        }

        public void show(Graphics g, Projection pr = 0, int x = 0, int y = 0, int z = 0, Pen pen = null)
        {
            pen = Pens.Red;
        }


        public void translate(float x, float y, float z)
        {
            view.translate(x, y, z);
            small_cube.Translate(x, y, z);
            rot_line.translate(x, y, z);
        }

        public void rotate(double angle, Axis a, Edge line = null)
        {
            view.rotate(angle, a, line);
            small_cube.Rotate(angle, a, line);
            rot_line.rotate(angle, a, line);
        }
    }
}
