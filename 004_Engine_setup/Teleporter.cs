using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class Teleporter:CircleObjectBase{

    public readonly int portalNumber;
    public readonly Vec2 normal;

    public Teleporter(Vec2 pPosition,int pPortalNumber,Vec2 pNormal) : base(40,pPosition) { 
        portalNumber= pPortalNumber;
        normal=pNormal;
        if (portalNumber == 0) {
            Draw(0, 0, 255);
        }
        if (portalNumber == 1) {
            Draw(255, 127, 80);
        }

    }
    protected override void AddCollider()
    {
        engine.AddTriggerCollider(myCollider);

    }

    protected override void OnDestroy()
    {
        engine.RemoveTriggerCollider(myCollider);
    }
}
