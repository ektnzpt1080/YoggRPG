using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    public MapManager.Pos currentLevel;
    //���� Ŭ����� �þ�� ��!!
    
    public List<MapManager.Pos> mapPos = new List<MapManager.Pos>();
    [SerializeField] public List<SpellInfo> cl = new List<SpellInfo>();
    [SerializeField] public List<SpellInfo> yl = new List<SpellInfo>();
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
        
        //Initialize Player Data
        player.Initialize(playerdata);

        if(SceneManager.GetActiveScene().name == "InitScene")
        {
            SceneManager.LoadScene("MapScene");
        }

    }

    public void AddCard(SpellInfo spell, bool isCardList){
        List<SpellInfo> list = isCardList ? cl : yl;
        list.Add(spell);
    }

}
