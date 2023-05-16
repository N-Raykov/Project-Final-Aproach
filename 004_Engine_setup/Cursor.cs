using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
public class Cursor:AnimationSprite{
    MyGame myGame = (MyGame)Game.main;
    public Cursor() : base("spritesheet.png",4,1,4) {
        SetOrigin(width / 2, height / 2);
        SetScaleXY(0.3f,0.3f);
    }


    void Update() {
        if (myGame.startLevel == "Game_Puzzle_TileSet_neeew.tmx")
        {
            var result = myGame.cameraManager.camera.ScreenPointToGlobal(Input.mouseX, Input.mouseY);
            SetXY(result.x, result.y);
            Console.WriteLine(result);
        }
        else {
            SetXY(Input.mouseX, Input.mouseY);

        }



        if (myGame.teleportManager.portals[0] == null && myGame.teleportManager.portals[1] == null) {
            SetFrame(0);
            return;
        }
        if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] == null)
        {
            SetFrame(1);
            return;
        }
        if (myGame.teleportManager.portals[0] == null && myGame.teleportManager.portals[1] != null)
        {
            SetFrame(2);
            return;
        }
        if (myGame.teleportManager.portals[0] != null && myGame.teleportManager.portals[1] != null)
        {
            SetFrame(3);
            return;
        }



    }

}

