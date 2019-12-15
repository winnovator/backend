using System;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class DesignShopViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
