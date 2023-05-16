using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using TiledMapParser;

public class ButtonMenu:ButtonBase{
    string levelName;

    public ButtonMenu(string filename,int cols,int rows,TiledObject obj):base(filename,cols,rows) {
        levelName=obj.GetStringProperty("levelName");
    }

    protected override void OnMouseClick(){

        if (levelName != "none") {
            ((MyGame)game).startLevel = levelName;
            ((MyGame)game).LoadLevel(levelName);
        }
    }

}
