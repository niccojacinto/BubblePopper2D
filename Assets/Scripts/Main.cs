using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{

    public int level;
    [SerializeField]
    TMP_Text _title;
    [SerializeField]
    CountdownTimer _timer;
    [SerializeField]
    TMP_Text _message;
    [SerializeField]
    Button _startButton;
    [SerializeField]
    TMP_Text _difficulty;


    [SerializeField]
    Best _bestScore;

    [SerializeField]
    BubbleSpawner _bubbleSpawner;

    int bubblesPopped = 0;

    public int bubbleAmount = 1;

    Vector2 topLeftPoint;
    Vector2 topRightPoint;
    Vector2 bottomRightPoint;
    Vector2 bottomLeftPoint;


    public int RequiredBubbles
    {
        get
        {
            return bubbleAmount;
        }
    }

    static Main _instance;
    public static Main Instance
    {
        get
        {
            // If the instance does not exist, find it in the scene or create a new one
            if (_instance == null)
            {
                _instance = FindObjectOfType<Main>();

                // If no instance exists in the scene, create a new GameObject with the script attached
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(Main).Name);
                    _instance = singletonObject.AddComponent<Main>();
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

    void Start()
    {
        SetBounds();
    }


    void SetBounds()
    {
        
        topLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        topRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        bottomRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        bottomLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        EdgeCollider2D boundsCollider = GetComponent<EdgeCollider2D>();
        boundsCollider.points =  new Vector2[]{ topLeftPoint, bottomLeftPoint, bottomRightPoint, topRightPoint, topLeftPoint };
    }

    // TODO: Maybe do a better way of preventing overshooting of bubbles over edge boundaries
    public void CheckBounds(Bubble bubble)
    {
        if (bubble.transform.position.x < topLeftPoint.x || bubble.transform.position.x > topRightPoint.x || 
            bubble.transform.position.y > topLeftPoint.y || bubble.transform.position.y < bottomLeftPoint.y)
        {
            BubbleSpawner.Instance.ResetBubble(bubble);
        }
    }



    public void PopOne() // reduce amount of bubbles you need to pop
    {
        bubblesPopped++;
        if (bubblesPopped >= RequiredBubbles)
        {
            CompleteLevel();
        }
    }

    public void StartNextLevel()
    {
        level++;
        bubblesPopped = 0;
        int bubblesToSpawn = RequiredBubbles;
        _bubbleSpawner.ClearBubbles();
        _bubbleSpawner.SpawnBubbles(bubblesToSpawn);
        _message.gameObject.SetActive(false);
        _timer.gameObject.SetActive(true);
        _timer.StartCountdown();
        _startButton.gameObject.SetActive(false);
        _difficulty.gameObject.SetActive(false);
        _title.gameObject.SetActive(false);
    }

    public void CompleteLevel()
    {
        _title.gameObject.SetActive(true);
        _message.gameObject.SetActive(true);
        _message.text = "Level: " + (level + 1);
        _timer.gameObject.SetActive(false);
        _startButton.gameObject.SetActive(true);
        _difficulty.gameObject.SetActive(true);
        SoundManager.Instance.PlayLevelUp();
        _bestScore.UpdateBest(level);


        float diffChange = Random.Range(0, 100f);

        if (diffChange <= 50)
        {
            int spawnAmt = Random.Range(1, 4);
            bubbleAmount+=spawnAmt;
            _difficulty.text = "Bubbles +" + spawnAmt;
        }
        else if (diffChange <= 60)
        {
            BubbleSpawner.Instance.MinBubbleSpeed *= 1.1f;
            _difficulty.text = "Min Speed +10%";
        }
        else if (diffChange <= 70)
        {
            BubbleSpawner.Instance.MaxBubbleSpeed *= 1.1f;
            _difficulty.text = "Max Speed +10%";
        }
        else if (diffChange <= 80)
        {
             BubbleSpawner.Instance.MinBubbleScale *= 0.9f;
            _difficulty.text = "Min Bubble Size -10%";
        }
        else if (diffChange <= 90)
        {
            BubbleSpawner.Instance.MaxBubbleScale *= 0.9f;
            _difficulty.text = "Max Bubble Size -10%";
        }
        else
        {
            _difficulty.text = "FREE ROUND!";
        }
    }

    public void GameOver()
    {
        BubbleSpawner.Instance.ClearBubbles();
        level = 0;
        _message.gameObject.SetActive(true);
        _message.text = "Game Over";
        _startButton.gameObject.SetActive(true);
        BubbleSpawner.Instance.Reset();
        bubbleAmount = 1;
    }


}
