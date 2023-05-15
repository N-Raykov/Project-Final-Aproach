using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using GXPEngine;
using GXPEngine.Core;
using Physics;

class LaserProjectile : CircleObjectBase
{

    GameObject owner;
    //public int bounces = 0;
    //public readonly int maxBounces = 1;
    float speed = 40f;
    public static int _radius = 5;
    int portalNumber = -1;
    bool hasTeleported = false;


    public static float _speed
    {
        get { return 10f; }//remember to also change speed
    }

    public LaserProjectile(Vec2 startPosition, Vec2 pVelocity, int pRadius, int pPortalNumber, GameObject pOwner = null) : base(pRadius, startPosition)
    {
        owner = pOwner;
        _radius = pRadius;
        velocity = pVelocity;
        bounciness = 1f;
        portalNumber = pPortalNumber;
        //Draw(255, 255, 255);
    }

    //protected void Draw(byte red, byte green, byte blue)
    //{
    //    Clear(Color.Empty);
    //    Fill(red, green, blue);
    //    Stroke(red, green, blue);
    //    Ellipse(radius, radius, 2 * radius, 2 * radius);

    //}

    protected override void AddCollider()
    {
        engine.AddTriggerCollider(myCollider);//might need to change back to solid

    }

    protected override void OnDestroy()
    {
        engine.RemoveTriggerCollider(myCollider);
    }

    protected override void Move()
    {
        //sprite.rotation = (velocity.GetAngleDegrees());
        CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity * speed);
        ResolveCollisions(colInfo);
    }

    void ResolveCollisions(CollisionInfo pCol)
    {

        if (pCol != null)
        {
            if (pCol.other is LineSegment)
            {
                if (pCol.other.owner is Line)
                {

                    Line line = (Line)pCol.other.owner;
                    if (line.Owner is LaserShooter && portalNumber != -1)
                    {
                        this.LateDestroy();
                        return;
                    }

                    if (line.Owner is Button)
                    {
                        if (portalNumber != -1)
                        {
                            this.LateDestroy();
                            return;
                        }
                        else
                        {

                            Button button = (Button)line.Owner;
                            button.StartMovingDoor();
                        }

                    }


                }



                if (!(pCol.other.owner is Teleporter))
                {
                    
                    {
                        if (owner is LaserShooter)
                        {
                            LaserShooter ls = (LaserShooter)owner;
                            ls.SetPortalNumber(-1);
                            if (!ls.laser1WasDrawn)
                            {
                                ls.DrawLaser1(myCollider.position);

                            }


                            if (ls.laser1WasDrawn)
                            {
                                if (ls.previousLaser2StartPos.x != ls.laser2StartPos.x || ls.previousLaser2StartPos.y != ls.laser2StartPos.y)
                                {
                                    ls.laser2WasDrawn = false;
                                }
                            }

                            if (ls.laser1WasDrawn && !ls.laser2WasDrawn && hasTeleported)
                            {
                                Console.WriteLine(ls.previousLaser2StartPos + " " + ls.laser2StartPos);
                                ls.DrawLaser2(ls.laser2StartPos, myCollider.position);
                            }



                        }

                        this.LateDestroy();
                    }
                }
                else
                {
                    if (owner is LaserShooter)
                    {
                        MyGame myGame = (MyGame)Game.main;
                        Teleporter teleporter = (Teleporter)pCol.other.owner;
                        LaserShooter ls = (LaserShooter)owner;



                        if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
                        {

                            ls.SetPortalNumber(teleporter.portalNumber);
                            myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            lastTeleport = Time.time;
                            velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            hasTeleported = true;
                            ls.previousLaser2StartPos = ls.laser2StartPos;
                            ls.laser2StartPos = myCollider.position;
                            return;

                        }
                        else
                        {

                            this.LateDestroy();
                        }




                    }


                }

                if (pCol.other.owner is Line)
                    this.LateDestroy();


            }


            if (pCol.other.owner is CircleMapObject)
            {
                velocity.Reflect(bounciness, pCol.normal);
                this.LateDestroy();
            }


        }


    }


    protected override void Update()
    {
        Move();
        UpdateScreenPosition();
    }



}

