using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    enum Turn{
        player,
        enemy
    }

    enum State{
        startBattleState,
        drawCardState,
        selectBehaviourState,
        pickMoveState,
        pickCardState,
        playCardState,
        EndTurnState,
        EnemyAttackState,
        
        EnemyMoveState,
    }

    bool isBattle;//battle이 시작되었는지
    bool enemyActing;//enmey가 행동중
    int mana; //마나
    
    Turn turn;
    State state;
    Stage stage;
    [SerializeField] private Collider2D moveButton;
    SpriteRenderer moveButtonSprite;
    [SerializeField] private Color clickedColor;
    [SerializeField] private Color nonclickedColor;
    
    [SerializeField] TextMeshProUGUI text;

    void Awake(){
        moveButtonSprite = moveButton.GetComponent<SpriteRenderer>();
        mana = 3;
    }
    void Update(){
        if(isBattle){
            switch (state){
                //Battle이 시작하는 state, 바로 MoveState로 간다
                case State.startBattleState :
                    StartCoroutine(_EnemyMoveStart(stage));
                    state = State.EnemyMoveState;
                    break;
                //드로우 하는 state
                case State.drawCardState :
                    GameManager.Instance.CardManager.DrawCard();
                    state = State.selectBehaviourState;
                    break;
                //카드를 고르거나 move 버튼을 누를 수 있음, 혹은 턴 엔드
                case State.selectBehaviourState :
                    ClickMoveButton();
                    GameManager.Instance.CardManager.PickCard();
                    EndPlayerTurn();
                    break;
                //move 버튼을 눌렀을 시, 움직일 범위를 선택 할 수 있음
                case State.pickMoveState :
                    Move();
                    break;
                //카드를 고른 후 범위를 선택, 몇몇 카드는 자동으로 생략됨
                case State.pickCardState :
                    GameManager.Instance.CardManager.PlayCard();
                    break;
                //카드 애니메이션 재생
                case State.playCardState :
                    break;
                //턴 종료
                case State.EndTurnState :
                    break;
                //상대 턴 공격 상태, 지정된 범위를 공격함
                case State.EnemyAttackState :
                    break;
                //상대 턴 이동 상태, 적절한 곳으로 이동하고, 공격을 예고함
                case State.EnemyMoveState :
                    break;
                default :
                    break;
            }
            
            //MoveButton UI
            if(state == State.pickMoveState){
                moveButtonSprite.color = clickedColor;
            }
            else{
                moveButtonSprite.color = nonclickedColor;
            }

            //Mana UI
            text.text = "Mana : " + mana + "/3";
        }
    }

    public void StartBattle(Stage s){
        isBattle = true;
        turn = Turn.player;
        state = State.startBattleState;
        stage = s;
    }
    public Stage GetStage(){
        return stage;
    }
    public void EndPlayerTurn(){
        if(Input.GetKeyDown(KeyCode.Z)){
            turn = Turn.enemy;
            state = State.EnemyAttackState;
            StartCoroutine(_EnemyAttackStart(stage));
        }
    }
    public void EndEnemyTurn(){
        turn = Turn.player;
        state = State.drawCardState;
        mana = 3;
    }
    public void EndStagePlayerDead(){
        //player 사망
    }
    public void EndStageEnemyDead(){
        Debug.Log("Enemy All Dead");
    }
    
    private IEnumerator _EnemyAttackStart(Stage stage){
        foreach(Enemy enemy in stage.enemies){
            enemyActing = true;
            enemy.Attack(stage);
            while(enemyActing){
                yield return null;
            }
        }
        state = State.EnemyAttackState;
        StartCoroutine(_EnemyMoveStart(stage));
    }
    
    private IEnumerator _EnemyMoveStart(Stage stage){
        foreach(Enemy enemy in stage.enemies){
            enemyActing = true;
            enemy.Move(stage);
            while(enemyActing){
                yield return null;
            }
        }
        EndEnemyTurn();
        state = State.drawCardState;
    }

    public void EndEnemyAct(){
        enemyActing = false;
    }
    public void RemoveEnemy(Enemy enemy){
        stage.enemies.Remove(enemy);
        if(stage.enemies.Count == 0){
            EndStageEnemyDead();
        }
    }

    public void ClickMoveButton(){
        if (Input.GetMouseButtonDown(0) && mana > 0){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
            if(hit.collider == moveButton) {
                foreach(Vector2 pos in Gridlib.CanReach(stage, stage.player.position, 2)){
                    stage.tiles[pos].Highlight();
                }
                state = State.pickMoveState;
            }
        }
    }

    public void Move(){
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);    
            if(hit.collider != null && hit.collider.TryGetComponent<Tile>(out Tile tile)){
                if(Gridlib.CanReach(stage, stage.player.position, 2).Contains(tile.position)){
                    stage.player.position = tile.position;
                    stage.player.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, stage.player.transform.position.z);
                    mana--;
                }
                state = State.selectBehaviourState;
            }
            else {
                state = State.selectBehaviourState;
            }
        }
        else if (Input.GetMouseButtonDown(1)) {
            state = State.selectBehaviourState;
        }
    }

    public void Select(bool select){
        if(select){
            state = State.pickCardState;
        }
        else{
            state = State.selectBehaviourState;
        }
    }
}
