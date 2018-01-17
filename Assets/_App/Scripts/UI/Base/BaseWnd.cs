// BaseWnd.cs
//  Author:prosics <Prosics@163.com>
//  Date:12/24/2017
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
	public class BaseWnd : MonoScriptBase
	{
		[SerializeField]
		protected Button _bgCloseBtn;
		[SerializeField]
		protected Button _closeBtn;

		public event System.Action eventClickCloseBtn;

		protected override void Awake ()
		{
			base.Awake ();
			if (_bgCloseBtn != null)
				_bgCloseBtn.onClick.AddListener (OnClickBgCloseBtn);

			if (_closeBtn != null)
				_closeBtn.onClick.AddListener (OnClickCloseBtn);
		}

		protected virtual void  OnClickBgCloseBtn()
		{
			if (eventClickCloseBtn != null)
				eventClickCloseBtn ();
		}
		protected virtual void  OnClickCloseBtn()
		{
			if (eventClickCloseBtn != null)
				eventClickCloseBtn ();
		}
	}
}

