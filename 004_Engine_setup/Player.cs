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
    float maxSpeedVertical = 50f;
	int cooldown=400;//500
	int lastShotTime = -10000;
    int hp = 100;
    public static readonly string tag = "player";
    int state = MOVE;
    const int MOVE = 1;
    const int JUMP = 2;
    const int FALL = 3;
    const int ROLLING = 4;
    const int FLYING = 5;
    int jumpCooldown = 100;
    int lastJumpTime = -100000;

    public Player(Vec2 startPosition,int pRadius) : base(pRadius,startPosition) {
        bounciness = 0f;//0.2f
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
                //if (colInfo.timeOfImpact < 0.01f) 
                repeat = true;
                remainingMovement = 1 - colInfo.timeOfImpact; // take it from here...
                ResolveCollisions(colInfo);
            }
            iteration++;
        }

        List<Collider> overlaps = engine.GetOverlaps(myCollider);

        foreach (Collider col in overlaps)
        {
            //Console.WriteLine(1);
            if (col.owner is Teleporter)
            {

                MyGame myGame = (MyGame)Game.main;
                Teleporter teleporter= (Teleporter)col.owner;
                if (myGame.teleportManager.portals[0] != null&& myGame.teleportManager.portals[1] != null&& (Time.time - lastTeleport >= teleporterCooldown))
                {
                    
                    myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].myCollider.position+radius* myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                    lastTeleport = Time.time;
                    Console.WriteLine(velocity.Length());

                    velocity=velocity.Length()*1.2f* myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;

                    //accelerationMultiplier = 0.75f;
                    Console.WriteLine(velocity.Length());
                }

                

            }
        }

        //base.Move();

        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol) {
        //Console.WriteLine(pCol.normal);

        //if (pCol.normal.y==-1)
        //    state = MOVE;
        if (pCol.normal.y < 0&&Mathf.Abs(pCol.normal.x)<0.9f)
            state = MOVE;
        //accelerationMultiplier = 1f;

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
                    //Console.WriteLine(1);
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

        }


    }


    void Shoot() {

        if (Input.GetMouseButton(0) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius,tag,0);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
        }

        if (Input.GetMouseButton(1) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius, tag, 1);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
        }
    }

	protected override void Update() {
        HandleInput();

        Move();
        Shoot();
    }

    void HandleInput()
    {
        // alternative:
        // if left or right is pressed, add a very high acceleration
        // use a max horizontal velocity (or a nonlinear friction)
        //
        // This would give responsive platformer controls, but also physics response (velocity is maintained when not pushing button)


        //Vec2 moveDirection = new Vec2(0, 0);

        //if (Input.GetKey(Key.LEFT))
        //{
        //    moveDirection = new Vec2(-1, 0);
        //}
        //if (Input.GetKey(Key.RIGHT))
        //{
        //    moveDirection = new Vec2(1, 0);
        //}

        //if (Input.GetKey(Key.UP) && state == MOVE)
        //{
        //    //Console.WriteLine(1);
        //    state = JUMP;
        //    velocity -= new Vec2(0, 20);

        //}


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

            if (Input.GetKey(Key.UP) && state == MOVE)
            {
                //Console.WriteLine(1);
                state = JUMP;
                velocity -= new Vec2(0, 20);

            }

        }


        velocity.x += moveDirection.x * speedEachFrame; // This seems a bit strict to me for a physics based game...

        //Console.WriteLine(state);
        //when falling down after the jump need to cap the speed while making sure the speed when going down slope doesnt cap
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
                    //Console.WriteLine(1);
                    CapSpeed();
                    //velocity.x *= friction;
                }
                    
                break;
        
        }


        velocity += acceleration * accelerationMultiplier;

        //Console.WriteLine(velocity+" "+state);



    }

    void CapSpeed() {
        if (velocity.x > maxSpeedHorizontal)
            velocity.x = maxSpeedHorizontal;
        if (velocity.x < -maxSpeedHorizontal)
            velocity.x = -maxSpeedHorizontal;
    }

    public void TakeDamage(int pDamage) { 
        hp-=pDamage;
        if (hp <= 0) {
            RemoveChild(((MyGame)game).camera);
            parent.AddChild(((MyGame)game).camera);
            (((MyGame)game).camera).x = myCollider.position.x;
            (((MyGame)game).camera).y = myCollider.position.y;
            this.LateDestroy();
        }
            
    }


}

