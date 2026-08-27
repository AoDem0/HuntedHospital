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
    Rigidbody2D rb;
    public SpritePacjentów patientSpritesLeft;
    public SpritePacjentów patientSpritesRight;
    public Vector3 HospitalDoors;
    public float moveSpeed = 3f;
    public bool movedToWaitingRoom = false;
    public PatientCharacteristicsSO currentChar = null;
    public PatientCharacteristicsSO averageJoeChar;
    public List<PatientCharacteristicsSO> characteristicList;
    public bool canGoToExit = false;
    public enum PatientSpawnSide
    {
        Left,
        Right
    }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer  != null)
        {
            //spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
        }
        bloodAmmount = GetRandomBloodAmmount();

        if (currentChar == null)
        {
            currentChar = GetRandomChar();
        }
        currentChar.ApplyCharacteristics(this);

    }

    private void Update()
    {
        if (!movedToWaitingRoom)
        { 
            MoveToTarget(HospitalDoors); 
        }
    }

    public void MoveToTarget(Vector3 target)
    {
        Vector2 direction = new Vector2(target.x - transform.position.x, 0f);
        rb.linearVelocity = direction.normalized * moveSpeed;
    }

    private float GetRandomBloodAmmount()
    {
        return Random.Range(minBloodAmmount, maxBloodAmmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HospitalDoor"))
        {
            RoundController.Instance.PatientEnteredHospital(this);
            Debug.Log($"Patient entered the hospital with {bloodAmmount} blood.");
        }
    }

    public void SetSpriteForSide(PatientSpawnSide side)
    {
        if (side == PatientSpawnSide.Left)
        {
            spriteRenderer.sprite = patientSpritesRight.SpriteList[Random.Range(0, patientSpritesLeft.SpriteList.Count)];
        }
        else if (side == PatientSpawnSide.Right)
        {
            spriteRenderer.sprite = patientSpritesLeft.SpriteList[Random.Range(0, patientSpritesRight.SpriteList.Count)];
        }
    }

    private PatientCharacteristicsSO GetRandomChar()
    {
        PatientCharacteristicsSO charToGive;
        var rollo = Random.Range(0, 2);
        if (rollo == 2)
        {
            var RandomCharIndex = Random.Range(0, characteristicList.Count);
            charToGive = characteristicList[RandomCharIndex];
        }
        else
        {
            charToGive = averageJoeChar;
        }
        return charToGive;
    }

}
