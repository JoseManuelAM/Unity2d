using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private static int lives = 3;
    [SerializeField] private Text livesText;
    [SerializeField] private GameObject gameOverPanel;
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource deathSoundEffect;

    private void Awake()
    {
        if (livesText != null)
        {
            livesText.text = "x" + lives;
        }
        gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    public void Die()
    {
        deathSoundEffect.Play();
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        lives--;
        livesText.text = "x" + lives;

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        lives = 3;
        livesText.text = "x" + lives;
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RestartLevel()
    {
        if (lives > 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

