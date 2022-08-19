using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBarUI : MonoBehaviour
{
    Enemy enemy;

    int maxhp;
    [SerializeField] Canvas HPUICanvas;
    RectTransform _rectParent;

    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float up;


    public void Init(Enemy _enemy, Canvas _HPUICanvas){
        enemy = _enemy;
        HPUICanvas = _HPUICanvas;
        _rectParent = HPUICanvas.GetComponent<RectTransform>();
        maxhp = _enemy.health;
    }

    void Update(){
        if(enemy != null){
            Vector3 pos = Camera.main.WorldToScreenPoint(enemy.transform.position + Vector3.up * up);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectParent, pos, null, out localPos);
            transform.localPosition = localPos;

            slider.value = (float) enemy.health / maxhp;
            text.text = enemy.health.ToString();
        }
        else GameObject.Destroy(this.gameObject);
        
        
    }


}
