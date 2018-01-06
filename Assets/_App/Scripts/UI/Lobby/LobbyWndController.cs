// LobbyWndController.cs
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
	public class LobbyWndController : BaseWndController<BaseWndModel,LobbyWnd>
	{
		protected override string wndPrefabPath
		{
			get
			{
				return GlobalPath.Path_Prefabs_UI_Wnds + "lobbyWnd";
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
			_wnd.eventClickGameItem += OnClickGameItem;
		}

		void OnClickGameItem(GameObject go)
		{
			SceneManager.LoadSceneAsync("baccaratRoom");
		}
	}
}

