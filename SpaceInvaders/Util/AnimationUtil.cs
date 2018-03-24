using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders.Util
{
    public class AnimationUtil : Label
    {
        //public void AnimateLabel(Label label)
        //{
        //    for(var i = 0; i < label.Text.Length; i++)
        //    {
        //        string ch = label.Text.Substring(i);
        //    }
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            TextRenderer.DrawText(e.Graphics, "sd", Font, ClientRectangle, ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter); 


            //e.Graphics.DrawString(e.ToString().Substring(0), Font, new SolidBrush(ForeColor), new Point(100, 100), StringFormat.GenericTypographic);
            //e.Graphics.DrawString(e.ToString().Substring(1), new Font(Font.FontFamily, 200), new SolidBrush(ForeColor), 10, 10);

        //    string str = "abcdefgh";
 
        //    SolidBrush brush = new SolidBrush(Color.Red);
        //    e.Graphics.DrawString(str, this.Font, brush, new PointF(100.0f, 100.0f), StringFormat.GenericTypographic);
        }
    }
}
