using Jumper.Classes;
using Jumper.Entity;
using Jumper.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public Agent SelectedAgent = new Agent();
        private List<int> DirPage = new List<int>();
        List<Agent> Agents = new List<Agent>();
        public AgentPage()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            start = 0;
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
            if (start < (fullCount / 10)) 
            {
                start++;
                //MessageBox.Show($"{start}");
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
            LViewAgent.ItemsSource = result; //Передача результат в ListView

            textResultAmount.Text = result.Count.ToString();//Передача количество записей после применения поиска, сортировки, фильтрации
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditPage(null));
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            SelectedAgent = LViewAgent.SelectedItem as Agent;
            try
            {
                NavigationService.Navigate(new EditPage(SelectedAgent));
            }
            catch 
            {
                MessageBox.Show("Выберите агента!");
            };
            
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

        private void EditPriority_Click(object sender, RoutedEventArgs e)
        {
            if (LViewAgent.SelectedItems.Count > 0)
            {
                int prt = 0;
                foreach (Agent agent in LViewAgent.SelectedItems)
                {
                    if (agent.Priority > prt) prt = agent.Priority;
                }
                ModWindow dlg = new ModWindow(prt);
                Entities.prioritet = prt;
                Entities.flag = false;
                dlg.ShowDialog();
                if (Entities.flag)
                {
                    foreach (Agent agent in LViewAgent.SelectedItems)
                    {
                        agent.Priority = Entities.prioritet;
                        Entities.GetContext().Entry(agent).State = EntityState.Modified;
                    }
                    Entities.GetContext().SaveChanges();
                    Load();
                }
            }
        }

        private void DeleteAgent_Click(object sender, RoutedEventArgs e)
        {
            if (LViewAgent.SelectedItem != null)
            {
                Agent selectedAgent = (Agent)LViewAgent.SelectedItem; // Получение контекста базы данных.
                var context = Entities.GetContext(); // Нахождение всех записе в таблице ProductSale, связанные с удаляемым агентом.
                var relatedSales = context.ProductSale.Where(sale => sale.AgentID == selectedAgent.ID).ToList(); // Удаление найденной записи из таблицы ProductSale.
                context.ProductSale.RemoveRange(relatedSales); // Затем удаление записи из таблицы Agent.
                context.Agent.Remove(selectedAgent); // Сохрание изменения в базе данных.
                context.SaveChanges();
                MessageBox.Show("Удаление прошло успешно");
                Load();
            }
        }
    }
}
