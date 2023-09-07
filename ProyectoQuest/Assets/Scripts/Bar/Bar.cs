using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public enum FillingDirection { Normal, Reverse}
    public enum FillMode { Instantaneous, Smoothed}
    public enum NumericType { Value, Percentage, Ratio, Seconds}
    public enum NumericFormat { Raw, Integer, PointOne, PointTwo}
    
    public FillMode fillMode = FillMode.Instantaneous;
    public FillingDirection fillingDirection = FillingDirection.Normal;
    public Image filler;
    public float smoothedModeTime = 0.5f;
    public bool startFull = false;

    [Space(20)]
    public NumericType counterType = NumericType.Percentage;
    public NumericFormat counterFormat = NumericFormat.Integer;
    
    [Space(5)]
    public bool updateTitle = false;
    public TMP_Text title;
    
    [Space(5)]
    public TMP_Text current;
    public TMP_Text max;

    private bool filling = false;


    private void Start()
    {
        filler.fillAmount = startFull ? 1 : 0;
    }

    public void Refresh(StatElement statElement)
    {
        if (updateTitle) title.text = statElement.GetStringValue();
        if (statElement.GetCurrentValue() < 0) return;

        switch(fillMode)
        {
            case FillMode.Instantaneous:
                FillerUpdate(statElement);
                break;
            case FillMode.Smoothed:
                StartCoroutine(SmoothedUpdate(statElement));
                break;
        }
        
    }

    private void FillerUpdate(StatElement statElement)
    {
        float current = statElement.GetCurrentValue();

        switch (fillingDirection)
        {
            case FillingDirection.Normal:
                filler.fillAmount = current / statElement.GetMaxValue();
                break;
            case FillingDirection.Reverse:
                current = statElement.GetMaxValue() - statElement.GetCurrentValue();
                filler.fillAmount = current / statElement.GetMaxValue();
                break;
        }


        string format = "";
        switch (counterFormat)
        {
            case NumericFormat.Integer:
                format = "0";
                break;
            case NumericFormat.PointOne:
                format = "0.0";
                break;
            case NumericFormat.PointTwo:
                format = "0.00";
                break;
        }

        if (current > statElement.GetMaxValue()) current = statElement.GetMaxValue();

        switch (counterType)
        {
            case NumericType.Value:
                this.current.text = current.ToString(format);
                max.text = "";
                break;
            case NumericType.Seconds:
                this.current.text = current.ToString(format) + "s";
                max.text = "";
                break;
            case NumericType.Ratio:
                this.current.text = (current).ToString(format);
                max.text = "| " + statElement.GetMaxValue().ToString(format);
                break;
            case NumericType.Percentage:
                this.current.text = (current / statElement.GetMaxValue() * 100).ToString(format) + "%";
                max.text = "";
                break;
        }
    }

    private IEnumerator SmoothedUpdate(StatElement statElement)
    {

        float max = statElement.GetMaxValue(); //4
        float start = max * filler.fillAmount * 1.0f; //4 * 0.5 = 2
        

        float end = statElement.GetCurrentValue(); //1
        
        float time = 0;
        while(time < smoothedModeTime)
        {
            float current = Mathf.Lerp(start, end, time / smoothedModeTime);
            FillerUpdate(current, max);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    private void FillerUpdate(float currentVal, float maxVal)
    {
        if(maxVal > 0)
        {
            switch (fillingDirection)
            {
                case FillingDirection.Normal:
                    filler.fillAmount = currentVal / maxVal;
                    break;
                case FillingDirection.Reverse:
                    currentVal = maxVal - currentVal;
                    filler.fillAmount = currentVal / maxVal;
                    break;
            }
        }
        else
        {
            filler.fillAmount = 0;
        }


        string format = "";
        switch (counterFormat)
        {
            case NumericFormat.Integer:
                format = "0";
                break;
            case NumericFormat.PointOne:
                format = "0.0";
                break;
            case NumericFormat.PointTwo:
                format = "0.00";
                break;
        }


        if (currentVal > maxVal) currentVal = maxVal;

        switch (counterType)
        {
            case NumericType.Value:
                current.text = currentVal.ToString(format);
                max.text = "";
                break;
            case NumericType.Seconds:
                current.text = currentVal.ToString(format) + "s";
                max.text = "";
                break;
            case NumericType.Ratio:
                current.text = (currentVal).ToString(format);
                max.text = "| " + maxVal.ToString(format);
                break;
            case NumericType.Percentage:
                if (maxVal == 0)
                {
                    current.text = 0.0f.ToString(format) + "%";
                }
                else
                {
                    current.text = (currentVal / maxVal * 100).ToString(format) + "%";
                }
                max.text = "";
                break;
        }

    }
    
    public void SimpleRefresh(float current, float max)
    {
        filler.fillAmount = current/max;
    }

    public void SimpleRefresh(float current, float max, NumericType numericType,NumericFormat numericFormat, string title = "")
    {
        filler.fillAmount = current / max;

        string format = "";
        switch (numericFormat)
        {
            case NumericFormat.Integer:
                format = "0";
                break;
            case NumericFormat.PointOne:
                format = "0.0";
                break;
            case NumericFormat.PointTwo:
                format = "0.00";
                break;
        }
        switch (numericType)
        {
            case NumericType.Value:
                this.current.text = current.ToString(format);
                this.max.text = "";
                break;
            case NumericType.Seconds:
                this.current.text = current.ToString(format) + "s";
                this.max.text = "";
                break;
            case NumericType.Ratio:
                this.current.text = (current).ToString(format);
                this.max.text = "| " + max.ToString(format);
                break;
            case NumericType.Percentage:
                if (max == 0)
                {
                    this.current.text = 0.0f.ToString(format) + "%";
                }
                else
                {
                    this.current.text = (current / max * 100).ToString(format) + "%";
                }
                this.max.text = "";
                break;
        }

        if(title != "") this.title.text = title;
    }
}
