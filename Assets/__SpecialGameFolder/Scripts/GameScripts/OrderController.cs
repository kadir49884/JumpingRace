using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderController : Singleton<OrderController>
{
    [SerializeField, ReadOnly] private List<OrderFinder> orderFinderList = new List<OrderFinder>();
    private CanvasManager canvasManager;

    private int finishOrderValue = 1;


    public List<OrderFinder> OrderFinderList { get => orderFinderList; set => orderFinderList = value; }

    private void Start()
    {
        canvasManager = CanvasManager.Instance;
    }
    public void AddList(OrderFinder getOrderFinder)
    {
        OrderFinderList.Add(getOrderFinder);
    }

    public void UpdateList()
    {
        OrderFinderList = OrderFinderList.OrderByDescending(x => x.OrderValue).ToList();
        for (int i = 0; i < OrderFinderList.Count; i++)
        {
            OrderFinderList[i].Crown.gameObject.SetActive(false);
        }
        canvasManager.PlayersOrder(OrderFinderList);

        if (OrderFinderList[0].OrderValue != 0)
        {
            OrderFinderList[0].Crown.gameObject.SetActive(true);
        }
    }

    public void GetFinishOrder(OrderFinder orderFinder)
    {
        orderFinder.FinishOrder = finishOrderValue;
        finishOrderValue++;
    }



}
