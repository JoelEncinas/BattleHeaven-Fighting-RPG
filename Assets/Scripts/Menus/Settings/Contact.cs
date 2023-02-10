using UnityEngine;
using UnityEngine.UI;

public class Contact : MonoBehaviour
{
    GameObject contactBtn;

    [System.Obsolete]
    private void Awake()
    {
        contactBtn = GameObject.Find("Button_Support");
        contactBtn.GetComponent<Button>().onClick.AddListener(() => OnClickOpenMail());
    }

    [System.Obsolete]
    private void OnClickOpenMail()
    {
        string email = "mamecorp.games@gmail.com";
        string subject = MyEscapeURL("Fight Game");
        string body = MyEscapeURL("");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    [System.Obsolete]
    string MyEscapeURL(string url)
    {
        // WWW class is deprecated
        // It has to be sanitized or else it will crash silently
        return WWW.EscapeURL(url).Replace("+", "%20");
    }
}
