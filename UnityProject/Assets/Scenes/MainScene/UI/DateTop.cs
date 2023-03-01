using RxPropertyChanged;
using Sessions;
using UnityEngine.UI;

public class DateTop : RxBehaviour<IDate>
{
    public Text year;
    public Text month;
    public Text day;
    public Text hour;

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.year, year);
        Binding(dataContext => dataContext.month, month);
        Binding(dataContext => dataContext.day, day);
        Binding(dataContext => dataContext.hour, hour);
    }
}

