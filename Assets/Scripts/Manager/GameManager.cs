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
    List<Spell> yl = new List<Spell>();

    [SerializeField] PlayerData playerdata;
    

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
        
        cl.Add(new Poke());
        cl.Add(new Poke());
        cl.Add(new Step());
        cl.Add(new Step());
        cl.Add(new RunRunRun());
        cl.Add(new Slash());
        cl.Add(new Slash());
        cl.Add(new RoundSlash());
        cl.Add(new ShieldUp());
        cl.Add(new ShieldUp());
        cl.Add(new WoodShield());

        yl.Add(new Poke());
        yl.Add(new Poke());
        yl.Add(new Step());
        yl.Add(new Step());
        yl.Add(new RunRunRun());
        yl.Add(new Slash());
        yl.Add(new Slash());
        yl.Add(new RoundSlash());
        yl.Add(new ShieldUp());
        yl.Add(new ShieldUp());
        yl.Add(new WoodShield());

        CardManager.SetCardList(cl, yl);
        Stage stage_ex = GridManager.GenerateStage(7,7,2);
        stage_ex.player.Initialize(playerdata);
        BattleManager.StartBattle(stage_ex);
    }

    

}
