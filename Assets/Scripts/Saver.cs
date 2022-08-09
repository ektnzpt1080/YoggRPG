using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    public MapManager.Pos currentLevel;
    //���� Ŭ����� �þ�� ��!!
    
    public List<MapManager.Pos> mapPos = new List<MapManager.Pos>();
    


    void Awake()
    {
        var obj = FindObjectsOfType<Saver>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
        if(SceneManager.GetActiveScene().name == "InitScene")
        {
            SceneManager.LoadScene("MapScene");
        }


    }

}
