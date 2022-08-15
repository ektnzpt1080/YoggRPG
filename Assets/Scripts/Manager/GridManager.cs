using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Player player;
    [SerializeField] private Camera cam;
    [SerializeField] private Enemy enemy, enemy2, enemy3;
    Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
    [SerializeField] private EnemyData enemydata;
    
    [SerializeField] private GameObject moveButton, endButton, yoggButton;
    [SerializeField] private List<GameObject> buttonList;
    

    public Stage GenerateStage(int w, int h, int e){
        //나중에 enemy를 List로 받아서 만들게 할것
        GenerateGrid(w,h);
        

        Player pb = Instantiate(player, new Vector3(w/2,1,-1), Quaternion.identity);        
        pb.position = new Vector2 (w/2,1);
        
        List<Vector2> assignedPositions = new List<Vector2>();
        assignedPositions.Add(pb.position);
        
        List<Enemy> assignedEnemies = new List<Enemy>();
        Vector2 assignPos;
        for (int i = 0; i < e ; i++){
            do{
                int x = Random.Range(0,w);
                int y = Random.Range(4,h);
                assignPos = new Vector2(x, y);
            } while(assignedPositions.Contains(assignPos));
            Enemy spawnedEnemy = Instantiate(enemy3, new Vector3(assignPos.x,assignPos.y,-1), Quaternion.identity);
            spawnedEnemy.Init(enemydata.Enemies[1]);
            spawnedEnemy.position = assignPos;
            assignedPositions.Add(assignPos);
            assignedEnemies.Add(spawnedEnemy);
        }

        return new Stage(new Vector2 (w,h), pb, assignedEnemies, tiles);

    }
    
    public void GenerateGrid(int w, int h){
        //나중에 width나 height도 직접 조정 가능하게 바꿀것
        for(int x = 0; x < w; x++){
            for(int y = 0; y < h; y++){
                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = "Tile" + x.ToString() + y.ToString();
                spawnedTile.position = new Vector2(x,y);
                bool offset = ((x+y)%2!=0);
                spawnedTile.Init(offset);
                tiles.Add(new Vector2(x,y), spawnedTile);
            }
        }
        
        buttonList.Add(Instantiate(moveButton, tiles[new Vector2(0,0)].transform.position + new Vector3(-2.5f,0.5f,0), Quaternion.identity));
        buttonList.Add(Instantiate(endButton, tiles[new Vector2(w-1,0)].transform.position + new Vector3(2.5f,0.5f,0), Quaternion.identity));
        buttonList.Add(Instantiate(yoggButton, tiles[new Vector2(w-1,0)].transform.position + new Vector3(2.5f,2.0f,0), Quaternion.identity));

        cam.transform.position = new Vector3(w/2, h/2 - 1, -10);
        
        //카메라 위치, 크기 조정 필요
    }

    public Tile FindTile(Vector2 pos){
        return tiles[pos];
    }

    public List<GameObject> GetButtonList(){
        return buttonList;
    }

}
