using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kimmidoll;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class clientapp : MonoBehaviour {

	public string ip;
	public int port;
	public GameObject kimmidoll;
	public Text TextUname;

	void Awake(){
		DontDestroyOnLoad(kimmidoll);
	}

	void Start () {
		UserInfo.Net = new NetworkInterface (this.ip, this.port);
		UserInfo.Net.Start ();
	}

	private bool isSend=false;
	void Update () {

		if (UserInfo.Net.IsConnected&&!isSend) {
//			ByteBuffer buffer = new ByteBuffer();
//			buffer.WriteInt(0);
//			buffer.WriteInt(0x100);
//			buffer.WriteString(TextUname.text);//account
//			buffer.WriteString("95270");//password
//			buffer.WriteInt(200);//version
//			buffer.WriteString("cn");//language
//			buffer.WriteString("iphonex");
//			buffer.WriteString("android");
//			buffer.WriteMd5();
//			UserInfo.Net.SendMessage(buffer.ToBytes());
//			isSend = true;
		}

		if (UserInfo.Login) {
			UserInfo.Login = false;
			SceneManager.LoadSceneAsync("baccaratRoom");
		}
			
	}

	private int[] roomlist;
	public void Testsend(){
		ByteBuffer buffer = new ByteBuffer();
		buffer.WriteInt(0);
		buffer.WriteInt(0x100);
		buffer.WriteString(TextUname.text==""?"3515":TextUname.text);//account
		buffer.WriteString("95270");//password
		buffer.WriteInt(200);//version
		buffer.WriteString("cn");//language
		buffer.WriteString("iphonex");
		buffer.WriteString("android");
		buffer.WriteInt(1);//0 login  1:注册
		buffer.WriteMd5();
		UserInfo.Net.SendMessage(buffer.ToBytes());

	}

	void FixedUpdate(){
	}

	void OnDestroy()
	{
		UserInfo.Net.SocketQuit ();
	}
}
