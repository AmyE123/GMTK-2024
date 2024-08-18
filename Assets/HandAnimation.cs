using UnityEngine;

public class HandAnimation : MonoBehaviour
{
    [SerializeField] private GameObject _staticHand;
    [SerializeField] private GameObject _handOut;
    [SerializeField] private GameObject _handMakeSmall;
    [SerializeField] private GameObject _handMakeLarge;

    public void PlayStaticHand()
    {
        _staticHand.SetActive(false);
        _handOut.SetActive(true);
        _handMakeSmall.SetActive(false);
        _handMakeLarge.SetActive(false);
    }

    public void PlayHandMakeSmall()
    {
        _staticHand.SetActive(false);
        _handOut.SetActive(false);
        _handMakeSmall.SetActive(true);
        _handMakeLarge.SetActive(false);
    }

    public void PlayHandMakeLarge()
    {
        _staticHand.SetActive(false);
        _handOut.SetActive(false);
        _handMakeSmall.SetActive(false);
        _handMakeLarge.SetActive(true);
    }

    public void PlayHandOut()
    {
        _staticHand.SetActive(false);
        _handOut.SetActive(true);
        _handMakeSmall.SetActive(false);
        _handMakeLarge.SetActive(false);
    }
}
