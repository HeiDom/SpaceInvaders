using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders.Util
{
    public class LabelUtil : Label
    {   
        /// <summary>
        /// Erstellt ein Label, mit dem Text und übergebener größe
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Label Create(string text, float size)
        {
            var lblStart = new Label();
            lblStart.Text = text;
            lblStart.AutoSize = true;
            lblStart.ForeColor = Color.Transparent;
            lblStart.Font = new Font(lblStart.Font.Name, lblStart.Font.Size + size, lblStart.Font.Style, lblStart.Font.Unit);

            return lblStart;
        }
    }
}
