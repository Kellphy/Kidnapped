using System.Collections;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerClockText;
    bool runningPuzzles;

    int puzzleSeconds;
    async void Start()
    {
        puzzleSeconds = Puzzle_Manager.instance.puzzles.Count * 45;
        await AudioSelector.instance.PlayIntro();
        while (true)
        {
            await Puzzle_Manager.instance.Puzzle_Start();

            runningPuzzles = true;
            StartCoroutine(TimerClock());
            await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(puzzleSeconds));
            runningPuzzles=false;

            if(Puzzle_Manager.instance.puzzleIterator < Puzzle_Manager.instance.puzzles.Count)
            {
                Debug.Log("You Lost");
                break;
            }
            else
            {
                Puzzle_Manager.instance.Reset();
                //play audio clip for the start of the second round
            }
        }
    }

    IEnumerator TimerClock()
    {
        float timer = puzzleSeconds;
        while (runningPuzzles)
        {
            timerClockText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt((timer / 60)), Mathf.FloorToInt((timer % 60)));
            yield return new WaitForSeconds(1);
            timer -= 1.0f;
        }
    }
}
