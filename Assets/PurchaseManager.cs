using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensions;

    public static string coins3000ProductId = "coins3000";
    public static string coins5000ProductId = "coins5000";
    public static string coins10000ProductId = "coins10000";
    public static string coins20000ProductId = "coins20000";

    public PurchaseStatusDialog purchaseStatusDialog;

    public static PurchaseStatusDialog sorryForThatHack;

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
        builder.AddProduct(coins3000ProductId, ProductType.Consumable);
        builder.AddProduct(coins5000ProductId, ProductType.Consumable);
        builder.AddProduct(coins10000ProductId, ProductType.Consumable);
        builder.AddProduct(coins20000ProductId, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void Buy3000Coins()
    {
        BuyProduct(coins3000ProductId);
    }
    public void Buy5000Coins()
    {
        BuyProduct(coins5000ProductId);
    }
    public void Buy10000Coins()
    {
        BuyProduct(coins10000ProductId);
    }
    public void Buy20000Coins()
    {
        BuyProduct(coins20000ProductId);
    }

    public void BuyProduct(string productId)
    {
        if (storeController == null)
        {
            Debug.Log("Cannot perform buying product. Manager is not initialized");
            return;
        }

        Product product = storeController.products.WithID(productId);
        sorryForThatHack = purchaseStatusDialog;
        if (product != null && product.availableToPurchase)
        {
            Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
            storeController.InitiatePurchase(product);
        } else
        {
            Debug.Log("PurchaseManager: buying product FAIL. Not purchasing product, either is not found or is not available for purchase");
            sorryForThatHack.Show("Failed to purchase: wrong productID", false);
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
        sorryForThatHack.Show("Can't initialize purchasing, please contact developers: " + error, false);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("PurchaseManager: failed to buy a product " + product.definition.storeSpecificId + ", failReason: " + reason);
        sorryForThatHack.Show("Failed to purchase: " + reason, false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var productId = args.purchasedProduct.definition.id;

        if (productId.StartsWith("coins"))
        {
            var count = int.Parse(productId.Substring(5));
            if (count == 3000)
            {
                count = 8000;
            }
            if (count == 5000)
            {
                count = 15000;
            }
            if (count == 10000)
            {
                count = 25000;
            }
            if (count == 20000)
            {
                count = 50000;
            }
            ProcessCoinPurchase(count);
        } else
        {
            Debug.Log("PurchaseManager: Purchased fail, unknown product Id: " + productId);
            sorryForThatHack.Show("Something goes wrong! Try again later or contact developers.", false);
        }

        return PurchaseProcessingResult.Complete;
    }

    private void ProcessCoinPurchase(int count)
    {
        Debug.Log("PurchaseManager: Purchased coins " + count);
        SoundManager.PlaySound("Chest");
        sorryForThatHack.Show("Puchased " + count + " coins!", true);

        var currentCount = PlayerPrefs.GetInt("coins");
        currentCount += count;
        PlayerPrefs.SetInt("coins", currentCount);
    }
}
