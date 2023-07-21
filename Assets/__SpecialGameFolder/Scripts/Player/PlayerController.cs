using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    private bool isTouchActive;
    private bool isLookAtAcitve;
    private GameManager gameManager;
    private Transform lookableTrampoline;
    private Vector3 lookAtDirection;
    private int trampolineCount;
    private CanvasManager canvasManager;
    private Transform lastTrampoline;
    private HelperLineRenderer helperLineRenderer;


    [SerializeField, ReadOnly] private Rigidbody rb;
    [SerializeField, ReadOnly] private Animator playerAnim;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float jumpForceSpeed;
    [SerializeField] private float strongJumpForceSpeed;



    public Transform LookableTrampoline { get => lookableTrampoline; set => lookableTrampoline = value; }
    public int TrampolineCount { get => trampolineCount; set => trampolineCount = value; }
    public Transform LastTrampoline { get => lastTrampoline; set => lastTrampoline = value; }

    private void Start()
    {

        gameManager = GameManager.Instance;
        canvasManager = CanvasManager.Instance;
        helperLineRenderer = HelperLineRenderer.Instance;
        gameManager.GameStart += GameStart;
        //orderController = OrderController.Instance;
#if UNITY_EDITOR
        rotationSpeed *= 3;
#endif

    }


    private void Update()
    {
        if (!gameManager.RunGame)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isTouchActive = true;
            isLookAtAcitve = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isTouchActive = false;
        }

        SwipeControl();

        LookAtTrampoline();
    }

    private void FixedUpdate()
    {

        if (!gameManager.RunGame)
        {
            return;
        }

        if (isTouchActive)
        {
            rb.velocity = new Vector3(transform.forward.x * forwardSpeed, rb.velocity.y + 0.001f, transform.forward.z * forwardSpeed);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

    }



    private void SwipeControl()
    {
        if (Input.GetMouseButton(0))
        {
            Quaternion rot = Quaternion.Euler(transform.rotation.x, transform.localEulerAngles.y + Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, transform.rotation.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 0.8f);
        }
    }

    private void LookAtTrampoline()
    {
        if (LookableTrampoline && !isTouchActive && isLookAtAcitve && helperLineRenderer.IsRayHitTrampoline)
        {
            lookAtDirection = LookableTrampoline.position - transform.position;
            Vector3 direction = new Vector3(lookAtDirection.x, 0, lookAtDirection.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), 0.05f);
        }

    }

    public void SetLookAt(Transform trampolineTransform)
    {
        lookableTrampoline = trampolineTransform;
        StartCoroutine(WaitForLookAt());
    }

    IEnumerator WaitForLookAt()
    {
        yield return new WaitForSeconds(0.1f);
        isLookAtAcitve = true;
    }



    private void GameStart()
    {
        rb.isKinematic = false;
        SetAnimValue(-2);

    }

    public void JumpActive()
    {
        JumpTime(jumpForceSpeed);
    }
    public void StrongJumpActive()
    {
        JumpTime(strongJumpForceSpeed * Random.Range(1, 1.5f));
    }

    public void ProgressBarControl(int getTrampolineOrderValue)
    {
        if (getTrampolineOrderValue == 0)
        {
            return;
        }
        canvasManager.SetProgressBarFill((float)getTrampolineOrderValue / (float)(TrampolineCount + 1f));
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
        playerAnim.SetInteger("JumpStatus", animValue);
    }

    private int GetRandomJumpAnim()
    {
        return Random.Range(1, 3);
    }


    public void SlowMotion()
    {
        StartCoroutine(WaitForSlowMotion());
    }

    IEnumerator WaitForSlowMotion()
    {
        Time.timeScale = 0.6f;
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1;
    }

    [Button("SetRef")]
    public void SetRef()
    {
        rb = transform.GetComponent<Rigidbody>();
        playerAnim = transform.GetComponent<Animator>();
    }

}