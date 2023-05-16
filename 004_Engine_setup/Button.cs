using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;
using TiledMapParser;

public class Button:AnimationSprite{

    Door door;
    public readonly int id;
    public ColliderManager engine;
    protected List<Line> colliders = new List<Line> { };

    bool setPosition;
    float secondX;
    Vec2 position;
    public bool enabled = false;

    public Button(string filename, int cols, int rows, TiledObject obj = null) : base(filename, cols, rows)
    {
        id=obj.GetIntProperty("id");
        
    }


    void SetPosition() {
        if (!setPosition) {
            
            setPosition = true;
            if (rotation == 90) {
                secondX = x - width / 2;
            }

            if (rotation == 270) { 
                secondX= x + width / 2;
            }
            position.SetXY(x, y);

            colliders.Add(new Line(new Vec2(x,y-height/2), new Vec2(secondX, y-height/2)));
            colliders.Add(new Line(new Vec2(x,y-height/2),new Vec2(x,y+height/2)));
            colliders.Add(new Line(new Vec2(secondX,y-height/2),new Vec2(secondX,y+height/2)));
            colliders.Add(new Line(new Vec2(x,y+height/2),new Vec2(secondX,y+height/2)));
            foreach (Line line in colliders)
            {
                line.SetOwner(this);
                parent.AddChild(line);
            }

            engine = ColliderManager.main;
            
        }
    }

    void Update() { 
        SetPosition();
    }

    public void SetDoor(Door pDoor) { 
        door=pDoor;
    }

    public void StartMovingDoor() {
        if (!enabled) {
            door.enabled = true;
            enabled = true;
            new Sound("Gate_Open.wav").Play();
        }
            
    }
}
