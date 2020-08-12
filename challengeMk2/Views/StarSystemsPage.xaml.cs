﻿using System;
using ChallengeMk2.Models;
using ChallengeMk2.ViewModels;
using Xamarin.Forms;

namespace ChallengeMk2.Views
{
    public partial class StarSystemsPage : ContentPage
    {
        public StarSystemsPage()
        {
            InitializeComponent();
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            var vm = BindingContext as StarSystemsViewModel;

            if (vm!= null)
            {
                vm.NavigateTodetailPage = async (starSystem) => await Navigation.PushAsync(new SystemDetailCarouselPage(new SystemDetailCarouselViewModel(starSystem)));
            }
        }


        //To trigger Auto PullToRefresh collection
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as StarSystemsViewModel;

            if (vm.Systems.Count == 0)
            {
                vm.IsBusy = true;
            }
        }

        //private async void OnSystemTapped(object sender, EventArgs args)
        //{
        //    var layout = (BindableObject)sender;
        //    var system = (StarSystem)layout.BindingContext;



        //    await Navigation.PushAsync(new SystemDetailPage(new SystemDetailViewModel(system)));
        //}

    }
}
