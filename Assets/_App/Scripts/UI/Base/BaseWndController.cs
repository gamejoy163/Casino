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
		where T : BaseWndModel  
		where W : BaseWnd
	{
		protected W _wnd = null;


		protected abstract string wndPrefabPath{ get;}

		protected override void OnInitialize ()
		{
			InstantiateWnd ();
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
	}
}

