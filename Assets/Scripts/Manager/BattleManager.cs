using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


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
        EndTurnEffectState,
        EnemyAttackState,
        EnemyMoveState,
    }

    bool isBattle;//battle이 시작되었는지
    bool enemyActing;//enmey가 행동중
    bool switched;//state가 바뀌었는지, 스테이트가 바뀌고 한번만 호출 되야 하는 것들이 있어서 만듦, 별로 좋은 방법은 아닌거 같아서 바꾸고 싶긴 함. 
    public int mana {get;set;} //마나
    int maxmana; //맥스 마나
    int movementmana; // 이동하는데 필요한 마나, 덱을 섞으면 1로 초기화 됨
    
    State state;
    Stage stage;
    
    public DamageText damageText;

    [SerializeField] Collider2D moveButton, turnEndButton, yoggCallingButton;
    [SerializeField] TextMeshPro moveButtonText, turnEndButtonText, yoggCallingButtonText;
    

    [SerializeField] TextMeshProUGUI manatext, hptext, shieldtext;

    
    //스테이지 배틀 시작
    public void StartBattle(Stage s){
        isBattle = true;
        SwitchState(State.startBattleState);
        stage = s;
        maxmana = 3;
        mana = maxmana;
        movementmana = 1;
        enemyActing = false;
        
        moveButton = GameManager.Instance.GridManager.GetButtonList()[0].GetComponent<Collider2D>();
        turnEndButton = GameManager.Instance.GridManager.GetButtonList()[1].GetComponent<Collider2D>();
        yoggCallingButton = GameManager.Instance.GridManager.GetButtonList()[2].GetComponent<Collider2D>();
        moveButtonText = moveButton.GetComponentInChildren<TextMeshPro>();
        turnEndButtonText = turnEndButton.GetComponentInChildren<TextMeshPro>();
        yoggCallingButtonText = yoggCallingButton.GetComponentInChildren<TextMeshPro>();
    }

    void Update(){
        if(isBattle){
            switch (state){
                //Battle이 시작하는 state, 바로 MoveState로 간다
                case State.startBattleState :
                    GameManager.Instance.GridManager.
                    StartCoroutine(_EnemyMoveStart(stage));
                    SwitchState(State.EnemyMoveState);
                    break;
                //드로우 하는 state
                case State.drawCardState :
                    for(int i = 0 ; i < 5; i++){
                        GameManager.Instance.CardManager.DrawCard();
                    }
                    SwitchState(State.selectBehaviourState);
                    break;
                //카드를 고르거나 move 버튼을 누를 수 있음, 혹은 턴 엔드
                case State.selectBehaviourState :
                    GameManager.Instance.CardManager.PickCard();
                    ClickButton();
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
                //요그사론 호출 버튼을 누름
                case State.YoggCallingState :
                    GameManager.Instance.CardManager.DiscardAllCard();
                    if(switched){
                        switched = false;
                        StartCoroutine(GameManager.Instance.CardManager.PlayYogg());
                    }
                    break;
                //턴 종료 버튼을 누름
                case State.EndTurnState :
                    GameManager.Instance.CardManager.DiscardAllCard();
                    SwitchState(State.EndTurnEffectState);
                    break;
                //턴 종료시 일어나는 일들을 처리함
                case State.EndTurnEffectState :
                    SwitchState(State.EnemyAttackState);
                    break;
                //상대 턴 공격 상태, 지정된 범위를 공격함
                case State.EnemyAttackState :
                    if(switched){
                        switched = false;
                        StartCoroutine(_EnemyAttackStart(stage));
                    }
                    break;
                //상대 턴 이동 상태, 적절한 곳으로 이동하고, 공격을 예고함
                case State.EnemyMoveState :
                    break;
                default :
                    break;
            }
            

            //Mana UI
            manatext.text = "Mana : " + mana + "/" + maxmana;
            hptext.text = "Health : " + stage.player.health + "/" + stage.player.maxHealth;
            shieldtext.text = "Shield : " + stage.player.shield;

            //moveButton
            moveButtonText.text = "Move\nneed " + movementmana +" mana";

            //debug 용도
            if(Input.GetKeyDown(KeyCode.C)){
                GameManager.Instance.saver.player.SynchroHP(stage.player);
                SceneManager.LoadScene("MapScene");
            }
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                mana += 1;
            }
        }
    }

    //YoggEnd()
    public void YoggEnd(){
        SwitchState(State.EndTurnEffectState);
        //나중에 EndTurnState로 바꿀것
    }

    //Enemy Turn End
    public void EndEnemyTurn(){
        SwitchState(State.drawCardState);
        mana = maxmana;
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
        SwitchState(State.EnemyMoveState);
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
        SwitchState(State.drawCardState);
    }

    //Enemy가 죽게 함
    public void RemoveEnemy(Enemy enemy){
        stage.enemies.Remove(enemy);
        if(stage.enemies.Count == 0){
            EndStageEnemyDead();
        }
    }

    //Button이 눌렸을 시의 동작
    public void ClickButton(){
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
            if(hit.collider == moveButton  && mana >= movementmana) {
                foreach(Vector2 pos in Gridlib.CanReach(stage, stage.player.position, 1)){
                    stage.tiles[pos].Highlight();
                }
                SwitchState(State.pickMoveState);
            }
            else if(hit.collider == turnEndButton){
                SwitchState(State.EndTurnState);
            }
            else if(hit.collider == yoggCallingButton){
                SwitchState(State.YoggCallingState);
            }
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
                SwitchState(State.selectBehaviourState);
            }
            else {
                SwitchState(State.selectBehaviourState);
            }
        }
        else if (Input.GetMouseButtonDown(1)) {
            SwitchState(State.selectBehaviourState);
        }
    }

    //카드를 Select했는지 알려줌, select가 true일시 카드를 select한 상태
    public void Select(bool select){
        if(select){
            SwitchState(State.pickCardState);
        }
        else{
            SwitchState(State.selectBehaviourState);
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

    public void SpendMana(int i){
        mana -= i;
    }

    public void GainMana(int i){
        mana += i;
    }

    //다음턴에 마나를 얻음 TODO
    public void NextTurnGainMana(int i){

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

    void SwitchState(State s){
        state = s;
        switched = true;    
    }
    
}
