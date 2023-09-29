using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MailBoxUIScript : MonoBehaviour
{
    public TextMeshProUGUI m_Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}
    
    public void DisplayMail(bool p_Bool, string p_String)
    {
        gameObject.SetActive(p_Bool);
        if (p_Bool == true)
        {
			if (m_Text != null)
            {
				m_Text.text = p_String;
			}
        }
    }
}
