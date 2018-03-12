

namespace BTreeFrame
{
    public abstract class BTreeNodePrecondition
    {
        public BTreeNodePrecondition(){}
        public abstract bool ExternalCondition(BTreeTemplateData _input);
    }

    public class BTreeNodePreconditionTRUE : BTreeNodePrecondition
    {
        public override bool ExternalCondition(BTreeTemplateData _input)
        {
            return true;
        }
    }
    public class BTreeNodePreconditionFALSE : BTreeNodePrecondition
    {
        public override bool ExternalCondition(BTreeTemplateData _input)
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
        public override bool ExternalCondition(BTreeTemplateData _input)
        {
            return !m_Precondition.ExternalCondition(_input);
        }
        public BTreeNodePrecondition GetChildPrecondition()
        {
            return m_Precondition;
        }
    }
    public class BTreeNodePreconditionAND : BTreeNodePrecondition
    {
        private BTreeNodePrecondition[] m_Preconditions;
        public BTreeNodePreconditionAND(params BTreeNodePrecondition[] param)
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
        public override bool ExternalCondition(BTreeTemplateData _input)
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
        public BTreeNodePrecondition[] GetChildPrecondition()
        {
            return m_Preconditions;
        }
        public int GetChildPreconditionCount()
        {
            if (m_Preconditions != null)
            {
                return m_Preconditions.Length;
            }
            return 0;
        }
    }
    public class BTreeNodePreconditionOR : BTreeNodePrecondition
    {
        private BTreeNodePrecondition[] m_Preconditions;
        public BTreeNodePreconditionOR(params BTreeNodePrecondition[] param)
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
        public override bool ExternalCondition(BTreeTemplateData _input)
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
        public BTreeNodePrecondition[] GetChildPrecondition()
        {
            return m_Preconditions;
        }
        public int GetChildPreconditionCount()
        {
            if (m_Preconditions != null)
            {
                return m_Preconditions.Length;
            }
            return 0;
        }
    }
}
