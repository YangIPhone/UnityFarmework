using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit{
    /// <summary>
    /// 瓦片类型
    /// </summary>
    public enum TileTypes
    {
        Traversable,
        NonTraversable,
        Effect
    }
    /// <summary>
    /// 阵营队伍
    /// </summary>
    public enum Team
    {
        NPC,
        Player,
        Enemy
    }
    /// <summary>
    /// 人物基础属性
    /// </summary>
    public enum Stats
    {
        CurrentHealth,//当前生命值
        MaxHealth,//最大生命值
        CurrentMana,//当前灵力值
        MaxMana,//最大灵力值
        Strenght,//攻击力
        Endurance,//防御力
        Speed,//速度
        CurrentActionPoint,//当前行动点
        MaxActionPoint,//最大行动点
        AttackRange,//攻击范围
        MoveRange,//移动范围
        Level,//境界
        attribute//灵根
    }

    /// <summary>
    /// 计算方式
    /// </summary>
    public enum Operation
    {
        Add,//加
        Minus,//减
        Multiply,//乘
        Divide,//除
        AddByPercentage,//加百分比
        MinusByPercentage//乘百分比
        // 加,减,乘,除,加百分比,乘百分比
    }
    /// <summary>
    /// 人物境界(等级)
    /// </summary>
    public enum CharacterLevel
    {
        未修炼,锻体, 炼气, 筑基, 金丹,元婴,化神,炼虚,合道,渡劫,大乘
    }
    /// <summary>
    /// 人物灵根(属性)
    /// </summary>
    public enum CharacterAttribute
    {
        金灵根, 木灵根, 水灵根, 火灵根, 土灵根, 冰灵根, 雷灵根, 风灵根
    }

    //瓦片颜色
    public enum TileColors
    {
        MovementColor,
        AttackRangeColor,
        UseRangeColor,
        AttackColor
    }
    
    //经验增加方式
    public enum XPScalingMode
    {
        Constant,
        Disgaea,
        AnimationCurve
    }
    
    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6,
        TopLeft = 7,
        BottomLeft = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }

    //回合管理的排序模式
    public enum TurnSorting
    {
        ConstantAttribute,
        CTB
    };
    /// <summary>
    /// 技能类型
    /// </summary>
    public enum AbilityTypes
    {
        Ally,//盟友
        Enemy,//敌人
        All//所有人
    }
    /// <summary>
    /// 战斗AI行动策略
    /// </summary>
    public enum Personality
    {
        Aggressive,//积极进攻,在尽可能靠近的同时攻击最近的角色。
        Defensive,//保守，攻击距离最近的角色，同时保持最大距离。
        Strategic//策略，攻击生命值最低的角色，同时保持最大距离
    }
}
