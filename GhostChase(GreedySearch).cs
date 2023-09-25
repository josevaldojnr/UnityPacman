using UnityEngine;

public class GhostChase : GhostBehavior
{
    

    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Do nothing while the ghost is frightened
        if (node != null && this.enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float bestDistance = float.MaxValue;
            Vector3 targetPosition = ghost.target.position;

            // DISTANCIA A PARTIR DO PONTOA ATUAL
            float dx = Mathf.Abs(node.transform.position.x - targetPosition.x);
            float dy = Mathf.Abs(node.transform.position.y - targetPosition.y);
            float newDistance = dx + dy;

            // CALCULA HEURISTICA PRA CADA PONTO DADO DISPONÍVEL NO NODE
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                //SIMULA ESTAR EM MUMA DAS POSIÇOES LIVRES
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);

                // CALCULA HEURISTICA
                dx = Mathf.Abs(newPosition.x - targetPosition.x);
                dy = Mathf.Abs(newPosition.y - targetPosition.y);
                float nodeNewDistance = dx + dy;

                // ATUALIZA SE O RESULTADO FOR MELHOR

                if (nodeNewDistance < bestDistance)
                {
                    direction = availableDirection;
                    bestDistance = nodeNewDistance;
                }
                
            }
            Debug.Log(bestDistance);
            
            if (bestDistance < newDistance)
            {
                ghost.movement.SetDirection(direction);
            }
            else
            {
                Disable();
            }
        
        }
    }
}
