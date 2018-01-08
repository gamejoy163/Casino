// HeadSelectWndController.cs
// Author:prosics <Prosics@163.com>
// Date:1/8/2018
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
	public class HeadSelectWndController :BaseWndController<BaseWndModel,HeadSelectWnd>
	{
		protected override string wndPrefabPath
		{
			get
			{
				return GlobalPath.Path_Prefabs_UI_Wnds + "headSelectWnd";
			}
		}
		protected override void OnInitialize ()
		{
			base.OnInitialize ();
		}
	}
}

