using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{   
    public override void Move(Stage stage){
        StartCoroutine(this._Move(stage));
    }

    public override void Attack(Stage stage){
        StartCoroutine(this._Attack(stage));
    }

    private IEnumerator _Move(Stage stage){
        int movement = 2;
        Vector2 pp = stage.player.position;
        List<Vector2> way4 = new List<Vector2>(){Vector2.left, Vector2.right, Vector2.up, Vector2.down};
        bool isAttackable = false;
        foreach(Vector2 v in way4){
            if(v * 2 + position == pp) isAttackable = true;
        }
        
        if(!isAttackable){
            List<Vector2> points = new List<Vector2>();
            foreach (Vector2 v in way4) {
                if(Gridlib.InRange(stage, pp + v * 2) && !stage.AssignedTile().Contains(pp + v * 2) ) points.Add(pp + v * 2);
            }
            List<Vector2> candidate = Gridlib.ClosePositions(points, position);
            
            points = new List<Vector2>();
            foreach (Vector2 v in way4) {
                if(Gridlib.InRange(stage, pp + v) && !stage.AssignedTile().Contains(pp + v)) points.Add(pp + v);
            }
            candidate.AddRange(Gridlib.ClosePositions(points, position));
            
            //이동속도 안에서 갈 수 있는 곳으로 움직임
            List<Vector2> way = new List<Vector2>();
            foreach (Vector2 v in candidate){
                way = Gridlib.FindWay(stage, position, v);
                if(way == null || way.Count > movement) {
                    way = null;
                    continue;
                }
                else break;
            }

            //전부 이동속도로 못가면 가장 가까운 곳으로 움직임
            if(way == null){
                way = Gridlib.FindWay(stage, position, pp);
                if(way != null && way.Count > 0){
                    way.RemoveAt(way.Count - 1);
                }
            }
            
            //아예 못 가면 안 움직이고, way를 따라 움직임
            if(way != null){
                int moveNum = (movement < way.Count) ? movement : way.Count;
                for(int i = 0; i < moveNum; i++){
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
        }
        
        attackRange = new List<Vector2>();
        List<Vector2> pos4 = new List<Vector2>();
        foreach(Vector2 v in way4){
            if(Gridlib.InRange(stage, position+v)) pos4.Add(position + v);
        }
        List<Vector2> pos4c = Gridlib.ClosePositions(pos4, stage.player.position); 
        Vector2 c;
        if(pos4c.Count > 1 && (stage.player.position - pos4c[0]).SqrMagnitude() == (stage.player.position - pos4c[1]).SqrMagnitude()){
            c = pos4c[Random.Range(0,2)] - position;            
        }
        else{
            c = pos4c[0] - position;
        }
        
        if(Gridlib.InRange(stage, position + c)) attackRange.Add(position + c); 
        if(Gridlib.InRange(stage, position + 2 * c)) attackRange.Add(position + 2 * c);

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
