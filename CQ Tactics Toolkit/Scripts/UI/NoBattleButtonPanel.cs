using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CQFramework;

namespace CQFramework.CQTacticsToolkit
{
    public class NoBattleButtonPanel : MonoBehaviour
    {
        private void OnEnable()
        {
            EventHandler.EnterBattle += OnEnterBattle;
            EventHandler.EndBattle += OnEndBattle;
        }

        private void OnDisable()
        {
            EventHandler.EnterBattle -= OnEnterBattle;
            EventHandler.EndBattle -= OnEndBattle;
        }

        private void OnEnterBattle(BattleTrigger battleTrigger)
        {
            if(TurnBasedController.Instance.isBattleing)return;
            EnterBattle();
        }
        public void EnterBattle()
        {
            GetComponent<Page>().Exit(false);
            foreach (RectTransform item in transform)
            {
                item.gameObject.SetActive(false);
            }
        }
        public void OnEndBattle(BattleResult battleResult)
        {
            GetComponent<Page>().Enter(false);
            foreach (RectTransform item in transform)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
