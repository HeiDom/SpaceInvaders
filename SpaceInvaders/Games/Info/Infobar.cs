using SpaceInvaders.Properties;
using SpaceInvaders.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders.Games.Info
{
    public class Infobar
    {
        #region - Vars -

        private int _score;
        private readonly int _points;

        private readonly Game _game;
        private Label _lblScore;
        private PictureBox[] _picBoxLives;

        #endregion
        
        #region - Constructor -

        /// <summary>
        /// Erstellt eine Infobar
        /// </summary>
        /// <param name="game"></param>
        public Infobar(Game game)
        {
            _game = game;
            _points = 20;
        }

        #endregion

        /// <summary>
        /// Erstellt die Infobar
        /// </summary>
        public void Create()
        {
            CreateLabelScore();
            CreateLableShipLives();
        }

        public void Restart()
        {
            
        }

        /// <summary>
        /// Erstellt ein Label mit dem Score des Spielers
        /// </summary>
        private void CreateLabelScore()
        {
            _lblScore = LabelUtil.Create(Resources.ScoreLabel + _score, 10);
            _game.AddControl(_lblScore);
        }

        /// <summary>
        /// Erstelt eine Label mit dem Leben des Schiffes
        /// </summary>
        private void CreateLableShipLives()
        {
            var lblLives = LabelUtil.Create(Resources.Lives, 10);

            _game.AddControl(lblLives);
            var x = _game.ContainerWidth * 77 / 100;

            lblLives.Location = new Point(x, 0);
            x += lblLives.Width;

            _picBoxLives = new PictureBox[_game.SpaceShipLives];
            for (var i = 0; i < _picBoxLives.Length; i++)
            {
                _picBoxLives[i] = new PictureBox();
                _picBoxLives[i].Image = Resources.ShipLive;
                _picBoxLives[i].Size = Resources.ShipLive.Size;
                _picBoxLives[i].Location = new Point(x, 0);

                _game.AddControl(_picBoxLives[i]);
                x += Resources.ShipLive.Size.Width;
            }
        }

        #region - Update - 

        /// <summary>
        /// Updated das Label für die Lebensanzeige des Schiffes
        /// </summary>
        /// <param name="lives"></param>
        public void UpdateSpaceShipLivesLabel(int lives)
        {
            switch(lives)
            {
                case 3:
                    for (var i = 0; i < _picBoxLives.Length; i++)
                    {
                        _picBoxLives[i].Image = Resources.ShipLive;
                        _picBoxLives[i].Size = Resources.ShipLive.Size;
                    }
                    break;
                case 2: _picBoxLives[lives].Image = new Bitmap(1, 1);
                        break;
                case 1: _picBoxLives[lives].Image = new Bitmap(1, 1);
                        break;
                case 0: _picBoxLives[lives].Image = new Bitmap(1, 1);
                        break;
            }
        }

        /// <summary>
        /// Updated das Label für die Punkteanzahl
        /// </summary>
        public void UpdateScoreLabel()
        {
            _score += _points;
            _lblScore.Text = Resources.ScoreLabel + _score;
        }

        #endregion 
    }
}
