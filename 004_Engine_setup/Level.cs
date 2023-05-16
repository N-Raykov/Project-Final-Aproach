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

    void CreateLevel(bool includeImageLayers = true)
    {
        
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.rootObject = this;
        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.LoadObjectGroups();

        AddChild(cursor);

        Button[] b = FindObjectsOfType<Button>();
        Door[] d = FindObjectsOfType<Door>();
        foreach (Button button in b) {
            foreach (Door door in d) { 
                if (button.id==door.id)
                    button.SetDoor(door);
            }
        }

       
        //AddChild(ui);

    }

    void Update() {
        //Console.WriteLine(1);\
        if (myGame.changedLevel) {
            myGame.teleportManager.shots = 50;
            myGame.changedLevel = false;
        }
    
    }
        
    protected override void OnDestroy()
    {
        backGroundMusic.Stop();
        base.OnDestroy();
    }
}
