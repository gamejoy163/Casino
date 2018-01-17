using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameJoy;

namespace Kimmidoll
{
	//自定义协议
	enum CProtocol
	{
		//登录协议
		SLogin = 0x100,//发送 
		RLogin = -0x100,//接收
		//获取房间信息
		SGetRoomInfo = 0x201, 
		RGetRoomInfo = -0x201,
		//进入房间
		SEnterRoom = 0x202, 
		REnterRoom = -0x202, 
		//退出房间
		SLevelRoom = 0x203, 
		RLevelRoom = -0x203,
		//开始下注通知
		SBeginBetNotice = 0x204, 
		RBeginBetNotice = -0x204, 
		//下注
		SBetResult = 0x205, 
		RBetResult = -0x205,
		//房间下注信息通知
		SBetNotice = 0x206, 
		RBetNotice = -0x206,
		//结算通知
		SBalanceNotice = 0x207, 
		RBalanceNotice = -0x207,
		//金币变化通知
		RGoldChanged = -0x200,
		//修改头像
		SEditHeadPic = 0x208,
		REditHeadPic = -0x208,

	}

	public class PackageManage 
	{
		// 定义一个静态变量来保存类的实例
		private static PackageManage uniqueInstance;

		// 定义私有构造函数，使外界不能创建该类实例
		private PackageManage()
		{
		}

		/// <summary>
		/// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
		/// </summary>
		/// <returns></returns>
		public static PackageManage GetInstance()
		{
			// 如果类的实例不存在则创建，否则直接返回
			if (uniqueInstance == null)
			{
				uniqueInstance = new PackageManage();
			}
			return uniqueInstance;
		}

		/// <summary>
		/// 拆解消息数据包
		/// </summary>
		/// <param name="recvBytes">数据包</param>
		public void UnpackData(byte[] recvBytes)
		{
			try
			{
				
				int packageLength = recvBytes.Length;
				ByteBuffer buffer = new ByteBuffer(recvBytes, packageLength);
				int iProtocol = buffer.ReadInt();
				List<System.Object> args = new List<object>();
				args.Add(iProtocol);

				switch (iProtocol) {
				case (int)CProtocol.RLogin://登录
					List<System.Object> ol = new List<System.Object>();
					ol.Add(iProtocol);
					int iRet = buffer.ReadInt ();
					ol.Add(iRet);
					
					if (iRet == 0) {
						UserInfo.PlayerID = buffer.ReadInt ();
						UserInfo.MyGold = Convert.ToUInt64(buffer.ReadULong ());
						UserInfo.Login = true;
						Debug.Log ("UserInfo.playerID666=" + UserInfo.PlayerID);
						Debug.Log ("UserInfo.myGold=" + UserInfo.MyGold);
						ol.Add(UserInfo.PlayerID);
						ol.Add(UserInfo.MyGold);
					}
					
						NetMsgCenter.instance.EnqueueMsg(ol);

					break;
				case (int)CProtocol.REditHeadPic:
						args.Add(buffer.ReadInt());
						NetMsgCenter.instance.EnqueueMsg(args);
					break;
				case (int)CProtocol.RGetRoomInfo://获取房间信息
					int roomcount = buffer.ReadInt ();//房间个数
					Debug.Log ("房间个数=" + roomcount);

					for (int i = 0; i < roomcount; i++) {
						int roomID = buffer.ReadInt ();
						long minGold = buffer.ReadLong ();
						long maxGold = buffer.ReadLong ();
						Debug.Log(string.Format("roomID={0},minGold={1},maxGold={2}", roomID, minGold, maxGold));
					}

					break;	
				case (int)CProtocol.REnterRoom://进入房间
					int res = buffer.ReadInt ();//返回值(成功为0，其它为失败)
					if(res != 0)break;
					Debug.Log("进入房间");
					int roomState = buffer.ReadInt ();//房间状态
					int time = buffer.ReadInt ();//剩余时间
					RoomModel.BetLimit[0] = buffer.ReadLong();//房间最小下注
					RoomModel.BetLimit[1] = buffer.ReadLong();//房间最大下注
					RoomModel.RemainingPoker = buffer.ReadInt();//房间剩余牌数
					int count = buffer.ReadInt();//历史记录数量
					RoomModel.History.Clear();
					for(int i=0;i<count;i++){
						short winner=buffer.ReadShort();
						short winnerDuiz=buffer.ReadShort();
						HistoryBet his = new HistoryBet();
						his.Winner=winner;
						his.WinerDuiz=winnerDuiz;
						RoomModel.History.Add(his);
//							Debug.Log(string.Format("winner={0},winnerDuiz={1}", winner, winnerDuiz));
					}


					if(roomState==1){
						RoomModel.RBeginBetNotice = true;
						RoomModel.RemainingTime=time;
					}else if(roomState==2){
						//结算
						RoomModel.RemainingBalanceTime = time;
						//庄家牌(数组,3个元素)
						RoomModel.ZhuangPokerList.Clear ();
						for (int i = 0; i < 3; i++) {
							ushort type = buffer.ReadUShort ();//桃3 心2 梅1 方0
							ushort num = buffer.ReadUShort ();//1到13
							RoomModel.ZhuangPokerList.Add (i, 13*type+num-1);
							RoomModel.ZhuangNum[i]=num>9?0:num;
						}

						RoomModel.XianPokerList.Clear ();
						for (int i = 0; i < 3; i++) {
							ushort type = buffer.ReadUShort ();//桃3 心2 梅1 方0
							ushort num = buffer.ReadUShort ();//1到13
							RoomModel.XianPokerList.Add (i, 13*type+num-1);
							RoomModel.XianNum[i]=num>9?0:num;
//							Debug.LogWarning(string.Format("进入房间,type={0},num={1}", type,num));

						}

						RoomModel.ZhuangPoints = buffer.ReadUInt ();
						RoomModel.XianPoints = buffer.ReadUInt ();

						RoomModel.Winner[0] = buffer.ReadShort();
						RoomModel.Winner[1] = buffer.ReadShort();

						RoomModel.RBalanceNotice=true;
					}

					RoomModel.REnterRoomSuccess=true;

					Debug.Log(string.Format("roomState={0},time={1},res={2}", roomState, time,res));

					break;	
				case (int)CProtocol.RLevelRoom://退出房间
					res = buffer.ReadInt ();//返回值(成功为0，其它为失败)
					Debug.Log(string.Format("res={0}", res));
					break;
				case (int)CProtocol.RBeginBetNotice://开始下注通知
					RoomModel.RemainingTime = buffer.ReadInt();
					RoomModel.RBeginBetNotice=true;
					Debug.Log("开始下注通知:"+RoomModel.RemainingTime);
					break;
				case (int)CProtocol.RBetResult://下注成功与否 以及返回的结果
					
					int unbet = buffer.ReadInt ();

					res = buffer.ReadInt ();//返回值(成功为0，其它为失败)
					if (res == 0) {
						//投注的集合 0:閑，1:閑對，2:莊，3:莊對,4:和
//						RoomModel.BetGoldList[2] = buffer.ReadLong ();
//						RoomModel.BetGoldList[0] = buffer.ReadLong ();
//						RoomModel.BetGoldList[4] = buffer.ReadLong ();
//						RoomModel.BetGoldList[3] = buffer.ReadLong ();
//						RoomModel.BetGoldList[1] = buffer.ReadLong ();
//						UserInfo.MyGold = Convert.ToUInt64(buffer.ReadULong ());
//						int hasBanker=buffer.ReadInt();
//						long hasBetGold = buffer.ReadLong();
					}else{
						RoomModel.BetFailed.Add(unbet);
						RoomModel.RBetFailed=true;
					}

					Debug.Log(string.Format("下注成功与否,res={0}", res));
					RoomModel.RBetResult=true;
					break;
				case (int)CProtocol.RBetNotice://房间下注信息通知
					//桌面上投注金币的总数分类 0:閑，1:閑對，2:莊，3:莊對,4:和

//					for(int i=0;i<5;i++){
						RoomModel.BetTotalGoldList[2]=buffer.ReadLong ();
						RoomModel.BetTotalGoldList[0]=buffer.ReadLong ();
						RoomModel.BetTotalGoldList[4]=buffer.ReadLong ();
						RoomModel.BetTotalGoldList[3]=buffer.ReadLong ();
						RoomModel.BetTotalGoldList[1]=buffer.ReadLong ();
//					}

					//庄筹码数量(数组4个，四种筹码，筹码值从小到大)
					for(int i=0;i<5;i++){
						for(int j=0;j<4;j++){
							RoomModel.Chips[i,j]=buffer.ReadInt();
						}
					}
//
//					int hasBanker=buffer.ReadInt();
//					long hasBetGold = buffer.ReadLong();

//					Debug.Log(string.Format("房间下注信息通知,閑={0},莊={0}", RoomModel.BetTotalGoldList[3]));
					RoomModel.RBetNotice=true;
					break;
				case (int)CProtocol.RBalanceNotice://结算通知

					RoomModel.RemainingPoker = buffer.ReadInt();
					RoomModel.RemainingBalanceTime = buffer.ReadInt();

					//庄家牌(数组,3个元素)
					RoomModel.ZhuangPokerList.Clear ();
					for (int i = 0; i < 3; i++) {
						ushort type = buffer.ReadUShort ();//桃3 心2 梅1 方0
						ushort num = buffer.ReadUShort ();//1到13
						RoomModel.ZhuangPokerList.Add (i, 13*type+num-1);
						RoomModel.ZhuangNum[i]=num>9?0:num;
					}

					RoomModel.XianPokerList.Clear ();
					for (int i = 0; i < 3; i++) {
						ushort type = buffer.ReadUShort ();//桃3 心2 梅1 方0
						ushort num = buffer.ReadUShort ();//1到13
						RoomModel.XianPokerList.Add (i, 13*type+num-1);
						RoomModel.XianNum[i]=num>9?0:num;
//						Debug.LogWarning(string.Format("结算通知,type={0},num={1}", type,num));

					}

					RoomModel.ZhuangPoints = buffer.ReadUInt ();
					RoomModel.XianPoints = buffer.ReadUInt ();

					RoomModel.Winner[0] = buffer.ReadShort();
					RoomModel.Winner[1] = buffer.ReadShort();

					for (int i = 0; i < 5; i++) {						
						RoomModel.WinArea[i]=buffer.ReadLong ();
//						Debug.LogError(string.Format("结算通知,WinArea={0},WinArea1={1}", t1,t2));
					}

					UserInfo.MyGold = Convert.ToUInt64(buffer.ReadULong ());
					Debug.Log(string.Format("结算通知,MyGold={0}", UserInfo.MyGold));
					RoomModel.RBalanceNotice=true;
					break;

				case (int)CProtocol.RGoldChanged://金币变化通知
					UserInfo.MyGold = Convert.ToUInt64(buffer.ReadULong ());
					break;
				}

			}
			catch(Exception ex){
				Debug.LogError (ex.Message);
			}

		}

	}
}
