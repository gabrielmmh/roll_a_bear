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

    private Vector3 lastPosition; // Armazena a última posição do jogador
    private float stuckTime = 0f; // Tempo que o jogador está preso
    private float maxStuckTime = 5f; // Tempo máximo antes de considerar o jogador preso
    private Vector3 resetPosition; // Posição inicial para reiniciar o jogador

    private float yThreshold = -20f; // Limite de Y para resetar a posição do jogador

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHudScript = GetComponent<PlayerHud>(); // Pega o componente PlayerHud diretamente no jogador

        lastPosition = transform.position; // Define a posição inicial
        resetPosition = transform.position; // Define a posição de reinício como a posição inicial do jogador
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

        // Verifica se o jogador caiu abaixo do limite Y (-20)
        if (transform.position.y < yThreshold)
        {
            Debug.Log("Jogador caiu abaixo do limite Y. Reiniciando a posição...");
            transform.position = resetPosition; // Reseta a posição do jogador
            return; // Sai da função para evitar outros cálculos enquanto reinicia a posição
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

        // Verifica se o jogador está preso
        CheckIfPlayerIsStuck();
    }

    private void CheckIfPlayerIsStuck()
    {
        // Se o jogador se moveu, reinicia o contador de "stuckTime"
        if (Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            stuckTime = 0f;
            lastPosition = transform.position; // Atualiza a última posição do jogador
        }
        else
        {
            stuckTime += Time.deltaTime; // Incrementa o tempo que o jogador está parado
        }

        // Se o jogador está parado por muito tempo, reinicializa a posição
        if (stuckTime >= maxStuckTime)
        {
            Debug.Log("Jogador preso! Reiniciando a posição...");
            transform.position = resetPosition; // Reseta a posição do jogador
            stuckTime = 0f; // Reseta o contador de tempo
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
