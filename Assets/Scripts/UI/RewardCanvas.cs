using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class RewardCanvas : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] List<CardUI> deckReward;
    [SerializeField] List<CardSmallUI> deckRewardYogg;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject nextStageButton;
    [SerializeField] SpellData sd;

    bool isRewardPicked;

    public void RandomReward(){        
        List<SpellInfo> sl = new List<SpellInfo>();
        isRewardPicked = false;
        while(sl.Count < 3){
            SpellInfo candidate = sd.spellinfo[Random.Range(0, sd.spellinfo.Length)];
            if(!sl.Contains(candidate)){
                sl.Add(candidate);
            }
        }

        for(int i = 0 ; i < 3 ; i++){
            deckReward[i].Copy(sl[i], CardUI.CardUIType.reward);
            deckReward[i].SetRewardCanvas(this);
            deckReward[i].UpdateCard();
        }

        for(int i = 0 ; i < 9 ; i++){
            deckRewardYogg[i].Copy(sd.spellinfo[Random.Range(0, sd.spellinfo.Length)], CardUI.CardUIType.yoggReward);
            deckRewardYogg[i].UpdateCard();
            deckRewardYogg[i].SetRewardCanvas(this);
        }
    }

    public void PickReward(CardUI cu){
        if(!isRewardPicked){
            GameManager.Instance.saver.AddCard(cu.spellinfo, true);
            switch(deckReward.IndexOf(cu)) {
                case 0 :
                    break;
                case 1 : 
                    break;
                case 2 :
                    break;
                default :
                    break;
            }
            isRewardPicked = true;
        }

    }

    public void OnPointerClick(PointerEventData eventdata){
        Debug.Log(eventdata.pointerPressRaycast.gameObject.name);
        if(eventdata.pointerPressRaycast.gameObject == skipButton){
            isRewardPicked = true;
        }
        else if(eventdata.pointerPressRaycast.gameObject == nextStageButton && isRewardPicked){
            Debug.Log("nextstage");
        }
    }

}
