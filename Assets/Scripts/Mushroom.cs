using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Collectable {
	protected override void OnRabitHit (HeroRabit rabit)
	{
		//Level.current.addCoins (1);
		rabit.changeSize();
		this.CollectedHide ();
	}


}
