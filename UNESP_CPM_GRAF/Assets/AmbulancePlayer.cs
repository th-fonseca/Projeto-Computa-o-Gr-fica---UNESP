using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryMove : MonoBehaviour
{
    public GameObject wheel_frontRight;
    public GameObject wheel_frontLeft;
    public GameObject wheel_backRight;
    public GameObject wheel_backLeft;

    public WheelCollider W_FR;
    public WheelCollider W_FL;
    public WheelCollider W_BR;
    public WheelCollider W_BL;

    public float speed = 1000f;
    public float lowestSpeed = 20f;
    public float lowestAngle = 70f;
    public float highestAngle = 40f;
    public float steerSmoothness = 10f;  // Taxa de suaviza��o da dire��o
    public float steerMultiplier = 2f;  // Multiplicador para a resposta do volante
    public float accelerationFactor = 0.5f;  // Fator de acelera��o ao virar
    public float rotationSpeed = 100f;   // Velocidade de rota��o adicional para curvas

    private float currentSteerAngle = 0f;  // Armazena o �ngulo de dire��o atual

    void Start()
    {

    }

    void FixedUpdate()
    {
        carMove();
    }

    void carMove()
    {
        // Aplica torque nas rodas traseiras para o movimento em linha reta
        W_BL.motorTorque = speed * Input.GetAxis("Vertical");
        W_BR.motorTorque = speed * Input.GetAxis("Vertical");

        // Calcula o fator de velocidade para o �ngulo de dire��o
        float speedFactor = this.GetComponent<Rigidbody>().velocity.magnitude / lowestSpeed;

        // Calcula o �ngulo de dire��o alvo considerando a acelera��o
        float targetAngle = Mathf.Lerp(lowestAngle, highestAngle, speedFactor) * Input.GetAxis("Horizontal") * steerMultiplier;

        // Reduz a influ�ncia do �ngulo alvo quando acelerando
        if (Input.GetAxis("Vertical") > 0)
        {
            targetAngle *= accelerationFactor;  // Diminui a resposta do �ngulo ao acelerar
        }

        // Suaviza a transi��o entre o �ngulo atual e o �ngulo alvo
        currentSteerAngle = Mathf.LerpAngle(currentSteerAngle, targetAngle, steerSmoothness * Time.deltaTime);

        // Aplica o �ngulo suavizado nas rodas dianteiras
        W_FL.steerAngle = currentSteerAngle;
        W_FR.steerAngle = currentSteerAngle;

        // Adiciona uma rota��o suave ao ve�culo nas curvas (opcional)
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            // Gira o ve�culo diretamente quando est� virando, baseado no input horizontal
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        }
    }
}
