using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using TiledMapParser;

public class ButtonBase:AnimationSprite{

    protected EasyDraw popUp = new EasyDraw(100,100,false);

    public ButtonBase(string filename, int cols, int rows, TiledObject obj = null) : base(filename, cols, rows) {
        popUp.Fill(255,255,255);
        popUp.TextSize(8);
        popUp.TextAlign(CenterMode.Center,CenterMode.Min); 
    }

    protected virtual void Update() {
        //Animate(0.1f);
        parent.RemoveChild(popUp);
        if (HitTestPoint(Input.mouseX, Input.mouseY))
            OnMouseHover();
        if(HitTestPoint(Input.mouseX, Input.mouseY) && Input.GetMouseButtonUp(0))
            OnMouseClick();
    }

    protected virtual void OnMouseHover() {
            popUp.SetXY(Input.mouseX - 50, Input.mouseY + 50);
            parent.AddChild(popUp);
    }

    protected virtual void OnMouseClick() { 
    
    }



}
