using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Elixir : MonoBehaviour
{
    public void OnClickTriggerElixirEffects()
    {
        FindObjectOfType<AudioManager>().Play("S_Elixir_Used");
        Image iconImage = this.transform.Find("Icon").GetComponent<Image>();
        iconImage.color = VFXUtils.GetUsedButtonColor(iconImage.color);
        this.GetComponent<Button>().interactable = false;
        TriggerElixirEffects(Combat.player);
    }

    IEnumerator StopElixirAnimation(Transform elixir)
    {
        yield return new WaitForSeconds(1.5f);
        elixir.GetComponent<Animator>().enabled = false;
        elixir.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void TriggerElixirEffects(Fighter fighter)
    {
        Transform elixir = fighter.transform.Find("VFX/Elixir_VFX");
        elixir.GetComponent<Animator>().Play("elixir_0", -1, 0f);

        //Heals for 50% of the missing hp
        float missingHp = fighter == Combat.player ? Combat.playerMaxHp - fighter.hp : Combat.botMaxHp - fighter.hp;
        float hpToRestore = missingHp * (GlobalConstants.SkillHeals.Elixir / 100);
        fighter.hp += hpToRestore;

        VFXUtils.DisplayFloatingHp(fighter, GameObject.Find("CombatManager").GetComponent<Combat>().floatingHp, hpToRestore, GlobalConstants.healColor);

        Combat.fightersUIDataScript.ModifyHealthBar(fighter);
        StartCoroutine(StopElixirAnimation(elixir));
    }
}