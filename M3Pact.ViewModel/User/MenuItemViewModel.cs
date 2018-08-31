using System.Collections.Generic;

namespace M3Pact.ViewModel.User
{
    public class MenuItemViewModel
    {
        public int MenuItemViewModelId { get; set; }
        public string NodeName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? NodeId { get; set; }
        public int? ParentId { get; set; } 
        public List<MenuItemViewModel> SubNodes { get; set; }

        public string Info { get; set; }

    }
}
