using Data.Interfaces;
using Data.SqlServer;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using static Domain.Request;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IRequestRepository _requestRepository = null!;
        private TechnoSoftDbContext _dbContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            var factory = new TechnoSoftDbContextFactory();
            _dbContext = factory.CreateDbContext(configuration);

            _dbContext.Database.Migrate();

            _requestRepository = new RequestRepository(_dbContext);

            SeedInitData();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void SeedInitData()
        {

            if (_requestRepository.GetAll().Any())
            {

                return;
            }


            var requests = new[]
            {
                new Request
                {
                    Number = "REQ-001",
                    Date = DateTime.Now.AddDays(-5),
                    EquipmentType = "Компьютер",
                    EquipmentModel = "Dell Optiplex 7070",
                    ProblemDescription = "Не включается, нет реакции на кнопку питания",
                    ClientFullName = "Иванов Иван Иванович",
                    ClientPhone = "+7(999)123-45-67",
                    Status = RequestStatus.Completed,
                    Engineer = "Петров А.С.",
                    Comments = "Заменен блок питания. Клиент проинформирован."
                },
                new Request
                {
                    Number = "REQ-002",
                    Date = DateTime.Now.AddDays(-3),
                    EquipmentType = "Принтер",
                    EquipmentModel = "HP LaserJet Pro M404dn",
                    ProblemDescription = "Зажевывает бумагу при печати",
                    ClientFullName = "Петрова Мария Сергеевна",
                    ClientPhone = "+7(999)765-43-21",
                    Status = RequestStatus.InProgress,
                    Engineer = "Сидоров В.К.",
                    Comments = "Заказаны ролики подачи бумаги. Ожидаем поставку."
                },
                new Request
                {
                    Number = "REQ-003",
                    Date = DateTime.Now.AddDays(-1),
                    EquipmentType = "Ноутбук",
                    EquipmentModel = "Lenovo ThinkPad X1 Carbon",
                    ProblemDescription = "Не работает Wi-Fi, не видит сети",
                    ClientFullName = "Сидоров Алексей Петрович",
                    ClientPhone = "+7(999)555-44-33",
                    Status = RequestStatus.New,
                    Engineer = "",
                    Comments = "Требуется диагностика"
                },
                new Request
                {
                    Number = "REQ-004",
                    Date = DateTime.Now.AddDays(-2),
                    EquipmentType = "Монитор",
                    EquipmentModel = "Samsung S24R350",
                    ProblemDescription = "Мерцание экрана, горизонтальные полосы",
                    ClientFullName = "Козлова Анна Викторовна",
                    ClientPhone = "+7(999)888-77-66",
                    Status = RequestStatus.WaitingParts,
                    Engineer = "Кузнецов Д.В.",
                    Comments = "Ожидаем поставку матрицы дисплея"
                },
                new Request
                {
                    Number = "REQ-005",
                    Date = DateTime.Now,
                    EquipmentType = "Сервер",
                    EquipmentModel = "HP ProLiant DL380",
                    ProblemDescription = "Перегрев, шумные вентиляторы",
                    ClientFullName = "ООО 'ТехноПрофи'",
                    ClientPhone = "+7(495)123-45-67",
                    Status = RequestStatus.InProgress,
                    Engineer = "Иванова М.П.",
                    Comments = "Произведена чистка, заменены вентиляторы системы охлаждения"
                }
            };

            foreach (var request in requests)
            {
                _requestRepository.Add(request);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {

            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }

}
