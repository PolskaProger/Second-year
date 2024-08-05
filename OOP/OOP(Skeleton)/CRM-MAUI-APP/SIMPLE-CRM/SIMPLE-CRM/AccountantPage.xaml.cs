using CommunityToolkit.Maui.Storage;
using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;

namespace SIMPLE_CRM;

public partial class AccountantPage : ContentPage
{
    private OrderRep orderRep;
    private AnalyticsService analyticsService;
    private Serializer serializer;
    private string selectedFilePath;
    private string selectedFolderPath= "D:\\University\\253505\\ООП\\OOP(Skeleton)\\CRM-MAUI-APP\\SIMPLE-CRM";
    public AccountantPage()
	{
		InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        orderRep = new OrderRep(dataStorage); // Adjust as needed
        analyticsService = new AnalyticsService();
        serializer = new Serializer();
    }
    private async void OnShowAnalyticsClicked(object sender, EventArgs e)
    {
        var averageCheck = analyticsService.CalculateAverageCheck();
        AverageCheckLabel.Text = $"Средний чек по всем заказам: {averageCheck}$";

        var month = DateTime.Now.Month;
        var year = DateTime.Now.Year;
        var monthlyAverageCheck = analyticsService.CalculateMonthlyAverageCheck(year, month);
        MonthlyAverageCheckLabel.Text = $"Средний чек за текущий месяц: {monthlyAverageCheck}$";

        var (customerName, contact) = analyticsService.GetMostValuableCustomer();
        MostValuableCustomerLabel.Text = $"Самый платежеспособный клиент: {customerName}, Контакты клиента: {contact}";

        var mostPopularPosition = analyticsService.GetMostPopularPosition();
        MostPopularPositionLabel.Text = $"Самая популярная позиция: {mostPopularPosition?.Name ?? "Нет данных"}";
    }

    private async void OnPickFileClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        if (result != null)
        {
            selectedFilePath = result.FullPath;
            ResultLabel.Text = $"Выбран файл: {selectedFilePath}";
        }
    }
    private async void OnExecuteClicked(object sender, EventArgs e)
    {
        var selectedAction = SerializationPicker.SelectedIndex;
        if (selectedAction == -1)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите действие.", "OK");
            return;
        }

        switch (selectedAction)
        {
            case 0:
                SerializeToCsv();
                break;
            case 1:
                SerializeToJson();
                break;
            case 2:
                if (string.IsNullOrEmpty(selectedFilePath))
                {
                    await DisplayAlert("Ошибка", "Пожалуйста, выберите файл.", "OK");
                    return;
                }
                else
                {
                    DeserializeFromCsv();
                }
                break;
            case 3:
                if (string.IsNullOrEmpty(selectedFilePath))
                {
                    await DisplayAlert("Ошибка", "Пожалуйста, выберите файл.", "OK");
                    return;
                }
                else
                {
                    DeserializeFromJson();
                }
                break;
            default:
                await DisplayAlert("Ошибка", "Неверный выбор. Пожалуйста, попробуйте снова.", "OK");
                break;
        }
    }

    private void SerializeToCsv()
    {
        string serializedDataPath = @"D:\University\253505\ООП\OOP(Skeleton)\SerData";
        string ordersCSVPath = Path.Combine(serializedDataPath, "orders.csv");
        var ordersToSerialize = orderRep.GetAll().Select(o => new Ord
        {
            CustomerName = o.CustomerName,
            Contact = o.Contact,
            PositionsInOrder = o.PositionsInOrder.Select(p => new Pos
            {
                Name = p.Name,
                Cost = p.Cost,
                category = p.category,
                Quantity = p.Quantity,
                Id = p.Id
            }).ToList(),
            TotalCost = o.TotalCost,
            Date = o.Date,
            Id = o.Id
        }).ToList();

        serializer.SerializeToCsv(ordersToSerialize, ordersCSVPath);
        ResultLabel.Text = "Сериализация в CSV произведена успешно!";
    }

    private void SerializeToJson()
    {
        var orders = orderRep.GetAll();
        string serializedDataPath = @"D:\University\253505\ООП\OOP(Skeleton)\SerData";
        string ordersJsonPath = Path.Combine(serializedDataPath, "orders.json");
        serializer.SerializeToJson(orders, ordersJsonPath);
        ResultLabel.Text = "Сериализация в JSON произведена успешно!";
    }

    private void DeserializeFromCsv()
    {
        var ordersFromCSV = serializer.DeserializeFromCsv(selectedFilePath).Select(ord => new Order
        {
            Contact = ord.Contact,
            CustomerName = ord.CustomerName,
            Id = ord.Id,
            Date = ord.Date,
            TotalCost = ord.TotalCost,
            PositionsInOrder = ord.PositionsInOrder.Select(pos => new Position
            {
                Name = pos.Name,
                Cost = pos.Cost,
                category = pos.category,
                Quantity = pos.Quantity,
                Id = pos.Id
            }).ToList()
        }).ToList();
        var existingOrders = orderRep.GetAll().ToDictionary(o => o.Id, o => o);
        foreach (var order in ordersFromCSV)
        {
            if (!existingOrders.ContainsKey(order.Id))
            {
                orderRep.Add(order);
            }
        }

        ResultLabel.Text = "Десериализация из CSV произведена успешно!";
    }

    private void DeserializeFromJson()
    {
        var ordersFromJSON = Serializer.DeserializeFromJson(selectedFilePath).Select(ord => new Order
        {
            Contact = ord.Contact,
            CustomerName = ord.CustomerName,
            Id = ord.Id,
            Date = ord.Date,
            TotalCost = ord.TotalCost,
            PositionsInOrder = ord.PositionsInOrder.Select(pos => new Position
            {
                Name = pos.Name,
                Cost = pos.Cost,
                category = pos.category,
                Quantity = pos.Quantity,
                Id = pos.Id
            }).ToList()
        }).ToList();

        var existingOrders = orderRep.GetAll().ToDictionary(o => o.Id, o => o);

        foreach (var order in ordersFromJSON)
        {
            if (!existingOrders.ContainsKey(order.Id))
            {
                orderRep.Add(order);
            }
        }

        ResultLabel.Text = "Десериализация из JSON произведена успешно!";
    }
}