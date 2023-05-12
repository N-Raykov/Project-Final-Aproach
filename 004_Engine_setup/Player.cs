using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing.Text;
using System.Dynamic;
using System.Threading;
using GXPEngine;
using Physics;

class Player : CircleObjectBase {
    
    
	float speedEachFrame = 2.5f;//6f
    float maxSpeedHorizontal = 7f;
	int cooldown=400;
	int lastShotTime = -10000;
    int hp = 100;
    public static readonly string tag = "player";
    int state = MOVE;
    const int MOVE = 1;
    const int JUMP = 2;
    const int FALL = 3;
    const int ROLLING = 4;
    const int FLYING = 5;
    bool hadPortalCollisionThisFrame = false;


    public Player(Vec2 startPosition,int pRadius) : base(pRadius,startPosition) {
        bounciness = 0f;
        friction = 0.5f;
        Draw(0, 255, 0);
    }


    protected override void AddCollider()
    {
        engine.AddSolidCollider(myCollider);//was trigger
    }
    protected override void OnDestroy() {
		engine.RemoveSolidCollider(myCollider);
	}

	protected override void Move() {

        bool repeat = true;
        int iteration = 0;
        float remainingMovement = 1;
        while (repeat && iteration < 2)
        {
            repeat = false;

            oldPosition = position;
            position += velocity;
            CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity*remainingMovement);
            if (colInfo != null)
            {
                repeat = true;
                remainingMovement = 1 - colInfo.timeOfImpact; 
                ResolveCollisions(colInfo);
            }
            iteration++;
        }

        List<Collider> overlaps = engine.GetOverlapsSolids(myCollider);

        List<Collider> triggerOverlaps = engine.GetOverlaps(myCollider);

        foreach (Collider pCol in overlaps)
        {
            
            if (pCol.owner is Teleporter&&!hadPortalCollisionThisFrame)
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

        foreach (Collider pTrig in triggerOverlaps)
        {
            if (pTrig.owner is Line)
            {
                
                pTrig.owner.Destroy();
            }
        }




        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol) {

        if (pCol.other.owner is Teleporter)
        {
            hadPortalCollisionThisFrame= true;
            MyGame myGame = (MyGame)Game.main;
            Teleporter teleporter = (Teleporter)pCol.other.owner;
            if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
            {

                myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                lastTeleport = Time.time;
                Console.WriteLine(velocity.Length());

                velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;

            }
            else {
                velocity.Reflect(bounciness, pCol.normal);
            }

            return;

        }

        if (pCol.other.owner is Line)
        {

            Line segment = (Line)pCol.other.owner;
            if (segment.isRotating) {
                
                Vec2 tempVelocity = pCol.normal * speedEachFrame;
                
                velocity -= tempVelocity;
                velocity.Reflect(bounciness, pCol.normal);
                velocity += tempVelocity;

                return;
            }
            else {
                Line segment1 = (Line)pCol.other.owner;
                if (segment1.start.GetAngleDegreesTwoPoints(segment1.end) != 0){
                    state = ROLLING;
                }
                else{
                    state = MOVE;
                }

                velocity.Reflect(bounciness, pCol.normal);
                return;
            }

        }

        if (pCol.other.owner is BouncyFloor) {
            velocity.Reflect(1.1f, pCol.normal);
            state = JUMP;
            return;
        }


        if (pCol.other.owner is CircleMapObject)
        {
            if (((CircleMapObject)pCol.other.owner).isMoving)
            {
                NewtonLawBalls((CircleMapObject)pCol.other.owner, pCol);
            }
            else
            {
                velocity.Reflect(bounciness, pCol.normal);
            }
            return;

        }


    }


    void Shoot() {

        if (Input.GetMouseButton(0) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).cameraManager.camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius,tag,0);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
        }

        if (Input.GetMouseButton(1) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).cameraManager.camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius, tag, 1);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
        }
    }

	protected override void Update() {
        hadPortalCollisionThisFrame= false;
        HandleInput();

        Move();
        Shoot();

        //Console.WriteLine(myCollider.position);
    }

    void HandleInput()
    {

        Vec2 moveDirection = new Vec2(0, 0);
        if (state != ROLLING) {

            if (Input.GetKey(Key.LEFT))
            {
                moveDirection -= new Vec2(1, 0);
            }
            if (Input.GetKey(Key.RIGHT))
            {
                moveDirection += new Vec2(1, 0);
            }

            if (Input.GetKey(Key.UP) && (state == MOVE))
            {
                state = JUMP;
                velocity -= new Vec2(0, 20);

            }

        }


        velocity.x += moveDirection.x * speedEachFrame; 

        switch (state){
            case MOVE:
                CapSpeed();

                if (velocity.y > 0)
                {
                    state = FALL;
                }

                if (moveDirection.x == 0 && velocity.y == 0)
                {
                    velocity.x = 0;
                }
                break;
            case JUMP:
                CapSpeed();
                if (velocity.y > 0){
                    state = FALL;
                }
                break;
            case FALL:
                CapSpeed();
                break;
            case ROLLING:
                if (velocity.y < 0) {
                    CapSpeed();
                }
                    
                break;
        
        }


        velocity += acceleration * accelerationMultiplier;

        if (velocity.Length() > maxSpeed) {
            velocity = velocity.Normalized() * maxSpeed;
        }

        //Console.WriteLine(velocity+" "+state);



    }

    void CapSpeed() {
        if (velocity.x > maxSpeedHorizontal)
            velocity.x = maxSpeedHorizontal;
        if (velocity.x < -maxSpeedHorizontal)
            velocity.x = -maxSpeedHorizontal;
    }
}

