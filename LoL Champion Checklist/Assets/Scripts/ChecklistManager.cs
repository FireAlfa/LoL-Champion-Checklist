using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChecklistManager : MonoBehaviour
{
    public ChampionList championList;
    public GameObject championItemPrefab;
    public Transform contentPanel;
    private string savePath;
    public TextMeshProUGUI doneCountText;
    public int doneCount = 0;

    private void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "champions.json");
        LoadChampions();
        PopulateChecklist();
        UpdateDoneCount();
    }

    private void PopulateChecklist()
    {
        foreach (var champ in championList.champions)
        {
            GameObject newItem = Instantiate(championItemPrefab, contentPanel);
            var itemImage = newItem.transform.Find("ChampionImage").GetComponent<Image>();
            var itemText = newItem.transform.Find("ChampionName").GetComponent<TMP_Text>();
            var itemButton = newItem.transform.Find("Button").GetComponent<Button>();

            // Load the champion image from Resources
            Sprite champSprite = Resources.Load<Sprite>($"ChampionImages/{champ.Name}");
            if (champSprite != null)
            {
                itemImage.sprite = champSprite;
            }

            itemText.text = champ.Name.ToString();
            itemButton.GetComponentInChildren<TMP_Text>().text = champ.IsDone ? "Done" : "";
            var tempColor = itemButton.GetComponent<Image>().color;
            tempColor.a = champ.IsDone ? 230f / 255f : 0f;
            itemButton.GetComponent<Image>().color = tempColor;

            // Capture the current champion in the loop
            Champion currentChamp = champ;
            itemButton.onClick.AddListener(() => ToggleChampionStatus(currentChamp, itemButton));
        }
    }

    private void ToggleChampionStatus(Champion champion, Button button)
    {
        champion.IsDone = !champion.IsDone;
        button.GetComponentInChildren<TMP_Text>().text = champion.IsDone ? "Done" : "";
        var tempColor = button.GetComponent<Image>().color;
        tempColor.a = champion.IsDone ? 230f/255f : 0f;
        button.GetComponent<Image>().color = tempColor;
        SaveChampions();
        UpdateDoneCount();
    }

    private void SaveChampions()
    {
        string json = JsonUtility.ToJson(championList);
        File.WriteAllText(savePath, json);
    }

    private void LoadChampions()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, championList);
        }
    }

    public void ResetAllChampions()
    {
        foreach (var champ in championList.champions)
        {
            champ.IsDone = false;
        }
        SaveChampions();
        RefreshChecklist();
        UpdateDoneCount();
    }
    private void RefreshChecklist()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        PopulateChecklist();
    }
    private void UpdateDoneCount()
    {
        doneCount = 0;
        foreach (var champ in championList.champions)
        {
            if (champ.IsDone)
            {
                doneCount++;
            }
        }
        doneCountText.text = $"{doneCount}/{championList.champions.Count}";
    }
}