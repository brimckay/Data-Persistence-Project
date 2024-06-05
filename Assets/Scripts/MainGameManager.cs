using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance; // Singleton instance of this class
    public string playerName; // Current player's name
    public int bestScore; // Highest score achieved

    [System.Serializable]
    public class SaveData
    {
        public string playerName; // Player name to save
        public int bestScore; // Best score to save
    }

    void Awake()
    {
        // Ensure that there is only one instance of this object in the game
        if (Instance != null)
        {
            Destroy(gameObject); // Destroy this if an instance already exists
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object alive throughout the game
        LoadNameAndScore(); // Load the name and best score from storage
    }

    public void SaveNameAndScore()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data); // Convert data to JSON format
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json); // Write JSON to file
    }

    public void LoadNameAndScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) // Check if the save file exists
        {
            string json = File.ReadAllText(path); // Read the JSON string from the file
            SaveData data = JsonUtility.FromJson<SaveData>(json); // Convert JSON back to SaveData object
            playerName = data.playerName; // Set the player name from saved data
            bestScore = data.bestScore; // Set the best score from saved data
        }
        else
        {
            playerName = "Player"; // Default player name if no save file exists
            bestScore = 0; // Default score if no save file exists
        }
    }

    public void DeleteSavedData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path)) // Check if the file exists
        {
            File.Delete(path); // Delete the file
            Debug.Log("Saved data deleted."); // Log message to console
        }
        else
        {
            Debug.Log("No saved data to delete."); // Log message if no file exists
        }
    }

}