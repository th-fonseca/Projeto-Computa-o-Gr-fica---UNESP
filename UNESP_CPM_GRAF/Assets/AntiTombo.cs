using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public Rigidbody rb;           // O Rigidbody do ve�culo
    public float antiRollForce = 5000f;  // A for�a anti-tombamento (ajust�vel)
    public float rollThreshold = 0.3f;   // O limite de inclina��o para ativar o anti-roll

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        ApplyAntiRoll();
    }

    void ApplyAntiRoll()
    {
        // Verificar a inclina��o em torno do eixo Z (rota��o lateral)
        float rollAngle = Mathf.Abs(transform.localEulerAngles.z);

        // Verificar se o carro est� inclinado al�m do limite
        if (rollAngle > rollThreshold && rollAngle < 360 - rollThreshold)
        {
            // Aplica uma for�a corretiva para baixo quando o carro inclinar
            rb.AddForce(-transform.up * antiRollForce * Time.fixedDeltaTime);
        }
    }
}
