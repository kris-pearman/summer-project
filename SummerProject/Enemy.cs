using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummerProject
{
    public class Enemy
    {
        public int X = 100;
        public int Y = 100;
        public int Width = 48;
        public int Height = 16;
        public string enemyType = "test";
        public Location Location
        {
            get
            {
                return new Location { X = X, Y = Y };
            }
        }

        public string SpriteLocation = "Test_graphics/dummy_player";
    }


}
