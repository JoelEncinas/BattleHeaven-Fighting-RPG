using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class CreditsManager : MonoBehaviour
{
    // UI
    GameObject buttonCloseCredits;
    TextMeshProUGUI title;
    TextMeshProUGUI dev1;
    TextMeshProUGUI dev2;
    TextMeshProUGUI dev3;
    TextMeshProUGUI dev4;
    TextMeshProUGUI thanks;
    TextMeshProUGUI copy;

    // animations
    Animator titleAnimator;
    Animator dev1Animator;
    Animator dev2Animator;
    Animator dev3Animator;
    Animator dev4Animator;
    Animator thanksAnimator;
    Animator copyAnimator;

    // fighter s
    Vector3 fighterStartingPosition = new Vector3(-6, -0.7f, 0);
    AnimationClip idleAnimation;
    AnimationClip runAnimation;
    AnimationClip blinkAnimation;
    AnimationClip slideAnimation;
    RectTransform fighter1;
    Animator fighterAnimator1;
    RectTransform fighter2;
    Animator fighterAnimator2;
    RectTransform fighter3;
    Animator fighterAnimator3;
    RectTransform fighter4;
    Animator fighterAnimator4;

    private void Awake()
    {
        SetupUI();
        SetupButtons();
        SetupFighters();
    }

    IEnumerator Start()
    {
        StartCoroutine(SceneManagerScript.instance.FadeIn());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));

        SceneFlag.sceneName = SceneNames.Credits.ToString();

        IStartAnimation();
    }

    private void SetupUI()
    {
        buttonCloseCredits = GameObject.Find("Button_Close_Credits");
        title = GameObject.Find("Title").GetComponent<TextMeshProUGUI>();
        dev1 = GameObject.Find("Dev1").GetComponent<TextMeshProUGUI>();
        dev2 = GameObject.Find("Dev2").GetComponent<TextMeshProUGUI>();
        dev3 = GameObject.Find("Dev3").GetComponent<TextMeshProUGUI>();
        dev4 = GameObject.Find("Dev4").GetComponent<TextMeshProUGUI>();
        copy = GameObject.Find("Copy").GetComponent<TextMeshProUGUI>();
        thanks = GameObject.Find("Thanks").GetComponent<TextMeshProUGUI>();

        titleAnimator = title.gameObject.GetComponent<Animator>();
        dev1Animator = dev1.gameObject.GetComponent<Animator>();
        dev2Animator = dev2.gameObject.GetComponent<Animator>();
        dev3Animator = dev3.gameObject.GetComponent<Animator>();
        dev4Animator = dev4.gameObject.GetComponent<Animator>();
        thanksAnimator = thanks.gameObject.GetComponent<Animator>();
        copyAnimator = copy.gameObject.GetComponent<Animator>();

        fighter1 = GameObject.Find("Fighter1").GetComponent<RectTransform>();
        fighterAnimator1 = GameObject.Find("Fighter1").GetComponent<Animator>();
        fighter2 = GameObject.Find("Fighter2").GetComponent<RectTransform>();
        fighterAnimator2 = GameObject.Find("Fighter2").GetComponent<Animator>();
        fighter3 = GameObject.Find("Fighter3").GetComponent<RectTransform>();
        fighterAnimator3 = GameObject.Find("Fighter3").GetComponent<Animator>();
        fighter4 = GameObject.Find("Fighter4").GetComponent<RectTransform>();
        fighterAnimator4 = GameObject.Find("Fighter4").GetComponent<Animator>();

        title.enabled = false;
        dev1.enabled = false;
        dev2.enabled = false;
        dev3.enabled = false;
        dev4.enabled = false;
        copy.enabled = false;
        thanks.enabled = false;

        fighterAnimator1.enabled = false;
        fighterAnimator2.enabled = false;
        fighterAnimator3.enabled = false;
        fighterAnimator4.enabled = false;
        titleAnimator.enabled = false;
        dev1Animator.enabled = false;
        dev2Animator.enabled = false;
        dev3Animator.enabled = false;
        dev4Animator.enabled = false;
        thanks.enabled = false;
        copyAnimator.enabled = false;

        idleAnimation = Resources.Load<AnimationClip>("Animations/Characters/" + SpeciesNames.FallenAngel1.ToString() + "/01_idle");
        runAnimation = Resources.Load<AnimationClip>("Animations/Characters/" + SpeciesNames.Orc.ToString() + "/02_run");
        blinkAnimation = Resources.Load<AnimationClip>("Animations/Characters/" + SpeciesNames.Golem3.ToString() + "/11_idle_blinking");
        slideAnimation = Resources.Load<AnimationClip>("Animations/Characters/" + SpeciesNames.Goblin.ToString() + "/09_slide");
        SetAnimationClipToAnimator(fighterAnimator1, idleAnimation);
        SetAnimationClipToAnimator(fighterAnimator2, runAnimation);
        SetAnimationClipToAnimator(fighterAnimator3, blinkAnimation);
        SetAnimationClipToAnimator(fighterAnimator4, slideAnimation);
    }

    private void SetupButtons()
    {
        buttonCloseCredits.GetComponent<Button>().onClick.AddListener(() => IHideCreditsPopup());
    }

    private void SetupFighters()
    {
        fighter1.position = fighterStartingPosition;
        fighter2.position = fighterStartingPosition;
        fighter3.position = fighterStartingPosition;
        fighter4.position = fighterStartingPosition;
    }

    private void IStartAnimation()
    {
        StartCoroutine(StartAnimation());
    }

    public IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));

        titleAnimator.enabled = true;
        copyAnimator.enabled = true;
        title.enabled = true;
        copy.enabled = true;

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));

        fighterAnimator1.enabled = true;
        dev1Animator.enabled = true;
        dev1.enabled = true;


        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(5f));

        fighterAnimator2.enabled = true;
        dev2Animator.enabled = true;
        dev2.enabled = true;

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(5f));

        fighterAnimator3.enabled = true;
        dev3Animator.enabled = true;
        dev3.enabled = true;

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(5f));

        fighterAnimator4.enabled = true;
        dev4Animator.enabled = true;
        dev4.enabled = true;

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(5f));

        thanksAnimator.enabled = true;
        thanks.enabled = true;
    }

    public void IHideCreditsPopup()
    {
        StartCoroutine(HideCreditsPopup());
    }

    private IEnumerator HideCreditsPopup()
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        SceneManager.LoadScene(SceneNames.MainMenu.ToString());
    }

    private void SetAnimationClipToAnimator(Animator animator, AnimationClip animation)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        AnimationClip clip = aoc.animationClips[0];

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(clip, animation));
        aoc.ApplyOverrides(anims);
        animator.runtimeAnimatorController = aoc;
    }
}
