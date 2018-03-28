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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public static class UnityExtension
{
    #region Transform
    public static void InitTransform(this Component cmpt)
    {
        Transform trans = cmpt.transform;
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    #region active / parent
    public static void SetActive(this Component cmpt, bool bActive)
    {
        cmpt.gameObject.SetActive(bActive);
    }

    public static void SetParentPure(this Component cmpt, Component parent)
    {
        cmpt.transform.SetParent(parent.transform);
    }
    public static void SetParentPure(this GameObject obj, Component parent)
    {
        obj.transform.SetParent(parent.transform);
    }

    //public static void SetParentEx(this Component cmpt, Component parent)
    //{
    //    LuaHelper.SetParent(cmpt.transform, parent);
    //}
    //public static void SetParentEx(this GameObject obj, Component parent)
    //{
    //    LuaHelper.SetParent(obj.transform, parent);
    //}
    #endregion

    #region local position
    //-----------读取相对位置
    public static void SetLocalPositionZero(this Component cmpt)
    {
        cmpt.transform.localPosition = Vector3.zero;
    }

    public static void SetLocalPositionEx(this Component cmpt, float x, float y, float z)
    {
        cmpt.transform.localPosition = new Vector3(x, y, z);
    }
    public static void SetLocalPosition(this Component cmpt, Vector3 pos)
    {
        cmpt.transform.localPosition = pos;
    }

    public static void SetLocalPositionX(this Component cmpt, float x)
    {
        Vector3 pos = cmpt.transform.localPosition;
        pos.x = x;
        cmpt.transform.localPosition = pos;
    }
    public static void SetLocalPositionY(this Component cmpt, float y)
    {
        Vector3 pos = cmpt.transform.localPosition;
        pos.y = y;
        cmpt.transform.localPosition = pos;
    }
    public static void SetLocalPositionZ(this Component cmpt, float z)
    {
        Vector3 pos = cmpt.transform.localPosition;
        pos.z = z;
        cmpt.transform.localPosition = pos;
    }
    public static void SetLocalPositionFromTarget(this Component cmpt, Transform target)
    {
        cmpt.transform.localPosition = target.localPosition;
    }
    public static Vector3 GetLocalPosition(this Component cmpt)
    {
        return cmpt.transform.localPosition;
    }
    public static void GetLocalPositionEx(this Component cmpt, out float x, out float y, out float z)
    {
        Transform trans = cmpt.transform;
        x = trans.localPosition.x;
        y = trans.localPosition.y;
        z = trans.localPosition.z;
    }
    public static float GetLocalPositionY(this Component cmpt)
    {
        return cmpt.transform.localPosition.y;
    }
    #endregion

    #region world position
    //-----------读取世界坐标位置
    public static void SetPostionZero(this Component cmpt)
    {
        cmpt.transform.position = Vector3.zero;
    }
    public static void SetPositionEx(this Component cmpt, float x, float y, float z)
    {
        cmpt.transform.position = new Vector3(x, y, z);
    }
    public static void SetPosition(this Component cmpt, Vector3 pos)
    {
        cmpt.transform.position = pos;
    }
    public static void SetPositionFromTarget(this Component cmpt, Transform target)
    {
        cmpt.transform.position = target.position;
    }
    public static Vector3 GetPosition(this Component cmpt)
    {
        return cmpt.transform.position;
    }

    public static void GetPositionEx(this Component cmpt, out float x, out float y, out float z)
    {
        Transform trans = cmpt.transform;
        x = trans.position.x;
        y = trans.position.y;
        z = trans.position.z;
    }
    #endregion

    #region local scale
    //----------------读取缩放
    public static void SetLocalScaleOne(this Component cmpt)
    {
        cmpt.transform.localScale = Vector3.one;
    }
    public static void SetLocalScaleEx(this Component cmpt, float x, float y, float z)
    {
        cmpt.transform.localScale = new Vector3(x, y, z);
    }
    public static void SetLocalScale(this Component cmpt, Vector3 scale)
    {
        cmpt.transform.localScale = scale;
    }
    public static void SetLocalScaleVal(this Component cmpt, float val)
    {
        cmpt.transform.localScale = new Vector3(val, val, val);
    }

    #endregion

    #region  rotation
    //-----------------旋转
    public static void SetLocalRotationOne(this Component cmpt)
    {
        cmpt.transform.rotation = Quaternion.identity;
    }
    public static void SetLocalEulerAngle(this Component cmpt, float x, float y, float z)
    {
        cmpt.transform.localRotation = Quaternion.Euler(new Vector3(x, y, z));
    }
    public static void SetLocalEulerAngleZ(this Component cmpt, float z)
    {
        var v3 = cmpt.transform.localEulerAngles;
        v3.z = z;
        cmpt.transform.localEulerAngles = v3;
    }
    public static void SetLocalEulerAngleY(this Component cmpt, float y)
    {
        var v3 = cmpt.transform.localEulerAngles;
        v3.y = y;
        cmpt.transform.localEulerAngles = v3;
    }

    public static void GetLocalEulerAngle(this Component cmpt, out float x, out float y, out float z)
    {
        Transform trans = cmpt.transform;
        x = trans.localEulerAngles.x;
        y = trans.localEulerAngles.y;
        z = trans.localEulerAngles.z;
    }
    public static Vector3 GetEulerAngle(this Component cmpt)
    {
        return cmpt.transform.rotation.eulerAngles;
    }
    public static float GetEulerAngleY(this Component cmpt)
    {
        return cmpt.transform.rotation.eulerAngles.y;
    }
    public static float GetLocalEulerAngleY(this Component cmpt)
    {
        return cmpt.transform.localRotation.eulerAngles.y;
    }

    public static void RotateAxisY(this Component cmpt, float y, Space relativeTo = Space.Self)
    {
        cmpt.transform.Rotate(0, y, 0, relativeTo);
    }
    #endregion

    #endregion

    #region RectTransfrom
    public static void SetSizeDelta(this Component cmpt, Vector2 size)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.sizeDelta = size;
    }

    public static void SetSizeDeltaEx(this Component cmpt, float x, float y)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.sizeDelta = new Vector2(x, y);
    }

    public static void SetSizeDeltaX(this Component cmpt, float x)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.sizeDelta = new Vector2(x, rectTrans.sizeDelta.y);
    }

    public static void SetSizeDeltaY(this Component cmpt, float y)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, y);
    }

    public static void GetSizeDeltaEx(this Component cmpt, out float x, out float y)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        x = rectTrans.sizeDelta.x;
        y = rectTrans.sizeDelta.y;
    }

    public static void GetSizeDeltaY(this Component cmpt, out float y)
    {
        y = (cmpt.transform as RectTransform).sizeDelta.y;
    }

    public static void GetAnchoredPosition(this Component cmpt, out float x, out float y)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        x = rectTrans.anchoredPosition.x;
        y = rectTrans.anchoredPosition.y;
    }

    public static void GetAnchoredPositionY(this Component cmpt, out float y)
    {
        y = (cmpt.transform as RectTransform).anchoredPosition.y;
    }

    public static void SetAnchoredPosition(this Component cmpt, Vector2 pos)
    {
        (cmpt.transform as RectTransform).anchoredPosition = pos;
    }
    public static void SetAnchoredPositionEx(this Component cmpt, float x, float y)
    {
        (cmpt.transform as RectTransform).anchoredPosition = new Vector2(x, y);
    }


    public static void SetAnchoredPositionX(this Component cmpt, float x)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.anchoredPosition = new Vector2(x, rectTrans.anchoredPosition.y);
    }

    public static void SetAnchoredPositionY(this Component cmpt, float y)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        rectTrans.anchoredPosition = new Vector2(rectTrans.anchoredPosition.x, y);
    }

    public static void GetRectWidth(this Component cmpt, out float width)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        width = rectTrans.rect.width;
    }

    public static void GetRectHeight(this Component cmpt, out float height)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        height = rectTrans.rect.height;
    }

    public static void GetRectSize(this Component cmpt, out float width, out float height)
    {
        RectTransform rectTrans = cmpt.transform as RectTransform;
        width = rectTrans.rect.width;
        height = rectTrans.rect.height;
    }

    public static void SetFontColor(this Text txt, float r, float g, float b, float a)
    {
        txt.color = new Color(r, g, b, a);
    }

    public static Vector2 TransformToCanvasLocalPosition(this Transform current, Canvas canvas)
    {
        var screenPos = canvas.worldCamera.WorldToScreenPoint(current.transform.position);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPos, canvas.worldCamera, out localPos);
        return localPos;
    }

    #endregion

    #region graphic
    public static void SetGraphicAlpha(this Graphic gp, float a)
    {
        Color clr = gp.color;
        clr.a = a;
        gp.color = clr;
    }
    #endregion

    #region gesture
    //public static void GetGesturePos(this Gesture gesture, out float x, out float y)
    //{
    //    x = gesture.position.x;
    //    y = gesture.position.y;
    //}

    //public static void GetGestureDeltaPos(this Gesture gesture, out float x, out float y)
    //{
    //    x = gesture.deltaPosition.x;
    //    y = gesture.deltaPosition.y;
    //}
    #endregion

    #region 位置相关计算
    public static float DistanceLocalToPos(this Transform trans, Vector3 pos)
    {
        return Vector3.Distance(trans.localPosition, pos);
    }
    public static float DistanceToPos(this Transform trans, Vector3 pos)
    {
        return Vector3.Distance(trans.position, pos);
    }

    public static float DistanceToTarget(this Transform trans, Transform target)
    {
        return Vector3.Distance(trans.position, target.position);
    }

    //距离的平方，只是用来作比较的时候可以节省性能
    public static float SqrDisToPos(this Transform trans, Vector3 pos)
    {
        Vector3 disVec = pos - trans.position;
        return disVec.sqrMagnitude;
    }


    public static float AngleYLocalToPos(this Transform trans, Vector3 pos)
    {
        Vector3 delta = pos - trans.localPosition;
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(delta.z, delta.x));
        return angle;
    }

    #endregion

    #region 国际化
    /// <summary>
    /// 重置LangItem
    /// </summary>
    /// <param name="mtran"></param>
    //public static void ResetLangItem(this Component cmpt)
    //{
    //    //获取子对象组件
    //    UILangItem[] items = cmpt.transform.GetComponentsInChildren<UILangItem>(true);
    //    for (int i = 0, len = items.Length; i < len; i++)
    //    {
    //        items[i].SetText();
    //    }
    //}

    public static string[] GetPath(this Component cmpt, Transform parent)
    {
        List<string> paths = new List<string>();
        Transform child = cmpt.transform;
        while (child.parent && child.parent != parent)
        {
            paths.Add(child.name);
            child = child.parent;
        }
        paths.Reverse();
        return paths.ToArray();
    }
    #endregion

    #region 图文混排
    //private static readonly Regex s_ImgRegex =
    //    new Regex(@"<img\s*src=([^>\n\s]+)\s*>", RegexOptions.RightToLeft);
    const string LANGUAGE_ARAB = "arab";

    //public static void MixedLayout(this Component cmpt, string content)
    //{
    //    List<SoarDText> texts = new List<SoarDText>();
    //    List<SDImage> images = new List<SDImage>();
    //    texts.AddRange(cmpt.GetComponentsInChildren<SoarDText>(true));
    //    images.AddRange(cmpt.GetComponentsInChildren<SDImage>(true));
    //    if (texts.Count > 0 && images.Count > 0)
    //    {
    //        for (int i = texts.Count - 1; i >= 0; i--) texts[i].gameObject.SetActive(false);
    //        for (int i = images.Count - 1; i >= 0; i--) images[i].gameObject.SetActive(false);

    //        if (!string.IsNullOrEmpty(content))
    //        {
    //            bool isRTL = ArabicFixerTool.CheckRTL(content);//是否阿语
    //            List<MixedContent> contents = new List<MixedContent>();

    //            //解析文本
    //            if (isRTL)
    //            {
    //                int endIndex = content.Length;
    //                foreach (Match match in s_ImgRegex.Matches(content))
    //                {
    //                    int index = match.Index + match.Length;
    //                    if (index < endIndex)
    //                        contents.Add(new MixedContent(MixedType.Text, content.Substring(index, endIndex - index)));
    //                    contents.Add(new MixedContent(MixedType.Image, match.Groups[0].Value, match.Groups[1].Value));
    //                    endIndex = match.Index;
    //                }

    //                if (endIndex > 0)
    //                    contents.Add(new MixedContent(MixedType.Text, content.Substring(0, endIndex)));
    //            }
    //            else
    //            {
    //                int endIndex = 0;
    //                foreach (Match match in s_ImgRegex.Matches(content))
    //                {
    //                    int index = match.Index;
    //                    if (index > endIndex)
    //                        contents.Add(new MixedContent(MixedType.Text, content.Substring(endIndex, index - endIndex)));
    //                    contents.Add(new MixedContent(MixedType.Image, match.Groups[0].Value, match.Groups[1].Value));
    //                    endIndex = index + match.Length;
    //                }

    //                if (endIndex < content.Length)
    //                    contents.Add(new MixedContent(MixedType.Text, content.Substring(endIndex, content.Length - endIndex)));
    //            }

    //            //生成对应的组件
    //            for (int i = 0, len = contents.Count; i < len; i++)
    //            {
    //                MixedContent con = contents[i];
    //                switch (con.Type)
    //                {
    //                    case MixedType.Text:
    //                        SoarDText text = GetIdleCmpt(texts);
    //                        text.transform.SetSiblingIndex(i);
    //                        text.text = con.Content;
    //                        break;
    //                    case MixedType.Image:
    //                        SDImage image = GetIdleCmpt(images);
    //                        image.transform.SetSiblingIndex(i);
    //                        if (Config.Instance.Language == LANGUAGE_ARAB)
    //                            image.SetSpriteName("client_logo_arab");
    //                        else
    //                            image.SetSpriteName("client_logo_en");
    //                        image.SetImage(con.Url);
    //                        break;
    //                }
    //            }
    //        }
    //    }
    //}

    static T GetIdleCmpt<T>(List<T> list)
    {
        Component cmpt = null;
        for (int i = 0, len = list.Count; i < len; i++)
        {
            cmpt = list[i] as Component;
            if (!cmpt.gameObject.activeInHierarchy)
            {
                cmpt.gameObject.SetActive(true);
                return list[i];
            }
        }

        cmpt = list[0] as Component;
        GameObject newObj = UnityEngine.Object.Instantiate(cmpt.gameObject);
        newObj.transform.parent = cmpt.transform.parent;
        newObj.transform.localScale = Vector3.one;
        T t = newObj.GetComponent<T>();
        list.Add(t);

        return t;
    }

    struct MixedContent
    {
        public MixedType Type;
        public string Content;
        public string Url;

        public MixedContent(MixedType type, string cont, string url = null)
        {
            Type = type;
            Content = cont;
            Url = url;
        }
    }

    enum MixedType
    {
        Text = 1,
        Image = 2,
    }
    #endregion
}
