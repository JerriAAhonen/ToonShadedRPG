using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
	public TextMeshProUGUI buildModeIndicator;

	private void Start()
	{
		InputHandler.Instance.BuildModeToggle += UpdateIndicator;
		UpdateIndicator(InputHandler.Instance.InBuildingMode);
	}

	private void UpdateIndicator(bool active)
	{
		buildModeIndicator.text = active ? "BuildMenu: On" : "BuildMenu: Off";
	}
}
