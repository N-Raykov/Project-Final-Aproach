using System;
using GXPEngine;
using Physics;


class Platform : AnimationSprite {
	Collider myCollider;
	ColliderManager engine;

	public Platform(Vec2 position) : base("tilesheet.png",14,7) {
		SetFrame(28);

		// Create collider, and register it (as solid / collision object):
		// (Note: to get the platform behavior, we use a one sided line segment as collider)
		myCollider=new HorizontalLineSegment(this, position,width);
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