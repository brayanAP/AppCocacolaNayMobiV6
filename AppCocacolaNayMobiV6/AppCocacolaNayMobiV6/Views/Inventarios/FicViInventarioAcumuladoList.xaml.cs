using AppCocacolaNayMobiV6.ViewModels.Inventarios;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCocacolaNayMobiV6.Views.Inventarios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FicViInventarioAcumuladoList : ContentPage
	{
        private object FicNavigationContext { get; set; }
        private bool modo = false;
        

        public FicViInventarioAcumuladoList(object FicNavigationContext)
        {
            InitializeComponent();
            this.FicNavigationContext = FicNavigationContext;
            BindingContext = App.FicVmLocator.FicVmInventarioAcumuladoList;
            
        }//CONSTRUCTOR

        protected async override void OnAppearing()
        {
            var FicViewModel = BindingContext as FicVmInventarioAcumuladoList;
            if (FicViewModel != null)
            {
                FicPickerSKU.ItemsSource = new ObservableCollection<string>() { "Ver todos los SKU", "Ver SKU con conteo", "Ver SKU sin conteo" };
               
                FicViewModel.FicNavigationContext = FicNavigationContext;
                FicViewModel.OnAppearing();
            }

            FicPickerSKU.SelectionChanged += (object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e) =>
            {
                FicViewModel.FicMetFiltroSKU();
            };

            
            FicGridAcuList.QueryCellStyle += DataGrid_QueryCellStyle;
        }//SE EJECUTA CUANDO SE ABRE LA VIEW
        
        private void Button_Clicked(object sender, EventArgs e)

        {
            modo = !modo;
            FicPickerSKU.IsVisible = modo;

        }

        private void DataGrid_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4 && e.CellValue == null)
                {
                    e.Style.BackgroundColor = Color.IndianRed;
                    e.Style.ForegroundColor = Color.White;
                }
                else if (e.ColumnIndex == 4 && int.Parse(e.CellValue.ToString()) >= 0)
                {
                    e.Style.BackgroundColor = Color.YellowGreen;
                    e.Style.ForegroundColor = Color.White;
                }
                
                e.Handled = true;
            }
            catch
            {
                if (e.ColumnIndex == 4)
                {
                    e.Style.BackgroundColor = Color.IndianRed;
                    e.Style.ForegroundColor = Color.White;
                    e.Handled = true;
                }
            }
        }
        //private void DataGrid_GridDoubleTapped(object sender, GridDoubleTappedEventsArgs e)
        //{
        //    var rowIndex = e.RowColumnIndex.RowIndex;
        //    var rowData = e.RowData;
        //    var columnIndex = e.RowColumnIndex.ColumnIndex;
        //}

    }//CLASS
}//NAMESPACE