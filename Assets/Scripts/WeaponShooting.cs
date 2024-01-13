using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private LineRenderer lineRendererPrefab;
    [SerializeField] private ParticleSystem barrelEffect;
    private bool canShoot = false;
    private List<LineRenderer> lineRendererPool = new List<LineRenderer>();
    private int poolSize = 50; // Adjust the pool size based on your needs

    private void Start()
    {
        StartCoroutine(EnableShootingDelayed(1.0f));
        InitializeLineRendererPool();
    }

    private IEnumerator EnableShootingDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private void InitializeLineRendererPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            LineRenderer laser = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
            laser.gameObject.SetActive(false);
            lineRendererPool.Add(laser);
        }
    }

    private void Update()
    {
        if (canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red, 0.1f);

            if (hit.collider.CompareTag("Enemy"))
            {
                PlayerController playerController = FindObjectOfType<PlayerController>();
                if (playerController != null)
                {
                    playerController.ActivateFailedText();
                }

                StopShooting();
            }

            LineRenderer laser = GetPooledLineRenderer();
            laser.gameObject.SetActive(true);
            laser.SetPosition(0, rayOrigin.position);
            laser.SetPosition(1, hit.point);

            // Play particle effect
            ParticleSystem particleEffect = Instantiate(barrelEffect, hit.point, Quaternion.identity);
            particleEffect.Play();

            // "Destroy" the LineRenderer and particle effect by deactivating them and returning them to the pool
            StartCoroutine(ReturnToPoolAfterDelay(laser, particleEffect, 0.15f));
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);

            LineRenderer laser = GetPooledLineRenderer();
            laser.gameObject.SetActive(true);
            laser.SetPosition(0, rayOrigin.position);
            laser.SetPosition(1, rayOrigin.position + rayOrigin.forward * 100f);

            // Play particle effect
            ParticleSystem particleEffect = Instantiate(barrelEffect, rayOrigin.position + rayOrigin.forward * 100f, Quaternion.identity);
            particleEffect.Play();

            // "Destroy" the LineRenderer and particle effect by deactivating them and returning them to the pool
            StartCoroutine(ReturnToPoolAfterDelay(laser, particleEffect, 0.15f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(LineRenderer laser, ParticleSystem particleEffect, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Stop particle effect
        particleEffect.Stop();

        // Deactivate LineRenderer and particle effect
        laser.gameObject.SetActive(false);
        particleEffect.gameObject.SetActive(false);
    }


    private LineRenderer GetPooledLineRenderer()
    {
        foreach (LineRenderer laser in lineRendererPool)
        {
            if (!laser.gameObject.activeInHierarchy)
            {
                return laser;
            }
        }

        // Expand the pool if needed (you can also handle this differently based on your requirements)
        LineRenderer newLaser = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);
        newLaser.gameObject.SetActive(false);
        lineRendererPool.Add(newLaser);

        return newLaser;
    }

    private IEnumerator ReturnToPoolAfterDelay(LineRenderer laser, float delay)
    {
        yield return new WaitForSeconds(delay);
        laser.gameObject.SetActive(false);
    }

    public void StopShooting()
    {
        canShoot = false;
    }
}
