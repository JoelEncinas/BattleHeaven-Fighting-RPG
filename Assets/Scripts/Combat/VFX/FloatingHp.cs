using UnityEngine;
using TMPro;
using System.Collections;
using System;

//TODO V2: The prefab, the file and the script should probably be called floating text as it not only shows hp.
public class FloatingHp : MonoBehaviour
{
    TextMeshPro floatingText;
    Animator floatingTextTravelAnimator;

    private void Awake()
    {
        //this = FloatingHp script
        floatingText = this.gameObject.GetComponent<TextMeshPro>();
        floatingTextTravelAnimator = this.gameObject.GetComponent<Animator>();

        //Make each floating text appear in front of the previous one
        int otherPrefabsCount = GameObject.FindGameObjectsWithTag("FloatingHp").Length;
        floatingText.sortingOrder = Combat.floatingTextInstancesCount;

        Combat.floatingTextInstancesCount++;
    }
    public void StartHpAnimation(float hpChange, Color? color = null)
    {
        double hpChangeRounded = Math.Round(hpChange);
        floatingText.text = color == GlobalConstants.healColor ? $"+{hpChangeRounded.ToString()}" : hpChangeRounded.ToString();
        floatingText.color = color ?? GlobalConstants.noColor;
        floatingTextTravelAnimator.Play("floating_text", -1, 0f);
        StartCoroutine(DestroyFloatingText());
    }

    public void StartTextAnimation(string text)
    {
        floatingText.fontSize = 7;
        floatingText.text = text;
        floatingTextTravelAnimator.Play("floating_text", -1, 0f);
        StartCoroutine(DestroyFloatingText());
    }

    IEnumerator DestroyFloatingText()
    {
        yield return new WaitForSeconds(.6f);
        Destroy(this.gameObject);
    }
}