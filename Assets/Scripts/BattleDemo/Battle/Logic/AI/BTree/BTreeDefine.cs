using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle.Logic.AI.BTree
{
    public enum BTreeRunningStatus
    {
        Error = 0,
        Executing = 1,
        Finish = 2,
    }
    public enum BTreeBevNodeStatus
    {
        Ready = 1,
        Running = 2,
        Finish = 3,
    }

}
