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
    public class UIManager : SingletonScript<UIManager>
    {
		[SerializeField]
		Transform _uiRoot;
		public Transform uiRoot{get{ return _uiRoot;}}

		[SerializeField]
		Camera _uiCamera;
		public Camera uiCamera{get{ return _uiCamera;}}
		/*
		public void LoadWnd<T>(string prefabPath) where T : BaseWnd
		{
			return LoadWnd<T> (prefabPath, null);
		}
		public T LoadWnd<T>(string prefabPath, Transform parent) where T : BaseWnd
		{
			GameObject prefab = ResManager.instance.Load (prefabPath) as GameObject;
			GameObject go = GameObject.Instantiate (prefab);
			go.transform.SetParent (parent);
			T wnd =go.GetComponent<T> ();
			SetWndOrder (wnd);
			return wnd;
		}*/
		protected override void Awake ()
		{
			base.Awake ();
			if (_uiRoot == null) 
			{
				GameObject go = new GameObject ("uiRoot");
				go.transform.SetParent (transform);
				_uiRoot = go.transform;
			}
		}

		public void LoadWnd(string prefabPath, Transform parent)
		{
			GameObject prefab = ResManager.instance.Instantiation (prefabPath) as GameObject;
			GameObject go = GameObject.Instantiate (prefab);
			go.transform.SetParent (parent);
			BaseWnd wnd =go.GetComponent<BaseWnd> ();
			SetWndOrder (wnd);
		}

		public void LoadWnd<T>(string prefabPath, Transform parent)
		{
			GameObject prefab = ResManager.instance.Instantiation (prefabPath) as GameObject;
			GameObject go = GameObject.Instantiate (prefab);
			go.transform.SetParent (parent);
			BaseWnd wnd =go.GetComponent<BaseWnd> ();
			SetWndOrder (wnd);
		}

		public void RegisterWnd(BaseWnd wnd)
		{
			wnd.transform.SetParent(uiRoot);
			wnd.transform.localPosition = Vector3.zero;
			wnd.transform.localScale = Vector3.one;
			SetWndOrder (wnd);
		}
		public void UnregisterWnd(BaseWnd wnd)
		{
			GameObject.Destroy (wnd.gameObject);
		}






		void SetWndOrder(BaseWnd wnd)
		{
			
		}
    }
}

