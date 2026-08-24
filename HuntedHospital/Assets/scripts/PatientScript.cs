using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PatientScript : MonoBehaviour
{
    public float bloodAmmount;
    float maxBloodAmmount = 6;
    float minBloodAmmount = 4;
    Collider2D col;
    SpriteRenderer spriteRenderer;
    List<Sprite> sprites  = new List<Sprite>();

    void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer  != null)
        {
            //spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
        bloodAmmount = GetRandomBloodAmmount();
    }

    private float GetRandomBloodAmmount()
    {
        return Random.Range(minBloodAmmount, maxBloodAmmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (col.CompareTag("HospitalEntrance"))
        {
            RoundController.Instance.PatientEnteredHospital(this);

        }
    }

}
