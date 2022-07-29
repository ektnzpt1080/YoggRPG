using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile : MonoBehaviour
{
    
    [SerializeField] private GameObject highlight;
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    public Vector2 position { get; set; }
    

    // Start is called before the first frame update
    public void Init(bool isOffset){
        _renderer.color = isOffset ? offsetColor : baseColor;
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
