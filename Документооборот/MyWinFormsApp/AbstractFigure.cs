using System.Windows.Forms;

namespace Deform
{
    public abstract class AbstractFigure
    {
        public TypeOfMetal material;
        public double origHeight, origOsnov, deformHeight, deformOsnov;

        public AbstractFigure()
        {
            origHeight = 1;
            origOsnov = 1;
            deformHeight = 1;
            deformOsnov = 1;
        }

        public AbstractFigure(double h, double osn, TypeOfMetal m)
        {
            origHeight = h;
            origOsnov = osn;
            deformHeight = h;
            deformOsnov = osn;
            material = m;
        }

        public abstract void Calc();
        public abstract void Draw(PictureBox pic, double scale);
    }
}
