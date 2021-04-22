using System.Collections;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public static IEnumerator Active(float nSeconds)
    {
        yield return new WaitForSeconds(nSeconds);
    }
}
