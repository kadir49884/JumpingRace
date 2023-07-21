using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    private Material boosterMat;
    private Color32 boosterColor = Color.green;
    private bool isOpen;
    private bool isRayHitTrampoline;
    private float distance;
    private CanvasManager canvasManager;

    private void Start()
    {
        boosterMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        boosterColor = boosterMat.color;
        boosterColor.a = 255;
        canvasManager = CanvasManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            playerController.SlowMotion();
            distance = Vector3.Distance(playerController.transform.position, transform.position);
            canvasManager.WriteSuccessText(distance);
        }
    }



    public void OpenBooster(float getDistance)
    {
        if (isRayHitTrampoline)
        {
            return;
        }
        OpenMesh();
        if (getDistance < 2.5f)
        {
            boosterColor.a = (byte)(255 - getDistance * 100);
        }
        else
        {
            boosterColor.a = 5;
        }
        if (boosterMat && !isRayHitTrampoline)
        {
            boosterMat.color = boosterColor;
        }
    }

    public void RayHitTrampoline()
    {
        OpenMesh();
        isRayHitTrampoline = true;
        boosterColor.a = 255;
        boosterMat.DOColor(boosterColor, 0.5f);
    }

    private void OpenMesh()
    {
        if (!isOpen)
        {
            isOpen = true;
            for (int i = 0; i < 2; i++)
            {
                transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }



}
