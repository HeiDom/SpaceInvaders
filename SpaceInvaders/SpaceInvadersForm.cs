using SpaceInvaders.Menu;
using System.Windows.Forms;

namespace SpaceInvaders
{
    public partial class SpaceInvadersForm : Form
    {
        #region - Contructor -

        /// <summary>
        /// Init Form
        /// </summary>
        public SpaceInvadersForm()
        {
            InitializeComponent();
            
            var gameMenu = new GameMenu(this);
            gameMenu.Create();
        }
                
        #endregion
    }
}
