using SpaceInvaders.Games;
using SpaceInvaders.Properties;
using SpaceInvaders.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders.Menu
{
    public class GameMenu
    {
        #region - Vars -

        private bool _gameStarted;

        private Label _lblStart;
        private Timer _refreshFrames;
        private readonly SpaceInvadersForm _container;

        #endregion

        #region - Contructor -

        /// <summary>
        /// Erstellt eine Menü auf der übergebenen Form
        /// </summary>
        /// <param name="container"></param>
        public GameMenu(SpaceInvadersForm container)
        {
            _container = container;
            ModifyContainer();
        }

        #endregion

        #region - Modify Game Container -

        /// <summary>
        /// Ändert Eigenschaften des Containers, wo das Spiel stattfindet
        /// </summary>
        private void ModifyContainer()
        {
            _container.MinimizeBox = false;
            _container.MaximizeBox = false;
            _container.KeyDown += OnKeyDown;
            _container.BackColor = Color.Black;
            _container.FormBorderStyle = FormBorderStyle.FixedSingle;

            _refreshFrames = new Timer();
            _refreshFrames.Tick += RefreshFrames;
            _refreshFrames.Start();
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Erstellt das GameMenü
        /// </summary>
        public void Create()
        {
            CreateLabel();
        }

        /// <summary>
        /// Erstellt ein Label in der mitte des Spielfeldes
        /// </summary>
        private void CreateLabel()
        {
            _lblStart = LabelUtil.Create(Resources.Start, 25);
            _container.Controls.Add(_lblStart);
            _lblStart.Click += lbl_Click;

            var x = _container.Width / 2;
            var y = _container.Height / 2;
            x = x - (_lblStart.Width / 2);
            y = y - (_lblStart.Height / 2);

            _lblStart.Location = new Point(x, y);
        }

        #endregion 

        #region - Start - 

        /// <summary>
        /// Startet das Spiel und entfernt alle Controls des Menüs
        /// </summary>
        private void Start()
        {
            _gameStarted = true;
            
            _lblStart.Dispose();
            _container.Controls.Remove(_lblStart);
            
            var game = new Game(_container);
            game.Start();            
        }

        #endregion 

        #region - Set -

        /// <summary>
        /// Setzt die Frames der Form
        /// </summary>
        private void SetFrames()
        {
            _container.Text = Resources.Titel + " " + FrameUtil.GetFrames();
        }

        #endregion 

        #region - Event -

        /// <summary>
        /// Startet das Spiel wenn der Spieler auf Start klickt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_Click(object sender, EventArgs e)
        {
            Start();
        }

        /// <summary>
        /// Prüft ob der User die Space Taste gedrückt hat, wenn ja Startet das Spiel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Space) && !_gameStarted)
                Start();
        }     
   
        /// <summary>
        /// Aktualisiert die Frames auf dem Spielfeld
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshFrames(object sender, EventArgs e)
        {
            SetFrames();
        }

        #endregion       
    }
}
