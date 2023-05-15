using System;
using System.Collections.Generic;
using GXPEngine;

public class MyGame : Game
{
    string startLevel = "Game_Puzzle_TileSet_neeew.tmx";//"Game_Puzzle_TileSet_neeew - Copy.tmx";
    string nextLevel;

    public TeleportManager teleportManager;
    public CameraManager cameraManager;

	public MyGame() : base(1200, 800, false,false)
	{
        RenderMain = false;

        teleportManager = new TeleportManager();
        AddChild(teleportManager);
        

        cameraManager= new CameraManager();

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

    void Update() {
        teleportManager.Update();
        cameraManager.Update();
    }
}