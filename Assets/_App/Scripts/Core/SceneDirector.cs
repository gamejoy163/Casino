//
//  Author: Prosics 
//  Time: 2017/11/13
//  Copyright (c) 2017, Prosics
// //Description:
// //
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;

namespace GameJoy
{
	public class SceneDirector<T> : MonoScriptBase where T : SceneDirector<T>
    {
        public static T instance {get;private set;}
		protected override void Awake ()
		{
			base.Awake ();
			instance = (T)this;
		}

        protected override void OnEnable()
        {
            base.OnEnable();
            
        }
        protected virtual void OnDestroy()
        {
            base.OnDestroy();
            instance = null;
        }

    }
}

