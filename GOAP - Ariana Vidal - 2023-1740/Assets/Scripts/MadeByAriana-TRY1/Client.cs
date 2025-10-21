using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Client : GAgent
{
    protected override void Start()
    {
        base.Start();
        //Client's main goal is to get
        SubGoal s1 = new SubGoal("seated", 1, true);
        goals.Add(s1, 5); // High priority goal

        SubGoal s2 = new SubGoal("goHome", 1, true);
        goals.Add(s2, 1); // Low priority goal
    }

    public void OnFoodEaten()
    {
        goals.Clear();
        SubGoal goHomeGoal = new SubGoal("goHome", 1, true);
        goals.Add(goHomeGoal, 10);
        if (currentTable != null)
        {
            string trayStateKey = "trayAt" + currentTable.name;
            GWorld.Instance.GetWorld().RemoveState(trayStateKey);

            TableScript tableScript = currentTable.GetComponent<TableScript>();
            if (tableScript != null)
            {
                tableScript.CleanupFoodTray(); // Cleanup food tray if exists
                tableScript.isOccupied = false;
                tableScript.customerName = "";

                // Remove customer seated state from world
                GWorld.Instance.GetWorld().RemoveState("customerSeated" + this.gameObject.name);
            }
        }
    }
} 