using BTreeFrame;
using System.Collections.Generic;

namespace Battle.Logic.AI.BTree
{
    public class FindTargetActionNode : BTreeNodeAction
    {
        public FindTargetActionNode()
            : base()
        {

        }
        public FindTargetActionNode(BTreeNode _parentNode) 
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

            List<TroopData> enemys;
            if (troop.isAtkTroop)
            {
                enemys = _input.battleData.mDefTroopList;
            }
            else
            {
                enemys = _input.battleData.mAtcTroopList;
            }
            //找最近的目标
            int minDis = -1;
            uint targetKey = 0;
            for (int i = 0; i < enemys.Count; i++)
            {
                int dis = MathHelper.TroopDistanceV2(troop, enemys[i]);
                if (minDis < 0)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
                else if (minDis > dis)
                {
                    minDis = dis;
                    targetKey = enemys[i].key;
                }
            }
            outTroop.targetKey = targetKey;
            return BTreeRunningStatus.Finish;
        }
    }
}
