

namespace BTreeFrame
{
    public abstract class BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        public abstract bool ExternalCondition(T _input);
    }

    public class BTreeNodePreconditionTRUE<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        public override bool ExternalCondition(T _input)
        {
            return true;
        }
    }
    public class BTreeNodePreconditionFALSE<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        public override bool ExternalCondition(T _input)
        {
            return false;
        }
    }
    public class BTreeNodePreconditionNOT<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        private BTreeNodePrecondition<T> m_Precondition;
        public BTreeNodePreconditionNOT(BTreeNodePrecondition<T> _precondition)
        {
            if (_precondition == null)
            {
                Debugger.Log("BTreeNodePreconditionNOT is null");
            }
            m_Precondition = _precondition;
        }
        public override bool ExternalCondition(T _input)
        {
            return m_Precondition.ExternalCondition(_input);
        }
    }
    public class BTreeNodePreconditionAND<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        private BTreeNodePrecondition<T> m_Precondition1;
        private BTreeNodePrecondition<T> m_Precondition2;
        public BTreeNodePreconditionAND(BTreeNodePrecondition<T> _precondition1, BTreeNodePrecondition<T> _precondition2)
        {
            if (_precondition1 == null || _precondition2 == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
            }
            m_Precondition1 = _precondition1;
            m_Precondition2 = _precondition2;
        }
        public override bool ExternalCondition(T _input)
        {
            return m_Precondition1.ExternalCondition(_input) && m_Precondition2.ExternalCondition(_input);
        }
    }
    public class BTreeNodePreconditionOR<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        private BTreeNodePrecondition<T> m_Precondition1;
        private BTreeNodePrecondition<T> m_Precondition2;
        public BTreeNodePreconditionOR(BTreeNodePrecondition<T> _precondition1, BTreeNodePrecondition<T> _precondition2)
        {
            if (_precondition1 == null || _precondition2 == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
            }
            m_Precondition1 = _precondition1;
            m_Precondition2 = _precondition2;
        }
        public override bool ExternalCondition(T _input)
        {
            return m_Precondition1.ExternalCondition(_input) || m_Precondition2.ExternalCondition(_input);
        }
    }
}
