using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO; 

public class Splat : MonoBehaviour
{
   [MenuItem("SplatConvert/Export Splat")]
static void Apply () {
    Texture2D texture = Selection.activeObject as Texture2D; 
   if (texture == null) 
   { 
      EditorUtility.DisplayDialog("Select Texture", "You Must Select a Texture first!", "Ok"); 
      return; 
   } 


   var bytes = texture.EncodeToPNG(); 
   File.WriteAllBytes(Application.dataPath + "/exported_texture.png", bytes); 
   
}
}
