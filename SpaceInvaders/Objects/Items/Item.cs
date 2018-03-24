using SpaceInvaders.Events;
using SpaceInvaders.Games;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using SpaceInvaders.Util;

namespace SpaceInvaders.Objects.Items
{
    public abstract class Item
    {
        #region - Vars -

        public bool Destoryed { get; private set; }
        
        public int X { get; private set; }
        public int Y { get; private set; }

        private int _interval;

        private Timer _moveTimer;
        private Color _itemColor;
        private Rectangle _rectItem;
        private Stopwatch _intervalTimer;

        private const int Width = 30;
        private const int Height = 30;
        

        private readonly Game _game;
        private readonly Color _removeColor;
        private readonly GraphicUtil _graphicUtil;
        
        #endregion 

        #region - Konstruktor -

        /// <summary>
        /// Konstruktor der Items, wird erstellt auf dem Spielfeld
        /// </summary>
        /// <param name="game"></param>
        protected Item(Game game)
        {
            Y = 10;
            _game = game;
            Destoryed = true;
            _removeColor = Color.Black;
            _graphicUtil = new GraphicUtil(game);
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Startet den Timer für das Item, erstell im zälligen abständen ein Item
        /// </summary>
        protected void Start()
        {
            _interval = GenerateInterval();
            
            _intervalTimer = new Stopwatch();
            _intervalTimer.Start();

            _moveTimer = new Timer();
            _moveTimer.Interval = 1;
            _moveTimer.Tick += Move;
            _moveTimer.Start();
        }

        /// <summary>
        /// Stoppt das Generieren von Items
        /// </summary>
        protected void Stop()
        {
            _moveTimer.Stop();
            _intervalTimer.Stop();
        }

        /// <summary>
        /// Erstellt ein Item, am oberen Formrand an einer Zufälligen X-Koordinate
        /// </summary>
        private void Cretate()
        {
            Destoryed = false;

            X = RandomUtil.Generate(0, _game.ContainerWidth);

            if (_rectItem.IsEmpty)
                _rectItem = _graphicUtil.CreateRectangle(X, Y, Width, Height);

            _rectItem.Y = Y;
            _rectItem.X = X;
            _intervalTimer.Restart(); 
            _interval = GenerateInterval();

            _graphicUtil.FillRectangle(_rectItem, _itemColor);
        }

        /// <summary>
        /// Gibt einen zufälligen Intervall zurück
        /// </summary>
        /// <returns></returns>
        private int GenerateInterval()
        {
            const int maxInterval = 7;
            const int minInterval = 20;

            return RandomUtil.Generate(maxInterval, minInterval);
        }

        #endregion

        #region - Interaction -

        /// <summary>
        /// Bewegt das Item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move(object sender, EventArgs e)
        {
            if (_intervalTimer.Elapsed.TotalSeconds >= _interval)
                Cretate();

            if(Destoryed)
                return;

            _graphicUtil.FillRectangle(_rectItem, _removeColor);
            _rectItem.Y = _rectItem.Y + 3;
            _graphicUtil.FillRectangle(_rectItem, _itemColor);

            var itemY = _rectItem.Location.Y;
            var shipY = _game.SpaceShipLocationY - _game.SpaceShipHeight;

            if (shipY > itemY)
                return;

            if (DetectCollision(_game.SpaceShipLocationX, _game.SpaceShipLocationY, _game.SpaceShipHeight, _game.SpaceShipWidth))
                OnCollision(this, new CollidedEvntArgs());

            if (_rectItem.Location.Y >= _game.ContainerHeight)
                Remove();
        }

        #endregion 

        #region - Calculate -

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
            var itemLocationStartY = _rectItem.Location.Y;
            var itemLocationEndY = _rectItem.Location.Y + _rectItem.Height;

            var itemLocationStartX = _rectItem.Location.X;
            var itemLocationEndX = _rectItem.Location.X + _rectItem.Width;
            
            return itemLocationEndX >= x && itemLocationEndY >= y && itemLocationStartX <= x + width && itemLocationStartY <= y + height;
        }

        #endregion

        #region - Remove -

        /// <summary>
        /// Entfernt das Item vom Spielfeld
        /// </summary>
        private void Remove()
        {
            Destoryed = true;
            _graphicUtil.FillRectangle(_rectItem, _removeColor);
        }

        #endregion

        #region - Set - 

        /// <summary>
        /// Setzt die Farbe des Items
        /// </summary>
        /// <param name="color"></param>
        protected void SetColor(Color color)
        {
            _itemColor = color;
            _graphicUtil.FillRectangle(_rectItem, _itemColor);
        }

        #endregion 

        /// <summary>
        /// Event das aufgerufen wird, wenn es eine Kollision gab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnCollision(object sender, EventArgs e)
        {
            Remove();
        }

        /// <summary>
        /// Erstellt zufällig ein Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateItem(object sender, EventArgs e)
        {
        }
    }
}
