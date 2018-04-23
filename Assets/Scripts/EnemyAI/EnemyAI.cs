using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

	public AIPatroling aiPatroling;

	public float fpsTargetDistance;
	public float enemyLookDistance;
	public float enemyMoveDistance;
	public float attackDistance;
	public float enemyMoveSpeed;
	public float damping;
	[Header("Color")]
	public Color Unaware;
	public Color Aware;
	public Color Moving;
	public Color Hostile; 
	[Header("Damage")]
	public float health = 3f;
	[Range(1, 4)]
	private int Status;
	[Header("\t")]
	public Transform fpsTarget;
	//Rigidbody rb;
	Renderer myRenderer;

	[Header("AI Seeing")]
	public GameObject player;
	public Collider playerCollider;
	public Camera cam;
	private Plane[] planes;

	[Header("Shooting")]
	public AudioSource enemyShot;
	public GameObject playerHead;
	public Camera npcCam;
	public GameObject hitEffect;
	public float shotsMissed;
	public float chanceToHit = 0.8f;
	public float firerate = 1f;
	public bool canFire;
	public float randTemp;
	public float temp;

	// Use this for initialization
	void Start () {
		aiPatroling = GetComponent<AIPatroling> ();
		fpsTarget = GameObject.FindGameObjectWithTag ("Player").transform;
		myRenderer = GetComponent<Renderer> ();
		//rb = GetComponent<Rigidbody> ();

		cam = GetComponent<Camera> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		playerCollider = player.GetComponent<CharacterController> ();
		playerHead = GameObject.FindGameObjectWithTag ("PlayerHead");
		canFire = true;
	}
	
	// Update is called once per frame
	void Update () {

		fpsTargetDistance = Vector3.Distance (fpsTarget.position, transform.position);
		if (fpsTargetDistance < enemyLookDistance) {
			Status = 2;

		} else {
			Status = 1;
		}
		if (fpsTargetDistance < enemyMoveDistance && fpsTargetDistance > attackDistance ) {
		//	moveTowardsPlayer ();
			Status = 3;
		}
		if (fpsTargetDistance < attackDistance) {
			Status = 4;
		} 
		switch (Status) {
		case 1:
			myRenderer.material.color = Unaware;
			aiPatroling.navMeshAgent.isStopped = false;
			Debug.Log ("All Is Quiet");
			break;

		case 2:
			myRenderer.material.color = Aware; 
			aiPatroling.navMeshAgent.isStopped = false;
			Debug.Log ("Visuals Confirmed");
			break;

		case 3:
			myRenderer.material.color = Moving;
			aiPatroling.navMeshAgent.isStopped = false;
			Debug.Log ("Moving To Capture");
			break;

		case 4:
			myRenderer.material.color = Hostile;
			aiPatroling.navMeshAgent.isStopped = true;
			Debug.Log ("Open Fire");
			lookAtPlayer ();
			enemyAttack ();
			break;
		}

	}

	public void lookAtPlayer () {
		Vector3 playerPos = player.transform.position;
		Vector3 enemyPos = transform.position;
		Vector3 delta = new Vector3 (playerPos.x - enemyPos.x, 0.0f, playerPos.z - enemyPos.z);
		Quaternion rotation = Quaternion.LookRotation (delta);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
	}
	public void enemyAttack(){
		planes = GeometryUtility.CalculateFrustumPlanes (cam);
		if (GeometryUtility.TestPlanesAABB (planes, playerCollider.bounds) == true) {
			Debug.Log ("I see " + player.name);
			lookAtPlayer ();
		} else {
			aiPatroling.SetDestination ();
			Status = 3;
		}
	}
	void Die () {
		Destroy (gameObject);
	}
}
