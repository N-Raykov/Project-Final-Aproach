using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

public class UI:GameObject {

    MyGame myGame = (MyGame)Game.main;
    EasyDraw collectableShit = new EasyDraw(200, 50);
    //EasyDraw collectableShit;
    Sprite batteryIcon;

    public int totalCollectables = 0;
    public int collectablesCollected = 0;

    public UI() {
        
        AddChild(collectableShit);
        collectableShit.SetXY(-580,-380);
    }

    void DrawShit() {
        collectableShit.graphics.Clear(Color.Empty);
        //collectableShit.ShapeAlign(CenterMode.Min, CenterMode.Min);
        //collectableShit.NoStroke();
        //collectableShit.Fill(0, 255, 0);
        //collectableShit.Rect(0, 0, 200, 50);
        collectableShit.TextSize(20);
        collectableShit.Fill(255, 255, 255);
        collectableShit.TextAlign(CenterMode.Min, CenterMode.Min);
        collectableShit.TextAlign(CenterMode.Center, CenterMode.Center);
        collectableShit.Text(String.Format("{0}/{1}", collectablesCollected, totalCollectables));
        batteryIcon = new Sprite("Battery_new.png", false);
        collectableShit.AddChild(batteryIcon);
        batteryIcon.rotation = 40;
        batteryIcon.SetXY(parent.x + 150, parent.y - 15);

    }

    void Update() {

        DrawShit();
        
    }

}
