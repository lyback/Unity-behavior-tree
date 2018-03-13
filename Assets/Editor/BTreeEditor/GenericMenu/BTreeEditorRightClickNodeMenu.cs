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
