using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class CrosshairTooltips : SingletonBehaviour<CrosshairTooltips>
{
	public TextMeshProUGUI targetDisplayName;
	public TextMeshProUGUI interactionTooltip;

	private void Start()
	{
		Hide();
	}

	public void ShowTargetDisplayName(string text, bool canInteract, string interactionText)
	{
		targetDisplayName.text = text;
		targetDisplayName.gameObject.SetActive(true);

		if (canInteract)
		{
			var interactionKey = InputHandler.Instance.InteractionKey;
			interactionTooltip.text = $"[{interactionKey.ToString()} {interactionText}]";
			interactionTooltip.gameObject.SetActive(true);
		}
	}

	public void Hide()
	{
		if (targetDisplayName.gameObject.activeSelf)
			targetDisplayName.gameObject.SetActive(false);
		if (interactionTooltip.gameObject.activeSelf)
			interactionTooltip.gameObject.SetActive(false);
	}
}
