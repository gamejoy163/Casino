using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour {

	//筹码集
	public GameObject[] GOchips;

	//当前选择的筹码
	private GameObject GOchoseChip;

	//當前籌碼的金幣數值
	private uint chipValue=100;

	//个人投注的集合 0:閑，1:閑對，2:莊，3:莊對,4:和
	private long[] betGoldList = new long[5]{0,0,0,0,0};

	//投注的集合
	private uint[] chipsListValue = new uint[4]{100,200,500,1000};

	//poker牌 方块，梅花，红桃，黑桃
	public GameObject[] GOpokers;

	//poker牌卡背，以后可以扩展vip卡背金色，vip1白金色等
	public GameObject[] GOpokerBack;

	//闲poker A-K
	private GameObject[] GOXianPokers = new GameObject[3];
	//庄poker A-K
	private GameObject[] GOZhuangPokers = new GameObject[3];

	//桌面上投注金币的总数分类 0:閑，1:閑對，2:莊，3:莊對,4:和
	private long[] betTotalGoldList = new long[5]{0,0,0,0,0};

	//table 我的筹码值，和大家的筹码值
	public Text[] TextTableXianBetGold;
	public Text[] TextTableXian2BetGold;
	public Text[] TextTableZhuangBetGold;
	public Text[] TextTableZhuang2BetGold;
	public Text[] TextTableHeBetGold;

	//UI 我的金币和钻石
	public Text[] TextMyGoldDiamonds;

	//UI 我赢的钱数
	public GameObject GOMeWinGold;

	//UI 闲庄的点数
	public Text[] TextResultNum;

	//UI 房间状态、结算结果、其他消息等提示语. 0::Pannel 1::msgImage  2::msgText
	public GameObject[] GOMsg;

	//UI 提示语 0:开始下注 1:停止下注 2:闲赢 3:庄赢 4:和 5:闲对 6:庄对
	public Sprite[] SPMsg;

	//房间 全局信息 state RemainingTime 等
	private int state=1;//state 0:无效 1:下注 2:结算
	private int RemainingTime=0;//剩余时间
	public Text TextRemainingTime;
	public Text TextRemainingPoker;//剩余牌数
	public Text TextLimit;//限红

	//倒计时
	public GameObject GORemainingTime; 

	//Audio 发牌音效
	public AudioSource ADDealCard;
	public AudioSource ADSendCard;
	public AudioSource ADStartBet;
	public AudioSource ADStopBet;
	public AudioSource ADZhuangWin;
	public AudioSource ADZhuangDui;
	public AudioSource ADXianWin;
	public AudioSource ADXianDui;
	public AudioSource ADDraw;
	private AudioSource ADWinner;
	public AudioSource[] ADZhuangPoints;
	public AudioSource[] ADXianPoints;
	public AudioSource ADAddchip;

	//历史记录
	public Transform HisItemsParent;
	public Sprite SPCR;//圆形精灵
	public Text[] TextHis;//0:庄 1：先 2:和

	////Effect
	//选中筹码效果
	public GameObject GOChipEffect;

	//投注明细
	private Dictionary<int,int[]> betDetails = new Dictionary<int, int[]>();

	void Start () {
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		GOchoseChip = GOchips [0];
		//进入房间
		RoomModel.EnterRoom(100);

		syncTableInfo ();

	}


	private bool balanced = false;
	private float balancedTimeSpan = 0.0f;
	private bool isReadXianPoints=false;
	private bool isReadZhuangPoints=false;
	void Update(){

		if (balanced) {			
			balancedTimeSpan += Time.deltaTime;
			if (!isReadXianPoints) {
				ADXianPoints [RoomModel.XianPoints].Play ();
				isReadXianPoints = true;
				return;
			}
			if (balancedTimeSpan >= 1.5f) {
				if (!isReadZhuangPoints) {
					ADZhuangPoints [RoomModel.ZhuangPoints].Play ();
					isReadZhuangPoints = true;
					return;
				}
				if (balancedTimeSpan >= 3.0f) {
					alterMsg (getWinnerMsg (), "balance", 3.5f);
					ADWinner.Play ();

					balanced = isReadZhuangPoints = isReadXianPoints = false;
					balancedTimeSpan = 0.0f;
				}
			}
		}

		//投注失败的撤销
		if (RoomModel.RBetFailed) {
			RoomModel.RBetFailed = false;
			betFailed ();
		}

	}

	void FixedUpdate(){

		if(RoomModel.REnterRoomSuccess)//成功进入房间
		{
			RoomModel.REnterRoomSuccess = false;

			int[] his = new int[3]{0,0,0}; 
			int count = 0;//RoomModel.History.Count - 48 > 0 ? RoomModel.History.Count - 48 : 0;
			if (count > 0)
				RoomModel.History.RemoveRange (0, count);
			foreach (HistoryBet betHis in RoomModel.History) 
			{
				//betHis.Winner
				GameObject go = new GameObject("x_Image", typeof(Image));
				go.transform.SetParent(HisItemsParent);
				go.GetComponent<Image> ().sprite = SPCR;
				switch (betHis.Winner) {
				case 1:
					go.GetComponent<Image> ().color = new Color32(198,42,42,255);
					his [0]++;
					break;
				case 2:
					go.GetComponent<Image> ().color = new Color32(33,115,198,255);
					his [1]++;
					break;
				case 3:
					go.GetComponent<Image> ().color = new Color32(13,153,46,255);
					his [2]++;
					break;
				}
				go.transform.localScale = new Vector2 (1, 1);
			}
			TextHis [0].text = "" + (his [0] > 0 ? his [0] : 0);
			TextHis [1].text = "" + (his [1] > 0 ? his [1] : 0);
			TextHis [2].text = "" + (his [2] > 0 ? his [2] : 0);

			//限红
			TextLimit.text = "限紅"+RoomModel.BetLimit[0]+"-"+RoomModel.BetLimit[1];
		}
		if(RoomModel.RBetNotice)//房间下注信息通知
		{
			RoomModel.RBetNotice = false;
			this.betTotalGoldList = RoomModel.BetTotalGoldList;
			syncTableInfo ();
		}
		if(RoomModel.RBetResult)//下注成功与否
		{
			RoomModel.RBetResult = false;
//			this.betGoldList = RoomModel.BetGoldList;
			syncTableInfo ();
		}
		if(RoomModel.RBalanceNotice)//结算通知
		{
			state = 2;
			RoomModel.RBalanceNotice = false;
			GORemainingTime.transform.localScale = new Vector2 (0, 0);
			ADStopBet.Play ();
			alterMsg ("停止投注","fapai");
		}
		if(RoomModel.RBeginBetNotice)//开始下注通知
		{
			RoomModel.RBeginBetNotice = false;
			chipsCount = pokersCount = 0;
			betDetails.Clear ();
			GORemainingTime.transform.localScale = new Vector2 (1, 1);
//			this.RemainingTime = RoomModel.RemainingTime;
			print("RoomModel.RemainingTime="+RoomModel.RemainingTime);
			GORemainingTime.GetComponent<RPB>().Start(15,RoomModel.RemainingTime);
			ADStartBet.Play ();
			alterMsg ("开始投注");
//			StartCoroutine (Countdown());
			StartCoroutine (StartBet());
		}
	}

	#region 玩家操作

	/// <summary>
	/// 選擇籌碼
	/// </summary>
	public void onClickChoseChip(int index){

		Vector2 v;
		switch (index) {
		case 0:
			v = new Vector2 (-2.37f, -4.31f);
			break;
		case 1:
			v = new Vector2 (-0.77f, -4.31f);
			break;
		case 2:
			v = new Vector2 (0.82f, -4.31f);
			break;
		case 3:
			v = new Vector2 (2.42f, -4.31f);
			break;
		default:
			return;
		}

		GOChipEffect.transform.position = v;
		
		this.chipValue = chipsListValue [index];
		this.GOchoseChip = GOchips [index];

		//動畫
		//GameObject chip = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
		//iTween.MoveBy(chip, iTween.Hash("y", 0.5, "easeType", .1));

	}

	/// <summary>
	/// 投注
	/// </summary>
	/// <param name="type">0:閑，1:閑對，2:莊，3:莊對,4:和</param>
	public void onClickBet(int type){

		if (UserInfo.MyGold < this.chipValue) {
			//alterMsg ("金幣餘額不足");
			return;
		}
		if (state == 2) {
			print ("停止下注");
			return;
		}

		UserInfo.MyGold -= this.chipValue;
		betGoldList [type] += this.chipValue;
		betTotalGoldList [type] += this.chipValue;
		betDetails.Add (chipsCount, new int[2]{type,(int)this.chipValue});

		//播放音效
		ADAddchip.Play();
		//发送消息到服务端
		RoomModel.Bet (type, this.chipValue,chipsCount);
		syncTableInfo ();		
		spawnChip (type==2);

		//把投注信息發送給服務器，服務器返回投注成功與否的消息，和最新的金幣

		//q&a 桌面上的信息多久同步一次，1秒嗎？金幣的變化都需要服務器同步給我

	}

	//撤销筹码
	public void RevocationChips(){

	}

	#endregion

	#region 服务端方法

	/// <summary>
	/// 初始化监听
	/// </summary>
	/// <returns>The state manager.</returns>
	IEnumerator Countdown(){
		

//		foreach (GameObject d in GameObject.FindGameObjectsWithTag ("chip")) {
//			Destroy (d);
//		}

		while (this.RemainingTime >= 0) {

			TextRemainingTime.text = this.RemainingTime.ToString ();

			this.RemainingTime--;

			yield return new WaitForSeconds (1);
		}
	}

	//开始投注
	public void beginBet(){

		alterMsg ("开始投注");

		this.state = 1;
		this.RemainingTime = 20;
	}

	private void alterMsg(string msg,string action="",float timeSec=2.0f){

		//如果有提示框，不提示
		if (GOMsg [0].activeInHierarchy)
			return;
		
		GOMsg[0].SetActive (true);
		//GOMsg[2].GetComponent<Text>().text = msg;
		switch (msg) {
		case "开始投注":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2 (281.8f, 69.5f);
			GOMsg [1].transform.localPosition = new Vector2 (13, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [0];
			break;
		case "停止投注":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2(281.8f,69.5f);
			GOMsg [1].transform.localPosition = new Vector2 (13, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [1];
			break;
		case "闲赢":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2(171.12f, 86.39f);
			GOMsg [1].transform.localPosition = new Vector2 (13.5f, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [2];
			break;
		case "庄赢":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2(171.12f, 86.39f);
			GOMsg [1].transform.localPosition = new Vector2 (13.5f, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [3];
			break;
		case "和":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2(96, 96);
			GOMsg [1].transform.localPosition = new Vector2 (13.5f, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [4];
			break;
		case "闲对":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2 (171.12f, 86.39f);
			GOMsg [1].transform.localPosition = new Vector2 (13.5f, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [5];
			ADXianDui.Play ();
			break;
		case "庄对":
			GOMsg [1].GetComponent<RectTransform> ().sizeDelta = new Vector2 (171.12f, 86.39f);
			GOMsg [1].transform.localPosition = new Vector2 (13.5f, 0);
			GOMsg [1].GetComponent<Image> ().sprite = SPMsg [6];
			ADZhuangDui.Play ();
			break;
		}


		Hashtable args = new Hashtable();
		args.Add("easeType", iTween.EaseType.easeInSine);
		args.Add("time",0.5);
		args.Add("y",1.5);
		iTween.MoveTo(GOMsg[1], args);
		StartCoroutine (WaitTimeClose (timeSec,GOMsg[0],action));
	}

	//等待x秒之后，关闭pannel
	IEnumerator WaitTimeClose (float timeSec,GameObject pannel,string action) {
		yield return new WaitForSeconds (timeSec);
		pannel.SetActive (false);
		GOMsg [1].transform.localPosition = new Vector2 (0, 0);
		if (action == "fapai")
			fapai ();
		else if (action == "balance") {
			initTable ();
			yield return new WaitForSeconds (1.5f);
			state = 1;
		}
	}

	//发牌
	public void fapai(){		

		StartCoroutine (Deal ());

	}


	IEnumerator Deal () {
		yield return new WaitForFixedUpdate ();
		int num = 4;
		ADDealCard.Play ();
		while (num>0) {	
			
			Vector2 spawnPosition = new Vector2(0.11f,2.79f);
			Quaternion spawnRotation = Quaternion.identity;
			GameObject pokerBack = (GameObject)Instantiate (this.GOpokerBack[0], spawnPosition, spawnRotation);
			pokerBack.GetComponent<SpriteRenderer> ().sortingOrder = ++pokersCount;

			Hashtable args = new Hashtable();
			args.Add("easeType", iTween.EaseType.easeInSine);
			args.Add("speed",20f);
			// x y z 标示移动的位置。
			args.Add("y",1.28);

			//前4张牌
			switch (num) {
			case 4:
				args.Add ("x", -3.66);
				GOXianPokers [0] = pokerBack;
				break;
			case 3:
				args.Add ("x", 3.46);
				GOZhuangPokers [0] = pokerBack;
				break;
			case 2:
				args.Add ("x", -2.49);
				GOXianPokers [1] = pokerBack;
				break;
			case 1:
				args.Add ("x", 4.62);
				GOZhuangPokers [1] = pokerBack;
				break;
			default:
				break;
			}

			iTween.MoveTo(pokerBack,args);

			//同步房间剩余牌数
			--RoomModel.RemainingPoker;
			TextRemainingPoker.text = RoomModel.RemainingPoker.ToString();
			num--;

			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (1.5f);
		Balance ();
	}

	//第5或者第6张牌
	IEnumerator Deal1 (int type,int num,bool lastpoker=false) {

		yield return new WaitForSeconds (0.5f);

		ADSendCard.Play ();

		Vector2 spawnPosition = new Vector2(0.11f,2.79f);
		Quaternion spawnRotation = Quaternion.identity;
		GameObject pokerBack = (GameObject)Instantiate (this.GOpokerBack[0], spawnPosition, spawnRotation);
		pokerBack.GetComponent<SpriteRenderer> ().sortingOrder = ++pokersCount;

		Hashtable args = new Hashtable();
		args.Add("easeType", iTween.EaseType.easeInSine);
		args.Add("speed",5f);
		args.Add("delay", 0.1f);

		if(type==0)//闲
		{
			// x y z 标示移动的位置。
			args.Add("y",1.28);
			args.Add ("x", -3.08);
			iTween.MoveTo(GOXianPokers [1],args);
			args ["x"] = -2.5;

			TextResultNum [0].text = RoomModel.XianPoints.ToString ();//闲

		}
		else
		{
			args.Add("y",1.28);
			args.Add ("x", 4.05);
			iTween.MoveTo(GOZhuangPokers [1],args);
			args ["x"] = 4.62;

			TextResultNum [1].text = RoomModel.ZhuangPoints.ToString ();//庄

		}

		args ["speed"] = 20f;
		args ["delay"] = 0;
		iTween.MoveTo(pokerBack,args);
		pokerBack.GetComponent<SpriteRenderer> ().sprite = GOpokers [num].GetComponent<SpriteRenderer> ().sprite;

//		yield return new WaitForSeconds (1.5f);

		if (lastpoker) {
//			ADXianPoints [RoomModel.XianPoints].Play ();
//			yield return new WaitForSeconds (1.5f);
//			ADZhuangPoints [RoomModel.ZhuangPoints].Play ();
//			yield return new WaitForSeconds (1.5f);
//			alterMsg (getWinnerMsg (), "balance", 3.5f);
//			ADWinner.Play ();
			balanced=true;
		}

	}

	/// <summary>
	/// 结算
	/// </summary>
	private void Balance(){

		//有没有第3张牌，>-1代表有
		int xian3=-1,zhuang3=-1;

		foreach(var item in RoomModel.XianPokerList)
		{
//			Debug.LogWarningFormat("key={0},value={1}",item.Key,item.Value);
			if (item.Key < 2) {
				GOXianPokers [item.Key].GetComponent<SpriteRenderer> ().sprite = GOpokers [item.Value].GetComponent<SpriteRenderer> ().sprite;
			}
			else
				xian3 = item.Value;
			
		}

		//判断闲对，如果是播放声音，提示闲对。 对子::前2张如果是一样的就是对子
		if(RoomModel.Winner[1]==2||RoomModel.Winner[1]==3)
			alterMsg ("闲对");

		foreach(var item in RoomModel.ZhuangPokerList)
		{
			if (item.Key < 2) {
				GOZhuangPokers [item.Key].GetComponent<SpriteRenderer> ().sprite = GOpokers [item.Value].GetComponent<SpriteRenderer> ().sprite;
			}
			else
				zhuang3 = item.Value;
		}

		//判断庄对，如果是播放声音，提示庄对。 对子::前2张如果是一样的就是对子
		if(RoomModel.Winner[1]==1||RoomModel.Winner[1]==3)
			alterMsg ("庄对");

		TextResultNum [0].text = ((RoomModel.XianNum[0]+RoomModel.XianNum[1])%10).ToString();//闲
		TextResultNum [1].text = ((RoomModel.ZhuangNum[0]+RoomModel.ZhuangNum[1])%10).ToString();//庄


		//第5或者第6张牌
		if (xian3 > -1) {

			StartCoroutine (Deal1 (0, xian3,zhuang3==-1));

		} 
		if (zhuang3 > -1) {

			StartCoroutine (Deal1 (1, zhuang3,true));

		}

		if (xian3 + zhuang3 == -2) {
//			ADXianPoints [RoomModel.XianPoints].Play ();
//			yield return new WaitForSeconds (1.5f);
//			ADZhuangPoints [RoomModel.ZhuangPoints].Play ();
//			yield return new WaitForSeconds (1.5f);
//			alterMsg (getWinnerMsg (), "balance", 3.5f);
//			ADWinner.Play ();
			balanced=true;
		}
		
	}

	private string getWinnerMsg(){

		GameObject go = new GameObject("x_Image", typeof(Image));
		go.transform.SetParent(HisItemsParent);
		go.GetComponent<Image> ().sprite = SPCR;
		go.transform.localScale = new Vector2 (1, 1);

		string msg="";
		string winnerName = "";
		switch (RoomModel.Winner[0]) {
		case 1:
			winnerName = "ButtonBetZhuang";
			ADWinner = ADZhuangWin;
			msg = "庄赢";

			go.GetComponent<Image> ().color = new Color32(198,42,42,255);
			TextHis [0].text = "" + (int.Parse (TextHis [0].text) + 1);

			break;
		case 2:
			winnerName = "ButtonBetXian";
			ADWinner = ADXianWin;
			msg = "闲赢";

			go.GetComponent<Image> ().color = new Color32(33,115,198,255);
			TextHis [1].text = "" + (int.Parse (TextHis [1].text) + 1);

			break;
		case 3:
			winnerName = "ButtonBetHe";
			ADWinner = ADDraw;
			msg = "和";

			go.GetComponent<Image> ().color = new Color32 (13, 153, 46, 255);
			TextHis [2].text = "" + (int.Parse (TextHis [2].text) + 1);

			break;
		}



		AnimBetButton winner = GameObject.Find (winnerName).GetComponent<AnimBetButton> ();
		winner.Play(true);


		string winnerDuizi = "";
		switch (RoomModel.Winner[1]) {
		case 0:
			winnerDuizi = "nil";
			break;
		case 1:
			winnerDuizi = "ButtonBetZhuang2";
			GameObject.Find (winnerDuizi).GetComponent<AnimBetButton> ().Play(true);
			break;
		case 2:
			winnerDuizi = "ButtonBetXian2";
			GameObject.Find (winnerDuizi).GetComponent<AnimBetButton> ().Play(true);
			break;
		case 3:
			winnerDuizi = "both";
			GameObject.Find ("ButtonBetXian2").GetComponent<AnimBetButton> ().Play(true);
			GameObject.Find ("ButtonBetZhuang2").GetComponent<AnimBetButton> ().Play(true);
			break;
		}


		long winGold = 0;
		for(int i=0; i<5; i++)
		{
			winGold += RoomModel.WinArea[i];
		}
		if (winGold > 0) {
			for (int i = 0; i < betGoldList.Length; i++) {
				winGold += betGoldList [i];
			}
			GOMeWinGold.GetComponent<AnimWinGold> ().Paly ("+" + winGold.ToString ("N0"));
		}
		TextMyGoldDiamonds [0].text = UserInfo.MyGold.ToString("N0");
		TextMyGoldDiamonds [1].text = UserInfo.MyDiamonds.ToString("N0");
//		string msg = string.Format ("{0}赢，{1},我赢了{2}", winnerName, winnerDuizi, winnGold);
		return msg;
	}

	//投注失败
	private void betFailed(){

		for (int i = 0; i < RoomModel.BetFailed.Count; i++) {
			int id = (int)RoomModel.BetFailed[i];
			Destroy (GameObject.Find ("chip" + id));

			foreach (var items in betDetails) {
				if(items.Key==id){
					betGoldList [items.Value[0]] -= items.Value[1];
				}
			}

			RoomModel.BetFailed.RemoveAt (i);
		}

		syncTableInfo ();


	}

	#endregion 

	#region 动画

	/// <summary>
	/// 开始投注区域高亮
	/// </summary>
	/// <returns>The bet.</returns>
	IEnumerator StartBet(){


		foreach (GameObject go in GameObject.FindGameObjectsWithTag("betButton")) {
			go.GetComponent<AnimBetButton> ().Play (false);
		}

		yield return new WaitForSeconds (3.5f);

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("betButton")) {
			go.GetComponent<AnimBetButton> ().Stop ();
		}

	}

	#endregion



	#region 私有方法

	//桌面的筹码数，新的一局开始钱这个值清零
	private int chipsCount=0,pokersCount=0;

	/// <summary>
	/// 生產籌碼
	/// </summary>
	private void spawnChip(bool zhuang=false){

		foreach (Touch touch in Input.touches) {
			HandleTouch(touch.fingerId, Camera.main.ScreenToWorldPoint(touch.position), touch.phase,zhuang);
		}

		// Simulate touch events from mouse events
		if (Input.touchCount == 0) {
			if (Input.GetMouseButtonDown(0) ) {
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Began,zhuang);
//				Debug.Log ("GetMouseButtonDown");
			}
			if (Input.GetMouseButton(0) ) {
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Moved,zhuang);
//				Debug.Log ("GetMouseButton");
			}
			if (Input.GetMouseButton(1) ) {
//				Debug.Log ("GetMouseButton(1)");
			}
			if (Input.GetMouseButtonUp(0) ) {
				HandleTouch(10, Camera.main.ScreenToWorldPoint(Input.mousePosition), TouchPhase.Ended,zhuang);
//				Debug.Log ("GetMouseButtonUp");
			}
		}
	}
	private void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase,bool zhuang) {
		switch (touchPhase) {
		case TouchPhase.Began:
			// TODO
			Debug.Log ("1");
			break;
		case TouchPhase.Moved:
			// TODO
			break;
		case TouchPhase.Stationary:
			// TODO
			break;
		case TouchPhase.Ended:
			// TODO
			Vector2 spawnPosition = touchPosition;
			if (pointinTriangle (new Vector3 (spawnPosition.x, spawnPosition.y, 0), zhuang))//按钮边缘不生产筹码
				return;
			Quaternion spawnRotation = Quaternion.identity;
			GameObject chip = (GameObject)Instantiate (GOchoseChip, spawnPosition, spawnRotation);
			chip.transform.localScale = new Vector2 (0.35f, 0.35f);
			chip.name = "chip" + chipsCount;
			chip.GetComponent<SpriteRenderer> ().sortingOrder = ++chipsCount;
			break;
		}
	}

	//初始化桌面
	private void initTable(){
		TextTableXianBetGold [0].text = "";
		TextTableXianBetGold [1].text = "";
		TextTableXian2BetGold [0].text = "";
		TextTableXian2BetGold [1].text = "";
		TextTableZhuangBetGold [0].text = "";
		TextTableZhuangBetGold [1].text = "";
		TextTableZhuang2BetGold [0].text = "";
		TextTableZhuang2BetGold [1].text = "";
		TextTableHeBetGold [0].text = "";
		TextTableHeBetGold [1].text = "";
		TextResultNum [0].text = "";//闲
		TextResultNum [1].text = "";//庄
		betGoldList = new long[5]{0,0,0,0,0};
		betTotalGoldList = new long[5]{0,0,0,0,0};
		foreach (GameObject d in GameObject.FindGameObjectsWithTag ("chip")) {
			Destroy (d);
		}
		foreach (GameObject d in GameObject.FindGameObjectsWithTag ("poker")) {
			Destroy (d);
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("betButton")) {
			go.GetComponent<AnimBetButton> ().Stop ();
		}

	}

	//同步桌面信息
	private void syncTableInfo(){
		//myself gold diamonds
		TextMyGoldDiamonds [0].text = UserInfo.MyGold.ToString("N0");
		TextMyGoldDiamonds [1].text = UserInfo.MyDiamonds.ToString("N0");
		//闲 自己投注的筹码 总投注筹码  0:閑，1:閑對，2:莊，3:莊對,4:和
		if(betGoldList [0]>0)
			TextTableXianBetGold [0].text = betGoldList [0].ToString();
		if(betTotalGoldList [0]>0)
			TextTableXianBetGold [1].text = betTotalGoldList [0].ToString();
		//闲对 自己投注的筹码 总投注筹码
		if(betGoldList [1]>0)
			TextTableXian2BetGold [0].text = betGoldList [1].ToString();
		if(betTotalGoldList [1]>0)
			TextTableXian2BetGold [1].text = betTotalGoldList [1].ToString();
		//庄 自己投注的筹码 总投注筹码
		if(betGoldList [2]>0)
			TextTableZhuangBetGold [0].text = betGoldList [2].ToString();
		if(betTotalGoldList [2]>0)
			TextTableZhuangBetGold [1].text = betTotalGoldList [2].ToString();
		//庄对 自己投注的筹码 总投注筹码
		if(betGoldList [3]>0)
			TextTableZhuang2BetGold [0].text = betGoldList [3].ToString();
		if(betTotalGoldList [3]>0)
			TextTableZhuang2BetGold [1].text = betTotalGoldList [3].ToString();
		//和 自己投注的筹码 总投注筹码
		if(betGoldList [4]>0)
			TextTableHeBetGold [0].text = betGoldList [4].ToString();
		if(betTotalGoldList [4]>0)
			TextTableHeBetGold [1].text = betTotalGoldList [4].ToString();

		TextRemainingPoker.text = RoomModel.RemainingPoker.ToString();
	}


	// 判断点P 是否在三角形ABC内
	private bool pointinTriangle(Vector3 P,bool zhuang)
	{
		Vector3 A = new Vector3 (-5.28f,-1.1f,0);
		Vector3 B = new Vector3 (-3.85f,-2.69f,0); 
		Vector3 C = new Vector3 (-5.15f,-2.58f,0);
		if (zhuang) {
			A = new Vector3 (5.42f,-1.1f,0);
			B = new Vector3 (4.05f,-2.67f,0); 
			C = new Vector3 (5.3f,-2.5f,0);
		}
		
		Vector3 v0 = C - A;
		Vector3 v1 = B - A;
		Vector3 v2 = P - A;

//		print (C);
//		print (A);
//		print (v0);

		float dot00 = v0.x * v0.x + v0.y * v0.y + v0.z * v0.z;
		float dot01 = v0.x * v1.x + v0.y * v1.y + v0.z * v1.z;
		float dot02 = v0.x * v2.x + v0.y * v2.y + v0.z * v2.z;
		float dot11 = v1.x * v1.x + v1.y * v1.y + v1.z * v1.z;
		float dot12 = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;

		float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

		float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
		if (u < 0 || u > 1) { // if u out of range, return directly
			return false;
//			print ("false");
//			return;
		}

		float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
		if (v < 0 || v > 1) { // if v out of range, return directly
			return false;
//			print ("false");
//			return;
		}
//		print (u + v <= 1);
		return u + v <= 1;
	}


	/// <summary>
	/// 同步筹码
	/// </summary>
	private void syncChips(){
		for(int j=0;j<4;j++){
			RoomModel.Chips [0, j] / 100;
		}
	}


	#endregion

}
