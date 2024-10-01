using UnityEngine;

public class KeepCarOnGround : MonoBehaviour
{
    private Rigidbody rb;
    public float groundDistance = 0.5f; // Dist�ncia para verificar a colis�o com o ch�o
    public LayerMask groundLayer; // Layer que representa o ch�o

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Verifica a dist�ncia at� o ch�o
        if (!IsGrounded())
        {
            // Ajusta a posi��o do carro se n�o estiver no ch�o
            AdjustCarPosition();
        }
    }

    private bool IsGrounded()
    {
        // Verifica se o carro est� tocando o ch�o
        return Physics.Raycast(transform.position, Vector3.down, groundDistance, groundLayer);
    }

    private void AdjustCarPosition()
    {
        // Move o carro para baixo na dire��o do ch�o
        Vector3 newPosition = transform.position;
        newPosition.y = GetGroundHeight();
        transform.position = newPosition;
    }

    private float GetGroundHeight()
    {
        // Faz um Raycast para obter a altura do ch�o abaixo do carro
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance, groundLayer))
        {
            return hit.point.y;
        }
        return transform.position.y; // Retorna a posi��o atual se n�o encontrar ch�o
    }
}
