using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CQFramework.CQTacticsToolkit;

namespace CQFramework
{
    public static class EventHandler
    {
        /// <summary>
        /// 更新小时与分钟
        /// </summary>
        // public static event Action<int, int> GameMinuteEvent;
        // public static void CallGameMinuteEvent(int minute, int hour)
        // {
        //     GameMinuteEvent?.Invoke(minute, hour);
        // }

        // /// <summary>
        // /// 更新天数
        // /// </summary>
        // public static event Action<int, Season> GameDayEvent;
        // public static void CallGameDayEvent(int day, Season season)
        // {
        //     GameDayEvent?.Invoke(day, season);
        // }

        // /// <summary>
        // /// 更新年月日季节
        // /// </summary>
        // public static event Action<int, int, int, int, Season> GameDateEvent;
        // public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
        // {
        //     GameDateEvent?.Invoke(hour, day, month, year, season);
        // }
        
        /// <summary>
        /// 地图初始化完成
        /// </summary>
        public static event Action MapInited;
        public static void CallMapInited()
        {
            MapInited?.Invoke();
        }
        /// <
        /// <summary>
        /// 转换到场景位置
        /// </summary>
        public static event Action<string, Vector3> TransitionEvent;
        public static void CallTransitionEvent(string sceneName, Vector3 pos)
        {
            TransitionEvent?.Invoke(sceneName, pos);
        }

        /// <summary>     
        /// 卸载场景之前的事件 
        /// </summary>     
        public static event Action BeforSceneUnloadEvent;
        public static void CallBeforSceneUnloadEvent()
        {
            BeforSceneUnloadEvent?.Invoke();
        }
        /// <summary>     
        /// 加载场景之后的事件    
        /// </summary>     
        public static event Action AfterSceneLoadedEvent;
        public static void CallAfterSceneLoadedEvent()
        {
            AfterSceneLoadedEvent?.Invoke();
        }

        /// <summary>    
        /// 转换到场景位置    
        /// </summary>     
        public static event Action<Vector3> PlayerMoveToPos;
        public static void CallPlayerMoveToPos(Vector3 pos)
        {
            PlayerMoveToPos?.Invoke(pos);
        }

        /// <summary>
        /// 鼠标指向的瓦片变化
        /// </summary>
        public static event Action<OverlayTile> FocusOnNewTile;
        public static void CallFocusOnNewTile(OverlayTile focusedOnTile)
        {
            FocusOnNewTile?.Invoke(focusedOnTile);
        }

        /// <summary>
        /// 进入战斗
        /// </summary>
        public static event Action<BattleTrigger> EnterBattle;
        public static void CallEnterBattle(BattleTrigger battleTrigger)
        {
            EnterBattle?.Invoke(battleTrigger);
        }

        /// <summary>
        /// 结束战斗
        /// </summary>
        public static event Action<BattleResult> EndBattle;
        public static void CallEndBattle(BattleResult battleResult)
        {
            EndBattle?.Invoke(battleResult);
        }

        // /// <summary>
        // /// 玩家进入移动模式
        // /// </summary>
        // public static event Action EnterMovementMode;
        // public static void CallEnterMovementMode()
        // {
        //     EnterMovementMode?.Invoke();
        // }
        
        /// <summary>
        /// 玩家释放技能
        /// </summary>
        public static event Action<string,int> CastAbility;
        public static void CallCastAbility(string abilityName,int abilityID)
        {
            CastAbility?.Invoke(abilityName,abilityID);
        }

        /// <summary>
        /// AI释放技能
        /// </summary>
        public static event Action<List<OverlayTile>, AbilityContainer,Character> AbilityCommand;
        public static void CallAbilityCommand(List<OverlayTile> TileList, AbilityContainer abilityContainer,Character AbilityUser)
        {
            AbilityCommand?.Invoke(TileList, abilityContainer,AbilityUser);
        }
        /// <summary>
        /// 角色结束回合
        /// </summary>
        public static event Action EndTurn;
        public static void CallEndTurn()
        {
            EndTurn?.Invoke();
        }

        /// <summary>
        /// 开始新角色回合
        /// </summary>
        public static event Action<Character> StartNewCharacterTurn;
        public static void CallStartNewCharacterTurn(Character newCharacter)
        {
            StartNewCharacterTurn?.Invoke(newCharacter);
        }
        
        public static event Action<string> LogAction;
        public static void CallLogAction(string log)
        {
            LogAction?.Invoke(log);
        }
        /// <summary>
        /// 更新血条
        /// </summary>
        public static event Action<float> UpdateHealthBar;
        public static void CallUpdateHealthBar(float fillAmount)
        {
            UpdateHealthBar?.Invoke(fillAmount);
        }

        /// <summary>
        /// 更新蓝条
        /// </summary>
        public static event Action<float> UpdateManaBar;
        public static void CallUpdateManaBar(float fillAmount)
        {
            UpdateManaBar?.Invoke(fillAmount);
        }

        /// <summary>
        /// 更新行动点数量
        /// </summary>
        public static event Action<int> UpdateActionPoint;
        public static void CallUpdateActionPoint(int ActionPointCount)
        {
            UpdateActionPoint?.Invoke(ActionPointCount);
        }
        /// <summary>
        /// 更新行动队列UI
        /// </summary>
        public static event Action<List<Character>> TurnOrderUpdated;
        public static void CallTurnOrderUpdated(List<Character> CharacterTurnOrder)
        {
            TurnOrderUpdated?.Invoke(CharacterTurnOrder);
        }

    }
}
