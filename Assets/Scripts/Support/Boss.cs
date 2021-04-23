using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject trophy = default;
    
    // Start is called before the first frame update
    private void Start()
    {
        trophy.SetActive(false);
    }

    // Update is called once per frame
    public void UnlockTrophy()
    {
        trophy.SetActive(true);
    }
}
