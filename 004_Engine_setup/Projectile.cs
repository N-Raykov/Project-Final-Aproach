using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using GXPEngine;
using GXPEngine.Core;
using Physics;

class Projectile : CircleObjectBase {

    GameObject owner;
	//public int bounces = 0;
	//public readonly int maxBounces = 1;
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

    protected override void OnDestroy() {
        engine.RemoveTriggerCollider(myCollider);
	}

	protected override void Move() {
        //sprite.rotation = (velocity.GetAngleDegrees());
        CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity * speed);
        ResolveCollisions(colInfo);
    }

    void ResolveCollisions(CollisionInfo pCol) {

        if (pCol != null)
        {
            if (pCol.other is LineSegment) {
                if (pCol.other.owner is Line)
                {

                    Line line = (Line)pCol.other.owner;
                    if (line.Owner is LaserShooter && portalNumber != -1)
                    {
                        this.LateDestroy();
                        return;
                    }

                   


                }



                if (!(pCol.other.owner is Teleporter))
                {
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

                        parent.AddChild(myGame.teleportManager.portals[portalNumber]);
                        this.LateDestroy();
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

