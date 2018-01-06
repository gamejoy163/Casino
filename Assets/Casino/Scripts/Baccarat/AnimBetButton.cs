using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimBetButton : MonoBehaviour {

	/// <summary>
	/// 播放动画
	/// </summary>
	public bool IsPlay = false;

	/// <summary>
	/// 变暗的最小值
	/// </summary>
	public float MinAlpha;

	/// <summary>
	/// 速度
	/// </summary>
	public float Speed;

	/// <summary>
	/// 按钮动画图片,开始投注是金色:0，结算是紫色:1
	/// </summary>
	public Sprite[] bg;

	private float m_alpha;
	private int symbol = 1;

	void Start () {
	}

	
	void Update () {
		if (!IsPlay && m_alpha == MinAlpha)
			return;
		
		m_alpha += symbol * Speed * Time.deltaTime;
		UpdateSplashAlpha ();
	}

	/// <summary>
	/// Play the specified Balance.
	/// </summary>
	/// <param name="Balance">投注=false ，结算=true</param>
	public void Play(bool Balance){
		IsPlay = true;
		GetComponent<Image> ().sprite = bg [Balance ? 1 : 0];
	}

	public void Stop(){
		IsPlay = false;
		Color spriteColor = GetComponent<Image> ().color;
		spriteColor.a = MinAlpha;
		GetComponent<Image> ().color = spriteColor;
	}

	private void UpdateSplashAlpha()
	{
		Color spriteColor = GetComponent<Image> ().color;
		spriteColor.a = m_alpha;
		GetComponent<Image> ().color = spriteColor;
		if (m_alpha > 1f) {
			m_alpha = 1f;
			symbol = -symbol;
		} else if (m_alpha < MinAlpha) {
			m_alpha = MinAlpha;
			symbol = -symbol;
		}
	}


}
