using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameSettings
{
    public static class SaveAndLoadSettings
    {
        // Save Settings Data
        public static void SaveSettings(SO_Settings settings)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/Settings.ini";
            Debug.Log("Saving at: " + filePath);
            FileStream stream = new FileStream(filePath, FileMode.Create);
            SettingsFile data = new SettingsFile(settings);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        // Load Settings Data
        public static SettingsFile LoadSettings()
        {
            string filePath = Application.persistentDataPath + "/Settings.ini";
            if (File.Exists(filePath))
            {
                Debug.Log("Settings file found: " + filePath);
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);
                SettingsFile data = formatter.Deserialize(stream) as SettingsFile;
                stream.Close();
                return data;
            }
            else
            {
                Debug.Log("Settings file not found!");
                return null;
            }
        }
    }
}