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

        private BTreeGraphDesigner<BTreeInputData, BTreeOutputData> mGraphDesigner = null;

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
        //是否显示右键菜单
        private bool mShowRightClickBlockMenu;
        //是否点击节点中
        private bool mNodeClicked;
        //是否在拖动中
        private bool mIsDragging;
        
        private BTreeEditorRightClickBlockMenu<BTreeInputData, BTreeOutputData> mRightClickBlockMenu = null;
        private GenericMenu mBreadcrumbGameObjectBehaviorMenu = new GenericMenu();
        private GenericMenu mBreadcrumbGameObjectMenu = new GenericMenu();
        private GenericMenu mBreadcrumbBehaviorMenu = new GenericMenu();

        private bool mIsFirst;

        public void OnGUI()
        {
            if (mIsFirst)
            {
                mGraphDesigner = new BTreeGraphDesigner<BTreeInputData, BTreeOutputData>();
                mIsFirst = false;
            }
            mCurrentMousePosition = Event.current.mousePosition;
            setupSizes();
            handleEvents();
            if (Draw())
            {
                base.Repaint();
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
            if (mGraphDesigner != null && mGraphDesigner.drawNodes(mousePosition, mGraphOffset, mGraphZoom))
            {
                result = true;
            }
            //if (mIsSelecting)
            //{
            //    GUI.Box(getSelectionArea(), "", BehaviorDesignerUtility.SelectionGUIStyle);
            //}
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
                if (mGraphDesigner != null)
                {
                    saveBTree();
                }
                else
                {
                    EditorUtility.DisplayDialog("Unable to Save Behavior Tree", "Select a behavior tree from within the scene.", "OK");
                }
            }
            if (GUILayout.Button("Export", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(42f)
            }))
            {
                if (mGraphDesigner != null)
                {
                    exportBtree();
                }
                else
                {
                    EditorUtility.DisplayDialog("Unable to Export Behavior Tree", "Select a behavior tree from within the scene.", "OK");
                }
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
                        if (mShowRightClickBlockMenu)
                        {
                            mShowRightClickBlockMenu = false;
                            if (mRightClickBlockMenu == null)
                            {
                                mRightClickBlockMenu = new BTreeEditorRightClickBlockMenu<BTreeInputData, BTreeOutputData>(this);
                            }
                            mRightClickBlockMenu.ShowAsContext();
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
                return false;
            }
            if (mGraphDesigner != null)
            {
                mGraphDesigner.clearNodeSelection();
                var nodeDesigner = mGraphDesigner.nodeAt(point, mGraphOffset);
                if (nodeDesigner != null)
                {
                    mGraphDesigner.select(nodeDesigner);
                    mNodeClicked = true;
                }
            }
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
            if (!getMousePositionInGraph(out point))
            {
                return false;
            }
            if (mGraphDesigner != null)
            {
                var nodeDesigner = mGraphDesigner.nodeAt(point, mGraphOffset);
                if (nodeDesigner == null)
                {
                    mShowRightClickBlockMenu = true;
                }
                else
                {

                }
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
            if (mShowRightClickBlockMenu)
            {
                mShowRightClickBlockMenu = false;
                mRightClickBlockMenu.ShowAsContext();
                return true;
            }
            return false;
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
        #endregion



        #region 配置文件相关
        public void loadBTree()
        {
            Debugger.Log("loadBTree");
            string text = EditorUtility.OpenFilePanel("Load Behavior Tree", "Assets/Editor/BtreeEditor/Config", "xml");
            BTreeEditorConfig _config = BTreeEditorSerialization.ReadXMLAtPath(text);
            mGraphDesigner = (new BTreeGraphDesigner<BTreeInputData, BTreeOutputData>());
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
                BTreeEditorConfig _config = BTreeEditorNodeFactory<BTreeInputData, BTreeOutputData>.CreateBtreeEditorConfigFromGraphDesigner(mGraphDesigner);
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
            TreeConfig _treeConfig = BTreeEditorNodeFactory<BTreeInputData, BTreeOutputData>.CreateTreeConfigFromBTreeGraphDesigner(mGraphDesigner);
            BTreeNodeSerialization.WriteBinary(_treeConfig, mGraphDesigner.m_RootNode.m_NodeName);
        }
        #endregion

        public void addNodeCallback(object node)
        {
            addNode((Type)node,true);
        }
    }
}
