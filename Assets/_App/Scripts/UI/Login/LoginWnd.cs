// LoginWnd.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using UnityEngine;
using UnityEngine.UI;

namespace GameJoy
{
	public class LoginWnd : BaseWnd
	{
		[SerializeField]
		Button _loginBtn;

		[SerializeField]
		InputField _accountInput;

		[SerializeField]
		InputField _passwdInput;


		public event System.Action<string,string> eventClickLogin;
		protected override void Start ()
		{
			base.Start ();
			_loginBtn.onClick.AddListener (OnClickLoginBtn);
			_accountInput.text = "3515";
			_passwdInput .text = "95270";
		}
		void OnClickLoginBtn()
		{
			string account = _accountInput.text;
			string passwd = _passwdInput.text;
			if (eventClickLogin != null)
				eventClickLogin (account, passwd);
			

		}

	}
}

