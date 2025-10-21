using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SitAtTable : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.RemoveTable();

    if (target == null)
    {
        Debug.LogWarning("<color=red>Client " + gameObject.name + " could not find an available table in the GWorld queue! The queue is empty.</color>");
        return false;
    }

    Debug.Log("<color=green>Client " + gameObject.name + " was given table: " + target.name + "</color>");

    TableScript tableScript = target.GetComponent<TableScript>();
    if (tableScript == null)
    {
        Debug.LogError($"Table {target.name} is missing its TableScript component!");
        return false;
    }
    tableScript.isOccupied = true;
    tableScript.customerName = this.gameObject.name;

    GWorld.Instance.GetWorld().SetState("customerSeated" + this.gameObject.name, 1);
    return true;

    }

    public override bool PostPerform()
    {
        GetComponent<GAgent>().beliefs.SetState("seated", 1);
        GetComponent<GAgent>().currentTable = target;

        GWorld.Instance.GetWorld().SetState("customerWaiting", 1);

        Worker[] workers = FindObjectsOfType<Worker>();
        foreach (Worker worker in workers)
        {
            worker.WakeUpForService();
        }
        return true;
    }
}



