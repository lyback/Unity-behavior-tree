using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorGenericMenuBase
    {
        protected GenericMenu m_Menu;
        protected BTreeEditorWindow m_Window;

        public BTreeEditorGenericMenuBase(BTreeEditorWindow _window)
        {
            m_Menu = new GenericMenu();
            m_Window = _window;
        }

        public void AddDisabledItem(GUIContent content)
        {
            m_Menu.AddDisabledItem(content);
        }

        public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction func)
        {
            m_Menu.AddItem(content, on, func);
        }

        public void AddItem(GUIContent content, bool on, GenericMenu.MenuFunction2 func, object userData)
        {
            m_Menu.AddItem(content, on, func, userData);
        }

        public void AddSeparator(string path)
        {
            m_Menu.AddSeparator(path);
        }

        public void DropDown(Rect position)
        {
            m_Menu.DropDown(position);
        }

        public int GetItemCount()
        {
            return m_Menu.GetItemCount();
        }
        public virtual void ShowAsContext()
        {
            m_Menu.ShowAsContext();
        }
    }
}
