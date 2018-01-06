using System.Collections;
using System.Collections.Generic;
using Kimmidoll;

public class HistoryBet{

	//0：无效值 1：庄赢 2：闲赢 3：和赢
	public short Winner { get; set; }
	//0：无对子 1：庄对 2：闲对 3：庄闲对
	public short WinerDuiz { get; set; }

}

public class RoomModel {

	//百家乐房间信息
	public static int[] RoomList;
	public static int RoomCount=0;
	public static int RoomID=0;
	public static int RoomState=0;

	//个人投注的集合 0:閑，1:閑對，2:莊，3:莊對,4:和
	public static long[] BetGoldList = new long[5]{0,0,0,0,0};

	//桌面上投注金币的总数分类 0:閑，1:閑對，2:莊，3:莊對,4:和
	public static long[] BetTotalGoldList = new long[5]{0,0,0,0,0};

	//庄牌
	public static Dictionary<int,int> ZhuangPokerList = new Dictionary<int, int> ();
	//庄3张牌的点数
	public static int[] ZhuangNum = new int[3]{0,0,0};
	//庄点数
	public static uint ZhuangPoints = 0;

	//闲牌
	public static Dictionary<int,int> XianPokerList = new Dictionary<int, int> ();
	//闲3张牌的点数
	public static int[] XianNum = new int[3]{0,0,0};
	//闲点数
	public static uint XianPoints = 0;

	//剩余牌数
	public static int RemainingPoker=0;
	//投注剩余时间
	public static int RemainingTime = 0;
	//结算剩余时间
	public static int RemainingBalanceTime = 0;
	//限红
	public static long[] BetLimit = new long[2]{10,10000};

	//桌面赢钱区域及对应的赢钱值  0庄，1闲，2和，4庄对，5闲对
	public static long[] WinArea = new long[5]{0,0,0,0,0};

	//开牌结果  0：无效值 1：庄赢 2：闲赢 3：和赢  |  0：无对子 1：庄对 2：闲对 3：庄闲对
	public static int[] Winner = new int[2]{0,0};

	//历史记录
	public static List<HistoryBet> History = new List<HistoryBet>();

	//投注失败的筹码
	public static ArrayList BetFailed = new ArrayList();

	//投注筹码
	public static int [,] Chips = new int[5,4];

	//有新的信息?
	public static bool RBetNotice=false;//房间下注信息通知
	public static bool RBetResult=false;//下注成功与否
	public static bool RBalanceNotice=false;//结算通知
	public static bool RBeginBetNotice=false;//开始下注通知
	public static bool REnterRoomSuccess=false;//成功进入房间
	public static bool RBetFailed=false;//投注失败

	#region 服务器相关方法

	public static void EnterRoom(int roomID){
		
		ByteBuffer buffer = new ByteBuffer();
		buffer.WriteInt(0);
		buffer.WriteInt((int)CProtocol.SEnterRoom);
		buffer.WriteInt(1000);
		buffer.WriteMd5();
		UserInfo.Net.SendMessage(buffer.ToBytes());

	}

	public static void LevelRoom(){

		ByteBuffer buffer = new ByteBuffer();
		buffer.WriteInt(0);
		buffer.WriteInt((int)CProtocol.SLevelRoom);
		buffer.WriteMd5();
		UserInfo.Net.SendMessage(buffer.ToBytes());

	}

	//投注
	public static void Bet(int betType,uint betGold,int chipsCount){

		switch (betType) {
		case 0:
			betType = 1;
			break;
		case 1:
			betType = 4;
			break;
		case 2:
			betType = 0;
			break;
		case 3:
			betType = 4;
			break;
		case 4:
			betType = 2;
			break;
		}

		ByteBuffer buffer = new ByteBuffer();
		buffer.WriteInt(0);
		buffer.WriteInt((int)CProtocol.SBetResult);

		buffer.WriteInt(betType);
		buffer.WriteLong((long)betGold);
		buffer.WriteInt(chipsCount);
		buffer.WriteMd5();
		UserInfo.Net.SendMessage(buffer.ToBytes());
	}

	#endregion

}
