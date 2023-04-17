using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using User.Wiebe.Scripts;

namespace Main._Scripts.Main
{
    public class RecordInputFromMic : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private DragonCommands dragonsCommands;
        [SerializeField] private AudioSource _audio;

        public void OnPointerDown(PointerEventData pEvent)
        {
            _audio.Play();
            dragonsCommands.StartRecordingCommand();
        }
        
        public void OnPointerUp(PointerEventData pEvent)
        {
            dragonsCommands.StopRecordingCommand();
        }
    }
}
