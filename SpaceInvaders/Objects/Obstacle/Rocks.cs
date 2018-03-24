using System;
using System.Windows.Forms;
using SpaceInvaders.Games;

namespace SpaceInvaders.Objects.Obstacle
{
    public class Rocks
    {
        #region - Vars -

        private Rock[] _rocks;
        private Timer _refreshTimer;
        
        private readonly Game _game;

        #endregion 

        #region - Constructor -

        /// <summary>
        /// Lässt Steine auf dem übergebenem Spielfeld erscheinen
        /// </summary>
        /// <param name="game"></param>
        public Rocks(Game game)
        {
            _game = game;
        }

        #endregion 

        #region - Create -

        /// <summary>
        /// Erstellt die Steine
        /// </summary>
        public void Create()
        {
            CreateRocks();

            _refreshTimer = new Timer();
            _refreshTimer.Interval = 1;
            _refreshTimer.Tick += RefreshRock;
            _refreshTimer.Start();
        }

        /// <summary>
        /// Erstellt Steine auf dem Spielfeld
        /// </summary>
        private void CreateRocks()
        {
            var locationX = (_game.ContainerWidth * 15) / 100;
            var locationY = (_game.ContainerHeight * 60) / 100;

            _rocks = new Rock[8];

            for (var i = 0; i < _rocks.Length; i++)
            {
                _rocks[i] = new Rock(_game);
                _rocks[i].Create(locationX, locationY);

                locationX += (_game.ContainerWidth * 8) / 100;

                if (i == 1 || i == 5)
                    locationX = (_game.ContainerWidth * 73) / 100;

                if (i == 3)
                {
                    locationX = (_game.ContainerWidth * 15) / 100;
                    locationY += 50;
                }
            }
        }

        /// <summary>
        /// Stopt das Zeichnen von den Steinen
        /// </summary>
        public void Stop()
        {
            _refreshTimer.Stop();
        }

        #endregion 

        #region - Set -

        /// <summary>
        /// Entfernt einen Stein vom Spielfeld
        /// </summary>
        /// <param name="indexOf"></param>
        public void Hitted(int indexOf)
        {
            _rocks[indexOf].Hitted();
        }

        #endregion 

        #region - Get -

        /// <summary>
        /// Gibt die Anzahl der Steine auf dem Spielfeld an
        /// </summary>
        public int Count
        {
            get
            {
                return _rocks.Length;
            }
        }

        /// <summary>
        /// Gibt die Höhe der Steine an
        /// </summary>
        public int GetHeight
        {
            get
            {
                return _rocks[0].GetHeight();
            }
        }

        /// <summary>
        /// Gibt die Breite der Steine an
        /// </summary>
        public int GetWidth
        {
            get
            {
                return _rocks[0].GetWidth();
            }
        }

        /// <summary>
        /// Gibt die Y Koordinate eines Steines an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int LocationY(int indexOf)
        {
            return _rocks[indexOf].LocationY;
        }

        /// <summary>
        /// Gibt die X Koordinate eines Steines an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int LocationX(int indexOf)
        {
            return _rocks[indexOf].LocationX;
        }

        /// <summary>
        /// Gibt an ob ein Stein zerstört wurde
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public bool Destroyed(int indexOf)
        {
            return _rocks[indexOf].Destoryed;
        }

        #endregion 

        #region - Event -

        /// <summary>
        /// Aktualisiert den Stein
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshRock(object sender, EventArgs e)
        {
            for(var i = 0; i < _rocks.Length; i++)
                _rocks[i].Refresh();
        }

        #endregion
    }
}
