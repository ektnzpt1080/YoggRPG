using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{
    
    [SerializeField] private GameObject highlight;
    [SerializeField] private Color baseColor, offsetColor, targetColor;
    [SerializeField] private SpriteRenderer _renderer;
    public Vector2 position { get; set; }
    
    Color originColor;

    // Start is called before the first frame update
    public void Init(bool isOffset){
        originColor = isOffset ? offsetColor : baseColor;
        _renderer.color = originColor;
    }

    public void targetTile(bool active){
        _renderer.color = active ? targetColor : originColor;
    }

    public void Highlight(){
        StartCoroutine(_Highlight());
    }

    private IEnumerator _Highlight(){
        highlight.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        highlight.SetActive(false);
    }
    
}
