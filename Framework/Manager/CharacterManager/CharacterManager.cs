using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ChangQFramework{
    //角色属性类
    public class Character{
        //名字
        public string name;
        //等级
        public int level = 1;
        //当前经验
        public int currentExp=0;
        //升级经验
        public int nextLevelExp{
            get{
                return level*50;
            }
        }
        //当前血量
        public int currentHp = 100;
        //最大血量
        public int maxHp = 100;
        //当前蓝量
        public int currentMp = 100;
        //最大蓝量
        public int maxMp = 100;
        //装备(对应物品ID,-1表示未装备)
        public int weaponID = -1;//武器
        public int clothesID = -1;//防具
        public int shoesID = -1;//鞋子
        //技能列表(对应技能ID)
        public List<int> skillList = new List<int>();
        //TODO 其他属性
    }

    public class CharacterManager 
    {
        public static CharacterManager instance;
        public static CharacterManager Instance{
            get{
                if(instance == null){
                    Debug.Log("创建实例");
                    instance = new CharacterManager();
                }
                return instance;
            }
        }
        //玩家
        public List<Character> characterList = new List<Character>();

        #region 公用属性
        //金钱
        public int Money = 0;
        //当前角色是否可控制
        public bool canControl = true;
        #endregion

        #region 人物属性操作
        //增减金钱
        public void AddMoney(int money){
            Money+=money;
            if(Money <= 0){
                Money = 0;
            } 
        }

        //给index号角色增加经验
        public void AddExp(int exp,int index){
            characterList[index].currentExp += exp;
            if(characterList[index].currentExp >= characterList[index].nextLevelExp){
                //升级
                characterList[index].level ++;
                characterList[index].currentExp -= characterList[index].nextLevelExp;
            }
        }

        //给index号角色增加最大血量
        public void AddHp(int maxHp,int index){
            characterList[index].maxHp += maxHp;
        }
        
        //给index号角色恢复/减少血量
        public void RestoreHp(int Hp,int index){
            characterList[index].currentHp += Hp;
            //将当前血量限制在0和最大值直接
            characterList[index].currentHp = Mathf.Clamp(characterList[index].currentHp,0,characterList[index].maxHp);
        }
        #endregion

        #region  人物技能
        //添加技能
        public bool AddSkill(int skillID,int index){
            if(characterList[index].skillList.Contains(skillID)){
                return false;
            }
            characterList[index].skillList.Add(skillID);
            return true;
        }

        //是否拥有此技能
        public bool HasSkill(int skillID,int index){
            return characterList[index].skillList.Contains(skillID);
        }

        //移除技能
        public void RemoveSkill(int skillID,int index){
            if(characterList[index].skillList.Contains(skillID)){
                characterList[index].skillList.Remove(skillID);
            }
        }

        //获取角色所有技能
        public int[] GetSkills(int index){
            return characterList[index].skillList.ToArray();
        }
        #endregion

        #region 人物装备
        public  void EquipWeapon(int EquipID,int index){
            InventoryItem equipItem = Managers.m_Inventory.GetItem(EquipID);
            //如果背包有这个武器才允许装备
            if(equipItem != null){
                Managers.m_Inventory.RemoveItem(EquipID);
            }
            //如果当前有装备武器，卸载下武器放入背包
            if(characterList[index].weaponID > -1){
                Managers.m_Inventory.AddItem(characterList[index].weaponID,1);
            }
            characterList[index].weaponID = EquipID;
        }

        //得到index号角色当前装备武器的ID
        public int GetWeaponID(int index){
            return characterList[index].weaponID;
        }
        //卸载index号角色装备的武器
        public void RemoveEquipWeapon(int index){
            if(characterList[index].weaponID > -1){
                Managers.m_Inventory.AddItem(characterList[index].weaponID,1);
                characterList[index].weaponID = -1;
            }
        }
        #endregion
    }
}