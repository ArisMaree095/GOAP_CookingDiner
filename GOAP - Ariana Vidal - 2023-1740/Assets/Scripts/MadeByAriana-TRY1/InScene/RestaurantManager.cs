using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using JetBrains.Annotations;

public class RestaurantManager : MonoBehaviour
{
   /* public static RestaurantManager Instance{ get; private set; }

public List<Transform> tables;
    public Transform orderCounter;
    public Transform kitchen;
    public Transform entrance;

    private HashSet<Transform> occupiedTables;
    private Queue<Order> pendingOrders;

    void Awake()
{
    if (Instance != null && Instance != this)
    {
        Destroy(gameObject);
    }
    else
    {
        Instance = this;
        occupiedTables = new HashSet<Transform>();
        pendingOrders = new Queue<Order>();
    }

}

    public Transform GetTable()
    {
        return tables.FirstOrDefault(t => !occupiedTables.Contains(t));
    }

    public void OccupyTable(Transform table)
    {
        occupiedTables.Add(table);
    }

    public void FreeTable(Transform table)
    {
        occupiedTables.Remove(table);
    }

    public void PlaceOrder(GoapAgent customer, Transform table)
    {
        pendingOrders.Enqueue(new Order(customer, table));
        Debug.Log("New order placed by a customer at table: " + table.name);
    }

    public Order GetPendingOrder()
    {
        if (pendingOrders.Count > 0)
        {
            return pendingOrders.Dequeue();
        }
        return null;
    }

    public class Order
    {
        public GoapAgent Customer { get; }
        public Transform Table { get; }

        public Order (GoapAgent customer, Transform table)
        {
            Customer = customer;
            Table = table;
        }
    }
   */
}
