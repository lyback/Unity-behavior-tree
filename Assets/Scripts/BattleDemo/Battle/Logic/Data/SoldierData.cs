using FSM;

namespace Battle.Logic
{
    public class SoldierData : FSMDataBase
    {
        //索引
        public int line;
        public int row;
        //坐标(表现层数据，不用是整型)
        public float x;
        public float y;
        //行列间距
        public uint lineSpace;
        public uint rowSpace;
        //是否是进攻方
        public bool isAtkTroop;
        //部队状态
        public TroopAnimState state;
        //兵种类型
        public TroopType type;

        public SoldierData(TroopData troop, int _line, int _row)
        {
            line = _line;
            row = _row;
            type = troop.type;
            state = troop.state;
            isAtkTroop = troop.isAtkTroop;
            float dline = troop.line / 2.0f - 0.5f;
            float drow = troop.row / 2.0f - 0.5f;
            TroopHelper.GetTroopLRSpace(troop.type, out lineSpace, out rowSpace);
            x = (dline - line) * lineSpace;
            y = (row - drow) * rowSpace;

        }
    }
}

