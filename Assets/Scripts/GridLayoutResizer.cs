using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutResizer : MonoBehaviour
{

    public Vector2 targetResolution = new Vector2(1920, 1080);
    public Vector2 cellSize         =     Vector2.one * 100f;

    public Vector2Int screenSize;
    public Vector2 screenSafeSize;

    GridLayoutGroup grid;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    public float scale;

    
    void OnEnable()
    {
        
        

    }
}
