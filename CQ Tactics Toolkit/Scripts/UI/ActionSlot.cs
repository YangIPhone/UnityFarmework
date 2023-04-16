using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using CQFramework;
namespace CQTacticsToolkit
{
    public class ActionSlot : MonoBehaviour
    {
        [SerializeField]private Image AbilityIcon;
        [SerializeField] private TextMeshProUGUI AbilityName;
        private Button actionButton=>GetComponent<Button>();
        [SerializeField] private Ability ability;

        private void OnEnable()
        {

        }
        private void OnDisable() {
            RemoveButtonClick(SoltClick);
        }
        /// <summary>
        /// 设置插槽数据(插槽可能是技能，也可能是消耗品)
        /// </summary>
        /// <param name="abilityContainer"></param>
        public void SetAbility(AbilityContainer abilityContainer)
        {
            AbilityIcon.sprite = abilityContainer.ability.Icon;
            AbilityName.text = abilityContainer.ability.AbilityName;
            this.ability = abilityContainer.ability;
            AddButtonClickEvent(SoltClick);
        }
        /// <summary>
        /// 设置快捷键
        /// </summary>
        public void SetShortcutKey(int index)
        {

        }
        /// <summary>
        /// 添加按钮监听
        /// </summary>
        /// <param name="action"></param>
        public void AddButtonClickEvent(UnityAction action)
        {
            if(actionButton!=null)
            {
                actionButton.onClick.AddListener(action);
            }
        }

        public void RemoveButtonClick(UnityAction action){
            if (actionButton != null)
            {
                actionButton.onClick.RemoveListener(action);
            }
        }
        private void SoltClick()
        {
            if(ability!=null) {
                // EventHandler.CallCastAbility(ability.AbilityName,ability.AbilityID);
                AbilityController.Instance.AbilityModeEvent(ability.AbilityName, ability.AbilityID);
            }
        }
    }
}

