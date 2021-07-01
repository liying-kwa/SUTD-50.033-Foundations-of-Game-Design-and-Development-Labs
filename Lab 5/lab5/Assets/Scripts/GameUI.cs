using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class GameUI : Singleton<GameUI>
{
	override  public  void  Awake(){
		base.Awake();
		Debug.Log("awake called");
		// other instructions...
	}
}
