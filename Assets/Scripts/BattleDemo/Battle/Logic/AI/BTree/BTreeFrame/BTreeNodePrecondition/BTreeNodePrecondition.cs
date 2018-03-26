
namespace BTreeFrame
{
    public abstract class BTreeNodePrecondition
    {
        public BTreeNodePrecondition() { }
        public abstract bool ExternalCondition(BTreeTemplateData _input);
    }

    public class BTreeNodePreconditionTRUE : BTreeNodePrecondition
    {
        public BTreeNodePreconditionTRUE() { }
        public override bool ExternalCondition(BTreeTemplateData _input)
        {
            return true;
        }
    }
    public class BTreeNodePreconditionFALSE : BTreeNodePrecondition
    {
        public BTreeNodePreconditionFALSE() { }
        public override bool ExternalCondition(BTreeTemplateData _input)
        {
            return false;
        }
    }

}
