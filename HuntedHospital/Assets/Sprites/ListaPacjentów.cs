using UnityEngine;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Sprite_Pacjentów", menuName = "ScriptableObjects/SpritePacjentów")]
public class SpritePacjentów : ScriptableObject
{
    public enum direction
    {
        Left,
        Right
    }

    public direction Direction;
    public List<Sprite> SpriteList = new List<Sprite>();
}
