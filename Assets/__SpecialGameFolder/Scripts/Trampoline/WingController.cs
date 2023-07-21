using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingController : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(WaitForStartAnim());
    }
    IEnumerator WaitForStartAnim()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
        transform.GetComponent<Animator>().enabled = true;
    }
}
