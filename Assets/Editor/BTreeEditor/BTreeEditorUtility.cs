using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public static class BTreeEditorUtility
    {
        public static readonly int ToolBarHeight = 18;

        public static readonly int PropertyBoxWidth = 320;

        public static readonly int ScrollBarSize = 15;

        public static readonly int EditorWindowTabHeight = 21;

        public static readonly int PreferencesPaneWidth = 290;

        public static readonly int PreferencesPaneHeight = 208;

        public static readonly float GraphZoomMax = 1f;

        public static readonly float GraphZoomMin = 0.4f;

        public static readonly float GraphZoomSensitivity = 150f;

        public static readonly int LineSelectionThreshold = 7;

        public static readonly int TaskBackgroundShadowSize = 3;

        public static readonly int TitleHeight = 20;

        public static readonly int IconAreaHeight = 52;

        public static readonly int IconSize = 44;

        public static readonly int IconBorderSize = 46;

        public static readonly int ConnectionWidth = 42;

        public static readonly int TopConnectionHeight = 14;

        public static readonly int BottomConnectionHeight = 16;

        public static readonly int TaskConnectionCollapsedWidth = 26;

        public static readonly int TaskConnectionCollapsedHeight = 6;

        public static readonly int MinWidth = 100;

        public static readonly int MaxWidth = 220;

        public static readonly int MaxCommentHeight = 100;

        public static readonly int TextPadding = 20;

        public static readonly float NodeFadeDuration = 0.5f;

        public static readonly int IdentifyUpdateFadeTime = 500;

        public static readonly int MaxIdentifyUpdateCount = 2000;

        public static readonly int TaskPropertiesLabelWidth = 150;

        public static readonly int MaxTaskDescriptionBoxWidth = 400;

        public static readonly int MaxTaskDescriptionBoxHeight = 300;

        private static GUIStyle graphStatusGUIStyle = null;

        private static GUIStyle taskFoldoutGUIStyle = null;

        private static GUIStyle taskTitleGUIStyle = null;

        private static GUIStyle taskGUIStyle = null;

        private static GUIStyle taskSelectedGUIStyle = null;

        private static GUIStyle taskRunningGUIStyle = null;

        private static GUIStyle taskRunningSelectedGUIStyle = null;

        private static GUIStyle taskIdentifyGUIStyle = null;

        private static GUIStyle taskIdentifySelectedGUIStyle = null;

        private static GUIStyle taskCommentGUIStyle = null;

        private static GUIStyle taskCommentLeftAlignGUIStyle = null;

        private static GUIStyle taskCommentRightAlignGUIStyle = null;

        private static GUIStyle graphBackgroundGUIStyle = null;

        private static GUIStyle selectionGUIStyle = null;

        private static GUIStyle labelWrapGUIStyle = null;

        private static GUIStyle taskInspectorCommentGUIStyle = null;

        private static GUIStyle taskInspectorGUIStyle = null;

        private static GUIStyle toolbarButtonSelectionGUIStyle = null;

        private static GUIStyle propertyBoxGUIStyle = null;

        private static GUIStyle preferencesPaneGUIStyle = null;

        private static GUIStyle plainButtonGUIStyle = null;

        private static GUIStyle transparentButtonGUIStyle = null;

        private static GUIStyle welcomeScreenIntroGUIStyle = null;

        private static GUIStyle welcomeScreenTextHeaderGUIStyle = null;

        private static GUIStyle welcomeScreenTextDescriptionGUIStyle = null;

        private static Texture2D taskBorderTexture = null;

        private static Texture2D taskBorderRunningTexture = null;

        private static Texture2D taskBorderIdentifyTexture = null;

        private static Texture2D taskConnectionTexture = null;

        private static Texture2D taskConnectionTopTexture = null;

        private static Texture2D taskConnectionBottomTexture = null;

        private static Texture2D taskConnectionRunningTopTexture = null;

        private static Texture2D taskConnectionRunningBottomTexture = null;

        private static Texture2D taskConnectionIdentifyTopTexture = null;

        private static Texture2D taskConnectionIdentifyBottomTexture = null;

        private static Texture2D taskConnectionCollapsedTexture = null;

        private static Texture2D contentSeparatorTexture = null;

        private static Texture2D docTexture = null;

        private static Texture2D gearTexture = null;

        private static Texture2D syncedTexture = null;

        private static Texture2D sharedTexture = null;

        private static Texture2D variableButtonTexture = null;

        private static Texture2D variableButtonSelectedTexture = null;

        private static Texture2D variableWatchButtonTexture = null;

        private static Texture2D variableWatchButtonSelectedTexture = null;

        private static Texture2D referencedTexture = null;

        private static Texture2D deleteButtonTexture = null;

        private static Texture2D identifyButtonTexture = null;

        private static Texture2D breakpointTexture = null;

        private static Texture2D enableTaskTexture = null;

        private static Texture2D disableTaskTexture = null;

        private static Texture2D expandTaskTexture = null;

        private static Texture2D collapseTaskTexture = null;

        private static Texture2D executionSuccessTexture = null;

        private static Texture2D executionFailureTexture = null;

        public static GUIStyle GraphStatusGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.graphStatusGUIStyle == null)
                {
                    BTreeEditorUtility.initGraphStatusGUIStyle();
                }
                return BTreeEditorUtility.graphStatusGUIStyle;
            }
        }

        public static GUIStyle TaskFoldoutGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskFoldoutGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskFoldoutGUIStyle();
                }
                return BTreeEditorUtility.taskFoldoutGUIStyle;
            }
        }

        public static GUIStyle TaskTitleGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskTitleGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskTitleGUIStyle();
                }
                return BTreeEditorUtility.taskTitleGUIStyle;
            }
        }

        public static GUIStyle TaskGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskGUIStyle();
                }
                return BTreeEditorUtility.taskGUIStyle;
            }
        }

        public static GUIStyle TaskSelectedGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskSelectedGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskSelectedGUIStyle();
                }
                return BTreeEditorUtility.taskSelectedGUIStyle;
            }
        }

        public static GUIStyle TaskRunningGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskRunningGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskRunningGUIStyle();
                }
                return BTreeEditorUtility.taskRunningGUIStyle;
            }
        }

        public static GUIStyle TaskRunningSelectedGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskRunningSelectedGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskRunningSelectedGUIStyle();
                }
                return BTreeEditorUtility.taskRunningSelectedGUIStyle;
            }
        }

        public static GUIStyle TaskIdentifyGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskIdentifyGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskIdentifyGUIStyle();
                }
                return BTreeEditorUtility.taskIdentifyGUIStyle;
            }
        }

        public static GUIStyle TaskIdentifySelectedGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskIdentifySelectedGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskIdentifySelectedGUIStyle();
                }
                return BTreeEditorUtility.taskIdentifySelectedGUIStyle;
            }
        }

        public static GUIStyle TaskCommentGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskCommentGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskCommentGUIStyle();
                }
                return BTreeEditorUtility.taskCommentGUIStyle;
            }
        }

        public static GUIStyle TaskCommentLeftAlignGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskCommentLeftAlignGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskCommentLeftAlignGUIStyle();
                }
                return BTreeEditorUtility.taskCommentLeftAlignGUIStyle;
            }
        }

        public static GUIStyle TaskCommentRightAlignGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskCommentRightAlignGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskCommentRightAlignGUIStyle();
                }
                return BTreeEditorUtility.taskCommentRightAlignGUIStyle;
            }
        }

        public static GUIStyle GraphBackgroundGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.graphBackgroundGUIStyle == null)
                {
                    BTreeEditorUtility.initGraphBackgroundGUIStyle();
                }
                return BTreeEditorUtility.graphBackgroundGUIStyle;
            }
        }

        public static GUIStyle SelectionGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.selectionGUIStyle == null)
                {
                    BTreeEditorUtility.initSelectionGUIStyle();
                }
                return BTreeEditorUtility.selectionGUIStyle;
            }
        }

        public static GUIStyle LabelWrapGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.labelWrapGUIStyle == null)
                {
                    BTreeEditorUtility.initLabelWrapGUIStyle();
                }
                return BTreeEditorUtility.labelWrapGUIStyle;
            }
        }

        public static GUIStyle TaskInspectorCommentGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskInspectorCommentGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskInspectorCommentGUIStyle();
                }
                return BTreeEditorUtility.taskInspectorCommentGUIStyle;
            }
        }

        public static GUIStyle TaskInspectorGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.taskInspectorGUIStyle == null)
                {
                    BTreeEditorUtility.initTaskInspectorGUIStyle();
                }
                return BTreeEditorUtility.taskInspectorGUIStyle;
            }
        }

        public static GUIStyle ToolbarButtonSelectionGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.toolbarButtonSelectionGUIStyle == null)
                {
                    BTreeEditorUtility.initToolbarButtonSelectionGUIStyle();
                }
                return BTreeEditorUtility.toolbarButtonSelectionGUIStyle;
            }
        }

        public static GUIStyle PropertyBoxGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.propertyBoxGUIStyle == null)
                {
                    BTreeEditorUtility.initPropertyBoxGUIStyle();
                }
                return BTreeEditorUtility.propertyBoxGUIStyle;
            }
        }

        public static GUIStyle PreferencesPaneGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.preferencesPaneGUIStyle == null)
                {
                    BTreeEditorUtility.initPreferencesPaneGUIStyle();
                }
                return BTreeEditorUtility.preferencesPaneGUIStyle;
            }
        }

        public static GUIStyle PlainButtonGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.plainButtonGUIStyle == null)
                {
                    BTreeEditorUtility.initPlainButtonGUIStyle();
                }
                return BTreeEditorUtility.plainButtonGUIStyle;
            }
        }

        public static GUIStyle TransparentButtonGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.transparentButtonGUIStyle == null)
                {
                    BTreeEditorUtility.initTransparentButtonGUIStyle();
                }
                return BTreeEditorUtility.transparentButtonGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenIntroGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.welcomeScreenIntroGUIStyle == null)
                {
                    BTreeEditorUtility.initWelcomeScreenIntroGUIStyle();
                }
                return BTreeEditorUtility.welcomeScreenIntroGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenTextHeaderGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle == null)
                {
                    BTreeEditorUtility.initWelcomeScreenTextHeaderGUIStyle();
                }
                return BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle;
            }
        }

        public static GUIStyle WelcomeScreenTextDescriptionGUIStyle
        {
            get
            {
                if (BTreeEditorUtility.welcomeScreenTextDescriptionGUIStyle == null)
                {
                    BTreeEditorUtility.initWelcomeScreenTextDescriptionGUIStyle();
                }
                return BTreeEditorUtility.welcomeScreenTextDescriptionGUIStyle;
            }
        }

        public static Texture2D TaskBorderTexture
        {
            get
            {
                if (BTreeEditorUtility.taskBorderTexture == null)
                {
                    BTreeEditorUtility.initTaskBorderTexture();
                }
                return BTreeEditorUtility.taskBorderTexture;
            }
        }

        public static Texture2D TaskBorderRunningTexture
        {
            get
            {
                if (BTreeEditorUtility.taskBorderRunningTexture == null)
                {
                    BTreeEditorUtility.initTaskBorderRunningTexture();
                }
                return BTreeEditorUtility.taskBorderRunningTexture;
            }
        }

        public static Texture2D TaskBorderIdentifyTexture
        {
            get
            {
                if (BTreeEditorUtility.taskBorderIdentifyTexture == null)
                {
                    BTreeEditorUtility.initTaskBorderIdentifyTexture();
                }
                return BTreeEditorUtility.taskBorderIdentifyTexture;
            }
        }

        public static Texture2D TaskConnectionTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionTexture();
                }
                return BTreeEditorUtility.taskConnectionTexture;
            }
        }

        public static Texture2D TaskConnectionTopTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionTopTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionTopTexture();
                }
                return BTreeEditorUtility.taskConnectionTopTexture;
            }
        }

        public static Texture2D TaskConnectionBottomTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionBottomTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionBottomTexture();
                }
                return BTreeEditorUtility.taskConnectionBottomTexture;
            }
        }

        public static Texture2D TaskConnectionRunningTopTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionRunningTopTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionRunningTopTexture();
                }
                return BTreeEditorUtility.taskConnectionRunningTopTexture;
            }
        }

        public static Texture2D TaskConnectionRunningBottomTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionRunningBottomTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionRunningBottomTexture();
                }
                return BTreeEditorUtility.taskConnectionRunningBottomTexture;
            }
        }

        public static Texture2D TaskConnectionIdentifyTopTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionIdentifyTopTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionIdentifyTopTexture();
                }
                return BTreeEditorUtility.taskConnectionIdentifyTopTexture;
            }
        }

        public static Texture2D TaskConnectionIdentifyBottomTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionIdentifyBottomTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionIdentifyBottomTexture();
                }
                return BTreeEditorUtility.taskConnectionIdentifyBottomTexture;
            }
        }

        public static Texture2D TaskConnectionCollapsedTexture
        {
            get
            {
                if (BTreeEditorUtility.taskConnectionCollapsedTexture == null)
                {
                    BTreeEditorUtility.initTaskConnectionCollapsedTexture();
                }
                return BTreeEditorUtility.taskConnectionCollapsedTexture;
            }
        }

        public static Texture2D ContentSeparatorTexture
        {
            get
            {
                if (BTreeEditorUtility.contentSeparatorTexture == null)
                {
                    BTreeEditorUtility.initContentSeparatorTexture();
                }
                return BTreeEditorUtility.contentSeparatorTexture;
            }
        }

        public static Texture2D DocTexture
        {
            get
            {
                if (BTreeEditorUtility.docTexture == null)
                {
                    BTreeEditorUtility.initDocTexture();
                }
                return BTreeEditorUtility.docTexture;
            }
        }

        public static Texture2D GearTexture
        {
            get
            {
                if (BTreeEditorUtility.gearTexture == null)
                {
                    BTreeEditorUtility.initGearTexture();
                }
                return BTreeEditorUtility.gearTexture;
            }
        }

        public static Texture2D SyncedTexture
        {
            get
            {
                if (BTreeEditorUtility.syncedTexture == null)
                {
                    BTreeEditorUtility.initSyncedTexture();
                }
                return BTreeEditorUtility.syncedTexture;
            }
        }

        public static Texture2D SharedTexture
        {
            get
            {
                if (BTreeEditorUtility.sharedTexture == null)
                {
                    BTreeEditorUtility.initSharedTexture();
                }
                return BTreeEditorUtility.sharedTexture;
            }
        }

        public static Texture2D VariableButtonTexture
        {
            get
            {
                if (BTreeEditorUtility.variableButtonTexture == null)
                {
                    BTreeEditorUtility.initVariableButtonTexture();
                }
                return BTreeEditorUtility.variableButtonTexture;
            }
        }

        public static Texture2D VariableButtonSelectedTexture
        {
            get
            {
                if (BTreeEditorUtility.variableButtonSelectedTexture == null)
                {
                    BTreeEditorUtility.initVariableButtonSelectedTexture();
                }
                return BTreeEditorUtility.variableButtonSelectedTexture;
            }
        }

        public static Texture2D VariableWatchButtonTexture
        {
            get
            {
                if (BTreeEditorUtility.variableWatchButtonTexture == null)
                {
                    BTreeEditorUtility.initVariableWatchButtonTexture();
                }
                return BTreeEditorUtility.variableWatchButtonTexture;
            }
        }

        public static Texture2D VariableWatchButtonSelectedTexture
        {
            get
            {
                if (BTreeEditorUtility.variableWatchButtonSelectedTexture == null)
                {
                    BTreeEditorUtility.initVariableWatchButtonSelectedTexture();
                }
                return BTreeEditorUtility.variableWatchButtonSelectedTexture;
            }
        }

        public static Texture2D ReferencedTexture
        {
            get
            {
                if (BTreeEditorUtility.referencedTexture == null)
                {
                    BTreeEditorUtility.initReferencedTexture();
                }
                return BTreeEditorUtility.referencedTexture;
            }
        }

        public static Texture2D DeleteButtonTexture
        {
            get
            {
                if (BTreeEditorUtility.deleteButtonTexture == null)
                {
                    BTreeEditorUtility.initDeleteButtonTexture();
                }
                return BTreeEditorUtility.deleteButtonTexture;
            }
        }

        public static Texture2D IdentifyButtonTexture
        {
            get
            {
                if (BTreeEditorUtility.identifyButtonTexture == null)
                {
                    BTreeEditorUtility.initIdentifyButtonTexture();
                }
                return BTreeEditorUtility.identifyButtonTexture;
            }
        }

        public static Texture2D BreakpointTexture
        {
            get
            {
                if (BTreeEditorUtility.breakpointTexture == null)
                {
                    BTreeEditorUtility.initBreakpointTexture();
                }
                return BTreeEditorUtility.breakpointTexture;
            }
        }

        public static Texture2D EnableTaskTexture
        {
            get
            {
                if (BTreeEditorUtility.enableTaskTexture == null)
                {
                    BTreeEditorUtility.initEnableTaskTexture();
                }
                return BTreeEditorUtility.enableTaskTexture;
            }
        }

        public static Texture2D DisableTaskTexture
        {
            get
            {
                if (BTreeEditorUtility.disableTaskTexture == null)
                {
                    BTreeEditorUtility.initDisableTaskTexture();
                }
                return BTreeEditorUtility.disableTaskTexture;
            }
        }

        public static Texture2D ExpandTaskTexture
        {
            get
            {
                if (BTreeEditorUtility.expandTaskTexture == null)
                {
                    BTreeEditorUtility.initExpandTaskTexture();
                }
                return BTreeEditorUtility.expandTaskTexture;
            }
        }

        public static Texture2D CollapseTaskTexture
        {
            get
            {
                if (BTreeEditorUtility.collapseTaskTexture == null)
                {
                    BTreeEditorUtility.initCollapseTaskTexture();
                }
                return BTreeEditorUtility.collapseTaskTexture;
            }
        }

        public static Texture2D ExecutionSuccessTexture
        {
            get
            {
                if (BTreeEditorUtility.executionSuccessTexture == null)
                {
                    BTreeEditorUtility.initExecutionSuccessTexture();
                }
                return BTreeEditorUtility.executionSuccessTexture;
            }
        }

        public static Texture2D ExecutionFailureTexture
        {
            get
            {
                if (BTreeEditorUtility.executionFailureTexture == null)
                {
                    BTreeEditorUtility.initExecutionFailureTexture();
                }
                return BTreeEditorUtility.executionFailureTexture;
            }
        }

        public static string SplitCamelCase(string s)
        {
            if (s.Equals(""))
            {
                return s;
            }
            s = s.Replace("_uScript", "uScript");
            s = s.Replace("_PlayMaker", "PlayMaker");
            if (s.Length > 2 && s.Substring(0, 2).CompareTo("m_") == 0)
            {
                s = s.Substring(2, s.Length - 2);
            }
            Regex regex = new Regex("(?<=[A-Z])(?=[A-Z][a-z])|(?<=[^A-Z])(?=[A-Z])|(?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
            s = regex.Replace(s, " ");
            s = s.Replace("_", " ");
            s = s.Replace("u Script", " uScript");
            s = s.Replace("Play Maker", "PlayMaker");
            return (char.ToUpper(s[0]) + s.Substring(1)).Trim();
        }


        private static string GetEditorBaseDirectory(ScriptableObject obj = null)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            string text = Uri.UnescapeDataString(new UriBuilder(codeBase).Path);
            return Path.GetDirectoryName(text.Substring(Application.dataPath.Length - 6));
        }

        public static Texture2D LoadTexture(string imageName, bool useSkinColor = true, ScriptableObject obj = null)
        {
            Texture2D texture2D = null;
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BehaviorDesignerEditor.Resources.{0}{1}", useSkinColor ? (EditorGUIUtility.isProSkin ? "Dark" : "Light") : "", imageName));
            if (manifestResourceStream != null)
            {
                texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(BTreeEditorUtility.ReadToEnd(manifestResourceStream));
                manifestResourceStream.Close();
            }
            if (texture2D != null)
            {
                texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }
            return texture2D;
        }

        public static Texture2D LoadIcon(string iconName, ScriptableObject obj = null)
        {
            Texture2D texture2D = null;
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BehaviorDesignerEditor.Resources.{0}", iconName.Replace("{SkinColor}", EditorGUIUtility.isProSkin ? "Dark" : "Light")));
            if (manifestResourceStream != null)
            {
                texture2D = new Texture2D(0, 0);
                texture2D.LoadImage(BTreeEditorUtility.ReadToEnd(manifestResourceStream));
                manifestResourceStream.Close();
            }
            if (texture2D == null)
            {
                string path = "Assets/Editor/BTreeEditor/Res/";
                texture2D = (AssetDatabase.LoadAssetAtPath(path + iconName.Replace("{SkinColor}", EditorGUIUtility.isProSkin ? "Dark" : "Light"), typeof(Texture2D)) as Texture2D);
            }
            if (texture2D != null)
            {
                texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }
            return texture2D;
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            byte[] array = new byte[16384];
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int count;
                while ((count = stream.Read(array, 0, array.Length)) > 0)
                {
                    memoryStream.Write(array, 0, count);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static void DrawContentSeperator(int yOffset)
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x = -5f;
            lastRect.y = (lastRect.y + (lastRect.height + (float)yOffset));
            lastRect.height = (2f);
            lastRect.width = (lastRect.width + 10f);
            GUI.DrawTexture(lastRect, BTreeEditorUtility.ContentSeparatorTexture);
        }

        private static void initGraphStatusGUIStyle()
        {
            BTreeEditorUtility.graphStatusGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.graphStatusGUIStyle.alignment = TextAnchor.MiddleLeft;
            BTreeEditorUtility.graphStatusGUIStyle.fontSize = 20;
            BTreeEditorUtility.graphStatusGUIStyle.fontStyle = FontStyle.Bold;
            if (EditorGUIUtility.isProSkin)
            {
                BTreeEditorUtility.graphStatusGUIStyle.normal.textColor = new Color(0.7058f, 0.7058f, 0.7058f);
                return;
            }
            BTreeEditorUtility.graphStatusGUIStyle.normal.textColor = new Color(0.8058f, 0.8058f, 0.8058f);
        }

        private static void initTaskFoldoutGUIStyle()
        {
            BTreeEditorUtility.taskFoldoutGUIStyle = new GUIStyle(EditorStyles.foldout);
            BTreeEditorUtility.taskFoldoutGUIStyle.alignment = TextAnchor.MiddleLeft;
            BTreeEditorUtility.taskFoldoutGUIStyle.fontSize = 15;
            BTreeEditorUtility.taskFoldoutGUIStyle.fontStyle = FontStyle.Bold;
        }

        private static void initTaskTitleGUIStyle()
        {
            BTreeEditorUtility.taskTitleGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.taskTitleGUIStyle.alignment = TextAnchor.UpperCenter;
            BTreeEditorUtility.taskTitleGUIStyle.fontSize = 12;
            BTreeEditorUtility.taskTitleGUIStyle.fontStyle = FontStyle.Normal;
        }

        private static void initTaskGUIStyle()
        {
            BTreeEditorUtility.taskGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("Task.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskSelectedGUIStyle()
        {
            BTreeEditorUtility.taskSelectedGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("TaskSelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static void initTaskRunningGUIStyle()
        {
            BTreeEditorUtility.taskRunningGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("TaskRunning.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskRunningSelectedGUIStyle()
        {
            BTreeEditorUtility.taskRunningSelectedGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("TaskRunningSelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static void initTaskIdentifyGUIStyle()
        {
            BTreeEditorUtility.taskIdentifyGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("TaskIdentify.png", true, null), new RectOffset(5, 3, 3, 5));
        }

        private static void initTaskIdentifySelectedGUIStyle()
        {
            BTreeEditorUtility.taskIdentifySelectedGUIStyle = BTreeEditorUtility.initTaskGUIStyle(BTreeEditorUtility.LoadTexture("TaskIdentifySelected.png", true, null), new RectOffset(5, 4, 4, 4));
        }

        private static GUIStyle initTaskGUIStyle(Texture2D texture, RectOffset overflow)
        {
            GUIStyle gUIStyle = new GUIStyle(GUI.skin.box);
            gUIStyle.border = (new RectOffset(10, 10, 10, 10));
            gUIStyle.overflow = (overflow);
            gUIStyle.normal.background = (texture);
            gUIStyle.active.background = (texture);
            gUIStyle.hover.background = (texture);
            gUIStyle.focused.background = (texture);
            gUIStyle.normal.textColor = (Color.white);
            gUIStyle.active.textColor = (Color.white);
            gUIStyle.hover.textColor = (Color.white);
            gUIStyle.focused.textColor = (Color.white);
            gUIStyle.stretchHeight = (true);
            gUIStyle.stretchWidth = (true);
            return gUIStyle;
        }

        private static void initTaskCommentGUIStyle()
        {
            BTreeEditorUtility.taskCommentGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.taskCommentGUIStyle.alignment = TextAnchor.UpperCenter;
            BTreeEditorUtility.taskCommentGUIStyle.fontSize = (12);
            BTreeEditorUtility.taskCommentGUIStyle.fontStyle = FontStyle.Normal;
            BTreeEditorUtility.taskCommentGUIStyle.wordWrap = (true);
        }

        private static void initTaskCommentLeftAlignGUIStyle()
        {
            BTreeEditorUtility.taskCommentLeftAlignGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.taskCommentLeftAlignGUIStyle.alignment = TextAnchor.UpperLeft;
            BTreeEditorUtility.taskCommentLeftAlignGUIStyle.fontSize = (12);
            BTreeEditorUtility.taskCommentLeftAlignGUIStyle.fontStyle = FontStyle.Normal;
            BTreeEditorUtility.taskCommentLeftAlignGUIStyle.wordWrap = (false);
        }

        private static void initTaskCommentRightAlignGUIStyle()
        {
            BTreeEditorUtility.taskCommentRightAlignGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.taskCommentRightAlignGUIStyle.alignment = TextAnchor.UpperRight;
            BTreeEditorUtility.taskCommentRightAlignGUIStyle.fontSize = (12);
            BTreeEditorUtility.taskCommentRightAlignGUIStyle.fontStyle = FontStyle.Normal;
            BTreeEditorUtility.taskCommentRightAlignGUIStyle.wordWrap = (false);
        }

        private static void initGraphBackgroundGUIStyle()
        {
            Texture2D texture2D = new Texture2D(1, 1);
            if (EditorGUIUtility.isProSkin)
            {
                texture2D.SetPixel(1, 1, new Color(0.1333f, 0.1333f, 0.1333f));
            }
            else
            {
                texture2D.SetPixel(1, 1, new Color(0.3647f, 0.3647f, 0.3647f));
            }
            texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            texture2D.Apply();
            BTreeEditorUtility.graphBackgroundGUIStyle = new GUIStyle(GUI.skin.box);
            BTreeEditorUtility.graphBackgroundGUIStyle.normal.background = (texture2D);
            BTreeEditorUtility.graphBackgroundGUIStyle.active.background = (texture2D);
            BTreeEditorUtility.graphBackgroundGUIStyle.hover.background = (texture2D);
            BTreeEditorUtility.graphBackgroundGUIStyle.focused.background = (texture2D);
            BTreeEditorUtility.graphBackgroundGUIStyle.normal.textColor = (Color.white);
            BTreeEditorUtility.graphBackgroundGUIStyle.active.textColor = (Color.white);
            BTreeEditorUtility.graphBackgroundGUIStyle.hover.textColor = (Color.white);
            BTreeEditorUtility.graphBackgroundGUIStyle.focused.textColor = (Color.white);
        }

        private static void initSelectionGUIStyle()
        {
            Texture2D texture2D = new Texture2D(1, 1);
            Color color;
            if (EditorGUIUtility.isProSkin)
            {
                color = new Color(0.188f, 0.4588f, 0.6862f, 0.5f);
            }
            else
            {
                color = new Color(0.243f, 0.5686f, 0.839f, 0.5f);
            }
            texture2D.SetPixel(1, 1, color);
            texture2D.hideFlags = HideFlags.HideInHierarchy | HideFlags.NotEditable | HideFlags.DontSaveInEditor;
            texture2D.Apply();
            BTreeEditorUtility.selectionGUIStyle = new GUIStyle(GUI.skin.box);
            BTreeEditorUtility.selectionGUIStyle.normal.background = (texture2D);
            BTreeEditorUtility.selectionGUIStyle.active.background = (texture2D);
            BTreeEditorUtility.selectionGUIStyle.hover.background = (texture2D);
            BTreeEditorUtility.selectionGUIStyle.focused.background = (texture2D);
            BTreeEditorUtility.selectionGUIStyle.normal.textColor = (Color.white);
            BTreeEditorUtility.selectionGUIStyle.active.textColor = (Color.white);
            BTreeEditorUtility.selectionGUIStyle.hover.textColor = (Color.white);
            BTreeEditorUtility.selectionGUIStyle.focused.textColor = (Color.white);
        }

        private static void initLabelWrapGUIStyle()
        {
            BTreeEditorUtility.labelWrapGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.labelWrapGUIStyle.wordWrap = (true);
            BTreeEditorUtility.labelWrapGUIStyle.alignment = TextAnchor.MiddleCenter;
        }

        private static void initTaskInspectorCommentGUIStyle()
        {
            BTreeEditorUtility.taskInspectorCommentGUIStyle = new GUIStyle(GUI.skin.textArea);
            BTreeEditorUtility.taskInspectorCommentGUIStyle.wordWrap = (true);
        }

        private static void initTaskInspectorGUIStyle()
        {
            BTreeEditorUtility.taskInspectorGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.taskInspectorGUIStyle.alignment = TextAnchor.MiddleLeft;
            BTreeEditorUtility.taskInspectorGUIStyle.fontSize = (11);
            BTreeEditorUtility.taskInspectorGUIStyle.fontStyle = FontStyle.Normal;
        }

        private static void initToolbarButtonSelectionGUIStyle()
        {
            BTreeEditorUtility.toolbarButtonSelectionGUIStyle = new GUIStyle(EditorStyles.toolbarButton);
            BTreeEditorUtility.toolbarButtonSelectionGUIStyle.normal.background = (BTreeEditorUtility.toolbarButtonSelectionGUIStyle.active.background);
        }

        private static void initPreferencesPaneGUIStyle()
        {
            BTreeEditorUtility.preferencesPaneGUIStyle = new GUIStyle(GUI.skin.box);
            BTreeEditorUtility.preferencesPaneGUIStyle.normal.background = (EditorStyles.toolbarButton.normal.background);
        }

        private static void initPropertyBoxGUIStyle()
        {
            BTreeEditorUtility.propertyBoxGUIStyle = new GUIStyle();
            BTreeEditorUtility.propertyBoxGUIStyle.padding = (new RectOffset(2, 2, 0, 0));
        }

        private static void initPlainButtonGUIStyle()
        {
            BTreeEditorUtility.plainButtonGUIStyle = new GUIStyle(GUI.skin.button);
            BTreeEditorUtility.plainButtonGUIStyle.border = (new RectOffset(0, 0, 0, 0));
            BTreeEditorUtility.plainButtonGUIStyle.margin = (new RectOffset(0, 0, 2, 2));
            BTreeEditorUtility.plainButtonGUIStyle.padding = (new RectOffset(0, 0, 1, 0));
            BTreeEditorUtility.plainButtonGUIStyle.normal.background = (null);
            BTreeEditorUtility.plainButtonGUIStyle.active.background = (null);
            BTreeEditorUtility.plainButtonGUIStyle.hover.background = (null);
            BTreeEditorUtility.plainButtonGUIStyle.focused.background = (null);
            BTreeEditorUtility.plainButtonGUIStyle.normal.textColor = (Color.white);
            BTreeEditorUtility.plainButtonGUIStyle.active.textColor = (Color.white);
            BTreeEditorUtility.plainButtonGUIStyle.hover.textColor = (Color.white);
            BTreeEditorUtility.plainButtonGUIStyle.focused.textColor = (Color.white);
        }

        private static void initTransparentButtonGUIStyle()
        {
            BTreeEditorUtility.transparentButtonGUIStyle = new GUIStyle(GUI.skin.button);
            BTreeEditorUtility.transparentButtonGUIStyle.border = (new RectOffset(0, 0, 0, 0));
            BTreeEditorUtility.transparentButtonGUIStyle.margin = (new RectOffset(4, 4, 2, 2));
            BTreeEditorUtility.transparentButtonGUIStyle.padding = (new RectOffset(2, 2, 1, 0));
            BTreeEditorUtility.transparentButtonGUIStyle.normal.background = (null);
            BTreeEditorUtility.transparentButtonGUIStyle.active.background = (null);
            BTreeEditorUtility.transparentButtonGUIStyle.hover.background = (null);
            BTreeEditorUtility.transparentButtonGUIStyle.focused.background = (null);
            BTreeEditorUtility.transparentButtonGUIStyle.normal.textColor = (Color.white);
            BTreeEditorUtility.transparentButtonGUIStyle.active.textColor = (Color.white);
            BTreeEditorUtility.transparentButtonGUIStyle.hover.textColor = (Color.white);
            BTreeEditorUtility.transparentButtonGUIStyle.focused.textColor = (Color.white);
        }

        private static void initWelcomeScreenIntroGUIStyle()
        {
            BTreeEditorUtility.welcomeScreenIntroGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.welcomeScreenIntroGUIStyle.fontSize = (16);
            BTreeEditorUtility.welcomeScreenIntroGUIStyle.fontStyle = FontStyle.Bold;
            BTreeEditorUtility.welcomeScreenIntroGUIStyle.normal.textColor = (new Color(0.706f, 0.706f, 0.706f));
        }

        private static void initWelcomeScreenTextHeaderGUIStyle()
        {
            BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle.alignment = TextAnchor.MiddleLeft;
            BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle.fontSize = (14);
            BTreeEditorUtility.welcomeScreenTextHeaderGUIStyle.fontStyle = FontStyle.Bold;
        }

        private static void initWelcomeScreenTextDescriptionGUIStyle()
        {
            BTreeEditorUtility.welcomeScreenTextDescriptionGUIStyle = new GUIStyle(GUI.skin.label);
            BTreeEditorUtility.welcomeScreenTextDescriptionGUIStyle.wordWrap = (true);
        }

        private static void initTaskBorderTexture()
        {
            BTreeEditorUtility.taskBorderTexture = BTreeEditorUtility.LoadTexture("TaskBorder.png", true, null);
        }

        private static void initTaskBorderRunningTexture()
        {
            BTreeEditorUtility.taskBorderRunningTexture = BTreeEditorUtility.LoadTexture("TaskBorderRunning.png", true, null);
        }

        private static void initTaskBorderIdentifyTexture()
        {
            BTreeEditorUtility.taskBorderIdentifyTexture = BTreeEditorUtility.LoadTexture("TaskBorderIdentify.png", true, null);
        }

        private static void initTaskConnectionTexture()
        {
            BTreeEditorUtility.taskConnectionTexture = BTreeEditorUtility.LoadTexture("TaskConnection.png", false, null);
        }

        private static void initTaskConnectionTopTexture()
        {
            BTreeEditorUtility.taskConnectionTopTexture = BTreeEditorUtility.LoadTexture("TaskConnectionTop.png", true, null);
        }

        private static void initTaskConnectionBottomTexture()
        {
            BTreeEditorUtility.taskConnectionBottomTexture = BTreeEditorUtility.LoadTexture("TaskConnectionBottom.png", true, null);
        }

        private static void initTaskConnectionRunningTopTexture()
        {
            BTreeEditorUtility.taskConnectionRunningTopTexture = BTreeEditorUtility.LoadTexture("TaskConnectionRunningTop.png", true, null);
        }

        private static void initTaskConnectionRunningBottomTexture()
        {
            BTreeEditorUtility.taskConnectionRunningBottomTexture = BTreeEditorUtility.LoadTexture("TaskConnectionRunningBottom.png", true, null);
        }

        private static void initTaskConnectionIdentifyTopTexture()
        {
            BTreeEditorUtility.taskConnectionIdentifyTopTexture = BTreeEditorUtility.LoadTexture("TaskConnectionIdentifyTop.png", true, null);
        }

        private static void initTaskConnectionIdentifyBottomTexture()
        {
            BTreeEditorUtility.taskConnectionIdentifyBottomTexture = BTreeEditorUtility.LoadTexture("TaskConnectionIdentifyBottom.png", true, null);
        }

        private static void initTaskConnectionCollapsedTexture()
        {
            BTreeEditorUtility.taskConnectionCollapsedTexture = BTreeEditorUtility.LoadTexture("TaskConnectionCollapsed.png", true, null);
        }

        private static void initContentSeparatorTexture()
        {
            BTreeEditorUtility.contentSeparatorTexture = BTreeEditorUtility.LoadTexture("ContentSeparator.png", true, null);
        }

        private static void initDocTexture()
        {
            BTreeEditorUtility.docTexture = BTreeEditorUtility.LoadTexture("DocIcon.png", true, null);
        }

        private static void initGearTexture()
        {
            BTreeEditorUtility.gearTexture = BTreeEditorUtility.LoadTexture("GearIcon.png", true, null);
        }

        private static void initSyncedTexture()
        {
            BTreeEditorUtility.syncedTexture = BTreeEditorUtility.LoadTexture("SyncedIcon.png", true, null);
        }

        private static void initSharedTexture()
        {
            BTreeEditorUtility.sharedTexture = BTreeEditorUtility.LoadTexture("SharedIcon.png", true, null);
        }

        private static void initVariableButtonTexture()
        {
            BTreeEditorUtility.variableButtonTexture = BTreeEditorUtility.LoadTexture("VariableButton.png", true, null);
        }

        private static void initVariableButtonSelectedTexture()
        {
            BTreeEditorUtility.variableButtonSelectedTexture = BTreeEditorUtility.LoadTexture("VariableButtonSelected.png", true, null);
        }

        private static void initVariableWatchButtonTexture()
        {
            BTreeEditorUtility.variableWatchButtonTexture = BTreeEditorUtility.LoadTexture("VariableWatchButton.png", true, null);
        }

        private static void initVariableWatchButtonSelectedTexture()
        {
            BTreeEditorUtility.variableWatchButtonSelectedTexture = BTreeEditorUtility.LoadTexture("VariableWatchButtonSelected.png", true, null);
        }

        private static void initReferencedTexture()
        {
            BTreeEditorUtility.referencedTexture = BTreeEditorUtility.LoadTexture("LinkedIcon.png", true, null);
        }

        private static void initDeleteButtonTexture()
        {
            BTreeEditorUtility.deleteButtonTexture = BTreeEditorUtility.LoadTexture("DeleteButton.png", true, null);
        }

        private static void initIdentifyButtonTexture()
        {
            BTreeEditorUtility.identifyButtonTexture = BTreeEditorUtility.LoadTexture("IdentifyButton.png", true, null);
        }

        private static void initBreakpointTexture()
        {
            BTreeEditorUtility.breakpointTexture = BTreeEditorUtility.LoadTexture("BreakpointIcon.png", false, null);
        }

        private static void initEnableTaskTexture()
        {
            BTreeEditorUtility.enableTaskTexture = BTreeEditorUtility.LoadTexture("TaskEnableIcon.png", false, null);
        }

        private static void initDisableTaskTexture()
        {
            BTreeEditorUtility.disableTaskTexture = BTreeEditorUtility.LoadTexture("TaskDisableIcon.png", false, null);
        }

        private static void initExpandTaskTexture()
        {
            BTreeEditorUtility.expandTaskTexture = BTreeEditorUtility.LoadTexture("TaskExpandIcon.png", false, null);
        }

        private static void initCollapseTaskTexture()
        {
            BTreeEditorUtility.collapseTaskTexture = BTreeEditorUtility.LoadTexture("TaskCollapseIcon.png", false, null);
        }

        private static void initExecutionSuccessTexture()
        {
            BTreeEditorUtility.executionSuccessTexture = BTreeEditorUtility.LoadTexture("ExecutionSuccess.png", false, null);
        }

        private static void initExecutionFailureTexture()
        {
            BTreeEditorUtility.executionFailureTexture = BTreeEditorUtility.LoadTexture("ExecutionFailure.png", false, null);
        }
    }
}
