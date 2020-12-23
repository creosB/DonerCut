﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sevval : MonoBehaviour
{
    private const float GRAVITY = 2.0f;

    public bool IsActive { set; get; }
    public SpriteRenderer dRenderer;

    private float verticalVelocity;
    private float speed;
    private bool isKesmek= false;

    public Sprite[] sprites;
    private int spriteIndex;
    private float lastSpriteUpdate;
    private float spriteUpdateDelta = 0.125f;
    private float rotationSpeed;

    public void LaunchSevval(float verticalVelocity, float xSpeed, float xStart)
    {
        IsActive = true;
        speed = xSpeed;
        this.verticalVelocity = verticalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        rotationSpeed = Random.Range(-180,180f);
        isKesmek = false;
        spriteIndex = 0;
        dRenderer.sprite = sprites[spriteIndex];
    }

    private void Update()
    {
        if (!IsActive)
            return;

        verticalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, verticalVelocity, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0,0,rotationSpeed) * Time.deltaTime);
        if (isKesmek)
        {
            if(spriteIndex != sprites.Length-1 && Time.time - lastSpriteUpdate > spriteUpdateDelta)
            {
                lastSpriteUpdate = Time.time;
                spriteIndex++;
                dRenderer.sprite = sprites[spriteIndex];
            }
        }

        if (transform.position.y < -1)
        {
            IsActive = false;
            if (!isKesmek)
                SevvalManager.Instance.LoseLP();
        }
    }

    public void Kesildi()
    {
        if(isKesmek)
            return;

        if (verticalVelocity < 0.5f)
            verticalVelocity = 0.5f;

        speed = speed * 0.5f;
        isKesmek = true;

        SevvalManager.Instance.IncrementScore(1);
    }
}