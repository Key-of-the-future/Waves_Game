using UnityEngine;
using TMPro;

public class TextCtrl : MonoBehaviour
{
    public TMP_Text textBullets;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBullets.text = " 10 ";
    }

    // Update is called once per frame
    public void UpdateBullets(int bullets)
    {
        textBullets.text = " " + bullets;
    }
}
