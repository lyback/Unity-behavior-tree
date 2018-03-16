
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using BTreeFrame;
namespace BTree.Editor
{
    public class BTreeEditorNodeInspector
    {

        public void drawInspector(BTreeNodeDesigner _selectNode)
        {
            var _node = _selectNode.m_EditorNode.m_Node;
            Type _nodeType = _selectNode.m_EditorNode.m_Node.GetType();
            FieldInfo[] fields = _nodeType.GetFields(BindingFlags.Instance | BindingFlags.Public);
            for (int i = fields.Length-1; i >= 0; i--)
            {
                DrawValue(_node, fields[i]);
            }
        }

        public void DrawValue(BTreeNode _node, FieldInfo _field)
        {
            if (_field == null)
            {
                return;
            }
            try
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(BTreeEditorUtility.SplitCamelCase(_field.Name), new GUILayoutOption[] { GUILayout.Width(120) });
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
    }
}
