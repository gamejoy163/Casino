//
//  Author: Prosics 
//  Time: 2017/11/14
//  Copyright (c) 2017, Prosics
// //Description:
// //
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;

namespace GameJoy
{
	public class ResManager : SingletonScript<ResManager>
	{
		Dictionary<string, Object> _assets = new Dictionary<string, Object> ();
		//将资源加载进内存而不进行实例话
		public Object Load (string path)
		{
			Object obj = null;
			if (_assets.ContainsKey (path))
			{
				obj = _assets [path];
			}
			else
			{
				obj = Resources.Load (path);
				if (obj != null)
					_assets.Add (path, obj);
			}
			return  obj;
		}

		//实例化资源
		public Object Instantiation (string path, bool releasOriginal = true)
		{
			Object prefab = Load (path);
			if (prefab == null)
				Debug.LogError ("not find asset:" + path);
			
			Object obj = GameObject.Instantiate (prefab);
			prefab = null;
			if (releasOriginal)
			{
				_assets.Remove (path);
			}
			Resources.UnloadUnusedAssets ();
			return obj;
		}
	}
}

