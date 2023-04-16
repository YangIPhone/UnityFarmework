using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CQFramework;

namespace CQFramework.CQTacticsToolkit
{

    public class SetActiveCharacter : MonoBehaviour
    {
        [SerializeField]private ActionSlot ActionSlot;
        [SerializeField] private Image CharactorSprite;
        [SerializeField] private Image HealthBar;
        [SerializeField] private Image ManaBar;
        [SerializeField] private RectTransform ActionContainer;
        private void OnEnable()
        {
            EventHandler.EnterBattle += OnEnterBattle;
            EventHandler.EndBattle += OnEndBattle;
            EventHandler.UpdateHealthBar += OnUpdateHealthBar;
            EventHandler.UpdateManaBar += OnUpdateManaBar;
            EventHandler.UpdateActionPoint += OnUpdateActionPoint;
            EventHandler.StartNewCharacterTurn += OnStartNewCharacterTurn;
        }

        private void OnDisable()
        {
            EventHandler.EnterBattle -= OnEnterBattle;
            EventHandler.EndBattle -= OnEndBattle;
            EventHandler.UpdateHealthBar -= OnUpdateHealthBar;
            EventHandler.UpdateManaBar -= OnUpdateManaBar;
            EventHandler.UpdateActionPoint -= OnUpdateActionPoint;
            EventHandler.StartNewCharacterTurn -= OnStartNewCharacterTurn;
        }
        private void OnEnterBattle(BattleTrigger battleTrigger  )
        {
            if (TurnBasedController.Instance.isBattleing) return;
            GetComponent<Page>().Enter(false);
        }

        private void OnEndBattle(BattleResult battleResult)
        {
            GetComponent<Page>().Exit(false);
            // foreach (Transform child in ActionContainer)
            // {
            //     Destroy(child.gameObject);
            // }
        }

        /// <summary>
        /// 更新当前行动人物血条
        /// </summary>
        /// <param name="fillAmount"></param>
        private void OnUpdateHealthBar(float fillAmount)
        {
            HealthBar.fillAmount = fillAmount;
        }

        /// <summary>
        /// 更新当前行动人物蓝条
        /// </summary>
        /// <param name="fillAmount"></param>
        private void OnUpdateManaBar(float fillAmount)
        {
            ManaBar.fillAmount = fillAmount;
        }

        /// <summary>
        /// 更新当前行动人物行动点
        /// </summary>
        /// <param name="ActionPointCount"></param>
        private void OnUpdateActionPoint(int ActionPointCount)
        {

        }
        
        private void OnStartNewCharacterTurn(Character newCharacter)
        {
            if(newCharacter.teamID!=Team.Player) return;
            HealthBar.fillAmount = newCharacter.healthPercentage;
            ManaBar.fillAmount = newCharacter.ManaPercentage;
            //更新技能框
            foreach (Transform child in ActionContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (var item in newCharacter.abilitiesForUse)
            {
                ActionSlot Action = Instantiate(ActionSlot, ActionContainer);
                Action.SetAbility(item);
                // Action.enabled = true;
            }
        }
    }
}
