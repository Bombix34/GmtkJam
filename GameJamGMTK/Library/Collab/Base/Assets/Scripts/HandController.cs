using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

    public string nameTrigger = "LeftTrigger";
    public HandController theOtherHand;
    public float speedHandForward = 10f;
    public float speedHandBackward = 10f;
    public float MaxDistanceHand = 5f;
    public float distanceOfHandFromWall = 1f;
    public float offsetGrip = 0.75f;
    public float forceThrow = 5f;
    public GameObject Anchor;
    public GameObject Player;
    public GameObject RestPosition;

    private bool isAxisInUse = false;
    private bool HandInUse = false;
    public bool HandOnWall = false;
    private bool enemyInHand = false;
    private bool laSourisEstAppuyer = false;

    private bool startGoingForward = false;
    private bool startGoingBackward = false;

    private Rigidbody rBody;

    private PlayerController myPlayerController;
    private GameObject ennemyReference;

    private float positionCameraY;

    private Vector3 rightVectorAnchor;

    private int mouseButton;

    public Vector3 offSetHand;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody>();
        myPlayerController = Player.GetComponent<PlayerController>();
        positionCameraY = Camera.main.transform.localPosition.y;

        if(nameTrigger == "LeftTrigger")
        {
            mouseButton = 0;
        }
        else
        {
            mouseButton = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButton(mouseButton))
        {
            laSourisEstAppuyer = true;
        }
        else
        {
            laSourisEstAppuyer = false;
        }

        if (Input.GetAxis(nameTrigger) == 0)
        {
            if(!laSourisEstAppuyer)
            {
                isAxisInUse = false;

                if (HandOnWall)
                {
                    releaseHandFromWall();
                }
                else if (HandInUse)
                {
                    startGoingForward = false;
                    startGoingBackward = true;

                    rBody.isKinematic = true;
                    rBody.velocity = Vector3.zero;
                }
            }
        }

        if((Input.GetAxis(nameTrigger) != 0 && !isAxisInUse) || Input.GetMouseButtonDown(mouseButton))
        {
            isAxisInUse = true;

            if(!HandInUse)
            {
                if(enemyInHand)
                {
                    enemyInHand = false;
                    ennemyReference.GetComponent<EnemiesScript>().resetGravity();
                    ennemyReference.transform.localPosition = new Vector3(0f, positionCameraY, 2f);
                    ennemyReference.transform.parent = null;
                    ennemyReference.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * forceThrow, ForceMode.Impulse);
                }
                else
                {
                    HandInUse = true;
                    rightVectorAnchor = Anchor.transform.right;
                    transform.localPosition = new Vector3(0f, positionCameraY, transform.localPosition.z);
                    rBody.isKinematic = false;
                    transform.parent = null;
                    rBody.velocity = Anchor.transform.forward * speedHandForward;
                    startGoingForward = true;
                }
            }
        }

        if(startGoingForward)
        {
            if((transform.position - Player.transform.position).magnitude >= MaxDistanceHand)
            {
                startGoingForward = false;
                startGoingBackward = true;

                rBody.isKinematic = true;
                rBody.velocity = Vector3.zero;
            }
        }
        else if (startGoingBackward)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, RestPosition.transform.position, speedHandBackward);

            if(transform.localPosition == RestPosition.transform.position)
            {
                transform.parent = Anchor.transform;
                transform.localRotation = Quaternion.identity;
                HandInUse = false;
                startGoingBackward = false;
            }
        }
        else if ((transform.position - Player.transform.position).magnitude >= MaxDistanceHand)
        {
            if (HandOnWall)
            {
                releaseHandFromWall();
            }
            else
            {
                startGoingBackward = true;

                rBody.isKinematic = true;
                rBody.velocity = Vector3.zero;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Struct" && startGoingForward == true)
        {   
            HandOnWall = true;

            if(theOtherHand.HandOnWall == true)
            {
                theOtherHand.releaseHandFromWall();
            }

            startGoingForward = false;
            rBody.velocity = Vector3.zero;
            rBody.isKinematic = true;


            transform.position = collision.contacts[0].point;

            Vector3 right = Vector3.Cross(collision.contacts[0].normal, Vector3.up);

            if (right == Vector3.zero)
            {
                if(collision.contacts[0].normal.y < 0)
                {
                    right = Vector3.down;
                }
                else
                {
                    right = rightVectorAnchor;
                }
            }

            Vector3 rotatedNormal = Quaternion.AngleAxis(-90, right) * collision.contacts[0].normal;

//            Debug.Log(rotatedNormal);

            if(right != rightVectorAnchor)
            {
                if(transform.position.y < Player.transform.position.y)
                {
                    rotatedNormal = rotatedNormal * -1f;
                }
            }


            

            rotatedNormal = rotatedNormal * offsetGrip;
            //myPlayerController.pullPlayer(((collision.contacts[0].normal * distanceOfHandFromWall) + collision.contacts[0].point) + rotatedNormal, false);
            myPlayerController.pullPlayer(((collision.contacts[0].normal * distanceOfHandFromWall)) + rotatedNormal, this);
            transform.parent = collision.transform;
        }        

        /*
            Si un seul bras il faut grapper l'enemy vers soit.
            Si deux bras touche il faut se grapper normalement vers l'enemy
            Pour détecter si les deux bras touche l'ennemi on compare la distance entre les deux bras
         */
        if(collision.gameObject.tag == "Enemy" && startGoingForward == true){

            enemyInHand = true;
            collision.gameObject.GetComponent<EnemiesScript>().grabEnemy();

            collision.gameObject.transform.parent = transform;
            collision.gameObject.transform.localPosition = Vector3.zero;
            collision.gameObject.transform.localRotation = Quaternion.identity;

            rBody.isKinematic = true;
            rBody.velocity = Vector3.zero;

            startGoingBackward = true;
            startGoingForward = false;

            ennemyReference = collision.gameObject;


            /*if(Vector3.Distance(transform.position, theOtherHand.transform.position) < 1){
                HandOnWall = true;

                if(theOtherHand.HandOnWall == true)
                {
                    theOtherHand.releaseHandFromWall();
                }

                startGoingForward = false;
                rBody.velocity = Vector3.zero;
                rBody.isKinematic = true;

                myPlayerController.pullPlayer(collision.contacts[0].normal);
            }   else    {
                
            }*/
        }
    }


    public bool getIsAxisInUse(){
        return isAxisInUse;
    }
    public bool getHandInUse(){
        return HandInUse;
    }
    public bool getHanOnWall(){
        return HandOnWall;
    }
    public bool getStartGoingForward(){
        return startGoingForward;
    }
    public bool getStartGoingBackward(){
        return startGoingBackward;
    }

    public void releaseHandFromWall()
    {
        HandOnWall = false;

        startGoingForward = false;
        startGoingBackward = true;

        rBody.isKinematic = true;
        rBody.velocity = Vector3.zero;

        
        myPlayerController.releasePlayerFromPull();
    }
}
