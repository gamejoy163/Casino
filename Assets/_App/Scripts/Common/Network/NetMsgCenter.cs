// NetMsgCenter.cs
// Author:prosics <Prosics@163.com>
// Date:1/6/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics;
using Prosics.Utils;
using Kimmidoll;

namespace GameJoy
{
	public class NetMsgCenter : MonoScriptBase
	{
		static NetMsgCenter _instance = null;
		public static NetMsgCenter instance
		{
			get
			{ 
				if (_instance == null)
				{
					GameObject go = new GameObject (typeof(NetMsgCenter).Name);
					go.AddComponent<NetMsgCenter> ();
				}
				return _instance;
			}
		}
		protected override void Awake ()
		{
			base.Awake ();
			_instance = this;
			DontDestroyOnLoad (gameObject);
		}
		Dictionary<int,List<System.Action<List<System.Object>>>> _handlerDic = new Dictionary<int, List<System.Action<List<object>>>> ();

		Queue<List<System.Object>> _msgQueue = new Queue<List<System.Object>> ();

		protected override void FixedUpdate ()
		{
			base.FixedUpdate ();
			DispatchMsgInMainThread ();

		}
		public void AddEventListener(int msgId, System.Action<List<System.Object>> handler)
		{
			Debug.Log ("AddEvent:" + msgId);
			if (!_handlerDic.ContainsKey (msgId))
				_handlerDic.Add (msgId,new List<System.Action<List<System.Object>>>());
			List<System.Action<List<System.Object>>> handlers = _handlerDic [msgId];
			handlers.Add (handler);
				
				
		}
		public void RemoveEventListener(int msgId, System.Action<List<System.Object>> handler)
		{
			Debug.Log ("RemoveEvent:" + msgId);
			if (_handlerDic.ContainsKey (msgId))
			{
				List<System.Action<List<System.Object>>> handlers = _handlerDic [msgId];
				if (handlers.Contains (handler))
					handlers.Remove (handler);
			}


		}
		//线程锁 入栈消息
		public void EnqueueMsg(List<System.Object> args)
		{
			lock (_msgQueue)
			{
				_msgQueue.Enqueue (args);
			}
		}
		//分发消息
		void DispatchMsg(int msgId,List<System.Object> args)
		{
			if (_handlerDic.ContainsKey (msgId))
			{
				List<System.Action<List<System.Object>>> handlers = _handlerDic [msgId];
				foreach (System.Action<List<System.Object>> handler in handlers)
				{
					handler (args);
				}
			}
		}
		//在FixedUpdate中分发消息
		void DispatchMsgInMainThread()
		{
			while (_msgQueue.Count > 0)
			{
				List<System.Object> args = DequeueMsg ();
				if (args != null)
				{
					int msgId = (int)args [0];
					DispatchMsg (msgId, args);
				}
			}
		}
		//线程锁 出栈消息
		List<System.Object> DequeueMsg()
		{
			List<System.Object> args = null;
			lock (_msgQueue)
			{
				args = _msgQueue.Dequeue ();
			}
			return args;
		}




		public void Request_Login(string account, string passwd)
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

