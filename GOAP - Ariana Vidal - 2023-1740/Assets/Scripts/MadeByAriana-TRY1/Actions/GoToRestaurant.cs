using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRestaurant : GAction
{
    public override bool PrePerform()
    {
        target = GameObject.FindWithTag(targetTag);
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GetComponent<GAgent>().beliefs.SetState("atCounter", 1);
        return true;
    }
}
