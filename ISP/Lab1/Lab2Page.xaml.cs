using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Diagnostics;

namespace Lab1;

public partial class Lab2page : ContentPage
{
    public Lab2page()
    {
        InitializeComponent();
    }

    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();


    public void CancelClicked(object sender, EventArgs e)
    {
        cancelTokenSource.Cancel();
    }

    private async void StartClicked(object sender, EventArgs e)
    {
        cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;
        double step = 0.001;
        double start = 0;
        double end = 1;
        double result = 0;


        await Task.Run(async () =>

        {
            for (double i = start; i < end; i += step)
            {
                if (token.IsCancellationRequested)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        this.progBar.Progress = 0;
                        this.pgLabel.Text = "Cancelled";
                    });
                    return;
                }
                await Task.Delay(1);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    this.progBar.Progress = i / end;
                    this.ProgNum.Text = ((int)(i / end * 100)).ToString() + "%";
                });
                result += Math.Sin(i) * step;

            }
        });

        if (token.IsCancellationRequested)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                this.progBar.Progress = 0;
                this.ProgNum.Text = "0%";
                this.pgLabel.Text = "Cancelled";
            });
            cancelTokenSource.Dispose();
            return;
        }
        this.ProgNum.Text = "100%";
        this.pgLabel.Text = result.ToString();

    }
}