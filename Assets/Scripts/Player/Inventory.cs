using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform activeItemPosition;
    public List<GameObject> gameObjects = new List<GameObject>();
    public int activeItemIndex = 0;
    public float dropForce = 10f;
    public float throwForce = 50f;

    private float timeStartHoldingDownF;

    void Update(){
      //Drop item
      if(Input.GetKeyDown(KeyCode.Q) && gameObjects.Count > 0){
        Drop(activeItemIndex);
      }

      //Throw active item
      if(Input.GetMouseButtonDown(2) && gameObjects.Count > 0){
        ThrowActiveItem();
      }

      //Update active object
      UpdateActiveItem();

      //Switch the active item index
      if(Input.mouseScrollDelta.y > 0){
        SwitchActiveItemIndex(activeItemIndex + 1);
      }
      else if(Input.mouseScrollDelta.y < 0){
        SwitchActiveItemIndex(activeItemIndex - 1);
      }
    }

    void UpdateActiveItem(){
      if(gameObjects.Count > activeItemIndex){
        GameObject gameObject = gameObjects[activeItemIndex];

        //Activate object if disabled
        if(!gameObject.activeSelf){
          gameObject.SetActive(true);
        }

        //Update position
        gameObject.transform.position = activeItemPosition.position;

        //Update rotation
        gameObject.transform.rotation = activeItemPosition.rotation;
      }
    }

    public void PickUp(GameObject gameObject){
      Debug.Log("Add object to inventory!");
      gameObjects.Add(gameObject);

      if(gameObject.GetComponent<Rigidbody>()){
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        rigidbody.useGravity = false;
        rigidbody.detectCollisions = false;
      }

      gameObject.SetActive(false);
    }

    public void Drop(int index){
      if(index >= 0 || index < gameObjects.Count){
        GameObject gameObject = gameObjects[index];
        gameObject.transform.position = activeItemPosition.position;
        gameObject.SetActive(true);

        if(gameObject.GetComponent<Rigidbody>()){
          Rigidbody rb = gameObject.GetComponent<Rigidbody>();
          rb.freezeRotation = false;
          rb.useGravity = true;
          rb.detectCollisions = true;
          rb.AddForce(activeItemPosition.forward * dropForce);
        }

        if(index > 0){
          activeItemIndex = index - 1;
        }

        gameObjects.RemoveAt(index);
      }
    }

    public void ThrowActiveItem(){
      GameObject gameObject = gameObjects[activeItemIndex];
      gameObject.transform.position = activeItemPosition.position;
      gameObject.SetActive(true);

      if(gameObject.GetComponent<Rigidbody>()){
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = false;
        rb.useGravity = true;
        rb.detectCollisions = true;
        rb.AddForce(activeItemPosition.forward * throwForce);
      }

      if(activeItemIndex > 0){
        activeItemIndex -= 1;
      }

      gameObjects.RemoveAt(activeItemIndex);
    }

    public void SwitchActiveItemIndex(int index){
      if(gameObjects.Count > 0){
        index = Mathf.Clamp(index, 0, gameObjects.Count - 1);

        if(index != activeItemIndex){
          gameObjects[activeItemIndex].SetActive(false);

          activeItemIndex = index;
          GameObject gameObject = gameObjects[activeItemIndex];

          gameObject.SetActive(true);
        }
      }
    }
}
