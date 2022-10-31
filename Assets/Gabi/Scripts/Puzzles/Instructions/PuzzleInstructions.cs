using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PuzzleInstructions : GenericPuzzle
{
    public GameObject textField;
    public List<GameObject> instructionObjects;

    public List<GameObject> firstType;
    public List<GameObject> secondType;

    int currentInstruction;

    public override void PuzzleAwake()
    {
        if (Random.Range(0, 2) == 1)
        {
            foreach (GameObject gameObject in firstType)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject gameObject in secondType)
            {
                gameObject.SetActive(true);
            }
        }
    }
    public override void PuzzleStart()
    {
        instructionObjects[currentInstruction].SetActive(true);
        base.PuzzleStart();
        StartCoroutine(RemoveText());
    }

    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(3);
        /*
        while (textField.transform.position.y > -1)
        {
            textField.transform.position += new Vector3(0, -1f * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }*/
        textField.SetActive(false);
    }

    public void NextInstruction()
    {
        if (currentInstruction - 1 >= 0)
        {
            instructionObjects[currentInstruction - 1].SetActive(false);
        }

        currentInstruction++;

        if (currentInstruction >= instructionObjects.Count)
        {
            PuzzleEndDelay(true, instructionObjects[currentInstruction - 1]);
        }
        else
        {
            instructionObjects[currentInstruction].SetActive(true);
        }
    }
    public void FailedInstruction()
    {
        PuzzleEndDelay(false, instructionObjects[currentInstruction]);
    }

    public async void PuzzleEndDelay(bool succeeded, GameObject goToDisable)
    {
        await Task.Delay(500);
        goToDisable.SetActive(false);
        PuzzleEnd(succeeded);
    }
}
