using CallingScore.Common.Enums;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICodificationHelper _codificationHelper;
        private readonly IArrivalsHelper _arrivalsHelper;
        private readonly ICallHelper _callHelper;

        public SeedDb(DataContext dataContext,
            IUserHelper userHelper,
            ICodificationHelper codificationHelper,
            IArrivalsHelper arrivalsHelper,
            ICallHelper callHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _codificationHelper = codificationHelper;
            _arrivalsHelper = arrivalsHelper;
            _callHelper = callHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckCampaignsAsync();
            UserEntity admin = await CheckUserAsync("cob1yga", "1234", "Yesid", "Garcia", "yesidgarcialopez@gmail.com", "304 329 35 82", UserType.Admin, "Admin");
            UserEntity supervisor = await CheckUserAsync("cob1alg", "1234", "Alexander", "Garcia", "yagarcia1402@gmail.com", "304 329 35 82", UserType.Supervisor, "Linea Salida");
            UserEntity user = await CheckUserAsync("cob1ylg", "1234", "Yesid", "Garcia", "yesidgarcia229967@correo.itm.edu.co", "304 329 35 82", UserType.CallAdviser, "Linea Entrada");
            UserEntity user2 = await CheckUserAsync("cob1ffb", "1234", "Facundo", "Fortunatti", "fortunattibernard@gmail.com", "304 329 35 82", UserType.CallAdviser, "Linea Salida");
            UserEntity user3 = await CheckUserAsync("cob1cml", "1234", "Carolina", "Muñoz", "caroml98@hotmail.com", "304 329 35 82", UserType.CallAdviser, "Linea Salida");
            List<UserEntity> users = new List<UserEntity>();
            users.Add(user);
            users.Add(user2);
            users.Add(user3);
            await CheckCodesAsync();
            await CheckArrivalsAsync(users);
            await CheckCallsAsync(users);
            await CheckMonitoringsAsync(users);
        }

        private async Task<UserEntity> CheckUserAsync(
           string userCode,
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           UserType userType,
           string campaignName)
        {
            UserEntity user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    UserCode = userCode,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                if (campaignName != "Admin")
                {
                    await _userHelper.AddUserToCampaignAsync(user, campaignName);
                }
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Supervisor.ToString());
            await _userHelper.CheckRoleAsync(UserType.CallAdviser.ToString());
        }

        private async Task CheckCodesAsync()
        {
            if (!_dataContext.Codifications.Any())
            {
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "Acuerdo de pago",
                    ContactType = ContactType.AccountHolder,
                    EffectivityType = EffectivityType.Effective,
                    Priority = 1
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "No hubo acuerdo",
                    ContactType = ContactType.AccountHolder,
                    EffectivityType = EffectivityType.Ineffective,
                    Priority = 3
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "Acepta un alivio financiero",
                    ContactType = ContactType.AccountHolder,
                    EffectivityType = EffectivityType.Effective,
                    Priority = 2
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "No se continúa con la llamada",
                    ContactType = ContactType.AccountHolder,
                    EffectivityType = EffectivityType.Ineffective,
                    Priority = 5
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "Llamada no efectiva",
                    ContactType = ContactType.NoContact,
                    EffectivityType = EffectivityType.Ineffective,
                    Priority = 5
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "Mensaje de voz",
                    ContactType = ContactType.NoContact,
                    EffectivityType = EffectivityType.Ineffective,
                    Priority = 4
                });
                await _codificationHelper.AddCode(new CodificationEntity
                {
                    CodeName = "Contacto con un tercero",
                    ContactType = ContactType.NoAccountHolder,
                    EffectivityType = EffectivityType.Ineffective,
                    Priority = 4
                });
            }
        }

        private async Task CheckArrivalsAsync(List<UserEntity> users)
        {
            foreach (UserEntity user in users)
            {
                await _arrivalsHelper.AddArrival(new ArrivalsEntity
                {
                    InDate = new DateTime(2020, 4, 27, 6, 40, 5),
                    OutDate = new DateTime(2020, 4, 27, 6, 40, 5).AddHours(9),
                    User = user
                });

                await _arrivalsHelper.AddArrival(new ArrivalsEntity
                {
                    InDate = new DateTime(2020, 4, 28, 7, 2, 5),
                    OutDate = new DateTime(2020, 4, 28, 7, 2, 5).AddHours(8),
                    User = user
                });

                await _arrivalsHelper.AddArrival(new ArrivalsEntity
                {
                    InDate = new DateTime(2020, 4, 29, 6, 30, 1),
                    OutDate = new DateTime(2020, 4, 29, 6, 30, 1).AddHours(8),
                    User = user
                });
            }
        }

        private async Task CheckCampaignsAsync()
        {
            if (!_dataContext.Campaigns.Any())
            {
                _dataContext.Add(new CampaignEntity { Name = "Linea Entrada" });
                _dataContext.Add(new CampaignEntity { Name = "Linea Salida" });
            }
            await _dataContext.SaveChangesAsync();
        }

        private async Task CheckCallsAsync(List<UserEntity> users)
        {
            Random rnd = new Random();
            if (!_dataContext.Calls.Any())
            {
                foreach (UserEntity user in users)
                {
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 8, 35, 21),
                        EndDate = new DateTime(2020, 4, 27, 8, 35, 21).AddMinutes(rnd.Next(2,5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1,7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Gold Mastercard",
                        User = user
                    });
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 9, 40, 21),
                        EndDate = new DateTime(2020, 4, 27, 9, 40, 21).AddMinutes(rnd.Next(2, 5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1, 7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Gold Mastercard",
                        User = user
                    });
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 10, 30, 21),
                        EndDate = new DateTime(2020, 4, 27, 10, 30, 21).AddMinutes(rnd.Next(2, 5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1, 7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Gold Mastercard",
                        User = user
                    });
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 11, 35, 21),
                        EndDate = new DateTime(2020, 4, 27, 11, 35, 21).AddMinutes(rnd.Next(2, 5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1, 7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Platinum Mastercard",
                        User = user
                    });
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 11, 40, 21),
                        EndDate = new DateTime(2020, 4, 27, 11, 40, 21).AddMinutes(rnd.Next(2, 5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1, 7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Platinum Mastercard",
                        User = user
                    });
                    await _callHelper.AddCall(new CallEntity
                    {
                        StartDate = new DateTime(2020, 4, 27, 12, 30, 21),
                        EndDate = new DateTime(2020, 4, 27, 12, 30, 21).AddMinutes(rnd.Next(2, 5)),
                        Codification = await _codificationHelper.GetCodification(rnd.Next(1, 7)),
                        CustomerId = rnd.Next(1000, 9000).ToString(),
                        CustomerProduct = "Tarjeta Platinum Mastercard",
                        User = user
                    });
                }
            }
        }

        private async Task CheckMonitoringsAsync(List<UserEntity> users)
        {
            Random rnd = new Random();
            if (!_dataContext.Monitorings.Any())
            {
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 29, 11, 30, 00),
                    EndDate = new DateTime(2020, 4, 29, 11, 30, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "La información brindada es correcta, muy buen manejo de objeciones",
                    Score = 5,
                    Call = await _callHelper.GetCall(2),
                    User = users[0]
                });
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 30, 13, 00, 00),
                    EndDate = new DateTime(2020, 4, 30, 13, 00, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "No se informan las politicas de uso",
                    Score = 3,
                    Call = await _callHelper.GetCall(5),
                    User = users[0]
                });
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 29, 10, 30, 00),
                    EndDate = new DateTime(2020, 4, 29, 10, 30, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "La información brindada es correcta",
                    Score = 4,
                    Call = await _callHelper.GetCall(8),
                    User = users[1]
                });
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 30, 13, 00, 00),
                    EndDate = new DateTime(2020, 4, 30, 13, 00, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "No se informan las politicas de uso",
                    Score = 3,
                    Call = await _callHelper.GetCall(12),
                    User = users[1]
                });
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 29, 11, 15, 00),
                    EndDate = new DateTime(2020, 4, 29, 11, 15, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "Se ofrece un alivio financiero no permitido",
                    Score = 1,
                    Call = await _callHelper.GetCall(14),
                    User = users[2]
                });
                await _callHelper.AddScoreToCall(new MonitoringEntity
                {
                    StartDate = new DateTime(2020, 4, 30, 15, 00, 00),
                    EndDate = new DateTime(2020, 4, 30, 15, 00, 00).AddMinutes(rnd.Next(3, 15)),
                    Observations = "No se informan las politicas de uso",
                    Score = 3,
                    Call = await _callHelper.GetCall(16),
                    User = users[2]
                });
            }
            await _dataContext.SaveChangesAsync();
        }
    }
}
