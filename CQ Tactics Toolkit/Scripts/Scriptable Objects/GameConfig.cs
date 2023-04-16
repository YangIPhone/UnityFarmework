using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CQFramework.CQTacticsToolkit
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "CQ Tactics Toolkit/Config/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        [Header("移动范围颜色")]
        public Color MoveRangeColor;
        [Header("攻击范围颜色")]
        public Color AttackRangeColor;
        [Header("使用范围颜色")]
        public Color UseRangeColor;
        [Header("障碍物颜色")]
        public Color BlockedTileColor;
        [Header("角色经验计算模式")]
        public XPScalingMode xpScalingMode;
        [Header("每级升级所需经验增加值")]
        public int LevelIncreaseAmount;
        [Header("经验曲线")]
        public AnimationCurve expCurve;

        [Header("最大等级")]
        public int MaxLevel=11;
        [Header("最大经验值")]
        public int MaxRequiredExp;
        [Header("速度加成移动范围")]
        public float SpeedIncreaseMoveRange = 150f;
        [Header("每回合回复行动点数量")]
        public int AugmentActionPointCount = 3;
        public int GetRequiredExp(int level)
        {
            int requiredExperience = 0;
            float xp;
            switch (xpScalingMode)
            {
                case XPScalingMode.Constant:
                    xp = (LevelIncreaseAmount * level);
                    requiredExperience = Mathf.CeilToInt(xp);
                    break;
                case XPScalingMode.Disgaea:
                    requiredExperience = Mathf.RoundToInt(0.04f * Mathf.Pow(level, 3) + 0.8f * Mathf.Pow(level, 2) + 2 * level);
                    break;
                case XPScalingMode.AnimationCurve:
                    requiredExperience = Mathf.RoundToInt(expCurve.Evaluate(Mathf.InverseLerp(0, MaxLevel, level)) * MaxRequiredExp);
                    break;

                default:
                    break;
            }

            return requiredExperience;
        }
    }
}
