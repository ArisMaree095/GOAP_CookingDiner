using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFood : GAction
{
    public override bool PrePerform()
    {
        string stateKey = "foodDelivered" + this.gameObject.name;

        Debug.Log("<color=orange>CLIENT is listening for signal: " + stateKey + "</color>");
        if (!GWorld.Instance.GetWorld().HasState("foodDelivered" + this.gameObject.name))
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        // 1. Remove the "foodDelivered" state from the world.
        GWorld.Instance.GetWorld().RemoveState("foodDelivered" + this.gameObject.name);

        // 2. Set the client's internal belief that it is now satisfied.
        GetComponent<GAgent>().beliefs.SetState("isSatisfied", 1);

        return true;
    }
}
