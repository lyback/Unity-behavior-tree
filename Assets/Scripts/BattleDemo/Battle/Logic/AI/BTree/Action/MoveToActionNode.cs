using BTreeFrame;

namespace Battle.Logic.AI.BTree
{
    public class MoveToActionNode : BTreeNodeAction<MyInputData, MyOutputData>
    {
        public MoveToActionNode(BTreeNode<MyInputData, MyOutputData> _parentNode)
            : base(_parentNode)
        {
        }

        protected override BTreeRunningStatus _DoExecute(MyInputData _input, out MyOutputData _output)
        {
            _output = null;

            int dis = MathHelper.DistanceV2(_input.x, _input.y, _input.tar_x, _input.tar_y);
            if (dis<=0)
            {
                _output = new MyOutputData();
                _output.x = _input.tar_x;
                _output.y = _input.tar_y;
                return BTreeRunningStatus.Finish;
            }
            else
            {
                MoveToTarget(_input, out _output);
            }
            return BTreeRunningStatus.Executing;
        }

        private void MoveToTarget(MyInputData _input, out MyOutputData _output)
        {
            _output = new MyOutputData();
            var x = _input.x;
            var y = _input.y;
            var tar_x = _input.tar_x;
            var tar_y = _input.tar_y;

            if (x > tar_x)
            {
                _output.x = x - 1;
                if (_output.x < tar_x)
                {
                    _output.x = tar_x;
                }
            }
            else if (x < tar_x)
            {
                _output.x = x + 1;
                if (_output.x > tar_x)
                {
                    _output.x = tar_x;
                }
            }
            if (y > tar_y)
            {
                _output.y = y - 1;
                if (_output.y < tar_y)
                {
                    _output.y = tar_y;
                }
            }
            else if (y < tar_y)
            {
                _output.y = y + 1;
                if (_output.y > tar_y)
                {
                    _output.y = tar_y;
                }
            }
        }
    }

}
