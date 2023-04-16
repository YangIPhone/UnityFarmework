using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;
using UnityEngine.EventSystems;

namespace CQFramework.CQTacticsToolkit
{
    public class MovementController : MonoBehaviour
    {
        public float speed = 10f;
        public Character activeCharacter;
        public bool showAttackRange;
        [Header("是否可穿过盟友")]
        public bool moveThroughAllies = true;
        [Header("是否忽略障碍物")]
        public bool ignoreObstacles = false;
        [SerializeField] private bool movementModeEnabled = false;
        [SerializeField] private bool isMoving = false;
        [SerializeField] private OverlayTile focusedTile;
        private bool isNewFocusedTile;
        // public GameEvent endTurnEvent;
        // public GameEventGameObject displayAttackRange;
        // public GameEventString cancelActionEvent;

        // private PathFinder pathFinder;
        // private RangeFinder rangeFinder;
        // private ArrowTranslator arrowTranslator;
        private List<OverlayTile> path = new List<OverlayTile>();
        private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        private List<OverlayTile> inAttackRangeTiles = new List<OverlayTile>();
        private void OnEnable() {
            EventHandler.StartNewCharacterTurn += SetActiveCharacter;
            EventHandler.FocusOnNewTile += FocusedOnNewTile;
        }
        private void OnDisable() {
            EventHandler.StartNewCharacterTurn -= SetActiveCharacter;
            EventHandler.FocusOnNewTile -= FocusedOnNewTile;
        }
        private void Start()
        {
            // pathFinder = new PathFinder();
            // rangeFinder = new RangeFinder();
            // arrowTranslator = new ArrowTranslator();
        }

        // Update is called once per frame
        void Update()
        {
            if (TurnBasedController.Instance.isBattleing&&activeCharacter && !activeCharacter.isAlive)
            {
                return;
            }
            if (focusedTile!=null && isNewFocusedTile)
            {
                if (inRangeTiles.Contains(focusedTile) && movementModeEnabled && !isMoving && TurnBasedController.Instance.isBattleing)
                {
                    foreach (var item in path)
                    {
                        item.SetArrowSprite(ArrowDirection.None);
                    }
                    // path = pathFinder.FindPath(activeCharacter.activeTile, focusedTile,activeCharacter, inRangeTiles, false, moveThroughAllies);
                    path = PathFinder.Instance.FindPath(activeCharacter.activeTile, focusedTile, activeCharacter, inRangeTiles, ignoreObstacles, moveThroughAllies);
                    isNewFocusedTile = false;
                    for (int i = 0; i < path.Count; i++)
                    {
                        var previousTile = i > 0 ? path[i - 1] : activeCharacter.activeTile;
                        var futureTile = i < path.Count - 1 ? path[i + 1] : null;
                        var arrowDir = ArrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                        path[i].SetArrowSprite(arrowDir);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject()&&MapManager.Instance.mapIsInited)
            {
                if (TurnBasedController.Instance.isBattleing &&!focusedTile.isBlocked && movementModeEnabled && path.Count > 0)
                {
                    isMoving = true;
                    activeCharacter.UpdateActionPoint(-1);
                    activeCharacter.SetPath(path);
                }
                //非战斗状态下自由移动
                else if(focusedTile!=null && !focusedTile.isBlocked && !TurnBasedController.Instance.isBattleing)
                {
                    activeCharacter = PlayerTeamContainer.Instance.characters[0];
                    path.Clear();
                    // path = pathFinder.FindPath(activeCharacter.activeTile, focusedTile, activeCharacter, null, ignoreObstacles);
                    path = PathFinder.Instance.FindPath(activeCharacter.activeTile, focusedTile, activeCharacter, null, ignoreObstacles,moveThroughAllies);
                    activeCharacter.SetPath(path);
                }
            }
            if (path.Count == 0 && movementModeEnabled && isMoving && TurnBasedController.Instance.isBattleing)
            {
                ResetMovementManager();
            }
        }
        
        private void ResetMovementManager()
        {
            movementModeEnabled = false;
            isMoving = false;
            OverlayController.Instance.ClearTiles(null);
            activeCharacter.CharacterMoved();
        }
        private void GetMoveRangeTiles()
        {
            var moveColor = OverlayController.Instance.MoveRangeColor;
            if (activeCharacter!=null && activeCharacter.activeTile!=null)
            {
                // inRangeTiles = rangeFinder.GetTilesInRange(activeCharacter.activeTile, activeCharacter.characterClass.GetMoveRange(), activeCharacter, ignoreObstacles, false);
                inRangeTiles = RangeFinder.Instance.GetTilesInRange(activeCharacter.activeTile, activeCharacter.characterClass.GetMoveRange(), activeCharacter, ignoreObstacles, false);
                // movementModeEnabled = true;
                OverlayController.Instance.ColorTiles(moveColor, inRangeTiles);
            }
        }

        //Moused over new tile and display the attack range. 
        public void FocusedOnNewTile(OverlayTile focusedOnTile)
        {
            if (!isMoving)
            {
                focusedTile = focusedOnTile;
                isNewFocusedTile = true;
            }
            if (movementModeEnabled && inRangeTiles.Where(x => x.grid2DLocation == focusedTile.grid2DLocation).Any() && !isMoving && TurnBasedController.Instance.isBattleing)
                ShowAttackRangeTiles(focusedTile);
        }

        public void ShowAttackRangeTiles(OverlayTile focusedOnTile)
        {
            var attackColor = OverlayController.Instance.AttackRangeColor;
            // inAttackRangeTiles = rangeFinder.GetTilesInRange(focusedOnTile, activeCharacter.GetStat<int>(Stats.AttackRange.ToString()),activeCharacter, true, moveThroughAllies);
            int range = showAttackRange ?activeCharacter.GetStat<int>(Stats.AttackRange.ToString()):0;
            inAttackRangeTiles = RangeFinder.Instance.GetTilesInRange(focusedOnTile, range,activeCharacter, true, moveThroughAllies);
            OverlayController.Instance.ColorTiles(attackColor, inAttackRangeTiles);
        }

        public void SetActiveCharacter(Character character)
        {
            activeCharacter = character;
        }

        //点击进入移动模式，显示移动范围
        public void EnterMovementMode()
        {
            //不在战斗状态则返回
            if(!TurnBasedController.Instance.isBattleing)return;
            if(activeCharacter.characterClass.CurrentActionPoint<=0) {
                Debug.Log("移动需消耗的点数不足");
                return;
            }
            AbilityController.Instance.CancelEventMode();
            GetMoveRangeTiles();
            movementModeEnabled = true;
        }

        public void EndTurn()
        {
            
        }
    }

}
