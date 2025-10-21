using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DeliverFood : GAction
{
    private GameObject targetCustomer;
    public GameObject foodTrayPrefab;
    public GameObject targetTable;
    private List<Client> waitingCustomers;

    public override bool PrePerform()
    {
        targetCustomer = null;
        targetTable = null;
        waitingCustomers = new List<Client>();

        // Find ALL customers who are seated and haven't been served yet
        Client[] clients = FindObjectsOfType<Client>();
        foreach (Client client in clients)
        {
            bool isSeated = client.GetComponent<GAgent>().beliefs.HasState("seated");
            bool alreadyServed = GWorld.Instance.GetWorld().HasState("foodDelivered" + client.name);

            if (isSeated && !alreadyServed)
            {
                waitingCustomers.Add(client);
            }
        }

        if (waitingCustomers.Count == 0)
        {
            Debug.Log("<color=red>PREPERFORM FAILED: No customers waiting for food delivery.</color>");
            return false;
        }

        // Pick the first waiting customer (or implement priority logic here)
        Client selectedClient = waitingCustomers[0];
        targetCustomer = selectedClient.gameObject;
        targetTable = selectedClient.GetComponent<GAgent>().currentTable;

        if (targetTable == null)
        {
            Debug.Log("<color=red>PREPERFORM FAILED: Selected customer has no table assigned.</color>");
            return false;
        }

        // Set the delivery target to the customer's table
        Transform deliverySpot = targetTable.transform.Find("DeliverySpot");
        if (deliverySpot != null)
        {
            target = deliverySpot.gameObject;
            Debug.Log($"<color=green>Delivering to DeliverySpot at table {targetTable.name} for customer {targetCustomer.name}</color>");
        }
        else
        {
            target = targetTable;
            Debug.Log($"<color=green>Delivering directly to table {targetTable.name} for customer {targetCustomer.name}</color>");
        }

        return true;
    }

    public override bool PostPerform()
    {
        if (targetCustomer != null)
        {
            string stateKey = "foodDelivered" + targetCustomer.name;
           // string trayStateKey = "trayAt" + targetTable.name;
            GWorld.Instance.GetWorld().SetState(stateKey, 1);
        }

        // Remove hasFood from worker's beliefs
        GetComponent<GAgent>().beliefs.RemoveState("hasFood");

        // Spawn the food tray at the correct table
        if (targetTable != null && foodTrayPrefab != null)
        {
            TableScript tableScript = targetTable.GetComponent<TableScript>();
            Vector3 spawnPosition;

            if (tableScript != null && tableScript.traySpawnPoint != null)
            {
                spawnPosition = tableScript.traySpawnPoint.position;
                Debug.Log($"<color=yellow>Using spawn point at: {spawnPosition}</color>");
            }
            else
            {
                spawnPosition = targetTable.transform.position + Vector3.up * 1.0f;
                Debug.Log($"<color=yellow>Using fallback position at: {spawnPosition}</color>");
            }

            GameObject spawnedTray = Instantiate(foodTrayPrefab, spawnPosition, targetTable.transform.rotation);
            spawnedTray.name = "FoodTray_" + targetCustomer.name;
            Debug.Log($"<color=cyan>Food tray spawned for {targetCustomer.name}</color>");
        }
        else
        {
            Debug.LogError($"<color=red>Cannot spawn tray! targetTable: {targetTable != null}, foodTrayPrefab: {foodTrayPrefab != null}</color>");
        }

        // Check if there are more customers waiting - if so, don't finish task yet
        bool hasMoreCustomers = CheckForMoreWaitingCustomers();

        if (hasMoreCustomers)
        {
            Debug.Log($"<color=yellow>More customers waiting! Worker will continue serving.</color>");
            // Set a flag to get more food and continue serving
            GWorld.Instance.GetWorld().SetState("customerWaiting", 1);
            // Don't call OnTaskFinished() yet - let the worker continue
        }
        else
        {
            Debug.Log($"<color=blue>All customers served! Worker task complete.</color>");
            GetComponent<Worker>().OnTaskFinished();
        }

        return true;
    }

    private bool CheckForMoreWaitingCustomers()
    {
        Client[] clients = FindObjectsOfType<Client>();
        foreach (Client client in clients)
        {
            bool isSeated = client.GetComponent<GAgent>().beliefs.HasState("seated");
            bool alreadyServed = GWorld.Instance.GetWorld().HasState("foodDelivered" + client.name);

            if (isSeated && !alreadyServed)
            {
                return true;
            }
        }
        return false;
    }
}