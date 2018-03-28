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
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BTreeFrame;

namespace BTree.Editor
{
    public class BTreeEditorNodeInspector
    {
        private UnityEngine.Object[] m_Precondition = new UnityEngine.Object[20];

        private const int SPACEDETLE = 10;

        public void drawInspector(BTreeNodeDesigner _selectNode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Script:", new GUILayoutOption[] { GUILayout.Width(100) });
            string[] scripts = AssetDatabase.FindAssets("t:Script " + _selectNode.m_EditorNode.m_Node.GetType().Name);
            if (scripts != null &&scripts.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(scripts[0]);
                MonoScript monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                EditorGUILayout.ObjectField("", monoScript, typeof(MonoScript), false);
            }
            GUILayout.EndHorizontal();

            //if (GUILayout.Button(BTreeEditorUtility.GearTexture, BTreeEditorUtility.TransparentButtonGUIStyle, new GUILayoutOption[0]))
            //{
            //    GenericMenu genericMenu = new GenericMenu();
            //    genericMenu.AddItem(new GUIContent("Edit Script"), false, new GenericMenu.MenuFunction2(openInFileEditor), _selectNode.m_EditorNode.m_Node);
            //    //genericMenu.AddItem(new GUIContent("Reset"), false, new GenericMenu.MenuFunction2(this.resetTask), _selectNode);
            //    genericMenu.ShowAsContext();
            //}
            var _node = _selectNode.m_EditorNode.m_Node;
            Type _nodeType = _selectNode.m_EditorNode.m_Node.GetType();
            FieldInfo[] fields = _nodeType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = fields.Length-1; i >= 0; i--)
            {
                DrawValue(_node, fields[i]);
            }
            GUILayout.Label("Precondition:");
            int index = -1;
            _node.m_NodePrecondition = DrawPrecondition(_node.GetNodePrecondition(), 0, ref index);
            GUILayout.FlexibleSpace();
        }

        BTreeNodePrecondition DrawPrecondition(BTreeNodePrecondition _condition, int _space, ref int index)
        {
            index = index + 1;
            BTreeNodePrecondition result = null;
            if (_condition == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(_space);
                
                m_Precondition[index] = EditorGUILayout.ObjectField("", m_Precondition[index], typeof(MonoScript), false);
                if (m_Precondition[index] != null)
                {
                    Type type = GetPreconditionType(m_Precondition[index].name);
                    if (type == null)
                    {
                        m_Precondition[index] = null;
                        return _condition;
                    }
                    result = (BTreeNodePrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                m_Precondition[index] = null;
                GUILayout.BeginHorizontal();
                GUILayout.Space(_space);
                string[] scripts = AssetDatabase.FindAssets("t:Script " + _condition.GetType().Name);
                if (scripts.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(scripts[0]);
                    MonoScript monoScript = (MonoScript)AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    var obj = EditorGUILayout.ObjectField("", monoScript, typeof(MonoScript), false);
                    Type type = GetPreconditionType(obj.name);
                    if (type == null)
                    {
                        return _condition;
                    }
                    result = (BTreeNodePrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                GUILayout.EndHorizontal();
                _space = _space + SPACEDETLE;

                BTreeNodePrecondition _lastPreCondition = _condition;
                if (result is BTreeNodePreconditionAND)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_space);
                    int childCount = 0;
                    if (_lastPreCondition is BTreeNodePreconditionAND)
                    {
                        BTreeNodePreconditionAND _lastCondAND = (BTreeNodePreconditionAND)_lastPreCondition;
                        childCount = _lastCondAND.GetChildPreconditionCount();
                    }
                    var val = EditorGUILayout.IntField(childCount);
                    GUILayout.EndHorizontal();

                    if (_lastPreCondition is BTreeNodePreconditionAND)
                    {
                        BTreeNodePreconditionAND _lastCondAND = (BTreeNodePreconditionAND)_lastPreCondition;
                        BTreeNodePrecondition[] childPreconditions = new BTreeNodePrecondition[val];
                        BTreeNodePrecondition[] curChildPreconditions = _lastCondAND.GetChildPrecondition();
                        for (int i = 0; i < val; i++)
                        {
                            BTreeNodePrecondition _cond = null;
                            if (curChildPreconditions.Length >= i + 1)
                            {
                                _cond = curChildPreconditions[i];
                            }
                            childPreconditions[i] = DrawPrecondition(_cond, _space, ref index);
                        }
                        ((BTreeNodePreconditionAND)result).SetChildPrecondition(childPreconditions);
                    }
                    else
                    {
                        BTreeNodePrecondition[] childPreconditions = new BTreeNodePrecondition[val];
                        for (int i = 0; i < val; i++)
                        {
                            childPreconditions[i] = DrawPrecondition(null, _space, ref index);
                        }
                        ((BTreeNodePreconditionAND)result).SetChildPrecondition(childPreconditions);
                    }

                }
                else if (result is BTreeNodePreconditionOR)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_space);
                    int childCount = 0;
                    if (_lastPreCondition is BTreeNodePreconditionOR)
                    {
                        BTreeNodePreconditionOR _lastCondOR = (BTreeNodePreconditionOR)_lastPreCondition;
                        childCount = _lastCondOR.GetChildPreconditionCount();
                    }
                    var val = EditorGUILayout.IntField(childCount);
                    GUILayout.EndHorizontal();

                    if (_lastPreCondition is BTreeNodePreconditionOR)
                    {
                        BTreeNodePreconditionOR _lastCondOR = (BTreeNodePreconditionOR)_lastPreCondition;
                        BTreeNodePrecondition[] childPreconditions = new BTreeNodePrecondition[val];
                        BTreeNodePrecondition[] curChildPreconditions = _lastCondOR.GetChildPrecondition();
                        for (int i = 0; i < val; i++)
                        {
                            BTreeNodePrecondition _cond = null;
                            if (curChildPreconditions.Length >= i + 1)
                            {
                                _cond = curChildPreconditions[i];
                            }
                            childPreconditions[i] = DrawPrecondition(_cond, _space, ref index);
                        }
                        ((BTreeNodePreconditionOR)result).SetChildPrecondition(childPreconditions);
                    }
                    else
                    {
                        BTreeNodePrecondition[] childPreconditions = new BTreeNodePrecondition[val];
                        for (int i = 0; i < val; i++)
                        {
                            childPreconditions[i] = DrawPrecondition(null, _space, ref index);
                        }
                        ((BTreeNodePreconditionOR)result).SetChildPrecondition(childPreconditions);
                    }
                }
                else if (result is BTreeNodePreconditionNOT)
                {
                    BTreeNodePrecondition curChildPreconditions = null;
                    if (_lastPreCondition is BTreeNodePreconditionNOT)
                    {
                        BTreeNodePreconditionNOT _lastCondNOT = (BTreeNodePreconditionNOT)_lastPreCondition;
                        curChildPreconditions = _lastCondNOT.GetChildPrecondition();
                    }
                    curChildPreconditions = DrawPrecondition(curChildPreconditions, _space, ref index);
                    ((BTreeNodePreconditionNOT)result).SetChildPrecondition(curChildPreconditions);
                }
            }
            
            return result;
        }
        void DrawValue(BTreeNode _node, FieldInfo _field)
        {
            if (_field == null)
            {
                return;
            }
            try
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(BTreeEditorUtility.SplitCamelCase(_field.Name)+":", new GUILayoutOption[] { GUILayout.Width(100) });
                if (_field.FieldType == typeof(int))
                {
                    var _val = EditorGUILayout.IntField((int)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(float))
                {
                    var _val = EditorGUILayout.FloatField((float)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(double))
                {
                    var _val = EditorGUILayout.DoubleField((float)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(string))
                {
                    var _val = EditorGUILayout.TextField((string)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType.IsEnum)
                {
                    var _val = EditorGUILayout.EnumPopup((Enum)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector2))
                {
                    var _val = EditorGUILayout.Vector2Field("",(Vector2)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector3))
                {
                    var _val = EditorGUILayout.Vector3Field("", (Vector3)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                else if (_field.FieldType == typeof(Vector4))
                {
                    var _val = EditorGUILayout.Vector4Field("", (Vector4)(_field.GetValue(_node)));
                    _field.SetValue(_node, _val);
                }
                GUILayout.EndHorizontal();
            }
            catch (Exception)
            {

            }
        }
        private void openInFileEditor(object _node)
        {
            Debugger.Log(_node.GetType().Name);
            
        }
        private Type GetPreconditionType(string name)
        {
            Type type = null;
            if (name == typeof(BTreeNodePreconditionAND).Name)
            {
                type = typeof(BTreeNodePreconditionAND);
            }
            else if (name == typeof(BTreeNodePreconditionOR).Name)
            {
                type = typeof(BTreeNodePreconditionOR);
            }
            else if (name == typeof(BTreeNodePreconditionNOT).Name)
            {
                type = typeof(BTreeNodePreconditionNOT);
            }
            else
            {
                if (BTreeNodeFactory.PreconditionTypeDic.ContainsKey(name))
                {
                    type = BTreeNodeFactory.PreconditionTypeDic[name];
                }
            }
            return type;
        }
    }
}
