using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit{
    [CreateAssetMenu(fileName = "Ability", menuName = "CQ Tactics Toolkit/Ability", order = 0)]
    public class Ability : ScriptableObject {
        [Header("技能ID")]
        public int AbilityID;
        //==================
        [Header("技能名")]
        public string AbilityName;
        //==================
        [Header("技能图标")]
        public Sprite Icon;
        //==================
        [Header("技能描述")]
        [TextArea]
        public string description;
        //==================
        [Header("技能消耗点数")]
        public int costPoint = 1;
        //==================
        [Header("技能消耗灵力")]
        public int cost = 10;
        //==================
        [Header("技能基础伤害")]
        public int value = 100;
        //==================
        [Header("技能释放范围")]
        public int range = 1;
        //==================
        [Header("技能有效范围")]
        public TextAsset abilityShape;
        //==================
        [Header("技能冷却回合")]
        public int cooldown = 1;
        //==================
        [Header("技能有效类型")]
        public AbilityTypes abilityType = AbilityTypes.Enemy;
        //==================
        [Header("技能是否包含释放点")]
        public bool includeOrigin;
        //==================
        [Header("技能需要的等级")]
        public int requiredLevel;
        //==================
        [Header("技能影响的高度")]
        public int abilityHeight = 2;
        //==================
        [Header("技能粒子效果")]
        public GameObject effectObj;
        //==================
        [Header("技能附加效果")]
        public List<BuffEffect> effects;
        //==================
        // [Header("增强Action范围的属性")]
        // public 范围增强属性 范围增强属性 = 范围增强属性.神识;
        //TODO damage types
    }
}
