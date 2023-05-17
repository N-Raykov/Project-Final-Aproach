using System;
using System.Collections.Generic;
using GXPEngine;

public class MyGame : Game
{
    public string startLevel = "Main_Menu.tmx";
    string nextLevel;
    public bool changedLevel = false;

    public TeleportManager teleportManager;
    public CameraManager cameraManager;
    public UI ui;

	public MyGame() : base(1200, 800, false,false)
	{
        RenderMain = true;//was true
        ShowMouse(false);
        teleportManager = new TeleportManager();
        AddChild(teleportManager);

        cameraManager = new CameraManager();

        ui = new UI();

        cameraManager.Initialize();

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
        changedLevel = true;
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

        //if (startLevel == "Main_Menu.tmx")
        //    RenderMain = true;
        //else
        //    RenderMain = true;
        //Console.WriteLine(cameraManager.camera.x + " " + cameraManager.camera.y);
    }
}