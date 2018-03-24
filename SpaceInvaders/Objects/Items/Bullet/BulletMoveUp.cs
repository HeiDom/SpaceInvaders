using SpaceInvaders.Games;
using System;
using System.Drawing;

namespace SpaceInvaders.Objects.Items.Bullet
{
    public class BulletItem : Item
    {
        #region - Vars -

        private const double BulletInterval = .4;

        private readonly Game _game;
        private readonly Color _color;

        #endregion

        #region - Constructor - 

        /// <summary>
        /// Konstuktor des Kugelgeschwindigkeitsitem, base Klasse ist Item, nimmt Klasse Game entgegen
        /// </summary>
        /// <param name="game"></param>
        public BulletItem(Game game) : base(game)
        {
            _game = game;
            _color = Color.Orange;
        }

        #endregion

        #region - Create -

        /// <summary>
        /// Erstellt das Item auf dem Spielfeld
        /// </summary>
        public new void Start()
        {
            base.Start();
            SetColor(_color);
        }

        /// <summary>
        /// Stoppt das Generieren von Items
        /// </summary>
        public new void Stop()
        {
            base.Stop();
        }

        #endregion 

        #region - Event -

        /// <summary>
        /// Methode die ausgelöst wird wenn es eine Kollision mit dem Schiff gab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCollision(object sender, EventArgs e)
        {
            base.OnCollision(this, new EventArgs());
            _game.SetSpaceShipBulletInterval(BulletInterval);
        }

        #endregion
    }
}
