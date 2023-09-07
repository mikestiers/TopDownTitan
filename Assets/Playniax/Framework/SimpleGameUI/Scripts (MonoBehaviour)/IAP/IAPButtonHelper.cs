using UnityEngine;
#if UNITY_PURCHASING
using UnityEngine.Purchasing;
#endif
using UnityEngine.UI;

namespace Playniax.UI.SimpleGameUI
{
    public class IAPButtonHelper : MonoBehaviour
    {
        public int quantity = 1;

        public Button button;

#if UNITY_PURCHASING
        public IAPButton iAPButton;

        void OnEnable()
        {
            _Refresh();
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            if (reason == PurchaseFailureReason.PurchasingUnavailable)
            {
            }
        }

        public void OnPurchaseCompleted(Product product)
        {
            if (product != null && product.definition.id == iAPButton.productId)
            {
                PlayerPrefs.SetInt(iAPButton.productId, PlayerPrefs.GetInt(iAPButton.productId) + quantity);
                PlayerPrefs.Save();

                _Refresh();
            }

        }

        void _Refresh()
        {
            if (button == null) button = GetComponent<Button>();
            if (iAPButton == null) iAPButton = GetComponent<IAPButton>();

            if (PlayerPrefs.GetInt(iAPButton.productId) > 0 && iAPButton.consumePurchase == false) button.interactable = false;
        }

#endif
    }
}