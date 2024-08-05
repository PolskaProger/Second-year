using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using CoreSIMPLECRM.Services;

namespace SIMPLE_CRM
{
    public partial class MainPage : ContentPage
    {
        private UserRep userRep;
        private CorrectInputService correctInputService;
        private User user;
        public MainPage()
        {
            InitializeComponent();
            var connectionString = "mongodb://localhost:27017";
            var dbName = "CRM-Database";
            var dataStorage = new DataStorage(connectionString, dbName);
            var userRep = new UserRep(dataStorage);
            User user = new User();
            CorrectInputService correct_input = new CorrectInputService();
            this.userRep = userRep;
            this.correctInputService = correct_input;
            this.user = user;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string userNameTempAuth = LoginEntry.Text;
            string passwordTempAuth = PasswordEntry.Text;
            bool correct_password = correctInputService.ValidatePassword(passwordTempAuth);
            if (correct_password)
            {
                bool enter = user.Authorize(userNameTempAuth, passwordTempAuth);
                if (enter)
                {
                    string role = user.RoleOfUser(userNameTempAuth);
                    if (role == "Менеджер")
                    {
                        Shell.Current.FlyoutIsPresented = false;
                        await DisplayAlert("Успех", "Вы авторизовались в приложении как Менеджер", "OK");
                        await Shell.Current.GoToAsync($"//{nameof(ManagerPage)}");
                    }
                    else if (role == "Кассир")
                    {
                        Shell.Current.FlyoutIsPresented = false;
                        await DisplayAlert("Успех", "Вы авторизовались в приложении как Кассир", "OK");
                        await Shell.Current.GoToAsync($"//{nameof(CashierPage)}");
                    }
                    else if (role == "Бухгалтер")
                    {
                        Shell.Current.FlyoutIsPresented = false;
                        await DisplayAlert("Успех", "Вы авторизовались в приложении как Бухгалтер", "OK");
                        await Shell.Current.GoToAsync($"//{nameof(AccountantPage)}");
                    }
                }
                else
                {
                    await DisplayAlert("Ошибка авторизации", "Неверный логин/пароль или данного пользователя не существует", "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Ошибка, пароль не является валидным! (больше 8 символов, 1 большая и одна прописная латинская буква)", "OK");
            }
        }
    }

}
