using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;
using TiledMapParser;

public class LaserShooter:AnimationSprite{

    MyGame myGame = (MyGame)Game.main;
    float radius=20;
    const int LEFT = 1; 
    const int RIGHT = 2;
    int side;
    Vec2 position;
    public Sprite laser1 = new Sprite("buttom_and_laser_blue.png", false);
    public Sprite laser2 = new Sprite("buttom_and_laser_blue.png", false);
    public bool laser1WasDrawn = false;
    public bool laser2WasDrawn = false;
    float lineWidthHalf = 3;
    bool addedToGame = false;
    bool setPosition = false;


    int portalNumber = -1;

    Line laser1Col1;
    Line laser1Col2;

    Line laser2Col1;
    Line laser2Col2;

    public Vec2 laser2StartPos = new Vec2();
    public Vec2 previousLaser2StartPos = new Vec2();
    //Vec2 pPosition,int pside
    public LaserShooter(string filename, int cols, int rows, TiledObject obj = null) : base(filename, cols, rows)
    {
        side=obj.GetIntProperty("side");
        laser1.alpha = 0;
        laser2.alpha = 0;
        laser1.SetOrigin(0, laser1.height / 2);
        laser2.SetOrigin(0, laser2.height / 2);
        SetOrigin(radius, radius);
        
    }

    void SetPosition() {
        if (!setPosition) {
            if (side == RIGHT)
                position.SetXY(x + width / 2, y);
            else
                position.SetXY(x - width / 2, y);
            setPosition = true;
        }
    }

    public void DrawLaser1(Vec2 end) {
        
        Vec2 vec = end - position;
        float length=vec.Length();
        laser1.alpha = 1;
        laser1.SetScaleXY(length/laser1.width, 1);
        laser1.SetXY(position.x,position.y);

        Vec2 normal = vec.Normal();
        Vec2 reverseNormal = vec.ReverseNormal();
        laser1Col1 = new Line(position+normal*lineWidthHalf,end+normal*lineWidthHalf);
        laser1Col1.SetOwner(this);
        laser1Col2 = new Line(position + reverseNormal * lineWidthHalf, end + reverseNormal * lineWidthHalf);
        laser1Col2.SetOwner(this);
        parent.AddChild(laser1Col1);
        parent.AddChild(laser1Col2);
        parent.AddChild(laser1);
        float rotation = position.GetAngleDegreesTwoPoints(end);
        laser1.rotation= rotation;


        laser1WasDrawn = true;
    }

    public void DrawLaser2(Vec2 start,Vec2 end) {
        
        
        laser2WasDrawn = true;
        Vec2 vec = end - start;
        float length = vec.Length();
        laser2.SetScaleXY(1, 1);
        laser2.alpha = 1;
        laser2.SetScaleXY(length / laser2.width, 1);
        laser2.SetXY(start.x, start.y);

        Vec2 normal = vec.Normal();
        Vec2 reverseNormal = vec.ReverseNormal();
        if (laser2Col1 != null)
            laser2Col1.Destroy();
        if (laser2Col2 != null)
            laser2Col2.Destroy();
        laser2Col1 = new Line(start + normal * lineWidthHalf, end + normal * lineWidthHalf);
        laser2Col1.SetOwner(this);
        laser2Col2 = new Line(start + reverseNormal * lineWidthHalf, end + reverseNormal * lineWidthHalf);
        laser2Col2.SetOwner(this);
        parent.AddChild(laser2Col1);
        parent.AddChild(laser2Col2);

        float rotation = start.GetAngleDegreesTwoPoints(end);
        laser2.rotation = rotation;

    }

    public void DestroyLaser2(bool delete) {

        if (delete || myGame.teleportManager.shots>0) {
            if (laser2Col1 != null)
                laser2Col1.Destroy();
            if (laser2Col2 != null)
                laser2Col2.Destroy();

            laser2.alpha = 0;
            laser2.SetScaleXY(1, 1);

            myGame.teleportManager.portalsChanged[0] = false;
            myGame.teleportManager.portalsChanged[1] = false;
            laser2WasDrawn = false;

        }


    }

    void Update() {
        Reset();
        SetPosition();
        AddLasers();
        if (myGame.teleportManager.shots>0)
            Shoot(side);
        DestroyLaser2(false);
    }

    void Shoot(int pSide) { 
    
        switch (pSide)
        {
            case 1:
                CreateBullet(180);
                break;
            case 2:
                CreateBullet(0);
                break;


        }

    }

    void CreateBullet(float pAngle) {
        Vec2 shotDirection = new Vec2(1000f, 0);
        shotDirection.RotateDegrees(pAngle);
        Projectile bullet = new Projectile(position, shotDirection, 1,-1,this);
        parent.AddChild(bullet);
    }

    void AddLasers() {
        if (!addedToGame) {
            
            parent.AddChild(laser2);
            addedToGame= true;
        }
    }

    public void SetPortalNumber(int pPortalNumber) {
        portalNumber = pPortalNumber;
    }

    void Reset() {

        //if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
        //    Console.WriteLine("mouse pressed");
        //    DestroyLaser2(true);
        //}

        if (Input.GetKeyUp(Key.R)|| Input.GetKeyUp(Key.F))
        {
            DestroyLaser2(true);    
            laser2StartPos = new Vec2();
            previousLaser2StartPos = new Vec2();
            laser2WasDrawn= false;
        }

    }

}
