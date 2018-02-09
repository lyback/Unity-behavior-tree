using System;

public enum TroopFSMState
{
    End = 0,
    Start,
    FindTarget,
    Move,
    Attack,
}
public enum TroopAnimState
{
    Idle,
    Move,
    Attack,
    Die,
}