using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorWindow : EditorWindow
    {
        [SerializeField]
        public static BTreeEditorWindow instance;
        [MenuItem("Window/BTree Editor")]
        public static void ShowWindow()
        {
            BTreeEditorWindow bTreeEditorWindow = EditorWindow.GetWindow(typeof(BTreeEditorWindow)) as BTreeEditorWindow;
            bTreeEditorWindow.wantsMouseMove = true;
            bTreeEditorWindow.minSize = new Vector2(600f, 500f);
            Object.DontDestroyOnLoad(bTreeEditorWindow);
        }

        private Vector2 mCurrentMousePosition = Vector2.zero;

        public void OnGUI()
        {
            this.mCurrentMousePosition = Event.current.mousePosition;
        }
    }
}
