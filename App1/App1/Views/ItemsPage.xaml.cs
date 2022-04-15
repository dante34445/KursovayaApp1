using App1.Models;
using App1.ViewModels;
using App1.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        private bool SlidingPanelIsShow = false;
        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
            Task.Run(AnimateBackground);
        }
            
        private async void AnimateBackground()
        {
            int animatioDuration = 7500;
            var forwadAnimation = new Animation(x => 
            backGradient.AnchorY = x, 0, 1,Easing.Linear);
            var backwardAnimation = new Animation(x => 
            backGradient.AnchorY = x, 1, 0, Easing.Linear);

            while(true)
            {
                forwadAnimation.Commit(backGradient, "forwadAnimation",16U, (uint) animatioDuration, Easing.Linear,null,(() =>false));
                await Task.Delay(animatioDuration);
                backwardAnimation.Commit(backGradient, "backwardAnimation", 16U, (uint) animatioDuration, Easing.Linear, null, (() => false));
                await Task.Delay(animatioDuration);

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            HideSlidingPanel();
        }

        private async void HideSlidingPanel()
        {
            while(this.Height ==  -1)
            {
                await Task.Delay(200);
                SlidingPanel.TranslationY = this.Height;
                SlidingPanelBackground.BackgroundColor = Color.Transparent;
                SlidingPanelBackground.InputTransparent = true;
            }
            SlidingPanel.TranslationY = this.Height;
            SlidingPanelIsShow = false;

        }

        private async void FloatingButton_Clicked(object sender, EventArgs e)
        {
            SwitchSlidingPanel();

            AnimateFloatingItem();
        }

        private void SwitchSlidingPanel()
        {
            if (SlidingPanelIsShow)
            {
                SlidingPanel.TranslateTo(0, this.Height, 250, Easing.SinIn);
                SlidingPanelBackground.BackgroundColor = Color.Transparent;
                SlidingPanelBackground.InputTransparent = true;
            }
            else
            {
                SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height - 80, 250, Easing.SpringOut);
                SlidingPanelBackground.BackgroundColor = Color.FromRgba(55,55,55,99);
                SlidingPanelBackground.InputTransparent = false;
            }
            SlidingPanelIsShow = !SlidingPanelIsShow;
        }

        private async Task AnimateFloatingItem()
        {
            FloatingButton.ScaleTo(0.9, 125);
            await FloatingButton.TranslateTo(0, -5, 125);

            FloatingButton.ScaleTo(1, 125);
            await FloatingButton.TranslateTo(0, 5, 125);
        }

        private void SlidingPanelBackground_Tapped(object sender, EventArgs e)
        {
            SwitchSlidingPanel();
        }

        private void QickMenuButton_Clicked(object sender, EventArgs e)
        {
            SwitchSlidingPanel();
        }

        private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
             
                case SwipeDirection.Up:
                    SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height - 350 - 30, 250, Easing.SinIn);
                    break;
                case SwipeDirection.Down:
                    SlidingPanel.TranslateTo(0, this.Height - QuickMenu.Height - 30, 250, Easing.SinIn);
                    break;
            }
            
        }
    }
}