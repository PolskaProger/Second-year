using CommunityToolkit.Maui.Storage;
using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using System.Collections.ObjectModel;

namespace SIMPLE_CRM;

public partial class HistoryOfOrders : ContentPage
{
    public ObservableCollection<Order> Orders { get; set; }
    public ObservableCollection<Position> Positions { get; set; }
    private PositionRep positionRep;
    public HistoryOfOrders()
	{
		InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        OrderRep orderRep = new OrderRep(dataStorage);
        Orders = new ObservableCollection<Order>(orderRep.GetAll());
        this.positionRep = new PositionRep(dataStorage);
        Positions = null;
        OrderPicker.ItemsSource = Orders;
        this.BindingContext = this;
    }
    private void OnContactSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedOrder = (Order)picker.SelectedItem;
        Positions = new ObservableCollection<Position>(selectedOrder.PositionsInOrder);
        positionsCollectionView.ItemsSource = Positions;
    }
}