using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaipanAnimator : MonoBehaviour
{
    private const string WALK = "Walk";
    private const string ATTACK = "Attack";

    // Lista de todas as cobras (TaipanAI) que o Animator vai controlar
    [SerializeField] private List<TaipanAI> taipans = new List<TaipanAI>();

    private void Update()
    {
        // Itera sobre cada cobra na lista para atualizar suas animações
        foreach (TaipanAI taipan in taipans)
        {
            Animator animator = taipan.GetComponent<Animator>();

            if (animator == null)
                continue;

            bool isWalking = taipan.IsWalking();
            bool isAttacking = taipan.IsAttacking();

            // Verifica se a cobra está atacando e ativa/desativa a animação de ataque
            if (isAttacking)
            {
                animator.SetBool(ATTACK, isAttacking);
                // Debug.Log("Cobra iniciou ataque.");
            }
            else
            {
                animator.SetBool(ATTACK, isAttacking);
                // Debug.Log("Cobra parou de atacar.");
            }
            
            // Debug.Log("Animação de ataque: " + isAttacking);
            // Configura a animação de andar
            animator.SetBool(WALK, isWalking);
        }
    }

    // Função para adicionar uma cobra à lista
    public void AddTaipan(TaipanAI taipan)
    {
        if (!taipans.Contains(taipan))
        {
            taipans.Add(taipan);
        }
    }

    // Função para remover uma cobra da lista (se necessário)
    public void RemoveTaipan(TaipanAI taipan)
    {
        if (taipans.Contains(taipan))
        {
            taipans.Remove(taipan);
        }
    }
}
