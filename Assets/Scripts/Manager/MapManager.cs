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
    [SerializeField] Transform starttransform;
    [SerializeField] Transform endtransform;

    [SerializeField] private Color startColor, bossColor, restColor, stageColor, eventColor;

    [SerializeField] TextMeshProUGUI text;
    
    List<Pos> mapPos = new List<Pos>();
    
    int randomValue;
    
    public class Pos
    {
        public int x;
        public int y;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 startPos = starttransform.position;
        Vector3 endPos = endtransform.position;

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);

        Instance = this;

        CreateMap(startPos, "Start", startColor);
        Pos startpos = new Pos();
        startpos.x = 0;
        startpos.y = 3;
        mapPos.Add(startpos);
        CreateMap(endPos, "Boss", bossColor);
        Pos endpos = new Pos();
        endpos.x = 6;
        endpos.y = 3;
        mapPos.Add(startpos);
        mapPos.Add(endpos);

        // 5:1 / 4:2 / 3:3 / 2:4 / 1:5
        for (int i = 1; i<=5; i++)
        {
            // 현재 위치들 지정
            List<int> currentPos = new List<int>();
            for (int j = 0; j < mapPos.Count; j++)
            {
                if (mapPos[j].x == i - 1)
                {
                    currentPos.Add(mapPos[j].y);
                    print(i - 1 +" "+ mapPos[j].y);
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
                    if(newPos.Contains(p - 1) || newPos.Contains(p) || newPos.Contains(p + 1))
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
                if(c==currentPos.Count && d == newPos.Count)
                {
                    foreach (int p in newPos)
                    {
                        Pos pos = new Pos();
                        pos.x = i;
                        pos.y = p;
                        mapPos.Add(pos);
                    }
                    break;
                }         
            }

            foreach (int k in newPos)
            {
                Pos p = new Pos();
                p.x = i;
                p.y = k;
                mapPos.Add(p);
                float y = (float)(1.8 * k - 5.4);
                if (i != 5)
                {
                    if (UnityEngine.Random.Range(1, 11) <= 8)
                    {
                        CreateMap((startPos * (6-i) + endPos * i) / 6 + new Vector3(0, y, 0), "Stage", stageColor);
                    }
                    else
                    {
                        CreateMap((startPos * (6 - i) + endPos * i) / 6 + new Vector3(0, y, 0), "Event", eventColor);
                    }
                }
                else
                {
                    CreateMap((startPos * (6 - i) + endPos * i) / 6 + new Vector3(0, y, 0), "Rest", restColor);
                }
                
            }
        }


        // 위아래 1.8 기본


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

    public void CreateMap(Vector3 vector, string name, Color color)
    {
        Map newmap = GameObject.Instantiate(mapobject, vector, Quaternion.identity);
        newmap.Init(name, color);
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