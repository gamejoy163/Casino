// ********************************************************************************************
// Author:  Prosics <Prosics@163.com>
// Time: 2017/6/6
// Description:
// ********************************************************************************************
using System;
using Prosics.Utils;
using Prosics.MVC;

using Kimmidoll;


namespace GameJoy
{
    public class GameModel : Model , IGameModel
    {


        public ModelRef<UserModel> _userModel = null;
        public IUserModel userModel
        {
            get
            {
                return _userModel.Model;
            }

        }

		public static NetworkInterface netInterface{private set; get;}




		public GameModel()
		{
		}


    }
}

