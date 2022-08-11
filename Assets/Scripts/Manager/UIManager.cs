using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CardUI cardUI;
    public Canvas CardCanvas;
    [SerializeField] LayoutGroup layoutgroup;
    List<CardUI> cards;

    public RewardCanvas RewardCanvas;

    // SpellInfo List를 받고 Card들을 생성시킴 
    public void TurnOnCardList(List<SpellInfo> sList){
        cards = new List<CardUI>();
        CardCanvas.gameObject.SetActive(true);
        foreach (SpellInfo spellinfo in sList){
            CardUI c = Instantiate(cardUI, layoutgroup.transform);
            c.Copy(spellinfo);
            c.UpdateCard();
            cards.Add(c);
        }
        //TODO : 정렬시켜서 보여줘야 됨
    }

    // 카드들을 모두 파괴시킴
    public void TurnOffCardList(){
        foreach(CardUI c in cards){
            GameObject.Destroy(c.gameObject);
        }
        CardCanvas.gameObject.SetActive(false);
    }

    public void TurnOnCardRewardCanvas(){
        RewardCanvas.gameObject.SetActive(true);
        RewardCanvas.RandomReward();
    }
}
