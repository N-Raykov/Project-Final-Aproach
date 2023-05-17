using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class Ending : AnimationSprite
{
    CircleObjectBase collectable;
    bool created = false;

    public Ending(TiledObject obj = null) : base("Battery_new.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        alpha = 0.0f;

        Vec2 position = new Vec2(obj.X + obj.Width / 2, obj.Y + obj.Height / 2);

        collectable = new CircleObjectBase((int)obj.Width / 2, position, default, false, true);

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