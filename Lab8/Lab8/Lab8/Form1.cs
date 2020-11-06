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

    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Polyhedron figure = null;

        public Form1()
        {
            InitializeComponent();
        }
    }
}
