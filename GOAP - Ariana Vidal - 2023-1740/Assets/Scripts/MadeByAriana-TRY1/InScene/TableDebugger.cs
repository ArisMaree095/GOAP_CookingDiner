using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDebugger : MonoBehaviour
{
            [Header("Debug Info")]
        public bool showDebugInfo = true;

        void Update()
        {
            if (showDebugInfo && Input.GetKeyDown(KeyCode.T))
            {
                DebugTableStatus();
            }
        }

        void DebugTableStatus()
        {
            Debug.Log("=== TABLE STATUS DEBUG ===");

            GameObject[] allTables = GameObject.FindGameObjectsWithTag("Table");
            Debug.Log($"Total tables in scene: {allTables.Length}");
            Debug.Log($"Available tables in GWorld: {GWorld.Instance.GetAvailableTableCount()}");

            foreach (GameObject table in allTables)
            {
                TableScript script = table.GetComponent<TableScript>();
                if (script != null)
                {
                    string status = script.isOccupied ? $"OCCUPIED by {script.customerName}" : "AVAILABLE";
                    Debug.Log($"Table: {table.name} - {status} - Position: {table.transform.position}");
                }
            }

            Debug.Log("=== END DEBUG ===");
        }

        void OnGUI()
        {
            if (showDebugInfo)
            {
                GUI.Label(new Rect(10, 10, 300, 20), $"Available Tables: {GWorld.Instance.GetAvailableTableCount()}");
                GUI.Label(new Rect(10, 30, 300, 20), "Press 'T' for detailed table debug");
            }
        }
}
