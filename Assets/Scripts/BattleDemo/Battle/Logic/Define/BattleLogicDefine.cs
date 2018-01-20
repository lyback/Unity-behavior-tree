
namespace Battle.Logic
{
    public enum LogicState
    {
        Nothing = 0,
        Playing = 1,
        End = 2,
    }
    public class BattleLogicDefine
    {
        public static int logicSecFrame = 10; //每秒x个逻辑帧
    }
}