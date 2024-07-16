using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Vosimerka.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageProduct.xaml
    /// </summary>
    public partial class PageProduct : Page
    {
        IEnumerable<Product> products = MainWindow.vosimerka.Product;
        List<Product> listProducts;
        private int _currentPage = 1;
        private int _countProducts = 20;
        private int _maxPages;
        public PageProduct()
        {
            InitializeComponent();
            lvProduct.ItemsSource = MainWindow.vosimerka.Product.ToList();
            if (products.ToList().Count >= 20)
            {
                pagination.Visibility = Visibility.Visible;
            }
            listProducts = products.ToList();
            Refresh();
            cbFilter.Items.Add("Все типы");
            foreach (ProductType i in MainWindow.vosimerka.ProductType.ToList())
            {
                cbFilter.Items.Add(i.Title);
            }
        }
        private void Refresh()
        {
            listProducts = products.ToList();
            _maxPages = (int)Math.Ceiling(listProducts.Count * 1.0 / _countProducts);

            var listHotelPage = listProducts.Skip((_currentPage - 1) * _countProducts).Take(_countProducts).ToList();

            TxtCurrentPage.Text = _currentPage.ToString();
            LblTotalPages.Text = "из " + _maxPages;
            lvProduct.ItemsSource = listHotelPage;

        }
        private void GoToFirstPage(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            Refresh();
        }

        private void GoToPreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage <= 1) _currentPage = 1;
            else
                _currentPage--;
            Refresh();
        }

        private void GoToNextPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage >= _maxPages) _currentPage = _maxPages;
            else
                _currentPage++;
            Refresh();
        }

        private void GoToLastPage(object sender, RoutedEventArgs e)
        {
            _currentPage = _maxPages;
            Refresh();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentPage = 1;
            Search();
        }
        public void Search()
        {
            if (tbSearch.Text != "")
            {
                products = products
                   .Where(product => product.Title.ToLower().Contains(tbSearch.Text.ToLower()) || product.Description.ToLower().Contains(tbSearch.Text.ToLower()));
            }
            Refresh();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        public void Sort()
        {
            int selectedIndex = cbSort.SelectedIndex;
            if (selectedIndex == 0)
            {
                products = products
                    .OrderBy(prod => prod.Title);
            }
            else if (selectedIndex == 1)
            {
                products = products
                    .OrderBy(prod => prod.ProductionWorkshopNumber);
            }
            else if (selectedIndex == 2)
            {
                products = products
                    .OrderBy(prod => prod.MinCostForAgent);
            }
            Refresh();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPage = 1;
            Filter();
        }

        public void Filter()
        {
            if (cbFilter.SelectedIndex != 0)
            {
                products = products
                    .Where(prod => prod.ProductTypeID == cbFilter.SelectedIndex);
            }
            else
            {
                products = MainWindow.vosimerka.Product;
                Search();
                Sort();
            }
            Refresh();
        }

        private void lvProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvProduct.SelectedItems.Count < 1)
                btAddProductCount.Visibility = Visibility.Collapsed;
            else
                btAddProductCount.Visibility = Visibility.Visible;
        }

        private void btAddClick(object sender, RoutedEventArgs e)
        {
            Product newProduct = new Product();
            newProduct.ID = 0;
            WindowAddProduct windowAddProduct = new WindowAddProduct(newProduct);
            windowAddProduct.ShowDialog();
            Refresh();
        }

        private void btEditClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;
            WindowAddProduct windowAddProduct = new WindowAddProduct(product);
            windowAddProduct.ShowDialog();
            Refresh();
        }

        private void btDeleteClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Product;
            if (product != null)
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    MainWindow.vosimerka.Product.Remove(product);
                    MainWindow.vosimerka.SaveChanges();
                }
            }
            Refresh();
        }

        private void btAddProductCountClick(object sender, RoutedEventArgs e)
        {
            List<Product> products = new List<Product>();
            foreach(Product prod in lvProduct.SelectedItems)
            {
                products.Add(prod);
            }
            WindowAddProductCost windowAddProductCost = new WindowAddProductCost(products);
            windowAddProductCost.ShowDialog();
            this.products = null;
            this.products = MainWindow.vosimerka.Product;
            if (cbFilter.SelectedIndex != -1)
            {
                Filter();
            }
            Sort();
            Search();
            btAddProductCount.Visibility = Visibility.Collapsed;
        }
    }
}
