using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject sedanPrefab;

    [SerializeField]
    private GameObject policePrefab;

    [SerializeField]
    private GameObject prison;

    private const int numSedans = 10;
    private const int numPolice = 2;

    private List<Car> sedanList = new List<Car>(numSedans);
    private List<Car> policeList = new List<Car>(numPolice);
    private List<GameObject> walls = new List<GameObject>();
    private List<GameObject> obstacles = new List<GameObject>();

    [HideInInspector]
    public string policeTag = "Police";
    [HideInInspector]
    public string sedanTag = "Sedan";
    [HideInInspector]
    public string obstacleTag = "Obstacle";
    [HideInInspector]
    public string wallTag = "Wall";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag(wallTag))
        {
            walls.Add(wall);
        }

        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag(obstacleTag))
        {
            obstacles.Add(obstacle);
        }

        for (int i = 0; i < numSedans; i++)
        {
            GameObject sedanCar = Instantiate(sedanPrefab, new Vector3(0f + i, 0f, 0f), Quaternion.identity);
            sedanList.Add(sedanCar.GetComponent<Sedan>());
        }

        for (int i = 0; i < numPolice; i++)
        {
            GameObject policeCar = Instantiate(policePrefab, new Vector3(10f + i, 0f, 0f), Quaternion.identity);
            policeList.Add(policeCar.GetComponent<Police>());
        }
    }

    void Update()
    {
        
    }

    public GameObject GetPrison()
    {
        return prison;
    }

    public List<Car> GetSedanList()
    {
        return sedanList;
    }

    public List<Car> GetPoliceList()
    {
        return policeList;
    }

    public List<GameObject> GetWallList()
    {
        return walls;
    }

    public List<GameObject> GetObstacleList()
    {
        return obstacles;
    }
}

