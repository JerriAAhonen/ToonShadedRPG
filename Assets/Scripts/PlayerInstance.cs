using UnityEngine;
using Util;

public class PlayerInstance : SingletonBehaviour<PlayerInstance>
{
	public void TakeDamage()
	{
		Debug.Log("Ouchies!");
	}
}
