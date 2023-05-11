using System;
using System.Collections.Generic;
using GXPEngine;

public class MyGame : Game
{
    string startLevel = "Test_Level.tmx";
    string nextLevel;

	public Camera camera;
    public TeleportManager teleportManager;

	public MyGame() : base(1200, 800, false,false)
	{
        RenderMain = false;

        camera = new Camera(0, 0, 1200, 800);


        teleportManager = new TeleportManager();

        LoadLevel(startLevel);

        OnAfterStep += CheckLoadLevel;
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