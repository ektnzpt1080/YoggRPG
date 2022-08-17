using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    public override void Move(Stage stage){
        StartCoroutine(this._Move(stage, true));
    }
    
    public override void Attack(Stage stage){
        StartCoroutine(this._Attack(stage));
    }
    
    private IEnumerator _Move(Stage stage, bool directlyAttack){
        Vector2 pp = stage.player.position;

        List<Vector2> candidate = Gridlib.WayStraightBlockable(stage, position, false);
        List<Vector2> range1 = new List<Vector2>() {pp + Vector2.up, pp + Vector2.down, pp + Vector2.left, pp + Vector2.right};
        List<Vector2> range2 = new List<Vector2>() {pp + Vector2.left + Vector2.up, pp + Vector2.right + Vector2.up, pp + Vector2.left + Vector2.down, pp + Vector2.right + Vector2.down};
        
        //range1 공격 범위 안이면 움직이지 않음
        if(!range1.Contains(position)){
            //AssignedTile을 지움
            foreach(Vector2 v in stage.AssignedTile()){
                candidate.Remove(v);
            }
            
            //공격 상태이면 Range1으로 이동
            List<Vector2> way = null;
            if(directlyAttack){
                foreach(Vector2 v in range1){
                    if(candidate.Contains(v)){
                        way = Gridlib.FindWay(stage, position, v);
                        if(way != null) break;
                    }
                }
            }
            //견제 상태이거나 Range1으로 못 가면 Range2로 감
            if(way == null) {
                foreach(Vector2 v in range2){
                    if(candidate.Contains(v)){
                        way = Gridlib.FindWay(stage, position, v);
                        if(way != null) break;
                    }
                }
            }
            //다 안되면 가장 가까운 곳으로 이동
            if(way == null){
                way = Gridlib.FindWay(stage, position, Gridlib.ClosePositions(candidate, pp)[0]);
            }

            //이동
            if(way != null){
                for(int i = 0; i < way.Count; i++){
                    Vector3 dest = transform.position + new Vector3 (way[i].x, way[i].y, 0);
                    int speed = 20;
                    for(int j = 0; j < speed - 1;j++){
                        transform.position += new Vector3(way[i].x, way[i].y, 0) / speed;
                        yield return new WaitForSeconds(0.01f);
                    }
                    transform.position = dest;
                    position += way[i];
                }
            }
            //여기 따로 뺄 생각해야 될듯?
        }
        
        attackRange = Gridlib.Circle(stage, position);
        foreach(Vector2 pos in attackRange){
            stage.tiles[pos].Highlight();
        }
        GameManager.Instance.BattleManager.EndEnemyAct();

    }

    private IEnumerator _Attack(Stage stage){
        int damage = strength; // 나중에 바꿀것
        if(attackRange.Contains(stage.player.position)){
            stage.player.GetDamage(damage, this);
        }
        GameManager.Instance.BattleManager.EndEnemyAct();
        yield break;
    }

}
