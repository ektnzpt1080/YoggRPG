using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GridManager GridManager { get; private set; }
    public BattleManager BattleManager { get; private set; }
    public CardManager CardManager { get; private set; }
    List<Spell> cl = new List<Spell>();
    List<Spell> yl = new List<Spell>();

    [SerializeField] PlayerData playerdata;
    public Saver saver;

    private void Awake()
    {
        saver = FindObjectOfType<Saver>();
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;

        GridManager = GetComponentInChildren<GridManager>();
        BattleManager = GetComponentInChildren<BattleManager>();
        CardManager = GetComponentInChildren<CardManager>();
    }

    private void Start(){
        CardManager.SetCardList(saver.cl, saver.yl);
        Stage stage_ex = GridManager.GenerateStage(7,7,2);
        stage_ex.player.Initialize(saver.player);
        BattleManager.StartBattle(stage_ex);
    }

    

}
