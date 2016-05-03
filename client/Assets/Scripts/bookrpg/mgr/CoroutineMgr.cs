﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using UnityEngine;

public class CoroutineBehaviour : MonoBehaviour
{
}

public static class CoroutineMgr
{

    private class CoroutineInfo
    {
        public string desc;
        public WeakReference obj;
    }

    private static MonoBehaviour mMain;
    private static CoroutineBehaviour   mPrivate;   
    private static List<CoroutineInfo>  routines = new List<CoroutineInfo>();

    public static int DebugGetCoroutineCount()
    {
        return routines.Count;
    }

    public static string DumpCoroutines()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("DumpCoroutines\n", new object[0]);
        int num = 0;
        foreach (CoroutineInfo info in routines)
        {
            if (info.obj.IsAlive)
            {
                object target = info.obj.Target;
                num++;
                builder.AppendFormat(" {0}: {1}\n", target, info.desc);
            }
        }
        builder.AppendFormat("  count: {0}\n", num);
        return builder.ToString();
    }

    public static void Init(MonoBehaviour main)
    {
        mMain = main;
    }

    [Conditional("ENABLE_TRACE")]
    private static void MiniszieRoutineList()
    {
        int num = 0;
        while (num < routines.Count)
        {
            CoroutineInfo item = routines[num];
            if (!item.obj.IsAlive)
            {
                routines.Remove(item);
            }
            else
            {
                num++;
            }
        }
    }

    public static Coroutine startCoroutine(IEnumerator routine)
    {
        return startCoroutine(routine, false);
    }

    public static Coroutine startCoroutine(IEnumerator routine, bool bDestroyWhenLoadLevel)
    {
        //use main MonoBehaviour exe coroutine
        if (!bDestroyWhenLoadLevel && mMain != null)
        {
            return mMain.StartCoroutine(routine);
        }
        //用当前场景内的MonoBehavior执行协成
        if (mPrivate == null)
        {
            mPrivate = new GameObject("CoroutineManager").AddComponent<CoroutineBehaviour>();
        }
        return mPrivate.StartCoroutine(routine);
    }


    public static void stopCoroutine(string routine)
    {
        if (mMain != null)
        {
            mMain.StopCoroutine(routine);
        }
        if (mPrivate != null)
        {
            mPrivate.StopCoroutine(routine);
        }
    }

    public static void stopCoroutine(IEnumerator routine)
    {
        if (mMain != null)
        {
            mMain.StopCoroutine(routine);
        }
        if (mPrivate != null)
        {
            mPrivate.StopCoroutine(routine);
        }
    }
}