using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ModeControler {

	private Dictionary<String, Mode> modeList = new Dictionary<String, Mode>();

	public void AddMode(String name, int priority, Mode.Action action, Mode.Actual actual) {
		modeList.Add(name, new Mode(name, priority, action, actual));
	}

	public Mode getCurrentMode () {
		Mode currentMode = null;
		int maxPriority = 0;
		foreach(Mode mode in modeList.Values)
		{
			if(mode.actual() && (mode.prioity > maxPriority)) {
				currentMode = mode;
				maxPriority = mode.prioity;
			}
		}
		return currentMode;
	}

	public class Mode {
		public String name;
		public Action action;
		public Actual actual;
		public int prioity;

		public Mode (String name, int priority, Action action, Actual actual) {
			this.name = name;
			this.action = action;
			this.prioity = priority;
			this.actual = actual;
		}

		public delegate void Action ();
		public delegate bool Actual ();
	}
}
