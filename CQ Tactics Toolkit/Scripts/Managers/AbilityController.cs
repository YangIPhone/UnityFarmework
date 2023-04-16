using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CQFramework;

namespace CQTacticsToolkit{
    public class AbilityController : MonoBehaviour
    {
        private static AbilityController _instance;
        public static AbilityController Instance { get { return _instance; } }
        // public GameEventString disableAbility;
        // public RangeFinder eventRangeController;

        [SerializeField]private Character activeCharacter;
        [SerializeField] private AbilityContainer abilityContainer;
        private List<OverlayTile> abilityRangeTiles;
        private List<OverlayTile> abilityAffectedTiles;
        private OverlayTile focusedOnTile;
        private ShapeParser shapeParser;
        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
        private void OnEnable() {
            EventHandler.FocusOnNewTile += SetAbilityPosition;
            // EventHandler.CastAbility += AbilityModeEvent;
            EventHandler.StartNewCharacterTurn += SetActiveCharacter;
            EventHandler.EndBattle += OnEndBattle;
            // EventHandler.AbilityCommand += OnAIAbilityCommand;
        }
        private void OnDisable() {
            EventHandler.FocusOnNewTile -= SetAbilityPosition;
            // EventHandler.CastAbility -= AbilityModeEvent;
            EventHandler.StartNewCharacterTurn -= SetActiveCharacter;
            EventHandler.EndBattle -= OnEndBattle;
            // EventHandler.AbilityCommand -= OnAIAbilityCommand;
        }
        // Start is called before the first frame update
        void Start()
        {
            // eventRangeController = new RangeFinder();
            shapeParser = new ShapeParser();
            abilityRangeTiles = new List<OverlayTile>();
            abilityAffectedTiles = new List<OverlayTile>();
        }

        private void Update()
        {
            if (abilityContainer != null && abilityContainer.ability != null && Input.GetMouseButtonDown(0))
            {
                if(focusedOnTile != null && abilityRangeTiles.Where(x => x.grid2DLocation == focusedOnTile.grid2DLocation).Any())
                {
                    CastAbility(activeCharacter);
                }
            }
        }

        //释放一个技能
        private void CastAbility(Character AbilityUser)
        {
            var inRangeCharacters = new List<Character>();
            //获取范围内的角色
            foreach (var tile in abilityAffectedTiles)
            {
                var targetCharacter = tile.activeCharacter;
                if (targetCharacter != null && CheckAbilityTargets(abilityContainer.ability.abilityType, targetCharacter) && targetCharacter.isAlive)
                {
                    inRangeCharacters.Add(targetCharacter);
                }
            }
            //附加BUFF效果
            foreach (var character in inRangeCharacters)
            {
                Debug.Log($"{character.characterClass.characterName}收到伤害");
                foreach (var effect in abilityContainer.ability.effects)
                {
                    character.AttachEffect(effect);
                }

                //TODO 不同技能不同表现形式(位移，伤害&回血,控制等)
                //触发伤害
                switch (abilityContainer.ability.abilityType)
                {
                    case AbilityTypes.Ally:
                        character.HealHealth(abilityContainer.ability.value);
                        break;
                    case AbilityTypes.Enemy:
                        character.TakeDamage(abilityContainer.ability.value);
                        break;
                    case AbilityTypes.All:
                        character.TakeDamage(abilityContainer.ability.value);
                        break;
                    default:
                        break;
                }
            }
            abilityContainer.turnsSinceUsed = 0;
            // activeCharacter.UpdateInitiative(Constants.AbilityCost);
            AbilityUser.HealMana(-abilityContainer.ability.cost);
            AbilityUser.UpdateActionPoint(-abilityContainer.ability.costPoint);
            //TODO 禁用技能
            // disableAbility.Raise(abilityContainer.ability.Name);
            abilityContainer = null;
            OverlayController.Instance.ClearTiles(null);
        }

        //TODO 接收释放技能事件(AI释放技能)
        public void OnAIAbilityCommand(List<OverlayTile> TileList, AbilityContainer ability,Character AbilityUser)
        {
            abilityAffectedTiles = TileList;
            abilityContainer = ability;
            CastAbility(AbilityUser);
        }
        // public void CastAbilityCommand(EventCommand abilityCommand)
        // {
            // if (abilityCommand is CastAbilityCommand)
            // {
            //     CastAbilityCommand command = (CastAbilityCommand)abilityCommand;
            //     CastAbilityParams castAbilityParams = command.StronglyTypedCommandParam();
            //     abilityAffectedTiles = castAbilityParams.affectedTiles;
            //     abilityContainer = castAbilityParams.abilityContainer;
            //     CastAbility();
            // }
        // }

        //TODO 检查技能是否针对正确的实体。
        private bool CheckAbilityTargets(AbilityTypes abilityType, Character characterTarget)
        {
            //敌人
            if (abilityType == AbilityTypes.Enemy)
            {
                return characterTarget.teamID != activeCharacter.teamID;
            }
            //盟友
            else if (abilityType == AbilityTypes.Ally)
            {
                return characterTarget.teamID == activeCharacter.teamID;
            }
            return true;
        }

        //TODO 角色行动完毕，切换新角色
        public void SetActiveCharacter(Character activeChar)
        {
            activeCharacter = activeChar;
        }

        //设置技能原点的位置
        public void SetAbilityPosition(OverlayTile focusedOnTile)
        {
            if (abilityContainer != null && abilityContainer.ability!=null)
            {
                var map = MapManager.Instance.map;
                this.focusedOnTile = focusedOnTile.GetComponent<OverlayTile>();
                if (abilityRangeTiles.Contains(map[this.focusedOnTile.gridLocation]))
                {
                    abilityAffectedTiles = shapeParser.GetAbilityTileLocations(this.focusedOnTile, abilityContainer.ability.abilityShape, activeCharacter.activeTile.grid2DLocation,abilityContainer.ability.abilityHeight);
                    if (abilityContainer.ability.includeOrigin)
                        abilityAffectedTiles.Add(this.focusedOnTile);
                    OverlayController.Instance.ColorTiles(OverlayController.Instance.AttackRangeColor, abilityAffectedTiles,false);
                }
            }
        }

        /// <summary>
        /// 玩家进入技能释放模式。 
        /// </summary>
        /// <param name="abilityName">技能名</param>
        /// <param name="abilityID">技能ID</param>
        public void AbilityModeEvent(string abilityName,int abilityID)
        {
            OverlayController.Instance.ClearTiles(null);
            //通过技能名和ID在人物技能列表查找对应技能
            var abilityContainer = activeCharacter.abilitiesForUse.Find(x => x.ability.AbilityName == abilityName&&x.ability.AbilityID == abilityID);
            if (abilityContainer!=null&&abilityContainer.ability!= null)
            {
                if(abilityContainer.ability.cost <= activeCharacter.characterClass.CurrentMana && abilityContainer.ability.costPoint <= activeCharacter.characterClass.CurrentActionPoint)
                {
                    // abilityRangeTiles = eventRangeController.GetTilesInUseRange(activeCharacter.activeTile, abilityContainer.ability.range);
                    abilityRangeTiles = RangeFinder.Instance.GetTilesInUseRange(activeCharacter.activeTile, abilityContainer.ability.range);
                    OverlayController.Instance.ColorTiles(OverlayController.Instance.UseRangeColor, abilityRangeTiles,false);
                    this.abilityContainer = abilityContainer;
                }else{
                    //TODO 提示无法释放
                    Debug.Log("技能无法释放");
                }
            }
        }

        /// <summary>
        /// 玩家退出技能释放模式
        /// </summary>
        public void CancelEventMode()
        {
            OverlayController.Instance.ClearTiles(null);
            abilityContainer = null;
        }

        public void OnEndBattle(bool isVictory)
        {
            CancelEventMode();
        }
    }

}