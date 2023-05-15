using System;
using System.Reflection;
using GXPEngine;

public struct Vec2
{
    static int count = 0;

    public float x;
    public float y;
    public static Random rand = new Random();
    public Vec2(float pX = 0, float pY = 0)
    {
        x = pX;
        y = pY;
    }

    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x + right.x, left.y + right.y);
    }
    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.x - right.x, left.y - right.y);
    }
    public static Vec2 operator *(float left, Vec2 right)
    {
        return new Vec2(left * right.x, left * right.y);
    }

    public static Vec2 operator *(Vec2 left, float right)
    {
        return new Vec2(left.x * right, left.y * right);
    }

    public static Vec2 operator /(Vec2 left, float right)
    {
        return new Vec2(left.x / right, left.y / right);
    }

    public override string ToString()
    {
        return String.Format("({0},{1})", x, y);
    }

    public float Length()
    {
        return (float)(Math.Sqrt(this.x * this.x + this.y * this.y));
    }

    public void Normalize()
    {
        float length = Length();
        if (length != 0)
        {
            this.x /= length;
            this.y /= length;
        }
    }

    public Vec2 Normalized()
    {
        float length = Length();
        if (length != 0)
            return new Vec2(this.x / length, this.y / length);
        else
            return new Vec2(0, 0);
    }

    public void SetXY(float pX, float pY)
    {
        x = pX;
        y = pY;
    }

    public static float Deg2Rad(float pAngle)
    {
        return ((float)pAngle / 180 * Mathf.PI);
    }
    public static float Rad2Deg(float pAngle)
    {
        return (pAngle / Mathf.PI * 180.0f);
    }

    public void SetAngleRadians(float pAngle)
    {
        Vec2 unit = GetUnitVectorRad(pAngle);
        this = unit * Length();
    }

    public void SetAngleDegrees(float pAngle)
    {
        pAngle = Deg2Rad(pAngle);
        SetAngleRadians(pAngle);
    }

    public void RotateRadians(float pAngle)
    {
        float oldX = this.x;
        float oldY = this.y;
        float sin = Mathf.Sin(pAngle);
        float cos = Mathf.Cos(pAngle);
        this.x = oldX * cos - oldY * sin;
        this.y = oldX * sin + oldY * cos;
    }
    public void RotateDegrees(float pAngle)
    {
        this.RotateRadians(Vec2.Deg2Rad(pAngle));
    }

    public static Vec2 GetUnitVectorRad(float pAngle)
    {
        return new Vec2(Mathf.Cos(pAngle), Mathf.Sin(pAngle));
    }
    public static Vec2 GetUnitVectorDeg(float pAngle)
    {
        return GetUnitVectorRad(Deg2Rad(pAngle));
    }

    public static Vec2 RandomUnitVector()
    {

        int angle = rand.Next(360);
        return GetUnitVectorDeg(angle);
    }

    public float GetAngleDegreesTwoPoints(Vec2 pPos)
    {
        float angle = GetAngleRadiansTwoPoints(pPos);
        angle *= (180.0f / Mathf.PI);
        return (angle + 360) % 360;
    }

    public float GetAngleRadiansTwoPoints(Vec2 pPos = new Vec2())
    {
        Vec2 pos = new Vec2();
        pos = pPos - this;
        float angle = Mathf.Atan2(pos.y, pos.x);
        return angle;
    }

    public float GetAngleDegrees()
    {
        return (Mathf.Atan2(y, x) * (180.0f / Mathf.PI));
    }

    public float GetAngleRadians()
    {
        return (Mathf.Atan2(y, x));
    }


    public void RotateAroundDegrees(Vec2 pPos, float pAngle)
    {
        Vec2 pos = new Vec2(this.x - pPos.x, this.y - pPos.y);
        pos.RotateDegrees(pAngle);
        pos += pPos;
        this = pos;
    }

    public void RotateAroundRadians(Vec2 pPos, float pAngle)
    {
        Vec2 pos = new Vec2(this.x - pPos.x, this.y - pPos.y);
        pos.RotateRadians(pAngle);
        pos += pPos;
        this = pos;
    }

    public float Dot(Vec2 pVec)
    {
        return (this.x * pVec.x + this.y * pVec.y);
    }

    public Vec2 Normal()
    {
        return new Vec2(-this.y, this.x).Normalized();
    }

    public Vec2 ReverseNormal()
    {
        return new Vec2(this.y, -this.x).Normalized();
    }


    public void Reflect(float pBounciness, Vec2 pVec)
    {

        this = (this - (1 + pBounciness) * (this.Dot(pVec)) * pVec);

    }

    public void RotateTowardsDegrees(Vec2 pTarget,float pMaxAngle=180) {
        if (pMaxAngle <= 0)
            pMaxAngle = 180;
        float targetRotation = pTarget.GetAngleDegrees();
        targetRotation = (targetRotation + 360) % 360;//make sure the angle is between 0 and 360
        float currentAngle = this.GetAngleDegrees();
        currentAngle = (currentAngle + 360) % 360;
        float oldAngle = currentAngle;


        if (targetRotation < currentAngle)
        {
            if (Math.Abs(currentAngle - targetRotation) >= 180)
                currentAngle += Math.Min(Math.Abs(targetRotation - currentAngle), pMaxAngle);
            else
                currentAngle -= Math.Min(Math.Abs(targetRotation - currentAngle), pMaxAngle);
        }
        else
        {
            if (Math.Abs(currentAngle - targetRotation) >= 180)
                currentAngle -= Math.Min(Math.Abs(targetRotation - currentAngle), pMaxAngle);
            else
                currentAngle += Math.Min(Math.Abs(targetRotation - currentAngle), pMaxAngle);

        }
        RotateDegrees(currentAngle-oldAngle);

    }
    public static float Dist(Vec2 pVec1,Vec2 pVec2) {
        return ((pVec1 - pVec2).Length());
    }

}






