using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderFood : GAction
{
    public override bool PrePerform()
    {
            return true;
    }

    public override bool PostPerform()
    {
        //GWorld.Instance.GetWorld().SetState("customerWaiting", 1);

        //Debug.Log("CAMERA 1 (OrderFood): 'customerWaiting' state has been ADDED to GWorld.");

        GetComponent<GAgent>().beliefs.SetState("hasOrdered", 1);
        return true;
    }
}
