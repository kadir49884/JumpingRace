using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperLineRenderer : Singleton<HelperLineRenderer>
{

    private Transform playerTransform;
    private bool isRayHitTrampoline;
    private LineRenderer lineRenderer;
    private Vector3 helperPos;

    private RaycastHit hit;
    private Material lineMat;

    private GameManager gameManager;

    [SerializeField] private Color32 colorGreen;
    [SerializeField] private Color32 colorRed;
    [SerializeField] private Transform lineEndPoint;

    public bool IsRayHitTrampoline { get => isRayHitTrampoline; set => isRayHitTrampoline = value; }

    private void Start()
    {
        playerTransform = PlayerController.Instance.transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineMat = lineRenderer.material;
        gameManager = GameManager.Instance;
        gameManager.GameFail += GameFail;
        gameManager.GameWin += GameWin;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.RunGame)
        {
            return;
        }

        helperPos = playerTransform.position;
        lineRenderer.SetPosition(0, helperPos);
        lineRenderer.SetPosition(1, Vector3.up * -10);

        if (Physics.Raycast(playerTransform.position + Vector3.up, Vector3.down * 100, out hit))
        {
            lineRenderer.SetPosition(0, playerTransform.position + Vector3.up * 0.25f);
            lineRenderer.SetPosition(1, hit.point);
            lineEndPoint.position = lineRenderer.GetPosition(1);
            RayHitObjects(hit);
        }
    }

    private void RayHitObjects(RaycastHit hit)
    {
        if (hit.transform.CompareTag("Trampoline") || hit.transform.CompareTag("FinishPlatform"))
        {
            IsRayHitTrampoline = true;
            lineMat.color = colorGreen;
            if (hit.transform.CompareTag("Trampoline") && hit.transform.GetComponent<TrampolineController>())
            {
                hit.transform.GetComponent<TrampolineController>().BoosterController.RayHitTrampoline();
            }
        }
        else
        {
            IsRayHitTrampoline = false;
            lineMat.color = colorRed;
        }
    }

    public void GameFail()
    {
        gameObject.SetActive(false);
    }
    public void GameWin()
    {
        gameObject.SetActive(false);
    }
}
