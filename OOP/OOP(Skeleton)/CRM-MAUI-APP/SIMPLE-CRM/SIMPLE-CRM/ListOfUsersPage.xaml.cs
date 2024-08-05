using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using System.Collections.ObjectModel;

namespace SIMPLE_CRM;

public partial class ListOfUsersPage : ContentPage
{
    public ObservableCollection<User> Users { get; set; }

    public ListOfUsersPage()
    {
        InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        UserRep userRep = new UserRep(dataStorage);
        Users = new ObservableCollection<User>(userRep.GetAll());
        this.BindingContext = this;
    }
}