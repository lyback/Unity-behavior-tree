namespace Battle.Logic.AI.FSM
{
    public enum TroopFSMState
    {
        End = 0,
        Start,
        FindTarget,
        Move,
        Attack,
    }
    //需要和BattleViewDefine中的TroopAnimState一一对应
    public enum TroopAnimState
    {
        Idle = 1,
        Move = 2,
        Attack = 3,
        Die = 4,
    }
}