using TD.ElementSystem;
using UnityEngine;

namespace TD.Entities.Towers
{
    public class TowerDestroyResponse : MonoBehaviour
    {
        private void OnDestroy()
        {
            RadiusGetter radiusGetter = GetComponent<RadiusGetter>();
            ElementPickpointGetter elementPickpointGetter = GetComponent<ElementPickpointGetter>();
            SelectionAreaGetter selectionAreaGetter = GetComponent<SelectionAreaGetter>();
            ElementSwapInterfaceGetter elementSwapInterfaceGetter = GetComponent<ElementSwapInterfaceGetter>();

            if(radiusGetter!=null)
            {
                Transform radiusTransform = radiusGetter.RadiusTransform;
                if (radiusTransform != null)
                {
                    Destroy(radiusTransform.gameObject);
                }
            }
            if(elementPickpointGetter!=null)
            {
                Transform elementPickpointTransform = elementPickpointGetter.ElementPickpointTransform;
                if (elementPickpointTransform != null)
                {
                    Destroy(elementPickpointTransform.gameObject);
                }
            }
            if(selectionAreaGetter!=null)
            {
                Transform selectionAreaTransform = selectionAreaGetter.SelectionAreaTransform;
                if (selectionAreaTransform != null)
                {
                    Destroy(selectionAreaTransform.gameObject);
                }
            }      
            if(elementSwapInterfaceGetter!=null)
            {
                Transform elementSwapInterfaceTransform = elementSwapInterfaceGetter.SwapInterfaceTransform;
                if (elementSwapInterfaceTransform != null)
                {
                    Destroy(elementSwapInterfaceTransform.gameObject);
                }
            }
        }           
    }
}