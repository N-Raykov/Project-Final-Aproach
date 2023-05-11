using System;
using GXPEngine;

public class MyGame : Game
{

	Player player;
	public Camera camera;
    public TeleportManager teleportManager;

	public MyGame() : base(1200, 800, false,false)
	{
        RenderMain = false;
        player = new Player(new Vec2(750, 750), 30);
        camera = new Camera(0, 0, 1200, 800);
        player.AddChild(camera);

        teleportManager = new TeleportManager();
        AddChild(player);
        AddChild(new Line(new Vec2(0, 500), new Vec2(500, 1000)));
        AddChild(new Line(new Vec2(500,1000),new Vec2(1500,1000)));
        AddChild(new Line(new Vec2(1500, 1000), new Vec2(2000, 1300)));
        AddChild(new Line(new Vec2(2000, 1600), new Vec2(2500, 1600)));


        //AddChild(new Line(new Vec2(500, 600), new Vec2(1500, 600)));



        //AddChild(new BouncyFloor(new Vec2(600,900),new Vec2(1000,900)));
        //AddChild(new CircleMapObject(30, new Vec2(1000, 500)));
        AddChild(new CircleMapObject(30, new Vec2(250, 250)));

        //AddChild(new Player(new Vec2(850, 750), 30));


        //AddChild(new Line(new Vec2(400, 600), new Vec2(600, 400)));
        //AddChild(new Line(new Vec2(600, 400), new Vec2(1150, 600)));
        //AddChild(new Line(new Vec2(1150, 600), new Vec2(1700, 400)));
        //AddChild(new Line(new Vec2(1700, 400), new Vec2(1900, 600)));


        //AddChild(new Line(new Vec2(400, 1700), new Vec2(600, 1900)));
        //AddChild(new Line(new Vec2(600, 1900), new Vec2(1150, 1700)));
        //AddChild(new Line(new Vec2(1150, 1700), new Vec2(1700, 1900)));
        //AddChild(new Line(new Vec2(1700, 1900), new Vec2(1900, 1700)));

        //AddChild(new CircleMapObject(750, new Vec2(-110, 1150), new Vec2(), false));
        //AddChild(new CircleMapObject(750, new Vec2(2410, 1150), new Vec2(), false));

        //AddChild(new CircleMapObject(30, new Vec2(750, 700)));
        //AddChild(new CircleMapObject(30, new Vec2(1500, 1600)));
        //AddChild(new CircleMapObject(30, new Vec2(750, 1600)));

        //AddChild(new Line(new Vec2(1050, 800), new Vec2(1250, 800), true));
        //AddChild(new Line(new Vec2(1050, 1500), new Vec2(1250, 1500), true));

        //AddChild(new Enemy(new Vec2(1150, 1150), 30, player));


        //Vec2Tests test = new Vec2Tests();
        //test.StartTests();

        //AddChild(new BoxTest(new Vec2(300,300)));

        //Console.WriteLine(Vec2.Dist(new Vec2(0,0),new Vec2(0,10)));

    }

    static void Main()
	{


        new MyGame().Start();
	}
}