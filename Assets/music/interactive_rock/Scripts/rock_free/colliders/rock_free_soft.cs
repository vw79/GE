using UnityEngine;
using System.Collections;

public class rock_free_soft : MonoBehaviour {

	public rock_free_master rock_free_master_script;

	void OnTriggerEnter (Collider collider){

		rock_free_master_script.Soft_OnClick ();

	}
}
