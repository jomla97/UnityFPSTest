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
    private float originalObjectBounciness;

    void GrabObject()
    {
        grabbedObject = hit.transform.gameObject;
        grabbedObject.GetComponent<Rigidbody>().freezeRotation = true;

        if (grabbedObject.GetComponent<Collider>())
        {
            originalObjectBounciness = grabbedObject.GetComponent<Collider>().material.bounciness;

            //Set the object bounciness to 0
            SetObjectBounciness(0);
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

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //Grab object
            if (Input.GetKeyDown(KeyCode.F) && grabbedObject == null && Physics.Raycast(camera.position, camera.forward, out hit, maxPickupDistance) && hit.transform.GetComponent<Rigidbody>() && hit.transform.GetComponent<Rigidbody>().mass <= maxObjectMass)
            {
                Debug.Log("Grabbing object!");
                GrabObject();
            }
            //Release grabbed object
            else if (Input.GetKeyDown(KeyCode.F) && grabbedObject != null)
            {
                Debug.Log("Releasing object");
                ReleaseObject();
            }
            //Throw grabbed object
            else if (Input.GetMouseButtonDown(0) && grabbedObject != null)
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
}
