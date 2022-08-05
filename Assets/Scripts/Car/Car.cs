using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    protected GameManager gameManager;
    protected FieldOfView fieldOfView;

    protected float minSpeed = 10f;
    protected float maxSpeed = 15f;

    protected float minRotationSpeed = 360f;
    protected float maxRotationSpeed = 720f;

    protected float carRotationSpeed;
    protected float carSpeed;

    protected float catchDistance = 2f;
    protected float jailedDistance = 7f;
    protected float obstacleDistance = 3.5f;

    protected GameObject target;
    protected GameObject targetObstacle;
    protected GameObject capturer;

    protected Vector3 direction;

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;

        fieldOfView = GetComponent<FieldOfView>();

        carRotationSpeed = SetCarRotation();
        carSpeed = SetCarSpeed();

        targetObstacle = GetRandomObstacle();
        MoveToRandom();
    }

    private void Move()
    {
        transform.Translate(direction * Time.deltaTime, Space.World);
        RotateCar(direction);
    }

    protected virtual void Update()
    {
        if (fieldOfView.detectedObjects.Count != 0)
            Move(fieldOfView.GetNearObstacles(), true);
        else
            Move();
    }

    public void MoveToRandom()
    {
        float distance = Vector2.Distance(transform.position, targetObstacle.transform.position);
        Debug.Log(distance);
        if (distance <= obstacleDistance)
            targetObstacle = GetRandomObstacle();    
        
        Move(targetObstacle, false);
    }

    public GameObject GetRandomObstacle()
    {
        List<GameObject> obstacles = gameManager.GetObstacleList();
        return obstacles[Random.Range(0, obstacles.Count)];
    }

    protected virtual void Move(GameObject entity, bool isRepulsion)
    {
        List<GameObject> entityList = new List<GameObject>();
        entityList.Add(entity);
        Move(entityList, isRepulsion);
    }

    protected virtual void Move(List<GameObject> entity, bool isRepulsion)
    {
        float distance;

        if (!isRepulsion)
        {
            foreach (GameObject e in entity)
            {
                Vector3 obstaclePosition;

                if (e.tag == gameManager.wallTag)
                    obstaclePosition = e.GetComponent<Collider>().ClosestPoint(transform.position);
                else
                    obstaclePosition = e.transform.position;

                direction = new Vector3(obstaclePosition.x, 0f, obstaclePosition.z) - transform.position;

                distance = direction.magnitude;
                direction = direction.normalized;

                if (distance < 0.1f)
                    continue;

                direction = direction * distance * 1f;

                transform.Translate(direction / carSpeed * Time.deltaTime, Space.World);
                RotateCar(direction);
            }
        }

        if (isRepulsion)
        {
            foreach (GameObject e in entity)
            {
                if(e != target && e != capturer)
                {
                    Vector3 obstaclePosition;

                    if (e.tag == gameManager.wallTag)
                        obstaclePosition = e.GetComponent<Collider>().ClosestPoint(transform.position);
                    else
                        obstaclePosition = e.transform.position;

                    direction = new Vector3(obstaclePosition.x, 0f, obstaclePosition.z) - transform.position;

                    distance = direction.magnitude;
                    direction = direction.normalized;

                    direction = direction * (carSpeed / distance) * -1f;

                    transform.Translate(direction * Time.deltaTime, Space.World);
                    RotateCar(direction);
                }
            }
        }
    }

    private void RotateCar(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, carRotationSpeed * Time.deltaTime);
        }
    }

    private float SetCarRotation()
    {
        return Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private float SetCarSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetTarget(GameObject targetObject)
    {
        target = targetObject;
    }

    public bool TransportToPrison(Sedan sedan)
    {
        GameObject prison = gameManager.GetPrison();
        Move(prison, false);
        return (Vector2.Distance(sedan.transform.position, new Vector2(prison.transform.position.x, prison.transform.position.z)) <= jailedDistance);
    }

}
