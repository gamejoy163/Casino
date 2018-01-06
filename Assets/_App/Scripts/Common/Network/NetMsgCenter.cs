// NetMsgCenter.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using System;
using System.Collections;
using System.Collections.Generic;
using Kimmidoll;

namespace GameJoy
{
	public class NetMsgCenter
	{

		static Dictionary<int,List<Action<List<Object>>>> _handlerDic = new Dictionary<int, List<Action<List<object>>>> ();
		public static void AddEventListener(int msgId, Action<List<Object>> handler)
		{
			
			if (!_handlerDic.ContainsKey (msgId))
				_handlerDic.Add (msgId,new List<Action<List<Object>>>());
			List<Action<List<Object>>> handlers = _handlerDic [msgId];
			handlers.Add (handler);
				
				
		}
		public static void RemoveEventListener(int msgId, Action<List<Object>> handler)
		{
			if (_handlerDic.ContainsKey (msgId))
			{
				List<Action<List<Object>>> handlers = _handlerDic [msgId];
				if (handlers.Contains (handler))
					handlers.Remove (handler);
			}


		}

		public static void DispatchMsg(int msgId,List<Object> args)
		{
			if (_handlerDic.ContainsKey (msgId))
			{
				List<Action<List<Object>>> handlers = _handlerDic [msgId];
				foreach (Action<List<Object>> handler in handlers)
				{
					handler (args);
				}
			}
		}



		public static void Request_Login(string account, string passwd)
		{
			ByteBuffer buffer = new ByteBuffer();
			buffer.WriteInt(0);
			buffer.WriteInt(0x100);
			buffer.WriteString(account);//account
			buffer.WriteString(passwd);//password
			buffer.WriteInt(200);//version
			buffer.WriteString("cn");//language
			buffer.WriteString("iphonex");
			buffer.WriteString("android");
			buffer.WriteInt(1);//0 login  1:注册
			buffer.WriteMd5();
			UserInfo.Net.SendMessage(buffer.ToBytes());
		}
	}
}

