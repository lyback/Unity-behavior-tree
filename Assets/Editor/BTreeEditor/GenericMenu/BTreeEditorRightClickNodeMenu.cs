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
        
        public void ShowAsContext(bool isMult, bool isEnable)
        {
            m_Menu = new GenericMenu();
            if (!isMult)
            {
                if (isEnable)
                {
                    AddItem(new GUIContent("Disable"), false, new GenericMenu.MenuFunction(DisableCallback));
                }
                else
                {
                    AddItem(new GUIContent("Enable"), false, new GenericMenu.MenuFunction(EnableCallback));
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
    }
}
