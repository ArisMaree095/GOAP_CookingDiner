using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    public Transform traySpawnPoint; //for the tray to spawn at the table
    public bool isOccupied = false;
    public string customerName = "";
    //public GameObject foodTrayPrefab;

    public bool hasFoodTray = false;
    public int maxSeats = 1;
    public int currentCustomers = 0;

    void Update()
    {
        // Optional: Visual feedback for occupied tables
        GetComponent<Renderer>().material.color = isOccupied ? Color.red : Color.green;
    }

    public void CleanupFoodTray()
    {
        hasFoodTray = false;
        string trayStateKey = "trayAt" + name;
        GWorld.Instance.GetWorld().RemoveState(trayStateKey);

        // Find and destroy any food trays at this table
        GameObject[] trays = GameObject.FindGameObjectsWithTag("FoodTray"); // If you tag your trays
        foreach (GameObject tray in trays)
        {
            if (tray.name.Contains(name))
            {
                Destroy(tray);
                break;
            }
        }
    }
}
