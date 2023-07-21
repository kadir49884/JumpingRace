using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableParentController : MonoBehaviour
{

    public void BreakActive(Transform getParent)
    {
        // Active breakable pieces and Destroy
        Transform getChild = transform.GetChild(0);
        getChild.gameObject.SetActive(true);
        getChild.parent = null;

        Destroy(getChild.gameObject, 5);


        // Destroy trampoline
        getParent.gameObject.SetActive(false);
        Destroy(getParent.gameObject, 5);
    }
}
