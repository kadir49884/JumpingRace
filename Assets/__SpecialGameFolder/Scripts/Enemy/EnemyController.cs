using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool isEnemyForwardActive;
    private bool isLookAtAcitve;
    private bool isTriggerPlayer;
    private int jumpCounter;

    [SerializeField, ReadOnly] private Animator enemyAnim;
    [SerializeField, ReadOnly] private Rigidbody rb;

    private GameManager gameManager;
    private Transform lookableTrampoline;
    private Vector3 lookAtDirection;
    private Transform playerTransform;



    [SerializeField] private float forwardSpeed;
    [SerializeField] private float jumpForceSpeed;
    [SerializeField] private float strongJumpForceSpeed;

    public Transform LookableTrampoline { get => lookableTrampoline; set => lookableTrampoline = value; }
    public int JumpCounter { get => jumpCounter; set => jumpCounter = value; }




    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameStart += GameStart;
    }


    private void Update()
    {
        if (!gameManager.RunGame || isTriggerPlayer)
        {
            return;
        }
        LookAtTrampoline();

    }

    private void FixedUpdate()
    {
        if (!gameManager.RunGame)
        {
            return;
        }
        else if (isTriggerPlayer)
        {
            rb.velocity = playerTransform.forward * 7;
            return;
        }


        if (isEnemyForwardActive && lookableTrampoline && !isTriggerPlayer)
        {
            Vector3 forwardDirection = (lookableTrampoline.position - transform.position).normalized;
            rb.velocity = new Vector3(forwardDirection.x * forwardSpeed, rb.velocity.y, forwardDirection.z * forwardSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out PlayerController playerController))
        {
            enemyAnim.SetTrigger("Death");
            rb.velocity = Vector3.zero;
            transform.GetComponent<CapsuleCollider>().enabled = false;
            playerTransform = playerController.transform;
            isTriggerPlayer = true;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                isTriggerPlayer = false;
                DeadEnemy();
            });
        }
    }


    public void DeadEnemy()
    {
        transform.GetComponent<OrderFinder>().OrderValue = -1;
        DOVirtual.DelayedCall(2, () =>
        {
            gameObject.gameObject.SetActive(false);
        });
    }


    private void LookAtTrampoline()
    {
        if (isLookAtAcitve && LookableTrampoline)
        {
            lookAtDirection = LookableTrampoline.position - transform.position;
            Vector3 direction = new Vector3(lookAtDirection.x, 0, lookAtDirection.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 0.05f);
        }

    }

    public void SetLookAt(Transform trampolineTransform)
    {
        StartCoroutine(WaitForLookAt(trampolineTransform));
    }


    private void GameStart()
    {
        DOVirtual.DelayedCall(Random.Range(0, 1f), () =>
        {
            rb.isKinematic = false;
            SetAnimValue(-2);
        });

    }

    IEnumerator WaitForLookAt(Transform trampolineTransform)
    {

        yield return new WaitForSeconds(2f);
        lookableTrampoline = trampolineTransform;
        isLookAtAcitve = true;
    }


    IEnumerator WaitForForwardMove()
    {
        JumpCounter++;
        if (JumpCounter >= 2)
        {
            JumpCounter = 0;
            isEnemyForwardActive = true;
            yield return new WaitForSeconds(2);
            isEnemyForwardActive = false;
        }

    }
    public void JumpActive()
    {
        JumpTime(jumpForceSpeed);
        StartCoroutine(WaitForForwardMove());

    }


    public void StrongJumpActive()
    {
        JumpTime(strongJumpForceSpeed);
    }


    private void JumpTime(float getJumpValue)
    {
        rb.AddForce(Vector3.up * getJumpValue, ForceMode.Impulse);
        SetAnimValue(GetRandomJumpAnim());

    }

    // Animation Event Function
    public void PlayFallAnim()
    {
        SetAnimValue(-1);
    }

    public void SetAnimValue(int animValue)
    {
        enemyAnim.SetInteger("JumpStatus", animValue);
    }

    private int GetRandomJumpAnim()
    {
        return Random.Range(1, 3);
    }

    public void SetEnemyMaterialColor(int getColorIndex)
    {

        Material bodyMat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        bodyMat.color = ObjectManager.Instance.EnemyColorList[getColorIndex];
    }

    [Button("SetRef")]
    public void SetRef()
    {
        rb = transform.GetComponent<Rigidbody>();
        enemyAnim = transform.GetComponent<Animator>();
    }
}
