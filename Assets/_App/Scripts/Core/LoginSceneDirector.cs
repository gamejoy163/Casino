// LoginSceneDirector.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;
namespace GameJoy
{
	public class LoginSceneDirector : SceneDirector<LoginSceneDirector>
	{
		protected override void Start ()
		{
			base.Start ();
			BaseWndModel m = new BaseWndModel ();
			Controller.Instantiate<LoginWndController> (m,transform);
		}
	}
}

