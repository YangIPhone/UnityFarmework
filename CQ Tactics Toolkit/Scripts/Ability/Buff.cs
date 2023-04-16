using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit
{
    //附加到属性上的效果。可能是一个buff或debuff。
    public class Buff
    {
        /// <summary>
        /// 影响属性
        /// </summary>
        public Stats attributeName; 
        public int value;
        public float duration;
        public Operation Operator;
        public bool isActive;
        public string statModName;//附加BUFF的名字

        /// <summary>
        /// 附加到属性上的效果。可能是一个buff或debuff。
        /// </summary>
        /// <param name="attribute">影响属性</param>
        /// <param name="value">值</param>
        /// <param name="duration">持续回合</param>
        /// <param name="op">影响方式</param>
        /// <param name="statModName">附加BUFF的名字</param>
        public Buff(Stats attribute, int value, float duration, Operation op, string statModName)
        {
            this.attributeName = attribute;
            this.value = value;
            this.duration = duration;
            this.Operator = op;
            this.statModName = statModName;
            isActive = true;
        }
    }
}
