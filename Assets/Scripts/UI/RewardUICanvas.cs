using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RewardUICanvas : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] GameObject nextStage;
    [SerializeField] LayoutGroup _layoutGroup;
    [SerializeField] RewardUI rewardUIObject;

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.pointerPressRaycast.gameObject == nextStage){
            SceneManager.LoadScene("MapScene");
        }
    }

    public void SetReward(RewardUI.RewardUIType type){
        RewardUI r = Instantiate(rewardUIObject, _layoutGroup.transform);
        r.SetReward(this, type);
        
    }



}
