// MainWndView.cs
// Author:prosics <Prosics@163.com>
// Date:12/24/2017
// Copyright (c) 2017 prosics
// Description:
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prosics;
using Prosics.Utils;

namespace GameJoy
{
	public class LobbyWnd : BaseWnd
	{
		[SerializeField]
		Button _testPlay;

		[SerializeField]
		Image _headPic;

		[SerializeField]
		Button _profileBtn;

		[SerializeField]
		Button _addGoldsBtn;

		[SerializeField]
		Button _addDiamondBtn;


		[SerializeField]
		Button _optionsBtn;


		[SerializeField]
		Button _rankBtn;

		[SerializeField]
		Button _shopBtn;

		[SerializeField]
		Button _friendstn;

		[SerializeField]
		Button _messageBtn;







		public event System.Action<GameObject> eventClickGameItem;

		public event System.Action<GameObject> eventClickRankBtn;
		public event System.Action<GameObject> eventClickShopBtn;
		public event System.Action<GameObject> eventClickFriendsBtn;
		public event System.Action<GameObject> eventClickMessageBtn;
		public event System.Action<GameObject> eventClickOptionsBtn;
		public event System.Action<GameObject> eventClickAddGoldsBtn;
		public event System.Action<GameObject> eventClickAddDiamondBtn;
		public event System.Action<GameObject> eventClickProfileBtn;



		protected override void Start ()
		{
			base.Start ();
			_testPlay.onClick.AddListener (OnClickTestPlay);

			if (_rankBtn != null)
				_rankBtn.onClick.AddListener (OnClickRankBtn);
			if (_shopBtn != null)
				_shopBtn.onClick.AddListener (OnClickShopBtn);
			if (_friendstn != null)
				_friendstn.onClick.AddListener (OnClickFriendsBtn);
			if (_messageBtn != null)
				_messageBtn.onClick.AddListener (OnClickMessageBtn);
			if (_optionsBtn != null)
				_optionsBtn.onClick.AddListener (OnClickOptionsBtn);
			if (_addGoldsBtn != null)
				_addGoldsBtn.onClick.AddListener (OnClickAddGoldsBtn);
			if (_addDiamondBtn != null)
				_addDiamondBtn.onClick.AddListener (OnClickAddDiamondBtn);
			if (_profileBtn != null)
				_profileBtn.onClick.AddListener (OnClickProfileBtn);
		}
		public void SetHeadPic(string headPicPath)
		{
			Texture2D tex=	ResManager.instance.Load(headPicPath) as Texture2D;
			Sprite spt = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			_headPic.sprite = spt;

		}
		void OnClickTestPlay()
		{
			if (eventClickGameItem != null)
				eventClickGameItem (_testPlay.gameObject);
		}

		void OnClickRankBtn()
		{
			if (eventClickRankBtn != null)
				eventClickRankBtn (_rankBtn.gameObject);
		}
		void OnClickShopBtn()
		{
			if (eventClickShopBtn != null)
				eventClickShopBtn (_shopBtn.gameObject);
		}
		void OnClickFriendsBtn()
		{
			if (eventClickFriendsBtn != null)
				eventClickFriendsBtn (_friendstn.gameObject);
		}
		void OnClickMessageBtn()
		{
			if (eventClickMessageBtn != null)
				eventClickMessageBtn (_messageBtn.gameObject);
		}
		void OnClickOptionsBtn()
		{
			if (eventClickOptionsBtn != null)
				eventClickOptionsBtn (_optionsBtn.gameObject);
		}
		void OnClickAddGoldsBtn()
		{
			if (eventClickAddGoldsBtn != null)
				eventClickAddGoldsBtn (_addGoldsBtn.gameObject);
		}
		void OnClickAddDiamondBtn()
		{
			if (eventClickAddDiamondBtn != null)
				eventClickAddDiamondBtn (_addDiamondBtn.gameObject);
		}
		void OnClickProfileBtn()
		{
			if (eventClickProfileBtn != null)
				eventClickProfileBtn (_profileBtn.gameObject);
		}
			

	
			
		
	}
}

