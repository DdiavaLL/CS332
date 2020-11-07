using System.Collections.Generic;
using System.Drawing;

namespace Lab8
{
    class RotationFigure : Polyhedron
    {
        public List<Point3D> Points { get; }

        public RotationFigure(List<Point3D> points)
        {
            Points = new List<Point3D>(points);
        }

        public RotationFigure(List<Point3D> startPoints, Axis axis, int count)
        {
            Polygons = new List<Polygon>();
            List<Point3D> rotatedPoints = new List<Point3D>();
            float angle = 360f / count;

            foreach (var p in startPoints)
            {
                rotatedPoints.Add(new Point3D(p.X, p.Y, p.Z));
            }

            for (int i = 0; i < count; ++i)
            {
                foreach (var p in rotatedPoints)
                    p.rotate(angle, axis);

                for (int j = 1; j < startPoints.Count; ++j)
                {
                    Polygon p = new Polygon(
                                    new List<Point3D>()
                                    {
                                        new Point3D(startPoints[j - 1]),
                                        new Point3D(rotatedPoints[j - 1]),
                                        new Point3D(rotatedPoints[j]),
                                        new Point3D(startPoints[j])
                                    });

                    Polygons.Add(p);
                }

                foreach (var p in startPoints)
                    p.rotate(angle, axis);
            }



        }

        public new void Show(Graphics g, Projection pr = 0, Pen pen = null)
        {
            foreach (Polygon f in Polygons)
            {
                f.Show(g, pr, pen);
            }
        }

    }
}
