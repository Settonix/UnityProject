using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable {
	protected override void OnRabitHit (HeroRabit rabit)
	{
		//Level.current.addCoins (1);s
		if (!rabit.IsGhost()) {
			rabit.bang ();
			this.CollectedHide ();
		}

	}


}
