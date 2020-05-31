using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensions;

    public static string coins1000ProductId = "coins1000";
    public static string coins3000ProductId = "coins3000";
    public static string coins5000ProductId = "coins5000";
    public static string coins10000ProductId = "coins10000";

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    private void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(coins1000ProductId, ProductType.Consumable);
        builder.AddProduct(coins3000ProductId, ProductType.Consumable);
        builder.AddProduct(coins5000ProductId, ProductType.Consumable);
        builder.AddProduct(coins10000ProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string productId)
    {
        if (storeController == null)
        {
            Debug.Log("Cannot perform buying product. Manager is not initialized");
            return;
        }

        Product product = storeController.products.WithID(productId);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
            storeController.InitiatePurchase(product);
        } else
        {
            Debug.Log("PurchaseManager: buying product FAIL. Not purchasing product, either is not found or is not available for purchase");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("PurchaseManager: initialization PASS");

        storeController = controller;
        storeExtensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("PurchaseManager: InitializationFailed, " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("PurchaseManager: failed to buy a product " + product.definition.storeSpecificId + ", failReason: " + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var productId = args.purchasedProduct.definition.id;

        if (productId.StartsWith("coins"))
        {
            var count = int.Parse(productId.Substring(5));
            ProcessCoinPurchase(count);
        } else
        {
            Debug.Log("PurchaseManager: Purchased fail, unknown product Id: " + productId);
        }

        return PurchaseProcessingResult.Complete;
    }

    private void ProcessCoinPurchase(int count)
    {
        Debug.Log("PurchaseManager: Purchased coins " + count);
        var currentCount = PlayerPrefs.GetInt("coins");
        currentCount += count;
        PlayerPrefs.SetInt("coins", currentCount);
    }
}
