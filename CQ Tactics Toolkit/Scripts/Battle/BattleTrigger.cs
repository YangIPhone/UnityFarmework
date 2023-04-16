using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CQFramework.CQTacticsToolkit
{  
    public class BattleTrigger : MonoBehaviour
    {
        [Header("战斗触发方式")]
        public BattleTriggerType triggerType = BattleTriggerType.触发器;
        [Header("战斗触发器")]
        public BoxCollider2D trigger;
        [Header("胜利条件")]
        public VictoryCondition victory = VictoryCondition.AllEnemyDie;
        [Header("失败条件")]
        public LoseCondition lose = LoseCondition.AllPlayerDie;
        [Header("战斗是否可逃跑")]
        public bool canFlee = true;
        [Header("战斗是否可中途加入")]
        public bool canJoin = false;
        [Header("进入战斗的怪物")]
        public List<Character> EnemyTeam;
        [Header("战斗胜利事件")]
        public UnityEvent VictoryEvnet;
        [Header("战斗失败事件")]
        public UnityEvent LoseEvnet;
        private WaitForSecondsRealtime waitTime = new WaitForSecondsRealtime(2f);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")&&triggerType == BattleTriggerType.触发器)
            {
                EnterBatter();
                trigger.enabled = false;
            }
        }

        public void EnterBatter()
        {
            BattleTrigger battleTrigger = TurnBasedController.Instance.GetBattleTrigger();
            //如果回合管理器没有正在进行的战斗，或者正在进行的战斗运行中途加入才可以进入战斗
            if(battleTrigger==null||battleTrigger.canJoin){
                // TryGetComponent<Character>(out Character character);
                // if(character!=null)EnemyTeam.Add(character);
                EventHandler.CallEnterBattle(this);
            }
        }
        //战斗结束，根据战斗结果处理
        public void OnEndBattle(BattleResult battleResult){
            if(battleResult==BattleResult.Victory){
                // Debug.Log("战斗胜利");
                VictoryEvnet?.Invoke();
            }else if(battleResult == BattleResult.Lose){
                // Debug.Log("战斗失败");
                LoseEvnet?.Invoke();
            }else if (battleResult == BattleResult.Flee)
            {
                if(triggerType == BattleTriggerType.触发器){
                    // Debug.Log("触发器战斗结束");
                    StartCoroutine(OnEnableTrigger());
                }
                //TODO 其他战斗结果处理
            }
        }

        private IEnumerator OnEnableTrigger()
        {
            yield return waitTime;
            trigger.enabled = true;
        }

    }
}
