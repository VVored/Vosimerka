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
    /// Логика взаимодействия для WindowAddProductCost.xaml
    /// </summary>
    public partial class WindowAddProductCost : Window
    {
        List<Product> products;
        public WindowAddProductCost(List<Product> products)
        {
            InitializeComponent();
            this.products = products;
            tbCostValue.Text = MainWindow.vosimerka.Product.Average(prod => prod.MinCostForAgent).ToString();
        }

        private void btAcceptClick(object sender, RoutedEventArgs e)
        {
            foreach (Product prod in products)
            {
                prod.MinCostForAgent += decimal.Parse(tbCostValue.Text);
            }
            MainWindow.vosimerka.SaveChanges();
            this.Close();
        }
    }
}
