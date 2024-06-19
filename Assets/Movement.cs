using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject speaker; // Assign the speaker object in the Inspector
    public GameObject[] characters; // Assign the 4 character objects in the Inspector
    public float proximityThreshold = 2.0f; // Adjust this value as needed
    public float characterProximityThreshold = 2.0f; // Adjust this value as needed for character-character interaction

    private Animator[] animators;
    private AudioSource[] audioSource;

    void Start()
    {
        // Initialize animators array
        animators = new Animator[characters.Length];
        audioSource = new AudioSource[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            animators[i] = characters[i].GetComponent<Animator>();
            audioSource[i] = characters[i].GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Check each character's proximity to the speaker
        foreach (GameObject character in characters)
        {
            float distanceToSpeaker = Vector3.Distance(character.transform.position, speaker.transform.position);

            if (distanceToSpeaker < proximityThreshold)
            {
                // Trigger the animation for being near the speaker
                Animator animator = character.GetComponent<Animator>();
                animator.SetBool("isNearSpeaker", true);
                AudioSource audioSource = character.GetComponent<AudioSource>();
                Debug.Log("isNearSpeaker is true");
                if(!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                // Revert to idle animation for the speaker
                Animator animator = character.GetComponent<Animator>();
                animator.SetBool("isNearSpeaker", false);
                AudioSource audioSource = character.GetComponent<AudioSource>();
                Debug.Log("isNearSpeaker is false");
                if(audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }

        // Check each character's proximity to other characters
        for (int i = 0; i < characters.Length; i++)
        {
            bool isNearOtherCharacter = false;
            for (int j = 0; j < characters.Length; j++)
            {
                if (i != j)
                {
                    float distanceToCharacter = Vector3.Distance(characters[i].transform.position, characters[j].transform.position);
                    if (distanceToCharacter < characterProximityThreshold)
                    {
                        isNearOtherCharacter = true;
                        // Set the other character's animator as well
                        animators[j].SetBool("isNearCharacter", true);
                        Debug.Log($"Character {i} and Character {j} are near each other");
                    }
                }
            }
            animators[i].SetBool("isNearCharacter", isNearOtherCharacter);
            Debug.Log($"Character {i} isNearCharacter is {isNearOtherCharacter}");
        }
    }
}
