using System;
using Extensions;
using UnityEngine;

namespace Util
{
    // Continues to exist only if the singleton doesn't exist yet
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : class
    {
        private static T instance;
        public static T Instance => instance != null && MiscUtil.IsNotNull(instance) ? instance : null;

        protected virtual void Awake()
        {
            ProcessSingleton();
        }

        protected bool IsAlive { get; private set; }

        private void ProcessSingleton()
        {
            if (!Application.isPlaying)
                return;

            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            IsAlive = true;
            instance = this as T;
            if (instance == null)
                throw new InvalidCastException($"Unable to cast {GetType()} to {typeof(T)}");

            gameObject.GetOrAddComponent<CallbackOnDestroy>().Init(() =>
            {
                instance = null;
            });
        }
    }

    // Keeps itself alive for the whole session
    public abstract class PersistentSingletonBehaviour<T> : SingletonBehaviour<T> where T : class
    {
        protected override void Awake()
        {
            if (!Application.isPlaying)
                return;

            base.Awake();
		
            if (IsAlive)
                DontDestroyOnLoad(gameObject);
        }
    }

    public sealed class CallbackOnDestroy : MonoBehaviour
    {
        private Action m_callback;
	
        public void Init(Action callback)
        {
            m_callback = callback;
        }

        private void OnDestroy()
        {
            m_callback?.Invoke();
        }
    }
}