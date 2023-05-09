using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using GXPEngine;
using GXPEngine.Core;

namespace Physics
{

    public class BoxCollider : Collider
    {
        public float width
        {
            get { return _width; }
        }

        public float height
        {
            get { return _height; }
        }

        float _width;
        float _height;
        float _rotation;//in degrees
        float oldRotation;

        public float rotation
        {
            get { return _rotation; }
            set
            {
                oldRotation= _rotation;
                _rotation = value;
                UpdateRotationMatrix();
                RotateCorners();
            }
        }

        Vec2 origin;
        static Vec2 widthVec2;
        static Vec2 heightVec2;

        float[,] rotationMatrix = new float[2, 2];
        public Vec2[,] corners= new Vec2[2, 2];

        //only works if pPosition is the center of the parent object aka center of this object to
        public BoxCollider(GameObject pOwner, Vec2 pPosition, int pWidth, int pHeight, int pRotation = 0) : base(pOwner, pPosition)
        {
            _width = pWidth;
            _height = pHeight;
            origin = pPosition;
            rotation = pRotation;
            UpdateRotationMatrix();
            FindCorners();  

            widthVec2 = new Vec2(_width, 0);
            heightVec2 = new Vec2(0,_height);

        }

        void FindCorners()
        {
            corners[0, 0] = new Vec2(origin.x - width / 2, origin.y - height / 2);//top left
            corners[0, 1] = new Vec2(origin.x + width / 2, origin.y - height / 2);//top right
            corners[1, 0] = new Vec2(origin.x - width / 2, origin.y + height / 2);//bottom left
            corners[1, 1] = new Vec2(origin.x + width / 2, origin.y + height / 2);//bottom right

        }

        void UpdateRotationMatrix(){
            float rotationInRadians = Vec2.Deg2Rad(rotation);
            rotationMatrix[0, 0] = Mathf.Cos(rotationInRadians);//top left
            rotationMatrix[0, 1] = -Mathf.Sin(rotationInRadians);//top right
            rotationMatrix[1, 0] = Mathf.Sin(rotationInRadians);//bottom left
            rotationMatrix[1, 1] = Mathf.Cos(rotationInRadians);//bottom right
        }

        void RotateCorners() {

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++)
                    corners[i, j].RotateAroundDegrees(origin,rotation-oldRotation);
            }

        }

        public override bool Overlaps(Collider other)
        {
            if (other is BoxCollider)
            {
                return Overlaps((BoxCollider)other);
            }
            else if (other is Circle)
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

        public bool Overlaps(BoxCollider other)
        {

            return false;
        }

        //public bool Overlaps(Circle other)
        //{
        //    Vec2 diffVec = position - other.position;
        //    return (diffVec.Length() < width + other.radius);
        //}

        //public bool Overlaps(LineSegment other)//works as an overlap with a line not with a segment
        //{
        //    float dist = other.CalculateDistanceSegment(position);
        //    return (dist < width && dist > -width);
        //}


        //public override CollisionInfo GetEarliestCollision(Collider other, Vec2 velocity)
        //{
        //    if (other is AABB)
        //    {
        //        return GetEarliestCollision((AABB)other, velocity);
        //    }
        //    else if (other is Circle)
        //    {
        //        return GetEarliestCollision((Circle)other, velocity);
        //    }
        //    else if (other is LineSegment)
        //    {
        //        return GetEarliestCollision((LineSegment)other, velocity);
        //    }
        //    else
        //        throw new NotImplementedException();

        //}

        //CollisionInfo GetEarliestCollision(AABB other, Vec2 velocity)
        //{
        //    return new CollisionInfo(new Vec2(Mathf.Sign(velocity.x), 0), other, 0);
        //}




        //float CalculateBallTimeOfImpact(Circle mover, Vec2 velocity)
        //{

        //    Vec2 u = ((position - velocity) - mover.position);
        //    float a = Mathf.Pow(velocity.Length(), 2);
        //    float b = 2 * u.Dot(velocity);
        //    float c = Mathf.Pow(u.Length(), 2) - Mathf.Pow(width + mover.radius, 2);
        //    float D = Mathf.Pow(b, 2) - 4 * a * c;


        //    if (c < 0)
        //    {
        //        if (b < 0)
        //            return 0f;
        //        else
        //            return -1f;
        //    }

        //    if (a == 0f)
        //        return -1f;

        //    if (D < 0f)
        //        return -1f;

        //    float t = (-b - Mathf.Sqrt(D)) / (2 * a);

        //    if (0 <= t && t < 1)
        //        return t;

        //    return -1f;

        //}

        //public float CalculateLineTimeOfImpact(LineSegment other, Vec2 velocity)
        //{
        //    MyGame myGame = (MyGame)Game.main;
        //    Vec2 oldPosition = position - velocity;

        //    float distance1 = other.CalculateDistanceSegment(oldPosition) - width;
        //    float distance2 = -(position - oldPosition).Dot(other.GetSegmentVector().Normal());
        //    float timeOfImpact = (distance1 / distance2);

        //    if (distance2 <= 0)
        //        return -1f;
        //    if (distance1 >= 0)
        //        return timeOfImpact;
        //    if (distance1 >= -width)
        //        return 0f;

        //    return -1f;
        //}


        //public void Move(float stepX, float stepY)
        //{
        //    float r = _rotation * Mathf.PI / 180.0f;
        //    float cs = Mathf.Cos(r);
        //    float sn = Mathf.Sin(r);
        //    _matrix[12] = (_matrix[12] + cs * stepX - sn * stepY);
        //    _matrix[13] = (_matrix[13] + sn * stepX + cs * stepY);
        //}



    }
}