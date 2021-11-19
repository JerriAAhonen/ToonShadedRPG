using TMPro;
using UnityEngine;

namespace UI
{
	public class InteractableNamePlate : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI label;

		private RectTransform rt;
		private Interactable interactable;
		public Vector3 InteractableWorldPos => interactable.transform.position;
		
		public void Init(Interactable i)
		{
			rt = GetComponent<RectTransform>();
			interactable = i;
			label.text = i.DisplayName;
		}

		public void SetPosition(Vector3 pos)
		{
			pos.z = 0;
			rt.position = pos;
		}
		
		public void SetActive(bool active)
		{
			if (gameObject.activeInHierarchy != active)
				gameObject.SetActive(active);
		}
	}
}