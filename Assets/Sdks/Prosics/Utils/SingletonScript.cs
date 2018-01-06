// ******************************************************************************************** 
// Author:  Prosics 
// Date: 2017/6/1
// Copyright (c) 2017 Prosics
// Description:
// ********************************************************************************************
using UnityEngine;

namespace Prosics.Utils
{
    public abstract class SingletonScript<T> : MonoScriptBase where T : SingletonScript<T>
    {
        protected static T _instance = null;
        public static T instance 
        {
            get
            {
                if(_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
					go.AddComponent<T>();
                }
                return _instance;
            }
			private set
			{ 
				if (_instance != null && _instance != value) 
				{
					GameObject.Destroy (_instance);
				}
				_instance = value;
			}
        }
		protected override void Awake ()
		{
			base.Awake ();
			instance = this as T;
			if(transform.parent == null)
				GameObject.DontDestroyOnLoad (gameObject);
		}
        public static void Release()
        {
            if(instance != null)
            {
                GameObject.Destroy(instance.gameObject);
            }
        }


        protected virtual void OnDestroy()
        {
            base.OnDestroy();
            _instance = null;
        }




    }
}

