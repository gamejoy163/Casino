using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kimmidoll;

public class Login : MonoBehaviour {


	public string ip;
	public int port;

	NetworkInterface nk;


	void Awake(){

	}

	void Start () {
		nk = new NetworkInterface (this.ip, this.port);

	}
	
	void Update () {
		
	}
}
