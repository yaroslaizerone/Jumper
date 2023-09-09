using Jumper.Classes;
using Jumper.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;

namespace Jumper
{
    public partial class AgentPage : Page
    {
        private int start = 0;
        private int fullCount = 0;
        private int SeePages = 3;
        private List<int> DirPage = new List<int>();
        List<Agent> Agents = new List<Agent>();
        public AgentPage()
        {
            InitializeComponent();
            Load();
        }

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "По возрастанию",
            "По убыванию",
            "По возрастанию приоритета",
            "По убыванию приоритета"
        };
        public string[] FilterList { get; set; } =
        {
            "---",
            "ЗАО",
            "МКК",
            "МФО",
            "ОАО",
            "ООО",
            "ПАО"
        };

        public void Load()
        {
            Agents = Entities.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            LViewAgent.ItemsSource = Agents;
            DataContext = this;
            fullCount = Entities.GetContext().Agent.Count();
            textAllAmount.Text = fullCount.ToString();
            textResultAmount.Text = Agents.Count.ToString();
            PagesCount.Text = "";
            DirPage.Clear();
            for (int i = 1; i <= fullCount / 10; i++)
            {
                DirPage.Add(i);
            }
            
            for (int i = 0; i < 3; i++)
            {
                PagesCount.Text = PagesCount.Text + " " + DirPage[i] + " ";
            }
            PagesCount.Visibility = Visibility.Visible;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (start < 10) 
            {
                start++;
                Agents = Entities.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                LViewAgent.ItemsSource = Agents;
                textResultAmount.Text = $"{Convert.ToInt32(textResultAmount.Text) + Agents.Count}";
                SeePages++;
                CheckPages(SeePages);
            }
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (start > 0) 
            {
                textResultAmount.Text = $"{Convert.ToInt32(textResultAmount.Text) - Agents.Count}";
                start--;
                Agents = Entities.GetContext().Agent.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
                LViewAgent.ItemsSource = Agents;
                SeePages--;
                CheckPages(SeePages);
            }
        }

        private void CheckPages(int pages) 
        {
            if (pages < 3) { return;}
            if (pages > DirPage.Count()) { return; }

            PagesCount.Text = "";
            for (int i = 3; i > 0; i--)
            {
                PagesCount.Text += $" {DirPage[pages - i]} ";
            }
        }

        private void turnButton()
        {
            
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            
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

        private void textSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Upload();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Upload();
            }
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Upload();
            }
        }

        public void Upload() 
        {
            var result = Entities.GetContext().Agent.ToList(); //Принимаем данные из таблицы Agents в переменную

            if (cmbSorting.SelectedIndex == 0) //Обрабатываем фильтры возрастания
            {
                result = result.OrderBy(Agent => Agent.ID).Skip(start * 10).Take(10).ToList();
            }
            else if (cmbSorting.SelectedIndex == 1)
            {
                result = result.OrderBy(agent => agent.Title).Skip(start * 10).Take(10).ToList();
            }
            else if (cmbSorting.SelectedIndex == 2)
            {
                result = result.OrderByDescending(agent => agent.Title).Skip(start * 10).Take(10).ToList();
            }
            else if (cmbSorting.SelectedIndex == 3)
            {
                result = result.OrderBy(agent => agent.Priority).Skip(start * 10).Take(10).ToList();
            }
            else if (cmbSorting.SelectedIndex == 4)
            {
                result = result.OrderByDescending(agent => agent.Priority).Skip(start * 10).Take(10).ToList();
            }


            if (cmbFilter.SelectedIndex == 0) //Обработка по организациям
            {}
            else if (cmbFilter.SelectedIndex == 1)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "ЗАО").ToList();
            }
            else if (cmbFilter.SelectedIndex == 2)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "МКК").ToList();
            }
            else if (cmbFilter.SelectedIndex == 3)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "МФО").ToList();
            }
            else if (cmbFilter.SelectedIndex == 4)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "ОАО").ToList();
            }
            else if (cmbFilter.SelectedIndex == 5)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "ООО").ToList();
            }
            else if (cmbFilter.SelectedIndex == 6)
            {
                result = Entities.GetContext().Agent.Where(agent => agent.AgentType.Title == "ПАО").ToList();
            }

            result = result.Where(x => x.Title.ToLower().Contains(textSearch.Text.ToLower())).ToList();//Реализация поиска
            LViewAgent.ItemsSource = result; //Передаём результат в ListView

            textResultAmount.Text = result.Count.ToString();//Передём количество записей после применения поиска, сортировки, фильтрации
        }

        private void btnUpdateOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
