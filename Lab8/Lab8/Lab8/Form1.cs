using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab8
{
    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, LINE };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };
    public enum Clipping { Clipp = 0, ZBuffer };

    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Polyhedron figure = null;
        Axis rotateLineMode = 0;
        Clipping clipping = 0;

        Camera camera = new Camera(50, 50);

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }

        //TRANSLATION ROTATION SCALE
        private void button2_Click(object sender, EventArgs e)
        {
            if (figure == null)
            {
                MessageBox.Show("Неодбходимо выбрать фигуру!", "Ошибка!", MessageBoxButtons.OK);
            }
            else
            {
                //TRANSLATE
                int offsetX = (int)numericUpDown1.Value, offsetY = (int)numericUpDown2.Value, offsetZ = (int)numericUpDown3.Value;
                figure.Translate(offsetX, offsetY, offsetZ);

                //ROTATE
                int rotateAngleX = (int)numericUpDown4.Value;
                figure.Rotate(rotateAngleX, 0);

                int rotateAngleY = (int)numericUpDown5.Value;
                figure.Rotate(rotateAngleY, Axis.AXIS_Y);

                int rotateAngleZ = (int)numericUpDown6.Value;
                figure.Rotate(rotateAngleZ, Axis.AXIS_Z);

                //SCALE
                if (checkBox1.Checked)
                {
                    float old_x = figure.Center.X, old_y = figure.Center.Y, old_z = figure.Center.Z;
                    figure.Translate(-old_x, -old_y, -old_z);

                    float kx = (float)numericUpDown9.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown7.Value;
                    figure.Scale(kx, ky, kz);

                    figure.Translate(old_x, old_y, old_z);
                }
                else
                {
                    float kx = (float)numericUpDown9.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown7.Value;
                    figure.Scale(kx, ky, kz);
                }
            }

            g.Clear(Color.White);
            if (clipping == 0)
                figure.Show(g, projection);
            else
                show_z_buff();
        }

        //DRAW FIGURE
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //Tetrahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Tetrahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else
                        show_z_buff();
                    break;
                case 1:
                    //Hexahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Hexahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else
                        show_z_buff();
                    break;
                case 2:
                    //Oktahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Octahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else
                        show_z_buff();
                    break;
                case 3:
                    //Icosahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Icosahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else
                        show_z_buff();
                    break;
                case 4:
                    //Dodecahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Dodecahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else
                        show_z_buff();
                    break;
                default:
                    break;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotateLineMode = (Axis)comboBox4.SelectedIndex;
            switch (comboBox4.SelectedIndex)
            {
                case 0:
                    checkNumeric(false);
                    break;
                case 1:
                    checkNumeric(false);
                    break;
                case 2:
                    checkNumeric(false);
                    break;
                case 3:
                    checkNumeric(true);
                    break;
                default:
                    break;
            }
            void checkNumeric(bool flag)
            {
                numericUpDown10.Enabled = flag;
                numericUpDown11.Enabled = flag;
                numericUpDown12.Enabled = flag;
                numericUpDown13.Enabled = flag;
                numericUpDown14.Enabled = flag;
                numericUpDown15.Enabled = flag;
            }
        }

        //ROTATE AROUND LINE
        private void RotateAroundLine()
        {
            Edge rotateLine = new Edge(
                                new Point3D(
                                    (float)numericUpDown12.Value,
                                    (float)numericUpDown11.Value,
                                    (float)numericUpDown10.Value),
                                new Point3D(
                                    (float)numericUpDown15.Value,
                                    (float)numericUpDown14.Value,
                                    (float)numericUpDown13.Value));

            float Ax = rotateLine.First.X, Ay = rotateLine.First.Y, Az = rotateLine.First.Z;
            figure.Translate(-Ax, -Ay, -Az);

            double angle = (double)numericUpDown16.Value;
            figure.Rotate(angle, rotateLineMode, rotateLine);

            figure.Translate(Ax, Ay, Az);

            g.Clear(Color.White);
            if (clipping == 0)
                figure.Show(g, projection);
            else
                show_z_buff();
        }

        private void button4_Click(object sender, EventArgs e) => RotateAroundLine();

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox5.SelectedIndex)
            {
                case 0:
                    clipping = 0;
                    break;
                case 1:
                    clipping = Clipping.ZBuffer;
                    break;
                default:
                    clipping = 0;
                    break;
            }
        }

        //Z-BUFFER
        private void show_z_buff()
        {
            int[] buff = new int[pictureBox1.Width * pictureBox1.Height];

            figure.calculateZBuffer(pictureBox1.Width, pictureBox1.Height, out buff);

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;

            g.Clear(Color.White);

            for (int i = 0; i < pictureBox1.Width; ++i)
                for (int j = 0; j < pictureBox1.Height; ++j)
                {
                    Color c = Color.FromArgb(buff[i * pictureBox1.Height + j], buff[i * pictureBox1.Height + j], buff[i * pictureBox1.Height + j]);
                    bmp.SetPixel(i, j, c);
                }

            pictureBox1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<Point3D> points = new List<Point3D>();
            var lines = textBox1.Text.Split('\n');

            foreach (var p in lines)
            {
                var arr = ((string)p).Split(',');
                points.Add(new Point3D(float.Parse(arr[0]), float.Parse(arr[1]), float.Parse(arr[2])));
            }

            Axis axis = 0;
            switch (comboBox6.SelectedItem.ToString())
            {
                case "OX":
                    axis = 0;
                    break;
                case "OY":
                    axis = Axis.AXIS_Y;
                    break;
                case "OZ":
                    axis = Axis.AXIS_Z;
                    break;
                default:
                    axis = 0;
                    break;
            }

            RotationFigure rotateFigure = new RotationFigure(points, axis, (int)numericUpDown23.Value);

            figure = rotateFigure;

            g.Clear(Color.White);
            if (clipping == 0)
                rotateFigure.Show(g, 0);
            else
                show_z_buff();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (figure == null)
            {
                MessageBox.Show("Сначала создайте фигуру", "Нет фигуры", MessageBoxButtons.OK);
            }
            else
            {
                int dx = (int)numericUpDown22.Value,
                        dy = (int)numericUpDown21.Value,
                        dz = (int)numericUpDown20.Value;
                figure.Translate(-dx, -dy, -dz);

                camera.translate(dx, dy, dz);

                float old_x_camera = figure.Center.X,
                        old_y_camera = figure.Center.Y,
                        old_z_camera = figure.Center.Z;

                camera.translate(-old_x_camera, -old_y_camera, -old_z_camera);

                double angleOX = (int)numericUpDown19.Value;
                figure.Rotate(-angleOX, Axis.AXIS_X);
                camera.rotate(angleOX, Axis.AXIS_X);

                double angleOY = (int)numericUpDown18.Value;
                figure.Rotate(-angleOY, Axis.AXIS_Y);
                camera.rotate(angleOY, Axis.AXIS_Y);

                double angleOZ = (int)numericUpDown17.Value;
                figure.Rotate(-angleOZ, Axis.AXIS_Z);
                camera.rotate(angleOZ, Axis.AXIS_Z);

                camera.translate(old_x_camera, old_y_camera, old_z_camera);
            }

            g.Clear(Color.White);

            camera.show(g, projection);
            figure.Show(g, projection);
            if (clipping != 0)
                show_z_buff();
        }
    }
}
