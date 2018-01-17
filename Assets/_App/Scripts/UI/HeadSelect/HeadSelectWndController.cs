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
			_wnd.eventSelectedHeadPic += OnSelectedHeadPic;
			//Message.AddListener (MVC_MsgId.Ntf_HeadPic_Changed.ToString(), OnHeadPicChanged);
			for (int i = 0; i < 5; i++)
			{
				_wnd.AddHeadPic (GlobalPath.Path_Textures_HeadPics + "headPic" + i,i);
			}
		}
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
		}
		void OnSelectedHeadPic(int headPicId)
		{
			NetMsgCenter.instance.Request_EditHeadPic (headPicId);
		}

	}
}

