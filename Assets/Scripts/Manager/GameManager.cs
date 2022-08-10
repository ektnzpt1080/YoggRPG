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
    public UIManager UIManager { get; private set; }

    List<SpellInfo> cl = new List<SpellInfo>();
    List<SpellInfo> yl = new List<SpellInfo>();

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
        UIManager = GetComponentInChildren<UIManager>();
    }

    private void Start(){
        cl = saver.cl;
        yl = saver.yl;

        CardManager.SetCardList(cl, yl);
        Stage stage_ex = GridManager.GenerateStage(7,7,5);
        stage_ex.player.Initialize(saver.player);
        BattleManager.StartBattle(stage_ex);
    }

    

}
