using System;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit
{
    //实体的属性
    [Serializable]
    public class Stat
    {
        public Stats statKey;
        public Character character;
        //BaseStatValue是最大值。
        public int baseStatValue;

        //StatValue是BaseStatValue通过效果/伤害改变后的值
        public int statValue;
        public bool isModified;
        public List<Buff> statMods;

        public Stat(Stats statKey, int statValue, Character character)
        {
            this.character = character;
            this.statValue = statValue;
            this.statKey = statKey;

            baseStatValue = statValue;
            statMods = new List<Buff>();
            isModified = false;
        }

        //更新一个属性值
        public void ChangeStatValue(int newValue)
        {
            statValue = newValue;
            baseStatValue = newValue;
        }

        //应用BUFF并改变属性的值。
        public void ApplyStatMods()
        {
            foreach (var statMod in statMods)
            {
                if (statMod != null)
                {
                    switch (statMod.Operator)
                    {
                        case Operation.Add:
                            statValue = Mathf.CeilToInt(statValue + statMod.value);
                            break;
                        case Operation.Minus:
                            if (statKey == Stats.CurrentHealth)
                            {
                                character.TakeDamage(Mathf.CeilToInt(statMod.value));
                            }
                            else
                            {
                                statValue = Mathf.CeilToInt(statValue - statMod.value);
                            }
                            break;
                        case Operation.Multiply:
                            statValue = Mathf.CeilToInt(statValue * statMod.value);
                            break;
                        case Operation.Divide:
                            statValue = Mathf.CeilToInt(statValue / statMod.value);
                            break;
                        case Operation.AddByPercentage:
                            statValue = Mathf.CeilToInt(statValue * (1 + statMod.value / 100));
                            break;
                        case Operation.MinusByPercentage:
                            if (statKey == Stats.CurrentHealth)
                            {
                                float percentageDifference = (float)(statMod.value / 100f) * (float)baseStatValue;
                                character.TakeDamage(Mathf.CeilToInt(percentageDifference), true);
                            }
                            else
                            {
                                statValue = Mathf.CeilToInt(statValue * (1 - statMod.value / 100));
                            }
                            break;
                    }

                    statMod.duration--;
                }
            }

            statMods.RemoveAll(x => x.duration <= 0);

            if (statMods.Count == 0)
                isModified = false;
        }
    }
}

