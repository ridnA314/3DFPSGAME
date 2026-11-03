using UnityEngine;

public class SingletoneBehaviour<T> : MonoBehaviour where T : SingletoneBehaviour<T>
{
    private static T _instance = null;
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        // T가 UnityEngine.Object이므로 파괴된 인스턴스도 null로 판정된다.
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;

        // DontDestroyOnLoad는 루트 오브젝트에만 적용되므로 Managers 같은 부모 그룹째로 유지한다.
        PersistentObjects.Register(transform.root.gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
