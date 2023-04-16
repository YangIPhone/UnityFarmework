using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using CQFramework.Utils;

namespace CQTacticsToolkit{
    public class CharacterClass : MonoBehaviour
    {
        [Header("人物ID")]
        [SerializeField] public int characterID;
        [Header("人物名字")]
        [SerializeField]public string characterName;
        [Header("人物头像")]
        [SerializeField]public Sprite characterHeadSprite;
        [Header("生命值")]
        [SerializeField]public int CurrentHealth=100;//当前生命值
        [SerializeField]public int MaxHealth=100;//最大生命值
        [Header("灵力值")]
        [SerializeField]public int CurrentMana=10;//当前灵力值
        [SerializeField]public int MaxMana=10;//最大灵力值
        [Header("攻击力")]
        [SerializeField]public int Strenght=100;//攻击力
        [Header("防御力")]
        [SerializeField]public int Endurance=50;//防御力
        [Header("速度")]
        [SerializeField]public int Speed=10;//速度
        [Header("行动点")]
        [SerializeField]public int CurrentActionPoint=3;//当前行动点
        [SerializeField]public int MaxActionPoint=3;//最大行动点
        [Header("攻击范围")]
        [SerializeField]public int AttackRange = 1;
        public int MoveRange = 3;
        // [SerializeField]public int MoveRange = 3;//移动范围
        [Header("境界")]
        [SerializeField] public CharacterLevel level = CharacterLevel.未修炼;
        [Header("灵根")]
        [SerializeField] public CharacterAttribute attribute;
        [Header("技能列表")]
        public List<Ability> abilities = new List<Ability>();
        [Header("人物属性成长曲线")]
        public CharacterGrow characterGrow;
        private void Awake() {
            // BaseStat h = GetStat<BaseStat>("Health");
            // Debug.Log(h.baseStatValue);
            // SetAttribute<int>("MoveRange",5);
        }

        /// <summary>
        /// 获取人物指定属性值
        /// </summary>
        /// <param name="field">要获取的字段</param>
        /// <typeparam name="T">字段的类型</typeparam>
        /// <returns></returns>
        public T GetAttribute<T>(string field)
        {
           return CQUtilsClass.GetFields<CharacterClass, T>(this, field);
        }

        /// <summary>
        /// 设置人物指定属性
        /// </summary>
        /// <param name="field">要设置的字段</param>
        /// <param name="value">要设置的字段值</param>
        public void SetAttribute(string field,System.Object value)
        {
            //如果设置的是属性的当前值，则保证属性的当前值不超过最大值
            if(field.Contains("Current")){
                string maxField = $"Max{field.Split("Current")[1]}";
                int FieldMaxValue =(int)GetAttribute<System.Object>(maxField);
                value = Mathf.Min((int)value, FieldMaxValue);
                value = Mathf.Max((int)value, 0);
            }
            //如果修改的是最大生命或者灵力，则当前生命或灵力保持比例
            if (field=="MaxHealth"||field == "MaxMana")
            {
                string CurrentField = $"Current{field.Split("Max")[1]}";
                int FieldCurrentValue = (int)GetAttribute<System.Object>(CurrentField);
                // FieldCurrentValue = Mathf.Min((int)value, FieldCurrentValue);
                int FieldCurrentMaxValue = (int)GetAttribute<System.Object>(field);
                float percentage = FieldCurrentValue/(float)FieldCurrentMaxValue;
                CQUtilsClass.SetFields(this, CurrentField, Mathf.CeilToInt(((int)value) * percentage));
                value = Mathf.Max((int)value, 1);
            }
            CQUtilsClass.SetFields(this,field,value);
        }

        /// <summary>
        /// 增减人物指定属性
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        public void UpdateAttribute(string field, int value)
        {
            int newValue = GetAttribute<int>(field) + value;
            SetAttribute(field,newValue);
        }

        /// <summary>
        /// 获取人物移动范围
        /// </summary>
        /// <returns></returns>
        public int GetMoveRange(){
            return Mathf.FloorToInt(Speed/OverlayController.Instance.gameConfig.SpeedIncreaseMoveRange) + 3;
        }

        /// <summary>
        /// 添加一个技能
        /// </summary>
        /// <param name="ability">要添加的技能</param>
        public void AddAbility(Ability ability)
        {
            if(abilities.FindIndex(x=>x.AbilityID==ability.AbilityID && x.AbilityName == ability.AbilityName)>-1)
            {
                Debug.Log("已存在相同技能");
                return;
            }
            abilities.Add(ability);
        }

        /// <summary>
        /// 移除一个技能
        /// </summary>
        /// <param name="ability">要移除的技能</param>
        public void RemoveAbility(Ability ability)
        {
            int abilityIndex = abilities.FindIndex(x => x.AbilityID == ability.AbilityID && x.AbilityName == ability.AbilityName);
            if (abilityIndex > -1)
            {
                abilities.RemoveAt(abilityIndex);
            }else{
                Debug.Log("不存在此技能");
            }
        }
    }
}
