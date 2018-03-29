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
