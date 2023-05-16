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
        portalNumber = pPortalNumber;
        Draw(255, 255, 255);
        
        
	}

    protected override void Draw(byte red, byte green, byte blue)
    {
        if (portalNumber!=-1)
            base.Draw(red, green, blue);
    }

    protected override void AddCollider()
    {
        engine.AddTriggerCollider(myCollider);//might need to change back to solid

    }

    protected override void OnDestroy() {
        engine.RemoveTriggerCollider(myCollider);
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
            if (pCol.other is LineSegment) {
                if (pCol.other.owner is Line) {
                    
                    Line line = (Line)pCol.other.owner;
                    if (line.Owner is LaserShooter&&portalNumber!=-1) {
                        this.LateDestroy();
                        return;
                    }

                    if (line.Owner is Button){
                        if (portalNumber != -1)
                        {
                            this.LateDestroy();
                            return;
                        }
                        else {
                            
                            Button button = (Button)line.Owner;
                            button.StartMovingDoor();
                        }

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
                        myGame.teleportManager.portals[portalNumber].sprite.rotation = rotation;

                        myGame.teleportManager.portalsChanged[portalNumber] = true;
                        

                        myGame.teleportManager.shots = 2;//might need to be higher


                        parent.AddChild(myGame.teleportManager.portals[portalNumber]);
                        this.LateDestroy();
                    }
                    else {
                        
                        if (owner is LaserShooter) {
                            LaserShooter ls = (LaserShooter)owner;
                            ls.SetPortalNumber(-1);
                            if (!ls.laser1WasDrawn)
                            {
                                ls.DrawLaser1(myCollider.position);

                            }


                            if (ls.laser1WasDrawn) {
                                if (ls.previousLaser2StartPos.x != ls.laser2StartPos.x || ls.previousLaser2StartPos.y != ls.laser2StartPos.y) {
                                    //Console.WriteLine(1);
                                    ls.laser2WasDrawn = false;
                                }
                            }

                            if (ls.laser1WasDrawn && !ls.laser2WasDrawn&&hasTeleported) {
                                Console.WriteLine(ls.previousLaser2StartPos+ " "+ls.laser2StartPos);
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

                            ls.SetPortalNumber(teleporter.portalNumber);
                            myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            lastTeleport = Time.time;
                            velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                            hasTeleported= true;
                            ls.previousLaser2StartPos=ls.laser2StartPos;
                            ls.laser2StartPos= myCollider.position;
                            return;

                        } else {
                            
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

