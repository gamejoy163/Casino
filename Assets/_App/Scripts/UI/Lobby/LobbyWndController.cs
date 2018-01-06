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
				throw new System.NotImplementedException ();
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
		}
	}
}

