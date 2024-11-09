﻿using System.Text;
using UnityEngine;

public static class Utils
{
    private static StringBuilder m_StringBuilder = new StringBuilder();

    public static string StringCombine(params string[] strings)
    {
        m_StringBuilder.Clear();
        foreach (var str in strings)
        {
            m_StringBuilder.Append(str);
        }

        return m_StringBuilder.ToString();
    }

    public static void RemoveAllChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(parent.GetChild(i).gameObject);
        }
    }
}