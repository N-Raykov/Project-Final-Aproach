using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using Physics;
using TiledMapParser;

public class Line:EasyDraw
{

    public GameObject Owner = null;
    public Vec2 start;
    public Vec2 end;
    public readonly Vec2 rotationOrigin;
    public readonly bool isRotating=false;
    public readonly int rotationEachFrame=1;
    protected byte r = 255;
    protected byte g = 255;
    protected byte b = 255;

    public uint lineWidth = 1;

    protected ColliderManager engine;
    protected List<Physics.Collider> colliders = new List<Physics.Collider> { };

    public Line(Vec2 pStart, Vec2 pEnd,bool pIsRotating=false):base(2000,2000)//its 1500 1500 just to make sure it works for the rotating lines
    {
        isRotating= pIsRotating;
        start = pStart;
        end = pEnd;
        rotationOrigin = (start + end) / 2;
        Draw(r,g,b);
        colliders.Add(new Physics.LineSegment(this, start, end));
        colliders.Add(new Physics.LineSegment(this, end, start));
        colliders.Add(new Physics.Circle(this, start,0));
        colliders.Add(new Physics.Circle(this, end, 0));
        engine = ColliderManager.main;
        foreach(Physics.Collider col in colliders)
            engine.AddSolidCollider(col);
    }

    // This is a destructor - call by garbage collector
    ~Line()
    {
        Console.WriteLine("GB removes line");
    }

    protected override void OnDestroy()
    {
        foreach(Physics.Collider col in colliders)
            engine.RemoveSolidCollider(col);
    }

    protected void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        StrokeWeight(0);//was 0
        Stroke(red, green, blue);
        Line(start.x,start.y,end.x,end.y);

    }

    public void Rotate(float pRotation) {
        start.RotateAroundDegrees(rotationOrigin, pRotation);
        end.RotateAroundDegrees(rotationOrigin, pRotation);

        foreach (Physics.Collider col in colliders) {
            if (col is Circle)
            {
                col.position.RotateAroundDegrees(rotationOrigin, pRotation);
            }
            else {
                
                ((LineSegment)col).start.RotateAroundDegrees(rotationOrigin, pRotation);
                ((LineSegment)col).end.RotateAroundDegrees(rotationOrigin, pRotation);
            }
        }
        Draw(r, g, b);
    }

    void Update() {
       
    }
    public void SetOwner(GameObject pGameObject) { 
        Owner= pGameObject;
    }

}
