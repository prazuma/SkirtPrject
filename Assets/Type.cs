using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Type : MonoBehaviour {
   private string type;
   // Use this for initialization
   void Start () {
		
　　}
	
   // Update is called once per frame
   void Update () {
		
   }

   public void setType (string type_name) {
      type = type_name;
   }

   public string getType () {
      return type;
   }
}
