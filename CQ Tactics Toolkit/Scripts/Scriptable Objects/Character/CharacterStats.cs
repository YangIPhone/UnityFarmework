using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit
{
    //生成包含角色类别和等级的角色属性。
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "CQ Tactics Toolkit/Character/CharacterStats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        public Stat Health;
        public Stat Mana;
        public Stat Strenght;
        public Stat Endurance;
        public Stat Speed;
        public Stat ActionPoint;
        public Stat CurrentActionPoint;
        public Stat Level;
        public Stat MoveRange;
        public Stat AttackRange;
        public Stat CurrentHealth;
        public Stat CurrentMana;

        public Stat getStat(Stats statKey)
        {
            var fields = typeof(CharacterStats).GetFields();

            foreach (var item in fields)
            {
                var type = item.FieldType;
                Stat value = (Stat)item.GetValue(this);

                if (value.statKey == statKey)
                    return value;
            }

            return null;
        }
    }
}
