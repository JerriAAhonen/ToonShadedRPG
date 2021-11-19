using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace UI
{
	public class Hud : SingletonBehaviour<Hud>
	{
		[SerializeField] private InteractableNamePlate interactableNamePlateProto;
		[SerializeField] private RectTransform namePlateParent;

		private Camera cam;
		private float namePlateOffset = 30f;
		private Dictionary<Interactable, InteractableNamePlate> namePlates = new Dictionary<Interactable, InteractableNamePlate>();
		private List<Interactable> toBeRemoved = new List<Interactable>();

		private void Start()
		{
			cam = Camera.main;
			interactableNamePlateProto.SetActive(false);
		}

		public void RegisterInteractable(Interactable interactable)
		{
			if (namePlates.ContainsKey(interactable))
				return;
			
			var newNamePlate = Instantiate(interactableNamePlateProto, namePlateParent);
			newNamePlate.Init(interactable);
			namePlates.Add(interactable, newNamePlate);
		}

		public void UnregisterInteractable(Interactable interactable)
		{
			if (interactable == null || !namePlates.ContainsKey(interactable))
				return;
			
			namePlates[interactable].SetActive(false);
			namePlates.Remove(interactable);
		}

		private void Update()
		{
			toBeRemoved.Clear();
			foreach (var namePlate in namePlates)
			{
				if (namePlate.Key == null)
				{
					namePlate.Value.SetActive(false);
					toBeRemoved.Add(namePlate.Key);
					continue;
				}
				
				var wpos = namePlate.Value.InteractableWorldPos;
				var spos = cam.WorldToScreenPoint(wpos);
				if (!IsVisible(spos))
				{
					namePlate.Value.SetActive(false);
					continue;
				}

				spos.y += namePlateOffset;
				namePlate.Value.SetPosition(spos);
				namePlate.Value.SetActive(true);
			}

			foreach (var interactable in toBeRemoved)
			{
				namePlates.Remove(interactable);
			}
		}

		private bool IsVisible(Vector3 spos)
		{
			if (spos.x >= 0 && spos.x < Screen.width
			                && spos.y >= 0 && spos.y < Screen.height
			                && spos.z > 0)
				return true;
			return false;
		}
	}
}