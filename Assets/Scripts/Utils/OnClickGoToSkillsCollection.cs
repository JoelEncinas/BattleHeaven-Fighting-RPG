using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickGoToSkillsCollection : MonoBehaviour
{
    public void GoToSkillsCollection()
    {
        Notifications.ResetCardsUnseen();
        Notifications.TurnOffNotification();
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Inventory.ToString());
    }
}
