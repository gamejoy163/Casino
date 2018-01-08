// GlobalMsgId.cs
// Author:prosics <Prosics@163.com>
// Date:1/7/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using System;

namespace GameJoy
{
	public enum MVC_MsgId
	{
		Rqt_Login,
		Rqt_Edit_HeadPic,
		Ntf_Login_Success,
		Ntf_Login_Failed,
		Ntf_Golds_Changed,
		Ntf_Diamond_Changed,
		Ntf_HeadPic_Changed
	}

	public class Msg_Rqt_Login
	{
		public const MVC_MsgId MsgId = MVC_MsgId.Rqt_Login;
	}
}

