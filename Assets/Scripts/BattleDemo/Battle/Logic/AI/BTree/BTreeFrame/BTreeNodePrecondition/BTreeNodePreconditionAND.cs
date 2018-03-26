
namespace BTreeFrame
{
    public class BTreeNodePreconditionAND : BTreeNodePrecondition
    {
        private BTreeNodePrecondition[] m_Preconditions;
        public BTreeNodePreconditionAND() { }
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
        public void SetChildPrecondition(params BTreeNodePrecondition[] param)
        {
            m_Preconditions = param;
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
