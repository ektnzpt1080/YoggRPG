using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(SceneManager.GetActiveScene().name == "TestScene")
            {
                SceneManager.LoadScene("MapScene");
            }
            else
            {
                SceneManager.LoadScene("TestScene");
            }
        }
    }
}
