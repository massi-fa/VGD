using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    #region Singleton
    public static PlayerTracker instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
}
