using Lab1.Services;
namespace Lab1;



public partial class Lab4Page : ContentPage
{
    private IEnumerable<Rate>? rates;
    private Rate CurrRate;
    private bool SkipTextChanged = false;

    IRateService _rateService;
    public Lab4Page(IRateService rateService)
    {
        InitializeComponent();
        //var serviceProvider = MauiProgram.services.BuildServiceProvider();
        _rateService = rateService;
        this.Entry1.IsEnabled = false;
        this.Entry2.IsEnabled = false;
        this.Currency2.IsEnabled = false;
        this.ConvertButton.IsEnabled = false;
        this.DatePicker.MaximumDate = DateTime.Today;
        rates = new List<Rate>();

    }
    public void Picker2SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Entry2.IsEnabled = true;
        this.Entry1.IsEnabled = true;
        SkipTextChanged = true;
        if (rates == null)
        {
            throw new NullReferenceException(nameof(rates));
        }

        CurrRate = rates.FirstOrDefault(curr => curr.Cur_Name == this.Currency2.SelectedItem.ToString());
        if (CurrRate == null)
        {
            throw new NullReferenceException(nameof(CurrRate));
        }
    }
    public void OnConvertClicked(object sender, EventArgs e)
    {
        this.Entry2.Text = ((Convert.ToDecimal(this.Entry1.Text)) * CurrRate.Cur_OfficialRate / CurrRate.Cur_Scale).ToString();
    }
    public async void DateSelected(object sender, EventArgs e)
    {
        SkipTextChanged = true;
        if (_rateService == null)
        {
            throw new NullReferenceException("Rates on date are null");
        }
        rates = await _rateService.GetRates(this.DatePicker.Date);
        this.Currency2.IsEnabled = true;
        this.ConvertButton.IsEnabled = true;
        SkipTextChanged = false;
    }
}