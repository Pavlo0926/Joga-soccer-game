using UnityEngine;

public class FST_DisableIfLeague : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GameObject[] Objects;
#pragma warning restore CS0649
    void Update()
    {
        bool b = string.IsNullOrEmpty(GameManager.CurrentLeagueID);

        for (int o = 0; o < Objects.Length; o++)
            if (Objects[o].activeSelf != b)
                Objects[o].SetActive(b);
    }
}