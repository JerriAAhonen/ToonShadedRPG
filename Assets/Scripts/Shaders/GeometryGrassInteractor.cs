using UnityEngine;

public class GeometryGrassInteractor : MonoBehaviour
{
	private void Update()
	{
		Shader.SetGlobalVector("_PositionMoving", transform.position);
	}
}