using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{

    /*[SerializeField]*/ public TextMeshProUGUI promtText;
    void Start()
    {
        
    }



  public void UpdateText(string promtmessege)
    {

        promtText.text = promtmessege;
    }
}
