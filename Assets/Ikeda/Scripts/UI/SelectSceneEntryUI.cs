using UnityEngine;

public class SelectSceneEntryUI : MonoBehaviour
{
    // selectScenePlayerManager‚ÍƒVƒ“ƒOƒ‹ƒgƒ“

    [SerializeField] GameObject[] entryUIs = new GameObject[2];

    private void Start()
    {

        entryUIs[0].SetActive(true);
        entryUIs[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectPlayerManager.Instance.Players.Count >= 1)
        {
            entryUIs[0].SetActive(false);
        }

        if (SelectPlayerManager.Instance.Players.Count >= 2)
        {
            entryUIs[1].SetActive(false);
        }
    }
}
