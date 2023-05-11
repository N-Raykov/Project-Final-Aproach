using System;
using System.Dynamic;
using System.Runtime.InteropServices;
using GXPEngine;
using Physics;
using TiledMapParser;

class Enemy : CircleObjectBase
{
    int cooldown = 200;
    int attackRotation = 0;
    int attackRotationChange = 20;
    int lastShotTime = -10000;
    int hp = 100;
    public static readonly string tag = "enemy";
    const int LEFT = -1;
    const int RIGHT = 1;
    int stateDuration = 10000;
    int stateStartTime = 0;
    int waitTimeBetweenStates = 1500;
    
    const int WAIT = 1;
    const int SPINATTACKLEFT = 2;
    const int SPINATTACKRIGHT = 3;
    const int AIMBOT = 4;
    const int RANDOMFASTSHOTS = 5;
    int state = WAIT;
    Player player;



    public Enemy(Vec2 startPosition, int pRadius,Player pPlayer) : base(pRadius,startPosition)
    {
        player = pPlayer;
        Draw(255,0,0);
    }


    protected override void OnDestroy()
    {
        engine.RemoveSolidCollider(myCollider);
    }

    protected override void Move()
    {

        CollisionInfo colInfo = engine.MoveUntilCollision(myCollider, velocity);

        UpdateScreenPosition();


    }

    void PredictPlayerPositionAndShoot()
    {

        if ((Time.time - lastShotTime >= 2*cooldown))
        {

            Vec2 relativePosition = player.myCollider.position - this.myCollider.position;
            Vec2 relativevelocity = player.velocity - this.velocity;

            float t = CalculatePrejectileTimeOfImpact(relativePosition, relativevelocity, Projectile._speed);

            Vec2 direction = (player.myCollider.position - this.myCollider.position + (player.velocity - this.velocity) * t).Normalized();
            CreateBullet(direction);
        }
    }

    void SpinAttack(int pSpinDirection) {

        if ((Time.time - lastShotTime >= cooldown))
        {
            Vec2 direction = new Vec2(1, 0) / 4;//i divide it to have a smaller speed; i could do this by changing the speed in the bullet class but i heve 2 speeds i need
            //so this is just less work
            direction.RotateDegrees(attackRotation);
            CreateBullet(direction);
            attackRotation += attackRotationChange*pSpinDirection;
            lastShotTime = Time.time;
        }

    }

    void CreateBullet(Vec2 pDirection) {
        Projectile bullet = new Projectile(position, pDirection, Projectile._radius, tag);
        parent.AddChild(bullet);
        lastShotTime = Time.time;
    }

    float CalculatePrejectileTimeOfImpact(Vec2 pRelativeDistance, Vec2 pRelativeVelocity, float pSpeed)
    {
        float a = pRelativeVelocity.Dot(pRelativeVelocity)-pSpeed*pSpeed;
        float b = 2f*pRelativeVelocity.Dot(pRelativeDistance);
        float c = pRelativeDistance.Dot(pRelativeDistance);

        float d = b * b -4f * a * c;
        float t = 0;
        if (d >= 0)
        {
            t = (-b - Mathf.Sqrt(d)) / (2 * a);
            if (t < 0)
                t = 0;

        }

        return t;

    }


    protected override void Update()
    {
        SwitchState();
        switch (state)
        {
            case WAIT:
                Wait();
                break;
            case SPINATTACKLEFT:
                SpinAttack(LEFT);
                break;
            case SPINATTACKRIGHT:
                SpinAttack(RIGHT);
                break;
            case AIMBOT:
                PredictPlayerPositionAndShoot();
                break;
            case RANDOMFASTSHOTS:
                RandomFastShots();
                break;

        }

    }

    void RandomFastShots() {
        if ((Time.time - lastShotTime >= cooldown/4))
        {
            Vec2 direction = Vec2.RandomUnitVector();
            CreateBullet(direction);
            lastShotTime = Time.time;
        }



    }

    void SwitchState() {
        if (Time.time - stateStartTime >= stateDuration) {
            state = WAIT;
            stateStartTime= Time.time;
        }
    }

    void Wait() {
        if (Time.time - stateStartTime > waitTimeBetweenStates) { 
        
            int randomNumber=Vec2.rand.Next(1, 4);
            switch(randomNumber){
                case 1:
                    state = AIMBOT;
                    break;
                case 2:
                    int randomNumber2 = Vec2.rand.Next(0, 2);
                    state = randomNumber2 + 2;
                    break;
                case 3:
                    state = RANDOMFASTSHOTS;
                    break;

            }
            stateStartTime= Time.time;
        }
    }



    public void TakeDamage(int pDamage)
    {
        hp -= pDamage;
        if (hp <= 0)
            this.LateDestroy();
    }
}

