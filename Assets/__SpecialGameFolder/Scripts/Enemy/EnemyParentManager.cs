using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyParentManager : Singleton<EnemyParentManager>
{

    public void EnemySpawn()
    {
        ObjectManager objectManager = ObjectManager.Instance;
        TrampolineManager trampolineManager = TrampolineManager.Instance;

        for (int i = 1; i < 4; i++)
        {
            Transform enemyTransform = Instantiate(objectManager.EnemyTransform);
            enemyTransform.position = trampolineManager.TrampolineControllerList[i].transform.position;
            enemyTransform.GetComponentInChildren<EnemyController>().SetEnemyMaterialColor(i - 1);
            enemyTransform.GetComponent<OrderFinder>().CharacterName = objectManager.PlayersNamesList[i - 1];
        }
    }
}
