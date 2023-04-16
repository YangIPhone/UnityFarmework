using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CQFramework;

namespace CQTacticsToolkit
{
    public class TurnBasedController : MonoBehaviour
    {
        private static TurnBasedController _instance;
        public static TurnBasedController Instance { get { return _instance; } }
        [Header("玩家队伍")]
        [SerializeField] private List<Character> PlayerTeam = new List<Character>();
        [Header("敌人队伍")]
        [SerializeField] private List<Character> EnemyTeam = new List<Character>();
        [Header("其他队伍")]
        [SerializeField] private List<Character> OtherTeam = new List<Character>();
        [Header("当前回合数")]
        public int turnCount = 0;
        [Header("是否正在战斗中")]
        public bool isBattleing = false;
        public TurnSorting turnSorting = TurnSorting.ConstantAttribute;

        //TODO 设置新角色回合事件
        // public GameEventGameObject startNewCharacterTurn;
        //TODO 
        // public GameEventGameObjectList turnOrderSet;

        public List<Character> combinedList;

        public bool ignorePlayers = false; //是否忽略玩家
        public bool ignoreEnemies = false;//是否忽略敌人
        private void Awake()
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
            EventHandler.EnterBattle+= EnterBattle;
            EventHandler.EndTurn += OnEndTurn;
        }
        private void OnDisable()
        {
            EventHandler.EnterBattle -= EnterBattle;
            EventHandler.EndTurn -= OnEndTurn;
        }

        public void EnterBattle(List<Character> EnemyTeam = null)
        {
            if (isBattleing) 
            {
                if(EnemyTeam != null && EnemyTeam.Count > 0)
                {
                    foreach (var newEnemy in EnemyTeam)
                    {
                        SpawnNewCharacter(newEnemy);
                    }
                }
                return;
            };
            if(EnemyTeam!=null&&EnemyTeam.Count>0&&!ignoreEnemies) 
            {
                this.EnemyTeam.AddRange(EnemyTeam);
                foreach (var enemy in EnemyTeam)
                {
                    enemy.isBattle = true;
                }
            }
            if (!ignorePlayers) 
            {
                this.PlayerTeam.AddRange(PlayerTeamContainer.Instance.characters);
                foreach (var player in PlayerTeam)
                {
                    player.isBattle = true;
                }
            }
            turnCount = 1;
            Invoke("StartBattle",0.5f);
        }
        public void EndBattle()
        {
            OverlayController.Instance.ClearTiles();
            foreach (var enemy in EnemyTeam)
            {
                enemy.isBattle = false;
            }
            foreach (var player in PlayerTeam)
            {
                player.isBattle = false;
            }
            foreach (var otherCharacter in OtherTeam)
            {
                otherCharacter.isBattle = false;
            }
            EnemyTeam.Clear();
            PlayerTeam.Clear();
            OtherTeam.Clear ();
            isBattleing=false;
            bool 战斗结果=false;
            EventHandler.CallEndBattle(战斗结果);
        }
        //Sort the team turn order based on TurnSorting.
        private void SortTeamOrder(bool updateListSize = false)
        {
            switch (turnSorting)
            {
                case TurnSorting.ConstantAttribute:
                    if (updateListSize)
                    {
                        combinedList = new List<Character>();
                        combinedList.AddRange(PlayerTeam.Where(x => x.isAlive).ToList());
                        combinedList.AddRange(EnemyTeam.Where(x => x.isAlive).ToList());
                        combinedList.AddRange(OtherTeam.Where(x => x.isAlive).ToList());
                        combinedList = combinedList.OrderByDescending(x => x.characterClass.Speed).ToList();
                    }
                    else
                    {
                        Character item = combinedList[0];
                        combinedList.RemoveAt(0);
                        if (combinedList.Count <= 0)
                        {
                            turnCount++;
                            SortTeamOrder(true);
                        }
                    }
                    break;
                case TurnSorting.CTB:
                    combinedList = new List<Character>();
                    combinedList.AddRange(PlayerTeam.Where(x => x.isAlive).ToList());
                    combinedList.AddRange(EnemyTeam.Where(x => x.isAlive).ToList());
                    combinedList = combinedList.OrderBy(x => x.characterClass.Speed).ToList();
                    // combinedList = combinedList.OrderBy(x => x.initiativeValue).ToList();
                    break;
                default:
                    break;
            }
            //发送队伍排序完成事件
            EventHandler.CallTurnOrderUpdated(combinedList.ToList());
        }

        public void StartBattle()
        {
            isBattleing = true;
            SortTeamOrder(true);
            if (combinedList.Where(x => x.isAlive).ToList().Count > 0)
            {
                var firsCharacter = combinedList.First();
                firsCharacter.StartTurn();
                //TODO 发送切换角色事件
                EventHandler.CallStartNewCharacterTurn(firsCharacter);
            }
        }

        //TODO 角色结束回合，更新回合并开始一个新的角色回合。
        public void OnEndTurn()
        {
            AttachTileEffects();
            var currentCharacter = combinedList.First();
            currentCharacter.isActive = false;
            if (currentCharacter.isAlive)
            {
                currentCharacter.ApplyEffects();
                foreach (var ability in currentCharacter.abilitiesForUse)
                {
                    ability.turnsSinceUsed++;
                }
                // if (currentCharacter.isAlive)
                // {
                //     currentCharacter.StartTurn();
                // }else{
                //     //角色因BUFF死亡
                // }
            }
            SortTeamOrder();
            //TODO 发送切换角色事件
            if(combinedList.Count>0){
                currentCharacter = combinedList.Where(x => x.isAlive).ToList().First();
                if(currentCharacter)
                {
                    currentCharacter.StartTurn();
                    EventHandler.CallStartNewCharacterTurn(currentCharacter);
                }
            }
        }

        // public void OnEndTurn()
        // {
        //     if (combinedList.Count > 0)
        //     {
        //         FinaliseEndCharactersTurn();
        //         SortTeamOrder();
        //         foreach (var character in combinedList)
        //             character.isActive = false;

        //         if (combinedList.Where(x => x.isAlive).ToList().Count > 0)
        //         {
        //             var firstCharacter = combinedList.First();

        //             if (firstCharacter.isAlive)
        //             {
        //                 firstCharacter.isActive = true;
        //                 firstCharacter.ApplyEffects();

        //                 if (firstCharacter.isAlive)
        //                 {
        //                     firstCharacter.StartTurn();
        //                     //TODO 发送切换角色事件
        //                     EventHandler.CallStartNewCharacterTurn(firstCharacter);
        //                 }
        //                 else
        //                     OnEndTurn();


        //                 foreach (var ability in firstCharacter.abilitiesForUse)
        //                 {
        //                     ability.turnsSinceUsed++;
        //                 }
        //             }
        //             else
        //             {
        //                 OnEndTurn();
        //             }
        //         }
        //     }
        // }

        /// <summary>
        /// 添加瓦片效果
        /// </summary>
        private void AttachTileEffects()
        {
            var characterEndingTurn = combinedList.First();

            if (characterEndingTurn.activeTile && characterEndingTurn.activeTile.tileData)
            {
                //添加瓦片效果
                var tileEffect = characterEndingTurn.activeTile.tileData.effect;
                if (tileEffect != null)
                    characterEndingTurn.AttachEffect(tileEffect);
            }
            // combinedList.First().UpdateInitiative(Constants.BaseCost);
        }


        //等待下一次循环以避免可能的竞争条件。
        IEnumerator DelayedSetActiveCharacter(Character firstCharacter)
        {
            yield return new WaitForFixedUpdate();
            //TODO 发送切换角色事件
            EventHandler.CallStartNewCharacterTurn(firstCharacter);
        }

        //TODO 新增一个角色到回合顺序
        public void SpawnNewCharacter(Character character)
        {
            Character newCharacter = character;
            if(newCharacter.teamID == Team.Player)
            {
                PlayerTeam.Add(newCharacter);
            }else if(EnemyTeam.Where(x=>x.teamID==newCharacter.teamID).Any()){
                EnemyTeam.Add(newCharacter);
            }else{
                OtherTeam.Add(newCharacter);
            }
            newCharacter.isBattle = true;
            combinedList.Add(newCharacter);
            EventHandler.CallTurnOrderUpdated(combinedList.ToList());
            // teamA.Add(character.GetComponent<CharacterManager>());
            // SortTeamOrder(true);
        }

        /// <summary>
        /// 获取PlayerTeam
        /// </summary>
        /// <returns></returns>
        public List<Character> GetPlayerTeam()
        {
            return PlayerTeam;
        }

        /// <summary>
        /// 获取EnemyTeam
        /// </summary>
        /// <returns></returns>
        public List<Character> GetEnemyTeam()
        {
            return EnemyTeam;
        }

        /// <summary>
        /// 移除一个角色
        /// </summary>
        /// <param name="character"></param>
        public void RemoveCharacter(Character character)
        {
            int removeCharacterIndex = combinedList.FindIndex(x => x.teamID == character.teamID && x.characterClass.characterID == character.characterClass.characterID);
            if (removeCharacterIndex > -1)
            {
                combinedList.RemoveAt(removeCharacterIndex);
            }
            removeCharacterIndex = PlayerTeam.FindIndex(x => x.teamID == character.teamID && x.characterClass.characterID == character.characterClass.characterID);
            if (removeCharacterIndex > -1)
            {
                PlayerTeam.RemoveAt(removeCharacterIndex);
            }
            removeCharacterIndex = EnemyTeam.FindIndex(x => x.teamID == character.teamID && x.characterClass.characterID == character.characterClass.characterID);
            if (removeCharacterIndex > -1)
            {
                EnemyTeam.RemoveAt(removeCharacterIndex);
            }
            removeCharacterIndex = OtherTeam.FindIndex(x => x.teamID == character.teamID && x.characterClass.characterID == character.characterClass.characterID);
            if (removeCharacterIndex > -1)
            {
                OtherTeam.RemoveAt(removeCharacterIndex);
            }
            if(BattleIsOver())
            {
                Debug.Log("战斗结束");
                EndBattle();
            }else{
                EventHandler.CallTurnOrderUpdated(combinedList.ToList());
            }
        }

        private bool BattleIsOver()
        {
            return PlayerTeam.Count==0||EnemyTeam.Count==0;
        }
    }

}
