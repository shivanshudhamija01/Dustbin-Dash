using UnityEngine;

public class CloudController : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed;

    private CloudPool pool;
    private CloudSpawner spawner;
    private CloudLane assignedLane;

    public void Initialize(
        Vector3 target,
        float speed,
        CloudPool cloudPool,
        CloudSpawner cloudSpawner,
        CloudLane lane)
    {
        targetPosition = target;
        moveSpeed = speed;

        pool = cloudPool;
        spawner = cloudSpawner;
        assignedLane = lane;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (spawner != null)
            {
                spawner.CloudRemoved(assignedLane);
            }

            pool.ReturnCloud(gameObject);
        }
    }
}