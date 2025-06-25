using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner Instance;
    public GameObject[] fruitPrefabs;
    public Transform spawnPoint;
    private Transform leftWall;
    private Transform rightWall;

    private GameObject currentFruit;
    public float spawnY = 5f;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        leftWall = GameObject.Find("WallLeft")?.transform;
        rightWall = GameObject.Find("WallRight")?.transform;
        
        // SpawnNewFruit();
        if (GameManager.Instance != null && GameManager.Instance.IsGameStarted())
        {
            SpawnNewFruit();
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsGameStarted()) return;
        
        if (Camera.main == null) return;
        
        if (leftWall == null || rightWall == null) return;
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float leftWallX = GameObject.Find("WallLeft").transform.position.x;
        float rightWallX = GameObject.Find("WallRight").transform.position.x;
        float clampedX = Mathf.Clamp(mousePos.x, leftWallX + 4f, rightWallX - 4f);
        Vector3 spawnPos = new Vector3(clampedX, spawnY, 0);
        
        if (currentFruit != null)
        {
            currentFruit.transform.position = spawnPos;
            
            if (Input.GetMouseButtonDown(0))
            {
                DropFruit();
            }
        }
    }

    public void SpawnNewFruit()
    {
        Vector3 startPos = new Vector3(0, spawnY + 1f, 0);
        int randomIndex = Random.Range(0, 4);
        currentFruit = Instantiate(fruitPrefabs[randomIndex], startPos, Quaternion.identity);
        
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // rb.bodyType = RigidbodyType2D.Dynamic;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        Fruit fruitScript = currentFruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.isDropped = false;
            fruitScript.isMerged = false;
        }
    }

    void DropFruit()
    {
        if (currentFruit == null) return;
        
        Fruit fruitScript = currentFruit.GetComponent<Fruit>();
        if (fruitScript != null)
        {
            fruitScript.isDropped = true;
            fruitScript.isMerged = false;
        }
        
        Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(0, -12f);
        }

        currentFruit = null;

        Invoke(nameof(SpawnNewFruit), 0.5f);
    }
    
    public GameObject GetFruitByLevel(int level)
    {
        if (level - 1 >= 0 && level - 1 < fruitPrefabs.Length)
        {
            return fruitPrefabs[level - 1];
        }
        return null; 
    }
}