using SpaceInvaders.Actors.Enemy;
using SpaceInvaders.Actors.Player;
using SpaceInvaders.Games.Info;
using SpaceInvaders.Objects.Items.Bullet;
using SpaceInvaders.Objects.Items.SpaceShip;
using SpaceInvaders.Objects.Obstacle;
using SpaceInvaders.Properties;
using SpaceInvaders.Util;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace SpaceInvaders.Games
{
    public class Game
    {
        #region - Vars -

        private bool _refreshShipLabel;

        private Label _lblContinue;
        private Label _lblGameState;

        private readonly Rocks _rocks;
        private readonly SpaceShip _ship;
        private readonly Timer _gameTimer;
        private readonly Infobar _infobar;
        private readonly Invaders _invaders;
        private readonly Stopwatch _itemWatch;
        private readonly BulletItem _bulletItem;
        private readonly MovespeedItem _movespeedItem;
        private readonly SpaceInvadersForm _container;

        #endregion

        #region - Contructor -

        /// <summary>
        /// Kontruktor der Klasse Game, nimmt die SpaceInvaderForm entgegen
        /// </summary>
        /// <param name="container"></param>
        public Game(SpaceInvadersForm container)
        {
            _container = container;

            _gameTimer = new Timer();
            _gameTimer.Interval = 1;
            _gameTimer.Tick += Play;

            _rocks = new Rocks(this);
            _ship = new SpaceShip(this);
            _infobar = new Infobar(this);
            _invaders = new Invaders(this);
            _bulletItem = new BulletItem(this);
            _movespeedItem = new MovespeedItem(this);

            _itemWatch = new Stopwatch();
        }

        #endregion

        #region - Game -

        /// <summary>
        /// Startet das Spiel
        /// </summary>
        public void Start()
        {
            _gameTimer.Start();
            _itemWatch.Start();
            _bulletItem.Start();
            _movespeedItem.Start();
            
            _ship.Create();
            _rocks.Create();
            _infobar.Create();
            _invaders.Create(3);
        }

        /// <summary>
        /// Beendet das Spiel
        /// </summary>
        private void Stop()
        {
            _rocks.Stop();
            _gameTimer.Stop();
            _bulletItem.Stop();
            _movespeedItem.Stop();
        }

        /// <summary>
        /// Hauptmethode des Spiels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play(object sender, EventArgs e)
        {
            FrameUtil.GetFrames();

            _ship.Refresh();
            if (HandleUserInput())
                _ship.Shoot();

            _invaders.Move();

            GameOver();
        }
        
        /// <summary>
        /// Prüft ob es eine Kollision zwischen Spieler und Gegner gab oder ob alle Gegern tot sind
        /// </summary>
        private void GameOver()
        {
            var countInvadersDeath = 0;
            var shipLocationY = _ship.LocationY;

            if (_refreshShipLabel)
                _infobar.UpdateSpaceShipLivesLabel(_ship.Lives);

            _refreshShipLabel = false;

            for (var i = 0; i < _invaders.Count; i++)
            {
                if (shipLocationY <= _invaders.GetLocationY(i) - _invaders.Width)
                {
                    Stop();
                    CreateGameStateLabel(false);
                }

                if (_invaders.InvaderDead(i))
                    countInvadersDeath++;
            }

            if (countInvadersDeath == _invaders.Count)
            {
                Stop();
                CreateGameStateLabel(true);
            }

            if (_ship.Alive)
                return;
            
            Stop();
            CreateGameStateLabel(false);
        }

        /// <summary>
        /// Startet das Spiel neu
        /// </summary>
        public void Restart()
        {
            _lblContinue.Dispose();
            _lblGameState.Dispose();
            _refreshShipLabel = true;
            _container.CreateGraphics().Clear(Color.Black);
            _ship.Destory();
            
            Start();
        }

        #endregion
                
        #region - Interaction -
        
        /// <summary>
        /// Überprüft die Eingabe des Users, gibt zurück ob eine Kugel geschossen werden soll
        /// </summary>
        /// <returns></returns>
        private bool HandleUserInput()
        {
            if (Keyboard.IsKeyDown(Key.Right))
                _ship.MoveRigth();

            if (Keyboard.IsKeyDown(Key.Left))
                _ship.MoveLeft();

            return Keyboard.IsKeyDown(Key.Space);
        }
        
        #endregion

        #region - Controls -
        
        /// <summary>
        /// Erstellt ein Label, wenn verloren oder gewonne wurde
        /// </summary>
        /// <param name="won"></param>
        private void CreateGameStateLabel(bool won)
        {
            
            
            if(won)
            {
                _lblContinue = LabelUtil.Create(Resources.Next, 25);
                _lblGameState = LabelUtil.Create(Resources.Win, 25);
            }
            else
            {
                _lblContinue = LabelUtil.Create(Resources.Again, 25);
                _lblGameState = LabelUtil.Create(Resources.Lost, 25);
            }

            _container.Controls.Add(_lblGameState);
            _container.Controls.Add(_lblContinue);

            var x = _container.Width / 2;
            var y = _container.Height / 2;
            x = x - (_lblGameState.Width / 2);

            _lblContinue.Location = new Point(x, y);
            _lblContinue.AutoSize = false;
            _lblContinue.Click += OnClick;
            _lblContinue.Size = _lblGameState.Size;
            _lblContinue.TextAlign = ContentAlignment.MiddleCenter;

            y = y - (_lblGameState.Height);

            _lblGameState.Location = new Point(x, y);
        }

        /// <summary>
        /// Setzt ein Control auf dem Spielfeld
        /// </summary>
        /// <param name="control"></param>
        public void AddControl(Control control)
        {
            _container.Controls.Add(control);
        }

        /// <summary>
        /// Gibt ein Grafikobjekt zurück
        /// </summary>
        /// <returns></returns>
        public Graphics GetGraphic()
        {
            return _container.CreateGraphics();
        }

        /// <summary>
        /// Entfernt ein Control vom Spielfeld
        /// </summary>
        /// <param name="control"></param>
        public void RemoveControl(Control control)
        {
            control.Dispose();
            _container.Controls.Remove(control);
        }

        /// <summary>
        /// Entfernt einen Invader vom Spielfeld
        /// </summary>
        /// <param name="indexOf"></param>
        public void RemoveInvader(int indexOf)
        {
            _invaders.Remove(indexOf);
            _infobar.UpdateScoreLabel();
        }

        /// <summary>
        /// Entfernt einen Stein vom Spielfeld
        /// </summary>
        /// <param name="indexOf"></param>
        public void RockHitted(int indexOf)
        {
            _rocks.Hitted(indexOf);
        }

        #endregion 
        
        #region - Set -

        /// <summary>
        /// Setzt den Zustand des Schiffes auf getroffen
        /// </summary>
        public void SetSpaceShipHitted()
        {
            _ship.Hitted();
            _refreshShipLabel = true;
        }

        /// <summary>
        /// Setzt die Bewegungsgeschwindigkeit des Schiffes
        /// </summary>
        /// <param name="moveSpeed"></param>
        public void SetSpaceShipMoveSpeed(int moveSpeed)
        {
            _ship.SetMoveSpeed(moveSpeed);
        }

        /// <summary>
        /// Setzt den Interval, des Schiffes, wie oft ein Kugel geschossen werden kann
        /// </summary>
        /// <param name="interval"></param>
        public void SetSpaceShipBulletInterval(double interval)
        {
            _ship.SetBulletInterval(interval);
        }

        #endregion 

        #region - Get -

        #region - SpaceShip -

        /// <summary>
        /// Gibt die Y Koordinate des Shiffes auf dem Spielfeld an
        /// </summary>
        public int SpaceShipLocationY
        {
            get
            {
                return _ship.LocationY;
            }
        }

        /// <summary>
        /// Gibt die X Koordinate des Shiffes auf dem Spielfeld an
        /// </summary>
        public int SpaceShipLocationX
        {
            get
            {
                return _ship.LocationX;
            }
        }

        /// <summary>
        /// Gibt die Höhe des Schiffes an
        /// </summary>
        public int SpaceShipHeight
        {
            get
            {
                return _ship.Height;
            }
        }

        /// <summary>
        /// Gibt die Breite des Schiffes an
        /// </summary>
        public int SpaceShipWidth
        {
            get
            {
                return _ship.Width;
            }
        }

        /// <summary>
        /// Gibt die Leben des Schiffes an
        /// </summary>
        public int SpaceShipLives
        {
            get
            {
                return _ship.Lives;
            }
        }

        #endregion 

        #region - Invaders - 

        /// <summary>
        /// Gibt die Höhe der Invaders an
        /// </summary>
        public int InvadersHeight
        {
            get 
            {
                return _invaders.Height;
            }
        }

        /// <summary>
        /// Gibt die breite der Invader an
        /// </summary>
        public int InvadersWidth
        {
            get
            {
                return _invaders.Width;
            }
        }

        /// <summary>
        /// Gibt die Anzahl der Invaders an
        /// </summary>
        public int InvadersCount
        {
            get
            {
                return _invaders.Count;
            }
        }

        /// <summary>
        /// Gibt die Y Koordinate eines Invaders an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int GetInvaderLocationY(int indexOf)
        {
            return _invaders.GetLocationY(indexOf);
        }

        /// <summary>
        /// Gibt die X Koordinate eines Invaders an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int GetInvaderLocationX(int indexOf)
        {
            return _invaders.GetLocationX(indexOf);
        }

        /// <summary>
        /// Gibt an ob ein Invader tot ist
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public bool GetInvaderDead(int indexOf)
        {
            return _invaders.InvaderDead(indexOf);
        }

        #endregion 

        #region - GameContainer -
        
        /// <summary>
        /// Gibt die Breite des Spielfeldes an
        /// </summary>
        public int ContainerWidth
        {
            get
            {
                return _container.Width;
            }
        }

        /// <summary>
        /// Gibt die Höhe des Spielfeldes an
        /// </summary>
        public int ContainerHeight
        {
            get
            {
                return _container.Height;
            }
        }

        #endregion

        #region - Rock - 

        /// <summary>
        /// Gibt die Anzahl der Steine auf dem Spielfeld an 
        /// </summary>
        public int RocksCount
        {
            get
            {
                return _rocks.Count;
            }
        }

        /// <summary>
        /// Gibt die Höhe der Steine an
        /// </summary>
        public int RocksHeight
        {
            get
            {
                return _rocks.GetHeight;
            }
        }

        /// <summary>
        /// Gibt die Breite der Steine an
        /// </summary>
        public int RocksWidth
        {
            get
            {
                return _rocks.GetWidth;
            }
        }

        /// <summary>
        /// Gibt die Y Koordinate eines Steines an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int RockLocationY(int indexOf)
        {
            return _rocks.LocationY(indexOf);
        }

        /// <summary>
        /// Gibt die X Koordinate eines Steines an
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int RockLocationX(int indexOf)
        {
            return _rocks.LocationX(indexOf);
        }

        /// <summary>
        /// Gibt an ob ein Stein zerstört wurde
        /// </summary>
        /// <param name="indexOf"></param>
        public bool RockDestoyed(int indexOf)
        {
            return _rocks.Destroyed(indexOf);
        }

        #endregion 

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick(object sender, EventArgs e)
        {
            Restart();
        }
    }
}
