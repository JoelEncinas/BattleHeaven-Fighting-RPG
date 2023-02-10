using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;
using System;
using UnityEngine.UI;
using TMPro;

public class Combat : MonoBehaviour
{
    // Data Objects
    public static Fighter player;
    public static Fighter bot;
    public static int botElo;

    // GameObjects
    public static GameObject playerGameObject;
    public GameObject playerWrapper;
    public static GameObject botGameObject;
    public GameObject results;
    public SpriteRenderer arena;
    public GameObject combatUI;
    public GameObject combatLoadingScreenUI;
    public GameObject combatLoadingScreenSprites;
    private TextMeshProUGUI levelTextBot;
    public GameObject boostButton;
    public GameObject elixirButton;
    public GameObject floatingHp;

    // tutorial 
    public GameObject tutorialResults;
    public GameObject message1;
    public GameObject message2;
    public Button message1Accept;
    public Button message2Accept;
    public bool isPaused1;
    public bool isPaused2;
    public bool message1Showed;
    public bool message2Showed;
    public int turnCounter;

    // Script references
    public static Movement movementScript;
    public static FightersUIData fightersUIDataScript;
    SkillsLogicInCombat skillsLogicScript;
    Attack attackScript;
    LoadingScreen loadingScreen;
    CupManager cupManager;
    Boost boostScript;
    Elixir elixirScript;

    // Positions data
    static Vector3 playerCombatPosition = new Vector3(-6, -0.7f, 0);
    static Vector3 botCombatPosition = new Vector3(6, -0.7f, 0);
    static Vector3 playerStartingPosition = new Vector3(-20, -0.7f, 0);
    static Vector3 botStartingPosition = new Vector3(20, -0.7f, 0);

    public const float DefaultDistanceFromEachotherOnAttack = 2.3f;
    public static float defaultBloodPositionY;

    // Game status data
    public static bool isGameOver;
    List<Fighter> fightersOrderOfAttack = new List<Fighter> { };
    public static float playerMaxHp;
    public static float botMaxHp;
    public static int floatingTextInstancesCount; //We track the amount of texts displayed so we can sort them on top of eachother.

    //Sorting layers
    public static int fighterSortingOrder;

    // Combat countdown timer
    const float COUNTDOWN_ANIMATION = 2.8f;
    const float ENTER_ARENA_ANIMATION = 2.5f;
    const float TIME_ANNOUNCEMENT = 2.5f;

    private void Awake()
    {
        isGameOver = false;
        FindGameObjects();
        GetComponentReferences();

        // Generate bot data
        if (Cup.Instance.isActive && !CombatMode.isSoloqEnabled) MatchMaking.GenerateCupBotData(player, bot);
        else MatchMaking.GenerateSoloQBotData(player, bot);

        // Set LoadingScreen
        loadingScreen.SetPlayerLoadingScreenData(player);
        loadingScreen.DisplayLoaderForEnemy();
        loadingScreen.HideBotLevelText(levelTextBot);
        EnableBoostElixirBtns(false);
        ShowLoadingScreen(true);

        // Load everything needed for the combat
        BoostFightersStatsBasedOnPassiveSkills();
        SetMaxHpValues();
        SetCombatGameObjectsVisibility();
        SetFighterPositions();
        SetOrderOfAttacks();
        GetRandomArena();
        GetLayersOrder();
        FighterSkin.SetFightersSkin(player, bot);
        FighterAnimations.ResetToDefaultAnimation(player);
        fightersUIDataScript.SetFightersUIInfo(player, bot, botElo);

        // tutorial
        SetUpTutorialFlags();
    }

    private void SetUpTutorialFlags()
    {
        isPaused1 = true;
        isPaused2 = false;
        message1Showed = false;
        message2Showed = false;
        turnCounter = 0;

        // add listeners to buttons
        message1Accept.onClick.AddListener(() => HideMessage1());
        message2Accept.onClick.AddListener(() => HideMessage2());

        // disable messages
        message1.SetActive(false);
        message2.SetActive(false);
    }

    private void HideMessage1()
    {
        isPaused1 = false;
        message1Showed = true;
        message1.SetActive(false);
    }

    private void HideMessage2()
    {
        isPaused2 = false;
        message2Showed = true;
        message2.SetActive(false);
    }

    IEnumerator Start()
    {
        Debug.Log($"Skills count -> Player: {player.skills.Count} / Bot: {bot.skills.Count}");

        FindObjectOfType<AudioManager>().StopAllAndPlay("V_Combat_Loading_Theme");

        StartCoroutine(SceneManagerScript.instance.FadeIn());

        //Comment this line to remove loading screen
        yield return StartCoroutine(LoadingScreenLogic());

        ShowLoadingScreen(false);

        // Enter the arena animation
        StartCoroutine(EnterArenaAnimations());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(ENTER_ARENA_ANIMATION));

        // 3 2 1 combat before initating
        StartCoroutine(fightersUIDataScript.Countdown());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(COUNTDOWN_ANIMATION));
        
        FindObjectOfType<AudioManager>().StopAllAndPlay("V_Combat_Theme");
        //Added delay to sync sound
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.2f));

        StartCoroutine(InitiateCombat());

        SceneFlag.sceneName = SceneNames.Combat.ToString();
    }

    private void GetLayersOrder()
    {
        fighterSortingOrder = botGameObject.GetComponent<Renderer>().sortingOrder;
    }

    IEnumerator LoadingScreenLogic()
    {
        float timeUntilOpponentIsFound = 2.8f;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(timeUntilOpponentIsFound));
        loadingScreen.SetBotLoadingScreenData(bot);
        levelTextBot.enabled = true;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(3f));
    }

    private void BoostFightersStatsBasedOnPassiveSkills()
    {
        skillsLogicScript.BoostStatsBasedOnPassiveSkills(player);
        skillsLogicScript.BoostStatsBasedOnPassiveSkills(bot);
        Debug.Log("PLAYER STATS -> hp: " + player.hp + " damage: " + player.damage + " speed: " + player.speed);
        Debug.Log("BOT STATS -> hp: " + bot.hp + " damage: " + bot.damage + " speed: " + bot.speed);
    }

    private void GetComponentReferences()
    {
        movementScript = GetComponent<Movement>();
        fightersUIDataScript = GetComponent<FightersUIData>();
        skillsLogicScript = GetComponent<SkillsLogicInCombat>();
        attackScript = GetComponent<Attack>();
        loadingScreen = GetComponent<LoadingScreen>();
        player = playerGameObject.GetComponent<Fighter>();
        bot = botGameObject.GetComponent<Fighter>();
        cupManager = GameObject.Find("CupManager").GetComponent<CupManager>();
        boostScript = boostButton.GetComponent<Boost>();
        elixirScript = elixirButton.GetComponent<Elixir>();
    }

    private void ShowLoadingScreen(bool displayLoadingScreen)
    {
        combatLoadingScreenUI.SetActive(displayLoadingScreen);
        combatLoadingScreenSprites.SetActive(displayLoadingScreen);
    }

    private void SetMaxHpValues()
    {
        playerMaxHp = player.hp;
        botMaxHp = bot.hp;
    }
    private void GetRandomArena()
    {
        Sprite[] arenas = Resources.LoadAll<Sprite>("Arenas/");
        int chosenArena = UnityEngine.Random.Range(0, arenas.Length);
        arena.sprite = arenas[chosenArena];
    }

    private void SetCombatGameObjectsVisibility()
    {
        results.SetActive(false);
        tutorialResults.SetActive(false);
        playerGameObject.SetActive(true);
    }
    private void FindGameObjects()
    {
        playerWrapper = GameObject.Find("FighterWrapper");
        playerGameObject = playerWrapper.transform.Find("Fighter").gameObject;
        botGameObject = GameObject.FindGameObjectWithTag("FighterBot");
        results = GameObject.FindGameObjectWithTag("Results");
        tutorialResults = GameObject.FindGameObjectWithTag("TutorialResults");
        arena = GameObject.FindGameObjectWithTag("Arena").GetComponent<SpriteRenderer>();
        combatUI = GameObject.FindGameObjectWithTag("CombatUI");
        combatLoadingScreenUI = GameObject.FindGameObjectWithTag("CombatLoadingScreenUI");
        combatLoadingScreenSprites = GameObject.FindGameObjectWithTag("CombatLoadingScreenSprites");
        levelTextBot = GameObject.Find("LevelTextBot").GetComponent<TextMeshProUGUI>();
        defaultBloodPositionY = GameObject.Find("VFX/Hit_VFX").transform.position.y;
        boostButton = GameObject.Find("Button_Boost");
        elixirButton = GameObject.Find("Button_Elixir");
    }

    private void SetFighterPositions()
    {
        //Set Objects
        player.initialPosition = playerCombatPosition;
        bot.initialPosition = botCombatPosition;
    }

    IEnumerator EnterArenaAnimations()
    {
        //Set GameObjects
        playerGameObject.transform.position = playerStartingPosition;
        botGameObject.transform.position = botStartingPosition;

        FighterAnimations.ChangeAnimation(player, FighterAnimations.AnimationNames.RUN);
        FighterAnimations.ChangeAnimation(bot, FighterAnimations.AnimationNames.RUN);

        StartCoroutine(movementScript.Move(player, playerStartingPosition, playerCombatPosition, ENTER_ARENA_ANIMATION));
        StartCoroutine(movementScript.Move(bot, botStartingPosition, botCombatPosition, ENTER_ARENA_ANIMATION));

        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(ENTER_ARENA_ANIMATION));

        FighterAnimations.ChangeAnimation(player, FighterAnimations.AnimationNames.IDLE);
        FighterAnimations.ChangeAnimation(bot, FighterAnimations.AnimationNames.IDLE);
    }

    public void SetFightersPortrait(GameObject playerPortrait, GameObject botPortrait)
    {
        playerPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterProfilePicture/" + player.species);
        botPortrait.GetComponent<Image>().sprite = Resources.Load<Sprite>("CharacterProfilePicture/" + bot.species);
    }

    private void EnableBoostElixirBtns(bool isEnabled)
    {
        boostButton.SetActive(isEnabled);
        elixirButton.SetActive(isEnabled);
    }
    private void StartBotElixirAndBoostTimer()
    {
        StartCoroutine(StartBotBoostTimer());
        StartCoroutine(StartBotElixirTimer());
    }
    IEnumerator StartBotElixirTimer()
    {
        //Between x and y seconds
        float elixirTimeRange = UnityEngine.Random.Range(15, 25);
        yield return new WaitForSeconds(elixirTimeRange);
        if (!isGameOver) elixirScript.TriggerElixirEffects(bot);
    }
    IEnumerator StartBotBoostTimer()
    {
        float boostTimeRange = UnityEngine.Random.Range(1, 20);
        yield return new WaitForSeconds(boostTimeRange);
        if (!isGameOver) boostScript.TriggerBoostEffects(bot);
    }

    IEnumerator InitiateCombat()
    {
        if (!User.Instance.firstTime)
        {
            StartBotElixirAndBoostTimer();
            EnableBoostElixirBtns(true);
        }

        Fighter firstAttacker = fightersOrderOfAttack[0];
        Fighter secondAttacker = fightersOrderOfAttack[1];

        //1 loop = 1 turn (both players attacking)
        while (!isGameOver)
        {
            // The StartTurn method should handle all the actions of a player for that turn. E.G. Move, Attack, Throw skill....
            yield return StartCoroutine(StartTurn(firstAttacker, secondAttacker));
            while (!isGameOver && attackScript.IsExtraTurn(firstAttacker)) yield return StartCoroutine(StartTurn(firstAttacker, secondAttacker));
            if (isGameOver) break;
            yield return StartCoroutine(StartTurn(secondAttacker, firstAttacker));
            while (!isGameOver && attackScript.IsExtraTurn(secondAttacker)) yield return StartCoroutine(StartTurn(secondAttacker, firstAttacker));
        }

        StartCoroutine(StartPostGameActions());
    }

    //ONLY FOR TESTING PURPOSES
    private void GenerateSkillsFixtures(Fighter fighter)
    {
        //GIVE ALL SKILLS TO THE PLAYER FOR THE COMBAT
        foreach (OrderedDictionary skill in SkillCollection.skills)
        {
            Skill skillInstance = new Skill(skill["name"].ToString(), skill["description"].ToString(),
                skill["skillRarity"].ToString(), skill["category"].ToString(), skill["icon"].ToString());

            fighter.skills.Add(skillInstance);
        }
    }

    IEnumerator StartTurn(Fighter attacker, Fighter defender)
    {
        if (User.Instance.firstTime)
        {
            if (turnCounter >= 5 && !message2Showed)
                isPaused2 = true;

            while (isPaused1)
            {
                yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.2f));

                if (!message1Showed)
                    message1.SetActive(true);
            }

            while (isPaused2)
            {
                yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(0.2f));

                if (!message2Showed)
                {
                    message2.SetActive(true);
                    EnableBoostElixirBtns(true);
                }    
            }
        }

        if (WillUseSkillThisTurn(attacker))
        {
            yield return StartCoroutine(UseSkill(attacker, defender, attacker));
            yield break;
        }
        yield return skillsLogicScript.AttackWithoutSkills(attacker, defender);
        FighterAnimations.ChangeAnimation(GetAttackerIfAlive(attacker, defender), FighterAnimations.AnimationNames.IDLE);

        turnCounter++;
    }

    // This check is important as the attacker might have lost due to a counter or reversal attack
    private Fighter GetAttackerIfAlive(Fighter attacker, Fighter defender)
    {
        return attacker.hp > 0 ? attacker : defender;
    }

    //We create the fighterWeTakeTheSkillFrom param for the ViciousTheft skill as we take a skill from the opponent instead.
    IEnumerator UseSkill(Fighter attacker, Fighter defender, Fighter fighterWeTakeTheSkillFrom)
    {
        //TODO v2: Each skill should have each own class with its own skill implementation. (methods, attributes, etc...)
        // Then we can instantiate a random class here to use a random SUPER skill this turn

        List<string> skillNamesList = fighterWeTakeTheSkillFrom.skills.
            Where(skill => skill.category == SkillCollection.SkillType.SUPER.ToString()).
            Select(skill => skill.skillName).ToList();

        bool isStolenSkill = defender == fighterWeTakeTheSkillFrom;

        // Can be 0 in some edge cases e.g. when ViciousTheft is used and the opponent does not have skills left.
        if (skillNamesList.Count() > 0)
        {
            string skillToCastThisTurn = GetSkillToCastThisTurn(attacker, skillNamesList);

            //Start ViciousTheft animation
            if (isStolenSkill & skillToCastThisTurn != SkillNames.ViciousTheft)
                yield return StartCoroutine(skillsLogicScript.ViciousTheft(attacker, skillToCastThisTurn));

            switch (skillToCastThisTurn)
            {
                case SkillNames.JumpStrike:
                    yield return skillsLogicScript.JumpStrike(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.JumpStrike);
                    break;
                case SkillNames.CosmicKicks:
                    yield return skillsLogicScript.CosmicKicks(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.CosmicKicks);
                    break;
                case SkillNames.ShurikenFury:
                    yield return skillsLogicScript.ShurikenFury(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.ShurikenFury);
                    break;
                case SkillNames.LowBlow:
                    yield return skillsLogicScript.LowBlow(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.LowBlow);
                    break;
                case SkillNames.ExplosiveBomb:
                    yield return skillsLogicScript.ExplosiveBomb(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.ExplosiveBomb);
                    break;
                case SkillNames.ShadowTravel:
                    yield return skillsLogicScript.ShadowTravel(attacker, defender);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.ShadowTravel);
                    break;
                case SkillNames.HealingPotion:
                    yield return skillsLogicScript.HealingPotion(attacker);
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.HealingPotion);
                    break;
                case SkillNames.ViciousTheft:
                    fighterWeTakeTheSkillFrom.removeUsedSkill(SkillNames.ViciousTheft);
                    yield return UseSkill(attacker, defender, defender);
                    break;
            }
        }
        FighterAnimations.ChangeAnimation(GetAttackerIfAlive(attacker, defender), FighterAnimations.AnimationNames.IDLE);
    }

    private string GetSkillToCastThisTurn(Fighter attacker, List<string> skillNamesList)
    {
        string GetRandomSkillName()
        {
            int randomSkillIndex = UnityEngine.Random.Range(0, skillNamesList.Count());
            return skillNamesList[randomSkillIndex];
        }

        string skillToCastThisTurn = GetRandomSkillName();
        float attackerMaxHp = attacker == player ? playerMaxHp : botMaxHp;

        while (attacker.hp == attackerMaxHp && skillToCastThisTurn == SkillNames.HealingPotion) skillToCastThisTurn = GetRandomSkillName();

        return skillToCastThisTurn;
    }

    public static Func<Fighter, bool> WillUseSkillThisTurn = attacker =>
        attacker.skills.Count() > 0 && Probabilities.IsHappening(GlobalConstants.ProbabilityOfUsingSkillEachTurn);

    public IEnumerator MoveForwardHandler(Fighter attacker, Fighter defender, float distanceFromEachOtherOnAttack = DefaultDistanceFromEachotherOnAttack)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.RUN);
        yield return StartCoroutine(movementScript.MoveForward(attacker, GetAttackerDestinationPosition(defender, distanceFromEachOtherOnAttack)));
    }

    public static Vector3 GetAttackerDestinationPosition(Fighter defender, float distanceFromEachOtherOnAttack)
    {
        Vector3 attackerDestinationPosition = defender.transform.position;
        attackerDestinationPosition.x = attackerDestinationPosition.x + (player == defender ? +distanceFromEachOtherOnAttack : -distanceFromEachOtherOnAttack);
        return attackerDestinationPosition;
    }

    public IEnumerator MoveBackHandler(Fighter attacker)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.RUN);
        FighterSkin.SwitchFighterOrientation(attacker.GetComponent<SpriteRenderer>());
        yield return StartCoroutine(movementScript.MoveBack(attacker, attacker.initialPosition));
        FighterSkin.SwitchFighterOrientation(attacker.GetComponent<SpriteRenderer>());
    }

    //The attack order is determined by the Initiator skill. If no players have it it is determined by the speed.
    private void SetOrderOfAttacks()
    {
        if (player.HasSkill(SkillNames.Initiator))
        {
            fightersOrderOfAttack.Add(player);
            fightersOrderOfAttack.Add(bot);
            return;
        }

        // Creates a dictionary with the Fighter class objects sorted by their speeds to get the order of attack.
        // Higher speeds will get sorted first
        OrderedDictionary fighterDictWithSpeed = new OrderedDictionary
        {
            {player, player.speed},
            {bot, bot.speed},
        };

        var fighterDictSortedBySpeed = fighterDictWithSpeed.Cast<DictionaryEntry>()
                       .OrderByDescending(r => r.Value)
                       .ToDictionary(c => c.Key, d => d.Value);

        foreach (var fighter in fighterDictSortedBySpeed)
        {
            fightersOrderOfAttack.Add((Fighter)fighter.Key);
        }
    }

    private IEnumerator StartPostGameActions()
    {
        EnableBoostElixirBtns(false);

        if (User.Instance.firstTime)
        {
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(3f));
            fightersUIDataScript.ShowPostTutorialInfo(tutorialResults);
        }
        else
        {
            bool isPlayerWinner = PostGameActions.HasPlayerWon(player);
            int eloChange = MatchMaking.CalculateEloChange(User.Instance.elo, botElo, isPlayerWinner);
            int playerUpdatedExperience = player.experiencePoints + Levels.GetXpGain(isPlayerWinner);
            bool isLevelUp = Levels.IsLevelUp(player.level, playerUpdatedExperience);
            int goldReward = PostGameActions.GoldReward(isPlayerWinner);
            int gemsReward = PostGameActions.GemsReward();

            //Reset fighter values that were modified in combat e.g. hp
            ResetPlayerObject();

            //PlayerData
            PostGameActions.SetElo(eloChange);
            PostGameActions.SetWinLoseCounter(isPlayerWinner);
            PostGameActions.SetExperience(player, isPlayerWinner);
            if (isLevelUp) PostGameActions.SetLevelUpSideEffects(player);
            if (CombatMode.isSoloqEnabled) EnergyManager.SubtractOneEnergyPoint(); // tournament doesn't cost energy

            //Rewards
            PostGameActions.SetCurrencies(goldReward, gemsReward);

            // Show winner
            StartCoroutine(fightersUIDataScript.AnnounceWinner(isPlayerWinner, player, bot));
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_ANNOUNCEMENT));

            //UI
            fightersUIDataScript.ShowPostCombatInfo(player, isPlayerWinner, eloChange, isLevelUp, goldReward, gemsReward, results);

            //Save
            PostGameActions.Save(player);

            //Profile
            ProfileData.SavePeakElo(User.Instance.elo);

            if (Cup.Instance.isActive && !CombatMode.isSoloqEnabled)
            {
                switch (Cup.Instance.round)
                {
                    case "QUARTERS":
                        cupManager.SimulateQuarters(isPlayerWinner);
                        break;
                    case "SEMIS":
                        cupManager.SimulateSemis(isPlayerWinner);
                        break;
                    case "FINALS":
                        cupManager.SimulateFinals(isPlayerWinner);
                        break;
                }

                if (!isPlayerWinner)
                {
                    // enable rewards button on cup menu
                    // disable battle button
                    Cup.Instance.isActive = true;
                    Cup.Instance.playerStatus = false;
                    Cup.Instance.SaveCup();
                }
            }
        }

    }

    //During the combat the player object experiences a lot of changes so we need to set it back to its default state after the combat.
    private Action ResetPlayerObject = () => player = JsonDataManager.ReadFighterFile();
}
