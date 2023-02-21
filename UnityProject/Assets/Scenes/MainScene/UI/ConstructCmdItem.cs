using RxPropertyChanged;
using Sessions;
using UnityEngine.UI;

public class ConstructCmdItem : RxBehaviour<ICommand>
{
    public Text title;
    public Button button;

    protected override void BindingInit()
    {
        Binding(dataContext => dataContext.title, title);

        button.onClick.AddListener(() => dataContext.Exec());
    }

    private void Update()
    {
        button.interactable = dataContext.isVaild();
    }
}
