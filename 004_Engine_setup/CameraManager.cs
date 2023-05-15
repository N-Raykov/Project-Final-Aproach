using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;
using TiledMapParser;
using GXPEngine;

public class CameraManager
{
    public Camera camera;
    public List<Vec2> cameraLocations = new List<Vec2>();

    public CameraManager()
    {
        camera = new Camera(0, 0, 1200, 800);
        camera.x = 600;
        camera.y = 800;
        camera.scale = 2;
    }

    public void MoveCamera(Collider trigger, int target)
    {
        camera.x = cameraLocations[target].x;
        camera.y = cameraLocations[target].y;
        trigger.owner.parent.Destroy();
    }
}
