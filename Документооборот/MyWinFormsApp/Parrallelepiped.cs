using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Deform
{
    public class Parrallelepiped : AbstractFigure
    {
        public Parrallelepiped() : base() { }
        public Parrallelepiped(double h, double osn, TypeOfMetal m) : base(h, osn, m) { }

        public override void Calc()
        {
            this.deformHeight = Math.Round(this.origHeight - (this.origHeight / (double)this.material), 3);
            this.deformOsnov = Math.Round(Math.Sqrt(this.origHeight * Math.Pow(this.origOsnov, 2) / this.deformHeight), 3);
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
            int deformXStart = (pic.Width / 2) - (defOsnov / 2);

            int y_Start = (pic.Height / 2) - (height / 2);
            int x_Start = (pic.Width / 2) - (osnov / 2);

            //OrigCube
            Point C = new Point(x_Start, y_Start + height / 4);
            Point A = new Point(C.X + osnov / 3, y_Start);
            Point B = new Point(A.X + osnov, y_Start);
            Point D = new Point(B.X - osnov / 3, B.Y + height / 4);

            Point B2 = new Point(B.X, B.Y + height);
            Point D2 = new Point(D.X, D.Y + height);
            Point C2 = new Point(C.X, C.Y + height);

            myGraph.DrawLine(pen, A, B);
            myGraph.DrawLine(pen, B, D);
            myGraph.DrawLine(pen, D, C);
            myGraph.DrawLine(pen, A, C);

            myGraph.DrawLine(pen, B, B2);
            myGraph.DrawLine(pen, D, D2);
            myGraph.DrawLine(pen, C, C2);

            myGraph.DrawLine(pen, C2, D2);
            myGraph.DrawLine(pen, D2, B2);

            //DeformCube
            Point C2_d = new Point(deformXStart, C2.Y + defHeight / 8);
            Point C_d = new Point(C2_d.X, C2_d.Y - defHeight);
            Point D2_d = new Point(C2_d.X + defOsnov, C2_d.Y);
            Point D_d = new Point(D2_d.X, D2_d.Y - defHeight);

            Point B2_d = new Point(D2_d.X + osnov / 3, D2_d.Y - height / 4);
            Point B_d = new Point(B2_d.X, B2_d.Y - defHeight);
            Point A_d = new Point(C_d.X + osnov / 3, C_d.Y - height / 4);

            pen = new Pen(Color.Brown, 2);
            myGraph.DrawLine(pen, C2_d, C_d);
            myGraph.DrawLine(pen, C2_d, D2_d);
            myGraph.DrawLine(pen, D2_d, D_d);
            myGraph.DrawLine(pen, D_d, C_d);

            myGraph.DrawLine(pen, D_d, B_d);
            myGraph.DrawLine(pen, D2_d, B2_d);
            myGraph.DrawLine(pen, B_d, B2_d);

            myGraph.DrawLine(pen, C_d, A_d);
            myGraph.DrawLine(pen, A_d, B_d);

            if ((height < 110 & osnov < 120) | (osnov < 110 & height < 120))
                Draw(pic, scale + 0.1);

            List<Point> points = new List<Point>() { C, A, D, B, C2, D2, B2, C2_d, C_d, A_d, D_d, D2_d, B_d, B2_d };
            foreach (Point pro_point in points)
            {
                if (pro_point.X <= 0 | pro_point.Y <= 0 | pro_point.X >= pic.Width | pro_point.Y >= pic.Height)
                {
                    Draw(pic, scale - 0.001);
                    break;
                }
            }
        }
    }
}
