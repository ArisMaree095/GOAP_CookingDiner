using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class ReturnToKitchen : GAction
{
    public override bool PrePerform()
    {
        target = GameObject.FindWithTag("Kitchen");

        return target != null;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().SetState("workerIdle", 1);
        GetComponent<GAgent>().beliefs.SetState("isIdle", 1);
        return true;
    }
}
   
