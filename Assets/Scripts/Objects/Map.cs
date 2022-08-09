using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public MapManager.Pos pos;

    [SerializeField] private SpriteRenderer _mapRenderer;
    [SerializeField] private Saver _saver;

    public void Init(string name, Color color, Saver saver)
    {
        this.name = name;
        _mapRenderer.color = color;
        _saver = saver;
    }
    
    public void OnMouseOver()
    {
        MapManager.Instance.ShowText(this);
        if (Input.GetMouseButtonDown(0))
        {
            if (this.pos.x == _saver.currentLevel.x + 1 && (this.pos.y >= _saver.currentLevel.y - 1 && this.pos.y <= _saver.currentLevel.y + 1))
            {
                _saver.currentLevel = this.pos;
                SceneManager.LoadScene("StageScene");
                // move to stage scene   
            }
            else
            {
                print("Unable to move!");
            }   
        }
    }

    public void OnMouseExit()
    {
        MapManager.Instance.HideText(this);
    }
    
}
