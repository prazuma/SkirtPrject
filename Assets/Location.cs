using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : Type {

   public string next_location_name;
   
　　// Use this for initialization
   void Start () {
      setType("location");
   }
	
   // Update is called once per frame
   void Update () {
   }

   public string getNextLocationName () {
      return next_location_name;
   }
}
