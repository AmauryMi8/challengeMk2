using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ChallengeMk2.DataBase;
using ChallengeMk2.Models;
using ChallengeMk2.Services;
using Xamarin.Forms;
using Prism;
using Prism.Ioc;
using System.Transactions;
using Prism.Navigation;

namespace ChallengeMk2.ViewModels
{
    public class StarSystemsViewModel : PrismBaseViewModel
    {
        IStarSystemService systemService;

        public StarSystemsViewModel(INavigationService navigationService, IStarSystemService starSystemService) : base(navigationService)
        {

            InitializeViewModel(starSystemService);
        }

        StarSystem selectedSystem;
        public StarSystem SelectedSystem
        {
            set
            {
                SetProperty<StarSystem>(ref selectedSystem, value, onChanged: () =>
                {
                    NavigateTodetailPage(selectedSystem);
                    if (value != null)
                    {
                        selectedSystem = null;
                    }
                });
            }
        }

        public IList<StarSystem> Systems { get; set; }

        public Command DisplaySystemDataCommand { get; set; }

        internal Action<StarSystem> NavigateTodetailPage { get; set; }


        void InitializeViewModel(IStarSystemService starSystemService)
        {
            //GetServices();

            Title = "Systems around SOL";

            Systems = new ObservableCollection<StarSystem>();

            DisplaySystemDataCommand = new Command(async () => await DisplaySystemDataAsync());

            systemService = starSystemService;
        }

        //void GetServices()
        //{
        //    systemService = DependencyService.Get<IStarSystemService>();
        //}

        async Task DisplaySystemDataAsync()
        {
            IsBusy = true;

            try
            {
                Title = GetCurrentTitle(true);

                var data = await systemService.GetStarSystemDataAsync();

                Systems.Clear();
                foreach (var system in data)
                {
                    Systems.Add(system);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                Title = GetCurrentTitle();

                IsBusy = false;
            }
        }

        string GetCurrentTitle(bool isRetreiving = false)
        {
            string title;

            if (isRetreiving)
            {
                title = systemService.GetLocalState() ? "Retreiving local data..." : "Retreiving data from web API...";
            }
            else
            {
                title = systemService.GetLocalState() ? "Systems around SOL (local)" : "Systems around SOL (API)";
            }

            return title;
        }
    }
}
