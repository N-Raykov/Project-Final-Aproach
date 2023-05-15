using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;
using TiledMapParser;

public class Door:AnimationSprite{
    public readonly int id;
    Line line;

    bool addedCollider = false;
    public bool startMoving = false;
    int initialDuration = 120;
    int duration = 120;
    float distance = 256;
    public bool enabled = false;
    public Door(string filename, int cols, int rows, TiledObject obj = null) : base(filename, cols, rows){
        id = obj.GetIntProperty("id");

    }

    void Update(){

        AddCollider();
        Move();
    }

    void AddCollider() {
        if (!addedCollider) {
            line = new Line(new Vec2(x, y-height / 2), new Vec2(x,y+height / 2));
            line.SetOwner(this);
            parent.AddChild(line);
            addedCollider = true;
        }
    }


    void Move() {
        if (enabled) {
            if (duration > 0) {
                line.MoveColliders(new Vec2(0, -distance / initialDuration));
                y -= distance / initialDuration;
                duration--;
            }
        
        }
    }
}
