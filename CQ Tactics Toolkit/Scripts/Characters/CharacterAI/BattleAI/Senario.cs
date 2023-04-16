using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQFramework.CQTacticsToolkit
{
    /// <summary>
    /// 用于敌人AI。一个Senario是敌人可以采取的策略。
    /// </summary>
    public class Senario
    {
        public float senarioValue;
        public AbilityContainer targetAbility;
        public OverlayTile targetTile;
        public OverlayTile positionTile;
        public bool useAutoAttack;

        /// <summary>
        /// 创建一个敌人可能采取的行动
        /// </summary>
        /// <param name="senarioValue">行动值</param>
        /// <param name="targetAbility">行动使用的技能</param>
        /// <param name="targetTile">行动的目标瓦片</param>
        /// <param name="positionTile">行动位置</param>
        /// <param name="useAutoAttack">是否启用自动攻击</param>
        public Senario(float senarioValue, AbilityContainer targetAbility, OverlayTile targetTile, OverlayTile positionTile, bool useAutoAttack)
        {
            this.senarioValue = senarioValue;
            this.targetAbility = targetAbility;
            this.targetTile = targetTile;
            this.positionTile = positionTile;
            this.useAutoAttack = useAutoAttack;
        }

        public Senario()
        {
            this.senarioValue = -10000;
            this.targetAbility = null;
            this.targetTile = null;
            this.positionTile = null;
            this.useAutoAttack = false;
        }
    }
}
