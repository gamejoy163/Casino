// GameController.cs
// Author:prosics <Prosics@163.com>
// Date:12/26/2017
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
	public class GameController : Controller<GameModel>
	{
		protected override void OnInitialize()
		{
			UserModel userM = new UserModel ();
			model._userModel = new ModelRef<UserModel> (userM);
			Controller.Instantiate<UserController> (userM,transform);

		}
	}
}

