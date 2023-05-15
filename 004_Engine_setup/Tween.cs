using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GXPEngine;

public enum TweenProperty { x, y, rotation,scale };

class Tween : GameObject{
    List<TweenProperty>targets=new List<TweenProperty> { };
    int totalTimeMs;
    float delta;

    float lastCurveValue = 0;
    int currentTimeMs = 0;

    int functionSelected = 0;
    bool destroy = false;

    public Tween(TweenProperty target, int timeMs, float delta,int function){
        this.targets .Add (target);
        totalTimeMs = timeMs;
        this.delta = delta;
        functionSelected= function;
    }

    public Tween(TweenProperty target1, TweenProperty target2, int timeMs, float delta, int function) :this(target1,timeMs,delta,function)
    { 
        this.targets.Add (target2);
    }

    public Tween(TweenProperty target1, TweenProperty target2,TweenProperty target3, int timeMs, float delta, int function) : this(target1,target2, timeMs, delta,function){
        this.targets.Add(target3);
    }

    void ApplyTween(){

        float newCurveValue = GetCurveValue(1f * currentTimeMs / totalTimeMs) * delta;

        float outputDelta = newCurveValue - lastCurveValue;

        for (int i=0;i<targets.Count; i++){
            switch (targets[i]) {
                case TweenProperty.x:
                    parent.x += outputDelta;
                    Console.WriteLine("x");
                    break;
                case TweenProperty.y:
                    parent.y += outputDelta;
                    Console.WriteLine("y");
                    break;
                case TweenProperty.rotation:
                    parent.rotation += outputDelta/5.0f;
                    Console.WriteLine("rotation");
                    break;
                case TweenProperty.scale:
                    if (parent is Camera) {
                        parent.scale += outputDelta;
                    }
                    break;
            }

        }
        lastCurveValue = newCurveValue;

    }

    float GetCurveValue(float t){
        t = Mathf.Clamp(t, 0, 1);

        switch (functionSelected) {
            case 1:
                return(t * t - 2 * t + (float)Math.Sqrt(t));
            case 2:
                return 1 - (1 - t) * (1 - t);
            case 3:
                return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
            default:
                return 0f;
        
        }

    }

    void Update(){

        if (parent == null) return;

        currentTimeMs += Time.deltaTime;

        ApplyTween();

        if (currentTimeMs >= totalTimeMs){
            if (destroy)
                parent.LateDestroy();
            Destroy();
        }
    }

    public void SetDestroy() {
        destroy = true;
    }
}

