using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class Collectable : AnimationSprite
{
    CircleObjectBase collectable;
    bool created = false;

    public Collectable(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        //alpha = 0.0f;

        Vec2 position = new Vec2(obj.X + obj.Width / 2, obj.Y + obj.Height / 2);

        collectable = new CircleObjectBase(width / 16, position, default, true, true);
    }


    public void PickUp()
    {
        Console.WriteLine("pickup collected");
        Destroy();
    }

    void Update()
    {
        if (created == false)
        {
            AddChild(collectable);
            created = true;
        }
    }
}
