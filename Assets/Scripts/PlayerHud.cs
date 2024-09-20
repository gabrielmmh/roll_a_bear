using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class PlayerHud : MonoBehaviour
{
    public TextMeshProUGUI pointsText;   // Exibe a pontuação do jogador
    public TextMeshProUGUI timeText;     // Exibe o tempo restante
    public GameObject timesUpTextObject; // Texto de "Time's Up"

    public TextMeshProUGUI plusPointsText;    // Texto de pontos ganhos
    public TextMeshProUGUI minusPointsText;   // Texto de pontos perdidos
    public GameObject snakeIcon;         // Ícone de cobra
    public GameObject watermelonIcon;    // Ícone de melancia

    public GameObject restartButton;     // Botão de reiniciar
    public GameObject menuButton;        // Botão de menu

    public Transform player; // Referência ao jogador (urso)

    public GameObject taipanParent;  // GameObject pai de vários Taipan's (Prefabs de cobras)
    private TaipanAI[] taipanInstances;  // Array de todas as instâncias de TaipanAI nos filhos

    public AudioClip watermelonBite1;  // Som de mordida na melancia 1
    public AudioClip watermelonBite2;  // Som de mordida na melancia 2
    public AudioClip watermelonBite3;  // Som de mordida na melancia 3
    public AudioClip watermelonBite4;  // Som de mordida na melancia 4

    public AudioClip damage1;         // Som de dano 1
    public AudioClip damage2;         // Som de dano 2
    public AudioClip damage3;         // Som de dano 3
    public AudioClip damage4;         // Som de dano 4

    AudioSource audioSource;

    public int points = 0;  // Placar de pontos
    private float timeRemaining = 60f;  // 1 minuto
    public bool timerIsRunning = true; // Controle do estado do timer

    void Start()
    {
        SetPointsText();
        timesUpTextObject.SetActive(false); // Esconde o texto de "Time's Up" no início
        plusPointsText.gameObject.SetActive(false);  // Esconde o texto de pontos ganhos
        minusPointsText.gameObject.SetActive(false);  // Esconde o texto de pontos perdidos
        snakeIcon.SetActive(false);  // Esconde o ícone de cobra
        watermelonIcon.SetActive(false);  // Esconde o ícone de melancia
        restartButton.SetActive(false);  // Esconde o botão de reiniciar

        taipanInstances = taipanParent.GetComponentsInChildren<TaipanAI>();

        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartTimer()); // Inicia a contagem regressiva
    }

    void Update()
    {
        // Verifica os triggers de dano de todas as instâncias de TaipanAI
        foreach (var taipan in taipanInstances)
        {
            if (taipan.damageTrigger)
            {
                int randomDamageSound = Random.Range(1, 5);
                switch (randomDamageSound)
                {
                    case 1:
                        audioSource.PlayOneShot(damage1);
                        break;
                    case 2:
                        audioSource.PlayOneShot(damage2);
                        break;
                    case 3:
                        audioSource.PlayOneShot(damage3);
                        break;
                    case 4:
                        audioSource.PlayOneShot(damage4);
                        break;
                }

                SetPoints(-5);  // Diminui os pontos
                ShowPoints("snake");  // Exibe o ícone de cobra e o texto de pontos perdidos
                StartCoroutine(HidePointsAfterSeconds(1f));  // Esconde o ícone e o texto após 1 segundo
                taipan.damageTrigger = false;  // Reseta o trigger após a aplicação do dano
            }
        }
    }

    public void SetPoints(int newPoints)
    {
        points += newPoints;
        if (points < 0)
        {
            points = 0;
        }
        // Debug.Log("points: " + points);
        SetPointsText();
    }

    public void ShowPoints(string type)
    {
        if (type == "snake")
        {
            snakeIcon.SetActive(true);
            minusPointsText.text = "-5";
            minusPointsText.gameObject.SetActive(true);
        }
        else if (type == "watermelon")
        {
            watermelonIcon.SetActive(true);
            plusPointsText.text = "+5";
            plusPointsText.gameObject.SetActive(true);
        }
    }	

    public void HidePoints()
    {
        snakeIcon.SetActive(false);
        watermelonIcon.SetActive(false);
        plusPointsText.gameObject.SetActive(false);
        minusPointsText.gameObject.SetActive(false);
    }

    private IEnumerator HidePointsAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HidePoints();
    }

    private void SetPointsText()
    {
        pointsText.text = "Points: " + points.ToString();
    }

    private IEnumerator StartTimer()
    {
        while (timeRemaining > 0 && timerIsRunning)
        {
            UpdateTimerDisplay();
            yield return new WaitForSeconds(1f);  // Aguarda 1 segundo
            timeRemaining--;
        }

        if (timeRemaining <= 0)
        {
            TimeUp();  // Chama a função quando o tempo acaba
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimeUp()
    {
        timerIsRunning = false;
        timeText.text = "00:00";
        timesUpTextObject.SetActive(true); // Exibe o texto de "Time's Up"
        restartButton.SetActive(true);  // Exibe o botão de reiniciar
    }

    // Detecta colisões com objetos como melancias e cobra
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Watermelon"))
        {
            int randomWatermelonBite = Random.Range(1, 5);

            switch (randomWatermelonBite)
            {
                case 1:
                    audioSource.PlayOneShot(watermelonBite1);
                    break;
                case 2:
                    audioSource.PlayOneShot(watermelonBite2);
                    break;
                case 3:
                    audioSource.PlayOneShot(watermelonBite3);
                    break;
                case 4:
                    audioSource.PlayOneShot(watermelonBite4);
                    break;
            }

            other.gameObject.SetActive(false);
            SetPoints(5);  // Ganha 5 pontos ao pegar uma melancia
            ShowPoints("watermelon");
            StartCoroutine(HidePointsAfterSeconds(1f));
        }
    }
}
