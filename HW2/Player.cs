using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HW2
{
    // inhereits from the GameObject class to represent the player character
    class Player : GameObject
    {
        // attributes
        int levelScore; // current level's score
        int totalScore; // score across all levels

        // default constructor
        public Player()
        {
            levelScore = 0;
            totalScore = 0;
        }

        // parameterized constructor
        public Player(int x, int y, int width, int height) : base(x, y, width, height)
        {
            levelScore = 0;
            totalScore = 0;
        }

        // properties
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }
    }
}
