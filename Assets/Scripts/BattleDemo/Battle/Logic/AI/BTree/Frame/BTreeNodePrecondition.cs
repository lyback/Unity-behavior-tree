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

    public class BTreeNodePreconditionTRUE : BTreeNodePrecondition
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
    public class BTreeNodePreconditionNOT : BTreeNodePrecondition
    {
        private BTreeNodePrecondition m_Precondition;
        public BTreeNodePreconditionNOT(BTreeNodePrecondition _precondition)
        {
            if (_precondition == null)
            {
                Debugger.Log("BTreeNodePreconditionNOT is null");
            }
            m_Precondition = _precondition;
        }
        public override bool ExternalCondition(BTreeInputData _input)
        {
            return m_Precondition.ExternalCondition(_input);
        }
    }
    public class BTreeNodePreconditionAND : BTreeNodePrecondition
    {
        private BTreeNodePrecondition m_Precondition1;
        private BTreeNodePrecondition m_Precondition2;
        public BTreeNodePreconditionAND(BTreeNodePrecondition _precondition1, BTreeNodePrecondition _precondition2)
        {
            if (_precondition1 == null || _precondition2 == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
            }
            m_Precondition1 = _precondition1;
            m_Precondition2 = _precondition2;
        }
        public override bool ExternalCondition(BTreeInputData _input)
        {
            return m_Precondition1.ExternalCondition(_input) && m_Precondition2.ExternalCondition(_input);
        }
    }
    public class BTreeNodePreconditionOR : BTreeNodePrecondition
    {
        private BTreeNodePrecondition m_Precondition1;
        private BTreeNodePrecondition m_Precondition2;
        public BTreeNodePreconditionOR(BTreeNodePrecondition _precondition1, BTreeNodePrecondition _precondition2)
        {
            if (_precondition1 == null || _precondition2 == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
            }
            m_Precondition1 = _precondition1;
            m_Precondition2 = _precondition2;
        }
        public override bool ExternalCondition(BTreeInputData _input)
        {
            return m_Precondition1.ExternalCondition(_input) || m_Precondition2.ExternalCondition(_input);
        }
    }
}
