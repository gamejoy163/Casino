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
				return "Prefabs/UI/loginWndPrefab";
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
			NetMsgCenter.AddEventListener ((int)CProtocol.RLogin,OnLogin);
			_wnd.eventClickLogin += OnClickLoginBtn;

		}
		protected override void Destroy ()
		{
			base.Destroy ();
			NetMsgCenter.RemoveEventListener ((int)CProtocol.RLogin,OnLogin);
		}
		void OnClickLoginBtn(string account, string passwd)
		{
			Debug.Log ("click login:" + account + passwd);
			NetMsgCenter.Request_Login (account,passwd);
		}
		bool isLogin = false;
		void OnLogin(List<System.Object> args)
		{
			isLogin = true;

		}
		protected override void Update ()
		{
			base.Update ();
			if (isLogin)
			{
				isLogin = false;
				SceneManager.LoadSceneAsync("baccaratRoom");
			}
		}

	}
}

