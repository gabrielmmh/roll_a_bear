using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaipanAI : MonoBehaviour
{
    public Transform player; // Referência ao jogador (urso)
    public Animator animator;  // Referência ao Animator da cobra

    public bool damageTrigger = false; // Trigger para acionar o dano no PlayerHud

    public float detectionRadius = 10f; // Raio de detecção da cobra
    public float attackRadius = 2f; // Raio de ataque da cobra
    public float moveSpeed = 3f; // Velocidade de movimento constante da cobra

    private bool isFollowingPlayer = false;
    private bool isAttackingPlayer = false;
    private bool canAttack = true;  // Controla se a cobra pode iniciar um novo ataque
    private bool attackInProgress = false;  // Verifica se o ataque já está em andamento
    private bool playerInAttackRange = false;  // Verifica se o jogador está no raio de ataque

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Cobra segue o jogador se ele estiver no raio de detecção, mas fora do raio de ataque
        if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius && !attackInProgress)
        {
            isFollowingPlayer = true;
            isAttackingPlayer = false;
            playerInAttackRange = false; // Jogador está fora do raio de ataque
            canAttack = true;  // Cobra pode voltar a atacar se o jogador sair e reentrar
        }
        // Cobra entra em modo de ataque se o jogador estiver dentro do raio de ataque
        else if (distanceToPlayer <= attackRadius && canAttack && !attackInProgress)
        {
            isAttackingPlayer = true;
            isFollowingPlayer = false;
            attackInProgress = true;  // O ataque começou
            canAttack = false;  // Previne múltiplos ataques simultâneos
            playerInAttackRange = true; // Jogador está no raio de ataque

            // Debug.Log("Cobra iniciou ataque ao jogador.");
        }
        // Cobra para de seguir o jogador se ele sair do raio de detecção
        else if (distanceToPlayer > detectionRadius)
        {
            isFollowingPlayer = false;
            isAttackingPlayer = false;
            playerInAttackRange = false;
            canAttack = true;  // Cobra pode voltar a atacar se o jogador retornar ao raio de detecção
        }
        // Cobra para de atacar se o jogador sair do raio de ataque
        else if (distanceToPlayer > attackRadius && playerInAttackRange)
        {
            // Jogador saiu do raio de ataque, então o ataque é interrompido
            isAttackingPlayer = false;
            attackInProgress = false;
            playerInAttackRange = false;
            canAttack = true;  // Cobra pode voltar a atacar se o jogador retornar ao raio de ataque
            // Debug.Log("Jogador saiu do raio de ataque, ataque interrompido.");
        }

        if (isFollowingPlayer && !attackInProgress)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        transform.position = newPosition;
        transform.forward = Vector3.Slerp(transform.forward, directionToPlayer, 0.1f);
    }

    // Função chamada ao fim da animação de ataque para liberar um novo ataque e acionar o trigger de dano
    public void AttackCompleted()
    {
        if (playerInAttackRange)
        {
            damageTrigger = true; // Ativa o trigger de dano somente se o jogador ainda estiver no raio de ataque
        }
        else
        {
            damageTrigger = false;
        }

        isAttackingPlayer = false;
        attackInProgress = false;
        playerInAttackRange = false;
        canAttack = true;  // Cobra pode voltar a atacar se o jogador retornar ao raio de ataque
    }

    public bool IsWalking()
    {
        return isFollowingPlayer;
    }

    public bool IsAttacking()
    {
        return isAttackingPlayer;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Raio de detecção
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius); // Raio de ataque
    }
}
