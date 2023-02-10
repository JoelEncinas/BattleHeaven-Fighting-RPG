using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickGoToShop : MonoBehaviour
{
    public void GoToShop()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Shop.ToString());
        ShopTab.SetTab(EventSystem.current.currentSelectedGameObject.name);
    }
}
