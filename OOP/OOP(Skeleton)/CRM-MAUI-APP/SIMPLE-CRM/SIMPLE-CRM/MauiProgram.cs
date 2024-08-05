using CommunityToolkit.Maui;
using CoreSIMPLECRM.DataLayer;
using CoreSIMPLECRM.LogicLayer;
using CoreSIMPLECRM.Services;
using Microsoft.Extensions.Logging;

namespace SIMPLE_CRM
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var connectionString = "mongodb://localhost:27017";
            var dbName = "CRM-Database";
            var dataStorage = new DataStorage(connectionString, dbName);
            var categoryRep = new CategoryRep(dataStorage);
            var userRep = new UserRep(dataStorage);
            var positionRep = new PositionRep(dataStorage);
            var orderRep = new OrderRep(dataStorage);

            bool exit = false;

            User user = new User();
            Category category = new Category();
            Position position = new Position();
            Order order = new Order();


            IEnumerable<Category> AllCategories = categoryRep.GetAll();
            Category.categories.AddRange(AllCategories);
            IEnumerable<User> AllUsers = userRep.GetAll();
            User.users.AddRange(AllUsers);
            IEnumerable<Position> AllPositions = positionRep.GetAll();
            Position.positions.AddRange(AllPositions);
            IEnumerable<Order> AllOrders = orderRep.GetAll();
            Order.orders.AddRange(AllOrders);

            Serializer serializer = new Serializer();

            CorrectInputService correct_input = new CorrectInputService();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<CorrectInputService>();
            builder.Services.AddSingleton<UserRep>();
            return builder.Build();
        }
    }
}
