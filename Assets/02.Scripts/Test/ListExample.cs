using UnityEngine;
using System.Collections.Generic;

public class ListExample : MonoBehaviour
{
    //Data Structure
    // Struture for Manage(CRUDS) Data efficiently
    // C - Create
    // R - Read
    // U - Update
    // D - Delete
    // S - Search
    // Linear DS    : Array, List, Queue, Stack
    // Non-Linear DS    : Tree, Graph, Hash Table

    private void Start()
    {
        /*
        int[] ages = new int[] { 22, 21, 20, 35 }; // Array declaration and definition

        // 1. can not change size of Array from declaration
        // -> When Add, Insert and Delete with Data, should create new Array
        ages[0] = 17;
        ages[1] = 40;
        ages[2] = 27;
        ages[3] = 32;
        ages[4] = 24; // can Add? IndexOutOfRange Error
        */

        //Because of above reason, use Dynamic Array or linked list
        // c# -> implement List in Collection namespace
        // Collection : Implement DS to Class

        // List -> Equal Array but can change size dynamically
        // using System.Collections.Generic;

        List<int> ages2 = new List<int>() { 22, 21, 20, 35 }; // Declaration and Definition about List
        ages2[0] = 17;
        ages2[1] = 40;
        ages2[2] = 27;
        ages2[3] = 32;
        //ages2[4] = 24; // IndexOutOfRange Error
        ages2.Add(24); // Add Data at End of List
        ages2.Insert(1, 12); // Insert Data of intermediate of List
        ages2.Remove(40);
        ages2.RemoveAt(3);
        //int teenager = ages2.Find(x => x < 20);   Search certain data

        for (int index = 0; index < ages2.Count; index++)
        {
            Debug.Log(ages2[index]); //Search by using index
        }

        foreach (int age in ages2)
        {
            Debug.Log(age); //Search by using foreach
        }

        //Today Assignment : List/Stack/Queue Advance/DisAdvance
    }
}