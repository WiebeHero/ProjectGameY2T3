using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private AudioSource click;
    // Start is called before the first frame update
    void Start()
    {
        Button[] button = FindObjectsOfType<Button>(true);
        for (int i = 0; i < button.Length; i++)
        {
            if (!button[i].gameObject.CompareTag("MicButton"))
            {
                button[i].onClick.AddListener(Deez);
            }
            
        }
    }

    private void Deez()
    {
        Debug.Log("Once");
        click.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
