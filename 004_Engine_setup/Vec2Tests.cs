using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

public class Vec2Tests
{


    public void StartTests()
    {

        Vec2 myVec = new Vec2(4, 3);
        Vec2 myVec2= new Vec2(1, 2);
        Console.Write("Printing to console: expected result is (4,3); actual result is ");
        Console.WriteLine(myVec);

        Console.WriteLine("Addition: expected result is (5,5); actual result is {0}",myVec+myVec2);
        Console.WriteLine("Subtraction: expected result is (3,1); actual result is {0}",myVec-myVec2);
        Console.WriteLine("Division: expected result is (2,1.5); actual result is {0}",myVec/2);
        Console.WriteLine("Multiplication(Vector * float): expected result is (12,9); actual result is {0}", myVec * 3);
        Console.WriteLine("Multiplication(float * vector): expected result is (12,9); actual result is {0}", 3 * myVec);
        Console.WriteLine("Vector Length: expected result is 5; actual result is {0}", myVec.Length());
        Console.WriteLine("Vector Normalized: expected result is (0.8,0.6); actual result is {0}", myVec.Normalized());
        myVec.Normalize();
        Console.WriteLine("Vector Normalize: expected result is (0.8,0.6); actual result is {0}", myVec);
        myVec.SetXY(5, 5);
        Console.WriteLine("Vector SetXY: expected result is (5,5); actual result is {0}", myVec);
        Console.WriteLine("Vector Deg2Rad: expected result is {0}; actual result is {1}", Mathf.PI, Vec2.Deg2Rad(180f));
        Console.WriteLine("Vector Rad2Deg: expected result is 180; actual result is {0}", Vec2.Rad2Deg(Mathf.PI));


        Console.WriteLine("Vector GetUnitVectorRad: expected result is (-1,0); actual result is {0}", Vec2.GetUnitVectorRad(Mathf.PI));
        Console.WriteLine("Vector GetUnitVectorDeg: expected result is (-1,0); actual result is {0}", Vec2.GetUnitVectorDeg(180));
        Console.WriteLine("Vector RandomUnitVector: {0}",Vec2.RandomUnitVector());

        myVec = new Vec2(0, 5);
        myVec.SetAngleDegrees(90);
        Console.WriteLine("Vector SetAngleDegrees: expected result is (0,5); actual result is {0}", myVec);

        myVec = new Vec2(0, 5);
        myVec.SetAngleRadians(Mathf.PI / 2);
        Console.WriteLine("Vector SetAngleRadians: expected result is (0,5); actual result is {0}", myVec);

        myVec = new Vec2(4, 5);
        Console.WriteLine("Vector GetAngleRadiansTwoPoints():expected result is PI/4(0.78...); actual result is {0}", myVec.GetAngleRadiansTwoPoints(new Vec2(7, 8)));
        Console.WriteLine("Vector GetAngleDegreeTwoPoints():expected result is 45; actual result is {0}", myVec.GetAngleDegreesTwoPoints(new Vec2(7, 8)));

        myVec = new Vec2(0, 5);
        Console.WriteLine("Vector GetAngleRadians: expected result is PI/2(1.57...); actual result is {0}", myVec.GetAngleRadians());
        Console.WriteLine("Vector GetAngleDegrees: expected result is 90; actual result is {0}", myVec.GetAngleDegrees());

        myVec = new Vec2(4, 5);
        myVec.RotateDegrees(90);
        Console.WriteLine("Vector RotateDegrees((4,5) rotated by 90): expected result is (-5,4); actual result is {0}", myVec);
        myVec = new Vec2(4, 5);
        myVec.RotateRadians(Mathf.PI / 2);
        Console.WriteLine("Vector RotateRadians((4,5) rotated by PI/2): expected result is (-5,4); actual result is {0}", myVec);

        myVec = new Vec2(4, 6);
        myVec.RotateAroundDegrees(new Vec2(2, 1), 90);
        Console.WriteLine("Vector RotateAroundDegrees((4,5) rotated 90 around (2,1)): expected result is (-3,3); actual result is {0}", myVec);
        myVec = new Vec2(4, 6);
        myVec.RotateAroundRadians(new Vec2(2, 1), Mathf.PI / 2);
        Console.WriteLine("Vector RotateAroundRadians((4,5) rotated PI/2 around (2,1)): expected result is (-3,3); actual result is {0}", myVec);


        myVec = new Vec2(2, 3);
        Console.WriteLine("Vector Dot(Dot product): expected result is 23; actual result is {0}", myVec.Dot(new Vec2(4, 5)));

        myVec = new Vec2(3, 4);
        Console.WriteLine("Vector Normal: expected result is (-0.8,0.6); actual result is {0}", myVec.Normal());
        myVec = new Vec2(3, 4);
        Console.WriteLine("Vector ReverseNormal: expected result is (0.6,-0.8); actual result is {0}", myVec.ReverseNormal());

        myVec = new Vec2(0, 5);
        Vec2 other = new Vec2(400, 300);
        myVec.Reflect(1, other.Normal());
        Console.WriteLine("Vector relfect: expected result is (4.8, -1.4); actual result is {0} ", myVec);


        myVec = new Vec2(1,0);
        myVec2= new Vec2(0,1);
        myVec.RotateTowardsDegrees(myVec2,45);
        Console.WriteLine("RotateTowardsDegrees: expcted result is (0.7071,0.7071); actual result is {0}",myVec);
        myVec = new Vec2(1, 0);
        myVec2= new Vec2(0,-1);
        myVec.RotateTowardsDegrees(myVec2, 45);
        Console.WriteLine("RotateTowardsDegrees: expcted result is (0.7071,-0.7071); actual result is {0}", myVec);

        Console.WriteLine("\n\n");













    }



}
