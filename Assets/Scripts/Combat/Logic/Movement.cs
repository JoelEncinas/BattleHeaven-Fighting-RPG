using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float runningDurationInSeconds = GeneralUtils.GetRealOrSimulationTime(0.6f);
    public double dodgeDurationInSeconds = GeneralUtils.GetRealOrSimulationTime(0.15f);

    //FIXME v2: This value is not correct so the position of the players after dodging multiple times could not be correct.
    // Is it possible to get this value automatically from the canvas?
    float screenEdgeX = 7;

    public IEnumerator MoveForward(Fighter fighter, Vector3 target)
    {
        yield return StartCoroutine(Move(fighter, fighter.transform.position, target, runningDurationInSeconds));
    }

    public IEnumerator MoveBack(Fighter fighter, Vector3 target)
    {
        yield return StartCoroutine(Move(fighter, fighter.transform.position, target, runningDurationInSeconds));
    }


    public IEnumerator Move(Fighter fighter, Vector3 startingPosition, Vector3 targetPosition, double duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            fighter.transform.position = Vector3.Lerp(startingPosition, targetPosition, (float)(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    public IEnumerator MoveShuriken(GameObject shuriken, Vector3 startingPosition, Vector3 targetPosition, double duration)
    {
        float elapsedTime = 0;

        duration = GeneralUtils.GetRealOrSimulationTime((float)duration);

        while (elapsedTime < duration)
        {
            shuriken.transform.position = Vector3.Lerp(startingPosition, targetPosition, (float)(elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //FIXME v2: Is there a way to reuse the "Move" functions that belong to each attack? Or do we need to do very specific things on each one?
    // Its not easy to reuse MoveShuriken as its complicated to have a function that accepts a param of different types (Fighter and Gameobject)
    public IEnumerator MoveSlide(Fighter fighter, Vector3 target)
    {
        float elapsedTime = 0;
        bool slideAnimationsOn = false;

        while (elapsedTime < runningDurationInSeconds)
        {
            if (elapsedTime >= runningDurationInSeconds / 2)
            {
                if (!slideAnimationsOn)
                {
                    Spark.StartAnimation(fighter);
                    FighterAnimations.ChangeAnimation(fighter, FighterAnimations.AnimationNames.SLIDE);
                }
                slideAnimationsOn = true;
            }
            fighter.transform.position = Vector3.Lerp(fighter.initialPosition, target, (float)(elapsedTime / runningDurationInSeconds));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Spark.StopAnimation(fighter);
    }

    public IEnumerator MoveJumpStrike(Fighter fighter, Vector3 target)
    {
        float elapsedTime = 0;

        Vector3 destinationPosition = target;

        while (elapsedTime < runningDurationInSeconds)
        {
            if (elapsedTime >= runningDurationInSeconds / 1.5)
            {
                FighterAnimations.ChangeAnimation(fighter, FighterAnimations.AnimationNames.JUMP);
                destinationPosition.y += .03f;
            }
            fighter.transform.position = Vector3.Lerp(fighter.initialPosition, destinationPosition, (float)(elapsedTime / runningDurationInSeconds));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator RotateObjectOverTime(GameObject gameObjectToMove, Vector3 eulerAngles, float duration)
    {
        Vector3 newRot = gameObjectToMove.transform.eulerAngles + eulerAngles;

        Vector3 currentRot = gameObjectToMove.transform.eulerAngles;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObjectToMove.transform.eulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }

    public IEnumerator DodgeMovement(Fighter defender)
    {
        //This initial position might be at the back if we are defending or at the front if we are attacking and the fighter got hit by a counter or reversal attack
        Vector2 initialPosition = defender.transform.position;

        Vector2 defenderInitialPosition = initialPosition;
        Vector2 defenderMaxHeightInAirPosition = initialPosition;
        Vector2 defenderLandingPosition = initialPosition;

        const int JumpHeight = 2;

        bool isPlayerDodging = Combat.player == defender;
        int xDistanceOnJump = isPlayerDodging ? -1 : 1;
        int xDistanceOnLand = GetBackwardMovement(isPlayerDodging);

        if (!IsFighterInTheEdgeOfScreen(isPlayerDodging, defender.transform.position.x))
        {
            defenderMaxHeightInAirPosition.x += xDistanceOnJump;
            defenderLandingPosition.x += xDistanceOnLand;
        }
        defenderMaxHeightInAirPosition.y += JumpHeight;

        //Animation to max height
        yield return StartCoroutine(Move(defender, defenderInitialPosition, defenderMaxHeightInAirPosition, dodgeDurationInSeconds));
        //Animation from max height to landing
        yield return StartCoroutine(Move(defender, defenderMaxHeightInAirPosition, defenderLandingPosition, dodgeDurationInSeconds));
    }

    private bool IsFighterInTheEdgeOfScreen(bool isPlayerDodging, float defenderXPosition)
    {
        return isPlayerDodging && defenderXPosition <= -screenEdgeX || !isPlayerDodging && defenderXPosition >= screenEdgeX;
    }

    private bool IsAtMeleeRange()
    {
        double currentDistanceAwayFromEachOther = GeneralUtils.ToSingleDecimal(Combat.player.transform.position.x - Combat.bot.transform.position.x);
        return System.Math.Abs(currentDistanceAwayFromEachOther) <= Combat.DefaultDistanceFromEachotherOnAttack;
    }

    private bool HasSpaceToKeepPushing(bool isPlayerAttacking, float attackerXPosition)
    {
        return isPlayerAttacking && attackerXPosition <= screenEdgeX - Combat.DefaultDistanceFromEachotherOnAttack || !isPlayerAttacking && attackerXPosition >= -screenEdgeX + Combat.DefaultDistanceFromEachotherOnAttack;
    }

    public bool FighterShouldAdvanceToAttack(Fighter attacker)
    {
        return !IsAtMeleeRange() && HasSpaceToKeepPushing(Combat.player == attacker, attacker.transform.position.x);
    }

    private int GetBackwardMovement(bool isPlayerDodging)
    {
        return isPlayerDodging ? -2 : 2;
    }

    public IEnumerator MoveToMeleeRangeAgain(Fighter attacker, Fighter defender)
    {
        Vector2 newDestinationPosition = attacker.transform.position;
        newDestinationPosition.x += GetBackwardMovement(Combat.player == defender);

        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.RUN);
        yield return StartCoroutine(Move(attacker, attacker.transform.position, newDestinationPosition, runningDurationInSeconds * 0.2f));
    }

    public void Rotate(Fighter fighter, float rotationDegrees)
    {
        fighter.transform.Rotate(0f, 0f, rotationDegrees, 0f);
    }
    public void ResetRotation(Fighter fighter)
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        fighter.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }
}