using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB;
    private BoxCollider2D BC2D;

    [Header("Player Movement Variables")]
    [SerializeField] public LayerMask GroundLayer;
    [SerializeField] private float MovementSpeed = 4f;
    [SerializeField][Range(0f, 1f)] private float AirControlAmount = 0.5f;
    [SerializeField] private float Jumpforce = 7f;
    [SerializeField] private float GroundedDrag = 5f;
    [SerializeField] private KeyCode InteractKey = KeyCode.E;
    private bool IsGrounded;
    private Vector2 PLayerInput;
    private RaycastHit CurrentFloor;

    [Header("Player Ability Variables")]
    [SerializeField] private KeyCode AbilityActivateKey = KeyCode.Mouse0;
    [SerializeField] private float AbilityDuration = 10f;
    private bool AbilityActive;
    private int ArrayRecorder = 0;
    private float AbilityCounter;
    private Vector2 PlayerStartLocation;
    private Transform[] PlayerLocations;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        BC2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        DetectAbility();
    }

    #region PlayerMovement
    private void MovePlayer()
    {
        //calculate if player is grounded
        IsGrounded = Physics2D.Raycast(RB.position, Vector3.down, 0.6f, GroundLayer);

        //process player input into a usable vector2
        Vector3 RawInput = (transform.right * Input.GetAxisRaw("Horizontal")) + (transform.up * Input.GetAxisRaw("Vertical"));
        PLayerInput = new Vector2(RawInput.x, RawInput.z);

        if (IsGrounded)
        {
            //Apply Friction to the play to make them stop
            RB.linearDamping = GroundedDrag;
            //apply force to make player move
            RB.AddForce(PLayerInput * MovementSpeed * (GroundedDrag * 2), ForceMode2D.Force);
            //Check If player is pressing the jump key
            if (Input.GetButtonDown("Jump"))
            {
                //Cancel any vertical velocity to ensure consistent
                RB.linearVelocity = new Vector2(RB.linearVelocity.x, 0f);
                //Apply jump force
                RB.AddForce(Vector2.up * Jumpforce, ForceMode2D.Impulse);
            }
        }
        else
        {
            //stop applying friction to player so they can fall normally
            RB.linearDamping = 0f;
            //apply adjusted force to make player move in air
            RB.AddForce(PLayerInput * MovementSpeed, ForceMode2D.Force);
        }

        //limit player speed to the set speed
        if (IsGrounded && Mathf.Abs(RB.linearVelocity.x) > MovementSpeed)
        {
            //Limit player speed to the max walk speed
            RB.linearVelocity = new Vector3(MovementSpeed * PLayerInput.x, RB.linearVelocity.y);
        }
        else if(!IsGrounded && Mathf.Abs(RB.linearVelocity.x) > (MovementSpeed * AirControlAmount))
        {
            //limit player speed side ways by the amount of air control allowed
            if (Mathf.Abs(PLayerInput.x) > 0f)
            {
                float limitedAirMovementSpeed = MovementSpeed * AirControlAmount;
                RB.linearVelocity = new Vector3(limitedAirMovementSpeed * PLayerInput.x, RB.linearVelocity.y);
            }

        }

    }
    #endregion

    #region Player Ability

    private void DetectAbility()
    {
        if (Input.GetButtonDown("Fire1") && !AbilityActive)
        {
            if(ArrayRecorder > 0)
            {
                RB.position = PlayerStartLocation;
                AbilityCounter = 0;
                StartCoroutine(UseAbilityPlayBack());
            }
            else
            {
                print("StartedAbility Record");
                PlayerStartLocation = RB.position;
                StopAllCoroutines();
                StartCoroutine(UseAbilityRecord());
                AbilityActive = true;
            }
        }
        else if(Input.GetButtonDown("Fire1") && AbilityActive)
        {
            RB.position = PlayerStartLocation;
            AbilityCounter = 0;
            StartCoroutine(UseAbilityPlayBack()); 
        }
    }
    private IEnumerator UseAbilityRecord()
    {
        //clear any play backs if they are any currently there
        while(AbilityCounter <= AbilityDuration)
        {
            PlayerLocations[ArrayRecorder] = RB.transform;
            ArrayRecorder++;
            AbilityCounter += Time.deltaTime;
            yield return null;
        }

        print("AbilityDone");
        AbilityActive = false;
        yield break;

    }

    private IEnumerator UseAbilityPlayBack()
    {
        while (ArrayRecorder > 0)
        {
            RB.transform.position = PlayerLocations[ArrayRecorder].position;
            RB.transform.rotation = PlayerLocations[ArrayRecorder].rotation;
            ArrayRecorder--;
            yield return null;
        }
        yield break;
    }
    #endregion
}
