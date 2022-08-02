using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Map : MonoBehaviour
{

    [SerializeField] private SpriteRenderer _mapRenderer;

    public void Init(string name, Color color)
    {
        this.name = name;
        _mapRenderer.color = color;
    }
    
    public void OnMouseOver()
    {
        MapManager.Instance.ShowText(this);
        
    }

    public void OnMouseExit()
    {
        MapManager.Instance.HideText(this);
    }
    
}
