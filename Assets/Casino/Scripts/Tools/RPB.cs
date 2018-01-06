using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RPB : MonoBehaviour {

	public Transform LoadingBar;
	public Transform TextIndicator;
//	[SerializeField]private float currentAmout;
//	[SerializeField]private float speed;
	private float currentAmout;
	private float speed;

	//剩余时间
	private float RemainingTime;

	//总时间
	private float TotalTime;

	private bool activate;

	void Start(){
//		TextIndicator.GetComponent<Text>().text = "100%";
//		TextIndicator.GetComponent<Text>().text = "15";
//		LoadingBar.GetComponent<Image> ().fillAmount = 1.0f;

		//初始化所剩时间
//		float passTime = TotalTime-RemainingTime;
//		float fill = 100.0f / TotalTime * passTime / 100.0f;
//		TextIndicator.GetComponent<Text>().text = RemainingTime.ToString();
//		LoadingBar.GetComponent<Image> ().fillAmount = fill;
//		currentAmout = 100.0f - 100.0f / TotalTime * passTime;
//		speed = 100.0f / TotalTime;

		currentAmout = 100;
		activate = false;
	}

	private float timeLeft = 0.0f;
	void Update () {

		if (activate) {
			if (currentAmout >= 0) {
				currentAmout -= speed * Time.deltaTime;
//			TextIndicator.GetComponent<Text>().text = ((int)currentAmout).ToString()+"%";
				timeLeft += Time.deltaTime;
				if (timeLeft >= 1.0f) {
					RemainingTime--;
					if (RemainingTime < 0)
						return;
					TextIndicator.GetComponent<Text> ().text = RemainingTime.ToString ();
					timeLeft = 0.0f;
				}
			}

			LoadingBar.GetComponent<Image> ().fillAmount = currentAmout / 100;
		}
	}

	public void Start(float totalTime,float remainingTime){
		this.TotalTime = totalTime;
		this.RemainingTime = remainingTime;

		//初始化所剩时间
		float passTime = TotalTime-RemainingTime;
		float fill = 100.0f / TotalTime * passTime / 100.0f;
		TextIndicator.GetComponent<Text>().text = RemainingTime.ToString();
		LoadingBar.GetComponent<Image> ().fillAmount = fill;
		currentAmout = 100.0f - 100.0f / TotalTime * passTime;
		speed = 100.0f / TotalTime;
		activate = true;
//		RemainingTime--;
	}

}
