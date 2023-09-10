using Jumper.Classes;
using Jumper.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Jumper
{
    public partial class EditPage : Page
    {
        private Agent EditAgent;
        private int curSelPr = 0;
        private int curTypAg = 0;

        public EditPage(Agent agent)
        {
            InitializeComponent();
            try
            {
                TypeAgent.ItemsSource = Entities.GetContext().AgentType.ToList();
                ProductList.ItemsSource = Entities.GetContext().Product.ToList();
            }
            catch { };
            if (agent != null)
            {
                EditAgent = agent;
                TypeAgent.SelectedItem = agent.AgentType;
                this.Title.Text = agent.Title;
                this.Adress.Text = agent.Address;
                this.Inn.Text = agent.INN;
                this.Kpp.Text = agent.KPP;
                this.Director.Text = agent.DirectorName;
                this.Phone.Text = agent.Phone;
                this.Logo.Text = agent.Logo;
                this.Email.Text = agent.Email;
                this.Prioritet.Text = agent.Priority.ToString();
                historyGrid.ItemsSource = Entities.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == EditAgent.ID).ToList();
            }
            else
            {
                EditAgent = new Agent();
                btnDelAg.IsEnabled = false;
                btnWritHi.IsEnabled = false;
                btnDelHi.IsEnabled = false;
            }
            this.DataContext = EditAgent;

        }

        private void btnWriteAg_Click(object sender, RoutedEventArgs e)
        {
            if (curTypAg == 0) { MessageBox.Show("Выберите Тип!"); return; }
            if (this.Title.Text == "") { MessageBox.Show("Введите наименование"); return; }
            if (!(new Regex(@"\d{10}|\d{12}")).IsMatch(this.Inn.Text)) { MessageBox.Show("Неверный ИНН"); return; }
            if (!(new Regex(@"\d{4}[\dA-Z][\dA-Z]\d{3}")).IsMatch(this.Kpp.Text)) { MessageBox.Show("Неверный КПП"); return; }
            if ((this.Email.Text != "") && (!(new Regex(@"(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)")).IsMatch(this.Email.Text))) { MessageBox.Show("Неверный E-Mail"); return; }
            EditAgent.Title = this.Title.Text;
            EditAgent.AgentTypeID = curTypAg;
            EditAgent.Address = this.Adress.Text;
            EditAgent.INN = this.Inn.Text;
            EditAgent.KPP = this.Kpp.Text;
            EditAgent.Phone = this.Phone.Text;
            EditAgent.DirectorName = this.Director.Text;
            EditAgent.Phone = this.Phone.Text;
            EditAgent.Email = this.Email.Text;
            EditAgent.Logo = this.Logo.Text;
            try
            {
                EditAgent.Priority = Convert.ToInt32(this.Prioritet.Text);
            }
            catch
            {
                return;
            }
            try
            {
                if (EditAgent.ID > 0)
                {
                    Entities.GetContext().Entry(EditAgent).State = EntityState.Modified;
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Обновление информации об агенте завершено");
                }
                else
                {
                    if (EditAgent.ID == 0 || EditAgent == null)
                    {
                        EditAgent.ID = Entities.GetContext().Agent.Count() + 1;
                    }
                    Entities.GetContext().Agent.Add(EditAgent);
                    Entities.GetContext().SaveChanges();
                    MessageBox.Show("Добавление информации об агенте завершено");
                }
                btnDelAg.IsEnabled = true;
                btnWritHi.IsEnabled = true;
                btnDelHi.IsEnabled = true;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); };
        }

        private void btnDelAg_Click(object sender, RoutedEventArgs e)
        {
            if (EditAgent.ProductSale.Count > 0)
            {
                MessageBox.Show("Удаление не возможно!");
                return;
            }
            foreach (Shop shop in EditAgent.Shop)
            {
                Entities.GetContext().Shop.Remove(shop);
            }
            foreach (AgentPriorityHistory apr in EditAgent.AgentPriorityHistory)
            {
                Entities.GetContext().AgentPriorityHistory.Remove(apr);
            }
            Entities.GetContext().Agent.Remove(EditAgent);
            Entities.GetContext().SaveChanges();
            MessageBox.Show("Удаление информации об агенте завешено!");
            this.NavigationService.Navigate(new AgentPage());

        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curTypAg = ((AgentType)TypeAgent.SelectedItem).ID;
        }

        private void product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            curSelPr = ((Product)ProductList.SelectedItem).ID;
        }

        private void btnWritHi_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;
            try
            {
                cnt = Convert.ToInt32(CountProduct.Text);
            }
            catch
            {
                return;
            }
            string dt = CountProduct.ToString();
            if (curSelPr > 0 && dt != "" && cnt > 0)
            {
                ProductSale pr = new ProductSale();
                pr.ID = Entities.GetContext().ProductSale.Count()+1;
                pr.AgentID = EditAgent.ID;
                pr.ProductID = curSelPr;
                pr.SaleDate = (DateTime)DateSale.SelectedDate;
                pr.ProductCount = cnt;
                try
                {
                    Entities.GetContext().ProductSale.Add(pr);
                    Entities.GetContext().SaveChanges();
                    historyGrid.ItemsSource = Entities.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == EditAgent.ID).ToList();
                }
                catch
                {
                    return;
                }
            }
        }
        private void btnDelHi_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < historyGrid.SelectedItems.Count; i++)
            {
                ProductSale prs = historyGrid.SelectedItems[i] as ProductSale;
                if (prs != null)
                {
                    Entities.GetContext().ProductSale.Remove(prs);
                }
            }
            try
            {
                Entities.GetContext().SaveChanges();
                historyGrid.ItemsSource = Entities.GetContext().ProductSale.Where(ProductSale => ProductSale.AgentID == EditAgent.ID).ToList();
            }
            catch { return; };

        }

        private void SearchMask_TextChanged(object sender, TextChangedEventArgs e)
        {
            string fnd = ((TextBox)sender).Text;
            try
            {
                ProductList.ItemsSource = Entities.GetContext().Product.Where(Product => Product.Title.Contains(fnd)).ToList();
            }
            catch { };
        }
    }
}
