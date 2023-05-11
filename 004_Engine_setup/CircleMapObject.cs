using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class CircleMapObject:CircleObjectBase{

    bool hadPortalCollisionThisFrame = false;

    public CircleMapObject(int pRadius, Vec2 pPosition, Vec2 pVelocity = new Vec2(), bool moving = true) : base(pRadius,pPosition)
    {

        isMoving = moving;
        Draw(230, 200, 0);
        _density = 2f;//10f
        friction = 0.8f;

    }
    protected override void Move() {

        if (velocity.y<=0)
            velocity.x *= friction;

        if (velocity.Length() > maxSpeed)
        {
            velocity = velocity.Normalized() * maxSpeed;
        }

        velocity += acceleration * accelerationMultiplier;

        bool repeat = true;
        int iteration = 0;
        float remainingMovement = 1;
        while (repeat && iteration < 2)
        {
            repeat = false;

            oldPosition = position;
            position += velocity;
            CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity * remainingMovement);
            if (colInfo != null)
            {
                //if (colInfo.timeOfImpact < 0.01f) 
                repeat = true;
                remainingMovement = 1 - colInfo.timeOfImpact; // take it from here...
                ResolveCollisions(colInfo);
            }
            iteration++;
        }


        List<Collider> overlaps = engine.GetOverlapsSolids(myCollider);

        foreach (Collider pCol in overlaps)
        {

            if (pCol.owner is Teleporter && !hadPortalCollisionThisFrame)
            {
                //Console.WriteLine(1);
                MyGame myGame = (MyGame)Game.main;
                Teleporter teleporter = (Teleporter)pCol.owner;
                if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
                {

                    myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                    lastTeleport = Time.time;
                    Console.WriteLine(velocity.Length());

                    velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;

                    //accelerationMultiplier = 0.75f;
                    Console.WriteLine(velocity.Length());
                }



            }
        }


        UpdateScreenPosition();
    }

    protected override void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        if (isMoving)
        {
            Fill(red, green, blue);
        }
        else {
            red = 255;
            green = 255;
            blue = 255;
            Fill(red, green, blue,0);
        }

        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }


    void ResolveCollisions(CollisionInfo pCol)
    {


        if (pCol.other.owner is Teleporter)
        {
            hadPortalCollisionThisFrame = true;
            MyGame myGame = (MyGame)Game.main;
            Teleporter teleporter = (Teleporter)pCol.other.owner;
            if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
            {

                myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                lastTeleport = Time.time;
                Console.WriteLine(velocity.Length());

                velocity = velocity.Length() * 1.1f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;

                //accelerationMultiplier = 0.75f;
                Console.WriteLine(velocity.Length());
            }else{
                velocity.Reflect(bounciness, pCol.normal);
            }

            return;

        }




        if (pCol.other.owner is Line)
        {
            
            Line segment = (Line)pCol.other.owner;
            if (segment.isRotating)
            {

                Vec2 tempVelocity = pCol.normal * 6;

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

        if (pCol.other.owner is BouncyFloor)
        {
            velocity.Reflect(1.1f, pCol.normal);
            return;
        }


        if (pCol.other.owner is Player)
        {
            NewtonLawBalls((Player)pCol.other.owner, pCol);
        }
        if (pCol.other.owner is CircleMapObject) {
            if (((CircleMapObject)pCol.other.owner).isMoving)
            {
                NewtonLawBalls((CircleMapObject)pCol.other.owner, pCol);
            }
            else {
                velocity.Reflect(bounciness,pCol.normal);
            }
        
        }


    }

    protected override void Update() {
        hadPortalCollisionThisFrame = false;
        base.Update();
    }

}
