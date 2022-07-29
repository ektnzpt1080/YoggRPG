using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridlib
{
    /// <summary>
    /// 두 점 p1, p2가 인접해있다면 true, 아니면 false를 반환 
    /// </summary>
    public static bool IsAdjacent(Vector2 p1, Vector2 p2){
        if (p1.x + 1 == p2.x && p1.y == p2.y){
            return true;
        }
        else if(p1.x -1 == p2.x && p1.y == p2.y){
            return true;
        }
        else if(p1.x == p2.x && p1.y + 1 == p2.y){
            return true;
        }
        else if(p1.x == p2.x && p1.y - 1 == p2.y){
            return true;
        }
        else return false;
    }

    /// <summary>
    /// stage에서 startpoint에서 endpoint로 가는 경로를 normalized Vector2의 List 형태로 반환
    /// </summary>
    public static List<Vector2> FindWay(Stage stage, Vector2 startpoint, Vector2 endpoint){
        Vector2 size = stage.size;
        List <Vector2> lastVisitedPoints = new List<Vector2>();
        lastVisitedPoints.Add(startpoint);
        Dictionary<Vector2, Vector2> way = new Dictionary<Vector2, Vector2>();
        List<Vector2> way4 = new List<Vector2> {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
        while(!lastVisitedPoints.Contains(endpoint) && lastVisitedPoints.Count != 0){
            List<Vector2> tempList = new List<Vector2>();
            foreach (Vector2 point in lastVisitedPoints){
                for(int i = 0; i < 4; i++){
                    Vector2 movePoint = point + way4[i];
                    if(movePoint == endpoint && !way.ContainsKey(movePoint)){
                        way.Add(movePoint, way4[i]);
                        tempList.Add(movePoint);
                        break;
                    }
                    else if(InRange(size, movePoint) && !stage.AssignedTile().Contains(movePoint) && !way.ContainsKey(movePoint)){    
                        way.Add(movePoint, way4[i]);
                        tempList.Add(movePoint);
                    }
                }
            }
            lastVisitedPoints = tempList;    
        }

        List<Vector2> result = new List<Vector2>();
        if(!way.ContainsKey(endpoint)) return result; // 갈수 없으면 빈 리스트를 return
        Vector2 _endpoint = endpoint;
        while(_endpoint != startpoint){
            result.Add(way[_endpoint]);
            _endpoint -= way[_endpoint];
        }
        result.Reverse();
        return result;
    }
    
    /// <summary>
    /// size.x * size.y의 직사각형안에 p가 있으면 true를, 아니면 false를 반환
    /// </summary>
    public static bool InRange(Vector2 size, Vector2 p){
        if(p.x >= 0 && p.x < size.x && p.y >= 0 && p.y < size.y) return true;
        else return false;
    }
    
    /// <summary>
    /// position의 1서클 주변 좌표들을 List<Vector2>의 형태로 반환
    /// </summary>
    public static List<Vector2> Circle(Vector2 position){
        List<Vector2> points = new List<Vector2>();
        float x = position.x;
        float y = position.y;
        points.Add(new Vector2(x-1, y+1));
        points.Add(new Vector2(x, y+1));
        points.Add(new Vector2(x+1, y+1));
        points.Add(new Vector2(x-1, y));
        points.Add(new Vector2(x+1, y));
        points.Add(new Vector2(x-1, y-1));
        points.Add(new Vector2(x, y-1));
        points.Add(new Vector2(x+1, y-1));
        return points;
    }

    /// <summary>
    /// position에서 <movement>번 이동해서 갈 수 있는 좌표들을 List<Vector2>의 형태로 반환
    /// Stage의 장애물을 고려함
    /// </summary>
    public static List<Vector2> CanReach(Stage stage, Vector2 position, int movement){
        List<Vector2> result = new List<Vector2>();
        List<Vector2> lastVisitedPoints = new List<Vector2> {position};
        List<Vector2> way4 = new List<Vector2> {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
        Vector2 size = stage.size;
        for(int i = 0 ; i < movement ; i++){
            List<Vector2> temp = new List<Vector2>();
            foreach(Vector2 point in lastVisitedPoints){
                for(int j = 0 ; j < 4 ; j++){
                    Vector2 reachPoint = point + way4[j];
                    if(InRange(size, reachPoint) && !stage.AssignedTile().Contains(reachPoint) && !result.Contains(reachPoint)){
                        temp.Add(reachPoint);
                        result.Add(reachPoint);
                    }
                }
            }
            lastVisitedPoints = temp;
        }

        return result;
    }
}
