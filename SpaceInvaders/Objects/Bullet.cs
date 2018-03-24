using SpaceInvaders.Games;
using SpaceInvaders.Util;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders.Objects
{
    public class Bullet
    {
        #region - Vars -

        private bool _calculated;

        private Rectangle _rectBullet;

        private const int Width = 5;
        private const int Height = 13;
        private const int DefaultBulletSpeed = 4;

        private readonly Game _game;
        private readonly Timer _moveTimer;
        private readonly GraphicUtil _graphicUtil;

        #endregion 

        #region - Contructor -

        /// <summary>
        /// Erstellt eine Kugel auf der übergebenen Form, bei dem übergebene Shiff
        /// </summary>
        /// <param name="game"></param>
        public Bullet(Game game)
        {
            _game = game;
            _graphicUtil = new GraphicUtil(game);
            CreateSpaceShipBullet();
            
            _moveTimer = new Timer();
            _moveTimer.Tick += (sender, e) => MoveShipBullet(sender, e);
            _moveTimer.Interval = 1;
            _moveTimer.Start();
        }

        /// <summary>
        /// Erstellt eine Kugel auf der übergebenen Form, bei einem übergebenen Invader
        /// </summary>
        /// <param name="game"></param>
        /// <param name="indexOf"></param>
        public Bullet(Game game, int indexOf)
        {
            _game = game;
            _graphicUtil = new GraphicUtil(game);
            CreateInvaderBullet(indexOf);

            _moveTimer = new Timer();
            _moveTimer.Tick += (sender, e) => MoveInvaderBullet(sender, e); // Geile sache, so kann man sender einem objekt übergeben
            _moveTimer.Interval = 1;
            _moveTimer.Start();
        }

        #endregion 

        #region - Create Bullet -
        
        /// <summary>
        /// Lässt bei dem Raumschiff eine Kugel erscheinen
        /// </summary>
        private void CreateSpaceShipBullet()
        {
            var y = _game.SpaceShipLocationY;
            var x = _game.SpaceShipLocationX + (_game.SpaceShipWidth / 2 - 5);

            _rectBullet = _graphicUtil.CreateRectangle(x, y, Width, Height);
            _graphicUtil.FillRectangle(_rectBullet, Color.Green);
        }

        /// <summary>
        /// Lässt bei einem Invader eine Kugel erscheinnen
        /// </summary>
        /// <param name="indexOf"></param>
        private void CreateInvaderBullet(int indexOf)
        {
            var y = _game.GetInvaderLocationY(indexOf) + _game.InvadersHeight;
            var x = _game.GetInvaderLocationX(indexOf) + (_game.InvadersWidth / 2) - 5;

            _rectBullet = _graphicUtil.CreateRectangle(x, y, Width, Height);
            _graphicUtil.FillRectangle(_rectBullet, Color.Red);
        }

        #endregion

        #region - Interaction -

        /// <summary>
        /// Bewegt die Kugel auf dem Spielfeld, zerstört einen Invader wenn Sie ihn trifft
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveShipBullet(object sender, EventArgs e)
        {
            _graphicUtil.FillRectangle(_rectBullet, Color.Black);
            
            _rectBullet.X = _rectBullet.Location.X;
            _rectBullet.Y = _rectBullet.Location.Y - BulletSpeed(); ;

            _graphicUtil.FillRectangle(_rectBullet, Color.Green);

            GetInvadersLocations();
        }

        /// <summary>
        /// Bewegt die Kugel des Invaders, zieht das Leben ab, wenn das Shiff getroffen wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveInvaderBullet(object sender, EventArgs e)
        {
            _graphicUtil.FillRectangle(_rectBullet, Color.Black);

            _rectBullet.X = _rectBullet.Location.X;
            _rectBullet.Y = _rectBullet.Location.Y + BulletSpeed(); ;

            _graphicUtil.FillRectangle(_rectBullet, Color.Red);

            GetShipLocation();
        }

        #endregion 

        #region - Calculate - 

        /// <summary>
        /// Berechnet die Fluggeschwindigkeit anhand der Frames, jewinger Frames es gibt, desto mehr Pixel muss die Kugel fliegen
        /// </summary>
        private int BulletSpeed()
        {
            double moveSpeed;
            var framesPercent = FrameUtil.CalculateMoveSpeed();

            if (framesPercent >= 1)
                _calculated = true;

            if (_calculated)
            {
                if (Math.Abs(framesPercent - 1.0F) < 0)
                    moveSpeed = 15;
                else if (framesPercent >= 100)
                    moveSpeed = DefaultBulletSpeed;
                else if (Math.Abs(framesPercent) < 0)
                    moveSpeed = DefaultBulletSpeed;
                else
                    moveSpeed = (DefaultBulletSpeed / framesPercent) * 100;
            }
            else
                moveSpeed = DefaultBulletSpeed;

            return Convert.ToInt32(moveSpeed);
        }

        /// <summary>
        /// Gibt an ob die Kugel auf dem Spielfeld eine Kollision mit einem anderen Objekt hat
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private bool DetectCollision(int x, int y, int height, int width)
        {
            var bulletLocationStartX = _rectBullet.Location.X;
            var bulletLocationEndX = _rectBullet.Location.X + _rectBullet.Width;

            var bulletLocationStartY = _rectBullet.Location.Y;
            var bulletLocationEndY = _rectBullet.Location.Y + _rectBullet.Height;

            return bulletLocationEndX >= x && bulletLocationEndY >= y && bulletLocationStartX <= x + width && bulletLocationStartY <= y + height;
        }

        /// <summary>
        /// Holt sich die Locations der Invader
        /// </summary>
        private void GetInvadersLocations()
        {
            var bulletLocationStartTop = _rectBullet.Location.Y;
            var bulletLocationEndTop = _rectBullet.Location.Y + _rectBullet.Height;

            var invadersLocationStartTop = _game.GetInvaderLocationY(21);
            var invadersLocationEndTop = _game.GetInvaderLocationY(21) + _game.InvadersHeight;

            if (bulletLocationStartTop >= _game.RockLocationY(0))
            {
                for (var i = 0; i < _game.RocksCount; i++)
                {
                    if (_game.RockDestoyed(i))
                        continue;

                    if (DetectCollision(_game.RockLocationX(i), _game.RockLocationY(i), _game.RocksHeight, _game.RocksWidth))
                    {
                        _game.RockHitted(i);
                        RemoveBullet();   
                    }
                }
            }

            //if (bulletLocationStartTop >= invadersLocationEndTop)
            //    return;

            for (var i = 0; i < _game.InvadersCount; i++)
            {
                if (_game.GetInvaderDead(i) || _game.GetInvaderLocationY(i) >= bulletLocationEndTop)
                    continue;

                if (DetectCollision(_game.GetInvaderLocationX(i), _game.GetInvaderLocationY(i), _game.InvadersWidth, _game.InvadersHeight))
                {
                    _game.RemoveInvader(i);
                    RemoveBullet();
                    break;
                }
            }

            if (_rectBullet.Location.Y < -5)
                RemoveBullet();
        }

        /// <summary>
        /// Holt sich die Location des Schiffes
        /// </summary>
        private void GetShipLocation()
        {
            if (_rectBullet.Location.Y + _rectBullet.Height >= _game.RockLocationY(0))
            {
                for (var i = 0; i < _game.RocksCount; i++)
                {
                    if (_game.RockDestoyed(i))
                        continue;

                    if (DetectCollision(_game.RockLocationX(i), _game.RockLocationY(i), _game.RocksHeight, _game.RocksWidth))
                    {
                        _game.RockHitted(i);
                        RemoveBullet();
                    }
                }
            }

            if (_rectBullet.Location.Y <= _game.SpaceShipLocationY)
                return;

            if (DetectCollision(_game.SpaceShipLocationX, _game.SpaceShipLocationY, _game.SpaceShipHeight, _game.SpaceShipWidth))
            {
                _game.SetSpaceShipHitted();
                RemoveBullet();
            }

            if (_rectBullet.Location.Y >= _game.ContainerHeight)
                RemoveBullet();
        }

        #endregion 

        #region - Remove -

        /// <summary>
        /// Entfernt die Kugel aus der Form
        /// </summary>
        private void RemoveBullet()
        {
            _moveTimer.Stop();
            _moveTimer.Dispose();
            _graphicUtil.FillRectangle(_rectBullet, Color.Black);
            _rectBullet.Y = _game.ContainerHeight;
        }

        #endregion

        #region - Get -

        /// <summary>
        /// Gibt die Y Koordinate der Kugel an
        /// </summary>
        /// <returns></returns>
        public int GetLocationY()
        {
            return _rectBullet.Location.Y;
        }

        /// <summary>
        /// Gibt X Koordinate der Kugel an
        /// </summary>
        /// <returns></returns>
        public int GetLocationX()
        {
            return _rectBullet.Location.X;
        }

        /// <summary>
        /// Gibt die Höhe der Kugel an
        /// </summary>
        public int GetHeight
        {
            get
            {
                return _rectBullet.Height;
            }
        }

        /// <summary>
        /// Gibt die Breite der Kugel an
        /// </summary>
        public int GetWidth
        {
            get
            {
                return _rectBullet.Width;
            }
        }

        #endregion
    }
}
