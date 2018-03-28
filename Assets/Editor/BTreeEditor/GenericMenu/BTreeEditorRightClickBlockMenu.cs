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

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BTreeFrame;

namespace BTree.Editor
{
    public class BTreeEditorRightClickBlockMenu : BTreeEditorGenericMenuBase
    {
        public BTreeEditorRightClickBlockMenu(BTreeEditorWindow _window)
            : base(_window)
        {
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
                            if (types[j].IsSubclassOf(typeof(BTreeNodeAction)))
                            {
                                actionList.Add(types[j]);
                            }
                            else if (types[j].IsSubclassOf(typeof(BTreeNodeCtrl)))
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
        
    }
}
