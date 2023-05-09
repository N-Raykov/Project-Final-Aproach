using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;


public class BoxTest:EasyDraw
{

    public ColliderManager engine;
    public BoxCollider myCollider;
    protected Vec2 position;
    bool moving = false;

    public BoxTest(Vec2 pPosition,bool pMoving=false):base(2000,2000,false){
        position= pPosition;
        myCollider = new BoxCollider(this, position, 50, 60);
        engine = ColliderManager.main;
        engine.AddSolidCollider(myCollider);
        Draw(255, 0, 0);
    }

    protected virtual void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        Fill(red, green, blue);
        Stroke(red, green, blue);
        BoxCollider col=(BoxCollider)myCollider;
        Line(col.corners[0, 0].x, col.corners[0, 0].y, col.corners[0, 1].x, col.corners[0, 1].y);
        Line(col.corners[0, 1].x, col.corners[0, 1].y, col.corners[1, 1].x, col.corners[1, 1].y);
        Line(col.corners[1, 1].x, col.corners[1, 1].y, col.corners[1, 0].x, col.corners[1, 0].y);
        Line(col.corners[0, 0].x, col.corners[0, 0].y, col.corners[1, 0].x, col.corners[1, 0].y);
    }


    void Update() {
        if (Input.GetKey(Key.A)) {
            myCollider.rotation--;
            Draw(255, 0, 0);
        }

        if (Input.GetKey(Key.D))
        {
            myCollider.rotation++;
            Draw(255, 0, 0);
        }

       

    }
}
