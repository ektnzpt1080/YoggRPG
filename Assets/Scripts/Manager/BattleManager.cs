using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{

    enum State{
        startBattleState,
        drawCardState,
        selectBehaviourState,
        pickMoveState,
        pickCardState,
        playCardState,
        YoggCallingState,
        EndTurnState,
        EnemyAttackState,
        EnemyMoveState,
    }

    bool isBattle;//battle이 시작되었는지
    bool enemyActing;//enmey가 행동중
    int mana; //마나
    int movementmana; // 이동하는데 필요한 마나, 덱을 섞으면 1로 초기화 됨
    
    State state;
    Stage stage;
    [SerializeField] private Color clickedColor;
    [SerializeField] private Color nonclickedColor;
    
    [SerializeField] TextMeshProUGUI manatext, hptext, shieldtext;
    [SerializeField] Button moveButton;
    [SerializeField] TextMeshProUGUI moveButtonText;

    void Awake(){
        mana = 3;
        movementmana = 1;
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
                    GameManager.Instance.CardManager.PickCard();
                    EndPlayerTurn();
                    break;
                //move 버튼을 눌렀을 시, 움직일 범위를 선택 할 수 있음
                case State.pickMoveState :
                    MoveButton();
                    break;
                //카드를 고른 후 범위를 선택, 몇몇 카드는 자동으로 생략됨
                case State.pickCardState :
                    GameManager.Instance.CardManager.PlayCard();
                    break;
                //카드 애니메이션 재생
                case State.playCardState :
                    break;
                //턴 종료, 요그사론 공격등이 나감
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
            

            //Mana UI
            manatext.text = "Mana : " + mana + "/3";
            hptext.text = "Health : " + stage.player.health + "/" + stage.player.maxHealth;
            shieldtext.text = "Shield : " + stage.player.shield;

            moveButtonText.text = "Move\nneed " + movementmana +" mana";
            if(mana < movementmana) moveButton.interactable = false;
            else moveButton.interactable = true;
        }
    }

    //스테이지 배틀 시작
    public void StartBattle(Stage s){
        isBattle = true;
        state = State.startBattleState;
        stage = s;
    }

    //Player Turn End
    public void EndPlayerTurn(){
        if(Input.GetKeyDown(KeyCode.Z)){
            state = State.EnemyAttackState;
            StartCoroutine(_EnemyAttackStart(stage));
            //나중에 EndTurnState로 바꾸고 턴종료시에 하는 일들을 해야 됨

        }
    }

    //Player Call Yogg
    public void EndPlayerTurnWithYogg(){
        if(Input.GetKeyDown(KeyCode.X)){
            state = State.YoggCallingState;
            
        }
    }

    //Enemy Turn End
    public void EndEnemyTurn(){
        state = State.drawCardState;
        mana = 3;
        stage.player.shield = 0;
    }

    //Player Dead
    public void EndStagePlayerDead(){
        Debug.Log("Player dead");
    }

    //Enemy All Dead
    public void EndStageEnemyDead(){
        Debug.Log("Enemy All Dead");
    }
    
    //모든 Enemy가 Attack하게 함
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
    
    //모든 Enemy가 Move하게 함
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

    //Enemy가 죽게 함
    public void RemoveEnemy(Enemy enemy){
        stage.enemies.Remove(enemy);
        if(stage.enemies.Count == 0){
            EndStageEnemyDead();
        }
    }

    //MoveButton이 눌렸을 시 호출, 갈 수 있는 위치를 표시
    public void ClickMoveButton(){
        if (mana >= movementmana && state == State.selectBehaviourState){
            foreach(Vector2 pos in Gridlib.CanReach(stage, stage.player.position, 1)){
                stage.tiles[pos].Highlight();
            }
            state = State.pickMoveState;
        }
    }

    //누른 위치로 player를 이동시킴
    public void MoveButton(){
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);    
            if(hit.collider != null && hit.collider.TryGetComponent<Tile>(out Tile tile)){
                if(Gridlib.CanReach(stage, stage.player.position, 1).Contains(tile.position)){ // 이동속도로 바꿀수도 있음
                    stage.player.Move(tile);
                    mana -= movementmana;
                    movementmana++;
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

    //카드를 Select했는지 알려줌, select가 true일시 카드를 select한 상태
    public void Select(bool select){
        if(select){
            state = State.pickCardState;
        }
        else{
            state = State.selectBehaviourState;
        }
    }

    //Tile을 highlight를 active 상태로 만듦 tilePositions가 null일시 모든 highlight를 끔
    public void EmphasizeTile( bool active, List<Vector2> tilePositions = null){
        if(tilePositions == null){
            if(active == false){
                foreach (Tile t in stage.tiles.Values) {
                    t.targetTile(active);
                }
            }
        }
        else{
            foreach (Vector2 v in tilePositions) {
                stage.tiles[v].targetTile(active);
            }
        }
    }

    public void InitializeMovementMana(){
        movementmana = 1;
    }

    public Stage GetStage(){
        return stage;
    }

    public void EndEnemyAct(){
        enemyActing = false;
    }
    
}
