using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;
using Physics;

public class TeleportManager{

    //public bool portal1HasChanged=false;
    //public bool portal2HasChanged = false;
    public bool[] portalsChanged = {false,false };

    public Teleporter[] portals = { null, null };

    public TeleportManager() { 
    }

}
