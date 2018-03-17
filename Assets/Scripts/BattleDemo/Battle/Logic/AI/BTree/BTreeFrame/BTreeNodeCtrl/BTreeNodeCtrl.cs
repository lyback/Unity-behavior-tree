

namespace BTreeFrame
{
    //选择节点基类
    public class BTreeNodeCtrl : BTreeNode
    {
        public BTreeNodeCtrl()
            : base()
        {

        }
        public BTreeNodeCtrl(BTreeNode _parentNode, BTreeNodePrecondition _precondition = null)
            : base(_parentNode, _precondition)
        {

        }
    }
}
