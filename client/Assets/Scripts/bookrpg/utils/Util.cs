﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace bookrpg.utils
{

    public class Util
    {
        public static void insertionSort<T>(IList<T> list, Comparison<T> comparison)
        {
            int count = list.Count;
            for (int i = 1; i < count; i++)
            {
                T y = list[i];
                int num3 = i - 1;
                while ((num3 >= 0) && (comparison(list[num3], y) > 0))
                {
                    list[num3 + 1] = list[num3];
                    num3--;
                }
                list[num3 + 1] = y;
            }
        }
    }
}