using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] CardUI cardUI;
    [SerializeField] Canvas cardCanvas;
    [SerializeField] RewardUICanvas rewardCanvas;
    [SerializeField] LayoutGroup layoutgroup;
    List<CardUI> cards;

    [SerializeField] RewardCardCanvas rewardCardCanvas;
    
    [SerializeField] Canvas HPUICanvas;
    [SerializeField] HPBarUI hpUI;


    // SpellInfo List를 받고 Card들을 생성시킴 
    public void TurnOnCardList(List<SpellInfo> sList){
        cards = new List<CardUI>();
        cardCanvas.gameObject.SetActive(true);
        foreach (SpellInfo spellinfo in sList){
            CardUI c = Instantiate(cardUI, layoutgroup.transform);
            c.Copy(spellinfo, CardUI.CardUIType.deck);
            c.SetRewardCanvas(rewardCardCanvas);
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
        cardCanvas.gameObject.SetActive(false);
    }

    public void TurnOnRewardCanvas(){
        GameManager.Instance.CardManager.SetCardInteractable(false);
        rewardCanvas.gameObject.SetActive(true);
        rewardCanvas.SetReward(RewardUI.RewardUIType.gold);
        rewardCanvas.SetReward(RewardUI.RewardUIType.card);
    }

    public void TurnOnCardRewardCanvas(){
        rewardCardCanvas.gameObject.SetActive(true);
        rewardCardCanvas.RandomReward();
    }

    public void HPBarInit(Enemy enemy){
        HPBarUI hp = Instantiate(hpUI, HPUICanvas.transform);
        hp.Init(enemy, HPUICanvas);
    }

}
