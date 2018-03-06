using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BTree.Editor
{
    public class BTreeEditorZoomArea
    {
        private static Matrix4x4 _prevGuiMatrix;

        public static Rect Begin(Rect screenCoordsArea, float zoomScale)
        {
            GUI.EndGroup();
            Rect rect = screenCoordsArea;
            //Rect rect = screenCoordsArea.ScaleSizeBy(1f / zoomScale, screenCoordsArea.TopLeft());
            rect.y = (rect.y + (float)BTreeEditorUtility.EditorWindowTabHeight);
            GUI.BeginGroup(rect);
            _prevGuiMatrix = GUI.matrix;
            Matrix4x4 matrix4x = Matrix4x4.TRS(rect.TopLeft(), Quaternion.identity, Vector3.one);
            Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1f));
            GUI.matrix = (matrix4x * matrix4x2 * matrix4x.inverse * GUI.matrix);
            return rect;
        }

        public static void End()
        {
            GUI.matrix = (_prevGuiMatrix);
            GUI.EndGroup();
            GUI.BeginGroup(new Rect(0f, (float)BTreeEditorUtility.EditorWindowTabHeight, (float)Screen.width, (float)Screen.height));
        }
    }
}
