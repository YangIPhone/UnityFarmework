using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQFramework.CQTacticsToolkit {
    [System.Serializable]
    public class BuffEffectContainer
    {
        /// <summary>
        /// BUFF的ID
        /// </summary>
        public int BuffID;
        /// <summary>
        /// BUFF名称
        /// </summary>
        public string BuffName;
        /// <summary>
        /// 剩余持续回合
        /// </summary>
        public int Duration = 0;

        /// <summary>
        /// 使用的BUFF数据
        /// </summary>
        public BuffEffect buffEffect;
        /// <summary>
        /// 初始化一个BUFF
        /// </summary>
        /// <param name="buffEffect"></param>
        /// <param name="BuffID"></param>
        /// <param name="BuffName"></param>
        /// <param name="Duration"></param>
        public BuffEffectContainer(BuffEffect buffEffect,int BuffID,string BuffName, int Duration)
        {
            this.buffEffect = buffEffect;
            this.BuffID = BuffID;
            this.BuffName = BuffName;
            this.Duration = Duration;
        }

        /// <summary>
        /// 增减BUFF持续回合
        /// </summary>
        /// <param name="Duration">增减的回合</param>
        /// <returns>剩余持续回合</returns>
        public int DurationLengthen(int Duration){
            this.Duration += Duration;
            return this.Duration;
        }

    }
}
