using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class Teleporter:Line{

    public readonly int portalNumber;
    public readonly Vec2 normal;
    public float offset;
    float portalHalfLength = 50;
    public Sprite sprite;

    public Teleporter(Vec2 pPosition,int pPortalNumber,Vec2 pNormal) : base(pPosition-new Vec2(50,0), pPosition + new Vec2(50, 0)) { 
        portalNumber= pPortalNumber;
        normal=pNormal;

        new Sound("Portal_Spawn.wav").Play();

        foreach (Collider col in colliders)
            col.owner = this;

        if (portalNumber == 0) {
            Draw(30, 144, 255);
            r = 30;
            g = 144;
            b = 255;
            sprite = new Sprite("portal1.png", false);
            sprite.name = "test";
        }
        if (portalNumber == 1) {
            Draw(255, 127, 80);
            r = 255;
            g = 127;
            b = 80;
            sprite = new Sprite("portal2.png", false);
            sprite.name = "test";
        }

        AddChild(sprite);
        sprite.SetOrigin(sprite.width/2,sprite.height/2);

    }

    void Update() {
        sprite.SetXY(rotationOrigin.x,rotationOrigin.y);
    }

    protected override void OnDestroy()
    {
        
        base.OnDestroy();
    }

}
