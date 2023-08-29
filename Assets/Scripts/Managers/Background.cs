using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public List<Sprite> backgroundItems;
    Sprite selectedSprite;
    public float scrollSpeed = 10.0f;
    public float backgroundHeight = -30.0f;
    public Vector3 topPosition;
    private GameObject backgroundSprite; // Reference to the previously created child

    private void Start()
    {
        // Randomly select a sprite from the list
        int randomIndex = Random.Range(0, backgroundItems.Count);
        selectedSprite = backgroundItems[randomIndex];

        // Create a new GameObject
        backgroundSprite = new GameObject("BackgroundSprite");

        // Attach a SpriteRenderer and set the sprite
        SpriteRenderer sr = backgroundSprite.AddComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;

        // Set the transform parent to the current transform
        backgroundSprite.transform.SetParent(transform);

        // Optionally, set the position, rotation, scale of the new GameObject as needed
        backgroundSprite.transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        // Move the background image down at a constant speed
        transform.Translate(-transform.up * scrollSpeed * Time.deltaTime);

        // Reset the position when the background goes off-screen
        if (transform.position.y < backgroundHeight) // whatever the height is of the bg image
        {
            transform.position = topPosition; // new Vector3(transform.position.x, 5.0f, transform.position.z);

            // Randomly select a new sprite from the list
            int randomIndex = Random.Range(0, backgroundItems.Count);
            selectedSprite = backgroundItems[randomIndex];

            // Change the sprite of the backgroundChild
            backgroundSprite.GetComponent<SpriteRenderer>().sprite = selectedSprite;

            // Change the position of the backgroundChild to a random position relative to the parent
            float randomX = Random.Range(-5f, 5f); // Adjust these values based on the desired range
            float randomY = Random.Range(-5f, 5f); // Adjust these values based on the desired range
            backgroundSprite.transform.localPosition = new Vector3(randomX, randomY, 0);
        }
    }
}