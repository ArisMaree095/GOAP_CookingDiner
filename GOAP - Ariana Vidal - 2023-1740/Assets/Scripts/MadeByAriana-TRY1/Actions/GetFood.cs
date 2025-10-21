using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetFood : GAction
{
    private GameObject resource;

    public override bool PrePerform()
    {

   
        // Check if there are customers waiting for food
        bool hasWaitingCustomers = CheckForWaitingCustomers();

        if (!hasWaitingCustomers)
        {
            Debug.Log("<color=red>GetFood: No customers waiting for food</color>");
            return false;
        }

        target = GameObject.FindWithTag("Kitchen");
        if (target == null)
        {
            Debug.Log("<color=red>GetFood: Kitchen not found!</color>");
            return false;
        }

        Debug.Log("<color=cyan>GetFood: Going to kitchen to collect food for waiting customers</color>");
        return true;
    }

    private bool CheckForWaitingCustomers()
    {
        Client[] clients = FindObjectsOfType<Client>();
        foreach (Client client in clients)
        {
            bool isSeated = client.GetComponent<GAgent>().beliefs.HasState("seated");
            bool alreadyServed = GWorld.Instance.GetWorld().HasState("foodDelivered" + client.name);

            if (isSeated && !alreadyServed)
            {
                Debug.Log($"<color=cyan>Found waiting customer: {client.name}</color>");
                return true;
            }
       
        target = GameObject.FindWithTag("Kitchen");
        if (target == null)
        {
            return false;
        }

        return true;
    
 
     }
          return false;

   }
    public override bool PostPerform()
    {
        // Worker now has food
        GetComponent<GAgent>().beliefs.SetState("hasFood", 1);

        // Keep customerWaiting state if there are still more customers after this delivery
        // The DeliverFood action will handle removing this state when all are served

        Debug.Log("<color=green>Worker has collected food from kitchen</color>");
        return true;
    }
 }