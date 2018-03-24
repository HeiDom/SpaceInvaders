using System;

namespace SpaceInvaders.Events
{
    public delegate void CollidedEventHandler(object source, CollidedEvntArgs e);

    public class CollidedEvntArgs : EventArgs
    {
        public CollidedEvntArgs()
        {

        }
    }
}
