using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TrampolineType
{
    Static,
    Moveable,
    Breakable,
    Ground
}

public class TrampolineManager : Singleton<TrampolineManager>
{
    private int trampolineCounter = 0;

    private List<TrampolineController> trampolineControllerList = new List<TrampolineController>();
    private List<Vector3> lineRendererPosList = new List<Vector3>();

    public List<TrampolineController> TrampolineControllerList { get => trampolineControllerList; set => trampolineControllerList = value; }
    public int TrampolineCounter { get => trampolineCounter; set => trampolineCounter = value; }
    public List<Vector3> LineRendererPosList { get => lineRendererPosList; set => lineRendererPosList = value; }

    void Start()
    {

        // Trampoline add list
        TrampolineAddList();

        // Write trampoline Number
        WriteTrampolineNumber();

        EnemyParentManager.Instance.EnemySpawn();

        PlayerController.Instance.TrampolineCount = TrampolineControllerList.Count;

        for (int i = 0; i < transform.childCount; i++)
        {
            LineRendererPosList.Add(transform.GetChild(i).position);
        }
        LineRendererConnections.Instance.Setup();


    }

    private void TrampolineAddList()
    {
        foreach (TrampolineController item in GetComponentsInChildren<TrampolineController>())
        {
            TrampolineControllerList.Add(item);
            item.TrampolineIndexInList = TrampolineCounter;
            TrampolineCounter++;
        }
        TrampolineCounter = TrampolineControllerList.Count;

    }

    private void WriteTrampolineNumber()
    {
        for (int i = 0; i < TrampolineControllerList.Count; i++)
        {
            TrampolineControllerList[TrampolineCounter - 1].WriteTrampolineNumber(i + 1);
            TrampolineCounter--;
        }

        trampolineControllerList[0].BoosterController.gameObject.SetActive(false);
    }



    public Transform GetLookTransform(int getLastTrampolineIndex)
    {
        getLastTrampolineIndex++;

        if (getLastTrampolineIndex == TrampolineControllerList.Count)
        {
            return FinishCollider.Instance.transform;
        }

        if (!TrampolineControllerList[getLastTrampolineIndex])
        {
            getLastTrampolineIndex++;
        }

        if (getLastTrampolineIndex < TrampolineControllerList.Count && TrampolineControllerList[getLastTrampolineIndex])
        {
            return TrampolineControllerList[getLastTrampolineIndex].transform;
        }
        else
        {
            return FinishCollider.Instance.transform;
        }
    }

}
