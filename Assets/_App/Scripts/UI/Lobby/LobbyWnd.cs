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


		public event System.Action<GameObject> eventClickGameItem;
		protected override void Start ()
		{
			base.Start ();
			_testPlay.onClick.AddListener (OnClickTestPlay);
		}

		void OnClickTestPlay()
		{
			if (eventClickGameItem != null)
				eventClickGameItem (_testPlay.gameObject);
		}
			

	
			
		
	}
}

