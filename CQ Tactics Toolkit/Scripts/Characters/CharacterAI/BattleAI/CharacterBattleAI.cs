using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CQFramework;

namespace CQFramework.CQTacticsToolkit
{
    public class CharacterBattleAI : Character
    {
        [Header("AI行动策略")]
        public Personality personality = Personality.Strategic;
        [Header("玩家列表")]
        public List<Character> playerCharacters;
        //TODO AI可触发的事件
        // public GameEventGameObjectList moveAlongPath; //移动
        // public GameEventCommand castAbility;//释放技能
        // public GameEventString logAction;//输出行动日志

        // private List<Character> enemyCharacters;
        private List<OverlayTile> path;
        private ShapeParser shapeParser;
        private Senario bestSenario;
        // private RangeFinder rangeFinder;
        // private PathFinder pathFinder;
        private void OnEnable() {
            // enemyCharacters = TurnBasedController.Instance.GetEnemyTeam();
        }
        private void OnDisable() {
        }
        private void Start() {
            // pathFinder = new PathFinder();
            // rangeFinder = new RangeFinder();
            shapeParser = new ShapeParser();
        }
        new void Update()
        {
            base.Update();
        }
        public override void StartTurn()
        {
            if(!isBattle) return;
            EventHandler.CallLogAction($"{characterClass.characterName}开始行动");
            base.StartTurn();
            playerCharacters = TurnBasedController.Instance.GetPlayerTeam();
            StartCoroutine(CalculateBestSenario());
        }

        public override void CharacterMoved()
        {
            if (!isBattle) return;
            //TODO 打印战斗信息
            EventHandler.CallLogAction($"{characterClass.characterName}移动到{bestSenario.positionTile.gridLocation}");
            //当角色完成移动时，检查攻击/技能是否可用并执行。否则结束
            if (bestSenario != null && (bestSenario.targetTile != null || bestSenario.targetAbility != null))
                Attack();
            else
                StartCoroutine(CalculateBestSenario());
        }

        private Senario AutoAttackBasedOnPersonality(OverlayTile position)
        {
            switch (personality)
            {
                case Personality.Aggressive:
                    //积极进攻,在尽可能靠近的同时攻击最近的角色。
                    return AggressiveBasicAttackTarget(position);
                case Personality.Defensive:
                    //保守，攻击距离最近的角色，同时保持最大距离。 
                    return DefenciveBasicAttackTarget(position);
                case Personality.Strategic:
                    //策略，攻击生命值最低的角色，同时保持最大距离
                    return StrategicBasicAttackTarget(position);
                default:
                    break;
            }

            return new Senario();
        }

        /// <summary>
        /// 策略型AI基本攻击
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Senario StrategicBasicAttackTarget(OverlayTile position)
        {
            var targetCharacter = FindClosestToDeathCharacter(position);
            if (targetCharacter)
            {
                // var closestDistance = pathFinder.GetManhattenDistance(position, targetCharacter.activeTile);
                var closestDistance = PathFinder.Instance.GetManhattenDistance(position, targetCharacter.activeTile);
                if (closestDistance <= GetStat<int>(Stats.AttackRange.ToString()))
                {
                    //计算senarioValue;
                    var senarioValue = GetStat<int>(Stats.Strenght.ToString())
                        + closestDistance
                        - targetCharacter.GetStat<int>(Stats.CurrentHealth.ToString());
                    //TODO 可以杀死目标,直接进行攻击,消耗最小化
                    if (targetCharacter.GetStat<int>(Stats.CurrentHealth.ToString()) < GetStat<int>(Stats.Strenght.ToString()))
                    {
                        senarioValue = 10000;
                    }
                    return new Senario(senarioValue, null, targetCharacter.activeTile, position, true);
                }
            }
            return new Senario();
        }

        /// <summary>
        /// 防守型AI基本攻击目标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Senario DefenciveBasicAttackTarget(OverlayTile position)
        {
            var targetCharacter = FindClosestCharacter(position);

            if (targetCharacter)
            {
                // var closestDistance = pathFinder.GetManhattenDistance(position, targetCharacter.activeTile);
                var closestDistance = PathFinder.Instance.GetManhattenDistance(position, targetCharacter.activeTile);
                //检查最近的角色是否在攻击范围内
                if (closestDistance <= GetStat<int>(Stats.AttackRange.ToString()))
                {
                    //计算senarioValue;
                    var senarioValue = 0;
                    senarioValue += GetStat<int>(Stats.Strenght.ToString()) + (closestDistance - GetStat<int>(Stats.MoveRange.ToString()));
                    return new Senario(senarioValue, null, targetCharacter.activeTile, position, true);
                }
            }
            return new Senario();
        }

        /// <summary>
        /// 进攻型AI基本攻击目标
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Senario AggressiveBasicAttackTarget(OverlayTile position)
        {
            var targetCharacter = FindClosestCharacter(position);

            if (targetCharacter)
            {
                // var closestDistance = pathFinder.GetManhattenDistance(position, targetCharacter.activeTile);
                var closestDistance = PathFinder.Instance.GetManhattenDistance(position, targetCharacter.activeTile);

                //检查最近的角色是否在攻击范围内，并确保我们不在角色的瓦片上。
                if (closestDistance <= GetStat<int>(Stats.AttackRange.ToString()) && position.gridLocation != targetCharacter.activeTile.gridLocation)
                {
                    // var targetTile = GetClosestNeighbour(targetCharacter.activeTile);

                    //计算senarioValue;
                    var senarioValue = 0;
                    senarioValue += GetStat<int>(Stats.Strenght.ToString()) + (GetStat<int>(Stats.MoveRange.ToString()) - closestDistance);
                    return new Senario(senarioValue, null, targetCharacter.activeTile, position, true);
                }
            }

            return new Senario();
        }

        //找到最接近的角色
        private Character FindClosestCharacter(OverlayTile position)
        {
            Character targetCharacter = null;

            var closestDistance = 1000;
            foreach (var player in playerCharacters)
            {
                if (player.isAlive)
                {
                    // var currentDistance = pathFinder.GetManhattenDistance(position, player.activeTile);
                    var currentDistance = PathFinder.Instance.GetManhattenDistance(position, player.activeTile);

                    if (currentDistance <= closestDistance)
                    {
                        closestDistance = currentDistance;
                        targetCharacter = player;
                    }
                }
            }

            return targetCharacter;
        }

        /// <summary>
        /// 找到范围内生命值最低的角色
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Character FindClosestToDeathCharacter(OverlayTile position)
        {
            Character targetCharacter = null;
            int lowestHealth = -1;
            var noCharacterInRange = true;
            foreach (var player in playerCharacters)
            {
                if (player.isAlive && player.activeTile!=null)
                {
                    // var currentDistance = pathFinder.GetManhattenDistance(position, player.activeTile);
                    var currentDistance = PathFinder.Instance.GetManhattenDistance(position, player.activeTile);
                    var currentHealth = player.GetStat<int>(Stats.CurrentHealth.ToString());

                    if (currentDistance <= GetStat<int>(Stats.AttackRange.ToString()) &&
                        ((lowestHealth == -1) || (currentHealth <= lowestHealth || noCharacterInRange)))
                    {
                        lowestHealth = currentHealth;
                        targetCharacter = player;
                        noCharacterInRange = false;
                    }
                    else if (noCharacterInRange && ((lowestHealth == -1) || (currentHealth <= lowestHealth)))
                    {
                        lowestHealth = currentHealth;
                        targetCharacter = player;
                    }
                }
            }

            //不能移动到单位瓦片，所以获得最近的邻居
            return targetCharacter;
        }

        //TODO 找到靠近瓦片的最近瓦片。
        /// <summary>
        /// 找到靠近瓦片的最近瓦片。  
        /// </summary>
        /// <param name="targetCharacterTile"></param>
        /// <returns></returns>  
        private OverlayTile GetClosestNeighbour(OverlayTile targetCharacterTile)
        {
            var targetNeighbours = MapManager.Instance.GetNeighbourTiles(targetCharacterTile, new List<OverlayTile>());
            var targetTile = targetNeighbours[0];
            // var targetDistance = pathFinder.GetManhattenDistance(targetTile, activeTile);
            var targetDistance = PathFinder.Instance.GetManhattenDistance(targetTile, activeTile);

            foreach (var item in targetNeighbours)
            {
                // var distance = pathFinder.GetManhattenDistance(item, activeTile);
                var distance = PathFinder.Instance.GetManhattenDistance(item, activeTile);
                if (distance < targetDistance)
                {
                    targetTile = item;
                    targetDistance = distance;
                }
            }

            return targetTile;
        }

        /// <summary>
        /// 查找一组瓦片中的所有角色。
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        private List<Character> FindAllCharactersInTiles(List<OverlayTile> tiles)
        {
            var playersInRange = new List<Character>();
            foreach (var tile in tiles)
            {
                //TODO 需要改变这个来考虑友方能力
                if (tile.activeCharacter && tile.activeCharacter.teamID != teamID && tile.activeCharacter.isAlive)
                {
                    playersInRange.Add(tile.activeCharacter);
                }
            }

            return playersInRange;
        }

        /// <summary>
        /// 如果能攻击，我们就攻击，如果有能力，就施放能力
        /// </summary>
        private void Attack()
        {
            if (bestSenario.useAutoAttack && bestSenario.targetTile.activeCharacter)
                StartCoroutine(AttackTargettedCharacter(bestSenario.targetTile.activeCharacter));
            else if (bestSenario.targetAbility != null)
                StartCoroutine(CastAbility());
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <returns></returns>
        private IEnumerator CastAbility()
        {
            var abilityAffectedTiles = shapeParser.GetAbilityTileLocations(bestSenario.targetTile, bestSenario.targetAbility.ability.abilityShape, bestSenario.positionTile.grid2DLocation,bestSenario.targetAbility.ability.abilityHeight);
            if (bestSenario.targetAbility.ability.includeOrigin) abilityAffectedTiles.Add(bestSenario.targetTile);
            OverlayController.Instance.ColorTiles(OverlayController.Instance.AttackRangeColor, abilityAffectedTiles);
            yield return new WaitForSeconds(0.5f);
            EventHandler.CallLogAction($"{characterClass.characterName}释放了{bestSenario.targetAbility.ability.AbilityName}");
            //通知AbilityController释放这个技能
            // EventHandler.CallAbilityCommand(abilityAffectedTiles,bestSenario.targetAbility,this);
            AbilityController.Instance.OnAIAbilityCommand(abilityAffectedTiles, bestSenario.targetAbility, this);
            //TODO 结束回合或者开始一个新策略
            StartCoroutine(CalculateBestSenario());
        }

        /// <summary>
        /// 应用普通攻击并攻击目标
        /// </summary>
        /// <param name="targetedCharacter"></param>
        /// <returns></returns>
        private IEnumerator AttackTargettedCharacter(Character targetedCharacter)
        {
            OverlayController.Instance.ColorSingleTile(OverlayController.Instance.BlockedTileColor, targetedCharacter.activeTile);
            yield return new WaitForSeconds(0.5f);
            if (GetStat<int>(Stats.CurrentActionPoint.ToString()) >= 1)
            {
                //TODO 打印战斗信息
                EventHandler.CallLogAction($"{characterClass.characterName}使用了普通攻击，造成{GetStat<int>(Stats.Strenght.ToString())}攻击力伤害");
                UpdateActionPoint(-1);
                //TODO 计算伤害，例如伤害只是攻击力属性。
                targetedCharacter.TakeDamage(GetStat<int>(Stats.Strenght.ToString()));
                //更新行动值
                // UpdateInitiative(Constants.AttackCost);
                //TODO 结束回合或者开始一个新策略
                StartCoroutine(CalculateBestSenario());
            }else{
                StartCoroutine(EndTurn());
            }
        }

        private IEnumerator EndTurn()
        {
            yield return new WaitForSeconds(0.25f);
            OverlayController.Instance.ClearTiles();
            //TODO 打印战斗信息
            StopAllCoroutines();
            EventHandler.CallLogAction($"{characterClass.characterName}结束回合");
            EventHandler.CallEndTurn();
        }

        /// <summary>
        /// 计算一个策略
        /// </summary>
        /// <returns></returns>
        private IEnumerator CalculateBestSenario()
        {
            //点数为0不可行动
            if(GetStat<int>(Stats.CurrentActionPoint.ToString())==0){
                StartCoroutine(EndTurn());
            }else{
                // var tileInMovementRange = rangeFinder.GetTilesInRange(activeTile, GetStat<int>(Stats.MoveRange.ToString()));
                // var tileInMovementRange = rangeFinder.GetTilesInRange(activeTile, characterClass.GetMoveRange());
                var tileInMovementRange = RangeFinder.Instance.GetTilesInRange(activeTile, characterClass.GetMoveRange());
                var senario = new Senario();
                foreach (var tile in tileInMovementRange)
                {
                    if (!tile.isBlocked||tile.gridLocation==activeTile.gridLocation)
                    {
                        var tempSenario = CreateTileSenarioValue(tile);
                        ApplyTileEffectsToSenarioValue(tile, tempSenario);
                        senario = CompareSenarios(senario, tempSenario);
                        senario = CheckIfSenarioValuesAreEqual(tileInMovementRange, senario, tempSenario);
                        senario = CheckSenarioValueIfNoTarget(senario, tile, tempSenario);
                    }
                }
                if (senario.positionTile!=null)
                {
                    ApplyBestSenario(senario);
                }
                else
                {
                    StartCoroutine(EndTurn());
                }
            }
            yield return null;
        }

        /// <summary>
        /// 执行最好的senario
        /// </summary>
        /// <param name="senario"></param>
        private void ApplyBestSenario(Senario senario)
        {
            bestSenario = senario;
            // path = pathFinder.FindPath(activeTile, bestSenario.positionTile);
            path = PathFinder.Instance.FindPath(activeTile, bestSenario.positionTile);

            //如果它可以攻击但不需要移动，就攻击
            if (path.Count == 0 && bestSenario.targetTile != null)
            {
                OverlayController.Instance.ClearTiles();
                Attack();
            }
            else
            {
                //进行移动
                StartCoroutine(Move(path));
            }
        }

        /// <summary>
        /// 将瓦片效果应用到Senario。即熔岩砖造成的烧伤损害
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tempSenario"></param>
        private void ApplyTileEffectsToSenarioValue(OverlayTile tile, Senario tempSenario)
        {
            if (tile.tileData && tile.tileData.effect)
            {
                var tileEffectValue = GetEffectsSenarioValue(new List<BuffEffect>() { tile.tileData.effect }, new List<Character>() { this });
                tempSenario.senarioValue -= tileEffectValue;
            }
        }

        /// <summary>
        /// 检查新senario是否比当前最好的senario更好。
        /// </summary>
        /// <param name="senario"></param>
        /// <param name="tempSenario"></param>
        /// <returns></returns>
        private static Senario CompareSenarios(Senario senario, Senario tempSenario)
        {
            if ((tempSenario != null && tempSenario.senarioValue > senario.senarioValue))
            {
                senario = tempSenario;
            }

            return senario;
        }

        /// <summary>
        /// 如果新的Senario和当前最好的Senario相等，则取瓦片距离最近的Senario。
        /// </summary>
        /// <param name="tileInMovementRange"></param>
        /// <param name="senario"></param>
        /// <param name="tempSenario"></param>
        /// <returns></returns>
        private Senario CheckIfSenarioValuesAreEqual(List<OverlayTile> tileInMovementRange, Senario senario, Senario tempSenario)
        {
            if (tempSenario.positionTile != null && tempSenario.senarioValue == senario.senarioValue)
            {
                // var tempSenarioPathCount = pathFinder.FindPath(activeTile, tempSenario.positionTile,this, tileInMovementRange).Count;
                // var senarioPathCount = pathFinder.FindPath(activeTile, senario.positionTile, this, tileInMovementRange).Count;
                var tempSenarioPathCount = PathFinder.Instance.FindPath(activeTile, tempSenario.positionTile, this, tileInMovementRange).Count;
                var senarioPathCount = PathFinder.Instance.FindPath(activeTile, senario.positionTile, this, tileInMovementRange).Count;
                if (tempSenarioPathCount < senarioPathCount)
                    senario = tempSenario;
            }
            return senario;
        }

        /// <summary>
        /// //如果我们没有攻击目标，检查瓦片与角色的距离。
        /// </summary>
        /// <param name="senario"></param>
        /// <param name="tile"></param>
        /// <param name="tempSenario"></param>
        /// <returns></returns>
        private Senario CheckSenarioValueIfNoTarget(Senario senario, OverlayTile tile, Senario tempSenario)
        {
            if (tempSenario.positionTile == null && senario.targetTile==null)
            {
                var targetCharacter = FindClosestToDeathCharacter(tile);
                if (targetCharacter)
                {
                    var targetTile = GetClosestNeighbour(targetCharacter.activeTile);

                    if (targetCharacter!=null && targetTile!=null)
                    {
                        // var pathToCharacter = pathFinder.FindPath(tile, targetTile,this, new List<OverlayTile>());
                        var pathToCharacter = PathFinder.Instance.FindPath(tile, targetTile, this, new List<OverlayTile>());
                        var distance = pathToCharacter.Count;
                        var senarioValue = -distance - targetCharacter.GetStat<int>(Stats.CurrentHealth.ToString());
                        if (distance > GetStat<int>(Stats.MoveRange.ToString()))
                        {
                            if (tile.tileData && tile.tileData.effect)
                            {
                                var tileEffectValue = GetEffectsSenarioValue(new List<BuffEffect>() { tile.tileData.effect }, new List<Character>() { this });
                                senarioValue -= tileEffectValue;
                            }
                            if (tile.gridLocation != activeTile.gridLocation && tile.gridLocation != targetCharacter.activeTile.gridLocation && (senarioValue > senario.senarioValue || senario.positionTile==null))
                                senario = new Senario(senarioValue, null, null, tile, false);
                        }
                    }
                }
            }
            return senario;
        }

        /// <summary>
        /// 告诉MovementController沿着路径移动敌人。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerator Move(List<OverlayTile> path)
        {
            OverlayController.Instance.ColorSingleTile(OverlayController.Instance.MoveRangeColor, bestSenario.positionTile);
            yield return new WaitForSeconds(0.25f);
            //点数足够才进行移动,反之结束回合
            if(GetStat<int>(Stats.CurrentActionPoint.ToString()) >= 1){
                UpdateActionPoint(-1);
                //TODO 告诉MovementController沿着路径移动敌人
                SetPath(path);
            }
            // moveAlongPath.Raise(path.Select(x => x.gameObject).ToList());
        }


        /// <summary>
        /// 根据是否可以从这个瓦片中攻击&释放技能来创建一个Senario。
        /// </summary>
        /// <param name="overlayTile"></param>
        /// <returns></returns>
        private Senario CreateTileSenarioValue(OverlayTile overlayTile)
        {
            int moveCost = 0;
            Senario attackSenario = new Senario() ;
            if (activeTile.gridLocation != overlayTile.gridLocation) {
                //需要移动到这个瓦片，点数消耗加上移动的消耗
                moveCost += 1 ;
            }
            //剩余点数大于攻击消耗加移动消耗的点数才计算普通攻击的策略
            if(characterClass.CurrentActionPoint>=moveCost+1){
                //基础攻击的策略值
                attackSenario = AutoAttackBasedOnPersonality(overlayTile);
            }
            //计算技能策略
            foreach (var abilityContainer in abilitiesForUse)
            {
                if (GetStat<int>(Stats.CurrentMana.ToString()) >= abilityContainer.ability.cost && 
                    abilityContainer.turnsSinceUsed >= abilityContainer.ability.cooldown &&
                    GetStat<int>(Stats.CurrentActionPoint.ToString())>=abilityContainer.ability.costPoint+moveCost)
                {
                    //技能的策略值
                    var tempSenario = CreateAbilitySenario(abilityContainer, overlayTile);
                    if (tempSenario.senarioValue > attackSenario.senarioValue)
                        attackSenario = tempSenario;
                }
            }
            return attackSenario;
        }

        /// <summary>
        /// 计算一个技能的senario值。
        /// </summary>
        /// <param name="abilityContainer"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private Senario CreateAbilitySenario(AbilityContainer abilityContainer, OverlayTile position)
        {
            // List<OverlayTile> tempPath = pathFinder.FindPath(activeTile,position);
            // var tilesInAbilityRange = rangeFinder.GetTilesInRange(position, abilityContainer.ability.range,this, true);
            // var tilesInAbilityRange = rangeFinder.GetTilesInUseRange(position, abilityContainer.ability.range);
            var tilesInAbilityRange = RangeFinder.Instance.GetTilesInUseRange(position, abilityContainer.ability.range);
            var senario = new Senario();
            foreach (var tile in tilesInAbilityRange)
            {
                var abilityAffectedTiles = shapeParser.GetAbilityTileLocations(tile, abilityContainer.ability.abilityShape, position.grid2DLocation,abilityContainer.ability.abilityHeight);
                //该技能可以击中多少玩家
                var players = FindAllCharactersInTiles(abilityAffectedTiles);
                //将技能的BUFF影响算入senario值
                var totalAbilityDamage = GetEffectsSenarioValue(abilityContainer.ability.effects, players);
                if (players.Count > 0)
                {
                    var totalPlayerHealth = 0;
                    //生命值最低的玩家角色的生命值
                    var weakestPlayerHealth = int.MaxValue;
                    var closestDistance = 0;
                    var damageValue = 0;
                    foreach (var player in players)
                    {
                        closestDistance = -1000;
                        totalPlayerHealth += player.GetStat<int>(Stats.CurrentHealth.ToString());
                        if (player.GetStat<int>(Stats.CurrentHealth.ToString()) < weakestPlayerHealth)
                        {
                            weakestPlayerHealth = player.GetStat<int>(Stats.CurrentHealth.ToString());
                        }
                        // var tempClosestDistance = pathFinder.GetManhattenDistance(position, player.activeTile);
                        var tempClosestDistance = PathFinder.Instance.GetManhattenDistance(position, player.activeTile);
                        if (tempClosestDistance > closestDistance)
                            closestDistance = tempClosestDistance;
                        //TODO 技能伤害根据属性加成
                        totalAbilityDamage += abilityContainer.ability.value;
                    }
                    damageValue += totalAbilityDamage;
                    var tempSenarioValue = damageValue
                            + closestDistance
                            - weakestPlayerHealth;

                    if (tempSenarioValue > senario.senarioValue)
                    {
                        senario = new Senario(tempSenarioValue, abilityContainer, tile, position, false);
                    }
                }
            }
            return senario;
        }

        /// <summary>
        /// TODO 计算一个BUFF对Senario值的影响程度
        /// </summary>
        /// <param name="effectsContainer"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static int GetEffectsSenarioValue(List<BuffEffect> effectsContainer, List<Character> characters)
        {
            var totalDamage = 0;
            int totalSenarioValue = 0;

            if (effectsContainer.Count > 0 && characters.Count > 0)
            {
                foreach (var effect in effectsContainer)
                {
                    foreach (var character in characters)
                    {
                        if (effect.Operator == Operation.Minus)
                        {
                            totalDamage += Mathf.RoundToInt(effect.Value * (effect.Duration > 0 ? effect.Duration : 1));
                        }
                        else if (effect.Operator == Operation.MinusByPercentage)
                        {
                            var value = character.GetStat<int>(Stats.CurrentHealth.ToString()) / 100 * effect.Value;
                            totalDamage += Mathf.RoundToInt(value * (effect.Duration > 0 ? effect.Duration : 1));
                        }
                        if (totalDamage >= character.GetStat<int>(Stats.CurrentHealth.ToString()))
                            totalSenarioValue += 10000;
                        else
                            totalSenarioValue += totalDamage;
                    }
                }
            }

            return totalSenarioValue;
        }
    }
}
