using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{    
    public List<Spell> cardDeck {get;set;} // 덱의 카드
    public List<Spell> cardGrave {get;set;} // 묘지의 카드
    public List<Card> cardHand {get;set;} // 핸드의 카드, 이쪽은 진짜 카드로 관리됨
    [SerializeField] Card cardObject;
    [SerializeField] Transform rightCardTransform;
    [SerializeField] Transform leftCardTransform;
    [SerializeField] Transform deckTransform;

    int maxCardCount = 10; // 최대 10장을 들 수 있음
    [SerializeField] bool selected;
    Card selectedCard;
    List<Vector2> PreDecisionRange;

    public void SetCardList(List<Spell> SpellList){
        List<Spell> cards = new List<Spell>(SpellList);
        List<Spell> shuffled = new List<Spell>();
        cardHand = new List<Card>();
        cardGrave = new List<Spell>();
        int count = cards.Count;
        for(int i = 0; i < count; i++){
            int j = Random.Range(0, cards.Count);
            shuffled.Add(cards[j]);
            cards.RemoveAt(j);
        }
        cardDeck = shuffled;
        selected = false;
        PreDecisionRange = new List<Vector2>();
    }

    public void DrawCard(){
        if(cardDeck.Count == 0){
            Debug.Log("deck count 0, need to shuffle cards");
        }
        else {
            int index = cardHand.Count;
            Card drawCard = GameObject.Instantiate(cardObject, deckTransform.position, Quaternion.identity);//덱에서 생성되게 할 것
            drawCard.Copy(cardDeck[0]);
            cardHand.Add(drawCard);
            cardDeck.RemoveAt(0);
            CardHandAlliance();
        }
    }

    //카드를 정렬시킴 
    public void CardHandAlliance(float r = 22.97f, float moveTime = 0.7f){
        float[] cardHandx = new float[cardHand.Count];
        float xPos(float s) {
            return Mathf.Lerp(leftCardTransform.position.x, rightCardTransform.position.x, s);
        } 
        switch (cardHand.Count){
            case 1 :
                cardHandx = new float[] {xPos(0.5f)};
                break;
            case 2 :
                cardHandx = new float[] {xPos(0.38f), xPos(0.62f)};//2.1 3.8
                break;
            case 3 :
                cardHandx = new float[] {xPos(0.26f), xPos(0.5f), xPos(0.74f)};//1.1 3 4.8
                break;
            case 4 :
                cardHandx = new float[] {xPos(0.19f), xPos(0.395f), xPos(0.605f), xPos(0.81f)};//1.1 3 4.8
                break;
            case 5 : 
                cardHandx = new float[] {xPos(0.1f), xPos(0.3f), xPos(0.5f), xPos(0.7f), xPos(0.9f)};//1.1 3 4.8
                break;
            default :
                for(int i = 0; i < cardHand.Count; i++){
                    cardHandx[i] = xPos((float)i / (cardHand.Count - 1));
                }
                break;
        }
        
        float[] cardHandy = new float[cardHand.Count];
        float yPos(float card_x){
            float y3 = leftCardTransform.position.y - Mathf.Sqrt(r*r -Mathf.Pow(xPos(0.5f),2));
            return y3 + Mathf.Sqrt(r*r - Mathf.Pow(card_x - xPos(0.5f),2));
        }
        if(cardHand.Count <= 3){
            for(int i = 0; i < cardHand.Count; i++) {
                cardHand[i].Ordering(i);
                cardHand[i].originOrder = i;
                Vector3 targetPos = new Vector3(cardHandx[i], leftCardTransform.position.y, leftCardTransform.position.z);
                cardHand[i].gameObject.transform.DOMove(targetPos, moveTime);
                cardHand[i].originPRS = new PRS(targetPos, cardHand[i].transform.rotation, cardObject.transform.localScale);
            }
        }
        else{
            for(int i = 0; i < cardHand.Count; i++) {
                Quaternion rotation = Quaternion.Slerp(leftCardTransform.rotation, rightCardTransform.rotation, (float)i/(cardHand.Count - 1));
                Vector3 targetPos = new Vector3(cardHandx[i], yPos(cardHandx[i]), leftCardTransform.position.z);
                cardHand[i].Ordering(i);
                cardHand[i].originOrder = i;
                cardHand[i].gameObject.transform.DOMove(targetPos, moveTime);
                cardHand[i].gameObject.transform.DORotateQuaternion(rotation, moveTime);
                cardHand[i].originPRS = new PRS(targetPos, rotation, cardObject.transform.localScale);
            }
        }
    }

    public void ChangeCardSize(Card card, bool isLarge){
        if(!selected){
            if(isLarge){
                Vector3 enlargePos = new Vector3(card.originPRS.position.x, -0.7f, -3f);
                card.transform.DOKill();
                card.MoveTransform(new PRS(enlargePos,Quaternion.identity, cardObject.transform.localScale * 1.5f), false);
                card.Ordering(11);
                float left = 0.7f;
                foreach(Card othercard in cardHand){
                    if(othercard == card){
                        if(cardHand.Count <= 6){
                            left = -0.7f;
                        }
                        else {
                            left = -0.9f;
                        }
                        continue;
                    }
                    othercard.MoveTransform(new PRS(othercard.originPRS.position + Vector3.left * left, othercard.originPRS.rotation, othercard.originPRS.scale), true, 0.4f);
                }
            }
            else{
                CardReturnPRS(card);
            }
        }
    }

    public void CardReturnPRS(Card card){
        foreach(Card othercard in cardHand){
            if(othercard == card){
                othercard.MoveTransform(othercard.originPRS, false);
            }
            else{
                othercard.MoveTransform(othercard.originPRS, true, 0.4f);
            }
            othercard.Ordering(othercard.originOrder);
        }
    }

    //고른 카드를 selectedCard에 넣고, 핸드에 있는 카드들을 밑으로 내림
    public void SelectCard(Card card){
        selectedCard = card;
        selected = true;
        PreDecisionRange = card.spell.PreDecision(); //predecisionrange에 누를 수 있는 범위를 넣음
        Vector3 down = Vector3.down * 1.7f;
        card.MoveTransform(new PRS(card.transform.position + down , Quaternion.identity, card.transform.localScale), true, 0.4f);
        float left = 0.7f;
        foreach(Card othercard in cardHand){
            if(othercard == card){
                if(cardHand.Count <= 6){
                    left = -0.7f;
                }
                else {
                    left = -0.9f;
                }
                continue;
            }
            othercard.MoveTransform(new PRS(othercard.originPRS.position + Vector3.left * left + down, othercard.originPRS.rotation, othercard.originPRS.scale), true, 0.4f);
        }
    }

    public void PickCard(){
        if((Input.GetMouseButtonDown(0)) && !selected){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity); //나중에 타일 레이어로 바꿔야 될수도 있음
            if(hit.collider != null && hit.collider.TryGetComponent<Card>(out Card card)){
                SelectCard(card);
                GameManager.Instance.BattleManager.Select(selected);
            }
        }
        
    }

    public void PlayCard(){
        if((Input.GetMouseButtonDown(0)) && selected){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
            //적절한 타일을 고름
            if(hit.collider != null && hit.collider.TryGetComponent<Tile>(out Tile tile) && PreDecisionRange.Contains(tile.position)){
                selectedCard.spell.Decision(tile.position);
                CardReturnPRS(selectedCard);
                selected = false;
                GameManager.Instance.BattleManager.Select(selected);
            }
            //적절하지 않은 타일을 고름 
            else{
                CardReturnPRS(selectedCard);
                selected = false;    
                GameManager.Instance.BattleManager.Select(selected);
            }
        }
        else if((Input.GetMouseButtonDown(1)) && selected){
            CardReturnPRS(selectedCard);
            selected = false;
            GameManager.Instance.BattleManager.Select(selected);
        }
    }


    void Update(){ 
        //디버그 용도
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            DrawCard();
        }
        
    }

    

}
