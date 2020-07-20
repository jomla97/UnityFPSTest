using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupObject : MonoBehaviour{
    public Transform camera;
    public Transform objectGrabPosition;
    public float maxPickupDistance = 3f;
    public float throwForce = 10f;
    public float maxObjectMass = 50f;

    private RaycastHit hit;
    private GameObject grabbedObject;
    private bool didHit = false;
    private float originalObjectBounciness;
    private float timeStartHoldingDownF;
    private bool paused = false;

    // Update is called once per frame
    void Update()
    {
        if(paused){
          if(grabbedObject){
            ReleaseObject();
          }

          return;
        }

        if (Time.timeScale > 0)
        {
            if(Input.GetKeyDown(KeyCode.F) && grabbedObject != null){
                Debug.Log("Releasing object");
                ReleaseObject();
            }
            else if(Input.GetKeyDown(KeyCode.F)){
                //Player starts holding down F key
                timeStartHoldingDownF = Time.realtimeSinceStartup;
                didHit = Physics.Raycast(camera.position, camera.forward, out hit, maxPickupDistance);
            }

            float timeHeldDown = Time.realtimeSinceStartup - timeStartHoldingDownF;

            if(Input.GetKeyUp(KeyCode.F)){
              if(grabbedObject == null && timeHeldDown < 0.2f && didHit){
                //Add object to inventory
                transform.gameObject.GetComponent<Inventory>().PickUp(hit.transform.gameObject);
              }

              timeStartHoldingDownF = default;
            }

            if(timeStartHoldingDownF > 0 && timeHeldDown > 0.2f){
                Debug.Log("Time held down F key: " + timeHeldDown);

                if(didHit || Physics.Raycast(camera.position, camera.forward, out hit, maxPickupDistance)){
                  Debug.Log("Grabbing object!");
                  GrabObject();
                }

                timeStartHoldingDownF = default;
            }

            if (Input.GetMouseButtonDown(2) && grabbedObject != null)
            {
                Debug.Log("Throwing object!");
                ThrowObject();
            }
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale > 0)
        {
            if (grabbedObject != null)
            {
                grabbedObject.GetComponent<Rigidbody>().velocity = (objectGrabPosition.position - grabbedObject.transform.GetComponent<Renderer>().bounds.center) * 10;
            }
        }
    }

    void GrabObject()
    {
        if(hit.transform.gameObject.GetComponent<Rigidbody>()){
          grabbedObject = hit.transform.gameObject;
          grabbedObject.GetComponent<Rigidbody>().freezeRotation = true;

          if (grabbedObject.GetComponent<Collider>())
          {
              originalObjectBounciness = grabbedObject.GetComponent<Collider>().material.bounciness;

              //Set the object bounciness to 0
              SetObjectBounciness(0);
          }
        }
    }

    void ReleaseObject()
    {
        grabbedObject.GetComponent<Rigidbody>().freezeRotation = false;

        //Reset the object bounciness to original value
        SetObjectBounciness(originalObjectBounciness);

        grabbedObject = null;
    }

    void ThrowObject()
    {
        //Reset the object bounciness to original value
        SetObjectBounciness(originalObjectBounciness);

        grabbedObject.GetComponent<Rigidbody>().freezeRotation = false;
        grabbedObject.GetComponent<Rigidbody>().AddForce(camera.transform.forward * (throwForce / grabbedObject.GetComponent<Rigidbody>().mass));

        grabbedObject = null;
    }

    void SetObjectBounciness(float bounciness)
    {
        if (grabbedObject.GetComponent<Collider>())
        {
            grabbedObject.GetComponent<Collider>().material.bounciness = bounciness;
        }
    }

    public void Resume(){
      paused = false;
    }

    public void Pause(){
      paused = true;
    }
}
