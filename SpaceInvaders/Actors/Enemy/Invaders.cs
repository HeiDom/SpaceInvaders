using SpaceInvaders.Games;
using SpaceInvaders.Objects;
using System;
using System.Diagnostics;

namespace SpaceInvaders.Actors.Enemy
{
    public class Invaders
    {
        #region - Vars -

        private bool _moveDirectionRight = true;

        private int _newLocationInvadersTop;
        
        private const int NumberOfInvadersInRow = 10;
        private const float MoveSpeedInterval = 1.2F;

        private Invader[] _invaders;

        private readonly Game _game;
        private readonly Stopwatch _moveTimer;
        private readonly Stopwatch _bulletShootTimer;

        #endregion 

        #region - Contructor -

        /// <summary>
        /// Erstellt Invaders auf der übergebenen Form
        /// </summary>
        /// <param name="game"></param>
        public Invaders(Game game)
        {
            _game = game;
            
            _moveTimer = new Stopwatch();
            _bulletShootTimer = new Stopwatch();
        }

        #endregion 

        #region - Create - 

        /// <summary>
        /// Erstellt die Invaders
        /// </summary>
        public void Create(int maxInvadersRows)
        {
            _moveTimer.Start();
            _bulletShootTimer.Start();

            CreateInvaders(maxInvadersRows);
        }

        /// <summary>
        /// Erstellt die Invaders
        /// </summary>
        /// <param name="maxInvadersRows"></param>
        private void CreateInvaders(int maxInvadersRows)
        {
            var countRowsDrawed = 0;
            var locationY = (_game.ContainerHeight * 8) / 100;
            var locationX = (_game.ContainerWidth * 25) / 100; //25.7 war genau die Mitte aber leider halt ein float (locationX = Convert.ToInt16((_game.ContainerWidth * 25.7) / 100);)
            var invadersCount = NumberOfInvadersInRow * maxInvadersRows;
            
            _invaders = new Invader[invadersCount];

            for (var i = 0; i < invadersCount; i++)
            {
                countRowsDrawed++;

                _invaders[i] = new Invader(_game);
                _invaders[i].Create(locationX, locationY);

                if (countRowsDrawed == NumberOfInvadersInRow)
                {
                    locationY += 50;
                    countRowsDrawed = 0;
                    locationX = (_game.ContainerWidth * 25) / 100;
                }
                else
                    locationX += (_game.ContainerWidth * 5) / 100;
            }

            _newLocationInvadersTop = _invaders[0].LocationY;
        }

        #endregion 

        #region - Interaction -

        /// <summary>
        /// Bewegt die Invaders innerhalb des Spielfeldes
        /// </summary>
        public void Move()
        {
            RefreshPosition();

            if (_moveTimer.Elapsed.TotalSeconds <= MoveSpeedInterval)
                return;

            if (_moveDirectionRight)
                MoveRight();
            else
                MoveLeft();

            ShootBullet();
            _moveTimer.Restart();
        }

        /// <summary>
        /// Prüft ob die Invaders am Rechten Rand sind, wenn nicht bewegen sie sich einen Schritt nach rechts, ansonsten wird ein Schritt nach unten 
        /// </summary>
        private void MoveRight()
        {
            var indexLastInavder = Count - 1;

            var moveRight = true;

            for (var i = 0; i < _invaders.Length; i++)
            {
                if ((_invaders[i].LocationX + Width) >= (_game.ContainerWidth - 75))
                {
                    moveRight = false;
                    break;
                }
            }

            if (moveRight)
            {
                var moveSpeed = 25;
                NewPosition(moveSpeed, 0);
            }
            else
            {
                var moveSpeedUp = 0;
                _moveDirectionRight = false;
                _newLocationInvadersTop += 30;

                NewPosition(moveSpeedUp, 30);
            }
        }

        /// <summary>
        /// Bewegt die Invaders nach Links solange sie nicht den Rand berühren, ansonsten bewegen sie sich nach unten und die Richtung wird gewechselt
        /// </summary>
        private void MoveLeft()
        {
            var moveLeft = true;

            for (var i = 0; i < _invaders.Length; i++)
            {
                if ((_invaders[i].LocationX - _invaders[i].Width) <= 0)
                {
                    if(!_invaders[i].Alive)
                        continue;

                    moveLeft = false;
                    break;
                }
            }
            
            if (moveLeft)
            {
                var moveSpeed = -25;
                NewPosition(moveSpeed, 0);
            }
            else
            {
                var moveSpeedUp = 0;
                _moveDirectionRight = true;
                _newLocationInvadersTop = _newLocationInvadersTop + 30;

                NewPosition(moveSpeedUp, 30);
            }
        }

        /// <summary>
        /// Berechnet die neuen Positionen der Invaders, anhand der Bewegeungsgeschwindigkeit
        /// </summary>
        /// <param name="moveSpeed"></param>
        /// <param name="moveUp"></param>
        private void NewPosition(int moveSpeed, int moveUp)
        {
            var countInvadersDrawed = 0;
            var oldLocationY = _newLocationInvadersTop;

            for (var i = 0; i < _invaders.Length; i++)
            {
                if (countInvadersDrawed == 10)
                {
                    countInvadersDrawed = 0;
                    _newLocationInvadersTop += 50;
                }

                countInvadersDrawed++;
                _invaders[i].Move(moveSpeed, moveUp);
            }

            _newLocationInvadersTop = oldLocationY;
        }

        /// <summary>
        /// Lässt eine Kugel auf dem übergebenen Form erscheinen, bei einem zufälligen Gegner
        /// </summary>
        public void ShootBullet()
        {
            if (_bulletShootTimer.Elapsed.TotalSeconds <= new Random().Next(2, 7))
                return;

            new Bullet(_game, NextInvaderShootIndex());
            _bulletShootTimer.Restart();
        }
        
        /// <summary>
        /// Wählt einen zufälligen Invader aus der schiessen soll, der ein freies Schussfeld hat
        /// </summary>
        public int NextInvaderShootIndex()
        {
            var index = 0;
            var nextInvader = true;

            while(nextInvader)
            {
                index = new Random().Next(0, Count - 1);

                if (InvaderDead(index)) 
                    continue;

                var indexOfInvaderLastRow = Count - InvadersInRow;

                if (index >= indexOfInvaderLastRow)
                    nextInvader = false;
                else
                {
                    if(InvaderDead(index + InvadersInRow))
                        nextInvader = false;
                }
            }

            return index;
        }

        /// <summary>
        /// Aktualisiert die Position des Invaders
        /// </summary>
        public void RefreshPosition()
        {
            foreach (var invader in _invaders)
            {
                if(invader.Alive)
                    invader.Refresh();
            }
        }
        
        #endregion

        #region - Get -

        /// <summary>
        /// Gibt die Breite der Invaders an
        /// </summary>
        public int Width
        {
            get
            {
                return _invaders[0].Width;
            }
        }

        /// <summary>
        /// Gibt die Höhe der Invaders an
        /// </summary>
        public int Height
        {
            get
            {
                return _invaders[0].Height;
            }
        }
        
        /// <summary>
        /// Gibt die Anzahl der Invaders an, die auf dem Spielfeld sind
        /// </summary>
        public int Count
        {
            get
            {
                return _invaders.Length;
            }
        }

        /// <summary>
        /// Gibt die Anzahl der Invaders in eine Row an
        /// </summary>
        public int InvadersInRow
        {
            get
            {
                return NumberOfInvadersInRow;
            }
        }

        /// <summary>
        /// Gibt die X-Koordinate eines Invaders zurück
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int GetLocationX(int indexOf)
        {
            return _invaders[indexOf].LocationX;
        }

        /// <summary>
        /// Gibt die Y-Koordinate eines Invaders zurück
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public int GetLocationY(int indexOf)
        {
            return _invaders[indexOf].LocationY;
        }

        /// <summary>
        /// Entfernt einen Invader vom Feld
        /// </summary>
        /// <param name="indexOf"></param>
        public void Remove(int indexOf)
        {
            _invaders[indexOf].Dead();
            _invaders[indexOf].Remove();
        }

        /// <summary>
        /// Gibt an ob der angefragt Invader am Leben ist
        /// </summary>
        /// <param name="indexOf"></param>
        /// <returns></returns>
        public bool InvaderDead(int indexOf)
        {
            return _invaders[indexOf].Alive == false;
        }

        #endregion
    }
}
