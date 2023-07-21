using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StarterEvent : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData data)
    {
        transform.gameObject.SetActive(false);
    }

}

