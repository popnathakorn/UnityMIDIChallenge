using System.IO;
using UnityEngine;

public class RhythmSceneController : MonoBehaviour
{

    [SerializeField]
    private string drumTrackFileName;
    private byte[] _midiTrackFileBuffer;
    
    void Start()
    {
        // Loading drum track from midi file
        _midiTrackFileBuffer = File.ReadAllBytes($"{Application.dataPath}/Resources/{drumTrackFileName}.midi");
        
        // Below this please implement scene initialize
        // from MIDI track above to any kind of rhythm game play scene
        // *  please ensure the best approach to draw the scene for memory efficiency 
        // ** any kind of scene is welcome ( both 2D or 3D )
        //
        // Hint for MIDI track 
        // this MIDI track represent 4 bars of drum track in Cha Cha Cha rhythm style
        // Looping for pad note
        // C#2 (37) is rim snare sound
        // C#3 (49) is Cymbal
        // D2  (38) snare
        // C2  (36) is Bass drum
        // C3  (48) is High tom
        // G2  (43) is Floor tom
        // G2  (43) is Floor tom
        
        
    }

    void Update()
    {
        
    }
}
