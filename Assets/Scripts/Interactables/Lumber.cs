using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

public class Lumber : Interactable
{
	public override string DisplayName { get; } = "Wood";
	public override string InteractionText { get; } = "Collect";

	public override void Interact()
	{
		Destroy(gameObject);
	}
}
