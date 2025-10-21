using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveRestaurant : GAction
{
    public override bool PrePerform()
    {
        target = GameObject.FindWithTag("ExitSpot");
        if (target == null)
        {
            return false;
        }
        return true;
    }   
public override bool PostPerform()
    {
        GameObject tray = GameObject.Find("FoodTray_" + this.name);
        if (tray != null)
        {
            Destroy(tray);
            Debug.Log($"<color=red>Cleaned up food tray for {this.name}</color>");
        }
        Destroy(this.gameObject, 1.0f); 
        return true;
    }


}
