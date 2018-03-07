using System.Collections.Generic;
using BTreeFrame;
using UnityEditor;
using UnityEngine;
using Battle.Logic.AI.BTree;

namespace BTree.Editor
{
    public class BTreeEditorWindow : EditorWindow
    {
        public static BTreeEditorWindow Instance;
        [MenuItem("ly/TestSave")]
        public static void TestSave()
        {
            BTreeEditorConfig data = new BTreeEditorConfig();
            BTreeEditorSerialization.WriteBinary(data, "test");
        }
        [MenuItem("ly/TestLoad")]
        public static void TestLoad()
        {
            var data = BTreeEditorSerialization.ReadBinary("test");
            Debugger.Log(data);
        }
        [MenuItem("Window/BTree Editor %#_t")]
        public static void ShowWindow()
        {
            BTreeEditorWindow bTreeEditorWindow = GetWindow(typeof(BTreeEditorWindow)) as BTreeEditorWindow;
            bTreeEditorWindow.mIsFirst = true;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            Object.DontDestroyOnLoad(bTreeEditorWindow);
        }

        private BTreeGraphDesigner<BTreeInputData, BTreeOutputData> mGraphDesigner = new BTreeGraphDesigner<BTreeInputData, BTreeOutputData>();

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
        private bool mShowRightClickMenu;
        //是否点击节点中
        private bool mNodeClicked;
        //是否在拖动中
        private bool mIsDragging;
        
        private GenericMenu mRightClickMenu = new GenericMenu();
        private GenericMenu mBreadcrumbGameObjectBehaviorMenu = new GenericMenu();
        private GenericMenu mBreadcrumbGameObjectMenu = new GenericMenu();
        private GenericMenu mBreadcrumbBehaviorMenu = new GenericMenu();


        private bool mIsFirst;

        public void OnGUI()
        {
            if (mIsFirst)
            {
                mIsFirst = false;
                loadBTree();
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
                mFileToolBarRect = new Rect((float)BTreeEditorUtility.PropertyBoxWidth, 0f, (float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth), (float)BTreeEditorUtility.ToolBarHeight);
                mPropertyToolbarRect = new Rect(0f, 0f, (float)BTreeEditorUtility.PropertyBoxWidth, (float)BTreeEditorUtility.ToolBarHeight);
                mPropertyBoxRect = new Rect(0f, this.mPropertyToolbarRect.height, (float)BTreeEditorUtility.PropertyBoxWidth, (float)Screen.height - this.mPropertyToolbarRect.height - (float)BTreeEditorUtility.EditorWindowTabHeight);
                mGraphRect = new Rect((float)BTreeEditorUtility.PropertyBoxWidth, (float)BTreeEditorUtility.ToolBarHeight, (float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth - BTreeEditorUtility.ScrollBarSize), (float)(Screen.height - BTreeEditorUtility.ToolBarHeight - BTreeEditorUtility.EditorWindowTabHeight - BTreeEditorUtility.ScrollBarSize));
                mPreferencesPaneRect = new Rect((float)BTreeEditorUtility.PropertyBoxWidth + this.mGraphRect.width - (float)BTreeEditorUtility.PreferencesPaneWidth, (float)(BTreeEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), (float)BTreeEditorUtility.PreferencesPaneWidth, (float)BTreeEditorUtility.PreferencesPaneHeight);
            }
            else
            {
                mFileToolBarRect = new Rect(0f, 0f, (float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth), (float)BTreeEditorUtility.ToolBarHeight);
                mPropertyToolbarRect = new Rect((float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth), 0f, (float)BTreeEditorUtility.PropertyBoxWidth, (float)BTreeEditorUtility.ToolBarHeight);
                mPropertyBoxRect = new Rect((float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth), this.mPropertyToolbarRect.height, (float)BTreeEditorUtility.PropertyBoxWidth, (float)Screen.height - this.mPropertyToolbarRect.height - (float)BTreeEditorUtility.EditorWindowTabHeight);
                mGraphRect = new Rect(0f, (float)BTreeEditorUtility.ToolBarHeight, (float)(Screen.width - BTreeEditorUtility.PropertyBoxWidth - BTreeEditorUtility.ScrollBarSize), (float)(Screen.height - BTreeEditorUtility.ToolBarHeight - BTreeEditorUtility.EditorWindowTabHeight - BTreeEditorUtility.ScrollBarSize));
                mPreferencesPaneRect = new Rect(this.mGraphRect.width - (float)BTreeEditorUtility.PreferencesPaneWidth, (float)(BTreeEditorUtility.ToolBarHeight + (EditorGUIUtility.isProSkin ? 1 : 2)), (float)BTreeEditorUtility.PreferencesPaneWidth, (float)BTreeEditorUtility.PreferencesPaneHeight);
            }
            if (this.mGraphScrollPosition == new Vector2(-1f, -1f))
            {
                this.mGraphScrollPosition = (this.mGraphScrollSize - new Vector2(this.mGraphRect.width, this.mGraphRect.height)) / 2f - 2f * new Vector2((float)BTreeEditorUtility.ScrollBarSize, (float)BTreeEditorUtility.ScrollBarSize);
            }
        }
        //绘制图形区域
        private bool drawGraphArea()
        {
            Vector2 vector = GUI.BeginScrollView(new Rect(this.mGraphRect.x, this.mGraphRect.y, this.mGraphRect.width + (float)BTreeEditorUtility.ScrollBarSize, this.mGraphRect.height + (float)BTreeEditorUtility.ScrollBarSize), this.mGraphScrollPosition, new Rect(0f, 0f, this.mGraphScrollSize.x, this.mGraphScrollSize.y), true, true);
            if (vector != this.mGraphScrollPosition && Event.current.type != EventType.DragUpdated && Event.current.type != EventType.Ignore)
            {
                mGraphOffset -= (vector - this.mGraphScrollPosition) / this.mGraphZoom;
                mGraphScrollPosition = vector;
                //this.mGraphDesigner.graphDirty();
            }
            GUI.EndScrollView();
            GUI.Box(this.mGraphRect, "", BTreeEditorUtility.GraphBackgroundGUIStyle);

            BTreeEditorZoomArea.Begin(this.mGraphRect, this.mGraphZoom);
            Vector2 mousePosition;
            if (!getMousePositionInGraph(out mousePosition))
            {
                mousePosition = new Vector2(-1f, -1f);
            }
            bool result = false;
            if (this.mGraphDesigner != null && this.mGraphDesigner.drawNodes(mousePosition, this.mGraphOffset, this.mGraphZoom))
            {
                result = true;
            }
            //if (this.mIsSelecting)
            //{
            //    GUI.Box(this.getSelectionArea(), "", BehaviorDesignerUtility.SelectionGUIStyle);
            //}
            BTreeEditorZoomArea.End();
            return result;
        }
        //绘制工具栏
        private void drawFileToolbar()
        {
            GUILayout.BeginArea(this.mFileToolBarRect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            if (GUILayout.Button("...", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(22f)
            }))
            {
                this.mBreadcrumbGameObjectBehaviorMenu.ShowAsContext();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Preferences", this.mShowPrefPanel ? BTreeEditorUtility.ToolbarButtonSelectionGUIStyle : EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(80f)
            }))
            {
                this.mShowPrefPanel = !this.mShowPrefPanel;
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        //绘制属性栏
        private void drawPropertiesBox()
        {
            GUILayout.BeginArea(this.mPropertyToolbarRect, EditorStyles.toolbar);
            GUILayout.EndArea();
            GUILayout.BeginArea(this.mPropertyBoxRect, BTreeEditorUtility.PropertyBoxGUIStyle);

            GUILayout.EndArea();
        }
        #region 操作消息处理相关
        //获取鼠标位置是否在绘图区域内
        private bool getMousePositionInGraph(out Vector2 mousePosition)
        {
            mousePosition = mCurrentMousePosition;
            if (!this.mGraphRect.Contains(mousePosition))
            {
                return false;
            }
            if (this.mShowPrefPanel && this.mPreferencesPaneRect.Contains(mousePosition))
            {
                return false;
            }
            mousePosition -= new Vector2(this.mGraphRect.xMin, this.mGraphRect.yMin);
            mousePosition /= this.mGraphZoom;
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
                        if (mShowRightClickMenu)
                        {
                            mShowRightClickMenu = false;
                            mRightClickMenu.ShowAsContext();
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
            mGraphDesigner.clearNodeSelection();
            var nodeDesigner = mGraphDesigner.nodeAt(point, this.mGraphOffset);
            if (nodeDesigner != null)
            {
                mGraphDesigner.select(nodeDesigner);
                mNodeClicked = true;
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
            mNodeClicked = false;
            return true;
        }
        //鼠标右键down
        private bool rightMouseDown()
        {
            return true;
        }
        private bool mouseZoom()
        {
            return true;
        }
        private bool mousePan()
        {
            return true;
        }
        #endregion
        public void loadBTree()
        {
            BTreeEditorConfig _config = new BTreeEditorConfig();
            mGraphDesigner = (new BTreeGraphDesigner<BTreeInputData, BTreeOutputData>());
            mGraphDesigner.load(_config);
        }
       

    }
}
