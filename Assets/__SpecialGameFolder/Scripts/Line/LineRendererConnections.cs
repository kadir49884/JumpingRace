using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LineRendererConnections : Singleton<LineRendererConnections>
{

    private TrampolineManager trampolineManager;

    private LineRenderer lineRenderer;


    public List<Vector3> linePos; 
    public float waveHeight = 0.5f; 
    public float waveFrequency = 1f; 

    public void Setup()
    {
        lineRenderer = GetComponent<LineRenderer>();
        trampolineManager = TrampolineManager.Instance;


        linePos = trampolineManager.LineRendererPosList; ;
        lineRenderer.positionCount = linePos.Count;
        lineRenderer.SetPositions(linePos.ToArray());

        float totalDistance = 0f;
        for (int i = 0; i < linePos.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(linePos[i], linePos[i + 1]);
        }
        float wavelength = totalDistance / linePos.Count;
        float phase = Random.Range(0f, Mathf.PI * 2f);


        for (int i = 0; i < linePos.Count; i++)
        {
            float distanceFromStart = Vector3.Distance(linePos[0], linePos[i]);
            float distanceFromEnd = Vector3.Distance(linePos[linePos.Count - 1], linePos[i]);
            float waveOffset = (distanceFromStart - distanceFromEnd) / totalDistance;
            float waveHeightOffset = Mathf.Sin((waveOffset + Time.time * waveFrequency / wavelength + phase) * Mathf.PI * 2f);
            Vector3 position = linePos[i] + transform.up * waveHeight * waveHeightOffset;
            lineRenderer.SetPosition(i, position);
        }

    }






}
