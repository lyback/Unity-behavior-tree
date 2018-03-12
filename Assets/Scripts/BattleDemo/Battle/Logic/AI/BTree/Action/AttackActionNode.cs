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
