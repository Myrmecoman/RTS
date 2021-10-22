using System.Collections;
using UnityEngine;


public class CoroutineHandler : MonoBehaviour
{
    public void CallCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(Caller(coroutine));
    }

    // Calls the coroutine
    private IEnumerator Caller(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
        Destroy(gameObject);
    }
}
