﻿using System;

namespace DraconiusGoGUI.Extensions
{
    public class MethodResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public DateTime Time { get; set; } // Will probably change this

        public MethodResult()
        {
            Time = DateTime.UtcNow;
        }
    }

    public class MethodResult<T> : MethodResult
    {
        public T Data { get; set; }
    }
}
