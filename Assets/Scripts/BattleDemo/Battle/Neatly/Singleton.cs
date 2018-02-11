#region Copyright © 2017 OneMT. All rights reserved.
//=====================================================
// NeatlyFrameWork v1.x
// Filename:    Singleton.cs
// Author:      RenGuiyou
// Feedback: 	mailto:renguiyou@onemt.com.cn
//=====================================================
#endregion

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
