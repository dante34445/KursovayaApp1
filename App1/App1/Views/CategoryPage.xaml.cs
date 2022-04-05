using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.ViewModels;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        private CategoryViewModel viewModel;
        public CategoryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CategoryViewModel();
        }
    }
}