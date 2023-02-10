using System.Collections;
using UnityEngine;

public class OnClickBattle : MonoBehaviour
{
    public void OnClickHandler()
    {
        if(gameObject.name.Contains("Battle"))
            CombatMode.isSoloqEnabled = true;
        if(gameObject.name.Contains("Cup"))
            CombatMode.isSoloqEnabled = false;

        IGoToCombat();
    }

    private IEnumerator GoToCombat()
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.Combat.ToString());
    }

    private void IGoToCombat()
    {
        StartCoroutine(GoToCombat());
    }
}
