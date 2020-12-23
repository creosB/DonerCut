using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Vegetable : MonoBehaviour
{
    private const float GRAVITY = 2.0f;

    public bool IsActive { set; get; }
    public SpriteRenderer sRenderer;

    private float verticalVelocity;
    private float speed;
    private bool isSliced = false;

    public Sprite[] sprites;
    private int spriteIndex;
    private float lastSpriteUpdate;
    private float spriteUpdateDelta = 0.125f;
    private float rotationSpeed;

    public void LaunchVegetable(float verticalVelocity, float xSpeed, float xStart)
    {
        IsActive = true;
        speed = xSpeed;
        this.verticalVelocity = verticalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        rotationSpeed = Random.Range(-180,180);
        isSliced = false;
        spriteIndex = 0;
        sRenderer.sprite = sprites[spriteIndex];
    }

    private void Update()
    {
        if (!IsActive)
            return;

        verticalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, verticalVelocity, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0,0,rotationSpeed) * Time.deltaTime);
        if (isSliced)
        {
            if(spriteIndex != sprites.Length-1 && Time.time - lastSpriteUpdate > spriteUpdateDelta)
            {
                lastSpriteUpdate = Time.time;
                spriteIndex++;
                sRenderer.sprite = sprites[spriteIndex];
            }
        }

        if (transform.position.y < -1)
        {
            IsActive = false;
            if (!isSliced)
                GameManager.Instance.LoseLP();
        }
    }

    public void Slice()
    {
        if(isSliced)
            return;

        if (verticalVelocity < 0.5f)
            verticalVelocity = 0.5f;

        speed = speed * 0.5f;
        isSliced = true;

        GameManager.Instance.IncrementScore(1);
    }
}