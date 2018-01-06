//与服务器通讯主要类
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.IO;

namespace Kimmidoll
{
    public class NetworkInterface
	{
		public string ip;
		public int port;
		// 主循环周期ms 优化去掉循环中做除法
		private static float threadUpdatePeriod = 1000f / 100;
		private System.DateTime _lasttime = System.DateTime.Now;

		public Thread mConnectThread = null;
		private static Socket clientSocket;
		//是否已连接的标识  
		public bool IsConnected = false;

		public NetworkInterface(string ip,int port)
		{
			this.ip = ip;
			this.port = port;
		}

		public void Start()
		{                       
			//开启一个线程连接，必须的，否则主线程卡死  
			mConnectThread = new Thread(new ThreadStart(ConnectToServer));
			//开启线程
			mConnectThread.Start();
		}

		/// <summary>  
		/// 连接指定IP和端口的服务器  
		/// </summary>  
		/// <param name="ip"></param>  
		/// <param name="port"></param>  
		public void ConnectToServer()
		{
			if (clientSocket != null)
				clientSocket.Close();

			IPAddress mIp = IPAddress.Parse(ip);
			IPEndPoint ip_end_point = new IPEndPoint(mIp, port);
			//定义套接字类型,必须在子线程中定义  
			clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
			{
				clientSocket.Connect(ip_end_point);
				IsConnected = true;
				Debug.Log("连接服务器成功");
			}
			catch
			{
				IsConnected = false;
				Debug.Log("连接服务器失败");
				return;
			}

			ReceiveMessage();

		}
		void _thread_wait()
		{
			TimeSpan span = DateTime.Now - _lasttime;

			int diff = (int)(threadUpdatePeriod - span.Milliseconds);

			if (diff < 0)
				diff = 0;

			System.Threading.Thread.Sleep(diff);
			_lasttime = DateTime.Now;
		}

		/// <summary>
		/// 因为客户端只接受来自服务器的数据
		/// 因此这个方法中不需要参数
		/// </summary>
		private void ReceiveMessage()
		{
			//设置循环标志位
			bool flag = true;
			while (flag)
			{
				try
				{
//					//获取数据长度
//					int receiveLength = clientSocket.Receive(result);
//					//获取服务器消息
//					int iPackageLength = BitConverter.ToInt32(result, 0);
//
//					ByteBuffer buffer = new ByteBuffer(result, iPackageLength);
//					int iReadLength = buffer.ReadInt();
//					int iProtocol = buffer.ReadInt();
//					int iRet = buffer.ReadInt();
//					if(0==iRet){
//						int iPlayerId = buffer.ReadInt();
//						int iGoldNum = buffer.ReadInt();
//						char cHeadIndex = buffer.ReadChar();
//						string strNickname = buffer.ReadString(iPackageLength);
//						short sGameRoomId = buffer.ReadShort();
//						Debug.Log(string.Format("接收到数据包中的iReadLength={0},iProtocol={1},iRet={2},iPlayerId={3},iGoldNum={4},cHeadIndex={5},strNickname={6},sGameRoomId={7}：", iReadLength, iProtocol, iRet, iPlayerId, iGoldNum, "", strNickname, sGameRoomId));
//						Debug.Log("strNickname=" + strNickname);
//						Debug.Log("sGameRoomId=" + sGameRoomId);
//						int head = Convert.ToInt16(cHeadIndex);
//						Debug.Log("cHeadIndex=" + head);
//					}

					//接受消息头（4字节）  
					int HeadLength = 4;  
					//存储消息头的所有字节数  
					byte[] recvBytesHead = new byte[HeadLength];  
					//如果当前需要接收的字节数大于0，则循环接收  
					while (HeadLength > 0)  
					{  
						byte[] recvBytes = new byte[4];  
						//将本次传输已经接收到的字节数置0  
						int iBytesHead = 0;  
						//如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收  
						if (HeadLength >= recvBytes.Length)  
						{  
							iBytesHead = clientSocket.Receive(recvBytes, recvBytes.Length, 0);  
						}  
						else  
						{  
							iBytesHead = clientSocket.Receive(recvBytes, HeadLength, 0);  
						}  
						//将接收到的字节数保存  
						recvBytes.CopyTo(recvBytesHead, recvBytesHead.Length - HeadLength);  
						//减去已经接收到的字节数  
						HeadLength -= iBytesHead;  
					}

					//接收完整消息  
					int BodyLength = BitConverter.ToInt32(recvBytesHead, 0) - 4;
					//存储消息体的所有字节数  
					byte[] recvBytesBody = new byte[BodyLength];  
					//如果当前需要接收的字节数大于0，则循环接收  
					while (BodyLength > 0)  
					{  
						byte[] recvBytes = new byte[BodyLength < 1024 ? BodyLength : 1024];
						//将本次传输已经接收到的字节数置0  
						int iBytesBody = 0;  
						//如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收  
						if (BodyLength >= recvBytes.Length)  
						{  
							iBytesBody = clientSocket.Receive(recvBytes, recvBytes.Length, 0);  
						}  
						else  
						{  
							iBytesBody = clientSocket.Receive(recvBytes, BodyLength, 0);  
						}  
						//将接收到的字节数保存  
						recvBytes.CopyTo(recvBytesBody, recvBytesBody.Length - BodyLength);  
						//减去已经接收到的字节数  
						BodyLength -= iBytesBody;  
					}
					//clientSocket.Receive(recvBytesBody, BodyLength, 0); 

					//一个数据包接收完毕，解析数据体
					//UnpackData(recvBytesBody);
					PackageManage.GetInstance().UnpackData(recvBytesBody);

				}
				catch (Exception e)
				{
					//停止消息接收
					flag = false;
					//断开服务器
					clientSocket.Shutdown(SocketShutdown.Both);
					//关闭套接字
					clientSocket.Close();
					SocketQuit();
					Debug.Log("服务器返回数据：" + e.Message);

				}
				_thread_wait();
			}

		}

		/// <summary>
		/// 拆解消息数据包
		/// </summary>
		/// <param name="recvBytes">数据包</param>
		public void UnpackData(byte[] recvBytes)
		{
//			int packageLength = recvBytes.Length;
////			Debug.Log ("iPackageLength=" + packageLength);
//			ByteBuffer buffer = new ByteBuffer(recvBytes, packageLength);
//			int iProtocol = buffer.ReadInt();
//			switch (iProtocol) {
//			case -0x100://登录
//				int iRet = buffer.ReadInt ();
//				if (iRet == 0) {
//					UserInfo.PlayerID = buffer.ReadInt ();
//					UserInfo.MyGold = Convert.ToUInt64(buffer.ReadLong ());
//					UserInfo.Login = true;
//					Debug.Log ("UserInfo.playerID=" + UserInfo.playerID);
//					Debug.Log ("UserInfo.myGold=" + UserInfo.myGold);
//				}
//				break;
//			case -0x201://获取房间信息
//				int roomcount = buffer.ReadInt ();//房间个数
//				Debug.Log ("房间个数=" + roomcount);
//
//				for (int i = 0; i < roomcount; i++) {
//					int roomID = buffer.ReadInt ();
//					long minGold = buffer.ReadLong ();
//					long maxGold = buffer.ReadLong ();
//					Debug.Log(string.Format("roomID={0},minGold={1},maxGold={2}", roomID, minGold, maxGold));
//				}
//
//				break;	
//			case -0x202://进入房间
//				int res = buffer.ReadInt ();//返回值(成功为0，其它为失败)
//				int roomState = buffer.ReadInt ();//房间状态
//				int time = buffer.ReadInt ();//剩余时间
//				Debug.Log(string.Format("roomState={0},time={1},res={2}", roomState, time,res));
//				break;	
//			case -0x203://退出房间
//				res = buffer.ReadInt ();//返回值(成功为0，其它为失败)
//				Debug.Log(string.Format("res={0}", res));
//				break;
//			case -0x204://开始下注通知
//				Debug.Log(string.Format("开始下注通知"));
//				break;
//			case -0x205://下注成功与否 以及返回的结果
//				Debug.Log(string.Format("开始下注通知"));
//				break;	
//			}
		}

		public void SocketQuit()
		{
			//关闭线程  
			if (mConnectThread != null)
			{
				mConnectThread.Interrupt();
				mConnectThread.Abort();
			}
			//最后关闭服务器  
			if (clientSocket != null)
				clientSocket.Close();

			Debug.LogWarning("SocketQuit");
		}

		/// <summary>  
		/// 发送数据给服务器  
		/// </summary>  
		public void SendMessage(byte[] msg)
		{
			if (IsConnected == false || clientSocket == null)
				return;
			clientSocket.Send(msg);
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="msg">消息文本</param>
		public void SendMessage(byte[] msg, int length)
		{
			if (IsConnected == false)
				return;
			clientSocket.Send(msg, length, 0);
		}

		/// <summary>  
		/// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据  
		/// </summary>  
		/// <param name="message"></param>  
		/// <returns></returns>  
		private static byte[] WriteMessage(byte[] message)
		{
			MemoryStream ms = null;
			using (ms = new MemoryStream())
			{
				ms.Position = 0;
				BinaryWriter writer = new BinaryWriter(ms);
				ushort msglen = (ushort)message.Length;
				writer.Write(msglen);
				writer.Write(message);
				writer.Flush();
				return ms.ToArray();
			}
		}
	}
}
