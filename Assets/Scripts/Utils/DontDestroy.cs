using UnityEngine;
public class DontDestroy : MonoBehaviour
{
    // used for fighterWrapper
    public static DontDestroy instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}