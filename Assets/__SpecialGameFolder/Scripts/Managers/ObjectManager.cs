using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{

    [SerializeField] private Transform waterSplash;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private List<string> playersNamesList = new List<string>();
    [SerializeField] private List<Color32> enemyColorList = new List<Color32>();

    public Transform WaterSplash { get => waterSplash; set => waterSplash = value; }
    public CinemachineVirtualCamera CinemachineVirtualCamera { get => cinemachineVirtualCamera; set => cinemachineVirtualCamera = value; }
    public List<Color32> EnemyColorList { get => enemyColorList; set => enemyColorList = value; }
    public Transform EnemyTransform { get => enemyTransform; set => enemyTransform = value; }
    public List<string> PlayersNamesList { get => playersNamesList; set => playersNamesList = value; }
}