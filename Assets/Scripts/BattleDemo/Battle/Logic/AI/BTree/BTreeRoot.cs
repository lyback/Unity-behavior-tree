//////////////////////////////////////////////////////////////////////////////////////
// The MIT License(MIT)
// Copyright(c) 2018 lycoder

// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

//////////////////////////////////////////////////////////////////////////////////////

using BTreeFrame;
namespace Battle.Logic.AI.BTree
{
    public class BTreeRoot
    {
        public BTreeNode m_TreeRoot { get; private set; }

        public void CreateBevTree()
        {
            //m_TreeRoot = BTreeNodeFactory<MyInputData, MyOutputData>.CreateParallelNode(null, "Root_ParallelNode", BTreeParallelFinishCondition.AND);
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<StartActionNode>(m_TreeRoot, "Start");
            //var troopAction = BTreeNodeFactory<MyInputData, MyOutputData>.CreatePrioritySelectorNode(m_TreeRoot, "TroopAction");
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<FindTargetActionNode>(troopAction, "FindTarget")
            //    .SetNodePrecondition(new BTreeNodePreconditionNOT<MyInputData>(new HasTargetCondition()));
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<AttackActionNode>(troopAction, "Attack")
            //    .SetNodePrecondition(new BTreeNodePreconditionAND<MyInputData>(new HasTargetCondition(), new IsInAttackRangeCondition()));
            //BTreeNodeFactory<MyInputData, MyOutputData>.CreateActionNode<MoveToActionNode>(troopAction, "MoveTo")
            //    .SetNodePrecondition(new HasTargetCondition());
           

            //BTreeNodeSerialization.WriteBinary(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadBinary(config.m_Name);

            //BTreeNodeSerialization.WriteXML(config, config.m_Name);
            //var _config = BTreeNodeSerialization.ReadXML("Btree");
            var _config = BTreeNodeSerialization.ReadXML("Btree_test");
            _config = BTreeNodeSerialization.ReadBinary("Root_ParallelNode");
            
            BTreeNodeFactory.Init();
            m_TreeRoot = BTreeNodeFactory.CreateBTreeRootFromConfig(_config);
            //var _testconfig = BTreeNodeFactory<BTreeInputData, BTreeOutputData>.CreateConfigFromBTreeRoot(m_TreeRoot);
            //BTreeNodeSerialization.WriteXML(_testconfig, "Btree_test");
        }
        public void Tick(BTreeTemplateData _input, ref BTreeTemplateData _output)
        {
            Debugger.Log_Btree("****************Tick_Begin****************");
            if (m_TreeRoot.Evaluate(_input))
            {
                m_TreeRoot.Tick(_input, ref _output);
            }
            else
            {
                m_TreeRoot.Transition(_input);
            }
            Debugger.Log_Btree("****************Tick_End****************");

        }
    }
    
}
