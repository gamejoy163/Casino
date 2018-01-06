//
// Author:  Prosics <Prosics@163.com>
// Date: 2017/6/10
// Description:
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;


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
        }
           
    }
}

