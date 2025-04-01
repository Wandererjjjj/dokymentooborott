using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Deform
{
    public class Cone : AbstractFigure
    {
        public Cone() : base() { }
        public Cone(double h, double osn, TypeOfMetal m) : base(h, osn, m) { }

        public override void Calc()
        {
            this.deformHeight = Math.Round(this.origHeight - this.origHeight / (double)this.material, 3);
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

            // Orig Cone
            Point A = new Point(x_Start + osnov / 2, y_Start);
            Point B = new Point(x_Start, y_Start + height);
            Point C = new Point(x_Start + osnov, y_Start + height);

            myGraph.DrawLine(pen, A, B);
            myGraph.DrawLine(pen, A, C);
            myGraph.DrawEllipse(pen, x_Start, y_Start + height - osnov / 6, osnov, osnov / 3);

            // Deform Cone
            pen = new Pen(Color.Brown, 2);
            Point A_d = new Point(def_x_Start + defOsnov / 2, y_Start + height - defHeight);
            Point B_d = new Point(def_x_Start, y_Start + height);
            Point C_d = new Point(def_x_Start + defOsnov, y_Start + height);

            myGraph.DrawLine(pen, A_d, B_d);
            myGraph.DrawLine(pen, A_d, C_d);
            myGraph.DrawEllipse(pen, def_x_Start, y_Start + height - defOsnov / 6, defOsnov, defOsnov / 3);

            if ((height < 110 & osnov < 300) | (osnov < 110 & height < 300))
                Draw(pic, scale + 0.1);

            List<Point> points = new List<Point>() { A, B, C, A_d, B_d, C_d };
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