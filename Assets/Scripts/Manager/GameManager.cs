using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GridManager GridManager { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public StageBuilder StageBuilder { get; private set; }
    public CardManager CardManager { get; private set; }
    List<Spell> cl = new List<Spell>();
    public int clLength;
    private void Awake() 
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;

        GridManager = GetComponentInChildren<GridManager>();
        BattleManager = GetComponentInChildren<BattleManager>();
        StageBuilder = GetComponentInChildren<StageBuilder>();
        CardManager = GetComponentInChildren<CardManager>();
    }

    private void Start(){
        for(int i = 0 ; i < clLength ; i++){
            cl.Add(new Poke());
        }
        CardManager.SetCardList(cl);
        Stage stage_ex = GridManager.GenerateStage(7,7,2);
        BattleManager.StartBattle(stage_ex);
    }


}
