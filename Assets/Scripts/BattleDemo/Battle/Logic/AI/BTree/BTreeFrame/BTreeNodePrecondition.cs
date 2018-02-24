

namespace BTreeFrame
{
    public abstract class BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        public BTreeNodePrecondition(){}
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
            return !m_Precondition.ExternalCondition(_input);
        }
    }
    public class BTreeNodePreconditionAND<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        private BTreeNodePrecondition<T>[] m_Preconditions;
        public BTreeNodePreconditionAND(params BTreeNodePrecondition<T>[] param)
        {
            if (param == null)
            {
                Debugger.Log("BTreeNodePreconditionAND is null");
                return;
            }
            if (param.Length == 0)
            {
                Debugger.Log("BTreeNodePreconditionAND's length is 0");
                return;
            }
            m_Preconditions = param;
        }
        public override bool ExternalCondition(T _input)
        {
            for (int i = 0; i < m_Preconditions.Length; i++)
            {
                if (!m_Preconditions[i].ExternalCondition(_input))
                {
                    return false;
                }
            }
            return true;
        }
    }
    public class BTreeNodePreconditionOR<T> : BTreeNodePrecondition<T> where T : BTreeTemplateData
    {
        private BTreeNodePrecondition<T>[] m_Preconditions;
        public BTreeNodePreconditionOR(params BTreeNodePrecondition<T>[] param)
        {
            if (param == null)
            {
                Debugger.Log("BTreeNodePreconditionOR is null");
                return;
            }
            if (param.Length == 0)
            {
                Debugger.Log("BTreeNodePreconditionOR's length is 0");
                return;
            }
            m_Preconditions = param;
        }
        public override bool ExternalCondition(T _input)
        {
            for (int i = 0; i < m_Preconditions.Length; i++)
            {
                if (m_Preconditions[i].ExternalCondition(_input))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
