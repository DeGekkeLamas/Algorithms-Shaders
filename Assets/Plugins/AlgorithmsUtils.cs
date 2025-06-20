using UnityEngine;

public class AlgorithmsUtils
{
    
    public static bool Intersects(RectInt a, RectInt b)
    {
        return a.xMin <= b.xMax &&
               a.xMax >= b.xMin &&
               a.yMin <= b.yMax &&
               a.yMax >= b.yMin;
    }

    public static RectInt Intersect(RectInt a, RectInt b)
    {
        int x = Mathf.Max(a.xMin, b.xMin);
        int y = Mathf.Max(a.yMin, b.yMin);
        int width = Mathf.Min(a.xMax, b.xMax) - x;
        int height = Mathf.Min(a.yMax, b.yMax) - y;
        
        return new RectInt(x, y, width, height);
    }
    
    public static void FillRectangle(int[,] array, RectInt area, int value)
    {
        for (int i = area.y; i < area.y + area.height; i++) //Height is row
        {
            for (int j = area.x; j < area.x + area.width; j++) //Width is column
            {
                array[i, j] = value;
            }
        }
    }
    
    public static void FillRectangleOutline(int[,] array, RectInt area, int value) 
    { 
        
        int endX = area.x + area.width - 1; //X is column
        int endY = area.y + area.height - 1; //Y is row

        // Draw top and bottom borders
        for (int x = area.x; x <= endX; x++)
        {
            array[area.y, x] = value;
            array[endY, x] = value;
        }

        // Draw left and right borders
        for (int y = area.y + 1; y < endY; y++)
        {
            array[y, area.x] = value;
            array[y, endX] = value;
        }
    }

    public static void DebugRectInt(RectInt rectInt, Color color, float duration = 0f, bool depthTest = false, float height = 0.01f)
    {
        DebugExtension.DebugBounds(new Bounds(new Vector3(rectInt.center.x, 0, rectInt.center.y), new Vector3(rectInt.width, height, rectInt.height)), 
                                    color, duration, depthTest);
    }
}
