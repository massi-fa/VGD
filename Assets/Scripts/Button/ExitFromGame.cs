using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitFromGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
    }
}
