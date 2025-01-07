public interface IView
{
    void Show();
    void Hide();
    void RefreshUI();
    void ShowLoading(bool result);
    void ShowMessage(string message);
}
