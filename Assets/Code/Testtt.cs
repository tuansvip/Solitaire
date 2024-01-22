using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Testtt : MonoBehaviour
{
    private int id = 52;
    private int width = 71;
    private int height = 96;
    private float cwidth;
    private float cheight;
    private float cwidthhalf;
    private float cheighthalf;

    private SpriteRenderer spriteRenderer;
    public Sprite particleSprite;

    private void Start()
    {
        float dpr = Screen.dpi / 96.0f; // Assuming 96 dpi as standard

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        cwidth = width * Mathf.Round(dpr);
        cheight = height * Mathf.Round(dpr);
        cwidthhalf = cwidth / 2;
        cheighthalf = cheight / 2;

        particleSprite = Resources.Load<Sprite>("ParticleSprite"); // Load your particle sprite here
        spriteRenderer.sprite = particleSprite;

        StartCoroutine(SimulateParticles());
    }

    private IEnumerator SimulateParticles()
    {
        while (true)
        {
            float startX = Random.Range(0f, Screen.width);
            float startY = Random.Range(0f, Screen.height);

            float speedX = Random.Range(-2f, 2f);
            float speedY = Random.Range(-2f, 2f);

            Particle particle = new Particle(id, startX, startY, speedX, speedY, cwidth, cheight, cwidthhalf, cheighthalf, particleSprite, spriteRenderer);
            particle.StartParticleCoroutine(); // Gọi phương thức Coroutine từ Particle

            yield return null;
        }
    }
}

public class Particle
{
    private int id;
    private float x;
    private float y;
    private float sx;
    private float sy;

    private float cwidth;
    private float cheight;
    private float cwidthhalf;
    private float cheighthalf;

    private SpriteRenderer spriteRenderer;
    private Sprite particleSprite;

    public Particle(int id, float x, float y, float sx, float sy, float cwidth, float cheight, float cwidthhalf, float cheighthalf, Sprite particleSprite, SpriteRenderer spriteRenderer)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.sx = sx == 0 ? 2 : sx;
        this.sy = sy;
        this.cwidth = cwidth;
        this.cheight = cheight;
        this.cwidthhalf = cwidthhalf;
        this.cheighthalf = cheighthalf;
        this.spriteRenderer = spriteRenderer;
        this.particleSprite = particleSprite;
    }

    public void StartParticleCoroutine()
    {
        MonoBehaviour mb = spriteRenderer.gameObject.GetComponent<MonoBehaviour>();
        mb.StartCoroutine(UpdateParticle());
    }

    private IEnumerator UpdateParticle()
    {
        while (true)
        {
            x += sx;
            y += sy;

            if (x < (-cwidthhalf) || x > (Screen.width + cwidthhalf))
            {
                GameObject.Destroy(spriteRenderer.gameObject);
                yield break;
            }

            if (y > Screen.height - cheighthalf)
            {
                y = Screen.height - cheighthalf;
                sy = -sy * 0.85f;
            }

            sy += 0.98f;

            spriteRenderer.sprite = particleSprite;
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
            spriteRenderer.size = new Vector2(cwidth, cheight);
            spriteRenderer.transform.position = new Vector3(x, y, 0);

            yield return null;
        }
    }
}
