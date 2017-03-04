using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HW2
{
    // inhereits from the GameObject class to represent the player character
    class Collectible : GameObject
    {
        // attributes
        bool active;

        // default constructor
        public Collectible()
        {
            active = true;
        }

        // parameterized constructor
        public Collectible(int x, int y, int width, int height) : base(x, y, width, height)
        {
            active = true;
        }

        // properties
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        
    // methods
        // checks to determine if a collectible has collided with a player
        public bool CheckCollision(GameObject player)
        {
            // variables
            bool collide;

            // determine if collectible is available for pickup
            if(active == true)
            {
                if((player.Location).Intersects(this.Location)) // possible error location
                {
                    collide = true;
                }
                else
                {
                    collide = false;
                }
            }
            else
            {
                collide = false;
            }

            // return
            return collide;
        }

        // overrides draw method // checks to determine whether a collectible should be drawn, based on its active attribute
        public override void Draw(SpriteBatch sprite)
        {
            if(active == true)
            {
                base.Draw(sprite);
                sprite.Draw(CurTexture, Location, new Rectangle(0, 90, 30, 30), Color.White);
            }
        }
    }
}
