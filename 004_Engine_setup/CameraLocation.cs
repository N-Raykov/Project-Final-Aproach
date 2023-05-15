using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;
using Physics;

using GXPEngine;

class CameraLocation : AnimationSprite
{
    bool created = false;
    Vec2 position;
    int index;
    float scale;


    public CameraLocation(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        alpha = 0.0f;

        index = obj.GetIntProperty("index", 0);
        scale = obj.GetFloatProperty("scale", 0);

        position = new Vec2(obj.X, obj.Y);

        name = obj.Name;
    }

    void Update()
    {
        if (created == false && index <= ((MyGame)game).cameraManager.cameraLocations.Count())
        {
            ((MyGame)game).cameraManager.cameraLocations.Insert(index, position);
            ((MyGame)game).cameraManager.cameraScales.Insert(index, scale);
            //((MyGame)game).cameraManager.camera.scale = scale;
            parent.AddChild(this);
            created = true;
        }
    }
}
