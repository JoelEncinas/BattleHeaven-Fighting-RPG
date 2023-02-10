using System.Collections;
using UnityEngine;

public class OnClickGoToMainMenuOrCup : MonoBehaviour
{
    public void GoToMainMenu()
    {
        if (!CombatMode.isSoloqEnabled)
            IGoToScene(SceneNames.Cup);
        else
            IGoToScene(SceneNames.MainMenu);
    }

    private IEnumerator GoToScene(SceneNames sceneName)
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
    }

    private void IGoToScene(SceneNames sceneName)
    {
        StartCoroutine(GoToScene(sceneName));
    }
}
