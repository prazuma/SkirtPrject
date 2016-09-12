using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {
   public GameObject controllerPivot;
   public GameObject sphere;
   public GameObject messageCanvas;
   public Text messageText;
   public Fade fade;

   private bool dragging;

   private const string TEXTURE_PATH = "Textures/";

	// Use this for initialization
	void Start () {
		UpdatePointer();
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
      controllerPivot.transform.rotation = GvrController.Orientation;

      if (dragging) {
         if (GvrController.TouchUp) {
	    fade.FadeIn(1, () => {
	       ChangeTexture("hikagami");
	       fade.FadeOut(1);
	    });
	    //ChangeTexture("hikagami");
	    EndDragging();
	 }
      } else {
         if (GvrController.TouchDown) {
	    StartDragging();
	 }
      }
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
      dragging = false;
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
        //messageText.color = Color.red;
        messageCanvas.SetActive(true);
        break;
      default:
        // Shouldn't happen.
        Debug.LogError("Invalid controller state: " + GvrController.State);
        break;
    }
  }
}
