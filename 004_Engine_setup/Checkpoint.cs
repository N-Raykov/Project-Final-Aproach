using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class Checkpoint : AnimationSprite
{
    CircleObjectBase checkpoint;
    bool created = false;

    public Checkpoint(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        //alpha = 0.0f;

        Vec2 position = new Vec2(obj.X + obj.Width / 2, obj.Y + obj.Height / 2);
        Console.WriteLine(obj.Width/2);
        checkpoint = new CircleObjectBase((int)(obj.Width / 2), position, default, false, true);
        //alpha = 0f;
    }


    void Update()
    {
        if (created == false)
        {
            AddChild(checkpoint);
            created = true;
        }
    }
}
