using UnityEngine;

namespace Util
{
	public static class LayerMaskUtil
	{
		public static bool IsLayerInMask(LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}
	}
}