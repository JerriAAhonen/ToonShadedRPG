using System;
using UnityEngine;

namespace Util
{
	public class TriggerCallbacks : MonoBehaviour
	{
		[SerializeField] private LayerMask mask;

		public event Action<Collider> Enter;
		public event Action<Collider> Exit;

		private void OnTriggerEnter(Collider other)
		{
			if (LayerMaskUtil.IsLayerInMask(mask, other.gameObject.layer))
				Enter?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			if (LayerMaskUtil.IsLayerInMask(mask, other.gameObject.layer))
				Exit?.Invoke(other);
		}
	}
}