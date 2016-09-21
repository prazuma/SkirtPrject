using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerManager : MonoBehaviour {
   public GameObject controllerPivot;
   public GameObject messageCanvas;
   public Text messageText;
   public GameObject quad;
   public GameObject discriptionCanvas;
   public Text discriptionText;
   public Fade fade;

   private GameObject selectedObject;

   //private string type;
   private bool isLocationSelected;
   private bool isDiscriptionSelected;

   private const string MATERIAL_PATH = "Materials/";
   private GameObject hoveredObject;
   
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
      controllerPivot.transform.rotation = controller_orientation;

      RaycastHit hitInfo;
      // rayDirection is a vector that points in the same direction as the controller is pointing.
      Vector3 rayDirection = GvrController.Orientation * Vector3.forward;
      if (Physics.Raycast(Vector3.zero, rayDirection, out hitInfo)) {
         if (hitInfo.collider != null && hitInfo.collider.gameObject != null) {
	    GameObject obj = hitInfo.collider.gameObject;
	    if (GvrController.TouchDown) {
	       selectedObject = obj;
	       string type = selectedObject.GetComponent<Type>().getType();
	       if (type == "location") {
	          isLocationSelected = true;
	       } else if (type == "discription") {
	          isDiscriptionSelected = true;
		  showDiscription();
	       }
	    } else {
	       string type = obj.GetComponent<Type>().getType();
	       if (type == "location") {
	          hoveredObject = obj;
		  ChangeLocationAreaMaterial(true);
	       }
	    }
	 }
      } else {
         ChangeLocationAreaMaterial(false);
	 hoveredObject = null;
      }

      if (GvrController.TouchUp) {
         if (isLocationSelected == true) {
	    goNextLocation();
	 } else if (isDiscriptionSelected == true) {
	    hideDiscription();
	 }
      }
   }

   private void ChangeLocationAreaMaterial (bool isLocationAreaHovered) {
      if (hoveredObject == null) {
         return;
      }
      string material_name;
      if (isLocationAreaHovered) {
         material_name = "LocationHoveredMaterial";
      } else {
         material_name = "LocationNonHoveredMaterial";
      }
      Material material = Resources.Load<Material>(MATERIAL_PATH + material_name);
      hoveredObject.GetComponent<Renderer>().material = material;
   }

   private void goNextLocation () {
      string next_location_name = selectedObject.GetComponent<Location>().getNextLocationName();
      SceneManager.LoadScene(next_location_name);
      isLocationSelected = false;
   }

   private void showDiscription () {
      string discription_text = selectedObject.GetComponent<Discription>().getDiscriptionText();
      discriptionText.text = discription_text;
      discriptionCanvas.SetActive(true);
   }

　 private void hideDiscription() {
      discriptionCanvas.SetActive(false);
      isDiscriptionSelected = false;
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
	    messageCanvas.SetActive(false);
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
