//
// Author:  Prosics <Prosics@163.com>
// Date: 2017/6/10
// Description:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;
using Kimmidoll;

namespace GameJoy
{
    public class UserController : Controller<UserModel>
    {
        protected override void OnInitialize()
        {


        }
        protected override void Awake()
        {
            base.Awake();
            GameObject.DontDestroyOnLoad(gameObject);
			NetMsgCenter.instance.AddEventListener ((int)CProtocol.RLogin, OnLogin);
			NetMsgCenter.instance.AddEventListener ((int)CProtocol.REditHeadPic, OnEditHeadPic);
        }
		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			NetMsgCenter.instance.RemoveEventListener ((int)CProtocol.RLogin,OnLogin);
			NetMsgCenter.instance.RemoveEventListener ((int)CProtocol.REditHeadPic, OnEditHeadPic);
		}
		void OnLogin(List<System.Object> args)
		{
			//model.uid = (int)args [2];
			//model.golds = (int)args [3];

			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("lobby");
		}

		void OnEditHeadPic(List<System.Object> args)
		{
			Message.Send (MVC_MsgId.Ntf_HeadPic_Changed.ToString());

		}






           
    }
}

