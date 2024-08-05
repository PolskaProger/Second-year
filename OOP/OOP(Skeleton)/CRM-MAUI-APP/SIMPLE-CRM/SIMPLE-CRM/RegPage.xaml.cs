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
            await DisplayAlert("Ошибка", "Пароли не совпадают!", "OK");
            return;
        }
        bool password_is_correct = correctInputService.ValidatePassword(passwordTemp);

        if (!password_is_correct)
        {
            await DisplayAlert("Ошибка", "Пароль не является валидным! (больше 8 символов, 1 большая и одна прописная латинская буква)", "OK");
            return;
        }

        string roleTemp = "";
        if (ManagerRadioButton.IsChecked)
        {
            roleTemp = "Менеджер";
        }
        else if (CashierRadioButton.IsChecked)
        {
            roleTemp = "Кассир";
        }
        else if (AccountantRadioButton.IsChecked)
        {
            roleTemp = "Бухгалтер";
        }
        else
        {
            await DisplayAlert("Ошибка", "Не выбрана роль пользователя.", "OK");
            return;
        }

        var newUser = new User().Register(userNameTemp, passwordTemp, roleTemp);
        userRep.Add(newUser);
        await DisplayAlert("Успех", $"Пользователь под именем {userNameTemp} успешно создан и добавлен в БД!", "OK");
    }
}