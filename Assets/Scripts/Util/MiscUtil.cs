using System.Collections.Generic;
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

        public static Interactable GetClosestInteractable(Vector3 pos, IEnumerable<Interactable> candidates)
        {
            float dist = 0f;
            Interactable closest = null;
            foreach (var candidate in candidates)
            {
                var distance = Vector3.Distance(pos, candidate.transform.position);
                if (distance < dist || closest == null)
                    closest = candidate;
            }

            return closest;
        }
    }
}