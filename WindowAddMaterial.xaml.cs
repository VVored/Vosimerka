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
    /// Логика взаимодействия для WindowAddMaterial.xaml
    /// </summary>
    public partial class WindowAddMaterial : Window
    {
        ProductMaterial productMaterial;
        public WindowAddMaterial(ProductMaterial productMaterial)
        {
            InitializeComponent();
            this.productMaterial = productMaterial;
            DataContext = productMaterial;
            cbMaterials.ItemsSource = MainWindow.vosimerka.Material.ToList();
        }

        private void tbSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            cbMaterials.ItemsSource = null;
            if (tbSearch.Text == "")
            {
                cbMaterials.ItemsSource = MainWindow.vosimerka.Material.ToList();
            }
            else
            {
                cbMaterials.ItemsSource = MainWindow.vosimerka.Material
                    .Where(mat => mat.Title.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            }
        }
        private void saveChanges(object sender, RoutedEventArgs e)
        {
            if (productMaterial.Count >= 0)
            {
                productMaterial.MaterialID = (cbMaterials.SelectedItem as Material).ID;
                MainWindow.vosimerka.ProductMaterial.Add(productMaterial);
                MainWindow.vosimerka.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("Количество не может быть меньше нуля.");
            }
            
        }
    }
}
