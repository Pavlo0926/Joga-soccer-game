using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class FST_DynamicScrollFitter : MonoBehaviour
{
    public bool expandX, expandY = true;

    RectTransform myTransform;
    GridLayoutGroup layoutGroup;

    Vector2 cellSize;
    RectOffset padding;

    Vector2 newScale;

    // Use this for initialization
    void Start()
    {

        myTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<GridLayoutGroup>();
        cellSize = layoutGroup.cellSize;
        padding = layoutGroup.padding;

    }

    void OnGUI()
    {
        newScale = myTransform.sizeDelta;

        if (expandX) newScale.x = padding.left + padding.right + ((cellSize.x + layoutGroup.spacing.x) * transform.childCount);
        if (expandY) newScale.y = padding.top + padding.bottom + ((cellSize.y + layoutGroup.spacing.y) * transform.childCount);

        myTransform.sizeDelta = newScale;
    }
}