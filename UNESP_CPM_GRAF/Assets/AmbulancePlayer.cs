using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Importar para usar UI

public class AmbulancePlayer : MonoBehaviour
{
    public float moveSpeed = 10f;         // Velocidade de movimento do carro
    public float rotationSpeed = 50f;     // Velocidade de rota��o do carro
    public float maxSpeed = 20f;          // Velocidade m�xima do carro
    public float acceleration = 35f;      // Taxa de acelera��o
    public float deceleration = 35f;      // Taxa de desacelera��o (ajustada para ser mais realista)
    public float reverseDeceleration = 30f; // Taxa de desacelera��o quando a dire��o � oposta (ajustada para ser mais realista)
    public float turnSmoothness = 2f;     // Suavidade da rota��o
    public float minTurningSpeed = 5f;    // Velocidade m�nima para come�ar a virar
    public float turnTransitionSpeed = 1f; // Velocidade da transi��o para a rota��o

    public RectTransform speedometerNeedle; // Refer�ncia � agulha do veloc�metro

    public AudioClip engineSound;          // Som do motor
    public AudioSource audioSource;       // Refer�ncia ao componente AudioSource

    private float currentSpeed = 0f;      // Velocidade atual do carro
    private float turnInput = 0f;         // Entrada para rota��o
    private bool isMoving = false;        // Indica se o carro est� se movendo

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = engineSound;
        audioSource.loop = true; // Define o som como um loop
        audioSource.Play(); // Inicia o som do motor
    }

    private void Update()
    {
        PlayerMovement();
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        UpdateMeters();
        UpdateEngineSound();
    }

    void PlayerMovement()
    {
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        isMoving = moveZ != 0;

        MoveVertically(moveZ);
        MoveHorizontally(moveX);
    }

    void MoveVertically(float moveZ)
    {
        if (moveZ != 0)
        {
            if (Mathf.Sign(moveZ) != Mathf.Sign(currentSpeed) && currentSpeed != 0)
            {
                // Frear mais suavemente se a dire��o for oposta
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, reverseDeceleration * Time.deltaTime);
            }
            else
            {
                // Aumenta ou diminui a velocidade com base na entrada do jogador
                currentSpeed += moveZ * acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            }
        }
        else
        {
            // Desacelera quando n�o h� entrada
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }

        // Atualiza o estado de movimenta��o
        isMoving = currentSpeed != 0;
    }

    void MoveHorizontally(float moveX)
    {
        // Ajusta o input para rota��o
        if (Mathf.Abs(currentSpeed) >= minTurningSpeed || currentSpeed == 0)
        {
            // Calcula a velocidade atual proporcional para a rota��o
            float turnSpeed = Mathf.Clamp01((Mathf.Abs(currentSpeed) - minTurningSpeed) / (maxSpeed - minTurningSpeed));

            if (moveX != 0)
            {
                // Atualiza a entrada de rota��o
                turnInput = Mathf.Lerp(turnInput, moveX, turnSmoothness * Time.deltaTime * turnSpeed);
            }
            else
            {
                // Gradualmente retorna a rota��o para zero quando n�o est� virando
                turnInput = Mathf.Lerp(turnInput, 0, turnSmoothness * Time.deltaTime);
            }

            // Rotaciona o carro suavemente com base na velocidade
            if (currentSpeed != 0)
            {
                float rotationAmount = turnInput * rotationSpeed * Time.deltaTime * Mathf.Sign(currentSpeed);
                transform.Rotate(Vector3.up * rotationAmount);
            }
        }
        else
        {
            // Quando n�o est� se movendo ou a velocidade est� abaixo do limite de rota��o, retorna a rota��o para zero
            turnInput = Mathf.Lerp(turnInput, 0, turnSmoothness * Time.deltaTime);
        }
    }

    void UpdateMeters()
    {
        // Atualiza o �ngulo da agulha do veloc�metro com base na velocidade
        if (speedometerNeedle != null)
        {
            // Converte a velocidade em um �ngulo (0 a 180 graus, ajust�vel)
            float needleAngle = Mathf.Lerp(0f, -270f, Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(currentSpeed)));
            speedometerNeedle.localRotation = Quaternion.Euler(0f, 0f, needleAngle);
        }
    }

    void UpdateEngineSound()
    {
        if (audioSource != null && engineSound != null)
        {
            // Ajusta o pitch do �udio com base na velocidade
            audioSource.pitch = Mathf.Lerp(1f, 2f, Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(currentSpeed)));
        }
    }
}
