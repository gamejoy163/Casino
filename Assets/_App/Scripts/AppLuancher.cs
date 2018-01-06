// ********************************************************************************************
// Author:  Prosics <Prosics@163.com>
// Time: 2017/6/10
// Description:
// ********************************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;
namespace GameJoy
{
    public class AppLuancher : MonoScriptBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
			GameManager.instance.Init();
        }
    }   
}


