using SuperShop.Data.Entities;
using SuperShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model,Guid imageId, bool isNew);

        ProductViewModel ToProductViewModel(Product product);
    }
}
