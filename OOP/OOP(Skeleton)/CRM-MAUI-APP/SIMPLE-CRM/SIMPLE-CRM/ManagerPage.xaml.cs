using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using System.Collections.ObjectModel;

namespace SIMPLE_CRM;

public partial class ManagerPage : ContentPage
{
    private Category category;
    private Position position;
    private CategoryRep categoryRep;
    private PositionRep positionRep;
    public ObservableCollection<Position> Positions  { get; set; }
    public ObservableCollection<Category> Categories { get; set; }
    public string IdOfUpdateCategory { get; set; }
    public string IdOfDeleteCategory { get; set; }
    public string IdOfCategoryForPos { get; set; }
    public string IdOfUpdatePosition { get; set; }
    public string IdOfDeletePosition { get; set; }
    public ManagerPage()
	{
        Shell.Current.FlyoutIsPresented = false;
        InitializeComponent();
        var connectionString = "mongodb://localhost:27017";
        var dbName = "CRM-Database";
        var dataStorage = new DataStorage(connectionString, dbName);
        this.categoryRep = new CategoryRep(dataStorage);
        this.positionRep = new PositionRep(dataStorage);
        this.IdOfUpdateCategory = null;
        this.IdOfDeleteCategory = null;
        this.IdOfCategoryForPos = null;
        this.IdOfUpdatePosition = null;
        this.IdOfDeletePosition = null;
        this.Categories = new ObservableCollection<Category>(this.categoryRep.GetAll());
        categoryPicker.ItemsSource = Categories;
        OldCatPicker.ItemsSource = Categories;
        DelCatPicker.ItemsSource = Categories;
        CatPickerForPos.ItemsSource = Categories;
        UpdatePosPicker.ItemsSource = null;
        DeletePosPicker.ItemsSource = null;
        this.position = new Position();
        this.category = new Category();
    }
    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        var categoryName = NewCatName.Text;

        if (string.IsNullOrEmpty(categoryName))
        {
            await DisplayAlert("Ошибка", "Введите название категории", "OK");
            return;
        }

        var existingCategory = Category.categories.Find(c => c.Name == categoryName);
        if (existingCategory != null)
        {
            await DisplayAlert("Ошибка", "Категория с таким именем уже существует", "OK");
            return;
        }
        Category newCategory = category.CreateCategory(categoryName);
        categoryRep.Add(newCategory);
        categoryPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        OldCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        DelCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        CatPickerForPos.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        await DisplayAlert("Успех", $"Категория {categoryName} добавлена", "OK");
    }

    private async void OnUpdateCategoryClicked(object sender, EventArgs e)
    {
        var CategoryUpdateId = this.IdOfUpdateCategory;
        var newCategoryName = NewNameOfCat.Text;

        if (string.IsNullOrEmpty(CategoryUpdateId) || string.IsNullOrEmpty(newCategoryName))
        {
            await DisplayAlert("Ошибка", "Введите новое названия категории", "OK");
            return;
        }

        var categoryToUpdate = categoryRep.GetById(CategoryUpdateId);
        if (categoryToUpdate == null)
        {
            await DisplayAlert("Ошибка", "Категория с таким старым именем не существует", "OK");
            return;
        }

        categoryToUpdate.Name = newCategoryName;
        categoryRep.Update(categoryToUpdate);
        categoryPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        OldCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        DelCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        CatPickerForPos.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        await DisplayAlert("Успех", $"Категория изменена на {newCategoryName}", "OK");
    }

    private async void OnDeleteCategoryClicked(object sender, EventArgs e)
    {
        var selectedCategory = this.IdOfDeleteCategory;
        if (string.IsNullOrEmpty(selectedCategory))
        {
            await DisplayAlert("Ошибка", "Введите название категории для удаления", "OK");
            return;
        }
        Category categoryToDelete = categoryRep.GetById(selectedCategory);
        if (categoryToDelete == null)
        {
            await DisplayAlert("Ошибка", "Категория с таким именем не существует", "OK");
            return;
        }
        categoryToDelete.DeleteCategory(selectedCategory);
        categoryRep.Delete(selectedCategory);
        categoryPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        OldCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        DelCatPicker.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        CatPickerForPos.ItemsSource = (System.Collections.IList)categoryRep.GetAll();
        await DisplayAlert("Успех", $"Категория была удалена", "OK");
    }

    private void OnAddPositionClicked(object sender, EventArgs e)
    {
        var positionName = NewPosName.Text;
        var positionCostText = NewPosCost.Text;
        var categoryName = ((Category)CatPickerForPos.SelectedItem).Name;

        if (string.IsNullOrEmpty(positionName) || string.IsNullOrEmpty(positionCostText) || string.IsNullOrEmpty(categoryName))
        {
            DisplayAlert("Ошибка", "Введите имя, стоимость и выберите категорию новой позиции", "OK");
            return;
        }

        if (!float.TryParse(positionCostText, out float positionCost))
        {
            DisplayAlert("Ошибка", "Стоимость должна быть числом", "OK");
            return;
        }
        Position newPos = position.CreatePosition(positionName, positionCost, categoryName);
        positionRep.Add(newPos);
        Positions = new ObservableCollection<Position>(Position.GetAllPositionsInCategory(categoryName));
        positionsCollectionView.ItemsSource = Positions;
        DisplayAlert("Успех", $"Позиция {positionName} добавлена в категорию {categoryName} и добавлена в БД", "OK");
    }

    private async void OnUpdatePositionClicked(object sender, EventArgs e)
    {
        var categoryName = ((Category)CatPickerForPos.SelectedItem).Name;
        var selectedPosition = this.IdOfUpdatePosition;
        if (selectedPosition == null)
        {
            await DisplayAlert("Ошибка", "Выберите позицию для обновления", "OK");
            return;
        }
        var newPositionName = NewNameOfPos.Text;
        if (string.IsNullOrEmpty(newPositionName))
        {
            await DisplayAlert("Ошибка", "Введите новое имя позиции", "OK");
            return;
        }

        if (!float.TryParse(NewCostOfPos.Text, out float newPositionCost))
        {
            await DisplayAlert("Ошибка", "Стоимость должна быть числом", "OK");
            return;
        }

        bool UpdatedPosBool = position.EditPosition(selectedPosition, newPositionName, newPositionCost);
        if (UpdatedPosBool == false) 
        {
            await DisplayAlert("Ошибка", "Позиция не была обновлена", "OK");
            return;
        }
        Position UpdatedPos = Position.positions.Find(p => p.Name==newPositionName);
        if (UpdatedPos == null)
        {
            await DisplayAlert("Ошибка", "Позиция не была обновлена", "OK");
            return;
        }
        positionRep.Update(UpdatedPos);
        Positions = new ObservableCollection<Position>(Position.GetAllPositionsInCategory(categoryName));
        positionsCollectionView.ItemsSource = Positions;
        await DisplayAlert("Успех", "Позиция была обновлена", "OK");
    }

    private async void OnDeletePositionClicked(object sender, EventArgs e)
    {
        var categoryName = ((Category)CatPickerForPos.SelectedItem).Name;
        var selectedPosition = IdOfDeletePosition;
        if (selectedPosition == null)
        {
            await DisplayAlert("Ошибка", "Выберите позицию для удаления", "OK");
            return;
        }
        if (Position.positions.Find(p => p.Id == selectedPosition) == null)
        {
            await DisplayAlert("Ошибка", "Данной позиции не сущетсвует", "OK");
            return;
        }
        position.DeletePosition(selectedPosition);
        positionRep.Delete(selectedPosition);
        Positions = new ObservableCollection<Position>(positionRep.GetAllInCategory(categoryName));
        positionsCollectionView.ItemsSource = Positions;
        await DisplayAlert("Успех", "Позиция была удалена", "OK");
    }
    private void OnCategorySelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Category)picker.SelectedItem;
        Positions = new ObservableCollection<Position>(positionRep.GetAllInCategory(selectedCategory.Name));
        positionsCollectionView.ItemsSource = Positions;
    }
    private void OnCategoryUpdateSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Category)picker.SelectedItem;
        if (selectedCategory == null)
        {
            return;
        }
        this.IdOfUpdateCategory = selectedCategory.Id;
    }
    private void OnCategoryDeleteSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Category)picker.SelectedItem;
        if (selectedCategory == null)
        {
            return;
        }
        this.IdOfDeleteCategory = selectedCategory.Id;
    }
    private void OnCatForPosSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Category)picker.SelectedItem;
        if (selectedCategory == null)
        {
            return;
        }
        UpdatePosPicker.ItemsSource=new ObservableCollection<Position>(this.positionRep.GetAllInCategory(selectedCategory.Name));
        DeletePosPicker.ItemsSource = new ObservableCollection<Position>(this.positionRep.GetAllInCategory(selectedCategory.Name));
        this.IdOfCategoryForPos = selectedCategory.Id;
    }
    private void OnUpdatePosPickerSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Position)picker.SelectedItem;
        if (selectedCategory == null)
        {
            return;
        }
        this.IdOfUpdatePosition = selectedCategory.Id;
    }
    private void OnDeletePosPickerSelected(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedCategory = (Position)picker.SelectedItem;
        if (selectedCategory == null)
        {
            return;
        }
        this.IdOfDeletePosition = selectedCategory.Id;
    }
}