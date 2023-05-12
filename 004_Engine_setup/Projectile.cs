using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using GXPEngine;
using GXPEngine.Core;
using Physics;

class Projectile : CircleObjectBase {

    GameObject owner;
	public int bounces = 0;
	public readonly int maxBounces = 1;
	float speed=40f;
    public static int _radius=5;
    int portalNumber=-1;
    bool hasTeleported = false;


    public static float _speed {
        get { return 10f; }//remember to also change speed
    }

	public Projectile(Vec2 startPosition, Vec2 pVelocity,int pRadius,int pPortalNumber,GameObject pOwner=null) : base(pRadius,startPosition) {
        owner = pOwner;
        _radius=pRadius;
		velocity=pVelocity;
        bounciness = 1f;
        Draw(255, 255, 255);
        portalNumber = pPortalNumber;
        
	}
    protected override void AddCollider()
    {
        engine.AddTriggerCollider(myCollider);//might need to change back to solid

    }

    protected override void OnDestroy() {
        engine.RemoveTriggerCollider(myCollider);
	}

    ~Projectile()
    {
        //Console.WriteLine("GC destroys projectile");
    }

	protected override void Move() {
        //sprite.rotation = (velocity.GetAngleDegrees());
        CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity*speed);
        ResolveCollisions(colInfo);
        
        if (bounces>=maxBounces) {
			LateDestroy();
		}
	}

	void ResolveCollisions(CollisionInfo pCol) {
        if (pCol != null)
        {
            //Console.WriteLine(owner);
            if (pCol.other is LineSegment) {
                if (pCol.other.owner is Line) {
                    
                    Line line = (Line)pCol.other.owner;
                    Console.WriteLine(line.Owner);
                    if (line.Owner is LaserShooter&&portalNumber!=-1) {
                        this.LateDestroy();
                        return;
                    }
                        

                }


                if (!(pCol.other.owner is Teleporter)) {
                    if (portalNumber != -1)
                    {
                        MyGame myGame = (MyGame)Game.main;
                        LineSegment line = (LineSegment)pCol.other;
                        if (myGame.teleportManager.portals[portalNumber] != null)
                        {
                            myGame.teleportManager.portals[portalNumber].Destroy();
                        }
                        float rotation = line.start.GetAngleDegreesTwoPoints(line.end);
                        myGame.teleportManager.portals[portalNumber] = new Teleporter(myCollider.position - pCol.normal * radius / 2, portalNumber, pCol.normal);
                        myGame.teleportManager.portals[portalNumber].Rotate(rotation);
                        if (portalNumber==0)
                            myGame.teleportManager.portal1HasChanged= true;

                        parent.AddChild(myGame.teleportManager.portals[portalNumber]);
                        this.LateDestroy();
                    }
                    else {

                        if (owner is LaserShooter) {
                            LaserShooter ls = (LaserShooter)owner;

                            if (!ls.laser1WasDrawn)
                            {
                                ls.DrawLaser1(myCollider.position);

                            }

                            //Console.WriteLine(ls.laser2StartPos+" "+ls.previousLaser2StartPos);

                            if (ls.laser1WasDrawn) {
                                if (ls.previousLaser2StartPos.x != ls.laser2StartPos.x || ls.previousLaser2StartPos.y != ls.laser2StartPos.y) { 
                                    ls.laser2WasDrawn = false;
                                }
                            }

                            if (ls.laser1WasDrawn && !ls.laser2WasDrawn&&hasTeleported) {
                                ls.DrawLaser2(ls.laser2StartPos, myCollider.position);
                            }

                            

                        }

                        this.LateDestroy();
                    }
                }
                else {
                    if (owner is LaserShooter){
                        MyGame myGame = (MyGame)Game.main;
                        Teleporter teleporter = (Teleporter)pCol.other.owner;
                        LaserShooter ls = (LaserShooter)owner;

                        if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown)) {
                            myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            lastTeleport = Time.time;
                            velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            hasTeleported= true;
                            //Console.WriteLine(ls.laser2StartPos + " " + ls.previousLaser2StartPos);
                            ls.previousLaser2StartPos=ls.laser2StartPos;
                            ls.laser2StartPos= myCollider.position;
                            //Console.WriteLine(ls.laser2StartPos + " " + ls.previousLaser2StartPos);
                            return;

                        } else {
                            this.LateDestroy();
                        }

                    }


                }

                if (pCol.other.owner is Line)
                    this.LateDestroy();
   
            
            }
 

            if (pCol.other.owner is CircleMapObject)// || pCol.other.owner is Line
            {
                velocity.Reflect(bounciness, pCol.normal);
                bounces++;
            }
            

        }


    }


    protected override void Update()
    {
        Move();
        UpdateScreenPosition();
    }



}

