using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCollider : Singleton<FinishCollider>
{

    private OrderController orderController;
    private OrderFinder orderFinder;

    private void Start()
    {
        orderController = OrderController.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager gameManager = GameManager.Instance;

        if (collision.transform.TryGetComponent(out PlayerController playerController) && gameManager.RunGame)
        {
            orderFinder = playerController.GetComponent<OrderFinder>();
            orderController.GetFinishOrder(orderFinder);
            if (orderFinder.FinishOrder == 1)
            {
                orderFinder.OrderValue = 50;
                gameManager.GameWin();
                playerController.SetAnimValue(99);
            }
            else
            {
                playerController.GetComponent<Animator>().SetTrigger("Fail");

                gameManager.GameFail();
            }
            orderController.UpdateList();


        }
        if (collision.transform.TryGetComponent(out EnemyController enemyController))
        {
            orderFinder = enemyController.GetComponent<OrderFinder>();
            orderController.GetFinishOrder(orderFinder);
            if (orderFinder.FinishOrder == 1)
            {
                orderFinder.OrderValue = 50;
            }
            enemyController.SetAnimValue(99);
            orderController.UpdateList();

        }
    }
}
