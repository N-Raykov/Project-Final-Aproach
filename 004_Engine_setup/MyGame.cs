using System;
using System.Collections.Generic;
using GXPEngine;

public class MyGame : Game
{
    string startLevel = "Test_Level.tmx";
    string nextLevel;

	public Camera camera;
    public TeleportManager teleportManager;
    Player player;
	public MyGame() : base(1200, 800, false,false)
	{
        //RenderMain = false;

        //camera = new Camera(0, 0, 1200, 800);

        //teleportManager = new TeleportManager();

        //LoadLevel(startLevel);

        //OnAfterStep += CheckLoadLevel;

        RenderMain = false;
        player = new Player(new Vec2(750, 750), 30);
        camera = new Camera(0, 0, 1200, 800);
        player.AddChild(camera);

        teleportManager = new TeleportManager();
        AddChild(player);
        AddChild(new Line(new Vec2(0, 500), new Vec2(500, 1000)));
        AddChild(new Line(new Vec2(500, 1000), new Vec2(1500, 1000)));
        AddChild(new Line(new Vec2(1500, 1000), new Vec2(2000, 1300)));
        AddChild(new Line(new Vec2(2000, 1600), new Vec2(2500, 1600)));


        AddChild(new Line(new Vec2(500, 600), new Vec2(1500, 600)));



        //AddChild(new BouncyFloor(new Vec2(600,900),new Vec2(1000,900)));
        //AddChild(new CircleMapObject(30, new Vec2(1000, 500)));
        AddChild(new CircleMapObject(30, new Vec2(250, 250)));
    }

    void DestroyAll()
    {
        List<GameObject> children = GetChildren();
        foreach (GameObject child in children)
        {
            child.Destroy();
        }
    }

    public void LoadLevel(string pNextLevel)
    {
        nextLevel = pNextLevel;
    }

    void CheckLoadLevel()
    {
        if (nextLevel != null)
        {
            DestroyAll();

            AddChild(new Level(nextLevel));

            nextLevel = null;
        }
    }

    static void Main()
	{


        new MyGame().Start();
	}
}