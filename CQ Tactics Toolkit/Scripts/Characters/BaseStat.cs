using UnityEngine;
using System;

namespace CQTacticsToolkit{
    //一个用于角色属性升级的统计对象。
    [Serializable]
    public class BaseStat
    {
        [SerializeField]
        public int baseStatValue;

        [SerializeField]
        public AnimationCurve baseStatModifier = new AnimationCurve();
    }
}
