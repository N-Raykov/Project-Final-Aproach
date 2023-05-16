﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class TeleportManager:GameObject{

    public bool[] portalsChanged = {false,false };

    public Teleporter[] portals = { null, null };

    public int shots = 50;//was 100

    public TeleportManager() { 
    }

    public void Update() {
        if (shots > 0)
            shots--;


        if (Input.GetKeyUp(Key.R)|| Input.GetKeyUp(Key.F)) {
            
            if (portals[0] != null) {
                portals[0].sprite.Destroy();
                portals[0].Destroy();
                portals[0] = null;
                portalsChanged[0] = false;
            }
            if (portals[1] != null){
                portals[1].sprite.Destroy();
                portals[1].Destroy();
                portals[1] = null;
                portalsChanged[1] = false;
            }

        }
    
    }


}
