// ******************************************************************************************** 
// Author:  Prosics 
// Date: 2017/5/11
// Copyright (c) 2017 Prosics
// Description:
// ********************************************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prosics.Utils;
using Prosics.MVC;

namespace GameJoy
{
    public class GameManager : SingletonScript<GameManager>
    {
        public ModelRef<GameModel> _gameModel = null;
        public IGameModel gameModel
        {
            get
            {
                return _gameModel.Model;
            }

        }

		protected override void Awake ()
		{
			base.Awake ();
		}


        public void Init()
        {
            GameModel gameM = new GameModel();
            _gameModel = new ModelRef<GameModel>(gameM);
            Controller.Instantiate<GameController>(gameM,transform);


			UnityEngine.SceneManagement.SceneManager.LoadScene("login");




        


        }

        
    }
}

