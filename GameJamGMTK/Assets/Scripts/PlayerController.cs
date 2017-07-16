using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speedWalking = 1f;
    public float airControlForce = 10f;
    public float maxAirSpeed = 100f;
    public float speedPull = 1f;
    public float distanceOfHandFromWall = 1f;
    public Animator leftHandAnimator;
    //public Animator rightHandAnimator;

    private bool startPullingPlayer = false;
    private Vector3 targetPull;

    private Rigidbody rBody;
    private int numberOfHandAttachOnWall = 0;

    private HandController handAttach;
    private Vector3 offsetHand;
    private Vector3 offsetToStayAt;

    private float distToGround;


    // Use this for initialization
    void Start () {
        distToGround = GetComponent<Collider>().bounds.extents.y;
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (startPullingPlayer)
        {
            /*transform.position = Vector3.MoveTowards(transform.position, targetPull, speedPull);

            if (transform.position == targetPull)
            {
                startPullingPlayer = false;
            }*/

                
            rBody.velocity = ((handAttach.transform.position + offsetHand) - transform.position).normalized* speedPull;


            /* 

            if(onMovingPlatform){
                rBody.velocity = (targetPull - transform.position).normalized* speedPull;
            }   else    {
                rBody.velocity = (targetPull - transform.position).normalized* speedPull;
            }*/

            if ((transform.position - (handAttach.transform.position + offsetHand)).magnitude < 0.15f)
            {
                startPullingPlayer = false;
                rBody.velocity = Vector3.zero;

                offsetToStayAt = transform.position - (handAttach.transform.position + offsetHand);
            }

            leftHandAnimator.SetFloat("Movement speed", 0f);
        }
        else if(numberOfHandAttachOnWall <= 0)
        {
            Vector2 inputValues = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);

            if (IsGrounded())
            {
                inputValues = inputValues * speedWalking;

                leftHandAnimator.SetFloat("Movement speed", inputValues.magnitude);

                Vector3 vectorForce = new Vector3(inputValues.x, rBody.velocity.y, inputValues.y);
                vectorForce = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * vectorForce;
                rBody.velocity = vectorForce;
            }
            else
            {
                leftHandAnimator.SetFloat("Movement speed", inputValues.magnitude);
                leftHandAnimator.SetFloat("Movement speed", 0f);

                inputValues = inputValues * airControlForce;
                Vector3 vectorForce = new Vector3(inputValues.x, 0f, inputValues.y);

                rBody.AddRelativeForce(vectorForce);

                if (rBody.velocity.magnitude > maxAirSpeed)
                {
                    rBody.velocity = rBody.velocity.normalized * maxAirSpeed;
                    Debug.Log("Allo le con");
                }
            }
        }
        else
        {
            transform.position = (handAttach.transform.position + offsetHand) + offsetToStayAt;
            leftHandAnimator.SetFloat("Movement speed", 0f);
        }

        if(IsGrounded())
        {
            leftHandAnimator.SetBool("Is in air?", false);
        }
        else
        {
            leftHandAnimator.SetBool("Is in air?", true);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    public void pullPlayer(Vector3 positionImpact, HandController hand)
    {
        //float distanceFromImpact = (positionImpact - transform.position).magnitude;
        //float realDistanceToTravel = Mathf.Clamp(distanceFromImpact - distanceOfHandFromWall, 0f, distanceFromImpact);
        //float scalairDistance = realDistanceToTravel / distanceFromImpact;

        offsetHand = positionImpact;
        handAttach = hand;
        rBody.velocity = Vector3.zero;
        /*Vector3 offsetPosition = transform.position - positionHand;
        offsetPosition = offsetPosition.normalized;
        offsetPosition = offsetPosition * 1f;
        targetPull = positionHand + offsetPosition;*/
        

        rBody.useGravity = false;
        startPullingPlayer = true;

        numberOfHandAttachOnWall++;
    }

    public void releasePlayerFromPull()
    {
        numberOfHandAttachOnWall--;

        if(numberOfHandAttachOnWall <= 0)
        {
            rBody.useGravity = true;
            startPullingPlayer = false;
        }
    }

    public bool getStartPullingPlayer(){
        return startPullingPlayer;
    }

    private static PlayerController s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static PlayerController instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(PlayerController)) as PlayerController;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
				Debug.LogError("No player in scene");
				/* 
                GameObject obj = Instantiate(Resources.Load("PlayerController") as GameObject);
                s_Instance = obj.GetComponent<PlayerController>();*/
            }
            return s_Instance;
        }
	}
}
