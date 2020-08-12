﻿using System;
using System.Collections.Generic;

using ChallengeMk2.ViewModels;
using Xamarin.Forms;

namespace ChallengeMk2.Views
{
    public partial class SystemDetailCarouselPage : ContentPage
    {
        public SystemDetailCarouselPage(SystemDetailCarouselViewModel selectedSystemViewModel)
        {
            InitializeComponent();

            BindingContext = selectedSystemViewModel;
        }
    }
}
