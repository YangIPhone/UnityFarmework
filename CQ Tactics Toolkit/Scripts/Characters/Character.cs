using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using CQFramework;

namespace CQFramework.CQTacticsToolkit
{
    [RequireComponent(typeof(SortingGroup),typeof(CharacterClass))]
    public class Character : MonoBehaviour
    {
        //TODO 完善Entity
        [Header("角色阵营队伍")]
        public Team teamID;
        public bool isMoving = false;
        public bool isBattle = false;
        [Header("血条")]
        public Image healthBar;
        [Header("蓝条")]
        public Image manaBar;
        public float healthPercentage;
        public float ManaPercentage;
        [Header("角色BUFF列表")]
        public List<BuffEffectContainer> effects;
        [Header("角色技能列表")]
        public List<AbilityContainer> abilitiesForUse;
        public CharacterClass characterClass => GetComponent<CharacterClass>();
        // [HideInInspector]
        public OverlayTile activeTile;//人物所在的瓦片
        [HideInInspector]
        public bool isAlive = true;//是否存活
        public bool isActive;//是否是行动角色
        public Vector2Int Direction;
        [HideInInspector]
        public SortingGroup myRenderer => gameObject.GetComponent<SortingGroup>();
        private float moveSpeed=10f;
        private List<OverlayTile> characterPath = new List<OverlayTile>();
        // [HideInInspector]
        // public int previousTurnCost = -1;
        private bool isTargetted = false;//是否成为了攻击目标
        // private PathFinder pathFinder = new PathFinder();
        // private RangeFinder rangeFinder = new RangeFinder();
        // Start is called before the first frame update
        void Awake()
        {
            effects = new List<BuffEffectContainer>();
            SetAbilityList();
        }
        public void Update()
        {
            if (isTargetted)
            {
                //TODO 当角色成为攻击目标时用的一个彩色Lerp。
            }
            if (characterPath.Count > 0 && isMoving)
            {
                MoveAlongPath();
            }
        }
        
        public void SetAbilityList()
        {
            if(characterClass.abilities.Count>0)
            {
                abilitiesForUse = new List<AbilityContainer>();
                foreach (var ability in characterClass.abilities)
                {
                    abilitiesForUse.Add(new AbilityContainer(ability));
                }
            }
        }
        /// <summary>
        /// 人物升级增加随机属性
        /// </summary>
        public void LevelUpStats()
        {
            CameraController.Shake(0.125f, 0.1f);
            if((int)characterClass.level >= System.Enum.GetNames(characterClass.level.GetType()).Length-1){
                return;
            }
            float v = (float)++characterClass.level;
            // Debug.Log($"当前等级:{(int)characterClass.level},下一级所需经验：{gameConfig.GetRequiredExp((int)characterClass.level)}");
            characterClass.MaxHealth += Mathf.RoundToInt(characterClass.characterGrow.HealthGrow.Evaluate(v) * 10);
            characterClass.MaxMana += Mathf.RoundToInt(characterClass.characterGrow.ManaGrow.Evaluate(v) * 10);
            characterClass.Strenght += Mathf.RoundToInt(characterClass.characterGrow.StrenghtGrow.Evaluate(v) * 10);
            characterClass.Endurance += Mathf.RoundToInt(characterClass.characterGrow.EnduranceGrow.Evaluate(v) * 10);
            characterClass.Speed += Mathf.RoundToInt(characterClass.characterGrow.SpeedGrow.Evaluate(v) * 10);
            characterClass.MaxActionPoint+=1;
            // 升级回满体力和灵力
            characterClass.CurrentHealth = characterClass.MaxHealth;
            characterClass.CurrentMana = characterClass.MaxMana;
            UpdateHealth();
            UpdateMana();
            //TODO 播放升级特效
        }

        //人物是否正在成为攻击目标. 
        public void SetTargeted(bool focused = false)
        {
            isTargetted = focused;
        }

        // public void UpdateInitiative(int turnValue)
        // {
        //     initiativeValue += Mathf.RoundToInt(turnValue / GetStat(Stats.Speed).statValue + 1);
        //     previousTurnCost = turnValue;
        // }

        /// <summary>
        /// 受到攻击&技能&BUFF的伤害。
        /// </summary>
        /// <param name="damage">受到的伤害</param>
        /// <param name="ignoreDefence">是否无视防御</param>
        public void TakeDamage(int damage, bool ignoreDefence = false)
        {
            //TODO TakeDamage
            int damageToTake = ignoreDefence ? damage : CalculateDamage(damage);
            if (damageToTake > 0)
            {
                characterClass.CurrentHealth -= damageToTake;
                CameraController.Shake(0.125f, 0.1f);
                UpdateHealth();
                if (characterClass.CurrentHealth <= 0)
                {
                    if(isBattle)
                    {
                        isAlive = false;
                        StartCoroutine(Die());
                        UnlinkCharacterToTile();
                        //TODO 人物是当前回合行动人物，因BUFF伤害/友伤死亡，发送结束回合事件
                        if (isActive) EventHandler.CallEndTurn();
                    }else{//非战斗状态，受伤生命值最小为1
                        characterClass.CurrentHealth = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 恢复&减少血量
        /// </summary>
        /// <param name="value">恢复&减少的值</param>
        public void HealHealth(int value)
        {
            characterClass.CurrentHealth += value;
            UpdateHealth();
        }
        /// <summary>
        /// 恢复&减少蓝量
        /// </summary>
        /// <param name="value">恢复&减少蓝量</param>
        public void HealMana(int value)
        {
            characterClass.CurrentMana += value;
            UpdateMana();
        }

        private void UpdateHealth()
        {
            healthPercentage = (float)characterClass.CurrentHealth / (float)characterClass.MaxHealth;
            //TODO 更新血条
            // healthBar.fillAmount = healthPercentage;
            if (isActive && teamID == Team.Player)
            {
                EventHandler.CallUpdateHealthBar(healthPercentage);
            }
        }
        
        public void UpdateMana()
        {
            ManaPercentage = (float)characterClass.CurrentMana / (float)characterClass.MaxMana;
            //TODO 更新蓝条
            // manaBar.fillAmount =ManaPercentage;
            if (isActive&&teamID==Team.Player)
            {
                EventHandler.CallUpdateManaBar(ManaPercentage);
            }
        } 
        
        /// <summary>
        /// 增减行动点
        /// </summary>
        /// <param name="value">增减量</param>
        public void UpdateActionPoint(int value) 
        {
            characterClass.CurrentActionPoint = Mathf.Clamp(characterClass.CurrentActionPoint+value,0,characterClass.MaxActionPoint);
            if (isActive && teamID == Team.Player)
            {
                EventHandler.CallUpdateActionPoint(characterClass.CurrentActionPoint);
            }
        }


        //使用防御属性的基本示例
        private int CalculateDamage(int damage)
        {
            //减伤百分比
            float percentage = (((float)GetStat<int>("Endurance") / (float)damage) * 100) / 2;
            percentage = percentage > 75 ? 75 : percentage;
            int damageToTake = damage - Mathf.CeilToInt((float)(percentage / 100f) * (float)damage);
            return damageToTake;
        }

        //获取一个特定的stat对象。
        public T GetStat<T>(string statName)
        {
            return characterClass.GetAttribute<T>(statName);
        }
        public IEnumerator Die()
        {
            float DegreesPerSecond = 360f;
            Vector3 currentRot, targetRot = new Vector3();
            currentRot = transform.eulerAngles;
            targetRot.z = currentRot.z + 90; // calculate the new angle

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            while (currentRot.z < targetRot.z)
            {
                currentRot.z = Mathf.MoveTowardsAngle(currentRot.z, targetRot.z, DegreesPerSecond * Time.deltaTime);
                transform.eulerAngles = currentRot;
                yield return null;
            }
            //TODO 播放死亡动画
            Debug.Log($"角色{characterClass.characterName}死亡");
            TurnBasedController.Instance.RemoveCharacter(this);
        }

        /// <summary>
        /// 从瓦片或技能中附加一个buff效果。
        /// </summary>
        /// <param name="BuffEffect">要应用的BUFF</param>
        public void AttachEffect(BuffEffect BuffEffect)
        {
            if (BuffEffect)
            {
                if(BuffEffect.Duration == 0)
                {
                    ApplySingleEffects(BuffEffect);
                }else{
                    BuffEffectContainer buff = effects.Find(x => x.BuffID == BuffEffect.BuffID && x.BuffName == BuffEffect.BuffName);
                    //如果当前BUFF列表存在相同BUFF，则增加持续回合
                    if(buff !=null)
                    {
                        buff.DurationLengthen(BuffEffect.Duration);
                    }else{
                        //反之则新增一个BUFF
                        buff = new BuffEffectContainer(BuffEffect,BuffEffect.BuffID,BuffEffect.BuffName,BuffEffect.Duration);
                        effects.Add(buff);
                    }
                }
            }
        }

        /// <summary>
        /// 减少BUFF持续回合
        /// </summary>
        /// <param name="BuffEffect"></param>
        /// <param name="Duration"></param>
        /// <returns>BUFF剩余回合</returns>
        public int DecreaseEffectDuration(BuffEffect BuffEffect,int Duration)
        {
            if (BuffEffect)
            {
                BuffEffectContainer buff = effects.Find(x => x.BuffID == BuffEffect.BuffID && x.BuffName == BuffEffect.BuffName);
                if (buff != null)
                {
                    Duration = Mathf.Min(buff.Duration , Duration);
                    int SurplusDuration = buff.DurationLengthen(-Duration);
                    return SurplusDuration;
                    //如果持续回合小于等于0，移除BUFF
                    // if(SurplusDuration<=0)
                    // {
                    //     effects.Remove(buff);
                    // }
                }
            }
            return -1;
        }

        /// <summary>
        /// 没有持续时间的buff效果直接应用(没有加入BUFF列表中)
        /// </summary>
        /// <param name="selectedStat"></param>
        public void ApplySingleEffects(BuffEffect BuffEffect)
        {
            if (BuffEffect)
            {
                BuffEffect.ApplyBuff(this);
            }
            UpdateHealth();
        }

        /// <summary>
        /// /应用所有当前附加的效果
        /// </summary>
        public void ApplyEffects()
        {
            for (int i = 0; i < effects.Count;)
            {
                if(!isAlive) return;
                effects[i].buffEffect.ApplyBuff(this);
                int SurplusDuration = DecreaseEffectDuration(effects[i].buffEffect,1);
                if(SurplusDuration <= 0){
                    effects.Remove(effects[i]);
                }else{
                    i++;
                }
            }
            UpdateHealth();
        }

        /// <summary>
        /// 通过名字和ID获取技能
        /// </summary>
        /// <param name="abilityName"></param>
        /// <returns></returns>
        public AbilityContainer GetAbilityByName(int AbilityID, string abilityName)
        {
            return abilitiesForUse.Find(x =>x.ability.AbilityID == AbilityID&&x.ability.AbilityName == abilityName);
        }

        public virtual void StartTurn()
        {
            isActive = true;
            healthPercentage = (float)characterClass.CurrentHealth / (float)characterClass.MaxHealth;
            ManaPercentage = (float)characterClass.CurrentMana / (float)characterClass.MaxMana;
            characterClass.CurrentActionPoint = characterClass.CurrentActionPoint + (int)characterClass.level + OverlayController.Instance.gameConfig.AugmentActionPointCount;
            characterClass.CurrentActionPoint = Mathf.Clamp(characterClass.CurrentActionPoint, 0, characterClass.MaxActionPoint);
        }
        public void SetPath(List<OverlayTile> pathToFollow)
        {
            characterPath.Clear();
            isMoving = true;
            if (pathToFollow.Count > 0) characterPath = pathToFollow;
        }
        private void MoveAlongPath()
        {
            // var zIndex = characterPath[0].transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, characterPath[0].gridWorldPosition, moveSpeed * Time.deltaTime);
            // if (Vector3.Distance(transform.position, characterPath[0].transform.position) < 0.0001f)
            if ((transform.position - characterPath[0].gridWorldPosition).sqrMagnitude < 0.0001f)
            {
                PositionCharacterOnTile(characterPath[0]);
                characterPath.RemoveAt(0);
            }
            else
            {
                SetDirection(characterPath[0].gridWorldPosition, transform.position);
            }
            if (characterPath.Count == 0)
            {
                ResetMovementManager();
            }
        }

        //当移动完成或取消时重置移动模式。
        private void ResetMovementManager()
        {
            isMoving = false;
            OverlayController.Instance.ClearTiles(null);
            CharacterMoved();
        }

        /// <summary>
        /// 角色移动完成
        /// </summary>
        public virtual void CharacterMoved()
        {

        }
        public void SetDirection(Vector3 targetPos,Vector3 currentPos)
        {
            Vector3 tempDirection = (targetPos-currentPos).normalized;
            Direction = new Vector2Int(Mathf.RoundToInt(tempDirection.x),Mathf.RoundToInt(tempDirection.y));
        }
        public void SetSortLayer(string sortingLayerName, int sortingOrder)
        {
            myRenderer.sortingLayerName = sortingLayerName;
            myRenderer.sortingOrder = sortingOrder;
        }

        public void PositionCharacterOnTile(OverlayTile tile)
        {
            if (tile != null)
            {
                transform.position = new Vector3(tile.gridWorldPosition.x, tile.gridWorldPosition.y + 0.0001f, tile.gridWorldPosition.z);
                LinkCharacterToTile(tile);
            }
        }

        //当一个实体移动时，将它链接到它所站的瓷砖上。
        public void LinkCharacterToTile(OverlayTile tile)
        {
            UnlinkCharacterToTile();
            tile.activeCharacter = this;
            tile.isBlocked = true;
            activeTile = tile;
            SetSortLayer(tile.sortingLayerName,tile.sortingOrder+1);
        }

        //将一个实体从它所在的前一个tile中断开链接。
        public void UnlinkCharacterToTile()
        {
            if (activeTile!=null)
            {
                activeTile.activeCharacter = null;
                activeTile.isBlocked = activeTile.isOriginBlocked;//如果初始化时是障碍物，人物断开连接时设置回去
                activeTile = null;
            }
        }
    }
}
