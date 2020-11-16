using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Windows.Forms;

namespace Lab9
{
    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, LINE };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };
    public enum Clipping { Clipp = 0, ZBuffer, Gouraud, Texture, Graph };

    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Axis rotateLineMode = 0;
        Polyhedron figure = null;
        int revertId = -1;

        Clipping clipping = 0;
        Camera camera = new Camera(50, 50);

        Color fill_color = Color.Red;
        byte[] rgbValuesTexture; // for picturebox and texture
        Bitmap texture;
        public Bitmap bmp;
        BitmapData bmpDataTexture; // for picturebox and texture
        byte[] rgbValues;
        public BitmapData bmpData;
        public IntPtr ptr; // pointer to the rgbValues
        public int bytes; // length of rgbValues

        Graphic Graph = null;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);


            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }

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
            else if (clipping == Clipping.Gouraud)
                show_gouraud();
            else if (clipping == Clipping.ZBuffer)
                show_z_buff();

            //camera.show(g, projection);
        }

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
                    else if (clipping == Clipping.Gouraud)
                        show_gouraud();
                    else if (clipping == Clipping.ZBuffer)
                        show_z_buff();
                    break;
                case 1:
                    //Hexahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Hexahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else if (clipping == Clipping.Gouraud)
                        show_gouraud();
                    else if (clipping == Clipping.ZBuffer)
                        show_z_buff();
                    break;
                case 2:
                    //Oktahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Octahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else if (clipping == Clipping.Gouraud)
                        show_gouraud();
                    else if (clipping == Clipping.ZBuffer)
                        show_z_buff();
                    break;
                case 3:
                    //Icosahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Icosahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else if (clipping == Clipping.Gouraud)
                        show_gouraud();
                    else if (clipping == Clipping.ZBuffer)
                        show_z_buff();
                    break;
                case 4:
                    //Dodecahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Dodecahedron();
                    if (clipping == 0)
                        figure.Show(g, projection);
                    else if (clipping == Clipping.Gouraud)
                        show_gouraud();
                    else if (clipping == Clipping.ZBuffer)
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
            else if (clipping == Clipping.Gouraud)
                show_gouraud();
            else if (clipping == Clipping.ZBuffer)
                show_z_buff();

            //camera.show(g, projection);
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
                case 2:
                    clipping = Clipping.Gouraud;
                    break;
                case 3:
                    clipping = Clipping.Texture;
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
            int[] colors = new int[pictureBox1.Width * pictureBox1.Height];

            figure.calculateZBuffer(camera.view, pictureBox1.Width, pictureBox1.Height, out buff, out colors);

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

        //GOURAUD
        private void show_gouraud()
        {
            float[] intensive = new float[pictureBox1.Width * pictureBox1.Height];

            figure.calc_gouraud(camera.view, pictureBox1.Width, pictureBox1.Height, out intensive, new Point3D(int.Parse(light_x.Text), int.Parse(light_y.Text), int.Parse(light_z.Text)));
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g.Clear(Color.White);

            for (int i = 0; i < pictureBox1.Width; ++i)
                for (int j = 0; j < pictureBox1.Height; ++j)
                {
                    Color c;
                    if (intensive[i * pictureBox1.Height + j] < 1E-6f)
                        c = Color.White;
                    else
                    {
                        float intsv = intensive[i * pictureBox1.Height + j];
                        if (intsv > 1)
                            intsv = 1;
                        c = Color.FromArgb((int)(fill_color.R * intsv) % 256, (int)(fill_color.G * intsv) % 256, (int)(fill_color.B * intsv) % 256);
                    }
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
            else if (clipping == Clipping.Gouraud)
                show_gouraud();
            else
                show_z_buff();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Graph = new Graphic(comboBox2.SelectedIndex);

            figure = Graph;

            g.Clear(Color.White);
            //Graph.isGraph = true;
            Graph.picture = pictureBox1;
            Graph.DrawGraphic();
        }
    }
}
