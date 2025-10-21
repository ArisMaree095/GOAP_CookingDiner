using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld _instance = new GWorld();
    private static WorldStates world;
    private static List<GameObject> availableTables;

    static GWorld()
    {
        world = new WorldStates();
        availableTables = new List<GameObject>();

        // Initialize available tables list
        RefreshTableList();
    }

    private GWorld() { }

    public static GWorld Instance
    {
        get { return _instance; }
    }

    public static void RefreshTableList()
    {
        availableTables.Clear();
        GameObject[] tableObjects = GameObject.FindGameObjectsWithTag("Table");

        foreach (GameObject table in tableObjects)
        {
            TableScript tableScript = table.GetComponent<TableScript>();
            if (tableScript != null && !tableScript.isOccupied)
            {
                availableTables.Add(table);
            }
        }

        Debug.Log($"<color=cyan>Refreshed table list: {availableTables.Count} available tables</color>");
    }

    public WorldStates GetWorld()
    {
        return world;
    }

    public void AddTable(GameObject table)
    {
        if (table != null && !availableTables.Contains(table))
        {
            TableScript tableScript = table.GetComponent<TableScript>();
            if (tableScript != null)
            {
                tableScript.isOccupied = false;
                tableScript.customerName = "";
            }

            availableTables.Add(table);
            Debug.Log($"<color=green>Table {table.name} returned to available tables. Total available: {availableTables.Count}</color>");
        }
    }

    public GameObject RemoveTable()
    {
        // Clean up null references
        availableTables.RemoveAll(table => table == null);

        // Remove any occupied tables from the list
        for (int i = availableTables.Count - 1; i >= 0; i--)
        {
            if (availableTables[i] != null)
            {
                TableScript tableScript = availableTables[i].GetComponent<TableScript>();
                if (tableScript != null && tableScript.isOccupied)
                {
                    availableTables.RemoveAt(i);
                }
            }
        }

        // FIXED: Check availableTables.Count, not tables.Count
        if (availableTables.Count == 0)
        {
            Debug.Log("<color=red>No available tables in list!</color>");
            return null;
        }

        // Get a random available table
        int randomIndex = Random.Range(0, availableTables.Count);
        GameObject selectedTable = availableTables[randomIndex];
        availableTables.RemoveAt(randomIndex);

        Debug.Log($"<color=yellow>Selected random table: {selectedTable.name}. Remaining available: {availableTables.Count}</color>");
        return selectedTable;
    }

    public GameObject RemoveClosestTable(Vector3 position)
    {
        availableTables.RemoveAll(table => table == null);

        for (int i = availableTables.Count - 1; i >= 0; i--)
        {
            if (availableTables[i] != null)
            {
                TableScript tableScript = availableTables[i].GetComponent<TableScript>();
                if (tableScript != null && tableScript.isOccupied)
                {
                    availableTables.RemoveAt(i);
                }
            }
        }

        if (availableTables.Count == 0) return null;

        GameObject closestTable = availableTables
            .OrderBy(table => Vector3.Distance(position, table.transform.position))
            .FirstOrDefault();

        if (closestTable != null)
        {
            availableTables.Remove(closestTable);
        }

        return closestTable;
    }

    public int GetAvailableTableCount()
    {
        availableTables.RemoveAll(table => table == null);
        for (int i = availableTables.Count - 1; i >= 0; i--)
        {
            if (availableTables[i] != null)
            {
                TableScript tableScript = availableTables[i].GetComponent<TableScript>();
                if (tableScript != null && tableScript.isOccupied)
                {
                    availableTables.RemoveAt(i);
                }
            }
        }
        return availableTables.Count;
    }

    public void ClearWorld()
    {
        world.GetStates().Clear();
    }
}