using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderFinder : MonoBehaviour
{
    string characterName;

    [SerializeField] private GameObject crown;

    private OrderController orderController;
    [SerializeField, ReadOnly] private int finishOrder;


    [SerializeField, ReadOnly] private int orderValue = 0;

    private void Start()
    {
        orderController = OrderController.Instance;
        orderController.AddList(this);
        if (characterName == null)
        {
            characterName = "You";
            orderValue = -1;
        }
    }

    public int OrderValue { get => orderValue; set => orderValue = value; }
    public GameObject Crown { get => crown; set => crown = value; }
    public int FinishOrder { get => finishOrder; set => finishOrder = value; }
    public string CharacterName { get => characterName; set => characterName = value; }
}
