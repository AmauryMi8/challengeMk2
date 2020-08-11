﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using challengeMk2.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace challengeMk2.ViewModels
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


        public NetworkAccess CurrentConnectivity { get; set; }

        public IList SystemInfos { get; set; }








        public SystemDetailCarouselViewModel(StarSystem selectedSystem = null)
        {
            Title = "Star System Details";

            SystemInfos = new ObservableCollection<StarSystem>();

            currentSystem = selectedSystem;

            //Get infos
            UpdateSystemData();
        }








        private async void UpdateSystemData()
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
            HttpClient client = new HttpClient();

            // Check system name for special characters : "+" must be replace by "%2b" => Try WebUtility.HtmlEncode(string)
            string encodedName = WebUtility.UrlEncode(currentSystem.Name);

            string url = $"https://www.edsm.net/api-v1/system?systemName={encodedName}&showInformation=1&showPrimaryStar=1&showPermit=1&showCoordinates=1";

            var response = await client.GetStringAsync(url);

            return JsonConvert.DeserializeObject<StarSystem>(response);
        }

        private void GetCompInfos()
        {
            CurrentBodyCount = currentSystem.BodyCount;
            CurrentDistance = currentSystem.Distance;
        }

        private void FillSystemInfos()
        {
            SystemInfos.Add(detailedSystem);
            SystemInfos.Add(detailedSystem);
            SystemInfos.Add(detailedSystem);
        }
    }
}
