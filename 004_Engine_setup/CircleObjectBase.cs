using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class CircleObjectBase : EasyDraw
{

    public ColliderManager engine;
    public Collider myCollider;
    protected Vec2 position;
    protected Vec2 oldPosition;
    public Vec2 velocity;
    protected int radius;
    protected float bounciness=0f;//0.2f
    protected Vec2 acceleration = new Vec2(0, 1f);//0,1
    protected float accelerationMultiplier = 1f;
    protected float teleporterCooldown=0;//500
    protected float lastTeleport = -100000;
    protected float maxSpeed=35f;

    public float _bounciness
    {
        get { return bounciness; }
    }

    protected float friction = 0.97f;
    public bool isMoving = true;
    public float Mass
    {
        get
        {
            return radius * radius * _density;
        }
    }
    protected float _density = 1;

    public CircleObjectBase(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius * 2 + 1, pRadius * 2 + 1)//pRadius * 2 + 1, pRadius * 2 + 1
    {

        isMoving= moving;
        radius = pRadius;
        position = pPosition;
        SetOrigin(radius, radius);
        myCollider = new Circle(this, position, radius);
        engine = ColliderManager.main;
        AddCollider();
        UpdateScreenPosition();
        Draw(230, 200, 0);
    }

    protected virtual void AddCollider() {
        engine.AddSolidCollider(myCollider);

    }

    protected virtual void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }

    protected virtual void Update()
    {
        //Console.WriteLine(velocity);
        //velocity *= friction;
        Move();
    }

    protected virtual void Move()//always call base.Move() before UpdateScreenPosition() in every class that inherits from this (if that object needs to interact
    {                                                                                                                          //with a rotating segment)
        List<Collider> overlaps = engine.GetOverlapsSolids(myCollider);

        foreach (Collider col in overlaps)
        {
            if (col.owner is Line)//corners dont work perfectly but good enought
            {

                Line line = (Line)col.owner;

                if (line.isRotating)
                {

                    Vec2 segmentVector = line.end - line.start;
                    Vec2 normal = segmentVector.Normal();
                    Vec2 differenceVector = myCollider.position - line.start;
                    float distance = differenceVector.Dot(normal);
                    Vec2 POI = myCollider.position + distance * normal;
                    Vec2 vec = POI - line.start;
                    float distFromStartToPOI = vec.Dot(segmentVector.Normalized());
                    if (0 <= distFromStartToPOI && distFromStartToPOI <= segmentVector.Length())
                    {

                        float newBounciness = bounciness * 2;
                        if (newBounciness < 0.4f)
                            newBounciness = 0.4f;
                        if (newBounciness > 0.8f)
                            newBounciness = 0.8f;
                        velocity = (normal * -6) * (2 * newBounciness);
                    }
                }

            }
        }


    }

    protected void UpdateScreenPosition()
    {
        oldPosition.SetXY(x, y);
        x = myCollider.position.x;
        y = myCollider.position.y;
        position.SetXY(x, y);
    }

    protected virtual void NewtonLawBalls(CircleObjectBase pOther, CollisionInfo pCol)
    {

        Vec2 relativeVelocity = velocity - pOther.velocity;
        if (relativeVelocity.Dot(pCol.normal) > 0) return;

        Vec2 centerOfMass = (Mass * this.velocity + pOther.Mass * pOther.velocity) / (Mass + pOther.Mass);
        velocity = velocity - (1 + bounciness) * ((velocity - centerOfMass).Dot(pCol.normal)) * pCol.normal;
        pOther.velocity = pOther.velocity - (1 + pOther._bounciness) * ((pOther.velocity - centerOfMass).Dot(pCol.normal)) * pCol.normal;
    }


}
