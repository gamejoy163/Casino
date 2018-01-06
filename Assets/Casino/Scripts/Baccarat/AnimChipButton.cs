using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimChipButton : MonoBehaviour {

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


	private void UpdateSplashAlpha()
	{
		Color spriteColor = GetComponent<SpriteRenderer> ().color;
		spriteColor.a = m_alpha;
		GetComponent<SpriteRenderer> ().color = spriteColor;
		if (m_alpha > 1f) {
			m_alpha = 1f;
			symbol = -symbol;
		} else if (m_alpha < MinAlpha) {
			m_alpha = MinAlpha;
			symbol = -symbol;
		}
	}


}
