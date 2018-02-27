using FSMFrame;
namespace Battle.Logic
{
    public class TroopData
    {
        //ID(不能等于0)
        public uint key;
        //行列
        public uint line;
        public uint row;
        //坐标
        public int x;
        public int y;
        //朝向
        public int dir_x;
        public int dir_y;
        //士兵数量
        public uint count;
        //普通攻击CD时间
        public uint norAtkCD;
        //攻击中
        public bool isAtk;
        //前置动作
        public bool inPrepose;
        //前置动作时间
        public uint preTime;
        //死亡停留时间
        public uint ghostTime;
        //是否处于战斗状态
        public bool inWar;
        //目标部队id
        public uint targetKey = 0;
        //是否是进攻方
        public bool isAtkTroop;
        //部队状态
        public int state;
        //兵种类型
        public SoldierType type;
        //是否有加入的部队
        public bool addTroop;
    }
}
