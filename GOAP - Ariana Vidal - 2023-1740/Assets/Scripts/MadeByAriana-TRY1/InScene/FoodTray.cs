using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTray : MonoBehaviour
{
    public string customerName;
    public string tableName;
    public float spawnTime;
    public float maxLifetime = 300f; // 5 minutes

    void Start()
    {
        spawnTime = Time.time;
        Invoke(nameof(CleanupTray), maxLifetime);
    }

    public void CleanupTray()
    {
        string trayStateKey = "trayAt" + tableName;
        GWorld.Instance.GetWorld().RemoveState(trayStateKey);
        Destroy(gameObject);
    }
}
