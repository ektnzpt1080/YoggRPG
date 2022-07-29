using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    public Spell spell {get;set;}
    [SerializeField] SpriteRenderer frontSpriteRenderer;
    [SerializeField] SpriteRenderer backSpriteRenderer;
    public PRS originPRS {get;set;}
    public int originOrder {get;set;}
    
    //아마 enum으로 어떤 타입의 카드인지를 적어 놔야 할 듯 함

    public void Ordering(int order){
        frontSpriteRenderer.sortingOrder = order * 10;
        backSpriteRenderer.sortingOrder = order * 10 - 1;
    }
    
    public void Copy(Spell _spell){
        spell = _spell;
    }

    public void OnMouseOver(){
        GameManager.Instance.CardManager.ChangeCardSize(this, true);
    }

    public void OnMouseExit(){
        GameManager.Instance.CardManager.ChangeCardSize(this, false);
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0f){
        if (useDotween)
        {
            transform.DOMove(prs.position, dotweenTime);
            transform.DORotateQuaternion(prs.rotation, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.position;
            transform.rotation = prs.rotation;
            transform.localScale = prs.scale;
        }
    }

    public Spell getSpell(){
        return spell;
    }
}

