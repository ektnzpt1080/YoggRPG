using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridlib
{
    static List<Vector2> way4 = new List<Vector2> {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
    static List<Vector2> way8 = new List<Vector2> {Vector2.right, Vector2.left, Vector2.up, Vector2.down,
        Vector2.right + Vector2.up, Vector2.right + Vector2.down, Vector2.left + Vector2.up, Vector2.left + Vector2.down};


    /// <summary>
    /// 두 점 p1, p2가 인접해있다면 true, 아니면 false를 반환 
    /// </summary>
    public static bool IsAdjacent(Vector2 p1, Vector2 p2){
        if (p1.x + 1 == p2.x && p1.y == p2.y){
            return true;
        }
        else if(p1.x - 1 == p2.x && p1.y == p2.y){
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
                    else if(InRange(stage, movePoint) && !stage.AssignedTile().Contains(movePoint) && !way.ContainsKey(movePoint)){    
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
    /// stage 안에 p가 있으면 true를, 아니면 false를 반환
    /// </summary>
    public static bool InRange(Stage stage, Vector2 p){
        if(p.x >= 0 && p.x < stage.size.x && p.y >= 0 && p.y < stage.size.y) return true;
        else return false;
    }
    
    /// <summary>
    /// position의 1서클 주변 좌표들을 List<Vector2>의 형태로 반환
    /// </summary>
    public static List<Vector2> Circle(Stage stage, Vector2 position){
        return WayStraight(stage, position, true, 1);
    }

    /// <summary>
    /// position에서 4방향으로 <movement>번 이동해서 갈 수 있는 좌표들을 List<Vector2>의 형태로 반환
    /// Stage의 장애물을 고려함
    /// </summary>
    public static List<Vector2> CanReach(Stage stage, Vector2 position, int movement){
        List<Vector2> result = new List<Vector2>();
        List<Vector2> lastVisitedPoints = new List<Vector2> {position};
        for(int i = 0 ; i < movement ; i++){
            List<Vector2> temp = new List<Vector2>();
            foreach(Vector2 point in lastVisitedPoints){
                for(int j = 0 ; j < 4 ; j++){
                    Vector2 reachPoint = point + way4[j];
                    if(InRange(stage, reachPoint) && !stage.AssignedTile().Contains(reachPoint) && !result.Contains(reachPoint)){
                        temp.Add(reachPoint);
                        result.Add(reachPoint);
                    }
                }
            }
            lastVisitedPoints = temp;
        }

        return result;
    }

    /// <summary>
    /// <vectorList>에 있는 좌표들중 <pos>에 더 가까운 좌표를 순서대로 List<Vector2>형태로 반환
    /// </summary>
    public static List<Vector2> ClosePositions(List<Vector2> vectorList, Vector2 pos){
        List<Vector2> result = new List<Vector2>();
        List<(float, Vector2)> distanceList = new List<(float, Vector2)>();
        foreach(Vector2 v in vectorList){
            distanceList.Add(((v - pos).SqrMagnitude(), v));
        }
        distanceList.Sort(delegate((float,Vector2) x, (float,Vector2) y) {
            return x.Item1.CompareTo(y.Item1);
        });
        foreach(var fv in distanceList) {
            result.Add(fv.Item2);
        }
        return result;
    }
    
    /// <summary>
    /// <pos>를 기준으로 4방향 혹은 8방향 직선으로 <distance>까지 떨어진 위치들을 리턴함
    /// <distance>가 -1일시 distance는 무한이 됨
    /// </summary>
    public static List<Vector2> WayStraight(Stage stage, Vector2 pos, bool is8way, int distance = -1){
        List<Vector2> result = new List<Vector2>();
        List<Vector2> way = is8way ? way8 : way4;
        if( distance == -1 && stage != null){
            bool inRange = true;
            int i = 1;
            while(inRange){
                inRange = false;
                foreach (Vector2 v in way){
                    if(InRange(stage, pos + v * i)){
                        result.Add(pos + v * i);
                        inRange = true;
                    }
                }
                i++;
            }
        }
        else{
            foreach(Vector2 v in way){
                for(int i = 1; i <= distance; i++) {
                    if(InRange(stage, pos + v * i)) result.Add(pos + v * i);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// <pos>를 기준으로 4방향 혹은 8방향 직선으로 <distance>까지 떨어진 위치들을 리턴하되, assigned tile에 막힘
    /// assigendtile도 포함해서 리턴함
    /// <distance>가 -1일시 distance는 무한이 됨
    /// </summary>
    public static List<Vector2> WayStraightBlockable(Stage stage, Vector2 pos, bool is8way, int distance = -1){
        List<Vector2> result = new List<Vector2>();
        List<Vector2> way = is8way ? way8 : way4;
        List<bool> iswayAvailable = new List<bool>();
        for(int j = 0; j < way.Count; j++){
            iswayAvailable.Add(true);
        }

        if(distance == -1 && stage != null){
            bool inRange = true;
            int i = 1;
            while(inRange){
                inRange = false;
                for(int j = 0; j < way.Count; j++){
                    Vector2 v = pos + way[j] * i;
                    if(iswayAvailable[j] && InRange(stage, v)){
                        result.Add(v);
                        inRange = true;
                        if(stage.AssignedTile().Contains(v)){
                            iswayAvailable[j] = false;
                        }
                    }
                }
                i++;
            }
        }
        else{
            for(int j = 0; j < way.Count; j++){
                for(int i = 1; i <= distance; i++) {
                    Vector2 v = pos + way[j] * i;
                    if(iswayAvailable[j] && InRange(stage, pos + way[j] * i)) {
                        result.Add(v);
                        if(stage.AssignedTile().Contains(v)){
                            iswayAvailable[j] = false;
                        }
                    }
                }
            }
        }
        
        return result;
    }
}
