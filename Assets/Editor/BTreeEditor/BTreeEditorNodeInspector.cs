
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

        public void drawInspector(BTreeNodeDesigner _selectNode)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Script", new GUILayoutOption[] { GUILayout.Width(100) });
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
            int index = -1;
            _node.m_NodePrecondition = DrawPrecondition(_node.GetNodePrecondition(), 0, ref index);
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
                        m_Precondition[index] = null;
                        return _condition;
                    }
                    result = (BTreeNodePrecondition)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
                }
                GUILayout.EndHorizontal();
                _space = _space + 5;

                if (result is BTreeNodePreconditionAND)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(_space);
                    BTreeNodePreconditionAND CurPreCondition = (BTreeNodePreconditionAND)_condition;
                    int childCount = CurPreCondition.GetChildPreconditionCount();
                    var val = EditorGUILayout.IntField(childCount);
                    GUILayout.EndHorizontal();

                    BTreeNodePrecondition[] childPreconditions = new BTreeNodePrecondition[val];
                    BTreeNodePrecondition[] curChildPreconditions = CurPreCondition.GetChildPrecondition();
                    for (int i = 0; i < val; i++)
                    {
                        BTreeNodePrecondition _cond = null;
                        if (curChildPreconditions.Length >= i+1)
                        {
                            _cond = curChildPreconditions[i];
                        }
                        childPreconditions[i] = DrawPrecondition(_cond, _space, ref index);
                    }
                    ((BTreeNodePreconditionAND)result).SetChildPrecondition(childPreconditions);
                }
                else if (result is BTreeNodePreconditionOR)
                {
                }
                else if (result is BTreeNodePreconditionNOT)
                {
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
                EditorGUILayout.LabelField(BTreeEditorUtility.SplitCamelCase(_field.Name), new GUILayoutOption[] { GUILayout.Width(100) });
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
