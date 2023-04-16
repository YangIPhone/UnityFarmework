namespace CQTacticsToolkit{
    //记录最后一次使用某个能力的时间
    [System.Serializable]
    public class AbilityContainer
    {
        public Ability ability;
        /// <summary>
        /// 大于等于冷却回合时可用(每次释放能力后归0)
        /// </summary>
        public int turnsSinceUsed;
        public bool canUse;

        public AbilityContainer(Ability ability)
        {
            this.ability = ability;
            turnsSinceUsed = 999;
            canUse = true;
        }
    }
}
