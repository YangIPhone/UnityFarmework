using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQFramework.CQTacticsToolkit
{
    public class BattleFlee : MonoBehaviour
    {
        public void Flee()
        {
            BattleTrigger battleTrigger = TurnBasedController.Instance.GetBattleTrigger();
            if(battleTrigger.canFlee){
                //TODO 随机逃跑几率
                TurnBasedController.Instance.EndBattle(BattleResult.Flee);
            }
        }
    }
}
