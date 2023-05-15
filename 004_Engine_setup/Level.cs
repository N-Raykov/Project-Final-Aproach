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

    public Level(string filename)
    {
        currentLevelName = filename;

        loader = new TiledLoader(filename);
        //loader.rootObject = myGame ;
        CreateLevel();

        if (filename == "Title_Screen.tmx")
        {
            new Sound("Title_Theme.wav", true, true).Play();
        }
        else
        {
            new Sound("Background_Music_Level.wav", true, true).Play();
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

        Button[] b = FindObjectsOfType<Button>();
        Door[] d = FindObjectsOfType<Door>();
        foreach (Button button in b) {
            foreach (Door door in d) { 
                if (button.id==door.id)
                    button.SetDoor(door);
            }
        }

    }

    void Update()
    {
        //Console.WriteLine(GetChildCount());
    }
}
