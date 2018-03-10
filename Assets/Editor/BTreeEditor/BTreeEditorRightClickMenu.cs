using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BTreeFrame;

namespace BTree.Editor
{
    public class BTreeEditorRightClickBlockMenu<T, P>
        where T : BTreeTemplateData
        where P : BTreeTemplateData
    {
        private GenericMenu m_Menu;
        private BTreeEditorWindow m_Window;
        public BTreeEditorRightClickBlockMenu(BTreeEditorWindow _window)
        {
            m_Menu = new GenericMenu();
            m_Window = _window;
            Init();
        }
        public void Init()
        {
            List<Type> actionList = new List<Type>();
            List<Type> selectorList = new List<Type>();
            for (int i = 0; i < 4; i++)
            {
                Assembly assembly = null;
                try
                {
                    switch (i)
                    {
                        case 0:
                            assembly = Assembly.Load("Assembly-CSharp");
                            break;
                        case 1:
                            assembly = Assembly.Load("Assembly-CSharp-firstpass");
                            break;
                        case 2:
                            assembly = Assembly.Load("Assembly-UnityScript");
                            break;
                        case 3:
                            assembly = Assembly.Load("Assembly-UnityScript-firstpass");
                            break;
                    }
                }
                catch (Exception)
                {
                    assembly = null;
                }
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    for (int j = 0; j < types.Length; j++)
                    {
                        if (!types[j].IsAbstract)
                        {
                            if (types[j].IsSubclassOf(typeof(BTreeNodeAction<T, P>)))
                            {
                                actionList.Add(types[j]);
                            }
                            else if (IsSubclassOfRawGeneric(typeof(BTreeNode<,>),types[j]))
                            {
                                selectorList.Add(types[j]);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < actionList.Count; i++)
            {
                AddItem(new GUIContent("Add Task/Action/" + actionList[i].Name), false, new GenericMenu.MenuFunction2(AddNodeCallback), actionList[i]);
            }
            for (int i = 0; i < selectorList.Count; i++)
            {
                AddItem(new GUIContent("Add Task/Selector/" + selectorList[i].Name), false, new GenericMenu.MenuFunction2(AddNodeCallback), selectorList[i]);
            }
        }

        private void AddNodeCallback(object obj)
        {
            m_Window.addNodeCallback(obj);
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
        public void ShowAsContext()
        {
            m_Menu.ShowAsContext();
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
