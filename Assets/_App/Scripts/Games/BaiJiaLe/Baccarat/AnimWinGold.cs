using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimWinGold : MonoBehaviour {

	public float TimeLeft;

	/// <summary>
	/// 变暗的最小值
	/// </summary>
	public float MinAlpha;

	/// <summary>
	/// 速度
	/// </summary>
	public float Speed;

	private float m_alpha;
	private int symbol = 1;
	private bool stared = false;
	public float timeLeft;

	void Start () {
		
	}

	void Update () {
		
		if (!stared)
			return;

		timeLeft += Time.deltaTime;
		if (timeLeft > TimeLeft)
			reset ();
		
		m_alpha += symbol * Speed * Time.deltaTime;
		UpdateSplashAlpha ();
	}

	public void Paly(string text){
		stared = true;
		GetComponent<Text> ().text = text;
	}

	private void reset(){
		GetComponent<Text> ().text = "";
		Color spriteColor = GetComponent<Text> ().color;
		spriteColor.a = 0.0f;
		GetComponent<Text> ().color = spriteColor;
		timeLeft = 0.0f;
		stared = false;
	}

	private void UpdateSplashAlpha()
	{
		Color spriteColor = GetComponent<Text> ().color;
		spriteColor.a = m_alpha;
		GetComponent<Text> ().color = spriteColor;
		if (m_alpha > 1f) {
			m_alpha = 1f;
			symbol = -symbol;
		} else if (m_alpha < MinAlpha) {
			m_alpha = MinAlpha;
			symbol = -symbol;
		}
	}

}
