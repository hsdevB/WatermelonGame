using UnityEngine;
using System.Collections;

public class Fruit : MonoBehaviour
{
    public int fruitLevel = 1;
    public bool isDropped = false;
    public bool isMerged = false;
    private bool hasLanded = false;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.mass = 1f;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Fruit other = collision.gameObject.GetComponent<Fruit>();

        if (other == null) return;

        if (this.isDropped && other.isDropped &&
            !this.isMerged && !other.isMerged &&
            this.fruitLevel == other.fruitLevel)
        {
            isMerged = true;
            other.isMerged = true;

            StartCoroutine(MergeWith(other));
        }
        if (isDropped && !hasLanded)
        {
            hasLanded = true;
            Invoke(nameof(NotifyLanding), 0.5f);
        }
    }
    
    void NotifyLanding()
    {
        if (transform.position.y > GameManager.Instance.gameOverHeight)
        {
            GameManager.Instance?.GameOver();
        }
    }

    private IEnumerator MergeWith(Fruit other)
    {
        yield return new WaitForSeconds(0.05f);

        Vector3 spawnPos = (transform.position + other.transform.position) / 2f;

        GameObject nextFruitPrefab = FruitSpawner.Instance.GetFruitByLevel(fruitLevel + 1);
        if (nextFruitPrefab != null)
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, spawnPos, Quaternion.identity);

            Fruit newFruitScript = newFruit.GetComponent<Fruit>();
            if (newFruitScript != null)
            {
                newFruitScript.fruitLevel = this.fruitLevel + 1;
                newFruitScript.isDropped = true;
                newFruitScript.isMerged = false;
            }

            Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                
                Vector2 bounceForce = new Vector2(Random.Range(-0.1f, 0.1f), 0.1f);
                rb.AddForce(bounceForce * 1.2f, ForceMode2D.Impulse);
            }
        }

        ScoreManager.Instance?.AddScore(fruitLevel * 10);

        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }
}