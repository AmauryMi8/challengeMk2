﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ChallengeMk2.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace ChallengeMk2.ViewModels
{
    public class SystemDetailCarouselViewModel : BaseViewModel
    {
        private StarSystem currentSystem;
        public StarSystem CurrentSystem
        {
            get => currentSystem;
            set
            {
                SetProperty<StarSystem>(ref currentSystem, value);
            }
        }

        private StarSystem detailedSystem;
        public StarSystem DetailedSystem
        {
            get => detailedSystem;
            set
            {
                SetProperty<StarSystem>(ref detailedSystem, value);
            }
        }


        private int currentBodyCount;
        public int CurrentBodyCount
        {
            get => currentBodyCount;
            set
            {
                SetProperty<int>(ref currentBodyCount, value);
            }
        }

        private double currentDistance;
        public double CurrentDistance
        {
            get => currentDistance;
            set
            {
                SetProperty<double>(ref currentDistance, value);
            }
        }

        private string currentBanner;
        public string CurrentBanner
        {
            get => currentBanner;
            set
            {
                SetProperty<string>(ref currentBanner, value);
            }
        }

        public Command SwitchBannerCommand { get; set; }

        public NetworkAccess CurrentConnectivity { get; set; }

        public ObservableCollection<StarSystem> SystemInfos { get; set; }


        private string[] banners;



        //CONSTRUCTOR
        public SystemDetailCarouselViewModel(StarSystem selectedSystem = null)
        {
            Title = "Star System Details";

            SystemInfos = new ObservableCollection<StarSystem>();

            currentSystem = selectedSystem;

            banners = new string[]
            {
                "BannerDetail_01",
                "BannerDetail_08",
                "BannerDetail_04",
                "BannerDetail_07"
            };

            SwitchBannerCommand = new Command<int>(p => SwitchBanner(p));
            currentBanner = banners[0];
        }



        //PRIVATE METHODS
        internal async Task UpdateSystemData()
        {
            CurrentConnectivity = Connectivity.NetworkAccess;

            //1° Get details from online API
            if (CurrentConnectivity == NetworkAccess.Internet)
            {
                DetailedSystem = await GetDetailsFromApi();
            }
            else
            {
                DetailedSystem.Name = "No internet connection ! Try again later...";
                return;
            }

            //2° Get distance and number of bodies from currentSystem and put them in detailedSystem
            GetCompInfos();

            //3° Fill collection for Carousel : Copy 3 times detailed system in SystemInfos (one for each tab)
            FillSystemInfos();
        }

        private async Task<StarSystem> GetDetailsFromApi()
        {
            // Check system name for special characters : "+" must be replace by "%2b" => Try WebUtility.HtmlEncode(string)
            string encodedName = WebUtility.UrlEncode(currentSystem.Name);

            string url = $"https://www.edsm.net/api-v1/system?systemName={encodedName}&showInformation=1&showPrimaryStar=1&showPermit=1&showCoordinates=1";


            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);

                return JsonConvert.DeserializeObject<StarSystem>(response);
            } 
        }

        private void GetCompInfos()
        {
            CurrentBodyCount = currentSystem.BodyCount;
            CurrentDistance = currentSystem.Distance;
        }

        private void FillSystemInfos()
        {
            SystemInfos.Add(GetSystemWithIdSelector(0));
            SystemInfos.Add(GetSystemWithIdSelector(1));
            SystemInfos.Add(GetSystemWithIdSelector(2));
            SystemInfos.Add(GetSystemWithIdSelector(3));
        }

        private StarSystem GetSystemWithIdSelector(int id)
        {
            StarSystem systemWithId = new StarSystem
            {
                DataSelectorID = id,

                Distance = currentSystem.Distance,
                BodyCount = currentSystem.BodyCount,
                Name = detailedSystem.Name,
                RequirePermit = detailedSystem.RequirePermit,
                PermitName = detailedSystem.PermitName,
                Information = detailedSystem.Information,
                PrimaryStar = detailedSystem.PrimaryStar,
                Coords = detailedSystem.Coords
            };

            return systemWithId;
        }

        private void SwitchBanner(int position)
        {
            CurrentBanner = banners[position];
        }
    }
}
