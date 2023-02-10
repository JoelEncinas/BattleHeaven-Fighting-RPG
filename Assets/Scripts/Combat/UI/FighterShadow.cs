using UnityEngine;

public class FighterShadow : MonoBehaviour
{
    Transform player;
    Transform bot;
    string parentFighterName;
    //TODO V2: We dont need this script at all. Making the shadow a child of the fighter is enough.
    void Start()
    {
        parentFighterName = transform.parent.parent.name;
        player = PlayerUtils.FindInactiveFighter().transform;
        bot = GameObject.Find("Bot").transform;
    }

    void Update()
    {
        if (parentFighterName == "Fighter") setShadowPosition(player);
        else setShadowPosition(bot);
    }

    private void setShadowPosition(Transform fighterTransform, float shadowDisplacement = 0)
    {
        // here we force the position of the current shadow to have the same X as the fighter (+ - a little displacement to make it look realistic)
        transform.position = new Vector3(fighterTransform.position.x + shadowDisplacement, transform.position.y, transform.position.z);
    }
}
