using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Logic.AI.BTree
{
    public abstract class BTreeNodePrecondition
    {
        public abstract bool ExternalCondition(BTreeInputData _input);
    }
    public class BTreeNodePreconditionTRUE: BTreeNodePrecondition
    {
        public override bool ExternalCondition(BTreeInputData _input)
        {
            return true;
        }
    }
    public class BTreeNodePreconditionFALSE : BTreeNodePrecondition
    {
        public override bool ExternalCondition(BTreeInputData _input)
        {
            return false;
        }
    }
}
