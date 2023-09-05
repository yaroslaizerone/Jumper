using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
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
    public partial class AgentPage : Page
    {
        private int start = 0;
        private int fullCount = 0;
        public AgentPage()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            agentGrid.ItemsSource = Helper.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            fullCount = Helper.GetContext().Agent.Count();
            full.Text = fullCount.ToString();
            int ost = fullCount % 10;
            int pag = (fullCount - ost) / 10;
            if (ost > 0) pag++;
            pagin.Children.Clear();
            for (int i = 0; i < pag; i++)
            {
                Button myButton = new Button();
                myButton.Height = 30;
                myButton.Content = i + 1;
                myButton.Width = 20;
                myButton.HorizontalAlignment = HorizontalAlignment.Center;
                myButton.Tag = i;
                myButton.Click += new RoutedEventHandler(paginButto_Click); ;
                pagin.Children.Add(myButton);
            }
            turnButton();
        }
        private void turnButton()
        {
            if (start == 0) { back.IsEnabled = false; }
            else { back.IsEnabled = true; };
            if ((start + 1) * 10 > fullCount) { forward.IsEnabled = false; }
            else { forward.IsEnabled = true; };
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (agentGrid.SelectedItems.Count > 0)
            {
                int prt = 0;
                foreach (Agent agent in agentGrid.SelectedItems)
                {
                    if (agent.Priority > prt) prt = agent.Priority;
                }
                WindowModal dlg = new WindowModal(prt);
                helper.prioritet = prt;
                helper.flag = false;
                dlg.ShowDialog();
                if (helper.flag)
                {
                    foreach (Agent agent in agentGrid.SelectedItems)
                    {
                        agent.Priority = helper.prioritet;
                        helper.GetContext().Entry(agent).State = EntityState.Modified;
                    }
                    helper.GetContext().SaveChanges();
                    Load();
                }
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void revButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void forward_Click(object sender, RoutedEventArgs e)
        {
            start++;
            Load();
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            start--;
            Load();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SelectAgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
