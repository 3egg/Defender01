﻿using UnityEngine;

namespace Utils
{
    public class SingletonUtil<T> where T :  new()
    {
        public static T Single { get; private set; } = new T();
    }
}