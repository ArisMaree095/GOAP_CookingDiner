using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject clientPrefab;
    public int startingClients = 3;
    public float minSpawnDelay = 10f;
    public float maxSpawnDelay = 15f;

    public GameObject workerPrefab;
    public Transform workerSpawnPoint;
    public int numberOfWorkers = 2;

    void Start()
    {
        // Make sure all tables are properly initialized first
        InitializeTables();

        // Refresh the GWorld table list to make sure it's up to date
        GWorld.RefreshTableList();

        // Spawn workers
        for (int i = 0; i < numberOfWorkers; i++)
        {
            if (workerPrefab != null && workerSpawnPoint != null)
            {
                Instantiate(workerPrefab, workerSpawnPoint.position, workerSpawnPoint.rotation);
            }
        }

        // Spawn initial clients
        for (int i = 0; i < startingClients; i++)
        {
            if (clientPrefab != null)
            {
                GameObject temporal = Instantiate(clientPrefab, transform.position, transform.rotation);
                temporal.name += "" +  Random.Range(0, 10000);
            }
        }

        StartCoroutine(SpawnClientRoutine());
    }

    void InitializeTables()
    {
        GameObject[] tables = GameObject.FindGameObjectsWithTag("Table");
        foreach (GameObject table in tables)
        {
            TableScript tableScript = table.GetComponent<TableScript>();
            if (tableScript != null)
            {
                tableScript.isOccupied = false;
                tableScript.customerName = "";
                Debug.Log($"Initialized table: {table.name} at position {table.transform.position}");
            }
            else
            {
                Debug.LogWarning($"Table {table.name} is missing TableScript component!");
            }
        }
    }

    IEnumerator SpawnClientRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            if (clientPrefab != null)
            {
                Instantiate(clientPrefab, transform.position, transform.rotation);
            }
        }
    }
}

