using UnityEngine;

public class VFXUtils : MonoBehaviour
{
    public static Color GetUsedButtonColor(Color color)
    {
        color.a = .4f;
        return color;
    }

    public static void DisplayFloatingHp(Fighter defender, GameObject floatingHp, float hpChange, Color? color = null)
    {
        GameObject floatingHpInstance = CreateFloatingTextInstance(defender, floatingHp);
        floatingHpInstance.GetComponent<FloatingHp>().StartHpAnimation(hpChange, color);
    }

    public static void DisplayFloatingText(Fighter defender, GameObject floatingHp, string text)
    {
        GameObject floatingHpInstance = CreateFloatingTextInstance(defender, floatingHp);
        floatingHpInstance.GetComponent<FloatingHp>().StartTextAnimation(text);
    }

    private static GameObject CreateFloatingTextInstance(Fighter defender, GameObject floatingText){
        Vector3 floatingTextPosition = defender.transform.position;
        floatingTextPosition.x = defender == Combat.player ? floatingTextPosition.x - 0.8f : floatingTextPosition.x + 0.8f;
        return Instantiate(floatingText, floatingTextPosition, Quaternion.identity, defender.transform);
    }
}