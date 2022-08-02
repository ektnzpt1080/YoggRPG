using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    public Camera camera_;

    [SerializeField] Map mapobject;
    [SerializeField] Location locationobject;
    [SerializeField] Transform starttransform;
    [SerializeField] Transform endtransform;

    [SerializeField] private Color startColor, bossColor, restColor, stageColor, eventColor;

    [SerializeField] TextMeshProUGUI text;
    
    List<Pos> mapPos;
    
    int randomValue;


    public class Pos
    {
        public int x;
        public int y;
        public string name;
    }


    void Awake()
    {
        
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        Instance = this;

        if (MapSaver.mapPos.Count == 0)
        {
            MakeNewMap();
        }
        else
        {
            MakeMapbymapPos(MapSaver.mapPos);
        }

        SpawnLocation();
    }

    public void MakeNewMap()
    {
        mapPos = new List<Pos>();
        
        Pos startpos = new Pos();
        startpos.x = 0;
        startpos.y = 3;
        startpos.name = "Start";
        DrawMap(startpos, startpos.name, startColor);
        mapPos.Add(startpos);
        
        Pos endpos = new Pos();
        endpos.x = 6;
        endpos.y = 3;
        endpos.name = "Boss";
        DrawMap(endpos, endpos.name, bossColor);
        mapPos.Add(endpos);

        // 5:1 / 4:2 / 3:3 / 2:4 / 1:5
        for (int i = 1; i <= 5; i++)
        {
            // 현재 위치들 지정
            List<int> currentPos = new List<int>();
            for (int j = 0; j < mapPos.Count; j++)
            {
                if (mapPos[j].x == i - 1)
                {
                    currentPos.Add(mapPos[j].y);
                }
            }

            //다음 위치들 지정
            List<int> newPos;
            while (true)
            {
                newPos = new List<int>();
                if (1 < i && i < 5)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        randomValue = UnityEngine.Random.Range(1, 6);
                        if (!newPos.Contains(randomValue))
                        {
                            newPos.Add(randomValue);
                        }
                    }
                    while (newPos.Count < 2)
                    {
                        randomValue = UnityEngine.Random.Range(1, 6);
                        if (!newPos.Contains(randomValue))
                        {
                            newPos.Add(randomValue);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        randomValue = UnityEngine.Random.Range(2, 5);
                        if (!newPos.Contains(randomValue))
                        {
                            newPos.Add(randomValue);
                        }
                    }
                    while (newPos.Count < 2)
                    {
                        randomValue = UnityEngine.Random.Range(2, 5);
                        if (!newPos.Contains(randomValue))
                        {
                            newPos.Add(randomValue);
                        }
                    }
                }
                int c = 0;
                foreach (int p in currentPos)
                {
                    if (newPos.Contains(p - 1) || newPos.Contains(p) || newPos.Contains(p + 1))
                    {
                        c += 1;
                    }
                }
                int d = 0;
                foreach (int p in newPos)
                {
                    if (currentPos.Contains(p - 1) || currentPos.Contains(p) || currentPos.Contains(p + 1))
                    {
                        d += 1;
                    }
                }
                if (c == currentPos.Count && d == newPos.Count)
                {
                    break;
                }
            }

            foreach (int k in newPos)
            {
                Pos p = new Pos();
                p.x = i;
                p.y = k;
                if (i != 5)
                {
                    if (UnityEngine.Random.Range(1, 11) <= 8)
                    {
                        DrawMap(p, "Stage", stageColor);
                        p.name = "Stage";
                    }
                    else
                    {
                        DrawMap(p, "Event", eventColor);
                        p.name = "Event";
                    }
                }
                else
                {
                    DrawMap(p, "Rest", restColor);
                    p.name = "Rest";
                }
                mapPos.Add(p);
            }
        }
        // 위아래 1.8 기본

        MapSaver.mapPos = mapPos;
        Pos pos = new Pos();
        pos.x = 0;
        pos.y = 3;
        pos.name = "현재 위치";
        MapSaver.currentLevel = pos;
    }

    public void MakeMapbymapPos(List<Pos> mapPos)
    {
        foreach (Pos pos in mapPos)
        {
            if(pos.name == "Start")
            {
                DrawMap(pos, pos.name, startColor);
            }
            else if (pos.name == "Boss")
            {
                DrawMap(pos, pos.name, bossColor);
            }
            else if (pos.name == "Stage")
            {
                DrawMap(pos, pos.name, stageColor);
            }
            else if (pos.name == "Event")
            {
                DrawMap(pos, pos.name, eventColor);
            }
            else
            {
                DrawMap(pos, pos.name, restColor);
            }
        }
        MapSaver.mapPos = mapPos;
    }

    public void SpawnLocation()
    {
        Pos level = MapSaver.currentLevel;
        Location loc = GameObject.Instantiate(locationobject, new Vector2((float)(2.4 * level.x - 7.2), (float)(1.8 * level.y - 5.4)), Quaternion.identity);
    }
    public void DrawMap(Pos pos, string name, Color color)
    {
        Vector3 vector = new Vector3((float)(pos.x * 2.4 - 7.2), (float)(pos.y * 1.8 - 5.4), 0);
        Map newmap = GameObject.Instantiate(mapobject, vector, Quaternion.identity);
        newmap.Init(name, color);
        newmap.pos = pos;
    }

    private float Width
    {
        get
        {
            return camera_.orthographicSize* Screen.width / Screen.height;
        }
    }

    private float Height
    {
        get
        {
            return camera_.orthographicSize;
        }
    }

    

    public void ShowText(Map map)
    {
        text.text = map.name;
        text.transform.position = new Vector3(map.transform.position.x * 960 / Width, map.transform.position.y * 540 / Height, 0);
        ChangeMapSize(map, true);
    }

    public void HideText(Map map)
    {
        text.text = "";
        ChangeMapSize(map, false);
    }

    public void ChangeMapSize(Map map, bool isLarge)
    {
        if (isLarge)
        {
            map.transform.DOScale(1.4f,0f);
        }
        else
        {
            map.transform.DOScale(1f, 0f);
        }
    }
}