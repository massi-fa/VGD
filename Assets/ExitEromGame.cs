using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class ExitEromGame : MonoBehaviour
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
