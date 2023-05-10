using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMapParser;

using GXPEngine;

class Level : GameObject
{
    TiledLoader loader;
    Player player;
    string currentLevelName;

    public Level(string filename)
    {
        currentLevelName = filename;

        loader = new TiledLoader(filename);

        CreateLevel();
    }

    void CreateLevel(bool includeImageLayers = true)
    {
        player = new Player(new Vec2(100, 0), 30);
        AddChild(player);
        AddChild(new Line(new Vec2(0, 500), new Vec2(500, 500)));

        loader.addColliders = false;
        loader.LoadImageLayers();
        loader.rootObject = this;
        loader.LoadTileLayers();
        loader.autoInstance = true;
        loader.LoadObjectGroups();
    }
}

