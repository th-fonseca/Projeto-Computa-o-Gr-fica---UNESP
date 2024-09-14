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
    public float deceleration = 20f;      // Taxa de desacelera��o (ajustada para ser mais realista)
    public float reverseDeceleration = 30f; // Taxa de desacelera��o quando a dire��o � oposta (ajustada para ser mais realista)
    public float turnSmoothness = 2f;     // Suavidade da rota��o

    public Slider speedSlider;            // Refer�ncia ao Slider de velocidade
    public RectTransform speedometerNeedle; // Refer�ncia � agulha do veloc�metro

    private float currentSpeed = 0f;      // Velocidade atual do carro
    private float turnInput = 0f;         // Entrada para rota��o

    // Update � chamado uma vez por frame
    void Update()
    {
        // Input para o eixo vertical (W e S) para controlar a acelera��o
        float moveZ = Input.GetAxis("Vertical");

        // Se a entrada do jogador estiver na dire��o oposta ao movimento atual, frear mais rapidamente
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

        // Movimenta o carro para frente e para tr�s
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Input para o eixo horizontal (A e D) para controlar a rota��o
        float moveX = Input.GetAxis("Horizontal");

        // Atualiza a entrada de rota��o
        if (moveX != 0 && currentSpeed != 0)
        {
            // Lerp para suavizar a rota��o
            turnInput = Mathf.Lerp(turnInput, moveX, turnSmoothness * Time.deltaTime);
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

        // Atualiza o valor do Slider com a velocidade atual
        if (speedSlider != null)
        {
            speedSlider.value = Mathf.Abs(currentSpeed); // Valor absoluto para mostrar velocidade positiva
        }

        // Atualiza o �ngulo da agulha do veloc�metro com base na velocidade
        if (speedometerNeedle != null)
        {
            // Converte a velocidade em um �ngulo (0 a 180 graus, ajust�vel)
            float needleAngle = Mathf.Lerp(0f, -270f, Mathf.InverseLerp(0f, maxSpeed, Mathf.Abs(currentSpeed)));
            speedometerNeedle.localRotation = Quaternion.Euler(0f, 0f, needleAngle);
        }
    }
}
