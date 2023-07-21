using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{

    [SerializeField] private TrampolineType trampolineType;
    [SerializeField] private BoosterController boosterController;

    [SerializeField] private TextMeshPro trampolineNumberText;
    [SerializeField] private GameObject triggerParticle;
    [SerializeField] private CanvasManager canvasManager;

    private Animator trampolineAnim;
    private int trampolineNumberValue;
    private int trampolineIndexInList;
    private GameManager gameManager;
    private Transform playerTransform;
    private float playerDistance;
    private Vector3 playerPos;
    private Vector3 trampolinePos;

    private BreakableParentController breakableParentController;

    private TrampolineManager trampolineManager;
    private OrderController orderController;

    public int TrampolineIndexInList { get => trampolineIndexInList; set => trampolineIndexInList = value; }
    public BoosterController BoosterController { get => boosterController; set => boosterController = value; }

    private void Start()
    {
        if (trampolineType != TrampolineType.Breakable)
        {
            trampolineAnim = transform.parent.GetComponentInChildren<SkinnedMeshRenderer>().GetComponent<Animator>();
        }
        else
        {
            breakableParentController = transform.parent.GetComponentInChildren<BreakableParentController>();
        }
        gameManager = GameManager.Instance;
        playerTransform = PlayerController.Instance.transform;
        trampolineManager = TrampolineManager.Instance;
        orderController = OrderController.Instance;
        canvasManager = CanvasManager.Instance;
    }


    private void Update()
    {
        if (!gameManager.RunGame)
        {
            return;
        }
        playerPos = playerTransform.position;
        trampolinePos = transform.position;
        playerPos.y = 0;
        trampolinePos.y = 0;
        playerDistance = Vector3.Distance(trampolinePos, playerPos);
        if (playerDistance < 4 && boosterController)
        {
            boosterController.OpenBooster(playerDistance);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out PlayerController playerController))
        {
            TriggerPlayer(playerController);
        }

        if (collision.transform.TryGetComponent(out EnemyController enemyController))
        {
            TriggerEnemy(enemyController);
        }
    }


    private void TriggerPlayer(PlayerController getPlayerController)
    {
        if (trampolineType != TrampolineType.Ground)
        {
          
            getPlayerController.SetLookAt(trampolineManager.GetLookTransform(TrampolineIndexInList));
            getPlayerController.ProgressBarControl(TrampolineIndexInList);
            getPlayerController.GetComponent<OrderFinder>().OrderValue = TrampolineIndexInList;
            orderController.UpdateList();
            triggerParticle.SetActive(true);

            if (getPlayerController.LastTrampoline && Vector3.Distance(getPlayerController.LastTrampoline.position, transform.position) > 15)
            {
                canvasManager.WriteSuccessText(16);
            }
            getPlayerController.LastTrampoline = transform;
        }

        boosterController.gameObject.SetActive(false);

        switch (trampolineType)
        {
            case TrampolineType.Static:
                getPlayerController.JumpActive();
                StartCoroutine(WaitForJumpAnim());
                break;
            case TrampolineType.Moveable:
                getPlayerController.JumpActive();
                StartCoroutine(WaitForJumpAnim());
                break;
            case TrampolineType.Breakable:
                getPlayerController.JumpActive();
                breakableParentController.BreakActive(transform.parent);
                break;
            case TrampolineType.Ground:
                getPlayerController.StrongJumpActive();
                Destroy(transform.parent.gameObject, 3);
                break;

        }
    }




    public void WriteTrampolineNumber(int getNumberValue)
    {
        trampolineNumberValue = getNumberValue;
        trampolineNumberText.text = trampolineNumberValue.ToString();
    }



    private void TriggerEnemy(EnemyController getEnemyController)
    {
        if (trampolineType != TrampolineType.Ground)
        {
            getEnemyController.SetLookAt(trampolineManager.GetLookTransform(TrampolineIndexInList));
            getEnemyController.GetComponent<OrderFinder>().OrderValue = TrampolineIndexInList;
            orderController.UpdateList();

        }
        switch (trampolineType)
        {
            case TrampolineType.Static:
                getEnemyController.JumpActive();
                StartCoroutine(WaitForJumpAnim());
                break;
            case TrampolineType.Moveable:
                getEnemyController.JumpActive();
                StartCoroutine(WaitForJumpAnim());
                break;
            case TrampolineType.Breakable:
                getEnemyController.JumpCounter += 3;
                getEnemyController.JumpActive();
                breakableParentController.BreakActive(transform.parent);
                break;
            case TrampolineType.Ground:
                getEnemyController.StrongJumpActive();
                Destroy(transform.parent.gameObject, 3);
                break;

        }
    }

    IEnumerator WaitForJumpAnim()
    {
        trampolineAnim.SetInteger("TrampolineStatus", 1);
        yield return new WaitForSeconds(0.7f);
        trampolineAnim.SetInteger("TrampolineStatus", 2);
    }

}
