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
            float minHeuristic = float.MaxValue;

            // Calculate the target position (Pac-Man's position)
            Vector3 targetPosition = ghost.target.position;

            // Calculate the total A* heuristic for the current node
            float heuristic = CalculateAStarHeuristic(node, targetPosition);
            Debug.Log(heuristic);

            // Find the available direction with the lowest heuristic value
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Calculate the new position in this direction
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);

                // Calculate the A* heuristic for this direction
                float newHeuristic = CalculateAStarHeuristic(node, newPosition, targetPosition);
                Debug.Log(newHeuristic);
                // Update the heuristic value if this direction is better
                if (newHeuristic < minHeuristic)
                {
                    direction = availableDirection;
                    minHeuristic = newHeuristic;
                    
                }
            }
           

            // Only change direction if the new heuristic is significantly better than the current one
            if (minHeuristic > heuristic)
            {
                ghost.movement.SetDirection(direction);
            }
            else{
                Disable();
            }
        }
    }

    // Calculate the A* heuristic for a given node and target position
    private float CalculateAStarHeuristic(Node node, Vector3 targetPosition)
    {
        Vector3 delta = node.transform.position - targetPosition;
        return Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
    }

    // Calculate the A* heuristic for a new position and target position
    private float CalculateAStarHeuristic(Node node, Vector3 newPosition, Vector3 targetPosition)
    {
        Vector3 delta = newPosition - targetPosition;
        return Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
    }
}
