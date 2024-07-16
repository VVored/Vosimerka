using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Vosimerka
{
    /// <summary>
    /// Логика взаимодействия для WindowAddMaterialInProduct.xaml
    /// </summary>
    public partial class WindowAddMaterialInProduct : Window
    {
        Product product;
        public WindowAddMaterialInProduct(Product product)
        {
            InitializeComponent();
            this.product = product;
            dgMaterials.ItemsSource = product.ProductMaterial.ToList();
        }

        private void deleteMaterial(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var productMaterial = button.DataContext as ProductMaterial;
            if (productMaterial != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Вы точно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    MainWindow.vosimerka.ProductMaterial.Remove(productMaterial);
                    MainWindow.vosimerka.SaveChanges();
                    dgMaterials.ItemsSource = null;
                    dgMaterials.ItemsSource = product.ProductMaterial.ToList();
                }
            }
        }

        private void addMaterial(object sender, RoutedEventArgs e)
        {
            ProductMaterial newProductMaterial = new ProductMaterial();
            newProductMaterial.ProductID = product.ID;
            WindowAddMaterial windowAddMaterial = new WindowAddMaterial(newProductMaterial);
            windowAddMaterial.ShowDialog();
            dgMaterials.ItemsSource = null;
            dgMaterials.ItemsSource = product.ProductMaterial.ToList();
        }
    }
}
