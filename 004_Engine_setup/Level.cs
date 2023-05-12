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
        loader.rootObject = myGame ;
        CreateLevel();
    }

    void CreateLevel(bool includeImageLayers = true)
    {
        
        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.rootObject = this;
        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.LoadObjectGroups();
    }
}
