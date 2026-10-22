using System.Collections;
using UnityEngine;

public class CoroutineExample : MonoBehaviour
{
    // every second write log with 1 sec -> 2 sec -> 3 sec and then write "NARO Shoot!"
    //unity coroutine is function that provide execution and control with async
    //1. async execute
    //2. control by time/frame
    // - IEnumerator : return of Coroutine
    // - StartCoroutine(func()) : way of execute with coroutine
    // - yield : wait for given time/frame
    private void Start()
    {
        Debug.Log("Start!");
        StartCoroutine(CountDown_Coroutine());
        Debug.Log("End!");
    }

    private IEnumerator CountDown_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("1Sec!");

        yield return new WaitForSeconds(1f);
        Debug.Log("2Sec!");

        yield return new WaitForSeconds(1f);
        Debug.Log("3Sec!");

        yield return new WaitForSeconds(1f);
        Debug.Log("NARO Shoot!");
    }
}