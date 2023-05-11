using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class Ball : AnimationSprite
{
    CircleMapObject ball;
    bool created = false;
    int radius = 30;

    public Ball(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        alpha = 0.0f;

        Vec2 position = new Vec2(obj.X, obj.Y);

        ball = new CircleMapObject(radius, position);
    }

    void Update()
    {
        if (created == false)
        {
            parent.AddChild(ball);
            created = true;
        }
    }
}
