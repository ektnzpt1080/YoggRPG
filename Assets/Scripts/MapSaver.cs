using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSaver : MonoBehaviour
{


    public static MapManager.Pos currentLevel;
    //레벨 클리어마다 늘어나야 함!!
    

    public static List<MapManager.Pos> mapPos = new List<MapManager.Pos>();

    

    void Awake()
    {

        var obj = FindObjectsOfType<MapSaver>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }


}
