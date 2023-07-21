using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    private GameManager gameManager;
    private ObjectManager objectManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        objectManager = ObjectManager.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out PlayerController playerController) && gameManager.RunGame)
        {
            playerController.transform.DOMoveY(transform.position.y - 1, 0.2f);
            playerController.GetComponent<OrderFinder>().OrderValue = -99;

            objectManager.CinemachineVirtualCamera.Follow = null;
            objectManager.CinemachineVirtualCamera.LookAt = null;

            WaterSplash(collision);


            OrderController.Instance.UpdateList();

            gameManager.GameFail();

        }

        if (collision.transform.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.DeadEnemy();
        }

    }

    private void WaterSplash(Collision collision)
    {
        Transform waterSplash = Instantiate(objectManager.WaterSplash);
        waterSplash.position = collision.contacts[0].point + Vector3.up * 1f;
    }

}
