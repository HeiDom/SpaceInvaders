using System;
using System.Drawing;
using System.Threading;
using SpaceInvaders.Games;
using SpaceInvaders.Util;

namespace SpaceInvaders.Objects.Obstacle
{
    public class Rock
    {
        #region - Vars -
        
        private const int Hight = 35;
        private const int Width = 35;

        private Rectangle _recRock;
        private Rectangle[] _recLittleRocks;

        private readonly Color _color;
        private readonly Color _removeColor; 
        private readonly GraphicUtil _graphicUtil;

        #endregion

        #region - Constuctor -

        /// <summary>
        /// Stellt eine Instanze des Objektes Rock bereit
        /// </summary>
        /// <param name="game"></param>
        public Rock(Game game)
        {
            Lives = 3;
            Destoryed = false;
            _color = Color.Yellow;
            _removeColor = Color.Black;
            _graphicUtil = new GraphicUtil(game);
        }

        #endregion

        #region - Create - 

        /// <summary>
        /// Erstellt einen Stein
        /// </summary>
        public void Create(int x, int y)
        {
            _recRock = _graphicUtil.CreateRectangle(x, y, Width, Hight);
            _graphicUtil.FillRectangle(_recRock, _color);
        }

        /// <summary>
        /// Generiert neue Koordinaten für kleine Stein, wo vorher der Komplette Stein stand
        /// </summary>
        private void CreateLittleRocks(int countRocks)
        {
            if(_recLittleRocks == null)
                _graphicUtil.FillRectangle(_recRock, _removeColor);
            else
                _graphicUtil.FillRectangles(_recLittleRocks, _removeColor);

            _recLittleRocks = new Rectangle[countRocks];

            for (var i = 0; i < _recLittleRocks.Length; i++)
            {
                var locationX = RandomUtil.Generate(LocationX, LocationX + Width);
                var locationY = RandomUtil.Generate(LocationY, LocationY + Hight);
                
                _recLittleRocks[i] = _graphicUtil.CreateRectangle(locationX, locationY, 5, 5);
            }

            _graphicUtil.FillRectangles(_recLittleRocks, _color);
        }
        
        #endregion

        #region - Interact -

        /// <summary>
        /// Lässt den Stein in Einzelteile springen oder er verschwindet komplett
        /// </summary>
        private void Explode()
        {
            switch (Lives)
            {
                case 2: CreateLittleRocks(20);
                        break;
                case 1: CreateLittleRocks(8);
                        break;
                case 0: Remove();
                        break;
            }
        }

        #endregion

        #region - Set -

        /// <summary>
        /// Entfernt einen Stein vom Feld
        /// </summary>
        private void Remove()
        {
            Destoryed = true;
            _graphicUtil.FillRectangle(_recRock, _removeColor);
        }

        /// <summary>
        /// Aktualisiert den Stein
        /// </summary>
        public void Refresh()
        {
            if (Lives < 3)
                return; 

            _graphicUtil.FillRectangle(_recRock, _color);
        }

        /// <summary>
        /// Setzt einen treffer beim Stein
        /// </summary>
        public void Hitted()
        {
            Lives += -1;
            Explode();

            if (Lives == 0)
                Remove();
        }
        
        #endregion

        #region - Get -

        /// <summary>
        /// Gibt die höhe des Steines an
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            return Hight;
        }

        /// <summary>
        /// Gibt die Breite des Steines an
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            return Width;
        }

        /// <summary>
        /// Gibt die X-Koordinate an
        /// </summary>
        public int LocationX
        {
            get
            {
                return _recRock.Location.X;
            }
        }

        /// <summary>
        /// Gibt die Y-Koordinate an
        /// </summary>
        public int LocationY
        {
            get
            {
                return _recRock.Location.Y;
            }
        }

        /// <summary>
        /// Gibt an wie viele Leben der Stein hat
        /// </summary>
        public int Lives { get; private set; }

        /// <summary>
        /// Gibt an ob der Stein zersört ist
        /// </summary>
        public bool Destoryed { get; private set; }
        
        #endregion 
    }
}
