using Lab1.Services;
namespace Lab1;

public partial class Lab3Page: ContentPage
{
    private IDbService _db;
	public Lab3Page(IDbService service)
	{
		InitializeComponent();
        _db = service;
    }

    public void OnPageLoaded(object sender, EventArgs e)
    {
        this.DBPicker.ItemsSource = _db.GetAllArtists().ToList();
    }
    void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        ContentStackLayout.Clear();
        var item = _db.GetAllArtists().FirstOrDefault(artist => artist.Name == DBPicker.Items[DBPicker.SelectedIndex]);
        var musics = _db.GetAllMusics(item.Id);
        string allMusic = "";
        foreach (var music in musics)
        {
            allMusic += music.NameOfMusic + music.YearOfRelease+"\n";
        };
        Border TypeBorder = new Border
        {
            Stroke = Colors.Gray,
            Content = new Label
            {
                Text = "Type of Music: " + item.typeOfMusic,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            },
            BackgroundColor = Color.FromArgb("#e1e1e1")
        };
        Border MusicsBorder = new Border
        {
            Stroke = Colors.Gray,
            Content = new Label
            {
                Text = "Musics: " + allMusic,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            },
            BackgroundColor = Color.FromArgb("#e1e1e1")
        };
        ContentStackLayout.Add(TypeBorder);
        ContentStackLayout.Add(MusicsBorder);
    }
}