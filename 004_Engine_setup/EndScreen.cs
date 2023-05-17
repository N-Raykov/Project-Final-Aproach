using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using TiledMapParser;

public class EndScreen : ButtonBase
{
    int frame = 0;
    int cooldown = 500;
    int lastPress = 0;

    public EndScreen(string filename, int cols, int rows, TiledObject obj) : base(filename, cols, rows)
    {
        
    }

    protected override void OnMouseClick()
    {
        if (Time.time - lastPress > cooldown)
        {
            frame++;
            lastPress = Time.time;
        }
    }

    protected override void Update()
    {
        base.Update();
        if(frame == frameCount)
        {
            ((MyGame)game).LoadLevel("finalestmenu.tmx");
        }
        SetFrame(frame);
        ((MyGame)Game.main).cameraManager.camera.x = 0;
        ((MyGame)Game.main).cameraManager.camera.y = 0;
        ((MyGame)Game.main).cameraManager.camera.scale = 1;
    }

}