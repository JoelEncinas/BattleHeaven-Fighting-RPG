using UnityEngine;

public class Spark : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Transform spark = fighter.transform.Find("VFX/Spark_VFX");
        spark.GetComponent<SpriteRenderer>().enabled = true;
        spark.GetComponent<Animator>().Play("spark_0", -1, 0f);
    }
    public static void StopAnimation(Fighter fighter)
    {
        fighter.transform.Find("VFX/Spark_VFX").GetComponent<SpriteRenderer>().enabled = false;
    }
}