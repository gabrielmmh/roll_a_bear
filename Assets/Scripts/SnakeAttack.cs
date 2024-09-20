using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttack : StateMachineBehaviour
{
    // Esse método é chamado quando a animação de ataque começa
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("A animação de ataque foi iniciada.");

        // O ataque pode ser iniciado diretamente ao entrar na animação de ataque, ou no final dela (em OnStateExit)
        TaipanAI taipanAI = animator.GetComponent<TaipanAI>();
        if (taipanAI != null)
        {
            // Aqui você pode adicionar lógica para iniciar a preparação do ataque, se necessário
            Debug.Log("Preparando para o ataque...");
        }
    }

    // Esse método é chamado quando a animação de ataque termina
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("A animação de ataque foi completada.");

        // Confirma que o ataque foi concluído e o jogador deve perder pontos
        TaipanAI taipanAI = animator.GetComponent<TaipanAI>();
        if (taipanAI != null)
        {
            taipanAI.AttackCompleted(); // Chama a função de ataque para finalizar o ataque
        }
    }
}
