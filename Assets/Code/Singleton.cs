using UnityEngine;
 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
 
	public static T global {
		get {
            if (_instance == null) {
				_instance = (T) FindObjectOfType(typeof(T));
            }

            return _instance;
		}
	}
 
	private static bool applicationIsQuitting = false;

	public void OnDestroy () {
		applicationIsQuitting = true;
	}
}