using Omnos.Desktop.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.ApiClient.Models.Stock;

namespace Omnos.Desktop.App.ViewModels
{
    public class StockViewModel : ObservableObject
    {
        private readonly StockService _service;
        
        public ObservableCollection<StockModel> Items { get; } = new();
        private StockModel? _selected;
        public StockModel? Selected { get => _selected; set => SetProperty(ref _selected, value); }
        
        private string? _query;
        public string? Query { get => _query; set => SetProperty(ref _query, value); }
        
        public ICommand RefreshCmd { get; }
        public ICommand NewCmd { get; }
        public ICommand SaveCmd { get; }
        public ICommand DeleteCmd { get; }
        public ICommand ClearFilterCmd { get; }
        
        public StockViewModel(StockService service)
        {
            _service = service;
            
            RefreshCmd = new RelayCommand(async _ => await LoadAsync(), _ => true);
            ClearFilterCmd = new RelayCommand(async _ => { Query = null; await LoadAsync(); }, _ => true);
            NewCmd = new RelayCommand(_ => CreateNew(), _ => true);
            SaveCmd = new RelayCommand(async _ => await SaveAsync(), _ => Selected is not null);
            DeleteCmd = new RelayCommand(async _ => await DeleteAsync(), _ => Selected is not null);
            
            _ = LoadAsync();
        }
        
        private async Task LoadAsync()
        {
            var list = await _service.GetAllAsync(Query);
            Items.Clear();
            foreach (var it in list) Items.Add(it);
        }
        
        private void CreateNew()
        {
            Selected = new StockModel
            {
                // predefina campos básicos se quiser
                // Quantity = 0, ProductId = ...
            };
        }
        
        private async Task SaveAsync()
        {
            if (Selected is null) return;
            
            // Supondo que há uma propriedade Id (Guid) no seu StockModel
            if (Selected.Id == Guid.Empty)
            {
                var created = await _service.CreateAsync(Selected);
                if (created is not null) Items.Add(created);
            }
            else
            {
                await _service.UpdateAsync(Selected.Id, Selected);
                // Atualize a linha já selecionada (Items já referencia Selected)
            }
        }
        
        private async Task DeleteAsync()
        {
            if (Selected is null || Selected.Id == Guid.Empty) return;
            
            await _service.DeleteAsync(Selected.Id);
            Items.Remove(Selected);
            Selected = null;
        }
    }
}
