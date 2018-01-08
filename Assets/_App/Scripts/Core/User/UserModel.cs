// ********************************************************************************************
// Author:  Prosics <Prosics@163.com>
// Time: 2017/6/6
// Description:
// ********************************************************************************************
using System;
using Prosics.Utils;
using Prosics.MVC;
namespace GameJoy
{
    public class UserModel : Model , IUserModel
    {
		public int uid{ get; set;}
		public string nick{ get; set;}
		public int golds{get; set;}
		public int dianmonds{get;set;}
		public int HeadPicId{get;set;}

    }
}

