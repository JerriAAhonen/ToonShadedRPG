using UnityEngine;

namespace Util
{
    public static class MiscUtil
    {
        public static bool IsNull(object o)
        {
            return o == null || o.Equals(null);
        }
	
        public static bool IsNotNull(object o)
        {
            return o != null && !o.Equals(null);
        }
        
        public static Vector2 RandDir
        {
            get
            {
                var x = Random.Range(-1f, 1f);
                var y = Random.Range(-1f, 1f);
                return new Vector2(x, y);
            }
        }
    }
}