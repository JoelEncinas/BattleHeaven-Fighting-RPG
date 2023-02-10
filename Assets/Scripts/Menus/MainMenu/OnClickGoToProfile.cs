using UnityEngine;

public class OnClickGoToProfile : MonoBehaviour
{
    public GameObject profile;

    public void ShowProfile()
    {
        profile.SetActive(true);
    }
}
