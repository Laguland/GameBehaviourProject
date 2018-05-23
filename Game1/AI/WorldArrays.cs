using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.AI
{
    static class WorldArrays
    {
        public static float[,] mapArray = new float[40, 30];
        public static float[,] walkableArray = new float[40, 30];

        // Generate map array
        // 1.0f ground
        // 0.0f empty
        public static void GenerateMapArray()
        {
            for (int x = 0; x<40;x++)
            {
                for(int y = 0; y<30;y++)
                {
                    if (x > 5 && x < 20 && y == 25)
                        mapArray[x, y] = 1.0f;

                    if (x > 3 && x < 10 && y == 15)
                        mapArray[x, y] = 1.0f;

                    if ( x > 15 && x < 35 && y == 10)
                        mapArray[x, y] = 1.0f;

                    if (x == 0 && y == 28)
                        mapArray[x, y] = 1.0f;

                    if (x == 39 && y == 28)
                        mapArray[x, y] = 1.0f;

                    if (x > 10 && x < 21 && y == 20)
                        mapArray[x, y] = 1.0f;

                    if(y == 29) // all botom tiles will be 1.0
                        mapArray[x, y] = 1.0f;
                    else
                        if(mapArray[x,y] != 1.0f)
                            mapArray[x, y] = 0.0f;
                }
            }
            GenerateWalkableArray();
        } 

        public static void GenerateWalkableArray()
        {
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    if (mapArray[x, y] == 1.0f)
                    {
                        walkableArray[x, y] = 0.0f;
                    }
                    else
                    {
                        walkableArray[x, y] = 1.0f;
                    }
                }
            }
        }
    }
}
