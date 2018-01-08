// ********************************************************************************************
// Author:  Prosics <Prosics@163.com>
// Time: 2017/6/10
// Description:
// ********************************************************************************************
using System;

namespace GameJoy
{
    public interface IUserModel
    {
		int uid{ get; }
		string nick{ get;}
		int golds{get;}
		int dianmonds{get;}
		int HeadPicId{get;}
    }
}

