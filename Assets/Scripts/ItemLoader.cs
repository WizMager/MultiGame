using System.Collections.Generic;
using PlayFab;
using PlayFab.ServerModels;
using TMPro;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
       [SerializeField] private TMP_Text text;

       private void Start()
       {
              PlayFabServerAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnSuccess, OnError);
       }

       private void OnSuccess(GetCatalogItemsResult catalog)
       {
              Debug.Log("Catalog load success.");
              FillCatalog(catalog.Catalog);
       }

       private void FillCatalog(List<CatalogItem> catalog)
       {
              foreach (var item in catalog)
              {
                     text.text += $"ID: {item.ItemId}, name: {item.DisplayName}\n";
                     Debug.Log($"ID: {item.ItemId}, name: {item.DisplayName}");
              }
       }

       private void OnError(PlayFabError error)
       {
             Debug.Log(error.ErrorMessage); 
       }

}