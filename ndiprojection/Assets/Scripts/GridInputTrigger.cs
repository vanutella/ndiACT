using UnityEngine;

public class GridInputTrigger : MonoBehaviour
{
    public UIFlipGridController gridController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (gridController != null)
                gridController.OpenGrid();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (gridController != null)
                gridController.CloseGrid();
        }
    }
}
