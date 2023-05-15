using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing.Text;
using System.Dynamic;
using System.Threading;
using GXPEngine;
using Physics;

class Player : CircleObjectBase
{

    Random rnd = new Random();

    float speedEachFrame = 2.5f;//6f
    float maxSpeedHorizontal = 7f;
    int cooldown = 400;
    int lastShotTime = -10000;
    int hp = 100;
    public static readonly string tag = "player";
    int state = MOVE;
    const int MOVE = 1;
    const int JUMP = 2;
    const int FALL = 3;
    const int ROLLING = 4;
    const int FLYING = 5;
    bool capSpeed = true;

    bool grounded = false;
    AnimationSprite sprite = new AnimationSprite("Robot_Frames.png", 32,5,158);


    int lastInputTime = -10000;
    int idleDelay = 1000;
    int lastInput = 0;


    public Player(Vec2 startPosition, int pRadius) : base(pRadius, startPosition)
    {
        bounciness = 0f;
        friction = 0.5f;
        Draw(255, 255, 255);
        
        sprite.SetOrigin(sprite.width / 2, sprite.height / 4*3);
        sprite.SetScaleXY(0.8f, 0.8f);
        AddChild(sprite);
    }


    protected override void AddCollider()
    {
        engine.AddSolidCollider(myCollider);//was trigger
    }
    protected override void OnDestroy()
    {
        engine.RemoveSolidCollider(myCollider);
    }

    protected override void Move()
    {

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
                repeat = true;
                remainingMovement = 1 - colInfo.timeOfImpact;
                ResolveCollisions(colInfo);
            }
            iteration++;
        }

        List<Collider> overlaps = engine.GetOverlapsSolids(myCollider);
        List<Collider> triggerOverlaps = engine.GetOverlaps(myCollider);

        foreach (Collider pTrig in triggerOverlaps)
        {
            if (pTrig.owner.parent is CameraTrigger)
            {
                CameraTrigger trigger = (CameraTrigger)pTrig.owner.parent;
                int target = trigger.GiveTarget();
                ((MyGame)game).cameraManager.MoveCamera(pTrig, target);
            }



            if (pTrig.owner.parent is Collectable)
            {
                Collectable collectable = (Collectable)pTrig.owner.parent;
                collectable.PickUp();
                new Sound("Collectible_Pickup.wav").Play();
            }
        }

        foreach (Collider pCol in overlaps)
        {

            if (pCol.owner is Teleporter)
            {
                //Console.WriteLine(1);
                MyGame myGame = (MyGame)Game.main;
                Teleporter teleporter = (Teleporter)pCol.owner;
                if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
                {

                    myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                    lastTeleport = Time.time;
                    //Console.WriteLine(velocity.Length());

                    velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                    state = FLYING;
                    //accelerationMultiplier = 0.75f;
                    //Console.WriteLine(velocity.Length());
                }



            }
        }




        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol)
    {

        if (pCol.normal.y < -0.9f) grounded = true;


        if (pCol.other.owner is Teleporter)
        {
            MyGame myGame = (MyGame)Game.main;
            Teleporter teleporter = (Teleporter)pCol.other.owner;
            if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null && (Time.time - lastTeleport >= teleporterCooldown))
            {

                myCollider.position = myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].rotationOrigin + radius * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                lastTeleport = Time.time;
                //Console.WriteLine(velocity.Length());

                velocity = velocity.Length() * 1.0f * myGame.teleportManager.portals[Mathf.Abs(teleporter.portalNumber - 1)].normal;
                state = FLYING;

                new Sound("PortalEnterSFXNew.wav").Play();
            }
            else
            {
                velocity.Reflect(bounciness, pCol.normal);
            }

            return;

        }

        if (pCol.other.owner is Line)
        {

            Line segment = (Line)pCol.other.owner;
            if (segment.isRotating)
            {

                Vec2 tempVelocity = pCol.normal * speedEachFrame;

                velocity -= tempVelocity;
                velocity.Reflect(bounciness, pCol.normal);
                velocity += tempVelocity;

                return;
            }
            else
            {
                Line segment1 = (Line)pCol.other.owner;
                //Console.WriteLine(segment1.start.GetAngleDegreesTwoPoints(segment1.end));
                float angle = Mathf.Abs(segment1.start.GetAngleDegreesTwoPoints(segment1.end)) % 180;

                if (Input.GetKey(Key.LEFT_SHIFT))
                {
                    Console.WriteLine("angle: {0}", angle);
                }

                if (angle >= 5 && angle < 90)
                {
                    //Console.WriteLine(segment1.start.GetAngleDegreesTwoPoints(segment1.end));
                    state = ROLLING;
                }
                else
                {
                    state = MOVE;
                }

                velocity.Reflect(bounciness, pCol.normal);
                return;
            }

        }

        if (pCol.other.owner is BouncyFloor)
        {
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


    void Shoot()
    {

        if (Input.GetMouseButton(0) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).cameraManager.camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius, 0);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
            int randomIndex = rnd.Next(0, 3);
            switch (randomIndex)
            {
                case 0:
                    new Sound("Portal_Gun_Shot1.wav").Play();
                    break;
                case 1:
                    new Sound("Portal_Gun_Shot2.wav").Play();
                    break;
                case 2:
                    new Sound("Portal_Gun_Shot3.wav").Play();
                    break;
            }
        }

        if (Input.GetMouseButton(1) && (Time.time - lastShotTime >= cooldown))
        {
            var result = ((MyGame)game).cameraManager.camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            Vec2 shotDirection = (new Vec2(result.x, result.y) - position).Normalized();
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius, 1);
            parent.AddChild(bullet);
            lastShotTime = Time.time;

            int randomIndex = rnd.Next(0, 3);
            switch (randomIndex)
            {
                case 0:
                    new Sound("Portal_Gun_Shot1.wav").Play();
                    break;
                case 1:
                    new Sound("Portal_Gun_Shot2.wav").Play();
                    break;
                case 2:
                    new Sound("Portal_Gun_Shot3.wav").Play();
                    break;
            }
        }
    }

    protected override void Update()
    {
        SetColor(state % 2, (state / 2) % 2, (state + 1) % 2);
        alpha = grounded ? 1 : 0.4f;


        HandleInput();

        grounded = false;

        Move();
        Shoot();

        State();
    }

    void HandleInput()
    {

        Vec2 moveDirection = new Vec2(0, 0);
        if (state != ROLLING || state == ROLLING)
        {

            if (Input.GetKey(Key.LEFT) || Input.GetKey(Key.A))
            {
                moveDirection -= new Vec2(1, 0);
                lastInputTime = Time.time;
                lastInput = 1;
            }
            if (Input.GetKey(Key.RIGHT) || Input.GetKey(Key.D))
            {
                moveDirection += new Vec2(1, 0);
                lastInputTime = Time.time;
                lastInput = 0;
            }

            if (Input.GetKey(Key.UP) && (state == MOVE) && grounded || Input.GetKey(Key.W) && (state == MOVE) && grounded)
            {
                state = JUMP;
                lastInputTime = Time.time;
                velocity -= new Vec2(0, 20);
                new Sound("Character_Jump.wav").Play();
            }

        }


        velocity.x += moveDirection.x * speedEachFrame;

        switch (state)
        {
            case MOVE:
                CapSpeed();

                if (velocity.y > 0)
                {
                    state = FALL;
                    capSpeed = true;
                }

                if (velocity.y < 0)
                {
                    state = JUMP;
                    capSpeed = false;
                }

                if (moveDirection.x == 0 && velocity.y == 0)
                {
                    velocity.x = 0;
                }
                break;
            case JUMP:
                CapSpeed();
                if (velocity.y > 0)
                {
                    state = FALL;
                    capSpeed = true;
                }
                break;
            case FALL:
                if (capSpeed)
                    CapSpeed();
                break;
            case ROLLING:
                if (velocity.y < 0)
                {
                    CapSpeed();
                    capSpeed = true;
                }

                break;
            case FLYING:

                break;

        }


        velocity += acceleration * accelerationMultiplier;

        if (velocity.Length() > maxSpeed)
        {
            velocity = velocity.Normalized() * maxSpeed;
        }





    }

    void CapSpeed()
    {
        if (velocity.x > maxSpeedHorizontal)
            velocity.x = maxSpeedHorizontal;
        if (velocity.x < -maxSpeedHorizontal)
            velocity.x = -maxSpeedHorizontal;
    }

    void State()
    {
        switch (lastInput) {
            case 0:
                sprite.Mirror(false,false);
                break;
            case 1:
                sprite.Mirror(true,false);
                break;
        
        }

        if (Time.time - lastInputTime > idleDelay && velocity.x == 0 && Mathf.Abs(velocity.y) <= 1)
        {
            sprite.SetCycle(36, 60);
            sprite.Animate(1);
        }
        else {
            sprite.SetCycle(97, 61);
            sprite.Animate(1);

        }


    }

}

