using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class Wall : AnimationSprite
{
    Line line;
    Vec2 start;
    Vec2 end;
    bool created = false;

    public Wall(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        alpha = 0.0f;

        // Define the position of one corner of the rectangle
        float x1 = obj.X;
        float y1 = obj.Y;

        // Define the width and height of the rectangle
        float width = obj.Width;
        float height = obj.Height;

        // Define the angle of rotation of the rectangle in radians
        float angleInRadians = obj.Rotation * ((float)Math.PI/ 180);

        // Calculate the coordinates of the other three corners of the rectangle
        float sin = (float)Math.Sin(angleInRadians);
        float cos = (float)Math.Cos(angleInRadians);
        float x2 = x1 + width * cos;
        float y2 = y1 + width * sin;
        float x3 = x2 - height * sin;
        float y3 = y2 + height * cos;
        float x4 = x1 - height * sin;
        float y4 = y1 + height * cos;

        // Output the coordinates of the four corners
        //Console.WriteLine("Corner 1: ({0},{1})", x1, y1);
        //Console.WriteLine("Corner 2: ({0},{1})", x2, y2);
        //Console.WriteLine("Corner 3: ({0},{1})", x3, y3);
        //Console.WriteLine("Corner 4: ({0},{1})", x4, y4);

        start = new Vec2(x1, y1);
        end = new Vec2(x3, y3);
        line = new Line(start, end);
    }

    void Update()
    {
        if (created == false)
        {
            parent.AddChild(line);
            created = true;
        }
    }
}

