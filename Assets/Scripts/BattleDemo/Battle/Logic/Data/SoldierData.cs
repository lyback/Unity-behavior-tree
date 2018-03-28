//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

using FSMFrame;
namespace Battle.Logic
{
    public class SoldierData
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
        public int state;
        //兵种类型
        public SoldierType type;

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

