using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public static IEnumerator Active(int nSeconds)
    {
        yield return new WaitForSeconds(nSeconds);
    }
}
