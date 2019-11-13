using System;

namespace WInnovator.ViewModels
{
    public class AppTokenModel
    {
        public Guid ShopId { get; set; }
        public string ShopDescription { get; set; }
        public string Token { get; set; }

        public AppTokenModel(Guid shopId, string shopDescription, string token)
        {
            ShopId = shopId;
            ShopDescription = shopDescription;
            Token = token;
        }
    }
}
