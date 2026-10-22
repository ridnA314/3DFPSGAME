using System.Collections.Generic;
using UnityEngine;

public class DictionaryExample : MonoBehaviour
{
    //Array or List is approaching by using index
    //But Dictionary is Saving Data with Pair of Key and Value
    //HashTable(=Dictionary) is DS that manage Data consisting of Key and Value
    private void Start()
    {
        List<int> ages = new List<int>() { 22, 21, 20, 35 };

        Dictionary<string, int> students =
            new Dictionary<string, int>() { { "OhNahyeon", 22 }, { "ParkCheolGyu", 22 } };

        Debug.Log(students["OhNahyeon"]);
        students["OhNahyeon"] = 2;
        students.Add("SongJaeook", 17);
        students.Remove("SongJaeook");
        bool has = students.ContainsKey("SongJaeook");

        foreach (KeyValuePair<string, int> item in students)
        {
            Debug.Log($"{item.Key}'s age is {item.Value}");
        }
    }
}