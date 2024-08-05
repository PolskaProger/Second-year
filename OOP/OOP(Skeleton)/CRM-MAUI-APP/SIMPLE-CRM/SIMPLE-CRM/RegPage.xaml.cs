using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using CoreSIMPLECRM.Services;

namespace SIMPLE_CRM;

public partial class RegPage : ContentPage
{
    private UserRep userRep;
    private CorrectInputService correctInputService;
    public RegPage()
	{
        InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        var userRep = new UserRep(dataStorage);
        CorrectInputService correct_input = new CorrectInputService();
        this.userRep = userRep;
        this.correctInputService = correct_input;
    }
	public async void OnRegisterButtonClicked(object sender, EventArgs e)
	{
        string userNameTemp = UsernameEntry.Text;
        string passwordTemp = PasswordEntry.Text;
        string checkPassword = ConfirmPasswordEntry.Text;
        if (passwordTemp != checkPassword)
        {
            await DisplayAlert("������", "������ �� ���������!", "OK");
            return;
        }
        bool password_is_correct = correctInputService.ValidatePassword(passwordTemp);

        if (!password_is_correct)
        {
            await DisplayAlert("������", "������ �� �������� ��������! (������ 8 ��������, 1 ������� � ���� ��������� ��������� �����)", "OK");
            return;
        }

        string roleTemp = "";
        if (ManagerRadioButton.IsChecked)
        {
            roleTemp = "��������";
        }
        else if (CashierRadioButton.IsChecked)
        {
            roleTemp = "������";
        }
        else if (AccountantRadioButton.IsChecked)
        {
            roleTemp = "���������";
        }
        else
        {
            await DisplayAlert("������", "�� ������� ���� ������������.", "OK");
            return;
        }

        var newUser = new User().Register(userNameTemp, passwordTemp, roleTemp);
        userRep.Add(newUser);
        await DisplayAlert("�����", $"������������ ��� ������ {userNameTemp} ������� ������ � �������� � ��!", "OK");
    }
}