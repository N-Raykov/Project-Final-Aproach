using System;
using GXPEngine;
using Physics;


class SolidBlock : AnimationSprite {
	Collider myCollider;
	ColliderManager engine;

	public SolidBlock(Vec2 position) : base("tilesheet.png",14,7) {
		SetFrame(17);
		SetOrigin(width/2, height/2);

		// Create collider, and register it (as solid / collision object):
		myCollider=new AABB(this, position, width/2, height/2);
		engine=ColliderManager.main;
		engine.AddSolidCollider(myCollider);

		x=position.x;
		y=position.y;
	}

	protected override void OnDestroy() {
		// Remove the collider when the sprite is destroyed:
		engine.RemoveSolidCollider(myCollider);
	}
}