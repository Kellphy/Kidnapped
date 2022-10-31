using System;
using TMPro;
using UnityEngine;

public class PuzzleRiddle : GenericPuzzle
{
    public TextMeshProUGUI questionTMP;
    public TextMeshProUGUI[] answerTMP = new TextMeshProUGUI[4];
    int _correctAnswer;
    bool answered;

    [Serializable]
    public struct Riddles
    {
        public string Question;
        public string Answer_1;
        public string Answer_2;
        public string Answer_3;
        public string Answer_4;
        public int correctAnswer;
    }
    public Riddles[] riddles;

    public override void PuzzleAwake()
    {
        Riddles riddle = riddles[UnityEngine.Random.Range(0, riddles.Length)];

        questionTMP.text = riddle.Question;
        answerTMP[0].text = riddle.Answer_1;
        answerTMP[1].text = riddle.Answer_2;
        answerTMP[2].text = riddle.Answer_3;
        answerTMP[3].text = riddle.Answer_4;
        _correctAnswer = riddle.correctAnswer;
    }

    public void Answer(int answerId)
    {
        if (!answered)
        {
            answered = true;
            for (int i = 0; i < answerTMP.Length; i++)
            {
                if (_correctAnswer == i + 1)
                {
                    answerTMP[i].color = Color.green;
                }
                else if (answerId == i + 1)
                {
                    answerTMP[i].color = Color.red;
                }
                else
                {
                    answerTMP[i].color = Color.gray;
                }
            }

            if (answerId == _correctAnswer)
            {
                PuzzleEnd(true);
            }
            else
            {
                PuzzleEnd(false);
            }
        }
    }
}