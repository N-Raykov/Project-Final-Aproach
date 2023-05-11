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
    float portalHalfLength = 50;

    public Teleporter(Vec2 pPosition,int pPortalNumber,Vec2 pNormal) : base(pPosition-new Vec2(50,0), pPosition + new Vec2(50, 0)) { 
        portalNumber= pPortalNumber;
        normal=pNormal;

        foreach (Collider col in colliders)
            col.owner = this;

        if (portalNumber == 0) {
            Draw(30, 144, 255);
            r = 30;
            g = 144;
            b = 255;
        }
        if (portalNumber == 1) {
            Draw(255, 127, 80);
            r = 255;
            g = 127;
            b = 80;
        }

    }

}
