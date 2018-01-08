// BaseWndController.cs
// Author:prosics <Prosics@163.com>
// Date:12/24/2017
// Copyright (c) 2017 prosics
// Description:
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;

namespace GameJoy
{
	public abstract class BaseWndController<T,W> : Controller<T> 
		where T : BaseWndModel, new()
		where W : BaseWnd
	{

		public static void Create<C> (Transform parent) where C : BaseWndController<T,W>
		{
			T m = new T ();
			Controller.Instantiate<C>(m,parent);
		}


		protected W _wnd = null;


		protected abstract string wndPrefabPath{ get;}

		protected override void OnInitialize ()
		{
			InstantiateWnd ();
			_wnd.eventClickCloseBtn += OnClickCloseBtn;

		}

		BaseWnd InstantiateWnd()
		{
			GameObject wndObj = ResManager.instance.Instantiation (wndPrefabPath) as GameObject;
			_wnd = wndObj.GetComponent<W> ();
			UIManager.instance.RegisterWnd (_wnd);
			return _wnd;
		}
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			UIManager.instance.UnregisterWnd (_wnd);
		}
		void OnClickCloseBtn(GameObject go)
		{
			Destroy ();
		}
	}
}

