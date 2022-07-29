using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage
{
    // Start is called before the first frame update
    public Vector2 size {get; set;}
    public Player player {get; set;}
    public List<Enemy> enemies {get; set;}
    public Dictionary<Vector2, Tile> tiles {get; set;}
    public Stage(Vector2 _size, Player _player, List<Enemy> _enemies, Dictionary<Vector2, Tile> _tiles){
        size = _size;
        player = _player;
        enemies = _enemies;
        tiles = _tiles;
    }

    public List<Vector2> AssignedTile(){
        List<Vector2> result = new List<Vector2>();
        result.Add(player.position);
        foreach (Enemy enemy in enemies){
            result.Add(enemy.position);
        }
        return result;
    }

}

public class PRS
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public PRS(Vector3 _position, Quaternion _rotation, Vector3 _scale){
        position = _position;
        rotation = _rotation;
        scale = _scale;
    }
}