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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorRightClickNodeMenu : BTreeEditorGenericMenuBase
    {
        public BTreeEditorRightClickNodeMenu(BTreeEditorWindow _window)
            :base(_window)
        {
        }
        
        public void ShowAsContext(List<BTreeNodeDesigner> _selectNodes)
        {
            bool isMult = _selectNodes.Count != 1; ;
            bool isDisable = _selectNodes[0].m_IsDisable;
            bool isAction = _selectNodes[0].m_EditorNode.m_Node.m_IsAcitonNode;
            bool isEntry = _selectNodes[0].m_ParentNode == null && !_selectNodes[0].m_IsEntryDisplay;
            m_Menu = new GenericMenu();
            if (!isMult)
            {
                if (!isAction)
                {
                    AddItem(new GUIContent("Make OutGoing Connection"), false, new GenericMenu.MenuFunction(ConnectionCallback));
                }
                if (isEntry)
                {
                    AddItem(new GUIContent("Set As EntryNode"), false, new GenericMenu.MenuFunction(SetEntryNodeCallback));
                }
                if (isDisable)
                {
                    AddItem(new GUIContent("Enable"), false, new GenericMenu.MenuFunction(EnableCallback));
                }
                else
                {
                    AddItem(new GUIContent("Disable"), false, new GenericMenu.MenuFunction(DisableCallback));
                }
            }
            AddItem(new GUIContent("Delect Node"), false, new GenericMenu.MenuFunction(DelectCallback));
            base.ShowAsContext();
        }

        private void DisableCallback()
        {
            m_Window.disableNodeCallback();
        }
        private void EnableCallback()
        {
            m_Window.enableNodeCallback();
        }
        private void DelectCallback()
        {
            m_Window.delectNodeCallback();
        }
        private void ConnectionCallback()
        {
            m_Window.connectLineCallback();
        }
        private void SetEntryNodeCallback()
        {
            m_Window.setEntryNodeCallback();
        }
    }
}
