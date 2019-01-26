using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
	public Image BlueBar;
	public Image RedBar;
	public Image ShipBar;

	public int ShipMax;
	public int BlueBarMax;
	public int RedBarMax; 

	public void SetBlueBar(int value)
	{
		BlueBar.fillAmount = remap(value, 0, BlueBarMax, 0, 1);
	}

	public void SetShipBar(int value)
	{
		ShipBar.fillAmount = remap(value, 0, ShipMax, 0, 1);
	}

	public void SetRedBar(int value)
	{
		RedBar.fillAmount = remap(value, 0, RedBarMax, 0, 1);
	}

	float remap(float s, float oldLow, float oldHigh, float newLow, float newHigh)
	{
		return newLow + (s-oldLow)*(newHigh-newLow)/(oldHigh-oldLow);
	}
}