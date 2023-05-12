using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using GXPEngine.Core;
using Physics;

public class LaserShooter:EasyDraw{

    MyGame myGame = (MyGame)Game.main;
    float radius=20;
    const int LEFT = 1;
    const int RIGHT = 2;
    Vec2 position;
    public EasyDraw laser1=new EasyDraw(2000,2000,false);
    public EasyDraw laser2 = new EasyDraw(2000, 2000, false);
    public bool laser1WasDrawn = false;
    public bool laser2WasDrawn = false;
    float lineWidthHalf = 3;

    Line laser1Col1;
    Line laser1Col2;

    Line laser2Col1;
    Line laser2Col2;

    public Vec2 laser2StartPos = new Vec2();
    public Vec2 previousLaser2StartPos = new Vec2();

    public LaserShooter(Vec2 pPosition) : base(2000, 2000, false)
    {
        
        SetOrigin(radius, radius);
        position = pPosition;
        UpdateScreenPosition();
        Draw(0, 0, 188);
        
    }

    public void DrawLaser1(Vec2 end) {
        
        parent.AddChild(laser1);
        //Console.WriteLine(position+" "+end);
        laser1.Clear(Color.Empty);
        laser1.StrokeWeight(1);//was lineWidthHalf*2
        laser1.Stroke(255,0,0);
        laser1.Line(position.x, position.y, end.x, end.y);//+radius
        Vec2 vec = end - position;
        Vec2 normal = vec.Normal();
        Vec2 reverseNormal = vec.ReverseNormal();
        //Console.WriteLine(position + " " + end);
        laser1Col1 = new Line(position+normal*lineWidthHalf,end+normal*lineWidthHalf);
        laser1Col2 = new Line(position + reverseNormal * lineWidthHalf, end + reverseNormal * lineWidthHalf);
        parent.AddChild(laser1Col1);
        parent.AddChild(laser1Col2);



        laser1WasDrawn= true;
    }

    public void DrawLaser2(Vec2 start,Vec2 end) { 
        parent.AddChild(laser2);
        laser2.Clear(Color.Empty);
        laser2.StrokeWeight(lineWidthHalf*2);//was 0
        laser2.Stroke(255, 0, 0);
        laser2.Line(start.x, start.y, end.x, end.y);//+radius
        laser2WasDrawn = true;
        //Console.WriteLine(1);
        Vec2 vec = end - position;
        Vec2 normal = vec.Normal();
        Vec2 reverseNormal = vec.ReverseNormal();
        if (laser2Col1!=null)
            laser2Col1.Destroy();
        if (laser2Col2!=null)
            laser2Col2.Destroy();
        laser2Col1 = new Line(start + normal * lineWidthHalf, end + normal * lineWidthHalf);
        laser2Col2 = new Line(start + reverseNormal * lineWidthHalf, end + reverseNormal * lineWidthHalf);
        parent.AddChild(laser2Col1);
        parent.AddChild(laser2Col2);

    }

    public void DestroyLaser2() {
        if (myGame.teleportManager.portal1HasChanged) {
            Console.WriteLine(1);
            laser2.Clear(Color.Empty);
            if (laser2Col1 != null)
                laser2Col1.Destroy();
            if (laser2Col2 != null)
                laser2Col2.Destroy();
            laser2WasDrawn = false;
            myGame.teleportManager.portal1HasChanged = false;

        }

    }

    protected void UpdateScreenPosition()
    {
        x = position.x;
        y = position.y;
    }

    protected virtual void Draw(byte red, byte green, byte blue)
    {
        Clear(Color.Empty);
        Fill(red, green, blue);
        Stroke(red, green, blue);
        Ellipse(radius, radius, 2 * radius, 2 * radius);
    }

    void Update() {
        Shoot(RIGHT);
        DestroyLaser2();
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
        var result = ((MyGame)game).camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
        Vec2 shotDirection = new Vec2(1000f, 0);
        shotDirection.RotateDegrees(pAngle);
        Projectile bullet = new Projectile(position, shotDirection, 1,-1,this);
        parent.AddChild(bullet);
    }

}
