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

using UnityEngine;

namespace BTree.Editor
{
    public static class RectExtensions
    {
        public static Vector2 TopLeft(this Rect rect)
        {
            return new Vector2(rect.xMin, rect.yMin);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x = (result.x - pivotPoint.x);
            result.y = (result.y - pivotPoint.y);
            result.x = (result.xMin * scale);
            result.x = (result.xMax * scale);
            result.y = (result.yMin * scale);
            result.y = (result.yMax * scale);
            result.x = (result.x + pivotPoint.x);
            result.y = (result.y + pivotPoint.y);
            return result;
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
        {
            return rect.ScaleSizeBy(scale, rect.center);
        }

        public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
        {
            Rect result = rect;
            result.x = (result.x - pivotPoint.x);
            result.y = (result.y - pivotPoint.y);
            result.x = (result.xMin * scale.x);
            result.x = (result.xMax * scale.x);
            result.y = (result.yMin * scale.y);
            result.y = (result.yMax * scale.y);
            result.x = (result.x + pivotPoint.x);
            result.y = (result.y + pivotPoint.y);
            return result;
        }
    }
}
