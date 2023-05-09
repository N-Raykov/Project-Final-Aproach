using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

namespace Physics
{

    /// <summary>
    /// A superclass for all shapes in our physics engine (like circles, lines, AABBs, ...)
    /// </summary>
    public class LineSegment: Collider
    {
        public Vec2 start;
        public Vec2 end;
        public LineSegment(GameObject pOwner, Vec2 pPosition1, Vec2 pPosition2) : base(pOwner, pPosition1)
        {
            start=pPosition1;
            end=pPosition2;
        }

        public Vec2 GetSegmentVector()
        {
            return (this.end - this.start);
        }

        public float CalculateDistanceSegment(Vec2 pPosition)
        {
            
            MyGame myGame = (MyGame)Game.main;
            Vec2 segmentVector = this.GetSegmentVector();
            Vec2 differenceVector = pPosition - this.start;
            Vec2 normal = segmentVector.Normal();
            return differenceVector.Dot(normal);
        }
       
    }
}
