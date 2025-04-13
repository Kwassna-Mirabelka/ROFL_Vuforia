using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Drawing;
using UnityEngine;
using Vuforia;

public class ImageTargetManagerCode : MonoBehaviour
{
    [System.Serializable]
    public struct cardNames
    {
        public string imageName;
        public string prefabName;
      
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  
    public string cardImagesPath = "CardImages/";
    public string prefabPath = "Prefabs/";


    public GameObject imageTargetPrefab;
    public cardNames[] cardsNames;

    private Dictionary<string, GameObject> RunesPrefabs = new Dictionary<string, GameObject>();
    
    public Dictionary<string, Texture2D> cardsImages = new Dictionary<string, Texture2D>();

    private Dictionary<string, GameObject> imageTrackerObjects = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach(var card in cardsNames)
        {
           
            cardsImages.Add(card.imageName, Resources.Load<Texture2D>(cardImagesPath + card.imageName + ".jpg"));
            
            RunesPrefabs.Add(card.imageName, Resources.Load<GameObject>(prefabPath + card.prefabName));
        }
        foreach (var card in cardsImages)
        {
            var tempTarget = Instantiate(imageTargetPrefab);
            tempTarget.name = card.Key;
            tempTarget.GetComponent<ImageTargetBehaviour>().name = card.Value.name;
            imageTrackerObjects.Add(card.Key, Instantiate(imageTargetPrefab));
            
            imageTrackerObjects[card.Key].name = card.Key+"ImageTracker";
            Instantiate(RunesPrefabs[card.Key], imageTrackerObjects[card.Key].transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
