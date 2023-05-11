﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Threading;
using GXPEngine;
using Physics;

class Player : CircleObjectBase {
    
    
	float speed = 7f;//6f
	int cooldown=400;//500
	int lastShotTime = -10000;
    int hp = 100;
    public static readonly string tag = "player";
    int state = MOVE;
    const int MOVE = 1;
    const int JUMP = 2;
    const int FALL = 3;

    public Player(Vec2 startPosition,int pRadius) : base(pRadius,startPosition) {
        bounciness = 0f;//0.2f
        Draw(0, 255, 0);
    }


    protected override void AddCollider()
    {
        engine.AddTriggerCollider(myCollider);
    }
    protected override void OnDestroy() {
		engine.RemoveTriggerCollider(myCollider);
	}

	protected override void Move() {

        bool repeat = true;
        int iteration = 0;
        while (repeat && iteration < 2)
        {
            repeat = false;

            oldPosition = position;
            position += velocity;
            CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity);
            if (colInfo != null)
            {
                if (colInfo.timeOfImpact < 0.01f) repeat = true;
                ResolveCollisions(colInfo);
            }
            iteration++;
        }

        //base.Move();

        UpdateScreenPosition();


    }

    void ResolveCollisions(CollisionInfo pCol) {
        state = MOVE;
        if (pCol.other.owner is Line)
        {

            Line segment = (Line)pCol.other.owner;
            if (segment.isRotating) {
                
                Vec2 tempVelocity = pCol.normal * speed;
                
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
        Vec2 moveDirection = new Vec2(0, 0);

        if (Input.GetKey(Key.LEFT))
        {
            moveDirection = new Vec2(-1, 0);
        }
        if (Input.GetKey(Key.RIGHT))
        {
            moveDirection = new Vec2(1, 0);
        }

        if (Input.GetKey(Key.UP) && state == MOVE)
        {
            Console.WriteLine(1);
            state = JUMP;
            velocity -= new Vec2(0, 20);

        }
        velocity.x = moveDirection.x * speed;
        velocity += acceleration * accelerationMultiplier;
        //Console.WriteLine(velocity);



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

