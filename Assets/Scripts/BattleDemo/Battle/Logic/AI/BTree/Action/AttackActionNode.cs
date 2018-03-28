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

using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class AttackActionNode : BTreeNodeAction
    {
        public AttackActionNode()
            : base()
        {
        }
        public AttackActionNode(BTreeNode _parentNode) 
            : base(_parentNode)
        {
        }

        protected override BTreeRunningStatus _DoExecute(BTreeTemplateData input, ref BTreeTemplateData output)
        {
            BTreeInputData _input = input as BTreeInputData;
            BTreeOutputData _output = output as BTreeOutputData;
            if (_input == null || _output == null)
            {
                Debugger.LogError("数据类型错误");
            }

            var troop = _input.troop;
            var outTroop = _output.troop;
            var target = _input.battleData.mAllTroopDic[troop.targetKey];
            var tar_x = target.x;
            var tar_y = target.y;

            outTroop.dir_x = tar_x;
            outTroop.dir_y = tar_y;

            outTroop.state = (int)TroopAnimState.Attack;
            outTroop.inPrepose = true;
            outTroop.preTime = TroopHelper.GetTroopAtkPrepareTime(outTroop.type);
            outTroop.isAtk = true;
            outTroop.norAtkCD = TroopHelper.GetTroopAtkCDTime(outTroop.type);
            return BTreeRunningStatus.Finish;
        }
    }
}
