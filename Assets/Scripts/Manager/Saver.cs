using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    public MapManager.Pos currentLevel;
    //���� Ŭ����� �þ�� ��!!
    
    public List<MapManager.Pos> mapPos = new List<MapManager.Pos>();
    public List<Spell> cl = new List<Spell>();
    public List<Spell> yl = new List<Spell>();
    public Player player;
    [SerializeField] PlayerData playerdata;


//저장해야 할것
//요그덱, 카드덱, Player HP, Player 장비(지금은 힘과 지능)


    void Awake()
    {
        var obj = FindObjectsOfType<Saver>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        
        //Initialize Card Deck
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
        yl.Add(new Slash());
        yl.Add(new Slash());
        yl.Add(new RoundSlash());
        yl.Add(new ShieldUp());
        yl.Add(new ShieldUp());
        yl.Add(new WoodShield());
        
        //Initialize Player Data
        player.Initialize(playerdata);

        if(SceneManager.GetActiveScene().name == "InitScene")
        {
            SceneManager.LoadScene("MapScene");
        }

    }

    public void AddCard(Spell spell, bool isCL){
        List<Spell> list = isCL ? cl : yl;
        list.Add(spell);
    }

}
