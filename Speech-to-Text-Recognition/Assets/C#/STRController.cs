/*
 * 本程式基於:https://www.youtube.com/watch?v=HwT6QyOA80E 的教學所製作的
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.UI;

public class STRController : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    public Text ShowText;
    public Animation GirlAnim;

    void Start()
    {
        //找尋裝置裡是否有麥克風
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        GirlAnim.Play("f02_idle_00");
         keywords.Add("move", () =>
        {
            GoCalled();
        });
        keywords.Add("jump", () =>
        {
            JumpCalled();
        });
        keywords.Add("stop", () =>
        {
            StopCalled();
        });
        keywords.Add("over", () =>
        {
            OverCalled();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += keywordRecognizerOnPhraseRecognized;
        keywordRecognizer.Start();
    }

    void keywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args) {

        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    public void GoCalled() {
        
        GirlAnim.Play("f02_walk_00");
        ShowText.text = ShowText.text + "\n You just said 'Move'!!!";
    }

    public void JumpCalled() {
        GirlAnim["f02_jump_10"].normalizedTime = 1.0f;
        GirlAnim.Play("f02_jump_10");
        ShowText.text = ShowText.text + "\n You just said 'Jump'!!!";
        StartCoroutine(AnimReturnIdle());
    }

    public void StopCalled()
    {
        GirlAnim.Play("f02_idle_00");
        ShowText.text = ShowText.text + "\n You just said 'Stop'!!!";
    }

    public void OverCalled(){
        ShowText.text = ShowText.text + "\n OK~I help you Exit.";
        StartCoroutine(ExitAnim());
    }

    IEnumerator AnimReturnIdle()
    {
        yield return new WaitForSeconds(1.0f);
        GirlAnim.Play("f02_idle_00");
    }

    IEnumerator ExitAnim()
    {
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + ".";
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + ".";
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + ".";
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + ".";
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + ".";
        yield return new WaitForSeconds(0.6f);
        ShowText.text = ShowText.text + "bye~bye~!";
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
}
