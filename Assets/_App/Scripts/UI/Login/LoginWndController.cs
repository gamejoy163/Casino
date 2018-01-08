// LoginWndController.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prosics;
using Prosics.Utils;
using Prosics.MVC;
using Kimmidoll;


namespace GameJoy
{
	public class LoginWndController :BaseWndController<BaseWndModel,LoginWnd>
	{
		protected override string wndPrefabPath
		{
			get
			{
				return GlobalPath.Path_Prefabs_UI_Wnds + "loginWnd";
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
			_wnd.eventClickLogin += OnClickLoginBtn;

		}
		protected override void Destroy ()
		{
			base.Destroy ();

		}
		void OnClickLoginBtn(string account, string passwd)
		{
			Debug.Log ("click login:" + account + passwd);
			NetMsgCenter.instance.Request_Login (account,passwd);
		}

	}
}

