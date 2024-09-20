using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    public float speed = 0;

    public bool walkForward;
    public bool walkBackward;
    public bool runForward;

    public Transform cameraTransform; // Referência à câmera para pegar sua rotação

    private PlayerHud playerHudScript; // Referência ao script PlayerHud no próprio jogador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHudScript = GetComponent<PlayerHud>(); // Pega o componente PlayerHud diretamente no jogador
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        // Verifica se o timer está rodando; se não, o movimento e as animações são bloqueados
        if (playerHudScript != null && !playerHudScript.timerIsRunning)
        {
            movementX = 0;
            movementY = 0;
            walkForward = false;
            walkBackward = false;
            runForward = false;
            return; // Impede o movimento e para as animações se o timer não estiver rodando
        }

        // Pega a direção da câmera no plano XZ (ignorando a inclinação da câmera)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // Ignora a componente Y para garantir que o movimento seja apenas no plano XZ
        cameraForward.Normalize(); // Normaliza para garantir magnitude correta

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f; // Ignora a componente Y
        cameraRight.Normalize();

        // Calcula a direção do movimento com base na orientação da câmera
        Vector3 movement = (cameraForward * movementY + cameraRight * movementX).normalized;

        // Move o jogador na direção calculada
        transform.position += movement * speed * Time.deltaTime;

        float dotProduct = Vector3.Dot(transform.forward, movement);

        walkForward = dotProduct > 0.1f && movement.magnitude > 0;
        walkBackward = dotProduct < -0.1f && movement.magnitude > 0;

        if (Input.GetKey(KeyCode.LeftShift) && walkForward)
        {
            runForward = true;
            speed = 10;
        }
        else
        {
            runForward = false;
            speed = 3;
        }

        if (movement != Vector3.zero)
        {
            float rotateSpeed = 10f;
            // Faz o jogador rotacionar na direção do movimento
            transform.forward = Vector3.Slerp(transform.forward, movement, rotateSpeed * Time.deltaTime);
        }
    }

    public bool WalkForward()
    {
        return walkForward;
    }

    public bool WalkBackward()
    {
        return walkBackward;
    }

    public bool RunForward()
    {
        return runForward;
    }
}
