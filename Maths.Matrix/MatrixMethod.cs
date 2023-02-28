using System;
using System.Collections.Generic;
using System.Linq;

namespace Maths.Matrix
{
    public static class MatrixMethod
    {
        public static IEnumerable<(int x, int y)>  Generate((int x, int y) start, int xCount, int yCount)
        {
            return Enumerable.Range(0, xCount)
                .SelectMany(i => Enumerable.Range(0, yCount).Select(j => (i, j)))
                .Select(d => (start.x + d.i, start.y + d.j));
        }

        public static IEnumerable<(int x, int y)> GetRing((int x, int y) start, int width, int xOffset, int yOffset)
        {
            for(int i= width*-1; i< xOffset + width; i++)
            {
                for (int j = width * -1; j < yOffset + width; j++)
                {
                    if (i < xOffset && i >= 0)
                    {
                        if (j < yOffset && j >= 0)
                        {
                            continue;
                        }
                    }

                    yield return (start.x + i, start.y + j);
                }
            }
        }
    }
}
