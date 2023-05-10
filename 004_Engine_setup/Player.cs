using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
    int jumpCooldown = 100;
    int lastJumpTime = -100000;

    public Player(Vec2 startPosition,int pRadius) : base(pRadius,startPosition) {
        bounciness = 0f;//0.2f
        friction = 0.99f;
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

        //base.Move();

        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol) {
        //Console.WriteLine(pCol.normal);

        //if (pCol.normal.y==-1)
        //    state = MOVE;
        if (pCol.normal.y < 0&&Mathf.Abs(pCol.normal.x)<0.9f)
            state = MOVE;

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
                velocity.Reflect(bounciness, pCol.normal);
                return;
            }

        }

        if (pCol.other.owner is Enemy)
        {
            velocity.Reflect(bounciness, pCol.normal);
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
            Projectile bullet = new Projectile(position, shotDirection, Projectile._radius,tag);
            parent.AddChild(bullet);
            lastShotTime = Time.time;
        }
    }

	protected override void Update() {
        HandleInput();
        //Console.WriteLine(velocity);
        //switch (state)
        //{

        //    case MOVE:

        //        break;
        //    case JUMP:

        //        break;
        //    case FALL:

        //        break;


        //}

        Move();
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

        velocity.x += moveDirection.x * speedEachFrame; // This seems a bit strict to me for a physics based game...
        if (velocity.x > maxSpeedHorizontal && velocity.y <= 0)//when falling down after the jump need to cap the speed while making sure the speed when going down slope doesnt cap
            velocity.x = maxSpeedHorizontal;
        if (velocity.x < -maxSpeedHorizontal && velocity.y <= 0)
            velocity.x = -maxSpeedHorizontal;


        if (state == FALL) {
            if (velocity.x > maxSpeedHorizontal)
                velocity.x = maxSpeedHorizontal;
            if (velocity.x < -maxSpeedHorizontal)
                velocity.x = -maxSpeedHorizontal;
        }

        //Console.WriteLine(moveDirection.x+" "+velocity.y);
        if (moveDirection.x == 0 && velocity.y == 0) {
            velocity.x = 0;
        }

        if (state== JUMP&&velocity.y>0)
        {
            state = FALL;
        }


        velocity += acceleration * accelerationMultiplier;
        //if (velocity.y > maxSpeedVertical)
         //   velocity.y= maxSpeedVertical;
        //velocity.x *= friction;
        Console.WriteLine(velocity+" "+state);



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

