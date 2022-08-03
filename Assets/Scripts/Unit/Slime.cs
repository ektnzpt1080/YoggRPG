using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    //두 칸 4방향으로 움직이고 공격
    //공격 후 턴이 종료됨
    void Awake(){
        health = info.health;
        strength = info.strength;
        //나중에 맵에 따라서 자동으로 성장하도록 만들 것
        //애초에 여기서 하면 안 될 듯?
    }

    public override void Move(Stage stage){
        StartCoroutine(_Move(stage));
    }
    
    public override void Attack(Stage stage){
        StartCoroutine(_Attack(stage));
    }
    
    private IEnumerator _Move(Stage stage){
        int movement = 3; //2번 움직일 수 있음
        List<Vector2> way = Gridlib.FindWay(stage,position,stage.player.position);
        attackRange = new List<Vector2>();
        for(int i = 0; i < movement; i++){    
            if(way == null) break; // 갈수 없으면 움직이지 않음
            if(Gridlib.IsAdjacent(stage.player.position, position) ){
                attackRange = new List<Vector2>(){stage.player.position};
                break;
            }
            Vector3 dest = transform.position + new Vector3 (way[i].x, way[i].y, 0);
            int speed = 20;
            for(int j = 0; j < speed - 1;j++){
                transform.position += new Vector3(way[i].x, way[i].y, 0) / speed;
                yield return new WaitForSeconds(0.01f);
            }
            transform.position = dest;
            position += way[i];
        }

        if(Gridlib.IsAdjacent(stage.player.position, position) ){
            attackRange = new List<Vector2>(){stage.player.position};
        }
        else{
            List<Vector2> way4 = new List<Vector2>(){Vector2.up, Vector2.down, Vector2.left, Vector2.right};
            List<Vector2> candidate = new List<Vector2>();
            foreach(Vector2 v in way4){
                if(Gridlib.InRange(stage, position + v)){
                    candidate.Add(position + v);
                }
            }
            foreach(Vector2 v in Gridlib.ClosePositions(candidate, stage.player.position)){
                if (!stage.EnemyTile().Contains(v)){
                    attackRange.Add(v);
                    break;
                }
            }
        }

        foreach(Vector2 pos in attackRange){
            stage.tiles[pos].Highlight();
        }
        GameManager.Instance.BattleManager.EndEnemyAct();
    }

    private IEnumerator _Attack(Stage stage){
        int damage = strength; // 나중에 바꿀것
        if(attackRange.Contains(stage.player.position)){
            stage.player.GetDamage(damage);
        }
        GameManager.Instance.BattleManager.EndEnemyAct();
        yield break;
    }

}
