using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour {

   public string next_location_name;
   private string type = "location";

　　// Use this for initialization
   void Start () {
		
   }
	
   // Update is called once per frame
   void Update () {

   }

   public string getType () {
      return type;
   }
}
