using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CQFramework;
namespace CQTacticsToolkit
{
    public class TurnOrderDisplay : MonoBehaviour
    {
        public GameObject CharacterSlot;
        [SerializeField]private RectTransform ScrollViewContent;
        private void OnEnable() {
            EventHandler.TurnOrderUpdated += OnTurnOrderUpdated;
            EventHandler.EnterBattle += OnEnterBattle;
            EventHandler.EndBattle += OnEndBattle;
        }

        private void OnDisable() {
            EventHandler.TurnOrderUpdated -= OnTurnOrderUpdated;
            EventHandler.EnterBattle -= OnEnterBattle;
            EventHandler.EndBattle -= OnEndBattle;
        }
        private void OnEnterBattle(List<Character> EnemyTeam)
        {
            if (TurnBasedController.Instance.isBattleing) return;
            GetComponent<Page>().Enter(false);
        }
        private void OnEndBattle(bool isVictory)
        {
            GetComponent<Page>().Exit(false);
        }
        public void OnTurnOrderUpdated(List<Character> characters)
        {
            foreach (Transform child in ScrollViewContent)
            {
                Destroy(child.gameObject);
            }
            //the order should be consistent
            foreach (var item in characters)
            {
                var spawnedObject = Instantiate(CharacterSlot, ScrollViewContent);
                spawnedObject.GetComponent<Image>().sprite = item.characterClass.characterHeadSprite;
                spawnedObject.GetComponentInChildren<TextMeshProUGUI>().text = item.characterClass.characterName;
            }
        }
    }
}
