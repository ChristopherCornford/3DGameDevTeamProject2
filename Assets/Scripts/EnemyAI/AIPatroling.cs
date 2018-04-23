using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatroling : MonoBehaviour {

	[Header("Pathfinding")]
	public bool patrolWaiting;
	public float totalWaitTime = 3f;
	public float waitProbability = 0.2f;

	public NavMeshAgent navMeshAgent;
	private ConnectedWaypoint currentWaypoint;
	private ConnectedWaypoint previousWaypoint;

	private bool traveling;
	private bool waiting;
	private float waitTimer;
	private int waypointsVisited;

	public void Start () {

		navMeshAgent = this.GetComponent<NavMeshAgent> ();

		if (navMeshAgent == null) {
			Debug.LogError ("NavMesh not connected to " + gameObject.name);
		} else {
			if (currentWaypoint == null) {
				GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
				if (allWaypoints.Length > 0) {
					while (currentWaypoint == null) {
						int random = UnityEngine.Random.Range (0, allWaypoints.Length);
						ConnectedWaypoint startingWaypoint = allWaypoints [random].GetComponent<ConnectedWaypoint> ();

						if (startingWaypoint != null) {
							currentWaypoint = startingWaypoint;
						}
					}
				}
			}
			SetDestination ();
		}
	}
	// Update is called once per frame
	void Update () {
		if (traveling && navMeshAgent.remainingDistance <= 1.0f) {
			
			traveling = false;
			waypointsVisited++;

			if (patrolWaiting) {
				waiting = true;
				waitTimer = 0f;
			} else {
				SetDestination ();
			}
		}
		if (waiting) {
			waitTimer += Time.deltaTime;
			if (waitTimer >= totalWaitTime) {
				waiting = false;
				SetDestination ();
			}
		}
	}
	public void SetDestination() {
		if (waypointsVisited > 0) {
			ConnectedWaypoint nextWaypoint = currentWaypoint.nextWaypoint (previousWaypoint);
			previousWaypoint = currentWaypoint;
			currentWaypoint = nextWaypoint;
		}
		Vector3 targetVector = currentWaypoint.transform.position;
		navMeshAgent.SetDestination (targetVector);
		traveling = true;

	}
}
