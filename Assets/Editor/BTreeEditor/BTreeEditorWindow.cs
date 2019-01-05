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

using BTreeFrame;
using UnityEditor;
using UnityEngine;
using System;

namespace BTree.Editor
{
    public class BTreeEditorWindow : EditorWindow
    {
        public static BTreeEditorWindow Instance;
        
        [MenuItem("Window/BTree Editor %#_t")]
        public static void ShowWindow()
        {
            BTreeEditorWindow bTreeEditorWindow = GetWindow(typeof(BTreeEditorWindow)) as BTreeEditorWindow;
            bTreeEditorWindow.titleContent = new GUIContent("行为树编辑器");
            bTreeEditorWindow.mIsFirst = true;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            DontDestroyOnLoad(bTreeEditorWindow);
        }
        private BTreeGraphDesigner _mGraphDesigner;
        private BTreeGraphDesigner mGraphDesigner
        {
            get
            {
                if (_mGraphDesigner == null)
                {
                    _mGraphDesigner = new BTreeGraphDesigner();
                }
                return _mGraphDesigner;
            }
            set
            {
                _mGraphDesigner = value;
            }
        }

        private Rect mGraphRect;
        private Rect mFileToolBarRect;
        private Rect mPropertyToolbarRect;
        private Rect mPropertyBoxRect;
        private Rect mPreferencesPaneRect;

        private Vector2 mCurrentMousePosition = Vector2.zero;

        private Vector2 mGraphScrollSize = new Vector2(20000f, 20000f);
        private Vector2 mGraphScrollPosition = new Vector2(-1f, -1f);
        private Vector2 mGraphOffset = Vector2.zero;
        private float mGraphZoom = 1f;

        //是否显示设置界面
        private bool mShowPrefPanel;
        //是否点击节点中
        private bool mNodeClicked;
        //是否在拖动中
        private bool mIsDragging;
        //是否在连线状态
        private bool mIsConnectingLine;
        //当前鼠标位置的节点
        private BTreeNodeDesigner mCurMousePosNode;

        //右键菜单
        private BTreeEditorRightClickBlockMenu mRightClickBlockMenu = null;
        private BTreeEditorRightClickNodeMenu mRightClickNodeMenu = null;

        //属性栏
        private BTreeEditorNodeInspector mNodeInspector = new BTreeEditorNodeInspector();

        private bool mIsFirst;

        public void OnGUI()
        {
            if (mIsFirst)
            {
                mIsFirst = false;
            }
            mCurrentMousePosition = Event.current.mousePosition;
            setupSizes();
            handleEvents();
            if (Draw())
            {
                Repaint();
            }
        }

        public bool Draw()
        {
            bool result = false;
            Color color = GUI.color;
            Color backgroundColor = GUI.backgroundColor;
            GUI.color = (Color.white);
            GUI.backgroundColor = (Color.white);
            drawFileToolbar();
            drawPropertiesBox();
            if (drawGraphArea())
            {
                result = true;
            }
            GUI.color = color;
            GUI.backgroundColor = backgroundColor;
            return result;
        }
        private void setupSizes()
        {

            mFileToolBarRect = new Rect(BTreeEditorUtility.PropertyBoxWidth, 0f, (Screen.width - BTreeEditorUtility.PropertyBoxWidth), BTreeEditorUtility.ToolBarHeight);
            mPropertyToolbarRect = new Rect(0f, 0f, BTreeEditorUtility.PropertyBoxWidth, BTreeEditorUtility.ToolBarHeight);
            mPropertyBoxRect = new Rect(0f, mPropertyToolbarRect.height, BTreeEditorUtility.PropertyBoxWidth, Screen.height - mPropertyToolbarRect.height - BTreeEditorUtility.EditorWindowTabHeight);
            mGraphRect = new Rect(BTreeEditorUtility.PropertyBoxWidth, BTreeEditorUtility.ToolBarHeight, (Screen.width - BTreeEditorUtility.PropertyBoxWidth - BTreeEditorUtility.ScrollBarSize), (Screen.height - BTreeEditorUtility.ToolBarHeight - BTreeEditorUtility.EditorWindowTabHeight - BTreeEditorUtility.ScrollBarSize));
            mPreferencesPaneRect = new Rect(BTreeEditorUtility.PropertyBoxWidth + mGraphRect.width - BTreeEditorUtility.PreferencesPaneWidth, (BTreeEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), BTreeEditorUtility.PreferencesPaneWidth, BTreeEditorUtility.PreferencesPaneHeight);

            if (mGraphScrollPosition == new Vector2(-1f, -1f))
            {
                mGraphScrollPosition = (mGraphScrollSize - new Vector2(mGraphRect.width, mGraphRect.height)) / 2f - 2f * new Vector2(BTreeEditorUtility.ScrollBarSize, BTreeEditorUtility.ScrollBarSize);
            }
        }
        //绘制图形区域
        private bool drawGraphArea()
        {
            Vector2 vector = GUI.BeginScrollView(new Rect(mGraphRect.x, mGraphRect.y, mGraphRect.width + BTreeEditorUtility.ScrollBarSize, mGraphRect.height + BTreeEditorUtility.ScrollBarSize), mGraphScrollPosition, new Rect(0f, 0f, mGraphScrollSize.x, mGraphScrollSize.y), true, true);
            if (vector != mGraphScrollPosition && Event.current.type != EventType.DragUpdated && Event.current.type != EventType.Ignore)
            {
                mGraphOffset -= (vector - mGraphScrollPosition) / mGraphZoom;
                mGraphScrollPosition = vector;
                //mGraphDesigner.graphDirty();
            }
            GUI.EndScrollView();
            GUI.Box(mGraphRect, "", BTreeEditorUtility.GraphBackgroundGUIStyle);

            BTreeEditorZoomArea.Begin(mGraphRect, mGraphZoom);
            Vector2 mousePosition;
            if (!getMousePositionInGraph(out mousePosition))
            {
                mousePosition = new Vector2(-1f, -1f);
            }
            bool result = false;
            if (mGraphDesigner.drawNodes(mousePosition, mGraphOffset, mGraphZoom))
            {
                result = true;
            }
            if (mIsConnectingLine)
            {
                var _curNode = mGraphDesigner.nodeAt(mousePosition, mGraphOffset);
                Vector2 des = _curNode == null ? mousePosition : _curNode.m_EditorNode.m_Pos;
                mGraphDesigner.drawTempConnection(des, mGraphOffset, mGraphZoom);
            }
            BTreeEditorZoomArea.End();
            return result;
        }
        //绘制工具栏
        private void drawFileToolbar()
        {
            GUILayout.BeginArea(mFileToolBarRect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("LoadXML", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                loadBTree("xml");
            }
            if (GUILayout.Button("SaveXML", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                saveBTree("xml");
            }
            if (GUILayout.Button("LoadBinary", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                loadBTree("binary");
            }
            if (GUILayout.Button("SaveBinary", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                saveBTree("binary");
            }
            if (GUILayout.Button("ExportRunTime", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(100f)
            }))
            {
                exportBtree();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Preferences", mShowPrefPanel ? BTreeEditorUtility.ToolbarButtonSelectionGUIStyle : EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                mShowPrefPanel = !mShowPrefPanel;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //绘制属性栏
        private void drawPropertiesBox()
        {
            GUILayout.BeginArea(mPropertyToolbarRect, EditorStyles.toolbar);
            GUILayout.EndArea();
            GUILayout.BeginArea(mPropertyBoxRect, BTreeEditorUtility.PropertyBoxGUIStyle);
            if (mGraphDesigner.m_SelectedNodes.Count == 1)
            {
                mNodeInspector.drawInspector(mGraphDesigner.m_SelectedNodes[0]);
            }
            GUILayout.EndArea();
        }
        
        #region 操作消息处理相关
        //获取鼠标位置是否在绘图区域内
        private bool getMousePositionInGraph(out Vector2 mousePosition)
        {
            mousePosition = mCurrentMousePosition;
            if (!mGraphRect.Contains(mousePosition))
            {
                return false;
            }
            if (mShowPrefPanel && mPreferencesPaneRect.Contains(mousePosition))
            {
                return false;
            }
            mousePosition -= new Vector2(mGraphRect.xMin, mGraphRect.yMin);
            mousePosition /= mGraphZoom;
            return true;
        }
        //处理操作消息
        private void handleEvents()
        {
            if (EditorApplication.isCompiling) return;

            Event e = Event.current;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (leftMouseDown(e.clickCount))
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (rightMouseDown())
                        {
                            e.Use();
                            return;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        if (leftMouseRelease())
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 1)
                    {
                        if (rightMouseRelease())
                        {
                            e.Use();
                            return;
                        }
                    }
                    break;
                case EventType.MouseMove:
                    if (mouseMove())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        if (leftMouseDragged())
                        {
                            e.Use();
                            return;
                        }
                        if (e.modifiers == EventModifiers.Alt && mousePan())
                        {
                            e.Use();
                            return;
                        }
                    }
                    else if (e.button == 2 && mousePan())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.KeyDown:
                    break;
                case EventType.KeyUp:
                    break;
                case EventType.ScrollWheel:
                    if (mouseZoom())
                    {
                        e.Use();
                        return;
                    }
                    break;
                case EventType.Repaint:
                    break;
                case EventType.Layout:
                    break;
                case EventType.DragUpdated:
                    break;
                case EventType.DragPerform:
                    break;
                case EventType.DragExited:
                    break;
                case EventType.Ignore:
                    break;
                case EventType.Used:
                    break;
                case EventType.ValidateCommand:
                    break;
                case EventType.ExecuteCommand:
                    break;
                case EventType.ContextClick:
                    break;
                default:
                    break;
            }
        }
        //鼠标移动
        private bool mouseMove()
        {
            return true;
        }
        //鼠标左键down
        private bool leftMouseDown(int clickCount)
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                mIsConnectingLine = false;
                return false;
            }
            var nodeDesigner = mGraphDesigner.nodeAt(point, mGraphOffset);
            if (mIsConnectingLine && nodeDesigner != null)
            {
                mGraphDesigner.addSelectNodeLine(nodeDesigner);
            }
            mGraphDesigner.clearNodeSelection();
            if (nodeDesigner != null)
            {
                mGraphDesigner.select(nodeDesigner);
                mNodeClicked = true;
            }
            mIsConnectingLine = false;
            return true;
        }
        private bool leftMouseDragged()
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            if (mNodeClicked)
            {
                bool flag = mGraphDesigner.dragSelectedNodes(Event.current.delta / mGraphZoom, Event.current.modifiers != EventModifiers.Alt, mIsDragging);
                if (flag)
                {
                    mIsDragging = true;
                }
            }
            return true;
        }
        //鼠标左键Release
        private bool leftMouseRelease()
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            mNodeClicked = false;
            return true;
        }
        //鼠标右键down
        private bool rightMouseDown()
        {
            Vector2 point;
            mIsConnectingLine = false;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            mGraphDesigner.clearNodeSelection();
            var nodeDesigner = mGraphDesigner.nodeAt(point, mGraphOffset);
            if (nodeDesigner != null)
            {
                mGraphDesigner.select(nodeDesigner);
                mNodeClicked = true;
            }
            return true;
        }
        //鼠标右键Release
        private bool rightMouseRelease()
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            if (mGraphDesigner.m_SelectedNodes != null && mGraphDesigner.m_SelectedNodes.Count != 0)
            {
                if (mRightClickNodeMenu == null)
                {
                    mRightClickNodeMenu = new BTreeEditorRightClickNodeMenu(this);
                }
                mRightClickNodeMenu.ShowAsContext(mGraphDesigner.m_SelectedNodes);
                return true;
            }
            else
            {
                if (mRightClickBlockMenu == null)
                {
                    mRightClickBlockMenu = new BTreeEditorRightClickBlockMenu(this);
                }
                mRightClickBlockMenu.ShowAsContext();
                return true;
            }
        }
        private bool mouseZoom()
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            return true;
        }
        private bool mousePan()
        {
            Vector2 point;
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            return true;
        }
        //添加节点
        private void addNode(Type type, bool useMousePosition)
        {
            Vector2 vector = new Vector2(mGraphRect.width / (2f * mGraphZoom), 150f);
            if (useMousePosition)
            {
                getMousePositionInGraph(out vector);
            }
            vector -= mGraphOffset;
            if (mGraphDesigner.addNode(type, vector) != null)
            {
                Debugger.Log("addNode");
            }
        }
        //禁用节点
        private void disableSelectNode()
        {
            mGraphDesigner.disableNodeSelection();
        }
        //启用节点
        private void enableSelectNode()
        {
            mGraphDesigner.enableNodeSelection();
        }
        //删除节点
        private void delectSelectNode()
        {
            mGraphDesigner.delectSelectNode();
        }
        //设置入口节点
        private void setSelectNodeAsEntry()
        {
            mGraphDesigner.setSelectNodeAsEntry();
        }
        //前移节点
        private void moveUpSelectNodeIndex()
        {
            mGraphDesigner.moveUpSelectNodeIndex();
        }
        //后移节点
        private void moveDownSelectNodeIndex()
        {
            mGraphDesigner.moveDownSelectNodeIndex();
        }
        #endregion

        #region 配置文件相关
        public void loadBTree(string type)
        {
            string text = EditorUtility.OpenFilePanel("Load Behavior Tree", BTreeEditorSerialization.m_ConfigPath, type);
            if (!string.IsNullOrEmpty(text))
            {
                Debugger.Log("loadBTree:" + type);
                BTreeEditorConfig _config = null;
                if (type == "xml")
                {
                    _config = BTreeEditorSerialization.ReadXMLAtPath(text);
                }
                else if (type == "binary")
                {
                    _config = BTreeEditorSerialization.ReadBinary(text);
                }
                mGraphDesigner = (new BTreeGraphDesigner());
                mGraphDesigner.load(_config);
            }
        }
        public void saveBTree(string type)
        {
            if (mGraphDesigner == null || mGraphDesigner.m_RootNode == null)
            {
                EditorUtility.DisplayDialog("Save Error", "未创建根节点", "ok");
                return;
            }
            string suffix = "";
            if (type == "xml")
            {
                suffix = "xml";
            }
            else if (type == "binary")
            {
                suffix = "btreeEditor";
            }
            string text = EditorUtility.SaveFilePanel("Save Behavior Tree", BTreeEditorSerialization.m_ConfigPath, mGraphDesigner.m_RootNode.m_NodeName, suffix);
            if (text.Length != 0 && Application.dataPath.Length < text.Length)
            {
                Debugger.Log("saveBTree");
                BTreeEditorConfig _config = BTreeEditorNodeFactory.CreateBtreeEditorConfigFromGraphDesigner(mGraphDesigner);
                BTreeEditorSerialization.WirteXMLAtPath(_config, text);
                EditorUtility.DisplayDialog("Save", "保存行为树编辑器成功:" + text, "ok");
            }
        }
        public void exportBtree()
        {
            if (mGraphDesigner == null || mGraphDesigner.m_RootNode == null)
            {
                EditorUtility.DisplayDialog("Export Error", "未创建根节点", "ok");
                return;
            }
            Debugger.Log("exportBtree");
            TreeConfig _treeConfig = BTreeEditorNodeFactory.CreateTreeConfigFromBTreeGraphDesigner(mGraphDesigner);
            string name = mGraphDesigner.m_RootNode.m_NodeName;
            BTreeNodeSerialization.WriteBinary(_treeConfig, name);
            EditorUtility.DisplayDialog("Export", "导出运行时行为树配置成功:" + name, "ok");
        }
        #endregion
        #region 右键菜单点击回调
        public void addNodeCallback(object node)
        {
            addNode((Type)node,true);
        }
        public void disableNodeCallback()
        {
            disableSelectNode();
        }
        public void enableNodeCallback()
        {
            enableSelectNode();
        }
        public void delectNodeCallback()
        {
            delectSelectNode();
        }
        public void connectLineCallback()
        {
            mIsConnectingLine = true;
        }
        public void setEntryNodeCallback()
        {
            setSelectNodeAsEntry();
        }
        public void moveUpIndexCallback()
        {
            moveUpSelectNodeIndex();
        }
        public void moveDownIndexCallback()
        {
            moveDownSelectNodeIndex();
        }
        #endregion
    }
}
