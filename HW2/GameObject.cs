using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    // stores a player or collectibles data
    public class GameObject // if breaks: class GameObject
    {
        // attributes
        Texture2D curTexture;
        Rectangle location;

        // default constructor
        public GameObject()
        {
            // default location
            location = new Rectangle(0, 0, 30, 30);
        }

        // parameterized constructor
        public GameObject(int x, int y, int width, int height)
        {
            location = new Rectangle(x, y, width, height);
        }

        // properties
        public Texture2D CurTexture
        {
            get { return curTexture; }
            set { curTexture = value; }
        }
        public Rectangle Location
        {
            get { return location; }
            set { location = value; }
        }
        public int XLocation
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public int YLocation
        {
            get { return location.Y; }
            set { location.Y = value; }
        }

        // draw method
        public virtual void Draw(SpriteBatch sprite)
        {
            sprite.Draw(curTexture, location, Color.White);
        }
    }
}
