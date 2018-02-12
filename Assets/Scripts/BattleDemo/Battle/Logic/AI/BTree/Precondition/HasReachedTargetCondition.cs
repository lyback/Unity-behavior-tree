using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class HasReachedTargetCondition : BTreeNodePrecondition<MyInputData>
    {
        public override bool ExternalCondition(MyInputData _input)
        {
            var troop = _input.troop;
            if (troop.targetKey != 0)
            {
                var target = _input.battleData.mAllTroopDic[troop.targetKey];
                var dis = MathHelper.DistanceV2(troop.x, troop.y, target.x, target.y);
                if (dis<=0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
