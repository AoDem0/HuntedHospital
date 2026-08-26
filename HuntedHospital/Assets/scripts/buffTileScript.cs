using UnityEngine;
using TMPro;

public class buffTileScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffName;
    [SerializeField] protected TextMeshProUGUI turnsLeft;

    public void SetValues(BuffsSO buff)
    {
        buffName.text = buff.buffName;
        turnsLeft.text = @$"Zostało: {buff.buffCurrentTime} rund";
    }
}
