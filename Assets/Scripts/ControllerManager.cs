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

      // bool True if the ray intersects with a Collider, otherwise false.
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
               // If the cursor hover over a location or discription area, a location area will be displayed or a discription area image will be changed.
               ChangeMaterialToHoveredMaterial(obj);
            }
         }
      } else {
         // When the ray does not intersects with a collider,
         // hide location area and disable to go to the next location, and
         // change the image of discription area.
         ChangeMaterialToNonHoveredMaterial();
         isLocationSelected = false;
      }

      if (GvrController.TouchUp) {
         if (isLocationSelected == true) {
            goNextLocation();
         } else if (isDiscriptionSelected == true) {
            hideDiscription();
         }
      }
   }

   private void ChangeTexture (string materialName) {
      if (hoveredObject == null) {
         return;
      }
      Material material = Resources.Load<Material>(MATERIAL_PATH + materialName);
      hoveredObject.GetComponent<Renderer>().material = material;
   }

   private void ChangeMaterialToHoveredMaterial (GameObject obj) {
      hoveredObject = obj;
      string type = hoveredObject.GetComponent<Type>().getType();
      if (type == "location") {
         ChangeTexture("LocationHoveredMaterial");
      } else if (type == "discription") {
         ChangeTexture("DiscriptionHoveredMaterial");
      }
   }

   private void ChangeMaterialToNonHoveredMaterial () {
      if (hoveredObject == null) {
         return;
      }
      string type = hoveredObject.GetComponent<Type>().getType();
      if (type == "location") {
         ChangeTexture("LocationNonHoveredMaterial");
      } else if (type == "discription") {
         ChangeTexture("DiscriptionNonHoveredMaterial");
      }
      hoveredObject = null;
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
