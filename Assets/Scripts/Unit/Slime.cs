using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    //두 칸 4방향으로 움직이고 공격
    //공격 후 턴이 종료됨
    void Awake(){
        health = 2;
    }
    
    public override void Behaviour(Stage stage){
        StartCoroutine(_Behaviour(stage));
    }
    
    private IEnumerator _Behaviour(Stage stage){
        int movement = 2; //2번 움직일 수 있음
        int damage = 0; //
        List<Vector2> way = Gridlib.FindWay(stage,position,stage.player.position);
        if(Gridlib.IsAdjacent(stage.player.position, position) ){
            stage.player.GetDamage(damage);
            GameManager.Instance.BattleManager.EndEnemyAct();
            yield break;
        }
        for(int i = 0; i < movement; i++){    
            Vector3 dest = transform.position + new Vector3 (way[i].x, way[i].y, 0);
            int speed = 20;
            for(int j = 0; j < speed - 1;j++){
                transform.position += new Vector3(way[i].x, way[i].y, 0) / speed;
                yield return new WaitForSeconds(0.01f);
            }
            transform.position = dest;
            position += way[i];
            if(Gridlib.IsAdjacent(stage.player.position, position) ){
                stage.player.GetDamage(damage);
                GameManager.Instance.BattleManager.EndEnemyAct();
                break;
            }
        }
        GameManager.Instance.BattleManager.EndEnemyAct();
    }
}
