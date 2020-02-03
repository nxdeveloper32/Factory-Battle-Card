using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class CardSOHolder : MonoBehaviour
{
    public static CardSOHolder instance;
    public List<CardSO> allSObjects;

    public List<int> randomList;
    void Awake()
    {
        instance = this;
    }

    public void GenerateRandomList()
    {
        randomList = Enumerable.Range(0, allSObjects.Count).ToList();
        System.Random r = new System.Random();
        randomList = randomList.OrderBy((x) => r.Next()).ToList<int>();


    }
    public List<CardSO> ReturnRandomCards(int count)
    {

        List<CardSO> toReturn = new List<CardSO>();
        for (int i = 0; i < count; i++)
        {

            toReturn.Add(allSObjects[randomList[i]]);
        }
        return toReturn;
    }
}
