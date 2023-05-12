using System;
using System.Collections.Generic;
using System.Dynamic;
using GXPEngine;
using GXPEngine.Core;
using Physics;

class Projectile : CircleObjectBase {

	public int bounces = 0;
	public readonly int maxBounces = 1;
	float speed=20f;
    string owner;
    int damage = 1;
    public static int _radius=5;
    int portalNumber=-1;

    public static float _speed {
        get { return 10f; }//remember to also change speed
    }
    //Sprite sprite = new Sprite("bullet.png",false);

	public Projectile(Vec2 startPosition, Vec2 pVelocity,int pRadius,string pOwner,int pPortalNumber) : base(pRadius,startPosition) {
		
        _radius=pRadius;
		velocity=pVelocity;
        owner = pOwner;
        bounciness = 1f;
        Draw(255, 255, 255);
        portalNumber = pPortalNumber;
        //sprite.SetScaleXY(2, 2);
        //AddChild(sprite);
        //sprite.SetOrigin(width/2,height/2);
        
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

            if (pCol.other.owner is Line&&pCol.other is LineSegment && portalNumber!=-1) {
                MyGame myGame = (MyGame)Game.main;
                LineSegment line = (LineSegment)pCol.other;
                if (myGame.teleportManager.portals[portalNumber] != null) {

                    myGame.teleportManager.portals[portalNumber].Destroy();
                    
                    //Console.WriteLine(myGame.teleportManager.portals[portalNumber]);
                }
                //oldPosition+=velocity*pCol.timeOfImpact
                float rotation = line.start.GetAngleDegreesTwoPoints(line.end);
                //myGame.teleportManager.portals[portalNumber] = new Teleporter(myCollider.position, portalNumber,pCol.normal);
                myGame.teleportManager.portals[portalNumber] = new Teleporter(myCollider.position-pCol.normal*radius/2, portalNumber, pCol.normal);
                myGame.teleportManager.portals[portalNumber].Rotate(rotation);


                parent.AddChild(myGame.teleportManager.portals[portalNumber]);
                //parent.AddChild(new Teleporter(myCollider.position,portalNumber));
                this.LateDestroy();
            }

            if (pCol.other.owner is CircleMapObject)// || pCol.other.owner is Line
            {
                velocity.Reflect(bounciness, pCol.normal);
                bounces++;
            }
            

            if (pCol.other.owner is Projectile) { 
                Projectile projectile=(Projectile)pCol.other.owner;
                NewtonLawBalls(projectile, pCol);   
            }
        }

        List<Physics.Collider> overlaps = engine.GetOverlaps(myCollider);

        foreach (Physics.Collider col in overlaps){
            if (col.owner is Player&&owner != Player.tag)
            {
                Player player = (Player)col.owner;
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

