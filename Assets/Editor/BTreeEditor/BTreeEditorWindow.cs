using System.Collections;
using System.Collections.Generic;
using BTreeFrame;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorWindow : EditorWindow
    {
        [SerializeField]
        public static BTreeEditorWindow instance;
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
        [MenuItem("Window/BTree Editor")]
        public static void ShowWindow()
        {
            BTreeEditorWindow bTreeEditorWindow = EditorWindow.GetWindow(typeof(BTreeEditorWindow)) as BTreeEditorWindow;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            Object.DontDestroyOnLoad(bTreeEditorWindow);
        }

        private BTreeGraphDesigner<BTreeTemplateData, BTreeTemplateData> mGraphDesigner = new BTreeGraphDesigner<BTreeTemplateData, BTreeTemplateData>();

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

        private GenericMenu mRightClickMenu = new GenericMenu();
        private GenericMenu mBreadcrumbGameObjectBehaviorMenu = new GenericMenu();
        private GenericMenu mBreadcrumbGameObjectMenu = new GenericMenu();
        private GenericMenu mBreadcrumbBehaviorMenu = new GenericMenu();

        [SerializeField]
        private Object mActiveObject;
        private Object mActiveBehaviorSource;

        public void OnGUI()
        {
            this.mCurrentMousePosition = Event.current.mousePosition;
            setupSizes();
            if (Draw())
            {
                base.Repaint();
            }
        }

        public bool Draw()
        {
            Color color = GUI.color;
            Color backgroundColor = GUI.backgroundColor;
            GUI.color = (Color.white);
            GUI.backgroundColor = (Color.white);
            drawFileToolbar();
            drawGraphArea();
            return true;
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
            if (!this.getMousePositionInGraph(out mousePosition))
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
            //string text = (this.mActiveObject as GameObject != null || this.mActiveObject as ExternalBehavior != null) ? this.mActiveObject.name : "(None Selected)";
            string text = (this.mActiveObject as GameObject != null) ? this.mActiveObject.name : "(None Selected)";
            if (GUILayout.Button(text, EditorStyles.toolbarPopup, new GUILayoutOption[]
            {
                GUILayout.Width(140f)
            }))
            {
                this.mBreadcrumbGameObjectMenu.ShowAsContext();
            }
            string text2 = (this.mActiveBehaviorSource != null) ? this.mActiveBehaviorSource.name : "(None Selected)";
            if (GUILayout.Button(text2, EditorStyles.toolbarPopup, new GUILayoutOption[]
            {
                GUILayout.Width(140f)
            }) && this.mActiveBehaviorSource != null)
            {
                this.mBreadcrumbBehaviorMenu.ShowAsContext();
            }
            if (GUILayout.Button("-", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(22f)
            }))
            {
                if (this.mActiveBehaviorSource != null)
                {
                    //this.removeBehavior();
                }
                else
                {
                    EditorUtility.DisplayDialog("Unable to Remove Behavior Tree", "No behavior tree selected.", "OK");
                }
            }
            if (GUILayout.Button("+", EditorStyles.toolbarButton, new GUILayoutOption[]
            {
                GUILayout.Width(22f)
            }))
            {
                if (this.mActiveObject != null)
                {
                    //this.addBehavior();
                }
                else
                {
                    EditorUtility.DisplayDialog("Unable to Add Behavior Tree", "No Game Object is selected.", "OK");
                }
            }
            //锁定
            //if (GUILayout.Button("Lock", this.mLockActiveGameObject ? BehaviorDesignerUtility.ToolbarButtonSelectionGUIStyle : EditorStyles.toolbarButton, new GUILayoutOption[]
            //{
            //    GUILayout.Width(42f)
            //}))
            //{
            //    if (this.mActiveObject != null)
            //    {
            //        this.mLockActiveGameObject = !this.mLockActiveGameObject;
            //        if (!this.mLockActiveGameObject)
            //        {
            //            this.updateTree(false);
            //        }
            //    }
            //    else
            //    {
            //        EditorUtility.DisplayDialog("Unable to Lock Game Object", "No Game Object is selected.", "OK");
            //    }
            //}
            //保存
            //if (GUILayout.Button("Save", EditorStyles.toolbarButton, new GUILayoutOption[]
            //{
            //    GUILayout.Width(42f)
            //}))
            //{
            //    if (this.mActiveBehaviorSource != null)
            //    {
            //        if (this.mActiveBehaviorSource.get_Owner().GetObject() as Behavior)
            //        {
            //            this.saveAsAsset();
            //        }
            //        else
            //        {
            //            this.saveAsPrefab();
            //        }
            //    }
            //    else
            //    {
            //        EditorUtility.DisplayDialog("Unable to Save Behavior Tree", "Select a behavior tree from within the scene.", "OK");
            //    }
            //}
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
        //获取鼠标位置
        private bool getMousePositionInGraph(out Vector2 mousePosition)
        {
            mousePosition = this.mCurrentMousePosition;
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
            return true;
        }
        private bool leftMouseDragged()
        {
            return true;
        }
        //鼠标左键Release
        private bool leftMouseRelease()
        {
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
    }
}
