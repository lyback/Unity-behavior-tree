using Neatly.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        NeatlyTimer.Init();
        MainBattleManager.Instance.Start();
	}
	
}
