using UnityEngine;
using UnityEngine.UI;
public class CustomSpriteAnimator : MonoBehaviour
{
    public Image targetImage;
    public Sprite[] frames;
    public float frameRate = 10f;
    private int currentFrame;
    private float timer;
    void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length;
            targetImage.sprite = frames[currentFrame];
        }
    }
}
