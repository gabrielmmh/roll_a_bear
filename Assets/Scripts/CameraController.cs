using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // Referência ao jogador
    public float rotationSpeed = 5f; // Velocidade de rotação horizontal da câmera
    public float verticalSpeed = 3f; // Velocidade de rotação vertical da câmera
    private Vector3 offset; // Deslocamento da câmera em relação ao jogador

    private float currentRotationX = 0f; // Para limitar a rotação vertical

    void Start()
    {
        // Calcula o deslocamento inicial entre a câmera e o jogador
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // Entrada do mouse (eixos X e Y)
        float horizontalMouseInput = Input.GetAxis("Mouse X");
        float verticalMouseInput = Input.GetAxis("Mouse Y");

        // Entrada do controle (analógico direito)
        // float horizontalRightStickInput = Input.GetAxis("Horizontal");
        // float verticalRightStickInput = Input.GetAxis("Vertical");

        // Combina a entrada de controle e mouse para ambos os eixos
        // float horizontalInput = horizontalMouseInput + horizontalRightStickInput;
        float horizontalInput = horizontalMouseInput;
        // float verticalInput = verticalMouseInput + verticalRightStickInput;
        float verticalInput = verticalMouseInput ;

        // Rotaciona horizontalmente ao redor do jogador com base no mouse ou controle
        offset = Quaternion.AngleAxis(horizontalInput * rotationSpeed, Vector3.up) * offset;

        // Controla a rotação vertical, limitando para evitar uma rotação completa
        currentRotationX -= verticalInput * verticalSpeed;
        currentRotationX = Mathf.Clamp(currentRotationX, -30f, 45f); // Limita o ângulo da câmera (cima e baixo)

        // Aplica a rotação vertical
        Vector3 cameraPosition = player.transform.position + offset;
        cameraPosition.y = player.transform.position.y + Mathf.Sin(Mathf.Deg2Rad * currentRotationX) * offset.magnitude;
        transform.position = cameraPosition;

        // Faz a câmera olhar para o jogador
        transform.LookAt(player.transform.position + Vector3.up * 1.5f); // Foca levemente acima do jogador
    }
}
