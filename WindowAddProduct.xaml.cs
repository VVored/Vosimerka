using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Vosimerka
{
    /// <summary>
    /// Логика взаимодействия для WindowAddProduct.xaml
    /// </summary>
    public partial class WindowAddProduct : Window
    {
        Product product;
        public WindowAddProduct(Product product)
        {
            InitializeComponent();
            this.product = product;
            DataContext = product;
            cbTypeOfProduct.ItemsSource = MainWindow.vosimerka.ProductType.ToList();
            cbImages.ItemsSource = MainWindow.vosimerka.Product.Where(prod => prod.Image != "нет").ToList();
        }

        private void btSaveChangesClick(object sender, RoutedEventArgs e)
        {
            if (product.ID == 0)
            {
                bool duplicateArticle = false;
                List<Product> productList = MainWindow.vosimerka.Product.ToList();
                foreach (Product prod in productList)
                {
                    if (product.ArticleNumber == prod.ArticleNumber)
                    {
                        duplicateArticle = true;
                    }
                }
                if (duplicateArticle == false)
                {
                    MainWindow.vosimerka.Product.Add(product);
                }
                else
                {
                    MessageBox.Show("Артикул существует.");
                }
            }
            MainWindow.vosimerka.SaveChanges();
            this.Close();
        }

        private void brDeleteClick(object sender, RoutedEventArgs e)
        {
            if (product.ID != 0)
            {
                MainWindow.vosimerka.Product.Remove(product);
                MainWindow.vosimerka.SaveChanges();
                this.Close();
            }
            else
            {
                MessageBox.Show("Вы не можете удалить элемент");
            }
        }

        private void btMaterialInProductClick(object sender, RoutedEventArgs e)
        {
            WindowAddMaterialInProduct windowAddMaterialInProduct = new WindowAddMaterialInProduct(product);
            windowAddMaterialInProduct.ShowDialog();
        }

        private void btLoadImage(object sender, RoutedEventArgs e)
        {
            try
            {
                string filename = "";
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".jpg";
                dialog.Filter = "Images (*.png;*.jpg;*jpeg)|*.png;*.jpg;*jpeg";

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    filename = dialog.FileName;
                    string fileTitle = Path.GetFileName(filename);
                    string fileTitleWithoutExtension = Path.GetFileNameWithoutExtension(filename);
                    string path = AppDomain.CurrentDomain.BaseDirectory + "..\\.." + "\\products\\" + fileTitle;
                    if (!File.Exists(path))
                        File.Copy(filename, path, true);

                    var newImgs = MainWindow.vosimerka.Product.Where(prod => prod.Image != "нет").ToList();
                    product.Image = "\\products\\" + fileTitleWithoutExtension;
                    newImgs.Add(product);
                    MainWindow.vosimerka.SaveChanges();
                    cbImages.ItemsSource = newImgs;
                    cbImages.SelectedItem = product;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
