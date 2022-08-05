using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public MapManager.Pos pos;

    [SerializeField] private SpriteRenderer _mapRenderer;

    public void Init(string name, Color color)
    {
        this.name = name;
        _mapRenderer.color = color;
    }
    
    public void OnMouseOver()
    {
        MapManager.Instance.ShowText(this);
        if (Input.GetMouseButtonDown(0))
        {
            if (this.pos.x == MapSaver.currentLevel.x + 1 && (this.pos.y >= MapSaver.currentLevel.y - 1 && this.pos.y <= MapSaver.currentLevel.y + 1))
            {
                SceneManager.LoadScene("TestScene");
                // 나중에 바꿀것
                MapSaver.currentLevel = this.pos;
            }
            else
            {
                print("이동 불가!");
            }
            
        }
        
    }

    public void OnMouseExit()
    {
        MapManager.Instance.HideText(this);
    }
    
}
