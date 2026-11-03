using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// DontDestroyOnLoad로 유지 중인 루트 오브젝트들을 관리한다.
public static class PersistentObjects
{
    private static readonly HashSet<GameObject> Roots = new HashSet<GameObject>();

    public static void Register(GameObject root)
    {
        if (Roots.Add(root))
        {
            Object.DontDestroyOnLoad(root);
        }
    }

    // 유지 중인 오브젝트를 현재 씬으로 되돌려 다음 씬 로드 때 함께 언로드되게 한다.
    // Destroy()는 프레임 끝까지 지연되어 새 씬의 싱글턴 Awake()보다 늦게 처리되므로 사용하지 않는다.
    public static void ReturnToActiveScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        foreach (GameObject root in Roots)
        {
            if (root != null)
            {
                SceneManager.MoveGameObjectToScene(root, activeScene);
            }
        }

        Roots.Clear();
    }
}
