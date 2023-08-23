using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public enum FillingDirection { Normal, Reverse}
    public enum NumericType { Value, Percentage, Ratio, Seconds}
    public enum NumericFormat { Raw, Integer, PointOne, PointTwo}
    
    public FillingDirection fillingDirection = FillingDirection.Normal;
    public Image filler;
    public TMP_Text current;
    public TMP_Text max;
    public NumericType counterType = NumericType.Percentage;
    public NumericFormat counterFormat = NumericFormat.Integer;
    public void Refresh(StatElement statElement)
    {
        if (statElement.GetCurrentValue() < 0) return;

        float current = statElement.GetCurrentValue();
        switch (fillingDirection)
        {
            case FillingDirection.Normal:
                filler.fillAmount =current / statElement.GetMaxValue();
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

        switch (counterType)
        {
            case NumericType.Value:
                this.current.text = current.ToString(format);
                break;
            case NumericType.Seconds:
                this.current.text = current.ToString(format) + "s";
                break;
            case NumericType.Ratio:
                this.current.text = ((int)current).ToString(format);
                max.text = "| " + statElement.GetMaxValue().ToString(format);
                break;
            case NumericType.Percentage:
                this.current.text = (current / statElement.GetMaxValue()*100).ToString(format) + "%";
                break;
        }
        
    }
}
