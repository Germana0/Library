using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Library
{
    class SearchResult
    {
        public string Author { get; set; }
        public string Book { get; set; }
        public SearchResult(string a, string b)
        {
            Author = a;
            Book = b;
        }
    }
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _searchBox;
        public string SearchBox
        {
            get { return _searchBox; }
            set
            {
                _searchBox = value;
                OnPropertyChanged("SearchBox");
            }
        }
        private string _shareBox;
        public string ShareBox
        {
            get { return _shareBox; }
            set
            {
                _shareBox = value;
                OnPropertyChanged("ShareBox");
            }
        }
        private List<SearchResult> _searchResults;
        public List<SearchResult> SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }
        private List<string> _authors;
        public List<string> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged("Authors");
            }
        }
        private List<string> _books;
        public List<string> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged("Books");
            }
        }
        public Command Search { get; set; }
        public Command AuthorSelect { get; set; }
        public Command BookSelect { get; set; }
        public Command SearchSelect { get; set; }
        public Command Share { get; set; }
        private string SelectedAuthor;
        public MainWindowViewModel()
        {
            Search = new Command();
            Search.ExecFunc = SearchFunction;
            AuthorSelect = new Command();
            AuthorSelect.ExecFunc = AuthorSelectFunction;
            BookSelect = new Command();
            BookSelect.ExecFunc = BookSelectFunction;
            SearchSelect = new Command();
            SearchSelect.ExecFunc = SearchSelectFunction;
            Authors = new List<string>(MakeRequest("select author from booklets group by author"));
            Share = new Command();
            Share.ExecFunc = ShareFunction;
        }
        private void ShareFunction(object parameter)
        {
            if (parameter == null || !(parameter is TextBox)) return;
            Window s = new ShareWindow(((TextBox)parameter).Text);
            s.Show();
        }
        private void SearchFunction(object parameter)
        {
            if (parameter == null || !(parameter is TextBox)) return;
            TextBox box = (TextBox)parameter;
            if (Validation.GetHasError(box))
            {
                MessageBox.Show(Validation.GetErrors(box)[0].ErrorContent.ToString());
                return;
            }
            string request = box.Text;
            List<SearchResult> Results = new List<SearchResult>();
            using (SqlConnection db = new SqlConnection(ConnectionString()))
            {
                SqlCommand query = new SqlCommand($"select author, book from booklets where author like \'%{request}%\' or book like \'%{request}%\'", db);
                SqlDataReader reader;
                try
                {
                    db.Open();
                    reader = query.ExecuteReader();
                    SqlDataAdapter adapter = new SqlDataAdapter(query);
                    DataTable table = new DataTable();
                    db.Close();
                    adapter.Fill(table);
                    foreach (DataRow row in table.Rows)
                        Results.Add(new SearchResult((string)row.ItemArray[0], (string)row.ItemArray[1]));
                }
                catch
                {
                    MessageBox.Show("Local database is inaccessible");
                }
            }
            SearchResults = new List<SearchResult>(Results);
        }
        private void AuthorSelectFunction(object parameter)
        {
            if (parameter == null || !(parameter is string)) return;
            Books = new List<string>(MakeRequest($"select book from booklets where author=\'{parameter}\'"));
            SelectedAuthor = (string)parameter;
        }
        private void BookSelectFunction(object parameter)
        {
            if (parameter == null || !(parameter is string)) return;
            ShareBox = $"Now I'm reading {(string)parameter} of {SelectedAuthor}.";
        }
        private void SearchSelectFunction(object parameter)
        {
            if (parameter == null || !(parameter is SearchResult)) return;
            SearchResult selection = (SearchResult)parameter;
            ShareBox = $"Now I'm reading {selection.Book} of {selection.Author}.";
        }
        private List<string> MakeRequest(string request)
        {
            List<string> Results = new List<string>();
            using (SqlConnection db = new SqlConnection(ConnectionString()))
            {
                SqlCommand query = new SqlCommand(request, db);
                SqlDataReader reader;
                try
                {
                    db.Open();
                    reader = query.ExecuteReader();
                    SqlDataAdapter adapter = new SqlDataAdapter(query);
                    DataTable table = new DataTable();
                    db.Close();
                    adapter.Fill(table);
                    foreach (DataRow row in table.Rows)
                        Results.Add((string)row.ItemArray[0]);
                }
                catch
                {
                    MessageBox.Show("Local database is inaccessible");
                }
            }
            return Results;
        }
        private string ConnectionString()
        {
            SqlConnectionStringBuilder connectionstring = new SqlConnectionStringBuilder();
            connectionstring.IntegratedSecurity = true;
            connectionstring.DataSource = "LAPTOP-VJ4LJ07E";
            connectionstring.InitialCatalog = "forbooks";
            return connectionstring.ConnectionString;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
