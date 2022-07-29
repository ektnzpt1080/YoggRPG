using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Vector2 position { get; set;}

    public void GetDamage(int d){
        Debug.Log("Player Get Damaged");
    }
}
