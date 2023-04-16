using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQFramework.CQTacticsToolkit
{
    /// <summary>
    /// BuffEffects 是附加到瓦片或技能上的buff. 
    /// </summary>
    [CreateAssetMenu(fileName = "BuffEffect", menuName = "CQ Tactics Toolkit/BuffEffect", order = 0)]
    public class BuffEffect : ScriptableObject {
        [Header("BUFF ID")]
        public int BuffID;
        [Header("BUFF名字")]
        public string BuffName;
        [Header("BUFF图标")]
        public Sprite BuffIcon;
        [Header("BUFF描述")]
        [TextArea]
        public string BuffDescription;
        [Header("影响的属性")]
        public Stats StatKey;
        [Header("影响方式(加减乘除)")]
        public Operation Operator;
        [Header("持续回合")]
        public int Duration;
        [Header("增减的值&百分比")]
        public int Value;

        /// <summary>
        /// 应用BUFF
        /// </summary>
        /// <param name="characterClass"></param>
        public void ApplyBuff(Character character)
        {
            if(StatKey == Stats.Level || Stats.attribute == StatKey) return;
            int statValue = character.characterClass.GetAttribute<int>(StatKey.ToString());
            switch (Operator)
            {
                //增加
                case Operation.Add:
                    statValue = Mathf.CeilToInt(statValue + Value);
                    break;
                //减少
                case Operation.Minus:
                    if (StatKey == Stats.CurrentHealth)
                    {
                        character.TakeDamage(Mathf.CeilToInt(Value),true);
                        return;
                    }
                    else
                    {
                        statValue = Mathf.CeilToInt(statValue - Value);
                    }
                    break;
                //乘
                case Operation.Multiply:
                    statValue = Mathf.CeilToInt(statValue * Value);
                    break;
                //除
                case Operation.Divide:
                    statValue = Mathf.CeilToInt((float)statValue / (float)Value);
                    break;
                //百分比增加
                case Operation.AddByPercentage:
                    statValue = Mathf.CeilToInt(statValue * (1 + Value / 100f));
                    break;
                case Operation.MinusByPercentage:
                    if (StatKey == Stats.CurrentHealth)
                    {
                        float percentageDifference = (float)(Value / 100f) * (float)character.characterClass.CurrentHealth;
                        character.TakeDamage(Mathf.CeilToInt(percentageDifference), true);
                        return;
                    }
                    else
                    {
                        statValue = Mathf.CeilToInt(statValue * (1 - Value / 100f));
                    }
                    break;
            }
            character.characterClass.SetAttribute(StatKey.ToString(),statValue);
        }
    }
}
