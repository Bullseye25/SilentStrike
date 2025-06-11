﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlayerCar {
	public int ID;
	public string Name;
	public string Type;
	public int UnlockPrice;
	public int UpgradePrice;
	public int UpgradeLevel;
	public float Speed;
	public float Acceleration;
	public float Control;
	public float N2O;
	public bool isLocked;
	public bool isSkip = false;

}
