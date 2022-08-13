using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RewardUI : MonoBehaviour, IPointerClickHandler
{
    public RewardUICanvas rewardCanvas;

    public enum RewardUIType{
        gold,
        card,
        equip
    }

    RewardUIType type;
    [SerializeField] TextMeshProUGUI typeText;

    int goldAmount;
    public void SetReward(RewardUICanvas rc, RewardUIType _type){
        rewardCanvas = rc;
        type = _type;
        if(type == RewardUIType.gold){
            goldAmount = 98;
            typeText.text = "골드 " + goldAmount;
        }
        else if(type == RewardUIType.card){
            typeText.text = "카드";
        }
    }

    public void OnPointerClick(PointerEventData eventData){
        if(type == RewardUIType.gold){
            GameManager.Instance.saver.AddGold(goldAmount);
            GameObject.Destroy(this.gameObject);
        }
        else if(type == RewardUIType.card){
            GameManager.Instance.UIManager.TurnOnCardRewardCanvas();
            GameObject.Destroy(this.gameObject);
        }


    }


}
