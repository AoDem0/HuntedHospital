using UnityEngine;

public class ColorForTierScript : MonoBehaviour
{
    private static Color Tier1 = new Color(0.2830189f, 0.1989142f, 0.1989142f);
    private static Color Tier2 = new Color(0.6886792f, 0.4589126f, 0.2436365f);
    private static Color Tier3 = new Color(0f, 0.7242807f, 0.8113208f);

    public Color GetColorForTier(BuffsSO buff)
    {
        Color ColorToReturn;

        switch (buff.buffTier)
        {
            case 1:
                ColorToReturn = Tier1;
                break;
            case 2:
                ColorToReturn = Tier2;
                break;
            case 3:
                ColorToReturn = Tier3;
                break;
            default:
                ColorToReturn = Color.white;
                break;
        }

        return ColorToReturn;
    }
}
