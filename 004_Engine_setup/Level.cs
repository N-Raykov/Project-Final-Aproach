using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

using GXPEngine;

class Level : GameObject
{
    TiledLoader loader;
    string currentLevelName;
    MyGame myGame = (MyGame)Game.main;
    SoundChannel backGroundMusic;
    Cursor cursor = new Cursor();
    public static Pivot ObjectLayers= new Pivot();
    public static Pivot Platfroms = new Pivot();
    public static Pivot Doors = new Pivot();
    public static Pivot Background1 = new Pivot();
    float bg1Multiplier = 0.9f;
    public static Pivot Background2 = new Pivot();
    float bg2Multiplier = 0.7f;
    public static Pivot Background3 = new Pivot();
    float bg3Multiplier = 1.0f;
    Vec2 distanceTravelled = new Vec2(0, 0);
    Vec2 lastDistanceTravelled = new Vec2(0, 0);
    bool assSubstracted = false;

    public Level(string filename)
    {
        currentLevelName = filename;

        loader = new TiledLoader(filename);
        //loader.rootObject = myGame ;
        CreateLevel();


        if (filename == "Main_Menu.tmx")
        {
            backGroundMusic = new Sound("Title_Theme.wav", true, true).Play();
        }
        else
        {
            backGroundMusic = new Sound("Background_Music_Level.wav", true, true).Play();
        }

    }

    void LoadMenu()
    {
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.rootObject = this;
        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.LoadObjectGroups();

        AddChild(cursor);
    }

    void LoadLevel()
    {

        AddChild(Background3);
        AddChild(Background2);
        AddChild(Background1);
        AddChild(Doors);
        AddChild(Platfroms);
        AddChild(ObjectLayers);

        loader.addColliders = false;
        loader.autoInstance = true;

        loader.rootObject = Background3;
        loader.LoadObjectGroups(0);
        loader.rootObject = Background2;
        loader.LoadObjectGroups(1);
        loader.rootObject = Background1;
        loader.LoadObjectGroups(2);
        loader.rootObject = Doors;
        loader.LoadObjectGroups(3);
        loader.rootObject = Platfroms;
        loader.LoadTileLayers();
        loader.rootObject = ObjectLayers;
        loader.LoadObjectGroups(4);



        AddChild(cursor);

        Button[] b = FindObjectsOfType<Button>();
        Door[] d = FindObjectsOfType<Door>();
        foreach (Button button in b)
        {
            foreach (Door door in d)
            {
                if (button.id == door.id)
                    button.SetDoor(door);
            }
        }


        //AddChild(ui);

    }

    void CreateLevel(bool includeImageLayers = true)
    {
        myGame.startLevel = currentLevelName;
        Console.WriteLine("creating level");
        myGame.cameraManager.camera.x = 0;
        myGame.cameraManager.camera.y = 0;

        if (currentLevelName == "Game_Puzzle_TileSet_neeew.tmx")
        {
            LoadLevel();
        } 
        else
        {
            LoadMenu();
        }
    }

    void Update() {
        if (myGame.changedLevel) {
            myGame.teleportManager.shots = 50;
            myGame.changedLevel = false;
        }

        lastDistanceTravelled = distanceTravelled;
        distanceTravelled = myGame.cameraManager.distanceTravelled;

        if (distanceTravelled.x != lastDistanceTravelled.x && distanceTravelled.y != lastDistanceTravelled.y)
        {
            if (!assSubstracted)
            {
                distanceTravelled.x -= 600;
                assSubstracted = true;
            }

            //Tween tween1 = new Tween(TweenProperty.x, 2000, distanceTravelled.x * bg1Multiplier, 3);
            //Tween tween2 = new Tween(TweenProperty.y, 2000, distanceTravelled.y * bg1Multiplier, 3);
            //Background1.AddChild(tween1);
            //Background1.AddChild(tween2);

            Tween tween3 = new Tween(TweenProperty.x, 2000, distanceTravelled.x * bg2Multiplier, 3);
            Tween tween4 = new Tween(TweenProperty.y, 2000, distanceTravelled.y * bg2Multiplier, 3);
            Background2.AddChild(tween3);
            Background2.AddChild(tween4);

            Tween tween5 = new Tween(TweenProperty.x, 2000, distanceTravelled.x * bg3Multiplier, 3);
            Tween tween6 = new Tween(TweenProperty.y, 2000, distanceTravelled.y * bg3Multiplier, 3);
            Background3.AddChild(tween5);
            Background3.AddChild(tween6);

            distanceTravelled = myGame.cameraManager.distanceTravelled;
        }

    }
        
    protected override void OnDestroy()
    {
        backGroundMusic.Stop();
        base.OnDestroy();
    }
}
