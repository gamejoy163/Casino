// HeadSelectWnd.cs
// Author:prosics <Prosics@163.com>
// Date:1/8/2018
// Copyright (c) 2018 ${Author}
// Description:
//
using UnityEngine;
using UnityEngine.UI;

namespace GameJoy
{
	public class HeadSelectWnd : BaseWnd
	{
		[SerializeField]
		GridLayoutGroup _grid;

		public event System.Action<int> eventSelectedHeadPic;
		public void AddHeadPic(string headPicPath,int HeadPicId)
		{
			
			GameObject ori =	ResManager.instance.Load(GlobalPath.Path_Prefabs_UI_Items + "headPic") as GameObject ;

			GameObject go = 	GameObject.Instantiate (ori,_grid.transform);
			go.name = "headPic_" + HeadPicId;
			Texture2D tex=	ResManager.instance.Load(headPicPath) as Texture2D;
			Sprite spt = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			go.GetComponent<Image> ().sprite = spt;
			go.GetComponent<Button> ().onClick.AddListener (delegate() {
				OnClickHeadPic(go);
				OnClickCloseBtn();
			});
		}

		void OnClickHeadPic(GameObject go)
		{
			if (eventSelectedHeadPic != null)
			{
				string[] strs = go.name.Split (new char[]{ '_' });
				int idx = System.Convert.ToInt32 (strs[1]);
				eventSelectedHeadPic(idx);

			}
				
		}
	}
}

