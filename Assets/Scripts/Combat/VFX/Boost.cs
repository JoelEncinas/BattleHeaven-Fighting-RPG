using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Boost : MonoBehaviour
{
    float boostDuration = 5f;
    bool isPlayerBoostActive;
    bool isBotBoostActive;

    //Entrypoint for user
    public void OnClickTriggerBoostEffects()
    {        
        FindObjectOfType<AudioManager>().Play("S_Boost_Used");
        Image iconImage = this.transform.Find("Icon").GetComponent<Image>();
        iconImage.color = VFXUtils.GetUsedButtonColor(iconImage.color);
        this.GetComponent<Button>().interactable = false;
        TriggerBoostEffects(Combat.player);
    }

    private void StartParticlesAnimation(Fighter fighter)
    {
        fighter.transform.Find("VFX/Boost_VFX/Particles_VFX").GetComponent<ParticleSystem>().Play();
    }

    //Entrypoint for bot
    public void TriggerBoostEffects(Fighter fighter)
    {
        SetIsBoostActiveValue(fighter, true);
        ToggleDamageMultiplier(fighter);
        Animator lightningAnimator = fighter.transform.Find("VFX/Boost_VFX/Lightning_VFX").GetComponent<Animator>();
        lightningAnimator.Play("lightning_0", -1, 0f);
        fighter.damage *= GlobalConstants.SkillDamages.Boost;
        fighter.GetComponent<Renderer>().material.color = new Color32(255, 192, 0, 255);
        StartCoroutine(StartBoostTimer(fighter));
        StartCoroutine(ShowParticlesWhileBoostLast(fighter));
    }

    private void SetIsBoostActiveValue(Fighter fighter, bool isBoostActive)
    {
        if (Combat.player == fighter) isPlayerBoostActive = isBoostActive;
        else isBotBoostActive = isBoostActive;
    }

    private void ToggleDamageMultiplier(Fighter fighter){
        if(fighter == Combat.player) {
            var damageBoostText = GameObject.Find("Button_Boost").transform.Find("Text").GetComponent<TextMeshProUGUI>();
            damageBoostText.enabled = !damageBoostText.enabled; 
        }
    }


    IEnumerator StartBoostTimer(Fighter fighter)
    {
        yield return new WaitForSeconds(boostDuration);
        SetIsBoostActiveValue(fighter, false);
    }

    IEnumerator ShowParticlesWhileBoostLast(Fighter fighter)
    {
        while (isBoostActive(fighter))
        {
            StartParticlesAnimation(fighter);
            yield return new WaitForSeconds(0.05f);
        }

        StartCoroutine(RemoveBoostEffects(fighter));
    }

    IEnumerator RemoveBoostEffects(Fighter fighter)
    {
        ToggleDamageMultiplier(fighter);
        fighter.damage /= GlobalConstants.SkillDamages.Boost;
        fighter.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
        //Wait for particles animation to finish
        yield return new WaitForSeconds(1f);
    }

    private bool isBoostActive(Fighter fighter)
    {
        return Combat.player == fighter ? isPlayerBoostActive : isBotBoostActive;
    }
}