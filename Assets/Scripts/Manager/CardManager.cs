using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardManager : MonoBehaviour
{    
    public List<SpellInfo> cardDeck {get;set;} // 덱의 카드
    public List<SpellInfo> cardGrave {get;set;} // 묘지의 카드
    public List<SpellInfo> cardYogg {get;set;} // 요그사론의 카드
    public List<Card> cardHand {get;set;} // 핸드의 카드, 이쪽은 진짜 카드로 관리됨
    [SerializeField] Card cardObject;
    [SerializeField] Transform rightCardTransform, leftCardTransform, deckTransform;
    [SerializeField] Transform yoggInitTransform, showCardTransform;
    [SerializeField] public SpellData spelldata;

    int maxCardCount = 10; // 최대 10장을 들 수 있음
    int yoggCounter = 5; // 이후 카드를 사용할때 마다 사용되는 것으로 바꿀 것
    [SerializeField] bool selected;
    Card selectedCard;
    List<Vector2> PreDecisionRange;

    //카드매니저를 시작시킴
    public void SetCardList(List<SpellInfo> SpellList, List<SpellInfo> YoggSpellList){
        List<SpellInfo> cards = new List<SpellInfo>(SpellList);
        List<SpellInfo> shuffled = new List<SpellInfo>();
        cardHand = new List<Card>();
        cardGrave = new List<SpellInfo>();
        int count = cards.Count;
        for(int i = 0; i < count; i++){
            int j = Random.Range(0, cards.Count);
            shuffled.Add(cards[j]);
            cards.RemoveAt(j);
        }
        cardDeck = shuffled;
        selected = false;
        PreDecisionRange = new List<Vector2>();
        cardYogg = new List<SpellInfo>(YoggSpellList);
    }

    //카드를 한장 뽑음
    public void DrawCard(){
        if(cardDeck.Count == 0){
            if(cardGrave.Count == 0){
                Debug.Log("Cannot draw card");
                return;
            }
            Debug.Log("shuffle deckgrave cards");
            ShuffleCard();//coroutine으로 만들어야 될수도 있음
        }
        if(cardHand.Count >= maxCardCount){
            Debug.Log("Hand is full");
            cardGrave.Add(cardDeck[0]);
            cardDeck.RemoveAt(0);
        }
        else{
            Card drawCard = GameObject.Instantiate(cardObject, deckTransform.position, Quaternion.identity);//덱에서 생성되게 할 것
            drawCard.Copy(cardDeck[0].spell, Card.CardType.handCard);
            cardHand.Add(drawCard);
            cardDeck.RemoveAt(0);
            CardHandAlliance();
        }
    }

    //카드를 정렬시킴 r은 원의 반지름, moveTime은 움직이는 시간
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
                Quaternion rotation = Quaternion.identity;
                Vector3 targetPos = new Vector3(cardHandx[i], leftCardTransform.position.y, leftCardTransform.position.z);
                cardHand[i].Ordering(i);
                cardHand[i].originOrder = i;
                cardHand[i].gameObject.transform.DOMove(targetPos, moveTime);
                cardHand[i].gameObject.transform.DORotateQuaternion(rotation, moveTime);
                cardHand[i].originPRS = new PRS(targetPos, rotation, cardObject.transform.localScale);
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

    //카드에 마우스를 올려 카드 사이즈를 바꿈
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

    //카드들을 원래 위치로 돌림
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
        PreDecisionRange = card.spell.PreDecision(); //predecisionrange에 누를 수 있는 범위를 넣음
        if(PreDecisionRange != null && PreDecisionRange.Count == 0){
            Debug.Log("No appropriate target on board");
            return;
        }
        GameManager.Instance.BattleManager.EmphasizeTile(true, PreDecisionRange);
        selectedCard = card;
        selected = true;
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

    //카드를 고름
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

    //카드를 플레이함
    public void PlayCard(){
        if((Input.GetMouseButtonDown(0)) && selected){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
            //PreDecisionRange가 null, 이경우 아무 타일이나 누르면 발동함
            if(hit.collider != null && hit.collider.TryGetComponent<Tile>(out Tile tile_) && PreDecisionRange == null){
                selectedCard.spell.Decision(Vector2.zero);
                DiscardCard(selectedCard);
            }
            //적절한 타일을 고름
            else if(hit.collider != null && hit.collider.TryGetComponent<Tile>(out Tile tile) && PreDecisionRange.Contains(tile.position)){
                selectedCard.spell.Decision(tile.position);
                DiscardCard(selectedCard);
            }
            //적절하지 않은 타일을 고름 
            else{
                CardReturnPRS(selectedCard);
            }
            selected = false;    
            GameManager.Instance.BattleManager.Select(selected);
            GameManager.Instance.BattleManager.EmphasizeTile(false);
        }
        else if((Input.GetMouseButtonDown(1)) && selected){
            CardReturnPRS(selectedCard);
            selected = false;
            GameManager.Instance.BattleManager.Select(selected);
            GameManager.Instance.BattleManager.EmphasizeTile(false);
        }
    }

    //카드를 버림
    void DiscardCard(Card card){
        //card가 소멸 속성이면 사라지게 할 것, 시간 있으면 애니메이션도
        cardHand.Remove(card);
        cardGrave.Add(card.spell.spellinfo);
        GameObject.Destroy(card.gameObject);
        CardHandAlliance();
    }

    //카드를 무덤에서 덱으로 다시 섞음
    void ShuffleCard(){
        int count = cardGrave.Count;
        for(int i = 0; i < count; i++){
            int j = Random.Range(0, cardGrave.Count);
            cardDeck.Add(cardGrave[j]);
            cardGrave.RemoveAt(j);    
        }
        GameManager.Instance.BattleManager.InitializeMovementMana();
    }

    //요그님 호출 발동
    public IEnumerator PlayYogg(){
        //발동할 카드들을 고름
        List<SpellInfo> yogglist = new List<SpellInfo>();
        List<SpellInfo> yogg = new List<SpellInfo>(cardYogg);
        for(int i = 0 ; i < yoggCounter ; i++){
            if(yogg.Count <= 0){
                Debug.Log("Not Enough Yogg Deck Card Quantity");
                break;
            }
            int picked = Random.Range(0, yogg.Count);
            yogglist.Add(yogg[picked]);
            yogg.RemoveAt(picked);
        }
        
        foreach (SpellInfo spellinfo in yogglist){
            //카드를 생성시킴
            Card yoggInstCard = GameObject.Instantiate(cardObject, yoggInitTransform.position, Quaternion.identity);
            yoggInstCard.Copy(spellinfo.spell, Card.CardType.yoggCard);
            float movetime = 0.5f;
            yoggInstCard.MoveTransform(new PRS(showCardTransform.position, Quaternion.identity, cardObject.transform.localScale * 1.8f), true, movetime);
            yield return new WaitForSeconds(movetime + 0.5f);

            GameObject.Destroy(yoggInstCard.gameObject);

            //카드를 플레이함
            spellinfo.spell.GetSpellInfo();
            Debug.Log("시전 " + (yogglist.IndexOf(spellinfo) + 1)+ " : " + spellinfo.name);
            List<Vector2> sl = spellinfo.spell.YoggDecision();
            if(sl == null){
                spellinfo.spell.Decision(Vector3.zero);    
            }
            else if(sl.Count == 0){
                Debug.Log("불발");
            }
            else{
                spellinfo.spell.Decision(sl[Random.Range(0, sl.Count)]);
            }
            yield return new WaitForSeconds(0.2f);
        }
        GameManager.Instance.BattleManager.YoggEnd();
        yield break;

        // 나중에 yoggCounter = 0;
    }

    public SpellInfo getSpellInfo(int i){
        return spelldata.FindSpell(i);
    }


    void Update(){ 
        //디버그 용도
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            DrawCard();
        }
        
    }


}
