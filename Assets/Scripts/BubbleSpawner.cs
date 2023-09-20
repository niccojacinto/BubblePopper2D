using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject _bubblePrefab;
    [SerializeField]
    GameObject _bubbleBurstPrefab;
    [SerializeField]
    float spawnRate = 1;

    Vector3 _screenCenter;

    private Camera _mainCamera;
    List<Bubble> _activeBubbles;
    Stack<Bubble> _inactiveBubbles;


    float _startMinBubbleScale = 1f;
    float _startMaxBubbleScale = 1f;
    float _startMinBubbleSpeed = 1f;
    float _startMaxBubbleSpeed = 1f;

    float _minBubbleScale;
    float _maxBubbleScale;
    float _minBubbleSpeed;
    float _maxBubbleSpeed;


    public float MinBubbleSpeed
    {
        get { return _minBubbleSpeed; }
        set 
        {
                _minBubbleSpeed = value;
        }
    }

    public float MaxBubbleSpeed
    {
        get { return _maxBubbleSpeed; }
        set { _maxBubbleSpeed = value; }
    }

    public float MinBubbleScale
    {
        get { return _minBubbleScale; }
        set
        {
                _minBubbleScale = value;
        }
    }

    public float MaxBubbleScale
    {
        get { return _maxBubbleScale; }
        set
        {
                _maxBubbleScale = value;
        }
    }


    static BubbleSpawner _instance;
    public static BubbleSpawner Instance
    {
        get
        {
            // If the instance does not exist, find it in the scene or create a new one
            if (_instance == null)
            {
                _instance = FindObjectOfType<BubbleSpawner>();

                // If no instance exists in the scene, create a new GameObject with the script attached
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(BubbleSpawner).Name);
                    _instance = singletonObject.AddComponent<BubbleSpawner>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Ensure there is only one instance of this object
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Reset()
    {
        _minBubbleScale = _startMinBubbleScale;
        _maxBubbleScale = _startMaxBubbleScale;
        _minBubbleSpeed = _startMinBubbleSpeed;
        _maxBubbleSpeed = _startMaxBubbleSpeed;
    }

    void Start()
    {

        Reset();

        _mainCamera = Camera.main;
        // InvokeRepeating("SpawnBubble", 0f, 0.2f);
        _screenCenter = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        _activeBubbles = new List<Bubble>();
        _inactiveBubbles = new Stack<Bubble>();
    }

    public void ClearBubbles()
    {
        while (_activeBubbles.Count > 0)
        {
            _activeBubbles[0].Pop();
        }
    }

    public void Pop(Bubble bubble)
    {
        bubble.gameObject.SetActive(false);
        _activeBubbles.Remove(bubble);
        _inactiveBubbles.Push(bubble);
        Instantiate(_bubbleBurstPrefab, bubble.transform.position, Quaternion.identity, null);
    }

    public void SpawnBubbles(int amount)
    {
        StartCoroutine(SpawnBubblesCR(amount));
    }

    public void ResetBubble(Bubble bubble)
    {
        Vector3 spawnPosition = GetRandomPointOutsideScreen();
        bubble.transform.position = spawnPosition;
    }

    IEnumerator SpawnBubblesCR(int amount)
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);
        int amountToSpawn = amount;
        while (amountToSpawn > 0)
        {
            SpawnBubble();
            amountToSpawn--;
            yield return wait;
        }
    }

    void SpawnBubble()
    {
        Vector3 spawnPosition = GetRandomPointOutsideScreen();

        Bubble bubble;
        GameObject g;
        if (_inactiveBubbles.Count > 0)
        {
            bubble = _inactiveBubbles.Pop();
            g = bubble.gameObject;
            g.SetActive(true);
        }
        else
        {
            // Instantiate the object at the calculated position
            g = Instantiate(_bubblePrefab, transform);
        }


        Rigidbody2D rb2d = g.GetComponent<Rigidbody2D>();
        g.transform.localPosition = spawnPosition;

        Vector3 randomDeviation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        Vector3 centerDirection = _screenCenter - g.transform.position;

        float randScale = Random.Range(_minBubbleScale, _maxBubbleScale);
        g.transform.localScale = Vector3.one * randScale;
        float randSpeed = Random.Range(_minBubbleSpeed, _maxBubbleSpeed);
        rb2d.velocity = centerDirection * randSpeed + randomDeviation;

        Bubble b = g.GetComponent<Bubble>();
        b.Wobble();
        _activeBubbles.Add(b);

    }

    Vector3 GetRandomPointOutsideScreen()
    {

        int edge = Random.Range(0, 4);  // 0,1,2,3 == up, down, left, right

        Vector3 randomPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f));
        switch (edge)
        {
            case 0:
                // Spawn outside upper edge
                randomPoint.y = 0.9f;
                break;
            case 1:
                // Spawn outside bottom edge
                randomPoint.y = 0.1f;
                break;
            case 2:
                // Spawn outside left edge
                randomPoint.x = 0.1f;
                break;
            case 3:
                // Spawn outside right edge;
                randomPoint.x = 0.9f;
                break;
            default:
                break;
        }
        randomPoint = Camera.main.ViewportToWorldPoint(randomPoint);
        randomPoint.z = 0f;
        return randomPoint;
    }
}
