using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Deform
{
    public class TrianglePrism : AbstractFigure
    {
        public TrianglePrism() : base() { }
        public TrianglePrism(double h, double osn, TypeOfMetal m) : base(h, osn, m) { }

        public override void Calc()
        {
            this.deformHeight = Math.Round(this.origHeight - (this.origHeight / (double)this.material), 3);
            this.deformOsnov = Math.Round(this.origOsnov * Math.Sqrt(this.origHeight / this.deformHeight), 3);
        }

        public override void Draw(PictureBox pic, double scale)
        {
            Graphics myGraph = pic.CreateGraphics();
            myGraph.Clear(Color.White);
            Pen pen = new Pen(Color.Black, 2);

            int height = Convert.ToInt32(origHeight * scale);
            int osnov = Convert.ToInt32(origOsnov * scale);
            int defHeight = Convert.ToInt32(deformHeight * scale);
            int defOsnov = Convert.ToInt32(deformOsnov * scale);

            int y_Start = (pic.Height / 2) - (height / 2);
            int x_Start = (pic.Width / 2) - (osnov / 2);
            int def_x_Start = (pic.Width / 2) - (defOsnov / 2);

            // Orig Prism
            int alt = Convert.ToInt32(osnov * Math.Sqrt(3) / 2 * 0.5);
            Point A = new Point(x_Start, y_Start + height);
            Point B = new Point(x_Start + osnov, y_Start + height);
            Point C = new Point(x_Start + osnov / 2, y_Start + height - alt);

            Point A2 = new Point(A.X, A.Y - height);
            Point B2 = new Point(B.X, B.Y - height);
            Point C2 = new Point(C.X, C.Y - height);

            myGraph.DrawPolygon(pen, new Point[] { A, B, C });
            myGraph.DrawPolygon(pen, new Point[] { A2, B2, C2 });
            myGraph.DrawLine(pen, A, A2);
            myGraph.DrawLine(pen, B, B2);
            myGraph.DrawLine(pen, C, C2);

            // Deform Prism
            pen = new Pen(Color.Brown, 2);
            int def_alt = Convert.ToInt32(defOsnov * Math.Sqrt(3) / 2 * 0.5);
            Point A_d = new Point(def_x_Start, y_Start + height);
            Point B_d = new Point(def_x_Start + defOsnov, y_Start + height);
            Point C_d = new Point(def_x_Start + defOsnov / 2, y_Start + height - def_alt);

            Point A2_d = new Point(A_d.X, A_d.Y - defHeight);
            Point B2_d = new Point(B_d.X, B_d.Y - defHeight);
            Point C2_d = new Point(C_d.X, C_d.Y - defHeight);

            myGraph.DrawPolygon(pen, new Point[] { A_d, B_d, C_d });
            myGraph.DrawPolygon(pen, new Point[] { A2_d, B2_d, C2_d });
            myGraph.DrawLine(pen, A_d, A2_d);
            myGraph.DrawLine(pen, B_d, B2_d);
            myGraph.DrawLine(pen, C_d, C2_d);

            if ((height < 110 & osnov < 300) | (osnov < 110 & height < 300))
                Draw(pic, scale + 0.1);

            List<Point> points = new List<Point>() { A, B, C, A2, B2, C2, A_d, B_d, C_d, A2_d, B2_d, C2_d };
            foreach (Point pro_point in points)
            {
                if (pro_point.X <= 0 | pro_point.Y <= 0 | pro_point.X >= pic.Width | pro_point.Y >= pic.Height)
                {
                    Draw(pic, scale - 0.01);
                    break;
                }
            }
        }
    }
}
