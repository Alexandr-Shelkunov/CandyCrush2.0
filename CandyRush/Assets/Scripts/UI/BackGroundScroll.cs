using UnityEngine;

public class InfiniteBackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 50f; 
    private RectTransform[] backgroundImages; 
    private float imageHeight;

    void Start()
    {
        backgroundImages = new RectTransform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            var rectTransform = transform.GetChild(i).GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                backgroundImages[i] = rectTransform;
            }
            else
            {
                Debug.LogError($"Child {transform.GetChild(i).name} does not have a RectTransform component!");
            }
        }

        if (backgroundImages.Length > 0)
        {
            imageHeight = backgroundImages[0].rect.height;
        }
        else
        {
            Debug.LogError("No background images found!");
        }
    }

    void Update()
    {
        foreach (var bg in backgroundImages)
        {
            if (bg == null) continue;

            bg.anchoredPosition -= new Vector2(0, scrollSpeed * Time.deltaTime);

            if (bg.anchoredPosition.y <= -imageHeight)
            {
                bg.anchoredPosition += new Vector2(0, imageHeight * backgroundImages.Length);
            }
        }
    }
}