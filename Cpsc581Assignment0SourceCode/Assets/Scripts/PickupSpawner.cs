using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickups;				// Array of pickup prefabs with the bomb pickup first and health second.
	public float pickupDeliveryTime = 1f;		// Delay on delivery.
	public float dropPos;					// Smallest value of x in world coordinates the delivery can happen at.

    public void DeliverPickup()
    {
        StartCoroutine(_deliverPickup());
    }

    public IEnumerator _deliverPickup()
	{
		// Wait for the delivery delay.
		yield return new WaitForSeconds(pickupDeliveryTime);

		// Create a position with the random x coordinate.
		Vector3 dropPosVec = new Vector3(dropPos, 15f, 1f);
		Instantiate(pickups[0], dropPosVec, Quaternion.identity);

	}
}
