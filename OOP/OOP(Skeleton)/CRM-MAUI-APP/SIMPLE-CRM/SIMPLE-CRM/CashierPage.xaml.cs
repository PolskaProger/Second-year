using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using CoreSIMPLECRM.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SIMPLE_CRM;

public partial class CashierPage : ContentPage
{
    private CorrectInputService correctInputService;
    private Category category;
    private Position position;
    private Order order = new Order();
    private CategoryRep categoryRep;
    private PositionRep positionRep;
    private OrderRep orderRep;
    private List<Position> positionsInOrder;
    private Order selectedOrder;
    public ObservableCollection<Position> Positions { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public CashierPage()
	{
		InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        correctInputService = new CorrectInputService();
        orderRep = new OrderRep(dataStorage); // Adjust as needed
        categoryRep = new CategoryRep(dataStorage); // Adjust as needed
        positionRep = new PositionRep(dataStorage); // Adjust as needed
        positionsInOrder = new List<Position>();
        LoadCategories();
        LoadOrders();
    }
    private async void LoadCategories()
    {
        var categories = categoryRep.GetAll();
        CategoryPicker.ItemsSource = (System.Collections.IList)categories;
        NewCategoryPicker.ItemsSource = (System.Collections.IList)categories;
    }
    private void LoadOrders()
    {
        var orders = orderRep.GetAll();
        OrderPicker.ItemsSource = (System.Collections.IList)orders;
        DelOrderPicker.ItemsSource = (System.Collections.IList)orders;
    }

    private void OnCategorySelected(object sender, EventArgs e)
    {
        var selectedCategory = (Category)CategoryPicker.SelectedItem;
        if (selectedCategory != null)
        {
            var positions = Position.GetAllPositionsInCategory(selectedCategory.Name);
            PositionPicker.ItemsSource = positions;
        }
    }

    private async void OnAddPositionClicked(object sender, EventArgs e)
    {
        var selectedPosition = (Position)PositionPicker.SelectedItem;
        if (selectedPosition == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите позицию.", "OK");
            return;
        }

        if (!int.TryParse(PositionQuantityEntry.Text, out int quantity) || quantity <= 0)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите корректное количество.", "OK");
            return;
        }

        var positionToAdd = new Position
        {
            Name = selectedPosition.Name,
            Cost = selectedPosition.Cost,
            category = selectedPosition.category,
            Quantity = quantity
        };

        positionsInOrder.Add(positionToAdd);
        PositionsCollectionView.ItemsSource = null;
        PositionsCollectionView.ItemsSource = positionsInOrder;
    }

    private async void OnCreateOrderClicked(object sender, EventArgs e)
    {
        var customerName = CustomerNameEntry.Text;
        var contactInfo = ContactInfoEntry.Text;
        var dateStr = OrderDateEntry.Text;

        if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(contactInfo) || string.IsNullOrEmpty(dateStr))
        {
            await DisplayAlert("Ошибка", "Все поля должны быть заполнены", "OK");
            return;
        }

        if (!correctInputService.ValidateEmail(contactInfo))
        {
            await DisplayAlert("Ошибка", "Ошибка ввода email! Поддерживаются форматы:...@gmail.com, ...@mail.ru, ...@yandex.by", "OK");
            return;
        }

        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfOrder))
        {
            await DisplayAlert("Ошибка", "Некорректный формат даты.", "OK");
            return;
        }
        Order newOrder = order.CreateOrder(customerName, contactInfo, dateOfOrder, positionsInOrder);
        orderRep.Add(newOrder);
        await DisplayAlert("Успех", $"Заказ на имя {customerName} создан!", "OK");
        await Navigation.PopAsync();
    }

    //ОБНОВЛЕНИЕ ЗАКАЗА
    private void OnOrderSelected(object sender, EventArgs e)
    {
        selectedOrder = (Order)OrderPicker.SelectedItem;
        if (selectedOrder != null)
        {
            NewCustomerNameEntry.Text = selectedOrder.CustomerName;
            NewContactInfoEntry.Text = selectedOrder.Contact;
            NewOrderDateEntry.Text = selectedOrder.Date.ToString("dd.MM.yyyy");
            NewPositionsCollectionView.ItemsSource = selectedOrder.PositionsInOrder;
        }
    }

    private void OnCategorySelectedUpdate(object sender, EventArgs e)
    {
        var selectedCategory = (Category)NewCategoryPicker.SelectedItem;
        if (selectedCategory != null)
        {
            var positions = Position.GetAllPositionsInCategory(selectedCategory.Name);
            NewPositionPicker.ItemsSource = positions;
        }
    }

    private async void OnAddPositionClickedUpdate(object sender, EventArgs e)
    {
        var selectedPosition = (Position)NewPositionPicker.SelectedItem;
        if (selectedPosition == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите позицию.", "OK");
            return;
        }

        if (!int.TryParse(NewPositionQuantityEntry.Text, out int quantity) || quantity <= 0)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, введите корректное количество.", "OK");
            return;
        }

        var positionToAdd = new Position
        {
            Name = selectedPosition.Name,
            Cost = selectedPosition.Cost,
            category = selectedPosition.category,
            Quantity = quantity
        };

        selectedOrder.PositionsInOrder.Add(positionToAdd);
        NewPositionsCollectionView.ItemsSource = null;
        NewPositionsCollectionView.ItemsSource = selectedOrder.PositionsInOrder;
    }

    private async void OnDeletePositionClickedUpdate(object sender, EventArgs e)
    {
        var selectedPosition = (Position)NewPositionsCollectionView.SelectedItem;
        if (selectedPosition == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите позицию для удаления.", "OK");
            return;
        }

        selectedOrder.PositionsInOrder.Remove(selectedPosition);
        NewPositionsCollectionView.ItemsSource = null;
        NewPositionsCollectionView.ItemsSource = selectedOrder.PositionsInOrder;
    }

    private async void OnUpdateOrderClicked(object sender, EventArgs e)
    {
        var newCustomerName = NewCustomerNameEntry.Text;
        var newContactInfo = NewContactInfoEntry.Text;
        var dateStr = NewOrderDateEntry.Text;

        if (string.IsNullOrEmpty(newCustomerName) || string.IsNullOrEmpty(newContactInfo) || string.IsNullOrEmpty(dateStr))
        {
            await DisplayAlert("Ошибка", "Все поля должны быть заполнены", "OK");
            return;
        }

        if (!correctInputService.ValidateEmail(newContactInfo))
        {
            await DisplayAlert("Ошибка", "Ошибка ввода email! Поддерживаются форматы:...@gmail.com, ...@mail.ru, ...@yandex.by", "OK");
            return;
        }

        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime NewdateOfOrder))
        {
            await DisplayAlert("Ошибка", "Некорректный формат даты.", "OK");
            return;
        }

        selectedOrder.CustomerName = newCustomerName;
        selectedOrder.Contact = newContactInfo;
        selectedOrder.Date = NewdateOfOrder;
        selectedOrder.TotalCost = CalculateTotalCost(selectedOrder.PositionsInOrder);
        orderRep.Update(selectedOrder);
        await DisplayAlert("Успех", "Заказ успешно обновлен!", "OK");
        await Navigation.PopAsync();
    }
    //Удаление заказа
    private async void OnDeleteOrderClicked(object sender, EventArgs e)
    {
        var selectedOrder = (Order)DelOrderPicker.SelectedItem;
        if (selectedOrder == null)
        {
            await DisplayAlert("Ошибка", "Пожалуйста, выберите заказ.", "OK");
            return;
        }

        bool confirmDelete = await DisplayAlert("Подтверждение", $"Вы уверены, что хотите удалить заказ на имя {selectedOrder.CustomerName}?", "Да", "Нет");
        if (confirmDelete)
        {
            orderRep.Delete(selectedOrder.Id);
            await DisplayAlert("Успех", "Заказ успешно удалён!", "OK");
            LoadOrders(); // Refresh the orders list
        }
    }

    private float CalculateTotalCost(List<Position> positions)
    {
        float totalCost = 0;
        foreach (var position in positions)
        {
            totalCost += position.Cost * position.Quantity;
        }
        return totalCost;
    }
}
