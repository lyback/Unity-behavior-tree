using System.Collections.Generic;
using BTreeFrame;
using UnityEditor;
using UnityEngine;
using Battle.Logic.AI.BTree;
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
            bTreeEditorWindow.mIsFirst = true;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            UnityEngine.Object.DontDestroyOnLoad(bTreeEditorWindow);
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
        
        private BTreeEditorRightClickBlockMenu mRightClickBlockMenu = null;
        private BTreeEditorRightClickNodeMenu mRightClickNodeMenu = null;
        private BTreeNodeDesigner mCurRightClickNode = null;

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
            //if (BehaviorDesignerPreferences.GetBool(BDPreferneces.PropertiesPanelOnLeft))
            if (true)
            {
                mFileToolBarRect = new Rect(BTreeEditorUtility.PropertyBoxWidth, 0f, (Screen.width - BTreeEditorUtility.PropertyBoxWidth), BTreeEditorUtility.ToolBarHeight);
                mPropertyToolbarRect = new Rect(0f, 0f, BTreeEditorUtility.PropertyBoxWidth, BTreeEditorUtility.ToolBarHeight);
                mPropertyBoxRect = new Rect(0f, mPropertyToolbarRect.height, BTreeEditorUtility.PropertyBoxWidth, Screen.height - mPropertyToolbarRect.height - BTreeEditorUtility.EditorWindowTabHeight);
                mGraphRect = new Rect(BTreeEditorUtility.PropertyBoxWidth, BTreeEditorUtility.ToolBarHeight, (Screen.width - BTreeEditorUtility.PropertyBoxWidth - BTreeEditorUtility.ScrollBarSize), (Screen.height - BTreeEditorUtility.ToolBarHeight - BTreeEditorUtility.EditorWindowTabHeight - BTreeEditorUtility.ScrollBarSize));
                mPreferencesPaneRect = new Rect(BTreeEditorUtility.PropertyBoxWidth + mGraphRect.width - BTreeEditorUtility.PreferencesPaneWidth, (BTreeEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), BTreeEditorUtility.PreferencesPaneWidth, BTreeEditorUtility.PreferencesPaneHeight);
            }
            else
            {
                mFileToolBarRect = new Rect(0f, 0f, (Screen.width - BTreeEditorUtility.PropertyBoxWidth), BTreeEditorUtility.ToolBarHeight);
                mPropertyToolbarRect = new Rect((Screen.width - BTreeEditorUtility.PropertyBoxWidth), 0f, BTreeEditorUtility.PropertyBoxWidth, BTreeEditorUtility.ToolBarHeight);
                mPropertyBoxRect = new Rect((Screen.width - BTreeEditorUtility.PropertyBoxWidth), mPropertyToolbarRect.height, BTreeEditorUtility.PropertyBoxWidth, Screen.height - mPropertyToolbarRect.height - BTreeEditorUtility.EditorWindowTabHeight);
                mGraphRect = new Rect(0f, BTreeEditorUtility.ToolBarHeight, (Screen.width - BTreeEditorUtility.PropertyBoxWidth - BTreeEditorUtility.ScrollBarSize), (Screen.height - BTreeEditorUtility.ToolBarHeight - BTreeEditorUtility.EditorWindowTabHeight - BTreeEditorUtility.ScrollBarSize));
                mPreferencesPaneRect = new Rect(mGraphRect.width - BTreeEditorUtility.PreferencesPaneWidth, (BTreeEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), BTreeEditorUtility.PreferencesPaneWidth, BTreeEditorUtility.PreferencesPaneHeight);
            }
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
            if (GUILayout.Button("Load", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                loadBTree();
            }
            if (GUILayout.Button("Save", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                saveBTree();
            }
            if (GUILayout.Button("Export", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
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
        #endregion

        #region 配置文件相关
        public void loadBTree()
        {
            Debugger.Log("loadBTree");
            string text = EditorUtility.OpenFilePanel("Load Behavior Tree", "Assets/Editor/BtreeEditor/Config", "xml");
            BTreeEditorConfig _config = BTreeEditorSerialization.ReadXMLAtPath(text);
            mGraphDesigner = (new BTreeGraphDesigner());
            mGraphDesigner.load(_config);
        }
        public void saveBTree()
        {
            if (mGraphDesigner == null)
            {
                return;
            }
            Debugger.Log("saveBTree");
            string text = EditorUtility.SaveFilePanel("Save Behavior Tree", "Assets/Editor/BtreeEditor/Config", mGraphDesigner.m_RootNode.m_NodeName,"xml");
            if (text.Length != 0 && Application.dataPath.Length < text.Length)
            {
                BTreeEditorConfig _config = BTreeEditorNodeFactory.CreateBtreeEditorConfigFromGraphDesigner(mGraphDesigner);
                BTreeEditorSerialization.WirteXMLAtPath(_config, text);
            }
        }
        public void exportBtree()
        {
            if (mGraphDesigner == null)
            {
                return;
            }
            Debugger.Log("exportBtree");
            TreeConfig _treeConfig = BTreeEditorNodeFactory.CreateTreeConfigFromBTreeGraphDesigner(mGraphDesigner);
            string name = mGraphDesigner.m_RootNode.m_NodeName;
            BTreeNodeSerialization.WriteBinary(_treeConfig, name);
            EditorUtility.DisplayDialog("Export", "导出行为树配置成功:" + name, "确定");
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
        #endregion
    }
}
