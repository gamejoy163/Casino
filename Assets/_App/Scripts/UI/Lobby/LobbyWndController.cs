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
			_wnd.eventClickRankBtn += OnOpenRank;
			_wnd.eventClickShopBtn += OnOpenShop;
			_wnd.eventClickFriendsBtn += OnOpenFriends;
			_wnd.eventClickMessageBtn += OnOpenMessages;
			_wnd.eventClickOptionsBtn += OnOpenOptions;
			_wnd.eventClickProfileBtn += OnOpenFrofile;
			_wnd.eventClickAddGoldsBtn += OnAddGolds;
			_wnd.eventClickAddDiamondBtn += OnAddDiamond;

			Message.AddListener (MVC_MsgId.Ntf_HeadPic_Changed.ToString(), UpdateUserHeadPic);

			UpdateUserHeadPic ();
			UpdateUserGolds ();
			UpdateUserNick ();
			UpdateUserUid ();


		}

		void OnClickGameItem(GameObject go)
		{
			SceneManager.LoadSceneAsync("baccaratRoom");
		}

		void OnOpenRank(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnOpenShop(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnOpenFriends(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnOpenMessages(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnAddGolds(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnAddDiamond(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnOpenOptions(GameObject go)
		{
			Debug.Log ("click:" + go.name);
		}

		void OnOpenFrofile(GameObject go)
		{
			Debug.Log ("click:" + go.name);
			HeadSelectWndController.Create<HeadSelectWndController>(LobbySceneDirector.instance.transform);
		}
		void UpdateUserHeadPic()
		{
			Debug.Log ("OnHeadPicChanged!");
			string path = GlobalPath.Path_Textures_HeadPics + "headPic" + GameManager.instance.gameModel.userModel.HeadPicId;
			_wnd.SetHeadPic (path);	
		}
		void UpdateUserGolds()
		{
			_wnd.SetGolds (GameManager.instance.gameModel.userModel.golds);
		}
		void UpdateUserNick()
		{
			_wnd.SetUserNick (GameManager.instance.gameModel.userModel.nick);
		}
		void UpdateUserUid()
		{
			_wnd.SetUserUid (GameManager.instance.gameModel.userModel.uid.ToString());
		}
	}
}

