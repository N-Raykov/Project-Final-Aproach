using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

using GXPEngine.Core;
using GXPEngine;

class CameraTrigger : AnimationSprite
{
    bool created = false;
    List<Vec2> vectorList = new List<Vec2>();
    List<Line> lineList = new List<Line>();
    int target;

    public CameraTrigger(TiledObject obj = null) : base("tilesheet.png", 1, 1, -1, false, false)
    {
        if (obj.polyLines != null)
        {
            Initialize(obj, obj.polyLines.Points);
        }
        else if (obj.polygonPoints != null)
        {
            Initialize(obj, obj.polygonPoints.Points);
        }
    }

    public int GiveTarget()
    {
        return target;
    }
    
    void Initialize(TiledObject obj, string nodes)
    {
        alpha = 0.0f;

        target = obj.GetIntProperty("nextLocation", 0);

        //Define the relative positions of the nodes and put them in a list
        List<string> nodeLocations = nodes.Split(' ').ToList();

        foreach (string str in nodeLocations)
        {
            string[] components = str.Split(',');
            float x = float.Parse(components[0]);
            float y = float.Parse(components[1]);
            Vec2 vector = new Vec2(x + obj.X, y + obj.Y);
            vectorList.Add(vector);
        }

        if (obj.polygonPoints != null)
        {
            vectorList.Add(vectorList[0]);
        }

        //Create lines between each of the objects and store them in a list
        for (int i = 0; i < vectorList.Count - 1; i++)
        {
            Vec2 start = vectorList[i];
            Vec2 end = vectorList[i + 1];
            Line line = new Line(start, end, false, true);
            lineList.Add(line);
        }


    }

    void Update()
    {
        if (created == false)
        {
            foreach (Line line in lineList)
            {
                AddChild(line);
            }
            created = true;
        }
    }
}