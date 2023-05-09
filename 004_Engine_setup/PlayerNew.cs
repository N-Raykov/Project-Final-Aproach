using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class PlayerNew:EasyDraw
{

    float bounciness;
    int radius;
    public ColliderManager engine;
    public Collider myCollider;
    protected Vec2 position;
    protected Vec2 oldPosition;
    public Vec2 velocity;

    Vec2 acceleration = new Vec2(0, 1f);//0,1
    //Vec2 gravity = new Vec2(0, 0);
    float accelerationMultiplier = 1f;
    float speed = 7f;//6f

    int state=MOVE;
    const int MOVE = 1;
    const int JUMP = 2;
    const int FALL = 3;
    
    public PlayerNew(Vec2 pPosition, int pRadius) : base(2000,2000,false)
    {
        radius = pRadius;
        position = pPosition;
        SetOrigin(radius, radius);
        myCollider = new Circle(this, position, radius);
        engine = ColliderManager.main;
        engine.AddSolidCollider(myCollider);
        UpdateScreenPosition();
        Draw(230, 200, 0);

    }

    protected virtual void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }

    protected void UpdateScreenPosition()
    {
        oldPosition.SetXY(x, y);
        x = myCollider.position.x;
        y = myCollider.position.y;
        position.SetXY(x, y);
    }

    void Update() {
        HandleInput();

        switch (state){

            case MOVE:

                break;
            case JUMP:

                break;
            case FALL:

                break;


        }

        Move();
    
    }

    protected void Move()
    {

        bool repeat = true;
        int iteration = 0;
        while (repeat && iteration < 2)
        {
            repeat = false;

            oldPosition = position;
            position += velocity;
            CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity);
            if (colInfo != null)
            {
                if (colInfo.timeOfImpact < 0.01f) repeat = true;
                ResolveCollisions(colInfo);
            }
            iteration++;
        }

        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol)
    {
        state = MOVE;
        if (pCol.other.owner is Line)
        {

            Line segment = (Line)pCol.other.owner;
            if (segment.isRotating)
            {

                Vec2 tempVelocity = pCol.normal * speed;

                velocity -= tempVelocity;
                velocity.Reflect(bounciness, pCol.normal);
                velocity += tempVelocity;
                
                return;
            }
            else
            {
                velocity.Reflect(bounciness, pCol.normal);
                return;
            }

            
        }

        //if (pCol.other.owner is Enemy)
        //{
        //    velocity.Reflect(bounciness, pCol.normal);
        //}


        //if (pCol.other.owner is CircleMapObject)
        //{
        //    if (((CircleMapObject)pCol.other.owner).isMoving)
        //    {
        //        NewtonLawBalls((CircleMapObject)pCol.other.owner, pCol);
        //    }
        //    else
        //    {
        //        velocity.Reflect(bounciness, pCol.normal);
        //    }

        //}


    }

    void HandleInput()
    {
        Vec2 moveDirection = new Vec2(0, 0);

        if (Input.GetKey(Key.LEFT))
        {
            moveDirection = new Vec2(-1, 0);
        }
        if (Input.GetKey(Key.RIGHT))
        {
            moveDirection = new Vec2(1, 0);
        }

        if (Input.GetKey(Key.UP) && state==MOVE)
        {
            Console.WriteLine(1);
            state = JUMP;
            //state = JUMP;
            //moveDirection -= new Vec2(0, 10);
            //accelerationMultiplier = 0.5f;
            velocity -= new Vec2(0,20);
            
        }
        velocity.x = moveDirection.x * speed;
        velocity += acceleration * accelerationMultiplier;
        Console.WriteLine(velocity);



    }
}
