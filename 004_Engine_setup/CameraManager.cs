using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

public class CameraManager
{
    public Camera camera;

    public CameraManager()
    {
        camera = new Camera(0, 0, 1200, 800);
    }
}

