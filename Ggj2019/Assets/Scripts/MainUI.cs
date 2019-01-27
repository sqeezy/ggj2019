using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
	public Image BlueBar;
	public Image RedBar;
	public Image ShipBar;

	public void SetBlueBar(int value, int max)
	{
		BlueBar.fillAmount = remap(value, 0, max, 0, 1);
	}

	public void SetShipBar(int value, int max)
	{
		ShipBar.fillAmount = remap(value, 0, max, 0, 1);
	}

	public void SetRedBar(int value, int max)
	{
		RedBar.fillAmount = remap(value, 0, max, 0, 1);
	}

	float remap(float s, float oldLow, float oldHigh, float newLow, float newHigh)
	{
		return newLow + (s-oldLow)*(newHigh-newLow)/(oldHigh-oldLow);
	}
}