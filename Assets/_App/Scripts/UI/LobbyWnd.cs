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
		public System.Action onClickPlay = null;

		[SerializeField]
		Button _btnPlay;

		protected override void Start ()
		{
			base.Start ();
			_btnPlay.onClick.AddListener (OnClickPlayBtn);
		}

		void OnClickPlayBtn()
		{
			if (onClickPlay != null)
				onClickPlay ();
		}

	
			
		
	}
}

