using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewardCanvas : MonoBehaviour
{
    [SerializeField] List<CardUI> deckReward;
    [SerializeField] List<CardSmallUI> deckRewardYogg;
    [SerializeField] GameObject skipButton;

    [SerializeField] SpellData sd;

    public void RandomReward(){        
        List<SpellInfo> sl = new List<SpellInfo>();
        while(sl.Count < 3){
            SpellInfo candidate = sd.spellinfo[Random.Range(0, sd.spellinfo.Length)];
            if(!sl.Contains(candidate)){
                sl.Add(candidate);
            }
        }

        for(int i = 0 ; i < 3 ; i++){
            deckReward[i].Copy(sl[i]);
            deckReward[i].UpdateCard();
        }

        for(int i = 0 ; i < 9 ; i++){
            deckRewardYogg[i].Copy(sd.spellinfo[Random.Range(0, sd.spellinfo.Length)]);
            deckRewardYogg[i].UpdateCard();
        }

    }



}
