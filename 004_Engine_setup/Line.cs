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
    public Vec2 start;
    public Vec2 end;
    Vec2 rotationOrigin;
    public readonly bool isRotating=false;
    public readonly int rotationEachFrame=1;

    public uint lineWidth = 1;

    ColliderManager engine;
    List<Physics.Collider> colliders = new List<Physics.Collider> { };

    public Line(Vec2 pStart, Vec2 pEnd,bool pIsRotating=false):base(2000,2000)//its 1500 1500 just to make sure it works for the rotating lines
    {
        isRotating= pIsRotating;
        start = pStart;
        end = pEnd;
        rotationOrigin = (start + end) / 2;
        Draw();
        colliders.Add(new Physics.LineSegment(this, start, end));
        colliders.Add(new Physics.LineSegment(this, end, start));
        colliders.Add(new Physics.Circle(this, start,0));
        colliders.Add(new Physics.Circle(this, end, 0));
        engine = ColliderManager.main;
        foreach(Physics.Collider col in colliders)
            engine.AddSolidCollider(col);
    }

    void Draw()
    {
        Clear(Color.Empty);
        StrokeWeight(0);//was 0
        Line(start.x,start.y,end.x,end.y);

    }

    void Rotate() {
        start.RotateAroundDegrees(rotationOrigin, rotationEachFrame);
        end.RotateAroundDegrees(rotationOrigin, rotationEachFrame);

        foreach (Physics.Collider col in colliders) {
            if (col is Circle)
            {
                col.position.RotateAroundDegrees(rotationOrigin, rotationEachFrame);
            }
            else {
                
                ((LineSegment)col).start.RotateAroundDegrees(rotationOrigin, rotationEachFrame);
                ((LineSegment)col).end.RotateAroundDegrees(rotationOrigin, rotationEachFrame);
            }
        }

        Draw();
    }

    void Update() {
       
        if (isRotating)
            Rotate();

    }

}
