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



		List<BaseWnd> _wndList = new List<BaseWnd>();
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
			

		public void RegisterWnd(BaseWnd wnd)
		{
			
			wnd.transform.SetParent(uiRoot);
			wnd.transform.localPosition = Vector3.zero;
			wnd.transform.localScale = Vector3.one;
			wnd.gameObject.GetComponent<Canvas> ().sortingOrder = GetNextSortOrder ();
			_wndList.Add (wnd);
		}
		public void UnregisterWnd(BaseWnd wnd)
		{
			_wndList.Remove (wnd);
			GameObject.Destroy (wnd.gameObject);
		}
			
		int GetNextSortOrder()
		{
			int order = 0;
			Canvas canvas = null;
			foreach (BaseWnd wnd in _wndList)
			{
				canvas = wnd.gameObject.GetComponent<Canvas> ();
				if (canvas.sortingOrder >= order)
					order = canvas.sortingOrder;
			}
			return order + 1;
			
		}
    }
}

