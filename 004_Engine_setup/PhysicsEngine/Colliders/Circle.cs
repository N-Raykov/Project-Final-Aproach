using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using GXPEngine;
using GXPEngine.Core;

namespace Physics
{

    public class Circle : Collider
    {

        const float epsilon = 0.001f;//might need to add more of these
        public float radius
        {
            get { return _radius; }
        }
        int _radius;
        public Circle(GameObject pOwner, Vec2 pPosition, int pRadius) : base(pOwner, pPosition)
        {
            _radius= pRadius;
        }


        public override bool Overlaps(Collider other)
        {
            if (other is Circle)
            {
                return Overlaps((Circle)other);
            }
            else if (other is LineSegment)
            {
                return Overlaps((LineSegment)other);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool Overlaps(Circle other)
        {
            Vec2 diffVec = position - other.position;
            return (diffVec.Length() < radius + other.radius);
        }

        public bool Overlaps(LineSegment other)//works as an overlap with a line not with a segment
        {
            float dist = other.CalculateDistanceSegment(position);
            return (dist < radius&&dist>-radius);
        }


        public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        {
            if (other is AABB)
            {
                return GetEarliestCollision((AABB)other, velocity);
            }
            else if (other is Circle)
            {
                return GetEarliestCollision((Circle)other, velocity);
            }
            else if (other is LineSegment) {    
                return GetEarliestCollision((LineSegment)other,velocity);
            }    
            else
                throw new NotImplementedException();
            
        }

        CollisionInfo GetEarliestCollision(AABB other, Vec2 velocity)
        {
            return new CollisionInfo(new Vec2(Mathf.Sign(velocity.x), 0), other, 0);
        }

        CollisionInfo GetEarliestCollision(Circle other, Vec2 velocity) 
        {
            Vec2 relativePosition = position - other.position;
            if (relativePosition.Length() < radius + other.radius){

                Vec2 oldPosition = position - velocity;
                float timeOfImpact = CalculateBallTimeOfImpact(other,velocity);

                if (timeOfImpact < 0 || timeOfImpact > 1)
                    return null;

                Vec2 POI = oldPosition + velocity * timeOfImpact;
                Vec2 unitNormal = (POI - other.position).Normalized();
                return(new CollisionInfo(unitNormal, other, timeOfImpact));
            }
            return null;
        }

        CollisionInfo GetEarliestCollision(LineSegment other, Vec2 velocity)
        {
            MyGame myGame = (MyGame)Game.main;
            float ballDistance = other.CalculateDistanceSegment(position);
            
            if (ballDistance+epsilon < this.radius)
            {

                float timeOfImpact = CalculateLineTimeOfImpact(other,velocity);
                if (timeOfImpact < 0 || timeOfImpact > 1)
                    return null;

                Vec2 segmentVector = other.GetSegmentVector();
                Vec2 normal = segmentVector.Normal();
                float distance = FindDistance(timeOfImpact, other, velocity);
                if (distance > 0 && distance < other.GetSegmentVector().Length())
                    return(new CollisionInfo(normal, other, timeOfImpact));
            }

            return null;
        }

        float FindDistance(float pTimeOfImpact, LineSegment pLine,Vec2 velocity)
        {
            MyGame myGame = (MyGame)Game.main;
            Vec2 POI = (position-velocity) + velocity * pTimeOfImpact;
            Vec2 vec = POI - pLine.start;
            Vec2 segmentVector = pLine.GetSegmentVector();
            segmentVector.Normalize();
            return vec.Dot(segmentVector);
        }


        float CalculateBallTimeOfImpact(Circle mover,Vec2 velocity)
        {

            Vec2 u = ((position-velocity) - mover.position);
            float a = Mathf.Pow(velocity.Length(), 2);
            float b = 2 * u.Dot(velocity);
            float c = Mathf.Pow(u.Length(), 2) - Mathf.Pow(radius + mover.radius, 2);
            float D = Mathf.Pow(b, 2) - 4 * a * c;


            if (c < 0)
            {
                if (b < 0)
                    return 0f;
                else
                    return -1f;
            }

            if (a ==0f)
                return -1f; 

            if (D < 0f)
                return -1f;

            float t = (-b - Mathf.Sqrt(D)) / (2 * a);

            if (0 <= t && t < 1)
                return t;

            return -1f;

        }

        public float CalculateLineTimeOfImpact(LineSegment other,Vec2 velocity)
        {
            MyGame myGame = (MyGame)Game.main;
            Vec2 oldPosition = position - velocity;

            float distance1 = other.CalculateDistanceSegment(oldPosition) - radius;
            float distance2 = -(position - oldPosition).Dot(other.GetSegmentVector().Normal());
            float timeOfImpact = (distance1 / distance2);

            if (distance2 <= 0)
                return -1f;
            if (distance1 >= 0)
                return timeOfImpact;
            if (distance1 >= -radius)
                return 0f;

            return -1f;
        }


    }
}