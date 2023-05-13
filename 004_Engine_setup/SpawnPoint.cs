using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class SpawnPoint : AnimationSprite
{
    Player player;
    bool created = false;
    int playerSize = 30;

    public SpawnPoint(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        Initialize(obj);
    }

    void Initialize(TiledObject obj)
    {
        alpha = 0.0f;

        player = new Player(new Vec2(obj.X, obj.Y), playerSize);
    }

    void Update()
    {
        if (created == false)
        {
            parent.AddChild(player);

            parent.AddChild(((MyGame)game).camera);
            ((MyGame)game).camera.x = 600;
            ((MyGame)game).camera.y = 800;
            ((MyGame)game).camera.scale = 2;
            created = true;
        }
    }
}