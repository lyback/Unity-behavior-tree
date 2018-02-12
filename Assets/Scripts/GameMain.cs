using Neatly.Timer;
using UnityEngine;
using Battle.Logic;
public class GameMain : MonoBehaviour
{

    public int speed = 10;
    // Use this for initialization
    void Awake()
    {
        //Application.targetFrameRate = 60;
        NeatlyTimer.Init();
        MainBattleManager.Instance.Start();
        MainBattleManager.Instance.InitLogicTime(1 / BattleLogicDefine.logicSecFrame);
    }
    void Start()
    {
        MainBattleManager.Instance.SetSpeed(speed);
    }
    void Update()
    {
        //if (MainBattleManager.Instance.m_Speed != speed)
        //{
        //    MainBattleManager.Instance.SetSpeed(speed);
        //}
    }
}
