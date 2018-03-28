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

public class Singleton<T> where T : class, new()
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                CreateInstance();
            }
            return m_Instance;
        }
    }

    public static void CreateInstance()
    {
        if (m_Instance == null)
        {
            m_Instance = Activator.CreateInstance<T>();
            (m_Instance as Singleton<T>).Init();
        }
    }

    public static void DestroyInstance()
    {
        if (m_Instance != null)
        {
            (m_Instance as Singleton<T>).Dispose();
            m_Instance = null;
        }
    }

    public static T GetInstance()
    {
        if (m_Instance == null)
        {
            CreateInstance();
        }
        return m_Instance;
    }

    public static bool HasInstance()
    {
        return m_Instance != null;
    }

    protected Singleton() { }

    public virtual void Init() { }

    public virtual void Dispose() { }
}
