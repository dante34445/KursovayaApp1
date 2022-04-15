using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using App1.Models;
using App1.Services;
using App1.Views;
using Xamarin.Forms;


namespace App1.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        private Category _selectedItem;

        public ObservableCollection<Category> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Category> ItemTapped { get; }

        public Command MoveToTopCommand { protected set; get; }
        public Command MoveToBottomCommand{ protected set; get; }
        public Command RemoveCommand { protected set; get; }

        public CategoriesViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Category>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Category>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            MoveToTopCommand = new Command(MoveToTop);
            MoveToBottomCommand= new Command(MoveToBottom);
            RemoveCommand = new Command(Remove);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStoreCategories.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Category SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(CategoryPage));
        }

        async void OnItemSelected(Category item)
        {
            if (item == null)
                return;

            // This will push the CategoryPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(CategoryViewModel.ItemId)}" +
               $"={item.Id}");
        }

        private void MoveToTop(object categoryObj)
        {
            Category category = categoryObj as Category;
            if (category == null) return;
            int oldIndex = Items.IndexOf(category);
            if (oldIndex > 0)
                Items.Move(oldIndex, oldIndex - 1);
        }
        private void MoveToBottom(object categoryObj)
        {
            Category category = categoryObj as Category;
            if (category == null) return;
            int oldIndex = Items.IndexOf(category);
            if (oldIndex < Items.Count - 1)
                Items.Move(oldIndex, oldIndex + 1);
        }
        private void Remove(object categoryObj)
        {
            Category category = categoryObj as Category;
            if (category == null) return;

            DataStoreCategories.DeleteItemAsync(category.Id);

            Items.Remove(category);
        }
    }
}