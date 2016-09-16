﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {
   public GameObject controllerPivot;
   public GameObject sphere;
   public GameObject messageCanvas;
   public GameObject quad;
   public Text messageText;
   public Fade fade;

   public Material selectedMaterial;

   private bool dragging;

   private const string TEXTURE_PATH = "Textures/";

   private GameObject selectedObject;

   private string type;

   private bool isMove;

   private bool isDiscription;

   // Use this for initialization
   void Start () {
   }
	
   // Update is called once per frame
   void Update () {
      UpdatePointer();
      UpdateStatusMessage();
   }

   private void UpdatePointer () {
      if (GvrController.State != GvrConnectionState.Connected) {
         controllerPivot.SetActive(false);
      }
      controllerPivot.SetActive(true);
      Quaternion controller_orientation = GvrController.Orientation;
      DisplayControllerPosition(controller_orientation);
      controllerPivot.transform.rotation = controller_orientation;

      if (GvrController.TouchDown) {
         RaycastHit hitInfo;
	 Vector3 rayDirection = GvrController.Orientation * Vector3.forward;
	 if (Physics.Raycast(Vector3.zero, rayDirection, out hitInfo)) {
	    if (hitInfo.collider && hitInfo.collider.gameObject) {
	       selectedObject = hitInfo.collider.gameObject;
	       string type = selectedObject.GetComponent<Type>().getType();
	       if (type == "location") {
	          isMove = true;
	       } else if (type == "discription") {
	          isDiscription = true;
	       }
	       //ChangeQuadTexture();
	    }
	 }
      }
      if (GvrController.TouchUp) {
         if (isMove == true) {
	    string next_location_name = selectedObject.GetComponent<Location>().getNextLocationName();
	    SceneManager.LoadScene(next_location_name);
	    isMove = false;
	 } else if (isDiscription == true) {
	    
	 }
      }
/*
      if (dragging) {
         if (GvrController.TouchUp) {
	    EndDragging();
	 }
      } else {
         RaycastHit hitInfo;
	 Vector3 rayDirection = GvrController.Orientation * Vector3.forward;
	 if (Physics.Raycast(Vector3.zero, rayDirection, out hitInfo)) {
	    if (hitInfo.collider && hitInfo.collider.gameObject) {
	       selectedObject = hitInfo.collider.gameObject;
	       messageText.text = hitInfo.collider.gameObject.name;
               messageText.color = Color.white;
               messageCanvas.SetActive(true);
	       ChangeQuadTexture();
	    }
	 }
         if (GvrController.TouchDown && selectedObject != null) {
	    StartDragging();
	 }
      }
      */
   }

   private void ChangeQuadTexture () {
      quad.GetComponent<Renderer>().material = selectedMaterial;
   }

   private void ChangeTexture (string texture_image_name) {
      string texture = TEXTURE_PATH + texture_image_name;
      Texture2D TEXTURE = Resources.Load<Texture2D>(texture);
      sphere.GetComponent<Renderer>().material.mainTexture = TEXTURE;
   }

   private void StartDragging () {
      dragging = true;
   }

   private void EndDragging () {
      if (selectedObject.name == "Move") {
         SceneManager.LoadScene("futomomo2");
      } else if (selectedObject.name == "DiscriptionPoint") {
         selectedObject.transform.FindChild("DiscriptionCanvas").gameObject.SetActive(true);
      }
      dragging = false;
   }

   private void DisplayControllerPosition (Quaternion controller_orientation) {
      messageText.text = controller_orientation.ToString();
      messageText.color = Color.white;
      messageCanvas.SetActive(true);
   }

   private void UpdateStatusMessage() {
      // This is an example of how to process the controller's state to display a status message.
      switch (GvrController.State) {
         case GvrConnectionState.Connected:
	    //messageCanvas.SetActive(false);
            break;
         case GvrConnectionState.Disconnected:
            messageText.text = "Controller disconnected.";
            messageText.color = Color.white;
            messageCanvas.SetActive(true);
       　 　 break;
　       case GvrConnectionState.Scanning:
            messageText.text = "Controller scanning...";
            messageText.color = Color.cyan;
            messageCanvas.SetActive(true);
            break;
         case GvrConnectionState.Connecting:
            messageText.text = "Controller connecting...";
            messageText.color = Color.yellow;
            messageCanvas.SetActive(true);
            break;
         case GvrConnectionState.Error:
            messageText.text = "ERROR: " + GvrController.ErrorDetails;
            messageText.color = Color.red;
            messageCanvas.SetActive(true);
            break;
         default:
            // Shouldn't happen.
            Debug.LogError("Invalid controller state: " + GvrController.State);
            break;
       }
   }
}
