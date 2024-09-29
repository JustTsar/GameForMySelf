using UnityEngine;

namespace Utility 
{ 
    public class SingletonBehavior<T> : MonoBehaviour where T : Component 
    {
        private static T _instance;
        
        public static T Instance 
        { 
            get
            {
                EnsureInstanceCreated(); 
                return _instance;
            } 
        }

        
        public static bool HasInstance => _instance != null;

        
        public static bool TryGetInstance(out T instance)
            {
                instance = _instance;
                return instance != null;
            }
        
        private static void EnsureInstanceCreated()
        {
            if (_instance != null)
                    return;

            var instances = FindObjectsOfType<T>(true);
            switch (instances.Length)
            {
                case 0:
                    Debug.LogWarning("Can't find instance");
                    break;
                case 1:
                    _instance = instances[0];
                    break;
                default:
                    Debug.LogWarning($"Found {instances.Length} instances");
                    _instance = instances[0];
                    break;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"Ignored the second instance: {name}. Using the first one: {_instance.name}");
            }
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}