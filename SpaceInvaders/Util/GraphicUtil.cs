using SpaceInvaders.Games;
using System.Drawing;

namespace SpaceInvaders.Util
{
    public class GraphicUtil
    {
        #region - Vars -

        private readonly Game _game;

        #endregion

        #region -Constuctor

        /// <summary>
        /// Stellt eine Instanz des Objektes Game bereit
        /// </summary>
        /// <param name="game"></param>
        public GraphicUtil(Game game)
        {
            _game = game;
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Erstellt ein Rechteck
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle CreateRectangle(int x, int y, int width, int height)
        {
            return new Rectangle(x, y, width, height);
        }

        #endregion

        #region - Fill -

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rects"></param>
        /// <param name="color"></param>
        public void FillRectangles(Rectangle[] rects, Color color)
        {
            foreach (var rect in rects)
                _game.GetGraphic().FillRectangle(new SolidBrush(color), rect);
        }

        /// <summary>
        /// Malt das übergebene Rechteck auf dem Spielfeld
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public void FillRectangle(Rectangle rect, Color color)
        {
            _game.GetGraphic().FillRectangle(new SolidBrush(color), rect);
        }

        #endregion
    }
}
