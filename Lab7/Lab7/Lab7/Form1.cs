using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Lab7
{
    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, LINE };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };
    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Axis rotateLineMode = 0;
        Polyhedron figure = null;
        int revertId = -1;

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
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
                    figure.Show(g, projection);
                    break;
                case 1:
                    //Hexahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Hexahedron();
                    figure.Show(g, projection);
                    break;
                case 2:
                    //Oktahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Octahedron();
                    figure.Show(g, projection);
                    break;
                case 3:
                    //Icosahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Icosahedron();
                    figure.Show(g, projection);
                    break;
                case 4:
                    //Dodecahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.Dodecahedron();
                    figure.Show(g, projection);
                    break;
                default:
                    break;
            }
        }

        private void RotateAroundLine()
        {
            Edge rotateLine = new Edge(
                                new Point3D(
                                    (float)numericUpDown11.Value,
                                    (float)numericUpDown12.Value,
                                    (float)numericUpDown13.Value),
                                new Point3D(
                                    (float)numericUpDown14.Value,
                                    (float)numericUpDown15.Value,
                                    (float)numericUpDown16.Value));

            float Ax = rotateLine.First.X, Ay = rotateLine.First.Y, Az = rotateLine.First.Z;
            figure.Translate(-Ax, -Ay, -Az);

            double angle = (double)numericUpDown10.Value;
            figure.Rotate(angle, rotateLineMode, rotateLine);

            figure.Translate(Ax, Ay, Az);

            g.Clear(Color.White);
            figure.Show(g, projection);
        }

        private void button4_Click(object sender, EventArgs e) => RotateAroundLine();

        private void button7_Click(object sender, EventArgs e)
        {

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
            figure.Show(g, projection);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            if (figure != null)
                figure.Show(g, projection);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (revertId == 0)
            {
                figure.reflectX();
                g.Clear(Color.White);
                figure.Show(g, projection);
            }
            else if (revertId == 1)
            {
                figure.reflectY();
                g.Clear(Color.White);
                figure.Show(g, projection);
            }
            else if (revertId == 2)
            {
                figure.reflectZ();
                g.Clear(Color.White);
                figure.Show(g, projection);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Graph Graph = new Graph((int)numericUpDown18.Value, (int)numericUpDown20.Value, (int)numericUpDown19.Value,
                (int)numericUpDown21.Value, (int)numericUpDown22.Value, comboBox6.SelectedIndex);
            figure = Graph;
            g.Clear(Color.White);
            Graph.Show(g, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            string fileText = System.IO.File.ReadAllText(filename);

            figure = new Polyhedron(fileText);
            g.Clear(Color.White);
            figure.Show(g, 0);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = saveFileDialog1.FileName;
            string text = "";

            if (figure != null)
            {
                text = figure.Save();
            }
            System.IO.File.WriteAllText(filename, text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) => projection = (Projection)comboBox2.SelectedIndex;

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) => revertId = comboBox3.SelectedIndex;

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotateLineMode = (Axis)comboBox4.SelectedIndex;
            if (comboBox4.SelectedIndex != 4)
            {
                numericUpDown11.Enabled = false;
                numericUpDown12.Enabled = false;
                numericUpDown13.Enabled = false;
                numericUpDown14.Enabled = false;
                numericUpDown15.Enabled = false;
                numericUpDown16.Enabled = false;
            }
        }
    }
}
