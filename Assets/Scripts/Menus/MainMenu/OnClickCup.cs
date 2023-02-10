using UnityEngine;
using UnityEngine.UI;

public class OnClickCup : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnClickHandler());
    }
    public void OnClickHandler()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Cup.ToString());
    }
}
