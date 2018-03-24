using SpaceInvaders.Games;
using SpaceInvaders.Properties;
using SpaceInvaders.Objects;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SpaceInvaders.Util;

namespace SpaceInvaders.Actors.Player
{
    public class SpaceShip
    {
        #region - Vars -

        private int _moveSpeed;

        private double _bulletShotInterval;

        private const int DefaultLives = 3;
        private const int DefaultResetTime = 5;
        private const int DefaultMoveSpeed = 3;
        private const double DefaultBullteShootInterval = 1;

        //private PictureBox _picBoxShip;
        private Rectangle[] _ship;
        private Timer _resetIntervalTimer;
        private Timer _resetMoveSpeedTimer;

        private readonly Game _game;
        private readonly Stopwatch _bulletWatch;
        private readonly Stopwatch _defaultIntervalTimer;
        private readonly Stopwatch _defaultMoveSpeedTimer;
        private readonly GraphicUtil _graphicUtil;

        #endregion

        #region - Contructor -

        /// <summary>
        /// Konstruktor des Schiffes, nimmt das Objekt Game entgegen
        /// </summary>
        /// <param name="game"></param>
        public SpaceShip(Game game)
        {
            _game = game;
            Alive = true;

            Lives = DefaultLives;
            _moveSpeed = DefaultMoveSpeed;

            _ship = new Rectangle[3];
            _bulletWatch = new Stopwatch();
            _graphicUtil = new GraphicUtil(game);
            _defaultIntervalTimer = new Stopwatch();
            _defaultMoveSpeedTimer = new Stopwatch();
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Erstellt eine Shiff auf der übergebenen Form
        /// </summary>
        public void Create()
        {
            _bulletWatch.Start();

            //_picBoxShip = new PictureBox();
            //_picBoxShip.Image = Resources.Ship;
            //_picBoxShip.Size = Resources.Ship.Size;

            CreateShip();

            //var y = (_game.ContainerHeight - _ship[0].Height) ;
            //var x = (_game.ContainerWidth / 2) - (_ship[0].Width / 2);

            //_ship[0].Location = new Point(x, y);

        }

        private void CreateShip()
        {
            int BlocWidth = 15;
            int BlocHeight = 15;

            int y = _game.ContainerHeight - BlocHeight * 4;
            int x = (_game.ContainerWidth / 2) - (BlocWidth * 3);

            _ship[0] = _graphicUtil.CreateRectangle(x, y, BlocWidth * 7, BlocHeight * 2);
            _ship[1] = _graphicUtil.CreateRectangle(_ship[0].X + (BlocWidth * 2), _ship[0].Y - BlocHeight, BlocWidth * 3, BlocHeight);
            _ship[2] = _graphicUtil.CreateRectangle(_ship[1].X + BlocWidth, _ship[1].Y - BlocHeight, BlocWidth, BlocHeight);
            _graphicUtil.FillRectangles(_ship, Color.Green);
        }

        #endregion 

        #region - Interaction -

        public void Refresh()
        {
            _graphicUtil.FillRectangles(_ship, Color.Green);
        }

        /// <summary>
        /// Bewegt das Shiff nach rechts
        /// </summary>
        public void MoveRigth()
        {
            _graphicUtil.FillRectangles(_ship, Color.Black);

            if ((_ship[0].Location.X + _ship[0].Width + 15) < _game.ContainerWidth)
                for(int i = 0; i < _ship.Length; i++)
                    _ship[i].Location = new Point(_ship[i].Location.X + _moveSpeed, _ship[i].Y);

            _graphicUtil.FillRectangles(_ship, Color.Green);
        }

        /// <summary>
        /// Bewegt das Schiff nach links
        /// </summary>
        public void MoveLeft()
        {
            _graphicUtil.FillRectangles(_ship, Color.Black);

            if (_ship[0].Location.X > 0)
                for(int i = 0; i < _ship.Length; i++)
                    _ship[i].Location = new Point(_ship[i].Location.X - _moveSpeed, _ship[i].Location.Y);

            _graphicUtil.FillRectangles(_ship, Color.Green);
        }

        /// <summary>
        /// Lässt das Schiff schießen
        /// </summary>
        public void Shoot()
        {
            if (_bulletWatch.Elapsed.TotalSeconds < _bulletShotInterval)
                return;

            _bulletWatch.Restart();
            new Bullet(_game);

            if (_bulletShotInterval == 0)
                _bulletShotInterval = 1;
        }

        /// <summary>
        /// Zerstört das Schiff und entfernt es
        /// </summary>
        public void Destory()
        {
            Alive = true;
            Lives = DefaultLives;
            _graphicUtil.FillRectangles(_ship, Color.Black);
        }

        #endregion

        #region - Set -

        /// <summary>
        /// Setzt den zustand von Alive auf false
        /// </summary>
        public void Hitted()
        {
            Lives += -1;
            
            if(Lives == 0)
                Alive = false;
        }

        /// <summary>
        /// Setzt den Interval wie oft ein Kugel geschossen werden kann, für eine bestimmte Zeit
        /// </summary>
        /// <param name="interval"></param>
        public void SetBulletInterval(double interval)
        {
            _bulletShotInterval = interval;

            _resetIntervalTimer = new Timer();
            _resetIntervalTimer.Tick += SetDefaultInterval;
            _resetIntervalTimer.Start();

            _defaultIntervalTimer.Start();
        }

        /// <summary>
        /// Setzt die MoveSpeed des Schiffes, für eine bestimmte Zeit
        /// </summary>
        /// <param name="moveSpeed"></param>
        public void SetMoveSpeed(int moveSpeed)
        {
            _moveSpeed = moveSpeed;
            
            _resetMoveSpeedTimer = new Timer();
            _resetMoveSpeedTimer.Tick += SetDefaultMoveSpeed;
            _resetMoveSpeedTimer.Start();

            _defaultMoveSpeedTimer.Start();
        }

        #endregion 

        #region - Get -

        /// <summary>
        /// Gibt an ob das Spaceshiff Lebendig ist
        /// </summary>
        public bool Alive { get; private set; }

        /// <summary>
        /// Gibt die Leben das Schiffes an
        /// </summary>
        public int Lives { get; private set; }

        /// <summary>
        /// Gibt die Breite des Schiffes an
        /// </summary>
        public int Width
        {
            get
            {
                return _ship[0].Width;
            }
        }

        /// <summary>
        /// Gibt die Höhre des Schiffes an
        /// </summary>
        public int Height
        {
            get
            {
                return _ship[0].Height * 2;
            }
        }

        /// <summary>
        /// Gibt die Y-Koordinate des Schiffes an
        /// </summary>
        public int LocationY
        {
            get
            {
                return _ship[0].Location.Y;
            }
        }

        /// <summary>
        /// Gibt die X-Koordinate des Schiffes an
        /// </summary>
        public int LocationX
        {
            get
            {
                return _ship[0].Location.X;
            }
        }

        #endregion

        #region - Event - 

        /// <summary>
        /// Setzt den Schussinterval automatisch zurück auf seinen Defaultwert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDefaultInterval(object sender, EventArgs e)
        {
            if (_defaultIntervalTimer.Elapsed.TotalSeconds < DefaultResetTime)
                return;

            _resetIntervalTimer.Stop();
            _resetIntervalTimer.Dispose();
            _defaultIntervalTimer.Reset();
            _bulletShotInterval = DefaultBullteShootInterval;
        }

        /// <summary>
        /// Event, dass die MoveSpeed auf den Defaultwert setzt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDefaultMoveSpeed(object sender, EventArgs e)
        {
            if (_defaultMoveSpeedTimer.Elapsed.TotalSeconds < DefaultResetTime)
                return;

            _resetMoveSpeedTimer.Stop();
            _resetMoveSpeedTimer.Dispose();
            _defaultMoveSpeedTimer.Reset();
            _moveSpeed = DefaultMoveSpeed;
        }

        #endregion 
    }
}
