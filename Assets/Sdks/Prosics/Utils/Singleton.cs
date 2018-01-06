//
//  Author: Prosics 
//  Time: 2017/11/18
//  Copyright (c) 2017, Prosics
// //Description:
// //
using System;

namespace Prosics.Utils
{
    public class Singleton<T> where T : Singleton<T>,new()
    {
        protected static T _instance = null;
        public static T instance 
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new T();
                    _instance.Init();
                }
                return _instance;
            }
        }
        protected virtual void Init()
        {
            
        }
    }
}

