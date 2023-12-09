using UnityEngine;
using System.Collections;

public class rock_free_stop : MonoBehaviour {

	public rock_free_master rock_free_master_script;
	
	void OnTriggerEnter (Collider collider){
		
		rock_free_master_script.Stop_OnClick ();
		
	}
}
