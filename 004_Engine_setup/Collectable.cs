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

    public Collectable(TiledObject obj = null) : base("Battery_new.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        //alpha = 0.0f;

        ((MyGame)game).ui.totalCollectables++;

        Vec2 position = new Vec2(obj.X + obj.Width / 2, obj.Y + obj.Height / 2);

        collectable = new CircleObjectBase(width / 16, position, default,false, true);
        
    }


    public void PickUp()
    {
        Console.WriteLine("pickup collected");
        ((MyGame)game).ui.collectablesCollected++;
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
