using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Deform
{
    public class Cylinder : AbstractFigure
    {
        public Cylinder() : base() { }

        public Cylinder(double h, double osn, TypeOfMetal m) : base(h, osn, m) { }

        public override void Calc()
        {
            this.deformHeight = Math.Round(this.origHeight - this.origHeight / (double)this.material, 3);
            this.deformOsnov = Math.Round(2 * Math.Sqrt(this.origHeight * Math.Pow((this.origOsnov * 0.5), 2) / this.deformHeight), 3);
        }

        public override void Draw(PictureBox pic, double scale)
        {
            Graphics myGraph = pic.CreateGraphics();
            myGraph.Clear(Color.White);
            Pen pen = new Pen(Color.Black);
            int height = Convert.ToInt32(origHeight * scale);
            int osnov = Convert.ToInt32(origOsnov * scale);
            int defHeight = Convert.ToInt32(deformHeight * scale);
            int defOsnov = Convert.ToInt32(deformOsnov * scale);

            int y_Start = (pic.Height / 2) - (height / 2);
            int x_Start = (pic.Width / 2) - (osnov / 2);
            int def_x_Start = (pic.Width / 2) - (defOsnov / 2);

            //orig
            Point A = new Point(x_Start, y_Start + osnov / 6);
            Point C = new Point(A.X, A.Y + height);
            Point B = new Point(A.X + osnov, A.Y);
            Point D = new Point(B.X, B.Y + height);

            myGraph.DrawEllipse(pen, x_Start, y_Start, osnov, osnov / 3);
            myGraph.DrawLine(pen, A, C);
            myGraph.DrawLine(pen, B, D);
            myGraph.DrawEllipse(pen, C.X, C.Y - osnov / 6, osnov, osnov / 3);

            //deform
            pen = new Pen(Color.Brown);
            Point C_d = new Point(def_x_Start, C.Y);
            Point A_d = new Point(C_d.X, C_d.Y - defHeight);
            Point D_d = new Point(C_d.X + defOsnov, C_d.Y);
            Point B_d = new Point(D_d.X, D_d.Y - defHeight);

            myGraph.DrawEllipse(pen, C_d.X, A_d.Y - defOsnov / 6, defOsnov, defOsnov / 3);
            myGraph.DrawLine(pen, A_d, C_d);
            myGraph.DrawLine(pen, B_d, D_d);
            myGraph.DrawEllipse(pen, C_d.X, C_d.Y - defOsnov / 6, defOsnov, defOsnov / 3);

            if ((height < 110 & osnov < 300) | (osnov < 110 & height < 300))
                Draw(pic, scale + 0.1);

            List<Point> points = new List<Point>() { C, A, D, B, C_d, A_d, D_d, B_d };
            foreach (Point pro_point in points)
            {
                if (pro_point.X <= 0 | pro_point.Y <= 0 | pro_point.X >= pic.Width | pro_point.Y >= pic.Height - defOsnov / 6)
                {
                    Draw(pic, scale - 0.01);
                    break;
                }
            }
        }
    }
}
