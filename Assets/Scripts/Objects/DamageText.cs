using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageText : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] float duration;
    [SerializeField] float x;
    [SerializeField] float y;
    [SerializeField] Ease easex, easey;


    void Start(){
        Random.Range(x, x* (-1));
        transform.DOMoveX(this.transform.position.x + Random.Range(x, x* (-1)), duration).SetEase(easex);
        transform.DOMoveY(this.transform.position.y + y, duration).SetEase(easey);
        StartCoroutine(DestroyAfterMove());
    }

    public void Initialize(string s, float d){
        text.text = s;
        duration = d;
    }

    IEnumerator DestroyAfterMove(){
        yield return new WaitForSeconds(duration);
        GameObject.Destroy(this.gameObject);
    }


}
