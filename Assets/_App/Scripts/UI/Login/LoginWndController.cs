// LoginWndController.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using UnityEngine;
using Prosics;
using Prosics.Utils;
using Prosics.MVC;


namespace GameJoy
{
	public class LoginWndController :BaseWndController<BaseWndModel,LoginWnd>
	{
		protected override string wndPrefabPath
		{
			get
			{
				return "Prefabs/UI/loginWndPrefab";
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
			_wnd.eventClickLogin += OnClickLoginBtn;
		}

		void OnClickLoginBtn(string account, string passwd)
		{
			Debug.Log ("click login:" + account + passwd);
			NetMsgCenter.Request_Login (account,passwd);
		}
	}
}

