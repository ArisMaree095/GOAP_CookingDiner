using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : GAgent
{
    /*public float customerCheckInterval = 2f; // How often to check for new customers

    protected override void Start()
    {
       base.Start();

       // The goal for the worker is to deliver food
        SubGoal serveGoal = new SubGoal("serveCustomer", 1, false);
        goals.Add(serveGoal, 1); // High priority goal  
        StartCoroutine(ContinuousCustomerCheck());

    }
    private void EnsureServingGoal()
    {
        // Clear current goals and set serving as priority
        goals.Clear();
        SubGoal serveGoal = new SubGoal("serveCustomer", 1, false);
        goals.Add(serveGoal, 5); // High priority

        beliefs.RemoveState("isIdle");
        GWorld.Instance.GetWorld().RemoveState("workerIdle");
    }

    private void GoIdle()
    {
        goals.Clear();
        SubGoal idleGoal = new SubGoal("workerIdle", 1, true);
        goals.Add(idleGoal, 1);

        beliefs.SetState("isIdle", 1);
        GWorld.Instance.GetWorld().SetState("workerIdle", 1);
        GWorld.Instance.GetWorld().RemoveState("customerWaiting");
    }
    private int GetWaitingCustomersCount()
    {
        int count = 0;
        Client[] clients = FindObjectsOfType<Client>();

        foreach (Client client in clients)
        {
            bool isSeated = client.GetComponent<GAgent>().beliefs.HasState("seated");
            bool alreadyServed = GWorld.Instance.GetWorld().HasState("foodDelivered" + client.name);

            if (isSeated && !alreadyServed)
            {
                count++;
            }
        }

        return count;
    }

    private IEnumerator CheckForMoreCustomers()
    {
        yield return new WaitForSeconds(1f); // Small delay to let states update

        bool hasWaitingCustomers = GetWaitingCustomersCount() > 0;

        if (hasWaitingCustomers)
        {
            // There are more customers, continue serving
            Debug.Log("<color=yellow>Worker found more customers to serve!</color>");

            // Ensure serving goal is active
            EnsureServingGoal();

            // Set world state so GetFood action can trigger
            GWorld.Instance.GetWorld().SetState("customerWaiting", 1);
        }
        else
        {
            // No more customers, go idle
            Debug.Log("<color=blue>No more customers to serve, worker going idle.</color>");
            GoIdle();
        }
    }
    private IEnumerator ContinuousCustomerCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(customerCheckInterval);

            // If worker is idle and there are customers waiting, wake up
            if (beliefs.HasState("isIdle"))
            {
                int waitingCount = GetWaitingCustomersCount();
                if (waitingCount > 0)
                {
                    WakeUpForService();
                }
            }
        }
    }

    public void OnTaskFinished()
    {
        SubGoal goHomeGoal = new SubGoal("workerIdle", 1, true);
        goals.Add(goHomeGoal, 5); //Low priority goal
        StartCoroutine(CheckForMoreCustomers());
    } 
    public void WakeUpForService()
    {
        if (beliefs.HasState("isIdle"))
        {
            Debug.Log("<color=green>Worker waking up from idle to serve new customer!</color>");
            goals.Clear();
            SubGoal serveGoal = new SubGoal("serveCustomer", 1, false);
            goals.Add(serveGoal, 5);

            beliefs.RemoveState("isIdle");
            GWorld.Instance.GetWorld().RemoveState("workerIdle");
        }
    }*/

    public float customerCheckInterval = 2f; // How often to check for new customers

    protected override void Start()
    {
        base.Start();

        // The goal for the worker is to deliver food
        SubGoal serveGoal = new SubGoal("serveCustomer", 1, false);
        goals.Add(serveGoal, 5); // High priority goal  

        // Start checking for customers periodically
        StartCoroutine(ContinuousCustomerCheck());
    }

    public void OnTaskFinished()
    {
        // This is called when a single delivery is complete
        StartCoroutine(CheckForMoreCustomers());
    }

    private IEnumerator CheckForMoreCustomers()
    {
        yield return new WaitForSeconds(1f); // Small delay to let states update

        bool hasWaitingCustomers = GetWaitingCustomersCount() > 0;

        if (hasWaitingCustomers)
        {
            // There are more customers, continue serving
            Debug.Log("<color=yellow>Worker found more customers to serve!</color>");

            // Ensure serving goal is active
            EnsureServingGoal();

            // Set world state so GetFood action can trigger
            GWorld.Instance.GetWorld().SetState("customerWaiting", 1);
        }
        else
        {
            // No more customers, go idle
            Debug.Log("<color=blue>No more customers to serve, worker going idle.</color>");
            GoIdle();
        }
    }

    private IEnumerator ContinuousCustomerCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(customerCheckInterval);

            // If worker is idle and there are customers waiting, wake up
            if (beliefs.HasState("isIdle"))
            {
                int waitingCount = GetWaitingCustomersCount();
                if (waitingCount > 0)
                {
                    WakeUpForService();
                }
            }
        }
    }

    private int GetWaitingCustomersCount()
    {
        int count = 0;
        Client[] clients = FindObjectsOfType<Client>();

        foreach (Client client in clients)
        {
            bool isSeated = client.GetComponent<GAgent>().beliefs.HasState("seated");
            bool alreadyServed = GWorld.Instance.GetWorld().HasState("foodDelivered" + client.name);

            if (isSeated && !alreadyServed)
            {
                count++;
            }
        }

        return count;
    }

    private void EnsureServingGoal()
    {
        // Clear current goals and set serving as priority
        goals.Clear();
        SubGoal serveGoal = new SubGoal("serveCustomer", 1, false);
        goals.Add(serveGoal, 5); // High priority

        beliefs.RemoveState("isIdle");
        GWorld.Instance.GetWorld().RemoveState("workerIdle");
    }

    private void GoIdle()
    {
        goals.Clear();
        SubGoal idleGoal = new SubGoal("workerIdle", 1, true);
        goals.Add(idleGoal, 1);

        beliefs.SetState("isIdle", 1);
        GWorld.Instance.GetWorld().SetState("workerIdle", 1);
        GWorld.Instance.GetWorld().RemoveState("customerWaiting");
    }

    // Method to wake up idle workers when new customers arrive
    public void WakeUpForService()
    {
        if (beliefs.HasState("isIdle"))
        {
            int waitingCount = GetWaitingCustomersCount();
            Debug.Log($"<color=green>Worker waking up from idle to serve {waitingCount} customer(s)!</color>");

            EnsureServingGoal();
            GWorld.Instance.GetWorld().SetState("customerWaiting", 1);
        }
    }

    // Public method to manually trigger customer check (can be called when new customers arrive)
    public void CheckForNewCustomers()
    {
        StartCoroutine(CheckForMoreCustomers());
    }

}

