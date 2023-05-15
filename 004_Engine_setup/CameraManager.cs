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
    public List<float> cameraScales=new List<float>();
    bool isMoving = false;
    int startTime = -1000;
    int duration = 2000;

    public CameraManager()
    {
        camera = new Camera(0, 0, 1200, 800);
        camera.x = 0;//600
        camera.y = 0;//800

    }

    public void MoveCamera(Collider trigger, int target)
    {
        if (!isMoving) {

            if (target == 0)
            {
                camera.x = cameraLocations[target].x;
                camera.y = cameraLocations[target].y;
                camera.scale = cameraScales[target];
                trigger.owner.parent.Destroy();
            }
            else
            {
                Console.WriteLine(camera.x+" "+camera.y+" "+camera.scale+" " + cameraLocations[target]+" " + cameraScales[target]);
                if (Mathf.Abs(camera.x - cameraLocations[target].x)<1 && Mathf.Abs(camera.y - cameraLocations[target].y)<1 && Mathf.Abs(camera.scale - cameraScales[target])<0.01f) {
                    target--;
                }


                startTime = Time.time;
                isMoving = true;
                Tween tween1 = new Tween(TweenProperty.x, duration, cameraLocations[target].x - camera.x, 3);
                Tween tween2 = new Tween(TweenProperty.y, duration, cameraLocations[target].y - camera.y, 3);
                Tween tween3 = new Tween(TweenProperty.scale, duration, cameraScales[target] - camera.scale, 3);
                camera.AddChild(tween1);
                camera.AddChild(tween2);
                camera.AddChild(tween3);
            }


        }

        
    }

    public void Update() {
        if (Time.time - startTime > duration) {
            isMoving = false;
        }
    }
}
